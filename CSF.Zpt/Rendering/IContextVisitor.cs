using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Interface for a type which can visit a <see cref="RenderingContext"/> instance.
  /// </summary>
  public interface IContextVisitor
  {
    /// <summary>
    /// Visit the given context, as well as its child contexts, and return a collection of the resultant contexts.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This operation performs the same work as <see cref="Visit"/>, but it then visits all of the resultant contexts,
    /// recursively moving down the exposed document tree, visiting each context in turn.
    /// </para>
    /// </remarks>
    /// <returns>Zero or more <see cref="RenderingContext"/> instances, determined by the outcome of this visit.</returns>
    /// <param name="context">The rendering context to visit.</param>
    RenderingContext[] VisitRecursively(RenderingContext context);
  }
}

