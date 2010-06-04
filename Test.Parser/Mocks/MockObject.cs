
using System;
using System.Collections.Generic;

namespace Test.CraigFowler.Web.ZPT.Mocks
{
  public class MockObject
  {
    #region private fields
    
    private Dictionary<string, string> dict;
    
    #endregion
    
    #region public fields
    
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
      private set {
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
    
    
    public MockObject() : this(false) {}
    
    public MockObject(bool createInner)
    {
      this.InnerObject = createInner? new MockObject() : null;
      this.IntegerValue = 0;
      
      dict = new Dictionary<string, string>();
      this["foo"] = "bar";
      this["baz"] = "sample";
    }
  }
}
