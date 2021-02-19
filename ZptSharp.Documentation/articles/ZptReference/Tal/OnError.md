# The `tal:on-error` attribute

The `tal:on-error` attribute is how TAL rendering errors are dealt-with in ZPT document templates.
Developers could think of the on-error attribute as behaving a little like a `try/catch`, available in many programming languages.

The value of a `tal:on-error` attribute is a single TALES expression.

## The on-error attribute has no effect without an error

The presence of a `tal:on-error` attribute does not affect the document or rendering process at all unless an error occurs.


## The scope of an on-error attribute matches the DOM

A `tal:on-error` attribute deals with errors which are caused by _the DOM element on which the attribute is declared, or upon any descendent element_.
Looking at this behaviour from the opposite direction, if/when a TAL rendering error occurs, that error is handled by an on-error attribute from _the closest ancestor_ element, including the element upon which the error occurs.

## When handling an error, `tal:on-error` behaves like `tal:content`

When an error occurs whilst rendering, and the error is handled by a `tal:on-error` attribute, that on-error attribute behaves in the same way as if it were [a `tal:content` attribute].
That is - the contents & descendents of the `tal:on-error` attribute are replaced with the value of the expression.
Be aware that _this can result in the removal & replacement of elements which have been successfully processed without error_, if the on-error attribute is removed from the element which raised the error (following the structure of the DOM).

Inside of the expression for a `tal:on-error` attribute, you may use [the `error` root context] to get a reference to the error/exception object which is being handled.

[the `error` root context]: ../Tales/GlobalContexts.md#error
[a `tal:content` attribute]: ContentAndReplace.md

## Error scenarios

One of the most common error scenarios is an attempt to use a model value which does not exist, such as via [a TALES path expression] which cannot be traversed.
This is not always a coding fault either, because the presence/absence of a value could depend upon the current state of the application.
The `tal:on-error` attribute allows designers to provide _fallback content_ rather than having the whole page-rendering process fail.

[a TALES path expression]: ../Tales/PathExpressions.md

## Unhandled errors

If a TAL rendering error occurs and there is no `tal:on-error` attribute on either the same element or any ancestor of the element which caused the error, then the error will be unhandled.
In this case, the entire rendering operation will fail.
In [an MVC view engine], this could result in the user seeing an error page, and [in the ZptSharp API] this will result in the raising of an exception from the rendering process.

[an MVC view engine]: ../../ViewEngines.md
[in the ZptSharp API]: ../../../api/index.md

## Example

This example shows a usage of a `tal:on-error` attribute.
For this example, presume that the path expression `here/nonexistent` cannot be traversed; the model contains no value named `nonexistent`.
Instead of causing a rendering error that halts the whole process with an error, this example will display an error message instead.

```html
<p tal:on-error="string:There was a problem determining your login status.">
    You are logged in as
    <span class="username" tal:content="here/nonexistent">joebloggs</span>.
</p>
```

We expect an error to be raised when processing the `tal:content` attribute on the `<span>` element.
The final output result will look like this:

```html
<p>There was a problem determining your login status.</p>
```