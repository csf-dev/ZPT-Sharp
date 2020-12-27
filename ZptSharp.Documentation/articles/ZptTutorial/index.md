# Page templates tutorial
<div class="note">
This tutorial assumes that you have followed one of the following quick-start guides. They each conclude with a functioning 'hello world' ZPT environment.

* [ASP.NET MVC 5]
* [ASP.NET Core MVC]
* [ZptSharp command-line app]
* [Using ZptSharp API]

[ASP.NET MVC 5]: ../QuickStart/Mvc5
[ASP.NET Core MVC]: ../QuickStart/MvcCore
[ZptSharp command-line app]: ../QuickStart/CliApp
[Using ZptSharp API]: ../QuickStart/ConsumingTheApi
</div>

# `tal:content` replaces an element's content
Let's look again at the template from the quick-start guides.

```html
<html>
<head>
</head>
<body>
<h1 tal:content="here/Message">A famous greeting</h1>
</body>
</html>
```

Notice how in the rendered document, the words "A famous greeting" are replaced by "Hello world!". The `<h1>` tags remain in the rendered output though.

## Elements are replaced too
Try changing the `h1` tag (in the source document) to the following:

```html
<h1 tal:content="here/Message">
  A <em>very</em> famous greeting
</h1>
```

If you render this again, the output will be the same. When a `tal:content` attribute replaces the content of an element, *all of its content is replaced*, including text and even trees of elements as applicable.

This effect can be put to good use in source documents, allowing the addition of *sample content*.

## Including markup in the content
Let's try another change. This time we will change the model rather than the document.

Change the value of `Message` in the model to the following string. We will see what happens if we include markup in the model value to be inserted into a document template.

```
"<strong>Hello world!</strong>"
```

If you render the document again now, you will see that *the markup from the model has been HTML encoded*. What is actually written to the output is:

```html
<h1>
  &lt;strong&gt;Hello world!&lt!/strong&gt;
</h1>
```

*By default, everything written to a document by `tal:content` is HTML (or XML) encoded.* This is a good thing, because it helps protect your site/app from [XSS attacks].

It is possible to include markup within the replacement content, though, but only by altering the source document to specifically permit it. Let's change the source document now. The change is to add the `structure` keyword to the content attribute, prefixing the reference to the model value.

```html
<h1 tal:content="structure here/Message">A famous greeting</h1>
```

If we try rendering the document once more, we will see that the markup within the model value is now honoured and not HTML encoded.

*The `structure` keyword, used as the first thing within a `tal:content` attribute, causes the content to be interpreted as markup and not strictly text*. In case it is not obvious, it is crucial that you only use the `structure` keyword when you are certain that the value has been thoroughly sanitised.

[XSS attacks]: https://wikipedia.org/wiki/Cross-site_scripting

## `tal:replace` replaces the whole element
The last thing we will look at in this step of the tutorial is the `tal:replace` attribute. Let's swap the `tal:content` attribute in our example for a replace attribute, so that the markup looks like this now:

```html
<h1 tal:replace="here/Message">A famous greeting</h1>
```

If you render this then you will see that the model value is rendered into the document, but that the entire containing `h1` has been replaced, not just it's content/children.

*`tal:replace` attributes work just like `tal:content`, except that they replace the whole element and not just the content.* This includes the replacement of child elements and also the HTML/XML-encoding of the replacement value, and the use of the `structure` keyword to override that.

## Next: Conditions and repetition
In the next tutorial page, we will look into [how to render elements conditionally & how to repeat them for items in a collection](Page2).