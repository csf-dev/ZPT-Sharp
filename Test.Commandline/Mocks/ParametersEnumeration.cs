using System;

namespace CraigFowler.Cli.Mocks
{
  public enum ParametersEnumeration : int
  {
    [Parameter(Type = ParameterType.ValueRequired, ValueType = typeof(string))]
    [ParameterLongNames("action")]
    [ParameterShortNames("A")]
    Action,
    
    [Parameter(Type = ParameterType.NoValue)]
    [ParameterLongNames("verbose")]
    [ParameterShortNames("v")]
    Verbose,
    
    [Parameter(Type = ParameterType.NoValue)]
    [ParameterLongNames("help")]
    [ParameterShortNames("h")]
    Help,
    
    [Parameter(Type = ParameterType.ValueOptional, ValueType = typeof(int))]
    [ParameterLongNames("count")]
    Count,
    
    [Parameter(Type = ParameterType.ValueOptional, ValueType = typeof(bool))]
    [ParameterLongNames("explode")]
    Explode
  }
}

