using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  public interface IMacroSubstituter
  {
    IRenderingContext MakeSubstitutions(IRenderingContext sourceContext, IRenderingContext macroContext);
  }
}

