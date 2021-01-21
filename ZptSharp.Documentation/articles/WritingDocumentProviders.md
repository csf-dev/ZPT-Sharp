# Writing document providers

Document providers are classes which implement the interface [`IReadsAndWritesDocument`].
You are encouraged to read the API docs for this interface.
As well as implementing this interface, a document provider is almost certain to require its own implementations of the following:

* [`IDocument`]
* [`INode`]
* [`IAttribute`]

The abstractions package contains minimal abstract base classes for each of these interfaces, which cater for some of the boilerplate.
These are [`DocumentBase`], [`NodeBase`] & [`AttributeBase`] respectively.
You may use these base classes if desired, but their use is optional.

[`IReadsAndWritesDocument`]: xref:ZptSharp.Dom.IReadsAndWritesDocument
[`IDocument`]: xref:ZptSharp.Dom.IDocument
[`INode`]: xref:ZptSharp.Dom.INode
[`IAttribute`]: xref:ZptSharp.Dom.IAttribute
[`DocumentBase`]: xref:ZptSharp.Dom.DocumentBase
[`NodeBase`]: xref:ZptSharp.Dom.NodeBase
[`AttributeBase`]: xref:ZptSharp.Dom.AttributeBase

## Registering/activating a document provider

In order to activate a document provider or must be added to dependency injection during start-up and it must be registered with the list of available document provider types.

Typically this is all accomplished with a single extension method for the [`IBuildsHostingEnvironment`] interface.
Here is the typical syntax for registering a document provider type:

```csharp
builder.ServiceRegistry.DocumentProviderTypes
  .Add(typeof(MyDocumentProvider));
```

Where:

* `builder` is the `IBuildsHostingEnvironment`
* `MyDocumentProvider` is the concrete class which implements `IReadsAndWritesDocument`


[`IBuildsHostingEnvironment`]: xref:ZptSharp.Hosting.IBuildsHostingEnvironment