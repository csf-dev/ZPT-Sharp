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
* Is intuitive to read and understand
* Encourages best-practices in separation between Model & View
* Offers the full range of functionality to support complex MVC applications

## What are its fundamentals?
The ZPT syntax has three fundamental parts:

### METAL is for reusing markup
The first thing which happens in the ZPT rendering process is the processing of METAL attributes.
With METAL you may define and consume **macros**, which are reusable pieces of markup.

Macros may have **slots**, which consuming markup can fill with it's own content, customising the macro on a use-by-use basis. Thus macros may be used as content (for example a control), or as a 'wrapper' for content (for example to surround an article with standard markup) or as a hybrid of both.

Macros may be defined in the same source document as they are consumed, or they may be consumed from other documents.

### TAL binds your model data
After METAL has assembled the source document, TAL is used to manipulate the DOM based upon your model data. This includes (but is not limited to):

* Writing model data to the document
* Adding, setting or removing attributes
* Conditionally excluding elements and their children
* Repeating an element and its children for every item in a collection

### TALES is how expressions are written
TALES is an intuitive syntax for accessing the model values, used by both TAL & METAL. Here are some examples.

| Expression type | Sample                                         |
| --------------- | ------                                         |
| `path`          | `path:here/Company/Name`                       |
| `string`        | `string:The ${group/Profession} who say "Ni!"` |
| `not`           | `not:loan/IsOverdue`                           |

Additionally *the configured default expression type* (usually path expressions) doesn't need the `type:` prefix.

What's more - TALES is extensible. You may write your own expression evaluator implementations and add support for any expression syntax you wish.