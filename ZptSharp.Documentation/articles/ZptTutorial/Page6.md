# Tutorial: Defining & filling macro slots

So far in our look at METAL macros we have defined & used macros but have yet to customize them at the point of consumption.
The way in which METAL permits macro customization is by defining **slots** within the defined macro and then optionally filling those slots where the macro is used.

## A macro with a slot

Let's adapt the `greeting` macro which we created in the previous tutorial step, specifically the macro which appears in its own `greeting.pt` source file.

### The macro markup

We will change the content of that macro (in its own `greeting.pt` source file) to the following:

```html
<html>
<head>
<title>ZptSharp 'greeting' macro</title>
</head>
<body>
<div>
    <div metal:define-macro="greeting">
        <p>Good <span tal:replace="time">afternoon</span>!</p>
        <p metal:define-slot="activity">What would you like to do?</p>
    </div>
</div>
</body>
</html>
```

### The macro usage

Now let's change the main document template we are rendering to the following, to use the macro:

```html
<html>
<head>
<title>ZptSharp METAL macro usage example</title>
</head>
<body>
<div tal:define="time string:morning">
    <div metal:use-macro="container/greeting.pt/macros/greeting"></div>
</div>
<div tal:define="time string:night;
                 name string:Joe">
    <div metal:use-macro="container/greeting.pt/macros/greeting">
        <p metal:fill-slot="activity">
            It's <em>getting pretty late now <span tal:replace="name">Name</span></em>,
            let's go to bed.
        </p>
    </div>
</div>
</body>
</html>
```

### The result

Let's render this and see what happens in the output.
We see the macro is used twice on the rendered document.

1. The first time the macro appears, the `time` variable was "morning" and so it renders a paragraph "Good morning!".  Also, it renders another paragraph which reads "What would you like to do?"
2. The second time the macro appears, the `time` variable was "night" and so it renders a paragraph "Good night!".  The "What would you like to do?" paragraph does not appear though.  Instead, a paragraph is rendered which reads "It's _getting pretty late now Joe_, let's go to bed."

## How slots and slot-fillers work

The `metal:define-slot` attribute is only permitted upon the descendents of an element which has the `metal:define-macro` attribute and it turns that element (and its descendents) into **a macro slot**.  Within a macro definition there may be zero or more slots defined; there is no upper limit to the number of slots a macro may have, except that each slot must have a unique name within that macro.

Macro slots are much like placeholders, when the macro is used, any slots may optionally be filled with filler content.
A slot is filled by an element which has a `metal:fill-slot` attribute (for the same slot name) which must be a descendent of the element which has the corresponding `metal:use-macro` attribute.
This is an exception to the basic rule of METAL macros that the element with the `metal:use-macro` attribute _and all of its descendents_ is replaced by the macro subtree.
Where the macro usage contains slot fillers, the matching slots are replaced by their corresponding fillers.

Slots which are not filled (do not have a matching `metal:fill-slot` attribute where the macro is used) will display the content from the macro definition.

As you can see from the example above, slot filler content may include markup and even TAL processing attributes.
Indeed, a slot-filler could even make use of a further `metal:use-macro` attribute.

## End of the ZPT tutorial: What's next?

You have reached the end of the ZPT syntax tutorial; all of the most important concepts have been covered.
[The ZPT reference](../ZPTReference/index.md) has a lot more detail about many of the concepts you have learned during the tutorial.

There are also a few more advanced concepts not covered in this tutorial; amongst those are:

* [METAL macro extension](../ZPTReference/Metal/ExtendMacro.md) (similar to creating a subclass of a macro)
* [TAL error handling](../ZPTReference/Tal/OnError.md) (gracefully dealing with errors)
* [The TALES expression types](../ZPTReference/index.md#tales-is-how-expressions-are-written) (we have only touched upon path and string expressions in this tutorial, there are more!)