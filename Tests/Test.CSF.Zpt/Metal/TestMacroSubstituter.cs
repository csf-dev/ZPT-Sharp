using System;
using NUnit.Framework;
using CSF.Zpt.Metal;
using Moq;
using CSF.Zpt.Rendering;
using Ploeh.AutoFixture;
using CSF.Zpt;
using System.Linq;

namespace Test.CSF.Zpt.Metal
{
  [TestFixture]
  public class TestMacroSubstituter
  {
    #region fields

    private IFixture _autoFixture;

    private MacroSubstituter _sut;

    private Mock<IRenderingContext> _slot, _filler;
    private Mock<IZptElement> _slotElement, _fillerElement, _replacementElement;
    private SlotToFill _slotAndFiller;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autoFixture = new Fixture();

      _slot = new Mock<IRenderingContext>();
      _filler = new Mock<IRenderingContext>();

      _slotElement = new Mock<IZptElement>();
      _fillerElement = new Mock<IZptElement>();
      _replacementElement = new Mock<IZptElement>();

      _slot.SetupGet(x => x.Element).Returns(_slotElement.Object);
      _filler.SetupGet(x => x.Element).Returns(_fillerElement.Object);

      _slotElement.Setup(x => x.ReplaceWith(_fillerElement.Object)).Returns(_replacementElement.Object);

      _slotAndFiller = new SlotToFill(_slot.Object, _filler.Object, _autoFixture.Create<string>());

      _sut = new MacroSubstituter();
    }

    #endregion

    #region tests

    [Test]
    public void FillSlot_replaces_slot_with_filler()
    {
      // Arrange

      // Act
      _sut.FillSlot(_slotAndFiller);

      // Assert
      _slotElement.Verify(x => x.ReplaceWith(_fillerElement.Object), Times.Once());
    }

    [Test]
    public void FillSlot_returns_replacement_element()
    {
      // Arrange

      // Act
      var result = _sut.FillSlot(_slotAndFiller);

      // Assert
      Assert.AreSame(_replacementElement.Object, result);
    }

    [Test]
    public void FillSlot_copies_fill_slot_attribute_from_slot_to_filler_when_present()
    {
      // Arrange
      var slotFillerAttrib = new Mock<IZptAttribute>();
      var slotFillerValue = _autoFixture.Create<string>();
      slotFillerAttrib.SetupGet(x => x.Value).Returns(slotFillerValue);

      _slotElement
        .Setup(x => x.GetAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.FillSlotAttribute))
        .Returns(slotFillerAttrib.Object);

      // Act
      _sut.FillSlot(_slotAndFiller);

      // Assert
      _replacementElement
        .Verify(x => x.SetAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.FillSlotAttribute, slotFillerValue),
                Times.Once());
    }

    [Test]
    public void FillSlot_does_not_copy_attribute_to_filler_when_not_present_on_slot()
    {
      // Arrange
      _slotElement
        .Setup(x => x.GetAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.FillSlotAttribute))
        .Returns((IZptAttribute) null);

      // Act
      _sut.FillSlot(_slotAndFiller);

      // Assert
      _replacementElement
        .Verify(x => x.SetAttribute(It.IsAny<ZptNamespace>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never());
    }

    [Test]
    public void GetSlotsToFill_queries_both_sources()
    {
      // Arrange
      _slotElement
        .Setup(x => x.SearchChildrenByAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.DefineSlotAttribute))
        .Returns(new IZptElement[0]);
      _fillerElement
        .Setup(x => x.SearchChildrenByAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.FillSlotAttribute))
        .Returns(new IZptElement[0]);

      // Act
      _sut.GetSlotsToFill(_filler.Object,
                          _slot.Object,
                          new IRenderingContext[0]);

      // Assert
      _slotElement
        .Verify(x => x.SearchChildrenByAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.DefineSlotAttribute),
                Times.Once());
      _fillerElement
        .Verify(x => x.SearchChildrenByAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.FillSlotAttribute),
                Times.Once());
    }

    [Test]
    [Ignore("This test needs to be written - see #182")]
    public void GetDefinedSlots_from_parent_queries_every_item_in_stack()
    {
      // Arrange
      

      // Act

      // Assert
    }

    [Test]
    [Ignore("This test needs to be written - see #182")]
    public void GetDefinedSlots_from_parent_does_not_find_slots_already_present_in_main_macro()
    {
      // Arrange


      // Act

      // Assert
    }

    [Test]
    [Ignore("This test needs to be written - see #182")]
    public void GetDefinedSlots_from_parent_uses_slots_from_bottom_of_stack_in_preference_to_top_of_stack()
    {
      // Arrange


      // Act

      // Assert
    }

    [Test]
    [Ignore("This test needs to be written - see #182")]
    public void GetSlotsToFill_query_does_not_find_unfilled_slots()
    {
      // Arrange


      // Act

      // Assert
    }

    [Test]
    [Ignore("This test needs to be written - see #182")]
    public void GetSlotsToFill_query_does_not_find_fillers_with_no_corresponding_definition()
    {
      // Arrange


      // Act

      // Assert
    }

    [Test]
    [Ignore("This test needs to be written - see #182")]
    public void GetSlotsToFill_query_matches_slots_with_fillers()
    {
      // Arrange


      // Act

      // Assert
    }

    [Test]
    [Ignore("This test needs to be written - see #182")]
    public void ReplaceMacroElement_replaces_the_source_context()
    {
      // Arrange


      // Act

      // Assert
    }

    [Test]
    [Ignore("This test needs to be written - see #182")]
    public void ReplaceMacroElement_returns_the_replacement_result()
    {
      // Arrange


      // Act

      // Assert
    }

    #endregion
  }
}

