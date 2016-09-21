using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Interface for a type which may 'cancel an action'.  The meaning of this is dependant upon the operation being
  /// performed, but generally it will mean that no change is made to an existing HTML structure.
  /// </summary>
  public interface IActionCanceller
  {
    /// <summary>
    /// Determines whether or not the current instance indicates that the action should be cancelled.
    /// </summary>
    /// <returns><c>true</c>, if the action should be cancelled, <c>false</c> otherwise.</returns>
    bool ShouldCancelAction();
  }
}

