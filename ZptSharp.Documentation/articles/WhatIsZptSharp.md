# What is ZptSharp?

ZptSharp is an open source [library for .NET] for writing HTML or XML documents based upon page templates.
It may be used in your own applications as a library.
Packages are also available to use ZptSharp:

* As a **View Engine** for AS<span>P.N</span>ET MVC5 or AS<span>P.N</span>ET Core MVC
* As a standalone command-line application

ZptSharp is based upon the *Zope Page Templates* specification & syntax.
This has its origins in Python & [the Zope application framework].
ZptSharp is a pure .NET implementation of just the page templates syntax from Zope.
*It does not depend upon Zope* or Python.

[library for .NET]: Compatibility.md
[the Zope application framework]: https://zope.org/

## What is the syntax?

ZPT is *an attribute language* designed specifically for use with HTML and/or XML documents. All of the ZPT directives are written using **attributes**.

ZPT syntax:

* Does not interfere with the validity or structure of the underlying HTML/XML document
* Allows template source files to be viewed/edited 'offline' from their applications with web browsers or WYSIWYG tools
* Is intuitive to read and understand, even to non-developers
* Encourages best-practices in separation between Model & View
* Offers the full range of functionality to support complex MVC applications

## What are its fundamentals?

ZPT's syntax is organised into three logical 'modules' of functionality.
The first of these is an extensible expression syntax for accessing the model, which has the goal of being easy to read & understand without programming knowledge.
Most expressions will look a lot like a URL path, such as `here/Product/Name`.
This expression syntax is named **TALES**.

The second is a mechanism (named **METAL**) for reusing markup across document templates and authoring logical parts of a document as separate source files.
Designers define *macros* of markup for re-use, which may optionally contain *slots* (placeholders).
At the point of macro usage, markup/content may be supplied to fill the available slots.

The third function of ZPT handles the actual data-binding and manipulation of the document using the model; this is a syntax named **TAL**.
Some aspects of that functionality include the following, although this is not a comprehensive list of its capabilities.

* Writing model content to the document
* Conditionally removing parts of the document
* Repeating parts of the document for items in a collection
* Adding/removing/updating attributes & values
