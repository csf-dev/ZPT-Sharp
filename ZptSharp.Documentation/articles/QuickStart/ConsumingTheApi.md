# Quick-start: Consuming the ZptSharp API

_This guide is for those who wish to use ZptSharp within their own application._
The guide assumes that you are .NET Core (or .NET 5+); the steps are adaptable for an app written with .NET Framework.

## Create a project to consume the API

In this tutorial, we will be creating a command-line app project.
This makes it easy to run & test without any other assumptions about your application.
Please adapt these instructions to your own intended usage, using information from the more detailled [reference documentation].
To begin with a new/empty project, use the following command:

```text
dotnet new console -o ZptSharpConsumer
```

[reference documentation]: ../../api/index.md

## Install ZptSharp packages

To install a minimal set of ZptSharp packages, use the following commands:

```text
dotnet add ZptSharpConsumer\ZptSharpConsumer.csproj package ZptSharp
dotnet add ZptSharpConsumer\ZptSharpConsumer.csproj package ZptSharp.HtmlAgilityPack
```

## Set up Generic Host

[.NET Generic Host] is an easy way to set up/scaffold an application, we will be using that here.
Install the generic host package using the following command:

```text
dotnet add ZptSharpConsumer\ZptSharpConsumer.csproj package  Microsoft.Extensions.Hosting
```

Now, we add the basic structure of generic host to the app.
Open and modify the `Program.cs` file in the new project, so that the class now looks like the following (the code is [based upon this example]):

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services
                    .AddZptSharp()
                    .AddStandardZptExpressions()
                    .AddHapZptDocuments();
            });
}
```

[.NET Generic Host]: https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host
[based upon this example]: https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host#set-up-a-host

## Create a class to consume ZptSharp

Create a new class named `Application` - this class should look like the following:

```csharp
public class Application : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("The app is starting.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
```

This class needs to be added to the Generic Host builder too, so that it is started-up.
Add the following to the `ConfigureServices` callback within `Program.cs`.
It may go either before or after the `AddZptSharp` line, it does not matter.

```csharp
services.AddHostedService<Application>();
```

You may run the app if you wish, at this point, using the following command.
You should see the message "The app is starting." logged to the console.

```text
dotnet run --project ZptSharpConsumer
```

## Create a document template file

Create a new text file in a path of your choosing, named `template.pt` and set its content to the following:

```html
<html>
<head>
<title>ZptSharp 'Hello world' example</title>
</head>
<body>
<h1>Example API usage</h1>
<p tal:content="here/Message">The greeting message appears here.</p>
</body>
</html>
```

## Add code to render the document using a model

We are now ready to add the logic which shall use ZptSharp to render our HTML.
For simplicity, we will output to the console.

In the `Application` class, we shall make use of the dependency injection set up within Generic Host and constructor-inject two ZptSharp services.
These services are `IRendersZptFile` & `IWritesStreamToTextWriter`.
Constructor injection is a fairly common pattern, but in case you are unfamiliar, refer to this code sample below _(only the constructor and two new fields are shown, the rest of the class is omitted)_.

```csharp
readonly IRendersZptFile fileRenderer;
readonly IWritesStreamToTextWriter streamCopier;

public Application(IRendersZptFile fileRenderer, IWritesStreamToTextWriter streamCopier)
{
    this.fileRenderer = fileRenderer ?? throw new ArgumentNullException(nameof(fileRenderer));
    this.streamCopier = streamCopier ?? throw new ArgumentNullException(nameof(streamCopier));
}
```

Now, we may write the real content of the `StartAsync` method:

```csharp
public async Task StartAsync(CancellationToken cancellationToken)
{
    var stream = await fileRenderer.RenderAsync(@"path\to\template.pt",
                                                new { Message = "Hello World!" });
    await streamCopier.WriteToTextWriterAsync(stream, Console.Out);
}
```

In this sample, replace `"path\to\template.pt"` with the actual file path to the `template.pt` file.

## Try it out

Run the application using:

```text
dotnet run --project ZptSharpConsumer
```

You are expecting to see some HTML output, showing the rendered template, with values substituted using the model.

## Continue learning in the tutorial

You have now completed the quick-start for consuming the ZptSharp API!
To continue learning about ZptSharp, please follow [the ZptSharp tutorial], which teaches the ZPT language.
Throughout the tutorial, _changes to the model_ are performed by altering the second parameter passed to `RenderAsync`.
If this becomes cumbersome, feel free to separate the creation of the model into a separate private method.

[the ZptSharp tutorial]: ../ZptTutorial/index.md


