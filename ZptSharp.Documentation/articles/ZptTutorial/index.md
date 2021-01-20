# Page templates tutorial

<div class="note">

_This tutorial assumes that you have followed one of the following quick-start guides_.
Each of them concludes with a functioning "hello world" ZPT environment which we shall build upon in this tutorial.

* [ASP.NET MVC 5]
* [ASP.NET Core MVC]
* [ZptSharp command-line app]
* [Using ZptSharp API]

[ASP.NET MVC 5]: ../QuickStart/Mvc5.md
[ASP.NET Core MVC]: ../QuickStart/MvcCore.md
[ZptSharp command-line app]: ../QuickStart/CliApp.md
[Using ZptSharp API]: ../QuickStart/ConsumingTheApi.md
</div>

## `tal:content` replaces an element's content

Let's look again at the ZPT template file from the quick-start guides.
If you follow the guide then it should look a little like this.

```html
<html>
<head>
<title>ZptSharp 'Hello world' example</title>
</head>
<body>
<h1>Example app</h1>
<p tal:content="here/Message">The greeting message appears here.</p>
</body>
</html>
```

Notice how in the rendered document, the words "A famous greeting" are replaced by "Hello world!".
The `<p>` tags remain in the rendered output but their content is substituted.
This is the primary function of the `tal:content` attribute.

## Elements are replaced too

Try changing the `p` tag (in the source document) to the following:

```html
<p tal:content="here/Message">
  The <em>very famous</em> greeting message appears here.
</p>
```

If you render this again, the output will be the same. When a `tal:content` attribute replaces the content of an element, *all of its content is replaced*, including text and even trees of elements as applicable.

This effect can be put to good use in source documents, allowing the addition of *sample or placeholder content*.
Sample content may be used so that a designer may see what the rendered page is expected to look like, even when the source file is 'offline' from its application logic.

## Including markup in the content

Let's try another change. This time we will change the model rather than the document.

Change the value of `Message` in the model to the following string. We will see what happens if we include markup in the model value to be inserted into a document template.

```text
<strong>Hello world!</strong>
```

If you render the document again now, you will see that *the markup from the model has been HTML encoded*. What is actually written to the output is:

```html
<p>
  &lt;strong&gt;Hello world!&lt;/strong&gt;
</p>
```

*By default, everything written to a document by `tal:content` is HTML (or XML) encoded.* This is a good thing, because it helps protect your site/app from [XSS attacks].

It is possible to include markup within the replacement content, though, but only by altering the source document to specifically permit it. Let's change the source document now. The change is to add the `structure` keyword to the content attribute, prefixing the reference to the model value.

```html
<p tal:content="structure here/Message">The greeting message appears here.</p>
```

If we try rendering the document once more, we will see that the markup within the model value is now honoured and not HTML encoded.

*The `structure` keyword, used as the first thing within a `tal:content` attribute, causes the content to be interpreted as markup and not strictly text*. In case it is not obvious, it is crucial that you only use the `structure` keyword when you are certain that the value has been thoroughly sanitised.

[XSS attacks]: https://wikipedia.org/wiki/Cross-site_scripting

## `tal:replace` replaces the whole element

The last thing we will look at in this step of the tutorial is the `tal:replace` attribute. Let's swap the `tal:content` attribute in our example for a replace attribute, so that the markup looks like this now:

```html
<p tal:replace="here/Message">The greeting message appears here.</p>
```

If you render this then you will see that the model value is rendered into the document, but that the entire containing `p` has been replaced, not just it's content/children.

*`tal:replace` attributes work just like `tal:content`, except that they replace the whole element and not just the content.* This includes the replacement of child elements and also the HTML/XML-encoding of the replacement value, and the use of the `structure` keyword to override that.

## Next: Conditions and repetition

In the next tutorial page, we will look into [how to render elements conditionally & how to repeat them for items in a collection](Page2.md).
