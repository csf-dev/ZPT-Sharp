# What is ZptSharp?
ZptSharp is an open source [library for .NET] for writing HTML or XML documents based upon page templates.
It is useable as a library, and packages are available for use:

* As an ASP.NET/ASP.NET Core MVC **View Engine**
* As a standalone command-line application

ZptSharp is based upon the *Zope Page Templates* specification & syntax.
This has its origins in Python & [the Zope application framework].
ZptSharp is a pure .NET implementation of just the page templates syntax from Zope.
*It does not depend upon Zope* or Python.

[library for .NET]: Compatibility.md
[the Zope application framework]: https://zope.org/

## What is the syntax?
ZPT is *an attribute language* designed specifically for use with HTML and/or XML documents. All of the ZPT directives are placed in **attributes**.

ZPT syntax:

* Does not interfere with the validity or structure of the underlying HTML/XML document
* Allows template source files to be viewed/edited 'offline' from their applications with web browsers or WYSIWYG tools
* Is intuitive to read and understand, even to non-developers
* Encourages best-practices in separation between Model & View
* Offers the full range of functionality to support complex MVC applications

## What are its fundamentals?
ZPT's syntax is organised into three logical 'modules' of functionality.

* **TALES** is an extensible expression syntax used to refer to your model values. It may also be used for limited manipulation, primarily aimed at formatting/transforming values for writing to the document. TALES is used by both TAL & METAL (below).
* **METAL** is an attribute syntax for reusing sections of markup: "macros". The syntax also allows contextual customisation of macros, by filling placeholder "slots" which they define.
* **TAL** is an attribute syntax for writing model content to the document, conditionally removing parts of the document, repeating sections of markup for each item in a model collection and more.