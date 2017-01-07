# Building ZPT-Sharp from source
ZPT-Sharp builds using the standard .NET MSBuild system, thus you will be performing builds using one of either:

* The **`msbuild`** application (Windows/.NET)
* The **`xbuild`** application (Mono framework)

The actual build commands are summarised towards the end of this file, along with where to find the built output. What follows is information about **build dependencies** and the available **build configurations**, which you should read before attempting the build.

## Dependencies
The build dependencies are a list of software applications/packages which you must have installed in order to successfully build the solution from source.  For ZPT-Sharp the dependencies depend upon the build configuration selected. See **Build configurations** below for more information.

*All of these dependencies must be available in your `PATH` environment variable before attempting a build.*  On GNU/Linux systems using packages installed by a package manager this is typically not an issue.  On MS Windows systems, you may need to issue a series of `SET PATH` commands before attempting the build.  *A warning to those using a Cygwin Bash shell and MSBuild -* MSBuild will invoke `cmd.exe` directly and will not perform the build using your Cygwin Bash shell.  Applications available in the Cygwin PATH environment might not be available in cmd.exe's PATH environment.

### Dependencies for Debug/Release builds
Builds using either of the configurations `Debug` or `Release` have only a small number of dependencies, which are likely to be found on any .NET developer's build environment:

* Either the .NET framework (version 4.5 or higher) or an equivalent Mono framework
 * At this time, *build & deployment has not been tested* using [DotNet Core].
* Either MSBuild or Mono XBuild
* The Nuget package manager

[DotNet Core]: https://www.microsoft.com/net/core

### Dependencies for Deploy builds
Builds using the `Deploy` configuration have the following additional dependencies.

* [Man (link to Wikipedia page)]
* [Man2html]
* [Tidy]
* [Doxygen]
* Pdflatex - a part of [the Latex suite]

All of the above dependencies are Free, Open Source Software.

[Man (link to Wikipedia page)]: https://en.wikipedia.org/wiki/Man_page
[Man2html]: http://www.nongnu.org/man2html/
[Tidy]: http://www.html-tidy.org/
[Doxygen]: http://www.stack.nl/~dimitri/doxygen/
[the Latex suite]: https://www.latex-project.org/

### Getting these dependencies on GNU/Linux
Installing the above dependencies on a GNU/Linux system is very simple - they are either likely to already be installed or they will be available from your distribution's package manager.  For example, on Debian-based systems including all flavours of Ubuntu, you may use the following.  This command must be executed as root, using `sudo` or `su` as applicable.

```bash
apt-get install man man2html tidy doxygen texlive-latex-base
```

### Getting these dependencies on MS Windows
On Windows, you will mainly need to install these dependencies manually.  Some are available via [the Cygwin application suite], which comes with a GUI package manager for downloading/installing software. *Please remember that these only only required for builds using the `Deploy` configuration.*

[the Cygwin application suite]: https://www.cygwin.com

* The `man` application is available from The Cygwin application suite
* The `man2html` application is available from the [MinGW application suite]
* The `tidy` application is available from either:
 * [The official HTML Tidy website]
 * The Cygwin application suite
* The `doxygen` application is available from either:
 * [The official Doxygen website]
 * The Cygwin application suite
* The `pdflatex` application is available as a part of [the MiKTEX distribution of the Latex suite for MS Windows]

[MinGW application suite]: http://www.mingw.org/
[The official HTML Tidy website]: http://www.html-tidy.org/
[The official Doxygen website]: http://www.stack.nl/~dimitri/doxygen/download.html
[the MiKTEX distribution of the Latex suite for MS Windows]: http://miktex.org/

## Build configurations
There are three available build configurations: `Debug`, `Release` and `Deploy`.

### Debug builds
The debug build configuration builds only the core assemblies. It does not build documentation or any packages.

In a debug build, assemblies are compiled with *full debugging symbols* (line numbers and file names in stack traces).

Assemblies are compiled with the constants `TRACE` and `DEBUG` defined. This will send additional logging messages to the `TraceSource`. If a `TraceListener` is configured and captures messages at the `Verbose` level then a lot of log information will be recorded, including TALES expression evaluation.

### Release builds
The release configuration builds only the core assemblies. It does not build documentation or any packages.

In a release build, assemblies are compiled *without debugging symbols*.

Assemblies are compiled with only the `TRACE` constant defined. This means that only the standard information will be written to the `TraceSource`. The verbose logging present in `Debug` builds *will not be available*, regardless of the settings of trace sources/switches/listeners.

### Deploy builds
Deploy builds compile the assemblies in exactly the same manner as `Release` builds. This includes the lack of debugging symbols and the compiler constants defined.

The difference between the `Deploy` and `Release` configurations is that builds using the `Deploy` configuration additionally build other content which both `Debug` and `Release` builds skip. Please note that because of this, there are a number of additional build dependencies for this build configuration.  See **Dependencies** above for more information.

This other content includes the following documentation:

* The docs for the `ZptBuilder.exe` application
 * These are built in both plain-text and HTML formats
 * They are exported from the groff-formatted manpage `ZptBuilder.1`
* The API documentation
 * These are built in both HTML and PDF formats
 * This is exported from the source code comments, using Doxygen
* The documentation website

Builds using the deploy configuration also create the following packages:

* Redistributable zip archives, which include:
 * A package for the core API only
 * A package for the MVC view engine
 * A package for ZptBuilder
 * An extra 'complete' package, which includes everything
* The various Nuget packages which make up ZPT-Sharp

### Why three build configurations and not two
A core design principle (and a reason for having an extra `Deploy` configuration) is so that `Debug` and `Release` builds *do not need any dependencies beyond the following*:

* Either the .NET or Mono framework
* Either MSBuild or xbuild
* Nuget
 * This includes dependency packages managed by Nuget, which would be retrieved and properly configured via `nuget restore`

The rationale for this is to facilitate building of the core assemblies using a 'vanilla' .NET development environment. Such an environment might only have the above software available. We should not be demanding the installation of extra software when we only need it for building non-essential output.

If the developer wishes to build the non-essential components of the solution, then they may specify a `Deploy` build configuration, and must install the extra dependencies accordingly.

## Summary of the build process
Once you have verified that all of the required dependencies are present, and you have selected a build configuration appropriate to your intentions, you are ready to attempt a build.

Once any required configuration of your `PATH` environment variable (to make the dependencies available) is complete, the procedure is similar on either GNU/Linux or MS Windows:

```
nuget restore
msbuild "/p:Configuration=Release"
```

Substitute the `msbuild` application for `xbuild` if appropriate; the parameters taken by either application are passed in the same format/syntax.
Substitute `Release` for `Debug` or `Deploy` if you wish to use one of those other build configurations.
If the build configuration parameter is omitted (IE: you just execute MSBuild/XBuild with no parameters) then a `Debug` build will be performed.

### Parameters for Deploy builds
Optionally, for `Deploy` builds only, you may specify the build property `WebsiteUrlRoot` with a value.
By default this property defaults to the URL of the ZPT documentation website.
Sspecifying a local value will permit you to host and use the documentation website locally.
This is performed as follows:

```
msbuild "/p:Configuration=Deploy;WebsiteUrlRoot=/my/local/path/"
```

## Where to find the build output
In a `Debug` or `Release` build, the build output will be available in each of the various project directories, in a directory named `bin`, and a subdirectory named after the build configuration used (the usual location for .NET software).

In a `Deploy` build, the additional built output will be found in the `Deployment` solution directory, in a subdirectory named `Output`.
