# Limitations of `csharp` expressions
In short, whilst `csharp` expressions are available, to execute arbitrary .NET code *it is almost always better to [use `pipe` expressions instead]*.

The CSharp expression type can be somewhat difficult to use, due to a number of limitations, documented here.

[use `pipe` expressions instead]: PipeExpressions.md

## Symbols must be entity-encoded
C# makes significant use of a number of symbols which are not HTML/XML-friendly. These symbols must be replaced with their entity representations in attribute values or else they will break the validity of the document. This includes (but might not be limited to):

* Double quotes: `&quot;`
* Less-than: `&lt;`
* Greater-than: `&gt;`

## Using extension methods
In order to use any extension methods, including those associated with the `System.Linq` namespace, two further expressions are required:

* A `using` expression variable must be 'in scope', for the appropriate namespace which activates the extension method.
* Unless the extension method operates upon `object`, a `type` expression variable must also be 'in scope', indicating the types of any objects which are to be the target of extension methods.

### Example
This example shows how to use `.FirstOrDefault()` with a list of string. Presume that the variable `myList` contains the list.

```html
<div tal:define="listType type:myList List&lt;string &gt;;
                 usingSystemLinq using:System.Linq;
                 firstItem csharp:myList.FirstOrDefault()">
    <p>
        The first item is
        <span tal:replace="firstItem">complicated</span>
    </p>
</div>
```

## Cannot access anonymous types
* CSharp expressions are evaluated from an in-memory assembly which is compiled at runtime.
* When anonymous types are exported, all of their members are `internal`.

These two phenomenon combined mean that a CSharp expression cannot 'see' any members declared upon anonymous objects. If you try to use them then you will receive compiler errors stating that the members do not exist on type `object`.

## Confusing overload selection
The default behaviour of C# expressions treats every in-scope variable as a `dynamic` object. This avoids the need to provide design-time type information for every single variable.

However, this makes a number of C# features behave in unusual ways. One of these is [the way that method overloads are selected].

[the way that method overloads are selected]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/using-type-dynamic#overload-resolution-with-arguments-of-type-dynamic

## Compilation performance
CSharp expressions do not perform particularly well; they take a moment to compile before first use. This can cause a noticeable delay on first load of a page that uses CSharp expressions, particularly if the page uses several of them.

This is mitigated by the use of an *expression cache*. Once an expression has been compiled, its compiled delegate is cached for the lifetime of the application. This means that subsequent executions are significantly faster.