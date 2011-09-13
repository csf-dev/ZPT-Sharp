using System;
using CraigFowler.Cli;

namespace CraigFowler.Web.ZPT.Builder
{
  /// <summary>
  /// <para>Main entry point class for the ZPT page builder commandline application.</para>
  /// </summary>
  public class EntryPoint
  {
    #region static entry point
    
    /// <summary>
    /// <para>Static entry point method for the ZPT page builder commandline application.</para>
    /// </summary>
    /// <param name="commandLine">
    /// A <see cref="System.String[]"/>
    /// </param>
    public static void Main(string[] commandLine)
    {
      BuilderParameters parameters;
      ParsedParameters castParams;
      IParameterParser parameterParser;
      ZptPageBuilder builder;
      
      parameterParser = new UnixParameters();
      parameterParser.RegisterParameters(typeof(BuilderParameterType));
      
      parameters = new BuilderParameters();
      castParams = parameters;
      parameterParser.Parse(commandLine, ref castParams);
      
      builder = parameters.ZptPageBuilderFactory();
      builder.BuildPages();
    }
    
    #endregion
  }
}

