# The CSharp expression syntax
ZptSharp offers an optional add-on expression evaluator for expressions written in C#. Primarily this is the `csharp` expression type, although there are a number of other related expression types, bundled in the same add-on package.

Please be aware that *using CSharp expressions in practice can be very cumbersome*; [a number of limitations are documented here]. For most use-cases of C# expressions, it is preferable to use [a `pipe` expression] instead.

[a number of limitations are documented here]: CSharpExpressionLimitations.md
[a `pipe` expression]: PipeExpressions.md

## `csharp` expressions
The TALES `csharp` expression syntax is as follows:

```
csharp:expression_body
```

The `expression_body` is any valid C# expression which would be valid after a `return` keyword. It should not include the return keyword, nor a trailing semicolon.

Remember that *some symbols used by the C# language may need replacing with their entities*, in order to preserve the validity of the HTML/XML document.

### Examples
Some valid CSharp expressions follow:

```html
<div tal:define="four csharp:2 + 2">

<div tal:define="lowerName csharp:name.ToLower()">

<div tal:define="isMoreThanThree csharp:myNumber &gt; 3">
```

## `assemblyref` expressions
In order to make use of members of types, the compiler which evaluates C# expressions must have references to the appropriate assemblies. This includes any assemblies which declare types that are to be used in expressions, as well as any assemblies that contain extension methods you wish to use.

The syntax for assembly reference expressions is:

```
assemblyref:assembly_name
```

The `assembly_name` is the string name of the assembly. Almost always the same as its filename, without the extension. For example: `assemblyref:MyProject.Models`.

Assembly reference expressions are only useful when used in `tal:define` attributes. The assembly reference expression will evaluate to a special value, held by the defined variable. As long as this variable is in-scope, all CSharp expressions which are evaluated will benefit from the referenced assembly.

## `using` expressions
Just like the C# `using` keyword, the using expression imports a namespace into scope, or makes the members of a static class available (in regular C# this would be `using static`).

The syntax for both of the above usages is the same in ZptSharp:

```
using:namespace_or_type_name
```

The `namespace_or_type_name` is either a namespace or the namespace-qualified name to a type.

When used with a namespace it means that:

* Expressions may reference types in this namespace
* Extension methods in this namespace are activated

When used with a type, it brings the static members of that type into scope, without needing to prefix them with their type name. This is equivalent to `using static` in regular C#.

Using expressions are only useful when used in `tal:define` attributes. The using expression will evaluate to a special value, held by the defined variable. As long as this variable is in-scope, all CSharp expressions which are evaluated will benefit from the imported namespace or type.

Note that any namespaces or types referenced by `using` expressions must also have their appropriate assemblies referenced using `assemblyref` expressions.

## `type` expressions
Type expressions inform the compiler which evaluates C# expressions of the design-time type of a variable. By default every variable which is in-scope is treated as a `dynamic` object. This default behaviour can cause problems such as:

* Method overload selection might not function as-expected
* Extension methods are not available, unless they would operate upon `object`

The syntax for a type expression is:

```
type:variable_name type_name
```

The `variable_name` is the name of an already-defined TALES variable. The `type_name` is a string type name, as would be written in a C# typed variable declaration. Remember (particularly for types which use generics) that *greater-than/less-than symbols must be replaced by their entities*.

Type expressions are only useful when used in `tal:define` attributes. The type expression will evaluate to a special value, held by the defined variable. As long as this variable is in-scope, all CSharp expressions which have an appropriately-named variable in-scope will treat that variable as an instance of that type, and not as a `dynamic` object.

Note that any types referenced by `type` expressions must:

* Have their containing assembly referenced, via an `assemblyref` expression
* Have their namespace imported, via a `using` expression (unless the namespace-qualified type name is used in the type expression).

### Example
Here is an example of a `type` expression, indicating that the variable `products` is a generic list of `Product`. Two `using` expressions are included also, because these namespaces must also be imported.

```html
<div tal:define="scgNs using:System.Collections.Generic;
                 prodNs using:MyApp.Models">
    <div tal:define="prodType type:products List&lt;Product&gt;">
        <!-- In this and child scopes, products is strongly-typed and not dynamic -->
    </div>
</div>
```