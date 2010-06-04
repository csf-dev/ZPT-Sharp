
using System;
using System.Collections.Generic;

namespace CraigFowler.Console
{
  public abstract class ParameterProcessorBase
  {
    #region fields
    
    private List<ParameterDefinition> definitions;
    
    #endregion
    
    #region properties
    
    public List<ParameterDefinition> Definitions
    {
      get {
        return definitions;
      }
    }
    
    #endregion
    
    #region public methods
    
    public abstract Dictionary<string, string> GetParameters(string[] commandlineInput, out string[] remainingInput);
    
    #endregion
    
    #region protected methods
    
    protected void NormaliseParameterList(out Dictionary<string, ParameterDefinition> longs,
                                          out Dictionary<string, ParameterDefinition> shorts)
    {
      longs = new Dictionary<string, ParameterDefinition>();
      shorts = new Dictionary<string, ParameterDefinition>();
      
      foreach(ParameterDefinition definition in definitions)
      {
        foreach(string longName in definition.LongNames)
        {
          longs.Add(longName, definition);
        }
        foreach(string shortName in definition.ShortNames)
        {
          shorts.Add(shortName, definition);
        }
      }
      
      return;
    }
    
    #endregion
    
    #region constructors
    
    public ParameterProcessorBase()
    {
      definitions = new List<ParameterDefinition>();
    }
    
    #endregion
  }
}
