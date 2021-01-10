# ZptSharp API reference

This guide takes you through the process of adding ZptSharp to your own application, consuming it as an API.

## Adding ZptSharp to your application

Firstly, you must install the appropriate [ZptSharp NuGet packages] to your application.
In addition to the summary below, there is also [a more in-depth writeup of the DI & setup functionality] available to read.

### Adding ZptSharp to dependency injection

The best way to consume ZptSharp is *to include it within [your application's dependency injection]*.
In your startup logic, add ZptSharp to the [`IServiceCollection`] from which you are building your application.
Ideally, if you are using [.NET Generic Host] then you will be using that framework to register and configure your services.
If using [OWIN] then you will have access to a startup class where services are registered as middleware.
Other frameworks will have their own equivalents to this process.

```csharp
using Microsoft.Extensions.DependencyInjection;
using ZptSharp;

void RegisterServices(IServiceCollection serviceCollection)
{
    // This example uses the AngleSharp document provider; you
    // may register any you like but register AT LEAST ONE.
    // 
    // This is also where you would register extra
    // expression evaluators if you wish.
    serviceCollection
        .AddZptSharp()
        .AddAngleSharpZptDocuments();
}
```

### One-time services setup

Once the ZptSharp services are registered with dependency injection, the `IServiceCollection` will be used to create an [`IServiceProvider`].
This service provider needs a small amount of one-time set-up for each document provider/expression evaluator you wish to use.

```csharp
using System;
using ZptSharp;

void SetupZptServices(IServiceProvider provider)
{
    provider
        .UseStandardZptExpressions()
        .UseAngleSharpZptDocuments();
}
```

### If you cannot use dependency injection

There is an alternative to registering ZptSharp with your application's dependency injection; that is [to use the ZptSharp.Hosting NuGet package].
This allows creation of a self-hosting object exposing the ZptSharp services, without needing to wire it up into a wider environment.

[a more in-depth writeup of the DI & setup functionality]: ./../articles/DiSetup.md
[ZptSharp NuGet packages]: ../articles/NuGetPackages.md
[your application's dependency injection]: https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection
[`IServiceCollection`]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection
[.NET Generic Host]: https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host
[OWIN]: https://docs.microsoft.com/en-us/aspnet/aspnet/overview/owin-and-katana/getting-started-with-owin-and-katana
[`IServiceProvider`]: https://docs.microsoft.com/en-us/dotnet/api/system.iserviceprovider
[to use the ZptSharp.Hosting NuGet package]: ./../articles/SelfHosting.md

## Injecting and consuming ZptSharp

Once ZptSharp is available via your application's dependency injection, you may inject & consume ZptSharp's service interfaces in your app logic.
The recommended interfaces to inject and consume are:

| Interface                     | Summary                                                                               |
| ---------                     | -------                                                                               |
| [`IRendersZptFile`]           | Used to render ZPT documents from file-system files                                   |
| [`IRendersZptDocument`]       | Used to render ZPT documents from sources *which are not* file-system files           |
| [`IWritesStreamToTextWriter`] | Used to save the results of rendering to a `System.IO.TextWriter` where applicable    |
| [`IRendersManyFiles`]         | Used to perform bulk-rendering of many source files, using the same model for all     |

[`IRendersZptFile`]: xref:ZptSharp.IRendersZptFile
[`IRendersZptDocument`]: xref:ZptSharp.IRendersZptDocument
[`IWritesStreamToTextWriter`]: xref:ZptSharp.IWritesStreamToTextWriter
[`IRendersManyFiles`]: xref:ZptSharp.BulkRendering.IRendersManyFiles

## Configuring the rendering operations

The methods [`IRendersZptFile.RenderAsync`], [`IRendersZptDocument.RenderAsync`] & [`IRendersManyFiles.RenderAllAsync`] all accept an instance of [`RenderingConfig`], either as a parameter or as part of a request object.

You are advised to read the documentation for the rendering configuration object to understand how it impacts the rendering process.

[`IRendersZptFile.RenderAsync`]: xref:ZptSharp.IRendersZptFile.RenderAsync(System.String,System.Object,ZptSharp.Config.RenderingConfig,System.Threading.CancellationToken)
[`IRendersZptDocument.RenderAsync`]: xref:ZptSharp.IRendersZptDocument.RenderAsync(System.IO.Stream,System.Object,ZptSharp.Config.RenderingConfig,System.Threading.CancellationToken,ZptSharp.Rendering.IDocumentSourceInfo)
[`IRendersManyFiles.RenderAllAsync`]: xref:ZptSharp.BulkRendering.IRendersManyFiles.RenderAllAsync(ZptSharp.BulkRendering.BulkRenderingRequest,System.Threading.CancellationToken)
[`RenderingConfig`]: xref:ZptSharp.Config.RenderingConfig