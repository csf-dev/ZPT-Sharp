================================================================================
                          ZPT-Sharp ZIP distribution
================================================================================

===============
README contents
===============

1. What is ZPT-Sharp
2. Contents of the ZIP file
3. What you need to install

=================
What is ZPT-Sharp
=================

ZPT-Sharp is a template mechanism for dynamic creation of HTML/XML documents.
It may be used as a ViewEngine for ASP.NET MVC, it may be used as a standalone
application (ZptBuilder.exe) or it may be consumed as an API by your own
applications.

See http://csf-dev.github.io/ZPT-Sharp/ for complete documentation and reference
material.

========================
Contents of the ZIP file
========================

* CSF.Caches.dll
* CSF.Cli.Parameters.dll
* CSF.Configuration.dll
* CSF.Reflection.dll
* CSF.Utils.dll
* CSF.Zpt.Abstractions.dll
* CSF.Zpt.Abstractions.xml
* CSF.Zpt.dll
* CSF.Zpt.DocumentProviders.HtmlHAP.dll
* CSF.Zpt.DocumentProviders.HtmlHAP.xml
* CSF.Zpt.DocumentProviders.XmlLegacy.dll
* CSF.Zpt.DocumentProviders.XmlLegacy.xml
* CSF.Zpt.DocumentProviders.XmlLinq.dll
* CSF.Zpt.DocumentProviders.XmlLinq.xml
* CSF.Zpt.ExpressionEvaluators.CSharpExpressions.dll
* CSF.Zpt.ExpressionEvaluators.CSharpExpressions.xml
* CSF.Zpt.ExpressionEvaluators.CSharpFramework.dll
* CSF.Zpt.ExpressionEvaluators.CSharpFramework.xml
* CSF.Zpt.ExpressionEvaluators.LoadExpressions.dll
* CSF.Zpt.ExpressionEvaluators.LoadExpressions.xml
* CSF.Zpt.ExpressionEvaluators.NotExpressions.dll
* CSF.Zpt.ExpressionEvaluators.NotExpressions.xml
* CSF.Zpt.ExpressionEvaluators.PathExpressions.dll
* CSF.Zpt.ExpressionEvaluators.PathExpressions.xml
* CSF.Zpt.ExpressionEvaluators.PythonExpressions.dll
* CSF.Zpt.ExpressionEvaluators.PythonExpressions.xml
* CSF.Zpt.ExpressionEvaluators.StringExpressions.dll
* CSF.Zpt.ExpressionEvaluators.StringExpressions.xml
* CSF.Zpt.Log4net.dll
* CSF.Zpt.MVC5.dll
* CSF.Zpt.xml
* HtmlAgilityPack.dll
* IronPython.dll
* IronPython.Modules.dll
* IronPython.SQLite.dll
* IronPython.Wpf.dll
* LICENSE
* log4net.dll
* Microsoft.Dynamic.dll
* Microsoft.Scripting.AspNet.dll
* Microsoft.Scripting.dll
* Microsoft.Scripting.Metadata.dll
* README (this file)
* ZptBuilder.1
* ZptBuilder.exe
* ZptBuilder.exe.config
* ZptBuilder.MANUAL.txt

========================
What you need to install
========================

The absolute minimum installation requires the following.
These files must be copied to the 'bin' directory for your installation.

* CSF.Caches.dll
* CSF.Cli.Parameters.dll
* CSF.Configuration.dll
* CSF.Reflection.dll
* CSF.Utils.dll
* CSF.Zpt.Abstractions.dll
* CSF.Zpt.dll

This will give you the ZPT-Sharp framework and nothing more.
With only these installed, ZPT-Sharp will not be able to provide any useful
output.  You must install at least one Document Provider plugin and at least
one Expression Evaluator plugin in order achieve that.

Installing plugins involves copying them to the 'bin' directory for the
application and also editing the application (or web) configuration file to
install them.  See the online documentation for the configuration syntax.

The document provider plugins are as follows:

HTML plugin
-----------
* CSF.Zpt.DocumentProviders.HtmlHAP.dll
* HtmlAgilityPack.dll

XML Linq plugin
---------------
* CSF.Zpt.DocumentProviders.XmlLinq.dll

XML Legacy plugin
-----------------
* CSF.Zpt.DocumentProviders.XmlLegacy.dll
* * This plugin is not part of the standard distribution,
    its use is discouraged unless absolutely required.

The expression evaluator plugins are as follows:

TALES path expressions
----------------------
* CSF.Zpt.ExpressionEvaluators.PathExpressions.dll

TALES string expressions
------------------------
* CSF.Zpt.ExpressionEvaluators.StringExpressions.dll
* * Note that the string expressions plugin REQUIRES the
    path expressions plugin to be installed also

TALES not expressions
---------------------
* CSF.Zpt.ExpressionEvaluators.NotExpressions.dll

TALES csharp expressions
------------------------
* CSF.Zpt.ExpressionEvaluators.CSharpExpressions.dll
* CSF.Zpt.ExpressionEvaluators.CSharpFramework.dll

TALES python expressions
------------------------
* CSF.Zpt.ExpressionEvaluators.PythonExpressions.dll
* IronPython.dll
* IronPython.Modules.dll
* IronPython.SQLite.dll
* IronPython.Wpf.dll
* Microsoft.Dynamic.dll
* Microsoft.Scripting.AspNet.dll
* Microsoft.Scripting.dll
* Microsoft.Scripting.Metadata.dll
* * This plugin is not part of the standard distribution.

TALES load expressions
----------------------
* CSF.Zpt.ExpressionEvaluators.LoadExpressions.dll
* * This plugin is not part of the standard distribution.

Beyond the plugins - the remaining things to install depend upon your use-case.

If you wish to use ZPT-Sharp as an ASP.NET MVC ViewEngine then you must install:

* CSF.Zpt.MVC5.dll

If you wish to use the ZptBuilder.exe application then you will require:

* ZptBuilder.exe
* ZptBuilder.exe.config   This will require customisation based on your chosen
                          plugins

If you wish to use ZPT-Sharp as an API for your own software then strongly
consider including the XML files distributed with each of the assemblies you
reference.  These contain documentation which should be interpreted by your IDE,
in Visual Studio for example, these will assist with IntelliSense.