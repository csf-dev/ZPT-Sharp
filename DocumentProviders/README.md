Each project in this directory is home to a different ZptSharp **document provider**. The main class/API of a document provider is an implementation of the interface `ZptSharp.Dom.IReadsAndWritesDocument`. 

Document providers are the mechanism by which ZptSharp reads/writes DOM documents. ZptSharp itself only deals with an abstraction (declared in the `ZptSharp.Dom` namespace.

Document providers are resolved by dependency injection and then registered for usage with `ZptSharp.Dom.IRegistersDocumentReaderWriter`.