# Pipe expressions

The `pipe:` expression type is an optional add-on to the TALES standard. Pipe expressions allow the transformation of values *via a function delegate*.

Pipe expressions are best used when a common transformation is required upon model values.
One scenario which would be served well by pipe expressions is the formatting of dates/times.

Generally-speaking, pipe expressions are preferable to [C# expressions] for the execution of arbitrary logic.
They are more performant and are easier to write.

[C# expressions]: CSharpExpressions.md

## Syntax

The syntax of a pipe expression is as follows:

```text
pipe:variable delegate_expression
```

In this syntax, `variable` must be a single identifier for a defined variable.
It is the input value which will be entered into the pipe delegate.
The `delegate_expression` is any TALES expression that evaluates to a delegate.

## Delegates

Valid delegates are those which match `Func<TInput, TOutput>`, where `TInput` and `TOutput` *may be any types* (except `void`).
An attempt will be made to convert the input value to the appropriate type accepted by the delegate.
An exception will be raised if the input value is of an incompatible type.

## Output

The evaluated result of a pipe expression is the output of the delegate, when executed with the specified input.

## Example

With a model that looks like the following:

```csharp
{
  MyDate = DateTime.Today,
  FormatDate = (DateTime d) => d.ToString("dd MM yyyy")
}
```

The following source in a template document would render the current date as a space-separated day/month/year pattern.

```html
<p tal:define="now here/MyDate">
  The date is
  <span tal:replace="pipe:now here/FormatDate">01 01 2000</span>.
</p>
```