
using System;
using System.Collections.Generic;

namespace CraigFowler
{
  public class CommandlineParameterProcessor
  {
    #region fields
    
    private ParameterProcessorBase plugin;
    private Dictionary<string, string> parameters;
    private string[] inputParameters, remainingText;
    
    #endregion
    
    #region properties
    
    public string[] Commandline
    {
      get {
        return inputParameters;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        
        inputParameters = value;
      }
    }
    
    public Dictionary<string, string> Parameters {
      get {
        if(parameters == null)
        {
          parseParameters();
        }
        
        return parameters;
      }
    }
    
    public string[] RemainingText {
      get {
        if(parameters == null)
        {
          parseParameters();
        }
        
        return remainingText;
      }
    }
    
    public List<ParameterDefinition> Definitions
    {
      get {
        return plugin.Definitions;
      }
    }
    
    #endregion
    
    #region methods
    
    private void parseParameters()
    {
      if(Commandline == null)
      {
        throw new InvalidOperationException("Commandline parameters are null");
      }
      else
      {
        parameters = plugin.GetParameters(Commandline, out remainingText);
      }
    }
    
    #endregion
    
    private CommandlineParameterProcessor()
    {
      plugin = null;
      parameters = null;
      remainingText = null;
      inputParameters = null;
    }
    
    public CommandlineParameterProcessor(ParameterStyle type) : this()
    {
      switch (type)
      {
      case ParameterStyle.Unix:
        plugin = new UnixParameters();
        break;
      default:
        throw new NotImplementedException("Selected parameter style not supported yet");
      }
    }
    
    public CommandlineParameterProcessor(ParameterStyle type, string[] commandlineText) : this(type)
    {
      Commandline = commandlineText;
    }
  }
}
