# The String expression syntax

String expressions are used to create string values dynamically, using a syntax [that is not dissimilar in principle to C# interpolated strings].
It allows the creation of a string value using **placeholders** where dynamic values may be inserted using [a path expression].

The syntax of a string expression is:

```text
string:A string here using zero or more $placeholders
```

[that is not dissimilar in principle to C# interpolated strings]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
[a path expression]: PathExpressions.md

## String expressions are included in the main ZptSharp package

Support for `string` expressions is included in [the main ZptSharp NuGet package].
Additionally, `string` expressions are one of the standard expression types [activated by `AddStandardZptExpressions()`].

[the main ZptSharp NuGet package]: ../../NuGetPackages.md#zptsharp-core
[activated by `AddStandardZptExpressions()`]: xref:ZptSharp.ZptSharpHostingBuilderExtensions.AddStandardZptExpressions(ZptSharp.Hosting.IBuildsHostingEnvironment)

## How to use placeholders

A placeholder always begins with a single dollar character `$`, after which is [a TALES path expression] (and _a path expression only_) which provides the value to be inserted into the placeholder.
If the TALES expression does not evaluate to a string then it will be converted to one via `Object.ToString()` unless it is null, which is interpreted as an empty string.

The expression may follow directly after the dollar character, for example `$myVariable` or it may be wrapped in braces, for example `${myVariable}`.

[a TALES path expression]: PathExpressions.md

### Braces or no braces?

Whether or not braces are used impacts which characters are permitted for use in the expression.
Without braces, expressions that are used in a string placeholder may only use a limited set of characters.
The table below shows which characters are permitted in each scenario.
If you wish to use any of the characters which are disallowed without braces, then your expression must be enclosed in braces.

| Character(s)                      | Without braces    | With braces   |
| ------------                      | :------------:    | :---------:   |
| Alphabetic (upper & lower case)   | Yes               | Yes           |
| Numeric                           | Yes               | Yes           |
| Underscore: `_`                   | Yes               | Yes           |
| Forward-slash: `/`                | Yes               | Yes           |
| Space                             | **No**            | Yes           |
| Period: `.`                       | **No**            | Yes           |
| Comma: `,`                        | **No**            | Yes           |
| Tilde: `~`                        | **No**            | Yes           |
| Pipe (aka "Vertical bar"): `\|`   | **No**            | Yes           |
| Question mark: `?`                | **No**            | Yes           |
| Hyphen-minus: `-`                 | **No**            | Yes           |

### Escaping dollar characters

If you wish your string to include a literal dollar character, it must be escaped by doubling-it up: `$$`.

## You must manually encode markup-reserved characters

Because string expressions are used exclusively in DOM attributes, this means that expressions must also follow the rules for the underlying markup.
For example TALES itself would not raise an error for this expression:

```text
string:He said "$greeting"
```

However, in practice this would not work because in context that expression would appear in an attribute like so.
It will be obvious that those extra double-quote characters break the DOM because they have a special meaning within an attribute.

```html
<p tal:content="string:He said "$greeting"">This will be replaced</p>
```

This problem is fixed by simply encoding the reserved characters according to the rules of the markup.
In this case those double-quote characters must be replaced with `&quot;`, meaning that the final working example would look like this:

```html
<p tal:content="string:He said &quot;$greeting&quot;">This will be replaced</p>
```

## Example

Here is an example of a string expression in use.
This makes use of [the path expression alternation technique] to show where braces might be required in a placeholder.
If the path expression `here/username` cannot be traversed, then TALES will use the alternate `guestUsername` variable instead.
This technique requires the use of a pipe character though, forcing the whole expression to be enclosed in braces.

```html
<p tal:define="guestUsername string:a guest;
               loginMessage string:You are logged in as ${here/username | guestUsername}."
   tal:content="loginMessage">You are logged in as JoeBloggs.</p>
```

[the path expression alternation technique]: PathExpressions.md#alternate-paths