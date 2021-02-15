# The `metal:define-slot` attribute

The `metal:define-slot` attribute indicates that the element upon which it appears is a slot in a METAL macro.
The value of this attribute indicates the _name_ of that slot.
The names of slots in a METAL macro must be unique within that macro.

`metal:define-slot` attributes are only valid upon elements which are descendents of [a `metal:define-macro` attribute].

[a `metal:define-macro` attribute]: DefineMacro.md

## Macro slots

A macro slot is much like a placeholder for arbitrary markup; that markup may be specified at the point where the macro is consumed.
This is a mechanism by which macros may be customized or used to 'wrap' other content.

The slot is the DOM subtree beginning with the element which has the `metal:define-slot` attribute.
In other words, it is that element and all of its content/descendents.

## Filling macro slots

A `metal:define-slot` attribute on its own has no effect upon rendering.
Its only function is to allow [a `metal:fill-slot` attribute] to provide 'slot filler' content at the point of usage.

[a `metal:fill-slot` attribute]: FillSlot.md

### Slots which are unfilled

If - when the macro is used by [a `metal:use-macro` attribute] - that usage does not provide `metal:fill-slot` attributes for all of the slots that are defined, then any 'unfilled slots' will render their original slot content from the macro.

[a `metal:use-macro` attribute]: UseMacro.md

## Example

The following example shows only how to create a macro named `productItem` with two slots named `productImage` & `productInfo`.
It does not include the usage of the macro or the filling of these slots.
Examples various slot-filling scenarios may be found in the documentation for [the `metal:fill-slot` attribute].

```html
<li metal:define-macro="productItem">
    <div class="imageContainer">
        <img metal:define-slot="productImage" src="images/noImageAvailable.png">
    </div>
    <div class="infoUnavailable" metal:define-slot="productInfo">
        <p>Further information unavailable.</p>
    </div>
</li>
```

[the `metal:fill-slot` attribute]: FillSlot.md
