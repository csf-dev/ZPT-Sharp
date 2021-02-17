# The `metal:fill-slot` attribute

The `metal:fill-slot` attribute is used at the point of consuming a METAL macro to provide slot-filler content for one of the slots in the consumed macro.
Content may only be provided for slots which exist in the the consumed macro, created via [the `metal:define-slot` attribute].

`metal:fill-slot` attributes are only valid upon descendents of an element which has either the [`metal:use-macro`] or [`metal:extend-macro`] attributes; in the latter case, for _macro extension_.

[the `metal:define-slot` attribute]: DefineSlot.md
[`metal:use-macro`]: UseMacro.md
[`metal:extend-macro`]: ExtendMacro.md

## How slots are filled

When a METAL macro defines one or more slots, these are subtrees within the macro which may optionally be replaced by an alternative subtree, provided by the element which contains a corresponding `metal:fill-slot` attribute.
If a slot is defined but at the point of consumption there is no corresponding `metal:fill-slot` attribute, then the original subtree (where the slot was defined) will be used, as if the slot did not exist.

## Examples

These examples use macros which are defined in the same template document; _this is only for simplicity of the examples_.
The most common usage of METAL macros sees each macro declared in a separate source file of its own.
Slots and slot-fillers function whether the macro is declared in the same document or an external document, [as shown in this example of macro usage].

[as shown in this example of macro usage]: UseMacro.md#consumption-of-a-macro-declared-in-a-different-file

### Filling a single slot

The following code listing shows an HTML template which defines one macro (named `sampleMacro`) with one slot (named `slotOne`).
It also includes a usage of that macro and a matching slot-filler.
Please note that to demonstrate that whole elements are replaced, the macro and slot elements include HTML `class` attribute as well.

```html
<html>
<head><title>Macro slot filling example</title></head>
<body>
<div metal:define-macro="sampleMacro" class="aMacro">
    <h2>This is a macro</h2>
    <div metal:define-slot="slotOne">Slot content here</div>
    <p>This is the end of the macro</p>
</div>
<div metal:use-macro="sampleMacro">
    <div metal:fill-slot="slotOne" class="info">
        <p>This is the slot-filler content</p>
    </div>
</div>
</body>
</html>
```

Rendering the above document would produce the following results:

```html
<html>
<head><title>Macro slot filling example</title></head>
<body>
<div class="aMacro">
    <h2>This is a macro</h2>
    <div>Slot content here</div>
    <p>This is the end of the macro</p>
</div>
<div class="aMacro">
    <h2>This is a macro</h2>
    <div class="info">
        <p>This is the slot-filler content</p>
    </div>
    <p>This is the end of the macro</p>
</div>
</body>
</html>
```

### Filling one slot and leaving another unfilled

This example shows what happens when a slot if left unfilled (there is no matching `metal:fill-slot` attribute for the slot name).

```html
<html>
<head><title>Macro slot filling example</title></head>
<body>
<ul class="products">
    <li metal:define-macro="productItem">
        <div class="imageContainer">
            <img metal:define-slot="productImage" href="images/noImageAvailable.png">
        </div>
        <div class="infoUnavailable" metal:define-slot="productInfo">
            <p>Further information unavailable.</p>
        </div>
    </li>
    <li metal:use-macro="productItem">
        <div class="productInfo" metal:fill-slot="productInfo">
            <p>This is some sample product info.</p>
        </div>
    </li>
</ul>
</body>
</html>
```

Rendering the above document would produce a result which looks like the following.
Notice how in the usage of the macro, the `<img>` element still has the original slot content, its `src` attribute points to `images/noImageAvailable.png`.
This is because the macro-usage did not include any slot filler for the `productImage` slot.
Filler content _was provided_ for the `productInfo` slot though, and so this renders with the filler content instead of the original macro content.

```html
<html>
<head><title>Macro slot filling example</title></head>
<body>
<ul class="products">
    <li>
        <div class="imageContainer">
            <img src="images/noImageAvailable.png">
        </div>
        <div class="infoUnavailable">
            <p>Further information unavailable.</p>
        </div>
    </li>
    <li>
        <div class="imageContainer">
            <img src="images/noImageAvailable.png">
        </div>
        <div class="productInfo">
            <p>This is some sample product info.</p>
        </div>
    </li>
</ul>
</body>
</html>
```