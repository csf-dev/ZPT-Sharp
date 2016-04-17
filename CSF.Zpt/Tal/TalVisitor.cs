using System;
using System.Linq;
using CSF.Zpt.Rendering;
using System.Collections.Generic;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Visitor type which is used to work upon an <see cref="ZptElement"/> and perform TAL-related functionality.
  /// </summary>
  public class TalVisitor : ElementVisitor
  {
    #region fields

    private IAttributeHandler[] _handlers;
    private IAttributeHandler _errorHandler;

    #endregion

    #region methods

    /// <summary>
    /// Visit the given element and perform modifications as required.
    /// </summary>
    /// <returns>A reference to the element which has been visited.  This might be the input <paramref name="element"/> or a replacement.</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="context">The rendering context provided to the visitor.</param>
    /// <param name="options">The rendering options to use.</param>
    public override RenderingContext[] Visit(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      IEnumerable<RenderingContext>
        output = new [] { context },
        newlyExposedElements = new RenderingContext[0];

      foreach(var handler in _handlers)
      {
        var handlingResult = (from ele in output
                              let processedBatch = handler.Handle(ele)
                              where processedBatch.ContinueHandling
                              select processedBatch);

        newlyExposedElements = newlyExposedElements.Union(handlingResult.SelectMany(x => x.NewlyExposedContexts));
        output = handlingResult.Where(x => x.ContinueHandling).SelectMany(x => x.Contexts);
      }

      output = output.Union(newlyExposedElements.SelectMany(x => this.Visit(x)));

      return output.ToArray();
    }

    /// <summary>
    /// Visits the given element, and then recursively visits all of its child elements.
    /// </summary>
    /// <returns>The recursively.</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="context">The rendering context provided to the visitor.</param>
    /// <param name="options">The rendering options to use.</param>
    public override RenderingContext[] VisitRecursively(RenderingContext context)
    {
      RenderingContext[] output;

      try
      {
        output = base.VisitRecursively(context);
      }
      catch(RenderingException ex)
      {
        output = new [] { context };

        if(context.Element.GetTalAttribute(ZptConstants.Tal.OnErrorAttribute) != null)
        {
          context.TalModel.AddError(ex);
          _errorHandler.Handle(context);
        }
        else
        {
          throw;
        }
      }

      return output;
    }

    /// <summary>
    /// Visits a root element and all of its child element.
    /// </summary>
    /// <returns>The root element(s), after the visiting process is complete.</returns>
    /// <param name="rootElement">Root element.</param>
    /// <param name="context">Context.</param>
    /// <param name="options">Options.</param>
    public override RenderingContext[] VisitContext(RenderingContext context)
    {
      var output = base.VisitContext(context);

      foreach(var item in output)
      {
        item.Element.PurgeTalAttributes();
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tal.TalVisitor"/> class.
    /// </summary>
    /// <param name="handlers">A collection of <see cref="IAttributeHandler"/> to process.</param>
    /// <param name="errorHandler">An <see cref="IAttributeHandler"/> which should be used to handle any errors.</param>
    public TalVisitor(IAttributeHandler[] handlers = null,
                      IAttributeHandler errorHandler = null)
    {
      _handlers = handlers?? new IAttributeHandler[] {
        new DefineAttributeHandler(),
        new ConditionAttributeHandler(),
        new RepeatAttributeHandler(),
        new ContentOrReplaceAttributeHandler(),
        new OmitTagAttributeHandler(),
        new AttributesAttributeHandler(),
      };

      _errorHandler = errorHandler?? new OnErrorAttributeHandler();
    }

    #endregion
  }
}

