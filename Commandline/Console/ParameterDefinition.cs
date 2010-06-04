
using System;
using System.Collections.Generic;

namespace CraigFowler.Console
{
  public class ParameterDefinition
  {
    #region fields
    
    private string name;
    private List<string> shortNames;
    private List<string> longNames;
    private ParameterType type;
    
    #endregion
    
    #region properties
    
    public string Name
    {
      get {
        return name;
      }
      set {
        if(String.IsNullOrEmpty(value))
        {
          throw new ArgumentException("Name cannot be null or empty", "value");
        }
        
        name = value;
      }
    }
    
    public ParameterType Type
    {
      get {
        return type;
      }
      set {
        type = value;
      }
    }
    
    public List<string> ShortNames
    {
      get {
        return shortNames;
      }
    }

    public List<string> LongNames
    {
      get {
        return longNames;
      }
    }
    
    #endregion
    
    #region constructor
    
    public ParameterDefinition(string title)
    {
      shortNames = new List<string>();
      longNames = new List<string>();
      type = 0;
      
      Name = title;
    }
    
    public ParameterDefinition(string title, IEnumerable<string> longTitles) : this(title)
    {
      if(longTitles != null)
      {
        foreach(string text in longTitles)
        {
          LongNames.Add(text);
        }
      }
    }
    
    public ParameterDefinition(string title,
                               IEnumerable<string> longTitles,
                               IEnumerable<string> shortTitles) : this(title, longTitles)
    {
      if(shortTitles != null)
      {
        foreach(string text in shortTitles)
        {
          ShortNames.Add(text);
        }
      }
    }
    
    #endregion
  }
}
