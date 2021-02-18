# The Path expression syntax

Path expressions provide access to objects and object associations in a manner which looks similar to a URL path.
This syntax is considerably simplified compared to full C# and thus lacks the power of a more sophisticated language.
Despite this, because the most common use-cases involve simply "getting a value from the model", it is often sufficient.

At its most basic, a path expression is simply a single variable reference like `myName`, which would evaluate to the value of the `myName` variable.
All path expressions _must begin with either [a variable] or [a root context]_.
Path expressions may also contain forward-slash characters `/` indicating object traversal:

```text
variable/ChildProperty/GrandchildProperty
```

Each forward-slash causes the expression evaluation to traverse into a descendent member.
The C# equivalent of the above might look like `variable.ChildProperty.GrandchildProperty`.

[a variable]: ../Tal/Define.md
[a root context]: GlobalContexts.md

## Path expressions are included in the main ZptSharp package

Support for `path` expressions is included in [the main ZptSharp NuGet package].
Additionally, `path` expressions are one of the standard expression types [activated by `AddStandardZptExpressions()`].

What's more, unless altered [via the rendering configuration], path expressions are the default TALES expression type.
This means that they may be used without requiring the `path:` prefix.

[the main ZptSharp NuGet package]: ../../NuGetPackages.md#zptsharp-core
[activated by `AddStandardZptExpressions()`]: xref:ZptSharp.ZptSharpHostingBuilderExtensions.AddStandardZptExpressions(ZptSharp.Hosting.IBuildsHostingEnvironment)
[via the rendering configuration]: xref:ZptSharp.Config.RenderingConfig.DefaultExpressionType

## Rules for traversal

In most cases in a path expression, the forward slash character is used in the same way as a C# period is used for object/property traversal.
For example the path expression `parent/Child` is likely to be equivalent to the C# `parent.Child`.
This is not always the case though.

This section details the rules for path expression traversal.
Each part of the process is presented as a sub-heading, and _each of these rules is applied in the order in which they are listed_, using [a chain of responsibility].
If a rule fails to provide a value then the next rule is attempted, and so on until an applicable rule is found.

[a chain of responsibility]: https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern

### For the first name in the expression, the contexts and variables are searched

For the very first name in the expression, the following locations are searched (in the following order).
This rule is _only applicable_ for the first name in the expression (the beginning of the expression).

1. If the name is the special keyword `CONTEXTS` then this is used to return [a root contexts object]
2. If the name matches an in-scope local variable then this variable is returned
3. If the name matches a global variable then this variable is returned
4. If the name matches the name of [a root context] then that context is returned
5. _If none of the above match then evaluation fails with an error_

See [the section below titled **Explicitly selecting local or global variables**] for some variants of the `path` expression type which use a modified version of the five steps above.

[the section below titled **Explicitly selecting local or global variables**]: PathExpressions.md#explicitly-selecting-local-or-global-variables
[a root contexts object]: GlobalContexts.md#the-special-root-context-contexts
[a root context]: GlobalContexts.md

### If the current object is a template directory

If the current object being traversed is [an instance of `TemplateDirectory`] then the name following the forward-slash is interpreted as a file or directory name.
If it matches a directory name then the result of traversal is a new `TemplateDirectory` instance for the traversed directory.

If it matches a file name then that file is assumed to be a ZPT document template file.
An [`IDocument`] is returned, reading that file using the same implementation of [`IReadsAndWritesDocument`] as was used to read the current template.

[an instance of `TemplateDirectory`]: xref:ZptSharp.TemplateDirectory
[`IDocument`]: xref:ZptSharp.Dom.IDocument
[`IReadsAndWritesDocument`]: xref:ZptSharp.Dom.IReadsAndWritesDocument

### If the current object is a named value provider

If the current object being traversed is [an object that implements `IGetsNamedTalesValue`] then [its `TryGetValueAsync` method] will be used to get the traversal result.

[an object that implements `IGetsNamedTalesValue`]: xref:ZptSharp.Expressions.IGetsNamedTalesValue
[its `TryGetValueAsync` method]: xref:ZptSharp.Expressions.IGetsNamedTalesValue.TryGetValueAsync(System.String,System.Threading.CancellationToken)

### If the current object is an IDictionary with string keys

If the current object being traversed implements a generic version of `IDictionary<TKey,TValue>` and `TKey` is `string` then the name being traversed is treated as if it were a key of that dictionary.
If the dictionary contains a key of that name then the expression result is the value of the item with the matching key.

### If the current object is an IDictionary with integer keys

As above, but if the current object being traversed implements a generic version of `IDictionary<TKey,TValue>` and `TKey` is `int` then the name being traversed is treated as if it were a key of that dictionary.
If the name can be parsed as an integer and the dictionary contains a key of that nueric value then the expression result is the value of the item with the matching key.

### If the current object is a dynamically-typed object

If the current object being traversed is [a dynamic object], specifically one [which implements `IDynamicMetaObjectProvider`] then an attempt will be made to get a value dynamically using the name which is being traversed.

[a dynamic object]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/using-type-dynamic
[which implements `IDynamicMetaObjectProvider`]: https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.idynamicmetaobjectprovider

### If the current object is IEnumerable and the name is an integer

If the current object being traversed implements `IEnumerable` and the name for traversal may be parsed as an integer then the name is treated as a numeric index and the _Nth_ item from the enumerable is returned as the traversal result.
Optimisations are in place so that if the object also implements `IList`, intermediate item are not enumerated.

### An attempt is made to use reflection to traverse the name

If all of the rules above have failed to produce a result, then reflection is used to find a **public** member which matches the name for traversal.
The order in which members are considered is:

1. Properties which have a getter
2. Methods which have a non-void return type and take no parameters
3. Fields

Please take note of the second item, which might not be obvious.
If a name in a path expression matches a parameterless method which returns anything that is not `void` then the traversal will succeed, where the result is the returned result of having executed that method.
In the case of properties and fields the result is simply getting the value from the member.

### If all of the above fails then traversal fails

If all of the previous rules fail to provide a value then the overall traversal is considered a failure.
If [an alternate path] is not available then _this will raise an error_.
If the attribute containing this expression is a METAL one then that error will cause the overall rendering to be an error.
If the attribute containing this expression is a TAL one then it may optionally [be handled via a `tal:on-error` attribute] if one is present.

[an alternate path]: PathExpressions.md#alternate-paths
[be handled via a `tal:on-error` attribute]: ../Tal/OnError.md

## Explicitly selecting local or global variables

The primary expression prefix for path expressions is `path:`, and because this is the default expression type it may be omitted.
There are three variations of the path expression though which are available for specialized circumstances.
These specialized expression types provide modified rules for [selecting the first name in the expression].

The table below shows how the process described in the section above is modified for these other expression types.
In each case, it indicates which steps (of [the five-step process for selecting the first name in the expression]) are skipped.

| Expression prefix | Steps skipped     |
| ----------------- | -------------     |
| `local:`          | Steps 1, 3 & 4    |
| `global:`         | Steps 1, 2 & 4    |
| `var:`            | Steps 1 & 4       |

This technique may be used to deal with ambiguity where a variable name is defined both locally and globally.
The `global:` expression type would permit accessing the global variable even when there is [a local variable of the same name 'hiding it'].

Apart from the logic change described above for getting the initial value for the path expression, the `local:`, `global:` and `var:` expression types work identically to `path:` expressions.

[selecting the first name in the expression]: PathExpressions.md#for-the-first-name-in-the-expression-the-contexts-and-variables-are-searched
[the five-step process for selecting the first name in the expression]: PathExpressions.md#for-the-first-name-in-the-expression-the-contexts-and-variables-are-searched
[a local variable of the same name 'hiding it']: ../Tal/Define.md#local-variables

## Alternate paths

Path expressions contain a mechanism which allows 'more than one bite at the cherry', this mechanism is called _alternate paths_.
The syntax for alternate path is one or more path expressions, separated by pipe (aka "vertical bar") characters: `|` and optional whitespace.
Here is an example of some alternate path expressions.

```text
here/tryThisFirst | here/tryThisSecond | here/tryThisLast
```

As suggested by the example, each of the individual paths is attempted in the order in which it is listed.
The first which produces a definitive, non-error result is taken as the result of the overall path expression and any further alternates are not used.

This technique is ideal for providing fallback content in order to prevent a rendering error (because a path could not be traversed).
It is common to see constructs like the following where there is doubt that a path expression can be traversed.

```html
<p>
    You logged in as
    <span tal:replace="here/username | default">a guest</span>.
</p>
```

In this example, the `<span>` element will usually be replaced by the value of the path `here/username`.
If that path cannot be traversed (for example, the user is logged in as a guest), then [the `tal:replace` attribute] will receive [a value of default, which aborts it].
Aborting the replace attribute will mean that the text "a guest" will be left in the rendered output.

[the `tal:replace` attribute]: ../Tal/ContentAndReplace.md
[a value of default, which aborts it]: ../Tal/ContentAndReplace.md#aborting-a-talreplace-attribute

## Interpolated paths

An advanced feature of path expressions is _path interpolation_.
This allows a name (within a path expression) to itself be evaluated using a variable name - allowing for a path expression to include a "dynamic name", so to speak.
Here is an example of the basic syntax for a path expression which uses an interpolated part; essentially the interpolated name is designated by a question mark `?` character and then a variable name.

```text
here/?propertyName/Value
```

When evaluating this path expression, the `propertyName` variable will be evaluated as a string (using `Object.ToString()` if neccesary).
The result from the `propertyName` variable will then be placed into the path at the same position.
The whole path is then evaluated using the normal rules.

If, in the example above, the `propertyName` variable has the value "MyProperty" then the overall expression result will be as if the expression were `here/MyProperty/Value`.

When using interpolated paths, the dynamic portion of the path must be derived from a single variable.
It is not permitted to use full expressions to resolve the name of the path segment.
Use [a `tal:define` attribute] to define a variable if needed.

[a `tal:define` attribute]: ../Tal/Define.md
