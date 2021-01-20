# Tutorial: Conditions & repetition

In this step of the tutorial we will learn about the `tal:condition` and `tal:repeat` attributes and how they control the rendering of a document template.

## `tal:condition` can remove an element

Let's make two changes to our one-page/one-model ZptSharp environment.
First, let's add the following to the markup (just before the close of the `<body>` tag is fine):

```html
<div tal:condition="here/AmITruthy">This renders if AmITruthy is true</div>
```

Second, let's make a change to the model so that it now looks like this:

```csharp
{ Message = "Hello world!", AmITruthy = true }
```

Now let's render the document once again; as you might expect, boolean `true` is "truthy" and so the `div` element is rendered into the output document.
Change the `AmITruthy` value in the model to `false` and render once again.
This time, ZptSharp will have _removed_ the div element _and all of its descendents_.
When a `tal:condition` attribute removes an element it matters not if that element has no descendents, just text node descendents or if it contains entire trees of markup, _all descendents are removed_.

You may experiment with different values for the `AmITruthy` model value to get a feel for what is considered 'truthy' and what is 'falsey'.
As a start, `null` references and empty strings are falsey.
A complete list of these may be found in the [`tal:condition` reference documentation].

[`tal:condition` reference documentation]: ../ZptReference/Tal/Condition.md

## `tal:repeat` is like `foreach` for an element

Let's make another change now, to both our document template and model.
Let's add the following to the document:

```html
<ul>
    <li tal:repeat="food here/Foods">
        I <em>really like</em> eating <span tal:replace="food">apples</span>.
    </li>
</ul>
```

In our model, let's add another property so that the model now looks like this (reformatted for readability):

```csharp
{
    Message = "Hello world!",
    AmITruthy = true,
    Foods = new string[] {
        "oranges",
        "bananas",
        "cucumber"
    },
}
```

Let's render this; what we will see is that `tal:repeat` works a lot like a C# `foreach` statement.
In this case we will see the `<li>` element _and all of its descendents_ are repeated three times, once for each listed food.
In each iteration a variable is created, named `food`, holding the value from the current iteration.
We use this variable in a `tal:replace` attribute (as described in [the previous tutorial page]) within the list item in order to show it on the rendered output.

Just like the `foreach` keyword in C#, `tal:repeat` works on objects that are `IEnumerable`.

[the previous tutorial page]: index.md

## Next: More on variables & expressions

In the next tutorial page we will [look deeper at variables & the different types of available expressions](Page3.md).