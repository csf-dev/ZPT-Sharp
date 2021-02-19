# The `tal:omit-tag` attribute

The `tal:omit-tag` attribute is used to remove the current element's start & end tags from the document without affecting its content or descendents.
Omitting a tag could be thought of "replacing the element with that element's contents".

The attribute value for a `tal:omit-tag` element is a TALES expression which determines whether or not to perform the omission.
If the expression is either empty or [the expression result is truthy] then the start & end tags for the current element are omitted.
If the expression is not empty and is not truthy then the omission does not occur.

[the expression result is truthy]: xref:ZptSharp.Tal.IInterpretsExpressionResult.CoerceResultToBoolean(System.Object)

## Aborting tag omission

If the result of a variable definition expression in a `tal:omit-tag` attribute is [an instance of `AbortZptActionToken`], such as via [the root context `default`], then the behaviour is the same as if the expression had evaluated to a 'falsey' value.
In this case the tags are not omitted.

[an instance of `AbortZptActionToken`]: xref:ZptSharp.Expressions.AbortZptActionToken
[the root context `default`]: ../Tales/GlobalContexts.md#default

## Example

It is very common to use an empty attribute value for `tal:omit-tag`.
As stated above, this means 'always omit'.

The following snippet demonstrates tag omission; it also includes [a `tal:condition` attribute] to show how this technique might be used in a real application.
In this case we want to use a condition attribute to control a second sentence of text, but we do not wish to wrap that sentence in an otherwise-extraneous `<span>` element.
This example assumes that there is a boolean model value available at the path `here/eligibleForRewardScheme`, indicating whether the purchase is eligible to collect 'reward scheme points'.

```html
<p>
    Thankyou for your purchase.
    <span tal:condition="here/eligibleForRewardScheme"
          tal:omit-tag="">Reward scheme points have been added to your balance.</span>
</p>
```

When this snippet is rendered, the output will look like the following if `here/eligibleForRewardScheme` is true.
The `<span>` element will have been omitted from the output.
If `here/eligibleForRewardScheme` is false then the second sentence would not render at all (per the `tal:condition` attribute).

```html
<p>
    Thankyou for your purchase.
    Reward scheme points have been added to your balance.
</p>
```

[a `tal:condition` attribute]: Condition.md
