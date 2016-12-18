using System;
using CSF.Zpt.Rendering;

namespace Test.CSF.Zpt.Rendering
{
  public class StubRenderingContextFactory : IRenderingContextFactory
  {
    public IRenderingContext Create(IZptElement element, IRenderingSettings options)
    {
      throw new NotSupportedException();
    }

    public IRenderingContext Create(IZptElement element, IRenderingSettings options, object model)
    {
      throw new NotSupportedException();
    }

    public void AddKeywordOption(string key, string value)
    {
      throw new NotSupportedException();
    }
  }
}

