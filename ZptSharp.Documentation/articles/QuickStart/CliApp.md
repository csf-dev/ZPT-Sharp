# Quick-start: ZptSharp command-line app

_This guide is for those who wish to use the ready-built command-line application to render ZPT templates using a model._

## Install prerequisites

To use the command-line ZptSharp application, you must have _either of the following_ environments installed:

* .NET Framework 4.6.1 or higher (and preferably, the NuGet command-line)
* .NET Core 2.0 or higher (or .NET 5+)

## Install ZptSharp NuGet package

With .NET Framework & NuGet, use this command.  If you do not have the NuGet command line utility then [you must download] and extract the package manually instead.

```text
nuget install -o packages ZptSharp.Cli
```

Alternatively, with .NET Core (or .NET 5+), use this command:

```text
dotnet tool install --global ZptSharp.Cli
```

[you must download]: https://www.nuget.org/packages/ZptSharp.Cli

## Create a JSON model file

The ZptSharp command-line app reads its model from a JSON file.
Create a text file at a path of your choice named `model.json`.
Its content should be as-follows:

```json
{"Message": "Hello world!"}
```

## Create a ZPT template

Create a new text file (again, at a path of your choice) named `quickstart.pt` to serve as our first ZPT document template.
This should have the following content.

```html
<html>
<head>
<title>ZptSharp 'Hello world' example</title>
</head>
<body>
<h1>Example command-line app usage</h1>
<p tal:content="here/Message">The greeting message appears here.</p>
</body>
</html>
```

## Use the command-line app

The command-line app is located in a directory within the package named `tools`, in a subdirectory named either `net461` or `netcoreapp2.0`.
These directories are for the .NET Framework and the .NET Core version of the app respectively.
To run the app, use the following command; on a .NET Core environment, prefix it with `dotnet` and a space.

```text
ZptSharp.Cli.exe -m path\to\model.json -o path\to\output -e html path\to\quickstart.pt
```

In this command, you should substitute:

* `path\to\model.json` with the path (relative or absolute) to the `model.json` file you created
* `path\to\quickstart.pt` with the path (relative or absolute) to the `quickstart.pt` file you created
* `path\to\output` with the path (relative or absolute) to a directory where you would like the output to be saved

You will see the rendered output saved as a file in the output directory as `quickstart.html`.

## Continue learning in the tutorial

You have now completed the quick-start for the command-line ZptSharp app!
To continue learning about ZptSharp, please follow [the ZptSharp tutorial], which teaches the ZPT language.
Throughout the tutorial, _changes to the model_ are performed in the `model.json` file.
Throughout the tutorial, _you will need to convert_ some C# constructs (to be added to the model) into JSON.

[the ZptSharp tutorial]: ../ZptTutorial/index.md
