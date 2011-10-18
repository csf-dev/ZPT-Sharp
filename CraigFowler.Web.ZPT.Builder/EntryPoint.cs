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
      BuilderParameters parameters = ParameterParser.Parse<BuilderParameters>(commandLine,
                                                                              ParameterStyle.Unix,
                                                                              typeof(BuilderParameterType));
      
      
      ZptPageBuilder builder = parameters.ZptPageBuilderFactory();
      builder.BuildPages();
    }
    
    #endregion
  }
}

