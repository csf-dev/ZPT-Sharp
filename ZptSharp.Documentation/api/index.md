# ZptSharp API reference

This guide takes you through the process of adding ZptSharp to your own application, consuming it as an API.

## Installing NuGet packages

The first step is to add references to the appropriate [ZptSharp NuGet packages] from your app.
The project/assembly which sets up your dependency injection must reference the package **ZptSharp**, as well as any document provider or expression evaluator packages you wish to use.
A working ZptSharp environment *requires at least one document provider*.

If your app makes use of ZptSharp from *a different project/assembly*, then that only need reference the **ZptSharp.Abstractions** package.
Once added to DI, ZptSharp may be consumed from only its interfaces.

## Adding ZptSharp to DI

The second step is to add ZptSharp to [your application's dependency injection] (the [`IServiceCollection`]). This should occur in your application's startup.
Ideally, if you are using [.NET Generic Host] then you will be using that framework to register and configure your services.
If using [OWIN] then you will have access to a startup class where services and middleware are configured.
Other frameworks will have their own equivalents to this process.

The addition to dependency injection begins with the use of [`AddZptSharp()`].
This returns a builder object which exposes further methods; some from the core and some made available by NuGet packages.
With these you should add *at least one expression evaluator* and *at least one document provider*.
Following is a typical example.

```csharp
using Microsoft.Extensions.DependencyInjection;
using ZptSharp;

void RegisterServices(IServiceCollection serviceCollection)
{
    serviceCollection
        .AddZptSharp()
        .AddStandardZptExpressions()
        .AddAngleSharpZptDocuments();
}
```

See the documentation for [the ZptSharp DI builder object] for a full list of the available extension methods.

### If you cannot use DI

It is strongly recommended to consume ZptSharp via dependency injection as described above.
If you cannot do this then it is possible to create/get an object which provides access to ZptSharp in a self-contained manner.
This is accomplished via the class [`ZptSharp.ZptSharpHost`].

The following example is equivalent to the dependency injection example above.
The mechanisms to add expression evaluators & document providers are also identical.
Note that there is no call to `AddZptSharp()`; this is implied.

```csharp
using ZptSharp;

var host = ZptSharpHost.GetHost(builder => 
    builder
        .AddStandardZptExpressions()
        .AddAngleSharpZptDocuments()
);
```

If using this mechanism instead of DI, then you are responsible for getting the returned host object to the class which will use it.

[a more in-depth writeup of the DI & setup functionality]: ./../articles/DiSetup.md
[ZptSharp NuGet packages]: ../articles/NuGetPackages.md
[your application's dependency injection]: https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection
[`IServiceCollection`]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection
[.NET Generic Host]: https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host
[OWIN]: https://docs.microsoft.com/en-us/aspnet/aspnet/overview/owin-and-katana/getting-started-with-owin-and-katana
[`AddZptSharp()`]: xref:ZptSharp.ZptSharpServiceCollectionExtensions.AddZptSharp(Microsoft.Extensions.DependencyInjection.IServiceCollection)
[the ZptSharp DI builder object]: xref:ZptSharp.Hosting.IBuildsHostingEnvironment
[`ZptSharp.ZptSharpHost`]: xref:ZptSharp.ZptSharpHost

## Injecting and consuming ZptSharp

Once ZptSharp is available via your application's dependency injection, you may inject & consume ZptSharp's service interfaces in your app logic.

The table below lists a few interfaces for ZptSharp services which are intended for use as entry-points into its logic.
[The self-contained host] which does not use DI also makes these same interfaces available via its properties.

| Interface                     | Summary                                                                               |
| ---------                     | -------                                                                               |
| [`IRendersZptFile`]           | Used to render ZPT documents from file-system files                                   |
| [`IRendersZptDocument`]       | Used to render ZPT documents from sources *which are not* file-system files           |
| [`IWritesStreamToTextWriter`] | Used to save the results of rendering to a `System.IO.TextWriter` where applicable    |
| [`IRendersManyFiles`]         | Used to perform bulk-rendering of many source files, using the same model for all     |

[The self-contained host]: xref:ZptSharp.Hosting.IHostsZptSharp
[`IRendersZptFile`]: xref:ZptSharp.IRendersZptFile
[`IRendersZptDocument`]: xref:ZptSharp.IRendersZptDocument
[`IWritesStreamToTextWriter`]: xref:ZptSharp.IWritesStreamToTextWriter
[`IRendersManyFiles`]: xref:ZptSharp.BulkRendering.IRendersManyFiles

## Configuring the rendering operations

The methods [`IRendersZptFile.RenderAsync`], [`IRendersZptDocument.RenderAsync`] & [`IRendersManyFiles.RenderAllAsync`] all accept an instance of [`RenderingConfig`], either as a parameter or as part of a request object.

You are advised to read the documentation for the rendering configuration, to see the available options which may be used to customize the process.

[`IRendersZptFile.RenderAsync`]: xref:ZptSharp.IRendersZptFile.RenderAsync(System.String,System.Object,ZptSharp.Config.RenderingConfig,System.Threading.CancellationToken)
[`IRendersZptDocument.RenderAsync`]: xref:ZptSharp.IRendersZptDocument.RenderAsync(System.IO.Stream,System.Object,ZptSharp.Config.RenderingConfig,System.Threading.CancellationToken,ZptSharp.Rendering.IDocumentSourceInfo)
[`IRendersManyFiles.RenderAllAsync`]: xref:ZptSharp.BulkRendering.IRendersManyFiles.RenderAllAsync(ZptSharp.BulkRendering.BulkRenderingRequest,System.Threading.CancellationToken)
[`RenderingConfig`]: xref:ZptSharp.Config.RenderingConfig