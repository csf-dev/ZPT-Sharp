# The ZptSharp command-line renderer

In [the NuGet package ZptSharp.Cli] is an executable tool for rendering ZPT document template source files using a model from a JSON file.
The tool may render a single template source file or batches of files within a specified directory structure.

## Compatibility and file path

The ZptSharp commmand line renderer package requires _either_ a minimum of **.NET Framework 4.6.1** or a minimum of **.NET Core 3.1** to be installed.
**.NET 5** is backwards-compatible with .NET Core 3.1 and so this is also OK.

Within the NuGet package, use the tool from the appropriate path listed in this table.

| Path                                      | Framework                 |
| ----                                      | ---------                 |
| `tools\net461\ZptSharp.Cli.exe`           | .NET Framework            |
| `tools\netcoreapp3.1\ZptSharp.Cli.exe`    | .NET Core (or .NET 5+)    |

[the NuGet package ZptSharp.Cli]: NuGetPackages.md#usage-specific-packages

## Usage

The basic syntax for the application is:

```text
ZptSharp.Cli.exe [options] [input path]
```

The **input path** should indicate the path to either a document template file or to a directory containing document template files.
In the case of a directory, that directory is searched for files.
This search is not recursive by default, so files in subdirectories will not be used.
This directory searching may be influenced by the `--include` & `--exclude` options described below.

The file or files indicated by the input path are the document template source files which will be rendered by the application.

Input files must have a compatible file extension.
This is required so that the application can determine whether to treat them as HTML files or as XML files.
The permitted extensions are shown in the following table.

| Extension | Type  |
| --------- | ----  |
| `.pt`     | HTML  |
| `.htm`    | HTML  |
| `.html`   | HTML  |
| `.xml`    | XML   |
| `.xhtml`  | XML   |

## Options

All of the fillowing options have both a long version which uses the double-hyphen-dash prefix (such as `--include`) and a short version which uses a single-hyphen-dash prefix (such as `-i`).
The long and short versions are equivalent and may be used interchangeably.

### `--include`, `-i`

This option accepts a comma-separated list of [file glob patterns] which indicates the patterns for document template files.
_This option is meaningless and ignored if the **input path** is a single file._

If the input path is a directory and this option is present then only files within the directory which match one of the include patterns will be processed.
**Recursive** directory searching may be enabled by using a pattern such as `**\*.*`.
If this option is not present then the application will treat it as if it had been `*.*`.

_It is advised to quote the value to this parameter_.
Some shells, such as Bash (common on GNU/Linux), will interpret glob patterns before passing them to the application.
This could mean that the application does not receive the options which were intended.
Surrounding the option value in quote symbols will protect it from this process.

[file glob patterns]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.filesystemglobbing.matcher?view=dotnet-plat-ext-3.1#remarks

### `--exclude`, `-x`

This option accepts a comma-separated list of [file glob patterns] which indicates file patterns to exclude/ignore from processing.
_This option is meaningless and ignored if the **input path** is a single file._

If this is present then files matching any of the specified patterns will not be processed.
If this option is not present then no files will be excluded (provided they match an `--include` pattern, if specified).

If both `--include` and `--exclude` options are specified then source file _must match both rules_ in order to be processed.

_It is advised to quote the value to this parameter_.
Some shells, such as Bash (common on GNU/Linux), will interpret glob patterns before passing them to the application.
This could mean that the application does not receive the options which were intended.
Surrounding the option value in quote symbols will protect it from this process.
### `--model`, `-m`

This option provides the file path to a file containing a JSON model to use in the rendering process.
If not present then [the `here` root context] for the rendering operation will be null.

[the `here` root context]: ZptReference/Tales/GlobalContexts.md#here

### `--keywords`, `-k`

This option provides a list of key/value pairs which will be made available to the rendering process via [the `options` root context].
These are the 'keyword options' which may be used to pass additional data directly from the command-line, aside from the model.

The format for the option values is `[key]=[value]`, with multiple key/value pairs separated by commas.

[the `options` root context]: ZptReference/Tales/GlobalContexts.md#options

### `--extension`, `-e`

This option selects the file extension which will be given to the output files.
By default, the output files will be given the exact same filename as their corresponding input file.
To use a different extension, use this option.

### `--output`, `-o`

This option chooses an output path for the rendered files.
By default, the application outputs to the current working directory.

_Beware when the source files are in the current working directory_, the default behaviour of the app will cause the output to overwrite the input, possibly damaging your source files.
Use either this option or `--extension` to specify either a different path for the output or at least a different file extension.

When rendered files are written to the output directory, they will have the maintain the same relative directory structure as they were found relative to the **input path**.
This is particularly relevant if the `--include` option has been used to enable recursive directory scanning.

### `--anglesharp`, `-s`

By default, when processing HTML source templates, [the HTML Agility Pack document provider] is used.
The tool package also includes support for using [the AngleSharp document provider] instead.
The presence of this option switches the application to use AngleSharp instead of HTML Agility Pack.

This option has no value, if present then it is enabled.

[the HTML Agility Pack document provider]: xref:ZptSharp.Dom.HapDocumentProvider
[the AngleSharp document provider]: xref:ZptSharp.Dom.AngleSharpDocumentProvider

### `--annotate`, `-a`

If present, then this option [enables source annotation] within the rendered output.
Source annotation is a diagnostic and debugging aid for developers to understand how a rendered file was put together.
Source annotation is disabled by default but may be enabled by this option.

This option has no value, if present then it is enabled.

[enables source annotation]: xref:ZptSharp.Config.RenderingConfig.IncludeSourceAnnotation

## Examples

### Render a single file with a model

This would render a single file named `template.pt` in the current directory using a model file named `model.json`.
The rendering output will be saved as `template.html` in the current directory.

```text
ZptSharp.Cli.exe -e html -m model.json template.pt
```

### Render all files in a directory recursively

This would recursively search the directory `c:\my_source_files` for all template files and render every one of them using the model from `model.json`.
The rendered output will be saved in the directory `c:\my_output_files`, using the same file relative directory structure and file names as within `c:\my_source_files`.

```text
ZptSharp.Cli.exe -m model.json -i "**\*.*" -o c:\my_output_files c:\my_source_files
```
