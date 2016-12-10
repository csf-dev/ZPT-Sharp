using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  public interface IMacroExtender
  {
    IRenderingContext Extend(IRenderingContext macro, IRenderingContext extendedMacro);
  }
}

