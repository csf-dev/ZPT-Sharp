# The `tal:attributes` attribute

The `tal:attributes` attribute is used to add, update and/or remove other markup attributes from the same element.

## Attribute syntax

The syntax of the `tal:attributes` attribute comprises of:

* The attribute name, followed by one or more space characters
* A TALES expression which indicates the new value for that attribute

If the expression the attribute value needs to make use of the semicolon `;` character, then this must be escaped by doubling-it-up: `;;`.
This could occur, for example, if a semicolon is used in [a `string` expression].
The reason for this is as described below - the semicolon character is used to separate multiple attribute assignments.

### Setting more than one attribute in the same `tal:attributes` attribute

A single `tal:attributes` attribute value may set more than one attribute value.
Where more than one attribute is to be set, each assignment must be separated with a semicolon `;` character.
There _may_ also be any amount of whitespace before or after these semicolons, as appropriate for readability.

## Values are encoded before being written

Because data is being written into markup attributes, any special/reserved characters in the expression result are encoded according to the markup scheme.
This means that - for example - a less-then symbol `<` will become `&lt;`.

## If the expression result is null, the attribute is removed

If the expression for an attribute evaluates to null then that attribute is removed from the element if it is currently present (in the template source).
If the expression result is null and the attribute is not already present then it is not added.

## If the expression result is not null, the attribute is added or updated

If the expression for an attribute evaluates to a non-null value (but does not abort the action, see below) then the attribute value is updated to the expression result.
If the attribute was not previously present in (in the template source) then it is added.
If the expression result is not a string then it will be converted to string via `Object.ToString()`.

## Aborting a `tal:attributes` attribute

Each attribute-value-assignment within a `tal:attributes` attribute is handled independently.
If multiple attributes are assigned from a single `tal:attributes` attribute then some assignments might be aborted and others might not.

If the expression for an attribute assignment evluates to [an instance of `AbortZptActionToken`], such as via [the root context `default`], then that particular attribute is left as it stands in the template source code, with no modification.

[an instance of `AbortZptActionToken`]: xref:ZptSharp.Expressions.AbortZptActionToken
[the root context `default`]: ../Tales/GlobalContexts.md#default

## Examples

### Adding an attribute

This example shows how to add an attribute to an element where that attribute does not already exist.
The expression `here/className` will be evaluated, converted to string if applicable and a `class` attribute will be added to the `<p>` element with that value.
The will only not occur if the expression evluates to either null or [an instance of `AbortZptActionToken`].

```html
<p tal:attributes="class here/className">This paragraph will have a 'class' attribute added.</p>
```

### Updating multiple attributes

In this example, the `src` attribute value will be updated to the result of the expression `product/imageUrl` and the `alt` attribute value will be updated to the result of the expression `product/imageText`.
The whitespace placing each attribute assignment onto a line of its own is not required, _it is included only for readability_.

```html
<img src="images/placeholder.png"
     alt="Product image"
     tal:attributes="src product/imageUrl;
                     alt product/imageText">
```