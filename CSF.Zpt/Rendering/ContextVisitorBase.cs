﻿using System;
using System.Linq;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Abstract base logic for implementations of <see cref="IContextVisitor"/>.
  /// </summary>
  public abstract class ContextVisitorBase : IContextVisitor
  {
    #region methods

    /// <summary>
    /// Visit the given context and return a collection of the resultant contexts.
    /// </summary>
    /// <returns>Zero or more <see cref="IRenderingContext"/> instances, determined by the outcome of this visit.</returns>
    /// <param name="context">The rendering context to visit.</param>
    public abstract IRenderingContext[] Visit(IRenderingContext context);

    /// <summary>
    /// Visit the given context, as well as its child contexts, and return a collection of the resultant contexts.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This operation performs the same work as <see cref="Visit"/>, but it then visits all of the resultant contexts,
    /// recursively moving down the exposed document tree, visiting each context in turn.
    /// </para>
    /// </remarks>
    /// <returns>Zero or more <see cref="IRenderingContext"/> instances, determined by the outcome of this visit.</returns>
    /// <param name="context">The rendering context to visit.</param>
    public virtual IRenderingContext[] VisitRecursively(IRenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      var output = this.Visit(context);

      var children = output.SelectMany(x => x.GetChildContexts());
      foreach(var child in children)
      {
        this.VisitRecursively(child);
      }

      return output;
    }

    /// <summary>
    /// Visits a rendering context and returns a collection of contexts which represent the result of that visit.
    /// </summary>
    /// <returns>The rendering contexts instances which are exposed after the visiting process is complete.</returns>
    /// <param name="context">The rendering context to visit.</param>
    public virtual IRenderingContext[] VisitContext(IRenderingContext context)
    {
      return this.VisitRecursively(context);
    }

    #endregion
  }
}
