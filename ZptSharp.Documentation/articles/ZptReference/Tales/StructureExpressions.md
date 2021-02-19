# The Structure expression syntax

A TALES structure expression may be used to indicate that an expression value should be treated as markup/structure when used with [a TAL content or replace attribute], even if that attribute does not use the `structure` keyword to enable processing of markup.

The syntax of the expression is as follows, it simply prefixes another expression, [typically a `path` expression] but it doesn't need to be:

```text
structure:other_expression
```

[a TAL content or replace attribute]: ../Tal/ContentAndReplace.md
[typically a `path` expression]: PathExpressions.md

## Structure expressions must be explicitly activated

Whilst support for structure expressions is included in [the main ZptSharp NuGet package], _they are not one of the standard expression types_.
Structure expressions must be explicitly activated [using the method `AddZptStructureExpressions()`] before they may be used.

[the main ZptSharp NuGet package]: ../../NuGetPackages.md#zptsharp-core
[using the method `AddZptStructureExpressions()`]: xref:ZptSharp.ZptSharpHostingBuilderExtensions.AddZptStructureExpressions(ZptSharp.Hosting.IBuildsHostingEnvironment)

## The expression result is wrapped

The result of a structure expression is the result of the 'inner' expression, wrapped [within an instance of `StructuredMarkupObjectAdapter`].
This object implements [the interface `IGetsStructuredMarkup`] and so content or replace attributes operating upon this value [will treat it as structure, without needing the `structure` keyword].

[within an instance of `StructuredMarkupObjectAdapter`]: xref:ZptSharp.Expressions.StructuredMarkupObjectAdapter
[the interface `IGetsStructuredMarkup`]: xref:ZptSharp.IGetsStructuredMarkup
[will treat it as structure, without needing the `structure` keyword]: ../Tal/ContentAndReplace.md#inserting-markup-using-content-or-replace-attributes

## Use with caution

The same advice as for using the `structure` keyword with a content or replace attribute goes for using structure expressions.
Designers should be vigilent that they do not allow untrusted content to be written to the output unless it has been thoroughly sanitized first.

The `structure` keyword (for content & replace attributes) is a generally a better solution than the use of structure expressions; it forces designers to be explicit when they want markup to be interpreted as such, and at the point where it is written.

## Example

The result of the path expression `here/iAmMarkup` will be rendered to the output as markup without escaping.

```html
<p tal:define="theMarkup structure:here/iAmMarkup" tal:content="theMarkup">This will be replaced</p>
```