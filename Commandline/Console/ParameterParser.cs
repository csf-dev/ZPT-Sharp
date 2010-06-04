
using System;
using System.Collections.Generic;

namespace CraigFowler.Console
{
  /// <summary>
  /// <para>Provides parsing mechanisms for commandline parameters.</para>
  /// </summary>
  public class ParameterParser
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
    
    #region constructors
    
    /// <summary>
    /// <para>Initialises this instance with default values.</para>
    /// </summary>
    private ParameterParser()
    {
      plugin = null;
      parameters = null;
      remainingText = null;
      inputParameters = null;
    }
    
    /// <summary>
    /// <para>Initialises this instance for the given style of parameters.</para>
    /// </summary>
    /// <param name="type">
    /// A <see cref="ParameterStyle"/>
    /// </param>
    public ParameterParser(ParameterStyle type) : this()
    {
      switch (type)
      {
      case ParameterStyle.Unix:
        plugin = new UnixParameters();
        break;
      default:
        throw new NotSupportedException("Selected parameter style not supported yet");
      }
    }
    
    /// <summary>
    /// <para>
    /// Initialises this instance with the given style of parameters and a string array of commandline parameters.
    /// </para>
    /// </summary>
    /// <param name="type">
    /// A <see cref="ParameterStyle"/>
    /// </param>
    /// <param name="commandlineText">
    /// A <see cref="System.String[]"/>
    /// </param>
    public ParameterParser(ParameterStyle type, string[] commandlineText) : this(type)
    {
      this.Commandline = commandlineText;
    }
    
    /// <summary>
    /// <para>
    /// Initialises this instance with the given style of parameters and a string that represents the commandline
    /// parameters.
    /// </para>
    /// </summary>
    /// <param name="type">
    /// A <see cref="ParameterStyle"/>
    /// </param>
    /// <param name="commandLineText">
    /// A <see cref="System.String"/>
    /// </param>
    public ParameterParser(ParameterStyle type, string commandLineText) : this(type)
    {
      if(commandLineText == null)
      {
        throw new ArgumentNullException("commandLineText");
      }
      
      this.Commandline = commandLineText.Split(new char[] {' '});
    }
    
    #endregion
  }
}
