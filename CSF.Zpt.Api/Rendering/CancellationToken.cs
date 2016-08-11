using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Simple type which indicates that the action should always be cancelled.
  /// </summary>
  public class CancellationToken : IActionCanceller
  {
    /// <summary>
    /// Determines whether or not the current instance indicates that the action should be cancelled.
    /// </summary>
    /// <returns><c>true</c>, if the action should be cancelled, <c>false</c> otherwise.</returns>
    public bool ShouldCancelAction() { return true; }
  }
}

