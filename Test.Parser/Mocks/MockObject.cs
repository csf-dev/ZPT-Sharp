
using System;

namespace Test.CraigFowler.Web.ZPT.Mocks
{
  public class MockObject
  {
    public MockObject InnerObject;
    public int IntegerValue;
    
    public MockObject() : this(false) {}
    
    public MockObject(bool createInner)
    {
      this.InnerObject = createInner? new MockObject() : null;
      this.IntegerValue = 0;
    }
  }
}
