# Tutorial: Attributes and omitting tags

In the fourth tutorial instalment we shall look at how to manipulate the attributes of a markup element.
We shall also see an advanced scenario for omitting the start/end tags for an element, whilst preserving its content & descendents.

## `tal:attributes` adds, removes & updates attributes

Let's try a change to our model, and some additional markup.
As in the previous tutorial page, the full model/markup is omitted for brevity, only the new content is shown.
Make the changes shown below to your model & template markup and then render them.

### New model values

```csharp
{
    UrlOne = "http://example.com/",
    UrlTwo = "",
    UrlThree = null,
}
```

### New markup

```html
<ul>
    <li>
        <a tal:attributes="href here/UrlOne">This will have an 'href' attribute added</a>
    </li>
    <li>
        <a href="http://nonexistent.xyz/"
           tal:attributes="href here/UrlOne">This will have an 'href' attribute updated</a>
    </li>
    <li>
        <a tal:attributes="href here/UrlTwo">This will have an 'href' attribute set to empty string</a>
    </li>
    <li>
        <a href="http://nonexistent.xyz/"
           tal:attributes="href here/UrlThree">This will have its 'href' attribute removed</a>
    </li>
</ul>
```

### What we learned

* A `tal:attributes` attribute value has two parts: an attribute name (which the `tal:attributes`-attribute will work upon) followed by a space and then an expression
* If the named attribute _does not already exist_ on the element and the expression result is not-null, then the attribute will be **added** with a value equal to expression result
* If the named attribute _already exists_ on the element and the expression result is not-null, then the attribute value will be **updated** to the expression result
* If the named attribute _already exists_ on the element and the expression result is null, then the attribute will be **removed** from the element entirely
  * _As an aside_, if the attribute does not already exist on the element and the expression result is null, then it will not be added

## You can specify multiple attribute/expression pairs

One other aspect of a `tal:attributes` attribute worth noting is that a single attributes-attribute may name as many attribute/expression pairs as it needs.
Each pair of attribute-name and value expression must be separated with a semicolon character.
Each named attribute will be handled separately, so a single `tal:attributes` attribute could plausibly perform a mixture of adding, updating and also removing attributes.

### Example

The following markup will add a `class` attribute with a value of "external\_link" and also update the href attribute value to "htt<span>p://example.c</span>om/".
The whitespace inside the `tal:attributes` attribute value, putting each new attribute on a new line _is only for readability_, it has no effect upon ZptSharp's interpretation of the template.

```html
<a href="http://nonexistent.xyz/"
   tal:attributes="href string:http://example.com/;
                   class string:external_link">This is a sample hyperlink</a>
```

## Omitting element tags

Sometimes, there is a desire to make use of a feature such as `tal:repeat` or `tal:define` at a position where you do not wish to write a markup element.
Because ZPT makes use of markup attributes for its processing instructions, each instruction requires an element.

This problem can be solved by use of the `tal:omit-tag` attribute, which very simply instructs ZPT to not-render the open/close tags in the output document.
Other processing attributes upon the element are still treated normally though.
Addditionally, all of the element's descendents are unaffected.
Let's try this out; here are some model changes and some markup to try in your document.

### Model values

```csharp
{
    SomeNames = new [] { "Jane", "Peter", "Jacques" },
}
```

### Markup

```html
<span tal:repeat="name here/SomeNames" tal:omit-tag="">
    <span tal:replace="name">The name</span>
</span>
```

### How this renders

The output markup for the example above is simply the three names, with no other markup tags surrounding them.
The `tal:omit-tag` attribute ensures that the outermost span is not rendered in the output (although it is still repeated three times) and the use of `tal:replace` ensures that the innermost span is not rendered either.

The `tal:omit-tag` attribute will remove the start & end tags for the element upon which it appears.
It is commonly used without any value, in which case it always omits the start/end tags.
A `tal:omit-tag` attribute may optionally contain an expression as its attribute value; if it does then it will omit the tag if the expression evalues to [a _truthy_ value, or it will not omit the tag if it is a _falsey_ value](xref:ZptSharp.Tal.IInterpretsExpressionResult.CoerceResultToBoolean(System.Object)).

## Next: Introducing METAL macros

So far we have only made use of TAL, which binds model data to the template.
In the next tutorial page [we will take our first look at METAL, which is for declaring and consuming reusable sections of markup](Page5.md).