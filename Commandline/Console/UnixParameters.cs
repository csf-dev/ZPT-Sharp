
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CraigFowler.Console
{
  public class UnixParameters : ParameterProcessorBase
  {
    #region fields
    
    private const string
      LONG_PARAMETER_PATTERN            = @"^--([A-Za-z0-9_-]{2,})$",
      SHORT_PARAMETER_PATTERN           = @"^-([A-Za-z0-9])$";
    
    private static readonly Regex
      LongParameter                     = new Regex(LONG_PARAMETER_PATTERN, RegexOptions.Compiled),
      ShortParameter                    = new Regex(SHORT_PARAMETER_PATTERN, RegexOptions.Compiled);
    
    #endregion
    
    #region methods
    
    public override Dictionary<string, string> GetParameters (string[] commandlineInput, out string[] remainingInput)
    {
      Dictionary<string, ParameterDefinition> longParams, shortParams;
      Dictionary<string, string> output = new Dictionary<string, string>();
      List<string> remaining = new List<string>();
      ParameterDefinition lastParameter = null;
      Match longMatch, shortMatch;
      
      NormaliseParameterList(out longParams, out shortParams);
      
      foreach (string item in commandlineInput)
      {
        if(lastParameter != null && lastParameter.Type == ParameterType.ValueRequired)
        {
          output.Add(lastParameter.Name, item);
          lastParameter = null;
        }
        else if(lastParameter != null && lastParameter.Type == ParameterType.ValueOptional)
        {
          throw new NotImplementedException("Optional values are not yet supported");
        }
        else
        {
          if(lastParameter != null)
          {
            output.Add(lastParameter.Name, null);
            lastParameter = null;
          }
          
          longMatch = LongParameter.Match(item);
          shortMatch = ShortParameter.Match(item);
          
          if(longMatch.Success && longParams.ContainsKey(longMatch.Groups[1].Value))
          {
            lastParameter = longParams[longMatch.Groups[1].Value];
          }
          else if(shortMatch.Success && shortParams.ContainsKey(shortMatch.Groups[1].Value))
          {
            lastParameter = shortParams[shortMatch.Groups[1].Value];
          }
          else
          {
            remaining.Add(item);
          }
        }
      }
      
      if(lastParameter != null)
      {
        output.Add(lastParameter.Name, null);
        lastParameter = null;
      }
      
      remainingInput = remaining.ToArray();
      return output;
    }
    
    #endregion
    
    #region constructor
    
    public UnixParameters() : base() {}
    
    #endregion
  }
}
