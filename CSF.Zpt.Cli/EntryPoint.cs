using System;
using System.Linq;
using System.Text;
using CSF.Cli;
using CSF.Zpt.Rendering;
using CSF.Zpt.BatchRendering;

namespace CSF.Zpt.Cli
{
  /// <summary>
  /// The application entry point.
  /// </summary>
  public class EntryPoint
  {
    #region static methods

    /// <summary>
    /// The entry point of the program, where the program control starts and ends.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
      var app = new Application(args);
      app.Begin();
    }

    #endregion
  }
}

