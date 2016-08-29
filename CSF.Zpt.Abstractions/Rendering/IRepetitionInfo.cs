using System;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents information about a repetition.
  /// </summary>
  public interface IRepetitionInfo : ITalesPathHandler
  {
    /// <summary>
    /// Gets a reference to the associated <see cref="ZptElement"/>, if applicable.
    /// </summary>
    /// <value>The associated element.</value>
    ZptElement AssociatedElement { get; }

    /// <summary>
    /// Gets the value associated with the current iteration.
    /// </summary>
    /// <value>The value.</value>
    object Value { get; }

    /// <summary>
    /// Gets the name of the repetition.
    /// </summary>
    /// <value>The name.</value>
    string Name { get; }
  }
}

