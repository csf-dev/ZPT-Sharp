using System;
using NUnit.Framework;
using CSF.Zpt;
using Moq;
using Moq.Protected;
using CSF.Zpt.Rendering;
using System.IO;

namespace Test.CSF.Zpt
{
  [TestFixture]
  public class TestZptDocument
  {
    #region fields

    private Mock<ZptDocument> _sut;
    private IZptElement _rootElement, _renderedElement;
    private Mock<IElementRenderer> _elementRenderer;

    private IRenderingSettings _defaultSettings;

    #endregion

    #region properties

    protected virtual ZptDocument Sut
    {
      get {
        return _sut.Object;
      }
    }

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _defaultSettings = RenderingSettings.Default;

      _rootElement = Mock.Of<IZptElement>();
      _renderedElement = Mock.Of<IZptElement>();
      _elementRenderer = new Mock<IElementRenderer>();

      _sut = new Mock<ZptDocument>(_elementRenderer.Object) { CallBase = true };

      _sut.Protected().Setup<IRenderingSettings>("GetDefaultOptions").Returns(_defaultSettings);
      _sut.Protected().Setup<IZptElement>("GetRootElement").Returns(_rootElement);
    }

    #endregion

    #region tests

    [Test]
    public void Render_uses_default_rendering_settings_where_none_provided()
    {
      // Arrange
      var model = new object();
      _elementRenderer
        .Setup(x => x.RenderElement(model,
                                    _rootElement,
                                    _defaultSettings,
                                    It.IsAny<Action<IModelValueContainer>>()))
        .Returns(_renderedElement);
      
      _sut.Protected().Setup("Render", ItExpr.IsAny<TextWriter>(), _renderedElement, _defaultSettings);

      // Act
      _sut.Object.Render(model, Mock.Of<TextWriter>());

      // Assert
      _sut.Protected().Verify("Render", Times.Once(), ItExpr.IsAny<TextWriter>(), _renderedElement, _defaultSettings);
    }

    [Test]
    public void Render_uses_provided_rendering_settings_where_given()
    {
      // Arrange
      var model = new object();
      var settings = Mock.Of<IRenderingSettings>();
      _elementRenderer
        .Setup(x => x.RenderElement(model,
                                    _rootElement,
                                    settings,
                                    It.IsAny<Action<IModelValueContainer>>()))
        .Returns(_renderedElement);

      _sut.Protected().Setup("Render", ItExpr.IsAny<TextWriter>(), _renderedElement, settings);

      // Act
      _sut.Object.Render(model, Mock.Of<TextWriter>(), options: settings);

      // Assert
      _sut.Protected().Verify("Render", Times.Once(), ItExpr.IsAny<TextWriter>(), _renderedElement, settings);
    }

    [Test]
    public void Render_uses_element_renderer()
    {
      // Arrange
      var model = new object();
      _elementRenderer
        .Setup(x => x.RenderElement(model,
                                    _rootElement,
                                    _defaultSettings,
                                    It.IsAny<Action<IModelValueContainer>>()))
        .Returns(_renderedElement);

      // Act
      _sut.Object.Render(model, Mock.Of<TextWriter>());

      // Assert
      _elementRenderer
        .Verify(x => x.RenderElement(model,
                                     _rootElement,
                                     _defaultSettings,
                                     It.IsAny<Action<IModelValueContainer>>()),
                Times.Once());
    }

    #endregion
  }
}

