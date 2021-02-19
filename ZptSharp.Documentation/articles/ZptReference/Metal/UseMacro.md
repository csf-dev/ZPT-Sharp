# The `metal:use-macro` attribute

The `metal:use-macro` attribute is used to consume a METAL macro which was [defined via the `metal:define-macro` attribute].
The value of the `metal:use-macro` attribute is a TALES expression which indicates the METAL macro to be consumed.

[defined via the `metal:define-macro` attribute]: DefineMacro.md

## The consuming element is replaced by the macro

Upon consumption, the element upon which the `metal:use-macro` attribute appears, as well as all of its content & descendents, is replaced with a copy of the macro's subtree.
The exception to this replacement process is when the macro includes **slots** and where the consuming usage of the macro provides **filler** content.
See documentation for the [`metal:define-slot`] & [`metal:fill-slot`] attributes for more information.

[`metal:define-slot`]: DefineSlot.md
[`metal:fill-slot`]: FillSlot.md

## The macro expression

The expression which is used as the value for the `metal:use-macro` attribute may be any valid TALES expression.
For a macro defined within the same template document, the usage of the macro must occur at a document position after the macro is defined.
In this case the expression is simply the name of the macro to use.

It is common to consume METAL macros from external documents though, where each macro is defined in its own template document.
In this case, the expression would usually be a TALES path expression making use of one of:

* [The `container` root context variable], which allows construction of a relative path, based upon the location of the current template
* _When using an MVC ViewEngine_ (either MVC5 or Core MVC) [the `Views` root context] which represents the root of the `Views` directory
* A reference into the model which makes the template available, such as via an instance of [the `TemplateDirectory` class]

[The `container` root context variable]: ../Tales/GlobalContexts.md#container
[the `Views` root context]: ../../ViewEngines.md#added-tales-contextsvariables-for-mvc
[the `TemplateDirectory` class]: xref:ZptSharp.TemplateDirectory

## Filling slots

If the consumed macro defines one or more slots then for each of these slots, the consuming markup (where `metal:use-macro` is used) may choose whether or not to fill each of these slots.

Filling slots is achieved by elements which are descendents of the element upon which the `metal:use-macro` attribute appears.
Each descendent element which has [a `metal:fill-slot` attribute] will fill one slot (of a matching name).
Slots do not need to be filled.  If a slot is defined in the macro definition but is not filled by the consuming markup then the content & descendents of the `metal:define-slot` element will be copied through to the position where the macro is used, as if the slot had not been defined.

[a `metal:fill-slot` attribute]: FillSlot.md

## Examples

The two examples below do not include examples of _filling slots_ or of _macro extension_.
Examples of these techniques may be found in the documentation for [the `metal:fill-slot` attribute] and [the `metal:extend-macro` attribute] respectively.

[the `metal:fill-slot` attribute]: FillSlot.md
[the `metal:extend-macro` attribute]: ExtendMacro.md

### Consumption of a macro declared in the same file

```html
<html>
<head><title>Macro example</title></head>
<body>
<div metal:define-macro="myMacro">
    <p>This is the content from my macro</p>
</div>
<div metal:use-macro="myMacro">
    <p>This will be replaced</p>
</div>
</body>
</html>
```

Upon rendering the above document, the result will be as follows.

```html
<html>
<head><title>Macro example</title></head>
<body>
<div>
    <p>This is the content from my macro</p>
</div>
<div>
    <p>This is the content from my macro</p>
</div>
</body>
</html>
```

### Consumption of a macro declared in a different file

For this example, presume that both the following documents are stored on a file system in the same directory.

#### File one: **`document.pt`**

```html
<html>
<head><title>Macro consumer</title></head>
<body>
<div metal:use-macro="container/myMacro.pt/macros/myMacro">
    <p>This will be replaced</p>
</div>
</body>
</html>
```

#### File two: **`myMacro.pt`**

```html
<html>
<head><title>Macro definer</title></head>
<body>
<div metal:define-macro="myMacro">
    <p>This is the content from my macro</p>
</div>
</body>
</html>
```

#### Expected rendering result

Upon rendering the document `document.pt`, this is the expected outcome.

```html
<html>
<head><title>Macro consumer</title></head>
<body>
<div>
    <p>This is the content from my macro</p>
</div>
</body>
</html>
```
