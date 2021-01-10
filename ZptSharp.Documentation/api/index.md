# ZptSharp API reference

## Adding ZptSharp to your application

## The interfaces to inject

Once ZptSharp is configured in your application's dependency injection, consuming it is a simple matter of injecting the appropriate interfaces and using them in your own classes.  For example:

```csharp
public class MyClassWhichUsesZptSharp
{
    readonly ZptSharp.IRendersZptFile fileRenderer;

    public async Task RenderTheFile()
    {
        // Use fileRenderer.RenderAsync here
    }

    public MyClassWhichUsesZptSharp(ZptSharp.IRendersZptFile fileRenderer)
    {
        this.fileRenderer = fileRenderer ?? throw new ArgumentNullException(nameof(fileRenderer));
    }
}
```

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
