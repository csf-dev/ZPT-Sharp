# Tutorial: Variables & expressions

So far through this tutorial we have been using some simple expressions to access values from the model.
Examples include `here/Message` and `here/Foods`.
In this tutorial page we will look at those expressions in more depth, along with how we may declare variables.
ZPT expressions use an extensible syntax named TALES.

## Introducing path expressions

The expressions used so far in this tutorial make use of the default TALES expression type, named **path** expressions.
These expressions look similar to a URL path, with forward-slashes indicating descent into a 'child' object or reference.
Let's try another one now, make a change to the model and add the following (the rest of the model is omitted for brevity):

```csharp
{
    AnObject = new {
        ChildObject = new {
            Value = 42
        }
    }
}
```

Now add the following the document template source file.
When we render it, the `span` element will be replaced by the text 42.

```html
<p>The answer is <span tal:replace="here/AnObject/ChildObject/Value">10</span>.</p>
```

Path expressions are _the default expression type_ for ZptSharp.
Strictly-speaking, in the example above, we could have written the `tal:replace` attribute value as `path:here/AnObject/ChildObject/Value`.
Because path expressions are the default though, the `path:` prefix is not required.
Any unprefixed expression is assumed to be a `path:` expression.

Something else to point out is how both `tal:content` and `tal:replace` attributes will convert any value they receive to string.
In this case, the integer 42 was converted to its string representation via the built-in `Object.ToString()` method.

## Introducing string expressions

Another available expression type is the **string** expression.
These use the prefix `string:` (or shortened `str:`) and are used for dynamically building strings, in a similar style to [C# interpolated strings].
Let's try replacing the HTML we added in the above step with the following:

```html
<p tal:content="string:The answer is ${here/AnObject/ChildObject/Value}.">The answer goes here.</p>
```

When we render it, the result is the same.
The string expression-type uses the placeholder `${...}` to insert data into a literal string value.
Inside the placeholder is evaluated as a TALES expression in its own right.
As you see, in this case we used a path expression to provide the placeholder value.

[C# interpolated strings]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated

## Defining variables

Notice how every path expression used so far has begun with the identifier `here`?
`here` is a ZPT built-in variable (we call those variables "root contexts") and it provides access to the model.
We may define our own variables though, using the `tal:define` attribute.
Let's make another change to the HTML that we were using above:

```html
<p tal:define="answer here/AnObject/ChildObject/Value">
    The answer is <span tal:replace="answer">10</span>.
</p>
```

Once again, upon rendering the result is the same.
The `tal:define` attribute allows us to define a variable named `answer` which has the value of the path expression `here/AnObject/ChildObject/Value`.
Variable definitions don't have to use path expressions to get their value, they may use any TALES expression type which has been installed & activated.

## Variable scope

By default, variable definitions produce **local variables**; these variables have a scope which follows the DOM.
Local variables are 'visible'/usable on the same element as which they are defined, as well as upon any descendent element.
Local variables _are not usable_ 'outside' of the element on which they are defined.
Local variables may also be _redefined_ upon a descendent element.
Let's try a short experiment in the template document source:

```html
<div tal:define="varOne string:I am variable one">
    <p tal:content="varOne">Message from a variable.</p>
    <p tal:define="varOne string:But I am variable two"
       tal:content="varOne">Message from a variable.</p>
    <p tal:content="varOne">Message from a variable.</p>
</div>
```

If you render the document, you will see a result similar to:

> I am variable one
>
> But I am variable two
>
> I am variable one

When a variable is redefined, the newly-defined variable (upon the descendent element) _hides_ the original variable of the same name, for the parts of the DOM where the redefined variable is in-scope.
This is why in the second `<p>` element the redefined `varOne` is 'visible' and takes precedence over the original `varOne` (defined upon the containing `<div>` element).
Once the redefined variable passes out-of-scope (outside the second `<p>`), the original `varOne` becomes visible & usable again.

## Repeat attributes create variables too

We have seen how `tal:define` may be explicitly used to define (or redefine) a variable.
[In the previous tutorial page] we also saw how a `tal:repeat` attribute also defines a variable.
In that example, the created variable is named `food`.
The variable created by a repeat attribute holds the value of the current iteration through the `IEnumerable`.

[In the previous tutorial page]: Page2.md

## Next: Manipulating attributes & omitting tags

In the next tutorial page we learn [how to set, unset and change attribute values within elements, as well as how to omit an element tag whilst preserving its content](Page4.md).
