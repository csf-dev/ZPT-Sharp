# The `tal:condition` attribute

The `tal:condition` attribute is used to conditionally render an element and its descendents.
The attribute value is a TALES expression and if [the expression result is 'truthy'] then the element and its content/descendents are rendered.
If the expression result is not 'truthy' then the element, its content & descendents are removed from the DOM and not rendered or further processed by ZPT.

[the expression result is 'truthy']: xref:ZptSharp.Tal.IInterpretsExpressionResult.CoerceResultToBoolean(System.Object)

## Aborting a condition attribute

If the result of an expression in a `tal:condition` attribute value is [an instance of `AbortZptActionToken`], such as via [the root context `default`], then a `tal:condition` attribute will behave as if it were not present.
In other words, the element and content/descendents will be rendered.

[an instance of `AbortZptActionToken`]: xref:ZptSharp.Expressions.AbortZptActionToken
[the root context `default`]: ../Tales/GlobalContexts.md#default

## Further processing is halted if the expression is not 'truthy'

If the condition expression is not 'truthy' and the element & contents/descendents is to be removed then additionally, _further processing of TAL attributes upon that element is also halted_.
Usually this is not an issue, because the element will be removed anyway, so further manipulation is irrelevant.

Be aware that [**global variable** definitions] present upon the same element will also not be processed in this case though; this could feasibly have an impact upon other parts of the document.
It is _probably best not to place global definitions upon elements that have condition attributes_, so as to avoid this potentially-confusing scenario.

[**global variable** definitions]: Define.md#variable-scope

## Example

In this example, the first `<p>` element will be rendered but the second will not.
This is because a non-empty-string is truthy but a null reference is not.

```html
<div tal:define="isTruthy string:I am truthy;isFalsey nothing">
    <p tal:condition="isTruthy">This text will be displayed.</p>
    <p tal:condition="isFalsey">This text <em>will not</em> be displayed.</p>
</div>
```