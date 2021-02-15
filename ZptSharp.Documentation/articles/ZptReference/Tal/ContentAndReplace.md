# The `tal:content` & `tal:replace` attributes

The `tal:content` and `tal:replace` attributes have very similar effects upon the rendering output.
The two attributes _are also mutually exclusive_, no element may have both attributes.

Both attributes use a TALES expression as their attribute value, this expression is evaluated and the result is written to the rendered document.
In the case of the `tal:content` attribute, the expression result is written as the new content of the element (leaving the element start & end tags intact).
In the case of the `tal:replace` attribute, the expression result _fully replaces the element_ (the start & end tags are removed).

## The expression result is treated as text by default

When a content or replace attribute evaluats a TALES expression, if the result is not a `string`, then it is converted to string via the built-in `Object.ToString()` method.
Additionally, by default, the expression result is treated strictly as text and not as markup.
This means that any symbols appearing in the expression result that are reserved symbols in the markup (for example the less-than symbol `<`) are replaced by their entity-encoded equivalent (for example `&lt;`).
This helps prevent [XSS attacks] against the application.

There are two ways to explicitly make a `tal:content` or `tal:replace` attribute write the expression result as markup (including elements & attributes etc).

* Include the word `structure` and a space in the attribute value before the expression
* If the expression result implements [the interface `IGetsStructuredMarkup`], such as if it has been through [a `structure:` expression].

In both cases above, the expression result will instead be written to the rendered document verbatim; reserved symbols will not be encoded.

Designers should be _very careful when permitting structure in a content or replace attribute_.
Ensure that all data is appropriately sanitized, particularly if it is user-generated or has come from an untrusted source.

[XSS attacks]: https://en.wikipedia.org/wiki/Cross-site_scripting
[the interface `IGetsStructuredMarkup`]: xref:ZptSharp.IGetsStructuredMarkup
[a `structure:` expression]: ../Tales/StructureExpressions.md

## Any existing content of the element is removed

When a content or replace attribute is processed, any and all existing content & descendents of the element which has the attribute is removed from the DOM.
This technique may be used to provide "sample content" in the document template.
This can help a designer understand the sorts of content which TAL will render into that element, without needing to see the template rendered in the full application.

## If the expression result is null

If the expression result for a `tal:content` attribute is `null`, then all content & descendents of the element are removed and the element is left empty.
If the expression result for a `tal:replace` attribute is `null`, then the entire element and all content & descendents are removed.

## Some TAL attributes are copied

For a `tal:replace` attribute only, if the element which has the attribute also has either of the following attributes, then these attributes are copied to the top-level nodes of the replacement, provided that any of these nodes are element nodes.

* [`tal:attributes` attributes]
* [`tal:omit-tag` attributes]

This occurs because these two attributes would usually be processed _after_ a replace attribute.  Thus they are evaluated as if they were present upon the replacement element(s).

[`tal:attributes` attributes]: Attributes.md
[`tal:omit-tag` attributes]: OmitTag.md

## Aborting a `tal:content` attribute

If the expression result is [an instance of `AbortZptActionToken`], such as via [the root context `default`], then a `tal:content` attribute will behave as if it were not present.
The element will be left in-place along with all of its original content & descendents from the template.

## Aborting a `tal:replace` attribute

If the expression result is [an instance of `AbortZptActionToken`], such as via [the root context `default`], then a `tal:replace` attribute is a little more complex than that of a content attribute.

1. The start & end tags of the element which bears the `tal:replace` attribute are omitted from the DOM, in a similar way to [a `tal:omit-tag` attribute].
2. The contents & descendents of the (now-omitted) element which had the `tal:replace` attribute are left intact and no further replacement is made

This means that if the replacement is aborted, the tag which has the replace attribute is essentially "replaced with its original content".
Note that even when a replace attribute is aborted, _the TAL attribute copying described above still occurs_.

[an instance of `AbortZptActionToken`]: xref:ZptSharp.Expressions.AbortZptActionToken
[the root context `default`]: ../Tales/GlobalContexts.md#default
[a `tal:omit-tag` attribute]: OmitTag.md

## Examples

### An element with a content attribute

This snippet would render as a `<p>` element with content equal to the value of the expression `here/message`.
Any reserved symbols present in the expression result will be encoded, for example `&` would become `&amp;`

```html
<p tal:content="here/message">This will be replaced by a message</p>
```

### An element with a replace attribute

This snippet would render as a `<p>` element with the text "This will show", immediately followed by the value of the expression `here/message`.
The `<span>` element present in the source template will not render at all, because the whole element will be replaced by the expression result.
Any reserved symbols present in the expression result will be encoded, for example `&` would become `&amp;`

```html
<p>This will show <span tal:replace="here/message">a message</span></p>
```

### Content that uses the `structure` keyword

This content prefixes the TALES expression with the `structure` keyword and a space.
This will behave as the `tal:content` example above, except that reserved symbols _will not be encoded_.
This means that if the expression result includes markup, then it will be rendered as markup.

```html
<p tal:content="structure here/message">This will be replaced by a message</p>
```
