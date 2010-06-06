
using System;
using System.Collections.Generic;
using CraigFowler.Web.ZPT.Tales;

namespace CraigFowler.Web.ZPT.Mocks
{
  public class MockObject
  {
    #region private fields
    
    private Dictionary<string, string> dict;
    
    #endregion
    
    #region public fields
    
    [TalesAlias("inner")]
    public MockObject InnerObject;
    
    public int IntegerValue;
    
    #endregion
    
    #region properties
    
    public string this [string key]
    {
      get {
        string output;
        
        if(dict.ContainsKey(key))
        {
          output = dict[key];
        }
        else
        {
          output = null;
        }
        
        return output;
      }
      set {
        if(dict.ContainsKey(key))
        {
          dict[key] = value;
        }
        else
        {
          dict.Add(key, value);
        }
      }
    }
    
    #endregion
    
    #region constructors
    
    public MockObject() : this(false) {}
    
    public MockObject(bool createInner)
    {
      this.InnerObject = createInner? new MockObject() : null;
      this.IntegerValue = 0;
      
      dict = new Dictionary<string, string>();
      this["foo"] = "bar";
      this["baz"] = "sample";
    }
    
    #endregion
  }
}
