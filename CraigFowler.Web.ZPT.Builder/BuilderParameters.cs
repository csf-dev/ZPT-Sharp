using System;
using CraigFowler.Cli;
using System.Collections.Generic;
using CraigFowler.Web.ZPT;

namespace CraigFowler.Web.ZPT.Builder
{
  /// <summary>
  /// <para>
  /// Represents the parameters that have been received on the commandline by the builder application.  This is
  /// populated by an <see cref="IParameterParser"/>.
  /// </para>
  /// </summary>
  public class BuilderParameters : ParsedParameters
  {
    #region parameter properties
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets the value for the input path argument.  This is the first non-parameter argument on the
    /// commandline.
    /// </para>
    /// </summary>
    public string InputPath
    {
      get {
        string output = null;
        
        IList<string> args = this.GetRemainingArguments();
        
        if(args.Count > 0)
        {
          output = args[0];
        }
        
        return output;
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the input filename pattern parameter value.</para>
    /// </summary>
    public string InputFilenamePattern
    {
      get {
        return this.GetValue(BuilderParameterType.InputFilenamePattern);
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the output path parameter value.</para>
    /// </summary>
    public string OutputPath
    {
      get {
        return this.GetValue(BuilderParameterType.OutputPath);
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the output filename extension parameter value.</para>
    /// </summary>
    public string OutputFileExtension
    {
      get {
        return this.GetValue(BuilderParameterType.OutputFileExtension);
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the ignore marker filename parameter value.</para>
    /// </summary>
    public string IgnoreMarkerFilename
    {
      get {
        return this.GetValue(BuilderParameterType.IgnoreMarkerFilename);
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the option definitions parameter value.</para>
    /// </summary>
    public string OptionDefinitions
    {
      get {
        return this.GetValue(BuilderParameterType.OptionDefinitions);
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Factory method creates a new page builder instance from the parameters stored in this instance.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="ZptPageBuilder"/>
    /// </returns>
    public ZptPageBuilder ZptPageBuilderFactory()
    {
      ZptPageBuilder output;
      
      if(this.InputPath == null)
      {
        throw new InvalidOperationException("Input path is null");
      }
      
      output = new ZptPageBuilder(this.InputPath);
      
      if(this.IgnoreMarkerFilename != null)
      {
        output.IgnoreMarkerFilename = this.IgnoreMarkerFilename;
      }
      
      if(this.InputFilenamePattern != null)
      {
        output.InputFilenamePattern = this.InputFilenamePattern;
      }
      
      if(this.OutputFileExtension != null)
      {
        output.OutputFilenameExtension = this.OutputFileExtension;
      }
      
      if(this.OutputPath != null)
      {
        output.OutputPath = this.OutputPath;
      }
      
      Dictionary<string, object> commandlineVariableAssignments = this.ParseCommandlineVariables();
      foreach(string key in commandlineVariableAssignments.Keys)
      {
        output.CommandLineOptions.Add(key, commandlineVariableAssignments[key]);
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Helper method used within this type to get parameter values or to return null if a parameter
    /// is not present.
    /// </para>
    /// </summary>
    /// <param name="parameterType">
    /// A <see cref="BuilderParameterType"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.String"/>
    /// </returns>
    private string GetValue(BuilderParameterType parameterType)
    {
      string output = null;
      
      if(this.HasParameter(parameterType))
      {
        output = this.GetValue<string>(parameterType);
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Parses the <see cref="OptionDefinitions"/> and returns them as a dictionary of string and object.</para>
    /// </summary>
    /// <returns>
    /// A dictionary of <see cref="System.String"/> and <see cref="System.Object"/>
    /// </returns>
    private Dictionary<string, object> ParseCommandlineVariables()
    {
      Dictionary<string, object> output = new Dictionary<string, object>();
      
      if(!String.IsNullOrEmpty(this.OptionDefinitions))
      {
        List<string> allOptions = new List<string>();
        allOptions.AddRange(this.OptionDefinitions.Split(';'));
        
        foreach(string optionDefinition in allOptions)
        {
          string[] optionParts = optionDefinition.Split(new char[] {'='}, 2);
          
          if(optionParts.Length == 2)
          {
            output.Add(optionParts[0], optionParts[1]);
          }
          else
          {
            output.Add(optionParts[0], true);
          }
        }
      }
      
      return output;
    }
    
    #endregion
  }
}

