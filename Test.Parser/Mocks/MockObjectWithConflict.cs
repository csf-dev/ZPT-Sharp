
using System;
using CraigFowler.Web.ZPT.Tales;

namespace CraigFowler.Web.ZPT.Mocks
{
  public class MockObjectWithConflict
  {
    public string SomeProperty;
    
    [TalesAlias("SomeProperty")]
    public string OtherProperty;
    
    [TalesAlias("Duplicate")]
    public string SomeDuplicate;
    
    [TalesAlias("Duplicate")]
    public string OtherDuplicate;
    
    public string this [string key]
    {
      get {
        return "foo";
      }
      set { }
    }
    
    public MockObjectWithConflict()
    {
      this.SomeProperty = "foo";
      this.OtherProperty = "bar";
      
      this.SomeDuplicate = "baz";
      this.OtherDuplicate = "sample";
    }
  }
}
