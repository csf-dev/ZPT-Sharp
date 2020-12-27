# ZPT reference
The ZPT syntax/language is organised into three logical parts.

## METAL is for reusing markup
When processing a ZPT source document, the first attributes processed are METAL attributes.

The concept at the heart of METAL is **macros**. A macro is a reusable subtree of an HTML or XML document. In other words, it is an element and all of that element's descendents & content. Macros are created using the [`define-macro`] attribute and consumed with the [`use-macro`] attribute.

METAL macros may also contain zero or more **slots**. Slots are also single elements which may be replaced ('filled') at the point where the macro is used. Slots are created with the [`define-slot`] attribute and are used/filled using the [`fill-slot`] attribute.

An advanced usage of METAL is *macro extension*. This is somewhat similar in concept to creating a 'subclass' of a macro. The [`extend-macro`] attribute is how this is performed.

[`define-macro`]: Metal/DefineMacro
[`use-macro`]: Metal/UseMacro
[`define-slot`]: Metal/DefineSlot
[`fill-slot`]: Metal/FillSlot
[`extend-macro`]: Metal/ExtendMacro

## TAL binds data to the template
TAL attributes are parsed and resolved after METAL attributes. The purpose of TAL is to bind/write your model and supporting data to the template.

The table below shows and links to the various TAL attributes. It also shows the order in which they are processed (if, for example, multiple attributes appear on the same element).

| Attribute          | Summary |
| ---------          | ------- |
| [`tal:define`]     | Creates variables which are usable by subsequent directives. |
| [`tal:condition`]  | Removes the element if the associated expression is not "truthy" |
| [`tal:repeat`]     | Repeats the element (and contents) for each item in an `IEnumerable` |
| [`tal:content`]    | Replaces the element's contents with a value from an expression |
| [`tal:replace`]    | Replaces the whole element with a value from an expression |
| [`tal:attributes`] | Sets attributes upon the element using expression values |
| [`tal:omit-tag`]   | Removes the start & end tags of an element, whilst preserving its content/children |
| [`tal:on-error`]   | Handles processing errors on the current element and children (similar to a C# `catch`) |

[`tal:define`]: Tal/Define
[`tal:condition`]: Tal/Condition
[`tal:repeat`]: Tal/Repeat
[`tal:content`]: Tal/ContentAndReplace
[`tal:replace`]: Tal/Replace
[`tal:attributes`]: Tal/Attributes
[`tal:omit-tag`]: Tal/OmitTag
[`tal:on-error`]: Tal/OnError

### Notes
* TAL content and replace attributes are mutually exclusive. It is invalid for the same element to have both.
* *The on-error attribute does not fit into the usual order of processing*. It is only processed if/when an unhandled exception occurs when processing another TAL attribute upon the same or a child element.

## TALES is how expressions are written
Both METAL and TAL attributes make use of expressions to locate the desired macros, content or logic.

TALES is *an extensible syntax* and so plugins may add further expression types. These are the expression types which are officially supported by ZptSharp.

### Path expressions
[Path expressions] look a lot like URLs. They offer a simple mechanism to access model values or other data.

*Path expressions are the default TALES expression.*

[Path expressions]: Tales/PathExpressions

### String expressions
String expressions 