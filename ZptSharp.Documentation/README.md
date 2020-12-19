# Documentation
This project directory is *not a direct part of ZptSharp*, nor does it build
any C# code.  It is a [DocFx API documentation] project.  This is used to
auto-generate an API documentation website using a combination of the source
code and markdown files within this project _(README files are excluded)_.

The generation process occurs automatically; it is a part of the overall build
process for the solution, triggered by `dotnet build`.  The output of the
generation process goes to the **`_site`** directory.

## Trying it out yourself
To see or preview the documentation on your own computer:

1. Build the solution with `dotnet build`
2. [Download the DocFx command-line tool] to your own environment
3. Run `docfx serve _site` from this directory

The documentation site will now be available from `http://localhost:8080/`
in your own environment.

## Reading/editing the docs
Markdown files in the [articles directory] are the source files for the corresponding
pages of the documentation website.

[DocFx API documentation]: https://dotnet.github.io/docfx/
[Download the DocFx command-line tool]: https://dotnet.github.io/docfx/tutorial/docfx_getting_started.html#2-use-docfx-as-a-command-line-tool
[articles directory]: articles/
