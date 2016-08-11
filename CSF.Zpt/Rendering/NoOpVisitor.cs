using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Implementation of <see cref="ContextVisitorBase"/> that performs no operation.
  /// </summary>
  public class NoOpVisitor : ContextVisitorBase
  {
    #region overrides

    /// <summary>
    /// Visit the given context and return a collection of the resultant contexts.
    /// </summary>
    /// <param name="context">Context.</param>
    public override RenderingContext[] Visit(RenderingContext context)
    {
      // Intentional no-op
      return new [] { context };
    }

    #endregion
  }
}

