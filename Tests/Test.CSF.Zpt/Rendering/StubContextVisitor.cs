﻿using System;
using CSF.Zpt.Rendering;

namespace Test.CSF.Zpt.Rendering
{
  public class StubContextVisitor : IContextVisitor
  {
    public IRenderingContext[] VisitContext(IRenderingContext context)
    {
      throw new NotImplementedException();
    }
  }

  public class AnotherStubContextVisitor : IContextVisitor
  {
    public IRenderingContext[] VisitContext(IRenderingContext context)
    {
      throw new NotImplementedException();
    }
  }
}
