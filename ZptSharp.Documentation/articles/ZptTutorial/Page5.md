# Tutorial: Introducing METAL macros

So far we have learned how to bind/connect your model data with page templates, this is a part of the ZPT syntax named "TAL".
Now we will begin looking at another part of the ZPT syntax named "METAL".
The purpose of METAL is to declare and consume reusable sections of markup named **macros**; you could think of macros like components of markup if you wish.

## Macros are defined and then used

To keep our example markup tidy, let's replace all of the current markup (from previous tutorial steps) with the following.  Also we will _clear the model_ (replace it with an empty object), we're not going to use any data from the model quite yet.

```html
<html>
<head>
<title>ZptSharp METAL macro usage example</title>
</head>
<body>
<div metal:define-macro="myFirstMacro">
    <h2>This is a METAL macro</h2>
    <p>Here is some content from the macro.</p>
</div>
<p>This is some plain content from the template.</p>
<div metal:use-macro="myFirstMacro">
    <p>This content will be replaced by the macro.</p>
</div>
</body>
</html>
```

Let's see what happens when we render this.
What we see is that the heading "This is a METAL macro" and paragraph "Here is some content from the macro." _is displayed twice_, once before the line which reads "This is some plain content from the template." and once again afterward.

A `metal:define-macro` attribute _does not affect the rendering of a document template on its own_.
The `metal:define-macro` attribute marks the element upon which it appears as a macro, with the name specified in the attribute value.
Macros are only useful when they are used, such as by a `metal:use-macro` attribute.  The `metal:use-macro` attribute replaces _the whole current element and all of its content/descendents_ with the markup of the macro which it uses.

A macro is sometimes referred to as a **subtree** of the markup document.
A subtree always begins with a single root element (the element which contains the `metal:define-macro` attribute) and includes all of that elements content & descendents.

## METAL processing happens before TAL

All ZPT-specific attributes (document-wide) which have the `metal` prefix are processed before any which have the `tal` prefix.
We can demonstrate that with the following modification to the markup; we will add a `tal:content` attribute to the macro and provide some model values from variable definitions.
Replace the entire contents of the `<body>` tag (from the example above) with the following.

```html
<div tal:define="time string:morning">
    <div metal:define-macro="greeting">
        <p tal:content="string:Good ${time}!">Good afternoon!</p>
    </div>
</div>
<div tal:define="time string:night">
    <div metal:use-macro="greeting">
        <p>This will be replaced</p>
    </div>
</div>
```

The outcome from rendering this markup is two paragraphs (surrounded in `<div>` elements), the first reading "Good morning!" and the second reading "Good night!".

### Analysis

Let's walk though this example logically to understand what has happened here:

1. METAL attributes are being processed and the `metal:define-macro` attribute creates a macro named "greeting".
2. METAL attributes are being processed and the `metal:use-macro` attribute is processed.
    * The `<div metal:use-macro="greeting">` element (and all content/descendents) are replaced with the subtree of the document which begins with the `<div metal:define-macro="greeting">` element.
3. TAL attributes are now being processed and the `time` variable is defined as "morning" for the first `<div>` element.
4. TAL attributes are being processed and the `<p>` element (on line 3 of the code listing above) has a `tal:content` attribute processed.
    * This writes content from the expression `string:Good ${time}!`.  Because the variable `time` has the value "morning", it writes "Good morning!".
5. TAL attributes are now being processed and the `time` variable is defined as "night" for the second-outermost `<div>` element.
6. TAL attributes are being processed and the `tal:content` attribute upon the second copy of the `<p>` element from the macro is processed.
    * _This element cannot be directly seen from the source markup_.  It is here because it was copied during the macro-usage in step 2, above.
    * Once again it writes content using the expression `string:Good ${time}!` but in this position the `time` variable has a value of "night", and so the written content is "Good night!".

### Using this technique

Whilst we have demonstrated the above with explicit variable definitions, the same principle holds true for variables created by a `tal:repeat` attribute.
Imagine a product-listing based upon `tal:repeat`, where each listed item is shown using a macro usage.
In each iteration the variable representing the product (which is defined via the repetition) is different.
When the macro uses this variable in TAL attributes, it will be using that iteration's version of the variable.

## Macros may be used from other files

So far we have seen a simple example of a macro which is both defined and used within the same document.
As reusable 'components' of markup though, macros come into their own when they are declared in a file of their own and consumed by referencing that 'shared' source file.
This is the most common way in which macros are used; the examples shown above, whilst simple, are actually quite rare in real applications.

### The greeting macro in its own file

Let's try this technique out; in the same directory as your current template file, create a new text file named `greeting.pt` and set its content to the following:

```html
<html>
<head>
<title>ZptSharp 'greeting' macro</title>
</head>
<body>
<div>
    <div metal:define-macro="greeting">
        <p tal:content="string:Good ${time}!">Good afternoon!</p>
    </div>
</div>
</body>
</html>
```

Notice that we have included HTML markup content outside of the macro itself in this source file.
It's not neccesary to do that at all; when we come to use the macro, everything outside of the `<div metal:define-macro="greeting">` element will be ignored.
It is a helpful & normal technique though, because it helps the developer designing and working upon the macro's markup _to see it in context_ of how it might be used.
When writing macros in their own source files, consider adding context surrounding the macro to help remind you of the expected markup which will surround the macro.
There is no need for this context-markup to include any TAL or METAL attributes, since they will not be processed.

### The macro usage

Let's now change our main template document to use the macro not from the same file but from the newly-created `greeting.pt` file.
Alter the content of the main template document (inside the `<body>` element) to the following:

```html
<div tal:define="time string:morning">
    <div metal:use-macro="container/greeting.pt/macros/greeting">Greeting here</div>
</div>
<div tal:define="time string:night">
    <div metal:use-macro="container/greeting.pt/macros/greeting">Greeting here</div>
</div>
```

### Seeing the result

When we render the main document we should see that the `greeting` macro, defined in the `greeting.pt` source file has been used twice, just as the previous example.
The macro is now truly reusable as a self-contained component.

### More info: The use-macro expression

Let's look in a little more depth at the expression which appears in the `metal:use-macro` attribute value.
The `container` variable is a special automatic variable (a **root context**) which is made available by ZPT.
Using the `container` variable is permitted when the current ZPT document template is being rendered from a file/directory path (or from somewhere which could logically be considered to be 'a container for things').
The variable's value is the container for the current ZPT document template; in this case (because we are rendering a template from a file) it is the directory containing the file.

From the directory, we may navigate to the `greeting.pt` template file in the same directory, then into that template file's collection of `macros` and finally to the macro named `greeting`.

## Next: Defining & filling macro slots

We have seen how macros are reusable subtrees of markup which may be consumed from other documents.
Macros are more powerful than this, though, because they may contain extension points named **slots**.
[Slots enable the injection of markup, customizing the macro, from the point of consumption](Page6.md).