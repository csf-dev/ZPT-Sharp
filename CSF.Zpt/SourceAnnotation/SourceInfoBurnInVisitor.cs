using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.SourceAnnotation
{
  /// <summary>
  /// Implementation of <see cref="IContextVisitor"/> which tells the <see cref="IZptElement"/> to burn its source
  /// information into the element attributes.
  /// </summary>
  public class SourceInfoBurnInVisitor : NoOpVisitor
  {
    /// <summary>
    /// Visit the given context and return a collection of the resultant contexts.
    /// </summary>
    /// <param name="context">Context.</param>
    public override IRenderingContext[] Visit(IRenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      context.Element.CacheSourceInformationInAttributes();

      return base.Visit(context);
    }
  }
}

