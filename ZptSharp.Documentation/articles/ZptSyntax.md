ZptSharp traces its origins back to Python and [the Zope framework]; ZPT stands for **Zope Page Templates**.

ZptSharp is an implementation of just the page template syntax for .NET. ZptSharp has no dependencies upon Zope, nor does it contain implementation of any other Zope technologies.

## How ZPT is different
ZPT is an *attribute language*, designed specifically for use with either HTML or XML. The control/binding statements are written in the attributes of elements.

Consider other template languages which use constructs like `<%` or their own block-level syntax. When mixed with HTML/XML, these languages can make the underlying markup invalid. This leads to difficulties working with those source files in isolation, without running the whole application.

A ZPT source file may be opened offline from the application in a web browser or WYSIWYG editor and remains *valid and readable*. Additionally, there is no 'alien' or unintelligible control syntax visible when the source file is viewed in a web browser.

[the Zope framework]: https://zope.org/

## Learning ZPT
The ZPT syntax has three conceptual parts:

* **METAL** - used to define & reuse 'macros' of markup, such as components or common page elements.
* **TAL** - used to bind your model data to the template.
* **TALES** - an extensible expression syntax used to describe values.