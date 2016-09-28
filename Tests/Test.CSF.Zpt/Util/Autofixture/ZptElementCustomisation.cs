using System;
using Ploeh.AutoFixture;
using Moq;
using CSF.Zpt.Rendering;

namespace Test.CSF.Zpt.Util.Autofixture
{
  public class ZptElementCustomisation : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      fixture.Customize<ZptElement>(x => x.FromFactory(() => {
        var output = new Mock<ZptElement>() { CallBase = true };

        output
          .Setup(e => e.Equals(It.IsAny<object>()))
          .Returns((object obj) => Object.ReferenceEquals(output.Object, obj));
        output
          .Setup(e => e.Equals(It.IsAny<ZptElement>()))
          .Returns((ZptElement obj) => Object.ReferenceEquals(output.Object, obj));
        output
          .Setup(e => e.GetHashCode())
          .Returns(5);

        return output.Object;
      }));
    }
  }
}

