# The Python expression syntax

TALES python expressions allow the evaluation of arbitrary Python language expressions in your document templates.
The syntax for this is:

```text
python:python_expression
```

The python expression is any valid Python language expression.
Python expressions have access to all of [the same root contexts] and [defined variables] as other expressions.

[the same root contexts]: GlobalContexts.md
[defined variables]: ../Tal/Define.md

## Python version

Python expressions make use of [the IronPython library] to provide the expression backend.
At the time of writing, IronPython supports **Python version 2.7**.

IronPython does have an ongoing effort to add support for 3.x; at the time of writing it is not yet ready for use.

[the IronPython library]: https://ironpython.net/

## Python expressions are included in an add-on package

Support for `python` expressions are included in [the ZptSharp.PythonExpressions NuGet package].
They are not one of the standard expression types; once the NuGet package is installed to your application they must be activated [by using the `AddZptPythonExpressions` method].

[the ZptSharp.PythonExpressions NuGet package]: ../../NuGetPackages.md#expression-evaluators
[by using the `AddZptPythonExpressions` method]: xref:ZptSharp.PythonHostingBuilderExtensions.AddZptPythonExpressions(ZptSharp.Hosting.IBuildsHostingEnvironment)

## Reserved markup characters must be encoded

As with all usage of expressions inside of markup, any usages of reserved characters must be encoded.
For example, the python expression `print("%.2f" % 13.946)` makes use of double-quote characters.
The following usage would obviously fail, because the quote characters break the DOM when used inside of an attribute.

```html
<p>The price is <span tal:replace="python:print("%.2f" % 13.946)">0.00</span></p>
```

The solution is to encode the reserved markup characters, replacing double-quotes with `&quot;`:

```html
<p>The price is <span tal:replace="python:print(&quot;.2f&quot; % 13.946)">0.00</span></p>
```

## Example

Here is a short example of a Python expression in use.
It is somewhat contrived but it demonstrates how a Python expression may make use of a defined variable.

```html
<p tal:define="two python:2">
    2 plus 2 equals <span tal:replace="python:two + 2">0</span>.
</p>
```