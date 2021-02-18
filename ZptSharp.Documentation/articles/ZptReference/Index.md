# ZPT reference

The ZPT syntax/language is organised into three logical parts.
Each primary mention of an attribute below is a link to its reference page.

## METAL is for reusing markup

When processing a ZPT source document, the first attributes processed (through the entire document) are METAL attributes.
_Only once all METAL attributes are processed_ does ZptSharp process TAL attributes.

The concept at the heart of METAL is **macros**. A macro is a reusable subtree of an HTML or XML document. In other words, it is an element and all of that element's descendents & content. Macros are created using [the `metal:define-macro` attribute] and reused within a document with [the `metal:use-macro` attribute].

METAL macros may also contain zero or more **slots**. Slots are also single elements which may be replaced ('filled') at the point where the macro is used. Slots are created with [the `metal:define-slot` attribute] and are used/filled using [the `metal:fill-slot` attribute].

An advanced usage of METAL is *macro extension*. This is somewhat similar in concept to creating a 'subclass' of a macro. [The `metal:extend-macro` attribute] is how this is performed.

[the `metal:define-macro` attribute]: Metal/DefineMacro.md
[the `metal:use-macro` attribute]: Metal/UseMacro.md
[the `metal:define-slot` attribute]: Metal/DefineSlot.md
[the `metal:fill-slot` attribute]: Metal/FillSlot.md
[The `metal:extend-macro` attribute]: Metal/ExtendMacro.md

## TAL binds data to the template

TAL attributes are parsed and resolved after METAL attributes. The purpose of TAL is to bind/write your model and supporting data to the template.

The table below shows and links to the various TAL attributes.
It also shows the order in which they are processed (if, for example, multiple attributes appear on the same element).

| Order | Attribute          | Summary                                                                                  |
| ----- | ---------          | -------                                                                                  |
| 1st   | [`tal:define`]     | Creates variables which are usable by subsequent directives.                             |
| 2nd   | [`tal:condition`]  | Removes the element if the associated expression is not "truthy"                         |
| 3rd   | [`tal:repeat`]     | Repeats the element (and contents) for each item in an `IEnumerable`                     |
| 4th*  | [`tal:content`]    | Replaces the element's contents with a value from an expression                          |
| 4th*  | [`tal:replace`]    | Replaces the whole element with a value from an expression                               |
| 5th   | [`tal:attributes`] | Sets attributes upon the element using expression values                                 |
| 6th   | [`tal:omit-tag`]   | Removes the start & end tags of an element, whilst preserving its content/children       |
| N/A** | [`tal:on-error`]   | Handles processing errors on the current element and children (similar to a C# `catch`)  |

[`tal:define`]: Tal/Define.md
[`tal:condition`]: Tal/Condition.md
[`tal:repeat`]: Tal/Repeat.md
[`tal:content`]: Tal/ContentAndReplace.md
[`tal:replace`]: Tal/ContentAndReplace.md
[`tal:attributes`]: Tal/Attributes.md
[`tal:omit-tag`]: Tal/OmitTag.md
[`tal:on-error`]: Tal/OnError.md

### Notes for the above table

* \* TAL content and replace attributes are *mutually exclusive*. It is invalid for the same element to have both.
* ** The on-error attribute *does not fit into the usual order of processing*. It is only processed if/when an error occurs.

## TALES is how expressions are written

Both METAL and TAL attributes make use of expressions to locate the desired macros, content or logic.
TALES is *an extensible syntax* and so plugins may add further expression types. These are the expression types which are officially supported by ZptSharp.

In the examples below all expressions are shown *using their fully qualified/prefixed names*.  Every rendering operation has *a default expression type* which does not require a prefix.  If not specified otherwise [using the configuration], this default type will be **path** expressions.

| Name          | Summary                                                                                                                               |
| ----          | -------                                                                                                                               |
| [`path`]      | Provide access to objects using a syntax which looks a lot like the path portion of a URL.  For example `path:here/Customers/3/Name`  |
| [`string`]    | Create interpolated strings including content from other expressions.  For example `string:Hello ${path:name}, how are you?`          |
| [`not`]       | Coerce another expression result to boolean, then negate it.  For example `not:path:loan/IsOverdue`                                   |
| [`pipe`]      | Use a delegate to transform a value.  For example `pipe:name path:pipes/ToLowercase`                                                  |
| [`csharp`]    | Evaluate arbitrary C# expressions (but *[please read about the limitations]*).  For example `csharp:name.ToLowerInvariant()`          |
| [`python`]    | Evaluate arbitrary expressions written in the Python 2 language.  For example `python:2 + 2`                                          |
| [`structure`] | Indicates that an 'inner' expression should be interpreted as structure/markup when rendered.  For example `structure:path:here/SomeMarkup`                                          |

[using the configuration]: xref:ZptSharp.Config.RenderingConfig.DefaultExpressionType
[`path`]: Tales/PathExpressions.md
[`string`]: Tales/StringExpressions.md
[`not`]: Tales/NotExpressions.md
[`pipe`]: Tales/PipeExpressions.md
[`csharp`]: Tales/CSharpExpressions.md
[please read about the limitations]: Tales/CSharpExpressionLimitations.md
[`python`]: Tales/PythonExpressions.md
[`structure`]: Tales/StructureExpressions.md

## In XML documents, namespaces matter

The `metal` and `tal` attribute prefixes used throughout ZPT are defined with the following XML namespaces:

| Prefix    | Namespace                                 |
| ------    | ---------                                 |
| `metal`   | `http://xml.zope.org/namespaces/metal`    |
| `tal`     | `http://xml.zope.org/namespaces/tal`      |

When working with XML document templates, these two namespaces must be declared (typically at the root of the document) via `xmlns` attributes.
Strictly-speaking, for an XML document _the namespaces are all that matters_, you could conveivably use aliases which are not `tal` or `metal`, although for readability's sake this is not advised.

When using HTML documents _the namespaces need not be declared_; the `metal` and `tal` prefixes are recognised by their prefix names alone.

### XML namespaces example

```xml
<root xmlns:metal="http://xml.zope.org/namespaces/metal"
      xmlns:tal="http://xml.zope.org/namespaces/tal">
    <child tal:content="string:Some content">This content attribute will work OK.</child>
</root>
```

## Elements in the TAL or METAL namespace

A feature of the ZPT syntax is elements which use the `tal` or `metal` prefix (or, for XML documents, elements in the namespaces listed in the previous section).

Elements which are prefixed with either `tal` or `metal` may be 'invented' by the designer, the element name after the prefix is irrelevant.
Such elements are detected by ZptSharp during rendering and their start/end tags are [always omitted from the rendered output as if they had a `tal:omit-tag=""` attribute] present.
What's more, all attributes upon a TAL or METAL element are automatically assumed to be TAL or METAL attributes (accordingly).
This means that within such an element, the `tal:` or `metal:` prefixes are not required for attributes.

The use-case for TAL & METAL elements is to allow use of TAL & METAL attributes in positions _where no other markup element would semantically make sense_.
This scenario does not occur very often and so it is not expected that this technique will be frequently used.

[always omitted from the rendered output as if they had a `tal:omit-tag=""` attribute]: Tal/OmitTag.md

### TAL element example

Here is a somewhat contrived example.
For the purpose of this example, presume that we cannot alter the `<ul>` element and add attributes to it.
Perhaps the `<ul>` element is part of a macro which we do not wish to alter.

```html
<ul>
  <tal:defs define="items here/Items | here/OtherItems | here/EmptyArray">
    <li tal:repeat="item items" tal:content="item/Name">Item name</li>
  </tal:defs>
</ul>
```

In a real application, we could more likely add [the complex `tal:define` attribute] onto the `<li>` element.
Because define attributes are processed before content attributes we would get the same result without needing a TAL element.

[the complex `tal:define` attribute]: Tal/Define.md