using System;
using System.Reflection;
using CSF.Reflection;
using CSF.Zpt.Rendering;
using HtmlAgilityPack;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Test.CSF.Zpt.Rendering
{
  [TestFixture]
  public class TestHtmlElement
  {
    #region fields

    private string _htmlSource;
    private HtmlDocument _document;

    #endregion

    #region setup

    [TestFixtureSetUp]
    public void FixtureSetup()
    {
      _htmlSource = Assembly.GetExecutingAssembly().GetManifestResourceText(this.GetType(), "TestHtmlElement.html");
    }

    [SetUp]
    public void Setup()
    {
      _document = new HtmlDocument();
      _document.LoadHtml(_htmlSource);
    }

    #endregion

    #region tests

    [Test]
    public void TestToString()
    {
      // Arrange
      var sut = new HtmlElement(_document.DocumentNode);

      // Act
      var result = sut.ToString();

      // Assert
      Assert.AreEqual(_htmlSource, result);
    }

    [Test]
    public void TestGetChildElements()
    {
      // Arrange
      var sut = new HtmlElement(_document.DocumentNode);

      // Act
      var result = sut.GetChildElements();

      // Assert
      Assert.AreEqual(2, result.Length, "Count of child element");
      Assert.AreEqual("head", result[0].Name, "Name of first child");
      Assert.AreEqual("body", result[1].Name, "Name of second child");
    }

    [Test]
    public void TestSearchChildrenByAttribute()
    {
      // Arrange
      var sut = new HtmlElement(_document.DocumentNode);
      var fixture = new Fixture();

      // Act
      var results = sut.SearchChildrenByAttribute(fixture.Create<string>(), "custom", "parent_attrib");

      // Assert
      Assert.NotNull(results, "Result nullability");
      Assert.AreEqual(1, results.Length, "Count of results");
      Assert.AreEqual("div", results[0].Name, "Name of found element");
    }

    [Test]
    public void TestGetAttributes()
    {
      // Arrange
      var sut = new HtmlElement(_document.DocumentNode.FirstChild.ChildNodes[3].ChildNodes[1].ChildNodes[1]);

      // Act
      var results = sut.GetAttributes();

      // Assert
      Assert.NotNull(results, "Result nullability");
      Assert.AreEqual(2, results.Length, "Count of results");
      Assert.AreEqual("custom:parent_attrib", results[0].Name, "Attribute name one");
      Assert.AreEqual("class", results[1].Name, "Attribute name two");
    }

    //        Prefix    Name              Expected name             Expected value
    //        ------    ----              -------------             --------------
    [TestCase("custom", "parent_attrib",  "custom:parent_attrib",   "Attribute value one")]
    [TestCase(null,     "class",          "class",                  "class_one class_two")]
    public void TestGetAttribute(string prefix, string name, string expectedName, string expectedValue)
    {
      // Arrange
      var sut = new HtmlElement(_document.DocumentNode.FirstChild.ChildNodes[3].ChildNodes[1].ChildNodes[1]);
      var fixture = new Fixture();

      // Act
      var result = sut.GetAttribute(fixture.Create<string>(), prefix, name);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(expectedName, result.Name, "Result name");
      Assert.AreEqual(expectedValue, result.Value, "Result value");
    }

    [Test]
    public void TestReplaceWith()
    {
      // Arrange
      var sut = new HtmlElement(_document.DocumentNode.FirstChild.ChildNodes[3].ChildNodes[1].ChildNodes[1]);
      var replacementHtml = "<p>Replacement element</p>";
      var secondDocument = new HtmlDocument();
      secondDocument.LoadHtml(replacementHtml);
      var secondElement = new HtmlElement(secondDocument.DocumentNode.FirstChild);

      // Act
      var result = sut.ReplaceWith(secondElement);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual("p", result.Name, "Result name");
      var expectedDom = @"<html>
<head>
<title>Document title</title>
</head>
<body>
<header>
  <p>Replacement element</p>
  Page header
</header>
<section>
  <header>
    <h1 id=""page_heading"">Page heading</h1>
  </header>
  <p>A paragraph of content</p>
</section>
<footer>Page footer</footer>
</body>
</html>";
      Assert.AreEqual(expectedDom, _document.DocumentNode.OuterHtml, "Correct modified HTML");
    }

    [Test(Description = "Test that an HtmlElement instance can be constructed from an element node")]
    public void TestConstructorElementNode()
    {
      // Act
      new HtmlElement(_document.DocumentNode.FirstChild);

      // Assert
      Assert.Pass("Test passes because no exception encountered");
    }

    [Test(Description = "Test that an HtmlElement instance can be constructed from a document node")]
    public void TestConstructorDocumentNode()
    {
      // Act
      new HtmlElement(_document.DocumentNode);

      // Assert
      Assert.Pass("Test passes because no exception encountered");
    }

    #endregion
  }
}

