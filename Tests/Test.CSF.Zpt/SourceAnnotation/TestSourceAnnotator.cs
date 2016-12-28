using System;
using NUnit.Framework;
using Moq;
using CSF.Zpt.Rendering;
using CSF.Zpt.SourceAnnotation;
using Ploeh.AutoFixture;
using CSF.Zpt;

namespace Test.CSF.Zpt.SourceAnnotation
{
  [TestFixture]
  public class TestSourceAnnotator
  {
    #region fields

    private IFixture _autofixture;
    private Mock<IZptElement> _element;
    private IRenderingContext _context;
    private Mock<ICommentFormatter> _formatter;
    private ISourceAnnotator _sut;
    private ISourceInfo _sourceInfo, _originalSourceInfo;
    private string _source, _originalSource, _startLine, _endLine;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();

      _source = _autofixture.Create<string>();
      _originalSource = _autofixture.Create<string>();
      _startLine = _autofixture.Create<string>();
      _endLine = _autofixture.Create<string>();
      _sourceInfo = Mock.Of<ISourceInfo>(x => x.GetRelativeName(It.IsAny<string>()) == _source);
      _originalSourceInfo = Mock.Of<ISourceInfo>(x => x.GetRelativeName(It.IsAny<string>()) == _originalSource);

      _element = new Mock<IZptElement>();
      _element.Setup(x => x.AddCommentBefore(It.IsAny<string>()));
      _element.Setup(x => x.AddCommentAfter(It.IsAny<string>()));
      _element.Setup(x => x.AddCommentInside(It.IsAny<string>()));
      _element.Setup(x => x.GetSourceInfo()).Returns(_sourceInfo);
      _element.Setup(x => x.GetFileLocation()).Returns(_startLine);
      _element.Setup(x => x.GetOriginalContextEndTagLocation()).Returns(_endLine);
      _element.Setup(x => x.GetOriginalContextSourceInfo()).Returns(_originalSourceInfo);

      _context = Mock.Of<IRenderingContext>(x => x.Element == _element.Object
                                                 && x.RenderingOptions.AddSourceFileAnnotation == true);

      _formatter = new Mock<ICommentFormatter>();

      _sut = new SourceAnnotator(_formatter.Object);
    }

    #endregion

    #region tests

    [Test]
    public void WriteAnnotationIfAppropriate_writes_nothing_if_element_is_not_root_or_imported()
    {
      // Arrange
      _element.SetupGet(x => x.IsRoot).Returns(false);
      _element.SetupGet(x => x.IsImported).Returns(false);

      // Act
      _sut.WriteAnnotationIfAppropriate(_context);

      // Assert
      _element.Verify(x => x.AddCommentBefore(It.IsAny<string>()), Times.Never());
      _element.Verify(x => x.AddCommentAfter(It.IsAny<string>()), Times.Never());
      _element.Verify(x => x.AddCommentInside(It.IsAny<string>()), Times.Never());
    }

    [Test]
    public void WriteAnnotationIfAppropriate_writes_annotation_for_root_element()
    {
      // Arrange
      _element.SetupGet(x => x.IsRoot).Returns(true);
      _element.SetupGet(x => x.IsImported).Returns(false);

      var commentString = _autofixture.Create<string>();

      _formatter
        .Setup(x => x.GetRootElementComment(_source, _startLine))
        .Returns(commentString);

      // Act
      _sut.WriteAnnotationIfAppropriate(_context);

      // Assert
      _element.Verify(x => x.AddCommentBefore(commentString), Times.Once());
      _element.Verify(x => x.AddCommentAfter(It.IsAny<string>()), Times.Never());
      _element.Verify(x => x.AddCommentInside(It.IsAny<string>()), Times.Never());
    }

    [Test]
    public void WriteAnnotationIfAppropriate_writes_annotation_before_macro_definition()
    {
      // Arrange
      _element.SetupGet(x => x.IsRoot).Returns(false);
      _element.SetupGet(x => x.IsImported).Returns(false);
      var attrib = Mock.Of<IZptAttribute>();
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                   ZptConstants.Metal.DefineMacroAttribute))
        .Returns(attrib);

      var commentString = _autofixture.Create<string>();

      _formatter
        .Setup(x => x.GetDefineMacroComment(_source, _startLine))
        .Returns(commentString);

      // Act
      _sut.WriteAnnotationIfAppropriate(_context);

      // Assert
      _element.Verify(x => x.AddCommentBefore(commentString), Times.Once());
      _element.Verify(x => x.AddCommentAfter(It.IsAny<string>()), Times.Never());
      _element.Verify(x => x.AddCommentInside(It.IsAny<string>()), Times.Never());
    }

    [Test]
    public void WriteAnnotationIfAppropriate_writes_annotation_after_slot_definition()
    {
      // Arrange
      _element.SetupGet(x => x.IsRoot).Returns(false);
      _element.SetupGet(x => x.IsImported).Returns(false);
      var attrib = Mock.Of<IZptAttribute>();
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                   ZptConstants.Metal.DefineSlotAttribute))
        .Returns(attrib);

      var commentString = _autofixture.Create<string>();

      _formatter
        .Setup(x => x.GetDefineSlotComment(_source, _startLine))
        .Returns(commentString);

      // Act
      _sut.WriteAnnotationIfAppropriate(_context);

      // Assert
      _element.Verify(x => x.AddCommentAfter(commentString), Times.Once());
      _element.Verify(x => x.AddCommentBefore(It.IsAny<string>()), Times.Never());
      _element.Verify(x => x.AddCommentInside(It.IsAny<string>()), Times.Never());
    }

    [Test]
    public void WriteAnnotationIfAppropriate_writes_annotation_before_imported_element()
    {
      // Arrange
      _element.SetupGet(x => x.IsRoot).Returns(false);
      _element.SetupGet(x => x.IsImported).Returns(true);

      var commentString = _autofixture.Create<string>();

      _formatter
        .Setup(x => x.GetImportedElementComment(_source, _startLine))
        .Returns(commentString);

      // Act
      _sut.WriteAnnotationIfAppropriate(_context);

      // Assert
      _element.Verify(x => x.AddCommentBefore(commentString), Times.Once());
      _element.Verify(x => x.AddCommentInside(It.IsAny<string>()), Times.Never());
    }

    [Test]
    public void WriteAnnotationIfAppropriate_writes_annotation_after_imported_element()
    {
      // Arrange
      _element.SetupGet(x => x.IsRoot).Returns(false);
      _element.SetupGet(x => x.IsImported).Returns(true);

      var commentString = _autofixture.Create<string>();

      _formatter
        .Setup(x => x.GetAfterImportedElementComment(_originalSource, _endLine))
        .Returns(commentString);

      // Act
      _sut.WriteAnnotationIfAppropriate(_context);

      // Assert
      _element.Verify(x => x.AddCommentAfter(commentString), Times.Once());
      _element.Verify(x => x.AddCommentInside(It.IsAny<string>()), Times.Never());
    }

    #endregion
  }
}

