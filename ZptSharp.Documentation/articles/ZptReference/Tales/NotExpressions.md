# The Not expression syntax

TALES `not` expressions convert a value to boolean and then perform a logical `NOT` on that value.
In other words the `not` expression is true if the expression it acts upon is false, and vice-versa.

The syntax of a `not` expression is another TALES expression; [typically a `path` expression] but it doesn't need to be:

```text
not:other_expression
```

[typically a `path` expression]: PathExpressions.md

## Not expressions are included in the main ZptSharp package

Support for `not` expressions is included in [the main ZptSharp NuGet package].
Additionally, `not` expressions are one of the standard expression types [activated by `AddStandardZptExpressions()`].

[the main ZptSharp NuGet package]: ../../NuGetPackages.md#zptsharp-core
[activated by `AddStandardZptExpressions()`]: xref:ZptSharp.ZptSharpHostingBuilderExtensions.AddStandardZptExpressions(ZptSharp.Hosting.IBuildsHostingEnvironment)

## Not expressions have their own rules for boolean conversion

The TALES `not` expression _does not use the same rules_ as [TAL does for boolean coercion].
Not expressions use [**their own logic** for converting a value to boolean].
Only a very specific set of values are treated as false and for some value-types (based upon `struct`) it is impossible to provide a value which corresponds to false (`System.DateTime` comes to mind).

[TAL does for boolean coercion]: xref:ZptSharp.Tal.IInterpretsExpressionResult.CoerceResultToBoolean(System.Object)
[**their own logic** for converting a value to boolean]: xref:ZptSharp.Expressions.NotExpressions.BooleanValueConverter.CoerceToBoolean(System.Object)

## Examples

This expression will evaluate to false if the path expression `here/anObject` resolves to an object created with `new object()`.
This is because the path expression is treated as true by the rules of the `not` expression, and so the overall expression is false.

```text
not:here/anObject
```

This expression will evaluate to true.
This is because the string expression evaluates to an empty string.
Because `System.String` implements `IEnumerable`, and an empty string is an enumerable of zero-length, the `not` expression treats it as false.
This means that the overall expression is true.

```text
not:string:
```
