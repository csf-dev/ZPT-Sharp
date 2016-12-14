using System;
using NUnit.Framework;
using Ploeh.AutoFixture;
using CSF.Zpt.Metal;
using Moq;
using CSF.Zpt.Rendering;
using CSF.Zpt;
using System.Linq;

namespace Test.CSF.Zpt.Metal
{
  [TestFixture]
  public class TestMacroExtensionSubstitutor
  {
    #region fields

    private IFixture _autoFixture;

    private MacroSubstituter _sut;

    private Mock<IRenderingContext> _contextOne, _contextTwo;
    private Mock<IZptElement> _elementOne, _elementTwo, _elementThree;
    private SlotToFill _slotAndFiller;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autoFixture = new Fixture();

      _contextOne = new Mock<IRenderingContext>();
      _contextTwo = new Mock<IRenderingContext>();

      _elementOne = new Mock<IZptElement>();
      _elementTwo = new Mock<IZptElement>();
      _elementThree = new Mock<IZptElement>();

      _contextOne.SetupGet(x => x.Element).Returns(_elementOne.Object);
      _contextTwo.SetupGet(x => x.Element).Returns(_elementTwo.Object);

      _elementOne.Setup(x => x.ReplaceWith(_elementTwo.Object)).Returns(_elementThree.Object);

      _slotAndFiller = new SlotToFill(_contextOne.Object, _contextTwo.Object, _autoFixture.Create<string>());

      _sut = new MacroExtensionSubstitutor();
    }

    #endregion

    #region tests

    [Test]
    public void FillSlot_copies_define_slot_attribute_if_it_is_not_redefined()
    {
      // Arrange
      _elementTwo
        .Setup(x => x.SearchChildrenByAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.DefineSlotAttribute))
        .Returns(Enumerable.Empty<IZptElement>().ToArray());

      // Act
      _sut.FillSlot(_slotAndFiller);

      // Assert
      _elementThree.Verify(x => x.SetAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.DefineSlotAttribute, _slotAndFiller.Name),
                           Times.Once());
    }

    [Test]
    public void FillSlot_does_not_copy_define_slot_attribute_if_it_is_redefined()
    {
      _elementTwo
        .Setup(x => x.SearchChildrenByAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.DefineSlotAttribute))
        .Returns(new [] {
          Mock.Of<IZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.DefineSlotAttribute).Value == _slotAndFiller.Name),
        });

      // Act
      _sut.FillSlot(_slotAndFiller);

      // Assert
      _elementThree.Verify(x => x.SetAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.DefineSlotAttribute, It.IsAny<string>()),
                           Times.Never());
    }

    #endregion
  }
}

