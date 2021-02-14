# The `metal:define-macro` attribute

The `metal:define-macro` attribute indicates that the element upon which it appears is a METAL macro.
The value of this attribute indicates the _name_ of that macro.
The names of METAL macros defined within a document template must be unique within that template.

## Consuming a METAL macro

The presence of a `metal:define-macro` attribute alone has no effect upon how that element or any of its descendents will render in the output.
To make use of a METAL macro created via the `metal:define-macro` attribute, the macro must be consumed.
This is accomplished via [the `metal:use-macro` attribute].

[the `metal:use-macro` attribute]: UseMacro.md

## A METAL macro

A METAL macro is the subtree of the DOM beginning at the element upon which the `metal:define-macro` attribute appears.
That is, the macro is the defining element as well as all of its content & descendents.

## Macro slots

Macros may contain zero or more slots, each designated [via the `metal:define-slot` attribute].
The element which has the `metal:define-macro` attribute may not also have a `metal:define-slot` attribute but any of its descendents may.

[via the `metal:define-slot` attribute]: DefineSlot.md

## Macro extension

An element which defines a METAL macro via `metal:define-macro` may optionally be declared as an extension of another existing macro.
This is accomplished by placing [a `metal:extend-macro` attribute] upon the same element.
`metal:extend-macro` attributes _are only valid_ where they appear upon the same element as a `metal:define-macro` attribute.

[a `metal:extend-macro` attribute]: ExtendMacro.md
