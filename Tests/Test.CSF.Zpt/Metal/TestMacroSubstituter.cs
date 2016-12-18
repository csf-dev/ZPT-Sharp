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
      _elementOne.Verify(x => x.ReplaceWith(_elementTwo.Object), Times.Once());
    }

    [Test]
    public void FillSlot_returns_replacement_element()
    {
      // Arrange

      // Act
      var result = _sut.FillSlot(_slotAndFiller);

      // Assert
      Assert.AreSame(_elementThree.Object, result);
    }

    [Test]
    public void FillSlot_copies_fill_slot_attribute_from_slot_to_filler_when_present()
    {
      // Arrange
      var slotFillerAttrib = new Mock<IZptAttribute>();
      var slotFillerValue = _autoFixture.Create<string>();
      slotFillerAttrib.SetupGet(x => x.Value).Returns(slotFillerValue);

      _elementOne
        .Setup(x => x.GetAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.FillSlotAttribute))
        .Returns(slotFillerAttrib.Object);

      // Act
      _sut.FillSlot(_slotAndFiller);

      // Assert
      _elementThree
        .Verify(x => x.SetAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.FillSlotAttribute, slotFillerValue),
                Times.Once());
    }

    [Test]
    public void FillSlot_does_not_copy_attribute_to_filler_when_not_present_on_slot()
    {
      // Arrange
      _elementOne
        .Setup(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                   ZptConstants.Metal.FillSlotAttribute))
        .Returns((IZptAttribute) null);

      // Act
      _sut.FillSlot(_slotAndFiller);

      // Assert
      _elementThree
        .Verify(x => x.SetAttribute(ZptConstants.Metal.Namespace,
                                    ZptConstants.Metal.FillSlotAttribute,
                                    It.IsAny<string>()),
                Times.Never());
    }

    [Test]
    public void FillSlot_marks_element_as_imported()
    {
      // Arrange
      _elementThree
        .Setup(x => x.SetAttribute(ZptConstants.SourceAnnotation.Namespace,
                                   ZptConstants.SourceAnnotation.ElementIsImported,
                                   Boolean.TrueString));

      // Act
      _sut.FillSlot(_slotAndFiller);

      // Assert
      _elementThree
        .Verify(x => x.SetAttribute(ZptConstants.SourceAnnotation.Namespace,
                                    ZptConstants.SourceAnnotation.ElementIsImported,
                                    Boolean.TrueString),
                Times.Once());
    }

    [Test]
    public void GetSlotsToFill_queries_both_sources()
    {
      // Arrange
      _elementOne
        .Setup(x => x.SearchChildrenByAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.FillSlotAttribute))
        .Returns(new IZptElement[0]);
      _elementTwo
        .Setup(x => x.SearchChildrenByAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.DefineSlotAttribute))
        .Returns(new IZptElement[0]);

      var source = _contextOne.Object;
      var macro = _contextTwo.Object;

      // Act
      _sut.GetSlotsToFill(source, macro, new IRenderingContext[0]);

      // Assert
      _elementOne
        .Verify(x => x.SearchChildrenByAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.FillSlotAttribute),
                Times.Once());
      _elementTwo
        .Verify(x => x.SearchChildrenByAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.DefineSlotAttribute),
                Times.Once());
    }

    [Test]
    public void GetDefinedSlots_from_parent_queries_every_item_in_stack()
    {
      // Arrange
      var macroElements = Enumerable
        .Range(0, 3)
        .Select(x => {
          var output = new Mock<IZptElement>();
          output
            .Setup(e => e.SearchChildrenByAttribute(ZptConstants.Metal.Namespace,
                                                    ZptConstants.Metal.DefineSlotAttribute))
            .Returns(Enumerable.Empty<IZptElement>().ToArray());
          return output;
        })
        .ToArray();
      var contexts = macroElements.Select(x => Mock.Of<IRenderingContext>(c => c.Element == x.Object)).ToList();
      

      // Act
      _sut.GetDefinedSlots(contexts, Enumerable.Empty<string>());

      // Assert
      for(var i = 0; i < macroElements.Length; i++)
      {
        var element = macroElements[i];
        element.Verify(e => e.SearchChildrenByAttribute(ZptConstants.Metal.Namespace,
                                                        ZptConstants.Metal.DefineSlotAttribute),
                       Times.Once(),
                       String.Format("Element {0} searched for slots", i));
      }
    }

    [Test]
    public void GetDefinedSlots_from_parent_does_not_find_slots_already_present_in_main_macro()
    {
      // Arrange
      var macroElements = new [] {
        new Mock<IZptElement>(),
      };
      macroElements[0]
        .Setup(x => x.SearchChildrenByAttribute(ZptConstants.Metal.Namespace,
                                                ZptConstants.Metal.DefineSlotAttribute))
        .Returns(new [] {
          Mock.Of<IZptElement>(e => e.GetAttribute(ZptConstants.Metal.Namespace,
                                                   ZptConstants.Metal.DefineSlotAttribute).Value == "found1"),
          Mock.Of<IZptElement>(e => e.GetAttribute(ZptConstants.Metal.Namespace,
                                                   ZptConstants.Metal.DefineSlotAttribute).Value == "found2"),
          Mock.Of<IZptElement>(e => e.GetAttribute(ZptConstants.Metal.Namespace,
                                                   ZptConstants.Metal.DefineSlotAttribute).Value == "unique"),
        });


      var contexts = macroElements.Select(x => Mock.Of<IRenderingContext>(c => c.Element == x.Object)).ToList();


      // Act
      var result = _sut.GetDefinedSlots(contexts, new [] { "found1", "found2" });

      // Assert
      Assert.AreEqual(1, result.Count, "Count of results");
      Assert.AreEqual("unique", result.Keys.Single(), "Only result found");
    }

    [Test]
    public void GetDefinedSlots_from_parent_uses_slots_from_bottom_of_stack_in_preference_to_top_of_stack()
    {
      // Arrange
      var macroElements = new [] {
        new Mock<IZptElement>(),
        new Mock<IZptElement>(),
      };
      var slotElements = new [] {
        Mock.Of<IZptElement>(e => e.GetAttribute(ZptConstants.Metal.Namespace,
                                                 ZptConstants.Metal.DefineSlotAttribute).Value == "name"),
        Mock.Of<IZptElement>(e => e.GetAttribute(ZptConstants.Metal.Namespace,
                                                 ZptConstants.Metal.DefineSlotAttribute).Value == "name"),
      };
      macroElements[0]
        .Setup(x => x.SearchChildrenByAttribute(ZptConstants.Metal.Namespace,
                                                ZptConstants.Metal.DefineSlotAttribute))
        .Returns(new [] { slotElements[0] });
      macroElements[1]
        .Setup(x => x.SearchChildrenByAttribute(ZptConstants.Metal.Namespace,
                                                ZptConstants.Metal.DefineSlotAttribute))
        .Returns(new [] { slotElements[1] });

      var contexts = macroElements.Select(x => Mock.Of<IRenderingContext>(c => c.Element == x.Object)).ToList();

      // Act
      var result = _sut.GetDefinedSlots(contexts, Enumerable.Empty<string>());

      // Assert
      Assert.AreSame(slotElements[0], result.Values.Single());
    }

    [Test]
    public void GetSlotsToFill_query_does_not_find_unfilled_slots()
    {
      // Arrange
      _contextOne
        .Setup(x => x.CreateSiblingContext(It.IsAny<IZptElement>(), It.IsAny<bool>()))
        .Returns((IZptElement ele, bool clone) => Mock.Of<IRenderingContext>(x => x.Element == ele));
      _contextTwo
        .Setup(x => x.CreateSiblingContext(It.IsAny<IZptElement>(), It.IsAny<bool>()))
        .Returns((IZptElement ele, bool clone) => Mock.Of<IRenderingContext>(x => x.Element == ele));

      var slots = new [] {
        new { Name = "foo", Element = Mock.Of<IZptElement>() },
        new { Name = "unfilled", Element = Mock.Of<IZptElement>() },
      }
        .ToDictionary(k => k.Name, v => v.Element);
      var fillers = new [] {
        new { Name = "foo", Element = Mock.Of<IZptElement>() },
      }
        .ToDictionary(k => k.Name, v => v.Element);

      var source = _contextTwo.Object;
      var macro = _contextOne.Object;

      // Act
      var result = _sut.GetSlotsToFill(source, fillers, macro, slots);

      // Assert
      Assert.AreEqual(1, result.Count(), "Count of results");
      Assert.AreEqual("foo", result.Single().Name, "Name of filled slot");
    }

    [Test]
    public void GetSlotsToFill_query_does_not_find_fillers_with_no_corresponding_definition()
    {
      // Arrange
      _contextOne
        .Setup(x => x.CreateSiblingContext(It.IsAny<IZptElement>(), It.IsAny<bool>()))
        .Returns((IZptElement ele, bool clone) => Mock.Of<IRenderingContext>(x => x.Element == ele));
      _contextTwo
        .Setup(x => x.CreateSiblingContext(It.IsAny<IZptElement>(), It.IsAny<bool>()))
        .Returns((IZptElement ele, bool clone) => Mock.Of<IRenderingContext>(x => x.Element == ele));

      var slots = new [] {
        new { Name = "foo", Element = Mock.Of<IZptElement>() },
      }
        .ToDictionary(k => k.Name, v => v.Element);
      var fillers = new [] {
        new { Name = "foo", Element = Mock.Of<IZptElement>() },
        new { Name = "undefined", Element = Mock.Of<IZptElement>() },
      }
        .ToDictionary(k => k.Name, v => v.Element);

      var source = _contextTwo.Object;
      var macro = _contextOne.Object;

      // Act
      var result = _sut.GetSlotsToFill(source, fillers, macro, slots);

      // Assert
      Assert.AreEqual(1, result.Count(), "Count of results");
      Assert.AreEqual("foo", result.Single().Name, "Name of filled slot");
    }

    [Test]
    public void GetSlotsToFill_query_matches_slots_with_fillers()
    {
      // Arrange
      _contextOne
        .Setup(x => x.CreateSiblingContext(It.IsAny<IZptElement>(), It.IsAny<bool>()))
        .Returns((IZptElement ele, bool clone) => Mock.Of<IRenderingContext>(x => x.Element == ele));
      _contextTwo
        .Setup(x => x.CreateSiblingContext(It.IsAny<IZptElement>(), It.IsAny<bool>()))
        .Returns((IZptElement ele, bool clone) => Mock.Of<IRenderingContext>(x => x.Element == ele));

      var slots = new [] {
        new { Name = "foo", Element = Mock.Of<IZptElement>() },
        new { Name = "bar", Element = Mock.Of<IZptElement>() },
      }
        .ToDictionary(k => k.Name, v => v.Element);
      var fillers = new [] {
        new { Name = "bar", Element = Mock.Of<IZptElement>() },
        new { Name = "foo", Element = Mock.Of<IZptElement>() },
      }
        .ToDictionary(k => k.Name, v => v.Element);

      var source = _contextTwo.Object;
      var macro = _contextOne.Object;

      // Act
      var result = _sut.GetSlotsToFill(source, fillers, macro, slots);

      // Assert
      Assert.AreEqual(2, result.Count(), "Count of results");
      Assert.That(result.Any(x => x.Name == "foo" && x.Slot.Element == slots["foo"] && x.Filler.Element == fillers["foo"]),
                  "Slot 'foo' is found");
      Assert.That(result.Any(x => x.Name == "bar" && x.Slot.Element == slots["bar"] && x.Filler.Element == fillers["bar"]),
                  "Slot 'bar' is found");
    }

    [Test]
    public void ReplaceMacroElement_replaces_the_source_context()
    {
      // Arrange
      var source = _contextOne.Object;
      var macro = _contextTwo.Object;
      var sourceElement = _elementOne.Object;
      var macroElement = _elementTwo.Object;

      // Act
      _sut.ReplaceMacroElement(source, macro);

      // Assert
      _elementOne.Verify(x => x.ReplaceWith(macroElement), Times.Once());
    }

    [Test]
    public void ReplaceMacroElement_returns_the_replacement_result()
    {
      // Arrange
      var source = _contextOne.Object;
      var macro = _contextTwo.Object;
      var sourceElement = _elementOne.Object;
      var macroElement = _elementTwo.Object;
      var replacementElement = _elementThree.Object;

      _elementOne.Setup(x => x.ReplaceWith(macroElement)).Returns(replacementElement);
      _contextOne
        .Setup(x => x.CreateSiblingContext(It.IsAny<IZptElement>(), It.IsAny<bool>()))
        .Returns((IZptElement ele, bool clone) => Mock.Of<IRenderingContext>(x => x.Element == ele));

      // Act
      var result = _sut.ReplaceMacroElement(source, macro);

      // Assert
      Assert.AreSame(replacementElement, result.Element);
    }

    [Test]
    public void ReplaceMacroElement_marks_the_element_as_imported()
    {
      // Arrange
      var source = _contextOne.Object;
      var macro = _contextTwo.Object;
      var sourceElement = _elementOne.Object;
      var macroElement = _elementTwo.Object;
      var replacementElement = _elementThree.Object;

      _elementThree
        .Setup(x => x.SetAttribute(ZptConstants.SourceAnnotation.Namespace,
                                   ZptConstants.SourceAnnotation.ElementIsImported,
                                   Boolean.TrueString));

      // Act
      _sut.ReplaceMacroElement(source, macro);

      // Assert
      _elementThree
        .Verify(x => x.SetAttribute(ZptConstants.SourceAnnotation.Namespace,
                                    ZptConstants.SourceAnnotation.ElementIsImported,
                                    Boolean.TrueString),
                Times.Once());
    }

    #endregion
  }
}

