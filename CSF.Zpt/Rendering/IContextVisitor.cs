using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Interface for a type which can visit a <see cref="RenderingContext"/> instance.
  /// </summary>
  public interface IContextVisitor
  {
    /// <summary>
    /// Visits a rendering context and returns a collection of contexts which represent the result of that visit.
    /// </summary>
    /// <returns>The rendering contexts instances which are exposed after the visiting process is complete.</returns>
    /// <param name="context">The rendering context to visit.</param>
    RenderingContext[] VisitContext(RenderingContext context);
  }
}

