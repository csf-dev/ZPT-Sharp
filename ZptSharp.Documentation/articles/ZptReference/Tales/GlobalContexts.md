---
uid: GlobalContextsArticle
---

# The TALES global root contexts

TALES root contexts could be thought of as pre-defined variables which are available to all templates.
They may be used in expressions and have the values/meanings as described below.
Please note that - with the exception of `CONTEXTS` - _none of these names are reserved words_.
You may declare your own variables in templates using these names and it will have the usual effect of 'hiding' the original definition.
See the information about the `CONTEXTS` keyword to access these pre-defined meanings, if a variable defined in a template has hidden the original meaning.

## How these variables are provided

These variables are made available by a combination of **configurable logic**.
This means that the actual list of variables which are available to your template might differ if that configuration has anything other than the default values.
In some cases that might mean that you have a superset of the list in [the following section], and in other cases it might mean that some/all of the variables are not present.
This is the logic which leads to the provision of these variables.

1. A **[root contexts provider]** is used to get the baseline set of root contexts (variables).
    * [The default implementation of this service] provides the variables listed in the section below.
    * If the [`RootContextsProvider`] property of [the rendering configuration] has been set using a non-default value then that alternative provider will be used instead.
    This could lead to a very different set of variables being made available.
    * _No logic in any of the production ZptSharp packages replaces this configuration property_. It is provided entirely as an extension point for other developers.
2. The **[context builder function]** is then executed in order to supplement the root contexts from the previous step with additional variables.
    * The default function defined upon [the rendering configuration] does nothing, and leaves the root contexts unchanged.
    * [The two MVC View Engine packages] make use of this technique in order to add [the additional contexts which are available to MVC document templates].
3. _Presuming step 1, above, has not been overridden_, the [`options`] root context is populated by [the default root contexts provider], using [the `KeywordOptions` configuration property].
    * If a different implementation was provided in step 1, then it is up to the replacement root contexts provider to populate the `options` root context (if it even provides that variable).

[the following section]: GlobalContexts.md#the-list-of-variables
[root contexts provider]: xref:ZptSharp.Expressions.IGetsDictionaryOfNamedTalesValues
[The default implementation of this service]: xref:ZptSharp.Expressions.BuiltinContextsProvider
[`RootContextsProvider`]: xref:ZptSharp.Config.RenderingConfig.RootContextsProvider
[the rendering configuration]: xref:ZptSharp.Config.RenderingConfig
[context builder function]: xref:ZptSharp.Config.RenderingConfig.ContextBuilder
[The two MVC View Engine packages]: ../../NuGetPackages.md#usage-specific-packages
[the additional contexts which are available to MVC document templates]: ../../ViewEngines.md#added-tales-contextsvariables-for-mvc
[`options`]: GlobalContexts.md#options
[the default root contexts provider]: xref:ZptSharp.Expressions.BuiltinContextsProvider
[the `KeywordOptions` configuration property]: xref:ZptSharp.Config.RenderingConfig.KeywordOptions

## The list of variables

### `options`

This root context makes available a collection of **keyword options**, which is a name/value collection of `string` names and `object` values.
The precise meaning/semantics of keyword options is left deliberately vague by the ZPT specification.
In practice this root context is very rarely used in favour of more suitable mechanisms which integrate into various frameworks.
For example, in an MVC web application it is generally _more suitable to use the view bag_ to hold arbitrary data which is made avilable to views.

The keyword options themselves (if you wish to use them) are set up by [the rendering configuration], using [the `KeywordOptions` configuration property].

### `repeat`

This root context provides access to a collection of [the repeat variables] which are currently in-scope.
Each named repeat variable is an item in this collection, exposing all of the standard properties of a repeat variable.

[the repeat variables]: ../Tal/Repeat.md#repeat-variables

### `here`

The `here` root context provides access to the model.
In MVC web applications using the ZptSharp view engines, the root context `Model` is an alias for `here`.

### `nothing`

This root context provides a non-object.  In .NET applications this means a `null` reference.

### `default`

This root context provides a singleton instance of [the abort-action token].
The abort-action token is a special object which is interpreted by a number of TAL attributes to mean "do nothing".
Please refer to [the individual TAL attributes] for their precise behaviour when they act upon an expression result which is the abort-action token.

The general rule-of-thumb for TAL attributes operating upon an abort-action token means that the attribute is processed with the same outcome as if it were not present.

[the abort-action token]: xref:ZptSharp.Expressions.AbortZptActionToken
[the individual TAL attributes]: ../Index.md#tal-binds-data-to-the-template

### `attrs`

The `attrs` root context provides access to _the attribute values_ for the current element.
The format of this object is a name/value collection of `string` attribute names and `string` attribute values.

[a `tal:attributes` attribute]: ../Tal/Attributes.md

### `template`

This root context provides access to an object which represents the current document template.
The object returned by this context provides two values:

* The `macros` present upon the document: A name/value collection of `string` macro names and [`MetalMacro`] macros
* The `sourcename` for the document: A `string` name for the document template; typically its file path.

[`MetalMacro`]: xref:ZptSharp.Metal.MetalMacro

### `container`

The `container` root context is only available when [the source of the current document template] is a source which may contain other templates.
It is primarily used by METAL attributes to find & reference other template documents.
In practice, for document templates rendered from files on disk, this is the case; the container is the file system directory that contains the current template.

In other cases (if the template came from a different source) then there may or may not be a logical 'container' available.
A container is available if the document's source implements [the `IHasContainer` interface].
If the source does not provide a container, then the `container` root context/variable will return `null`.

In the most common usage of ZptSharp (template documents rendered from files), this `container` root context may be used to navigate to other template documents relative to the current document.
The `container` variable will provide a reference to the directory which contains the current template.

_If you are using a ZptSharp MVC view engine_ then you may wish to use [the `Views` root context] instead.

[the source of the current document template]: xref:ZptSharp.Rendering.IDocumentSourceInfo
[the `IHasContainer` interface]: xref:ZptSharp.Rendering.IHasContainer
[the `Views` root context]: ../../ViewEngines.md#added-tales-contextsvariables-for-mvc

### `error`

If ZptSharp is currently processing [a `tal:on-error` attribute] then this root context provides access to the current error object.
This should always be an object which derives from `System.Exception` when it is present.

If there is no current error (ZptSharp is not handling an error) then this variable will be `null`.

[a `tal:on-error` attribute]: ../Tal/OnError.md

## The special root context `CONTEXTS`

As noted at the beginning of this page, none of the names of root contexts/variables listed in the previous section are reserved words.
This means that any of these variables may be overridden/hidden by variable definitions in your templates.
The special root context name `CONTEXTS` is a reserved word however and _no variable may be manually-defined with this name_.

The `CONTEXTS` root context serves as a container for all of the other variables available in the root context.
This means that any other root context may be used unambiguously, even if it has been hidden by another variable definition of the same name, by accessing it from the `CONTEXTS` root context.

For example, even if another variable named `default` has been defined within the template, the TALES path expression `CONTEXTS/default` will provide access to the abort-action token.