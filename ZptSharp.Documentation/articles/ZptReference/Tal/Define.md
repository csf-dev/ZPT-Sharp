# The `tal:define` attribute

The `tal:define` attribute is used to define one or more new variables within the template, using TALES expressions to provide the variable value.

## Attribute value syntax

The value for a `tal:define` attribute is composed of the following space-separated parts:

* An _optional_ scope signifier
* A variable name
* The TALES expression providing the variable value

If the expression used to define the variable needs to make use of the semicolon `;` character, then this must be escaped by doubling-it-up: `;;`.
This could occur, for example, if a semicolon is used in [a `string` expression].
The reason for this is as described below - the semicolon character is used to separate multiple variable definitions.

[a `string` expression]: ../Tales/StringExpressions.md

### Defining more than one variable in the same attibute

A single `tal:define` attribute value may define more than one variable.
Where more than one variable is defined, each definition must be separated with a semicolon `;` character.
There _may_ also be any amount of whitespace before or after these semicolons, as appropriate for readability.


## Variable names

In ZptSharp, [all variable names must be valid C# variable names].
Whilst it may be possible in some cases to use names which are not valid C# variable names, this is highly discouraged.
Particularly if the [`csharp` expressions] or [`python` expressions] packages are installed, the presence of invalid names may cause unexpected errors.

[all variable names must be valid C# variable names]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/identifier-names
[`csharp` expressions]: ../Tales/CSharpExpressions.md
[`python` expressions]: ../Tales/PythonExpressions.md

## Variable scope

Every variable which is defined is either a **local** variable or a **global** one.
This may be chosen by including a scope signifier, either the word `local` or `global`, before the variable name.
If a scope signifier is omitted then the variable will be a local one.

The scope of a defined variable decides where it may be used and how it may be hidden or overridden by another variable of the same name.
It is _strongly recommended that your own variables should be locally-scoped_ where possible.

### Local variables

Local variables have a scope which follows the structure of the DOM.
A locally-scoped variable is **in-scope** (may be used) for TAL attributes upon the element which defines the variable and for TAL attributes upon _any descendent of the element which defines it_.
Outside of the element which defines it, for example sibling or parent elements, a locally-scoped variable is treated _as if it was not defined_ - it is **out of scope**.

If there is both a global and a local variable of the same name in-scope for the same element, then _by default the local variable takes precedence_.
A local variable which is in-scope 'hides' a global variable of the same name.
There is a workaround to this scenario if desired, using [explicit `global:` or `local:` variants of the path expression syntax].
In general though, _it is better to avoid local and global variables with the same names_.

Local variables may also be hidden by new local variables defined at a descendent scope.
If a local variable is defined upon a given element and another local variable is defined using the same name upon a descendent element then the variable defined in the descendent element will 'hide' the variable from the original element as long as it is in-scope.

[explicit `global:` or `local:` variants of the path expression syntax]: ../Tales/PathExpressions.md#explicitly-selecting-local-or-global-variables

### Global variables

Global variables do not follow the structure of the DOM; they are in-scope (usable) from the point in the document where they are defined and onwards in simple document order.
A global variable may be used by TAL attributes upon elements which are outside of the subtree of the element that defined it, as long as those attributes appear in the document after the variable definition.

As noted above, any global variable will be 'hidden' by a local variable of the same name; it is advised to avoid this scenario if possible by not giving global and local variables the same names.
Global or local variables [may be explicitly used in expressions by using variants of the path expression syntax].

Because global variables do not follow any structured rules about their scope, if two global variable definitions exist in a document for the same name then subsequent definitions will update/redefine the variable value.
In this case the original value will be discarded and the global variable will provide the new value from that point in the document onwards.

[may be explicitly used in expressions by using variants of the path expression syntax]: ../Tales/PathExpressions.md#explicitly-selecting-local-or-global-variables

## Aborting a variable definition

If the result of a variable definition expression in a `tal:define` attribute is [an instance of `AbortZptActionToken`], such as via [the root context `default`], then that particular variable is not defined or altered.
The behaviour is as if that variable were not included in the `tal:define` attribute.

In a `tal:define` attribute which declares more than one variable, each variable definition is treated individually.
This means that if a single variable evaluates to an abort-action token but a different variable does not then only the aborted definition is not performed.
Other definitions in the same attribute will go-ahead and be defined as normal.

[an instance of `AbortZptActionToken`]: xref:ZptSharp.Expressions.AbortZptActionToken
[the root context `default`]: ../Tales/GlobalContexts.md#default

## Examples

### Defining a single local variable

In this example, a local variable is defined and then used by a child element.

```html
<div tal:define="myVariable string:This is my variable">
    <p tal:content="myVariable">This will read "This is my variable".</p>
</div>
```


### Defining a single global varibale

In this example, a global variable is defined and then used by a subsequent sibling element.

```html
<p tal:define="global myVariable string:This is my variable">This is some text.</p>
<p tal:content="myVariable">This will read "This is my variable".</p>
```

### Defining three variables

In this example, three variables are defined using the same attribute.
This includes a mixture of both local and global variable; the prefix `local` is not required since it is the default behaviour where there is no scope signifier.
The whitespace which puts each variable definition on a new line _is not required by the ZPT syntax_; it is allowed for readability though.

```html
<div tal:define="variableOne string:This is variable one;
                 global variableTwo string:This is variable two;
                 variableThree string:This is variable three">
</div>
```

### Demonstrating local variable scope

This example demonstrates the way in which local variable scope and 'hiding' of variables with the same name works.
In this example, the second of three `<p>` elements defines a new local variable of the same name (`varOne`) as was defined by an ancestor element.
This means that the new definition will 'hide' the previous one for as long as the new definition is in-scope.
The 'hiding' effect ends once the new definition passes out of scope, as shown by the third `<p>` element.

```html
<div tal:define="varOne string:I am variable one">
    <p tal:content="varOne">This will read "I am variable one".</p>
    <p tal:define="varOne string:But I am variable two"
       tal:content="varOne">This will read "But I am variable two".</p>
    <p tal:content="varOne">This will read "I am variable one".</p>
</div>
```