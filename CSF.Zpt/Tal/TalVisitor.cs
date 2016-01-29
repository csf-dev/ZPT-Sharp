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
    public override ZptElement[] Visit(ZptElement element,
                                       RenderingContext context,
                                       RenderingOptions options)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }

      IEnumerable<ZptElement>
        output = new [] { element },
        newlyExposedElements = new ZptElement[0];

      foreach(var handler in _handlers)
      {
        var handlingResult = (from ele in output
                              let processedBatch = handler.Handle(ele, context.TalModel)
                              where processedBatch.ContinueHandling
                              select processedBatch);

        newlyExposedElements = newlyExposedElements.Union(handlingResult.SelectMany(x => x.NewlyExposedElements));
        output = handlingResult.Where(x => x.ContinueHandling).SelectMany(x => x.Elements);
      }

      output = output.Union(newlyExposedElements.SelectMany(x => this.Visit(x, context, options)));

      return output.ToArray();
    }

    /// <summary>
    /// Visits the given element, and then recursively visits all of its child elements.
    /// </summary>
    /// <returns>The recursively.</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="context">The rendering context provided to the visitor.</param>
    /// <param name="options">The rendering options to use.</param>
    public override ZptElement[] VisitRecursively(ZptElement element,
                                                  RenderingContext context,
                                                  RenderingOptions options)
    {
      ZptElement[] output;

      try
      {
        output = base.VisitRecursively(element, context, options);
      }
      catch(RenderingException ex)
      {
        output = new [] { element };

        if(element.GetTalAttribute(ZptConstants.Tal.OnErrorAttribute) != null)
        {
          context.TalModel.AddError(ex);
          _errorHandler.Handle(element, context.TalModel);
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
    public override ZptElement[] VisitRoot(ZptElement rootElement,
                                           RenderingContext context,
                                           RenderingOptions options)
    {
      var output = base.VisitRoot(rootElement, context, options);

      foreach(var item in output)
      {
        item.PurgeTalAttributes();
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

