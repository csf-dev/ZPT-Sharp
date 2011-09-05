
using System;

namespace CraigFowler.Cli
{
  /// <summary>
  /// <para>Enumerates the various styles of creating and parsing commandline parameters.</para>
  /// </summary>
  [Flags]
  public enum ParameterStyle : int
  {
    /// <summary>
    /// <para>Unix-style parameters.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// These parameters use single dashes to indicate short parameters and double-dashes to indicate long parameters.
    /// </para>
    /// </remarks>
    Unix            = 1,
    
    /// <summary>
    /// <para>Windows-style parameters.</para>
    /// </summary>
    Windows         = 2
  }
}
