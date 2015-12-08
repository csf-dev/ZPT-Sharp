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


      IEnumerable<AttributeHandlingResult> output = new [] { new AttributeHandlingResult(new [] { element }, true) };

      foreach(var handler in _handlers)
      {
        var elementsAfterProcessing = (from batch in output.Where(x => x.ContinueHandling)
                                       from ele in batch.Elements
                                       let handledBatch = handler.Handle(ele, context.TalModel)
                                       select handledBatch);

        output = output
          .Where(x => !x.ContinueHandling)
          .Union(elementsAfterProcessing)
          .ToArray();
      }

      return output
        .SelectMany(x => x.Elements)
        .ToArray();
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
        new AttributesAttributeHandler(),
        new OmitTagAttributeHandler(),
      };

      _errorHandler = errorHandler?? new OnErrorAttributeHandler();
    }

    #endregion
  }
}

