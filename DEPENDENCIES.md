# ZPT-Sharp packaging dependencies
This page documents the various dependency software packages required by ZPT-Sharp.
*This information is for future packaging purposes only.*
If you are looking for the **[build dependencies]** (outside of packaging for GNU/Linux) then you would do better to use the linked documentation.

[build dependencies]: https://github.com/csf-dev/ZPT-Sharp/blob/master/BUILDING.md

It is an intention to make ZPT-Sharp available to GNU/Linux distributions, such as Debian.
In order to do this though, I am not permitted to rely on packages from non-DFSG sources such as Nuget.
I would need to ensure that all dependencies are themselves packaged first.

This text file serves as a list of those dependencies which must be packaged (or depended-upon) in order to comply with (at first attempt) the DFSG.
I am only targetting Debian first because:
* It is the distro with which I have the most experience.
* It has a wide coverage, given that many other distros are based upon it.

Others are welcome to package it for other distros as they please.

## Required for core compilation
These software packages are required for the basic compilation of the software.
* Either the .NET framework (version 4.6 or higher) or an equivalent Mono framework
* Either MSBuild 2015+ or Mono XBuild
* The Nuget package manager

## Required for deployment
These software packages are only required to create 'deployment builds' of the software.
They might not be needed in order to package the software for GNU/Linux (for example).
* Man
* Man2html
* Tidy
* Doxygen
* Pdflatex (part of the Latex suite)

## Required Nuget packages
The following nuget packages are required for ZPT-Sharp:

### Required by all
Nothing will build without this.
* MSBuild.Extension.Pack, version 1.8.0
* CSF.Caches, version 1.0.0
* CSF.Configuration, version 1.0.0
* CSF.Reflection, version 1.0.0
* CSF.Utils, version 6.0.0

### Required for unit tests
Only needed for unit tests, might not be relevant for GNU/Linux packaging.
* AutoFixture, version 3.34.2
* Moq, version 4.2.1507.0118
* NUnit, version 2.6.4

### Required for the MVC view engine
* Microsoft.AspNet.Mvc, version 5.2.3
* Microsoft.AspNet.Razor, version 3.2.3
* Microsoft.AspNet.WebPages, version 3.2.3
* Microsoft.Web.Infrastructure, version 1.0.0.0

### Required for HTML HAP documents
* HtmlAgilityPack, version 1.4.9

### Required for Python expressions
* IronPython, 2.7.5

### Required for ZptBuilder
* CSF.Cli.Parameters, version 1.0.0

### Required for deployment
This might not be relevant for GNU/Linux packaging.
* CSF.IniDocuments, version 6.0.0