using System;

namespace CSF.Zpt.ExpressionEvaluators
{
  /// <summary>
  /// Non-generic abstract base class representing a wrapper by which a reflection-based reference may be traversed.
  /// </summary>
  public abstract class TraversalWrapper
  {
    /// <summary>
    /// Traverse the specified object, optionally using a value, and gets the result.
    /// </summary>
    /// <returns>The result of the traversal.</returns>
    /// <param name="source">Source.</param>
    /// <param name="value">Value.</param>
    public abstract object Traverse(object source, string value);
  }
}

