using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Context visitor that tidies up TAL elements, removing any elements or attributes that should not be rendered.
  /// </summary>
  public class TalTidyUpVisitor : NoOpVisitor
  {
    #region overrides

    /// <summary>
    /// Visits a rendering context and returns a collection of contexts which represent the result of that visit.
    /// </summary>
    /// <returns>The rendering contexts instances which are exposed after the visiting process is complete.</returns>
    /// <param name="context">The rendering context to visit.</param>
    public override IRenderingContext[] VisitContext(IRenderingContext context)
    {
      context.Element.PurgeTalElements();
      context.Element.PurgeTalAttributes();

      return new [] { context };
    }

    #endregion
  }
}

