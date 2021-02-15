# The `metal:extend-macro` attribute

Macro extension is an advanced usage of METAL.
The `metal:extend-macro` attribute facilitates creating a new macro which derives from an existing macro.
You may think of this similarly to 'subclassing' the existing macro.

The `metal:extend-macro` attribute is _only valid_ where it appears on the exact same element as [a `metal:define-macro` attribute].
The value of the `metal:extend-macro` attribute is an expression which must evaluate to an existing macro, the same kind of expression as would be used for [a `metal:use-macro` attribute].

The meaning of the define & extend macros together is that a new macro is created, which extends an existing one.
The newly-defined macro (which extends another) is now consumable as a macro in its own right.
It may be used via `metal:use-macro` or extended further by another `metal:extend-macro` attribute.

[a `metal:define-macro` attribute]: DefineMacro.md
[a `metal:use-macro` attribute]: UseMacro.md

## Macro extension is about the slots

When a macro extends another (the **extender** extends an **extended** macro), the nature of the extension relates entierly to the **slots** in the extender and the extended macro.

### An extender may fill slots from the extended macro

In a macro that extends another, the extender may include any number of [`metal:fill-slot` attributes].
These attributes must correspond to defined slots in the extended macro.

In this scenario, the extender provides a common filler for the slots defined in the extended macro.
The slots from the extended macro are not available to consumers of the extender.
Any slots which the extender _does not fill_ are left as they are; they may be filled by the macro-usage or they may be left to show their default content from where they are defined.

Here is an example of that usage of macro extension, both macros and usage have been condensed into a single source file _for brevity of the example_.
Typically, the macros will each be in their own source files.

```html
<html>
<head><title>Macro extension example</title></head>
<body>
<div metal:define-macro="baseMacro">
    <p metal:define-slot="slotOne">Slot one default content.</p>
    <p metal:define-slot="slotTwo">Slot two default content.</p>
</div>
<div metal:define-macro="extendedMacro" metal:extend-macro="baseMacro">
    <p metal:fill-slot="slotOne">Slot one extended content.</p>
</div>
<div metal:use-macro="extendedMacro">
    <p metal:fill-slot="slotTwo">Slot two filler content.</p>
</div>
</body>
</html>
```

The rendering result (just the area of the document which uses the macro, not including the original macro definitions) would look like this.

```html
<div>
    <p>Slot one extended content.</p>
    <p>Slot two filler content.</p>
</div>
```

[`metal:fill-slot` attributes]: FillSlot.md

### An extender may redefine slots from the extended macro

Following-on from the scenario above, in a macro that extends another, the extender may include pairs of `metal:fill-slot` & `metal:define-slot` attributes for the same slot name.
This is how an extender macro may _redefine a slot_ which was made available by the extended macro.

Typical usages of this technique are to add decoration to the slot, as appropriate to the function of the extender macro.
Let's look at another example, once again the macros and usage are condensed into a single file for brevity.

```html
<html>
<head><title>Macro extension example</title></head>
<body>
<div metal:define-macro="baseMacro">
    <div metal:define-slot="slotOne">Slot one default content.</div>
    <p>Footer from base macro.</p>
</div>
<div metal:define-macro="extendedMacro" metal:extend-macro="baseMacro">
    <div metal:fill-slot="slotOne" tal:omit-tag="">
        <h2>This is a heading from the extender</h2>
        <div metal:define-slot="slotOne">Slot one default content from extender.</div>
    </div>
</div>
<div metal:use-macro="extendedMacro">
    <p metal:fill-slot="slotOne">Slot one filler content.</p>
</div>
</body>
</html>
```

When rendering this document, the area where the macro is used would look like the following.
Also notice in the example that the slot-filler included [a `tal:omit-tag` attribute].
The tag omission has nothing specific to do with macro extension, but it does demonstrate how we can avoid the need for extra tags to be written to the rendered output.

```html
<div>
    <h2>This is a heading from the extender</h2>
    <p>Slot one filler content.</p>
    <p>Footer from base macro.</p>
</div>

```

[a `tal:omit-tag` attribute]: ../Tal/OmitTag.md

### An extender may create new slots within slot-fillers

The final common usage of macro extension is for the extender macro to fill a slot from the extended macro, and within that filler content, to define new slots of its own with new names.
In reality this differs very little from the redefinition of existing slots as described above.
Any macro consuming the extender now has a different set of slots available to those which are available on the extended macro.