using CSF;
using System.Reflection;

namespace CSF.Zpt.Cli
{
  /// <summary>
    /// Service which gets the current version information for the ZptBuilder application.
    /// </summary>
  public class VersionNumberInspector : IVersionNumberInspector
  {
      #region methods

      /// <summary>
      /// Gets the version information for the ZptBuilder application.
      /// </summary>
      /// <returns>A string representing the version ZptBuilder version.</returns>
      public virtual string GetZptBuilderVersion()
      {
          return Assembly.GetExecutingAssembly().GetName().Version.ToSemanticVersion();
      }

      #endregion
  }
}