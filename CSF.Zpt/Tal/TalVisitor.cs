using System;
using System.Linq;
using CSF.Zpt.Rendering;
using System.Collections.Generic;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Implementation of <see cref="IContextVisitor"/> which performs TAL-related functionality.
  /// </summary>
  public class TalVisitor : ContextVisitorBase
  {
    #region fields

    #pragma warning disable 414
    private static readonly log4net.ILog _logger;
    #pragma warning restore 414

    private IAttributeHandler[] _handlers;
    private IAttributeHandler _errorHandler;

    #endregion

    #region methods

    /// <summary>
    /// Visit the given context and return a collection of the resultant contexts.
    /// </summary>
    /// <returns>Zero or more <see cref="RenderingContext"/> instances, determined by the outcome of this visit.</returns>
    /// <param name="context">The rendering context to visit.</param>
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
        var handlingResult = new HashSet<AttributeHandlingResult>();

        foreach(var ctx in output)
        {
          var processedBatch = handler.Handle(ctx);
          if(processedBatch.ContinueHandling)
          {
            handlingResult.Add(processedBatch);
          }
        }

        newlyExposedElements = newlyExposedElements.Union(handlingResult.SelectMany(x => x.NewlyExposedContexts));
        output = handlingResult.Where(x => x.ContinueHandling).SelectMany(x => x.Contexts).ToArray();
      }

      output = output.Union(newlyExposedElements.SelectMany(x => this.Visit(x)));

      return output.ToArray();
    }

    /// <summary>
    /// Visit the given context, as well as its child contexts, and return a collection of the resultant contexts.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This operation performs the same work as <see cref="Visit"/>, but it then visits all of the resultant contexts,
    /// recursively moving down the exposed document tree, visiting each context in turn.
    /// </para>
    /// <para>
    /// In this implementation, the visit additionally provides error-handling via the TAL on-error attribute.
    /// </para>
    /// </remarks>
    /// <returns>Zero or more <see cref="RenderingContext"/> instances, determined by the outcome of this visit.</returns>
    /// <param name="context">The rendering context to visit.</param>
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

        if(context.GetTalAttribute(ZptConstants.Tal.OnErrorAttribute) != null)
        {
          context.TalModel.AddError(ex);
          _errorHandler.Handle(context);
        }
        else
        {
          throw;
        }
      }

      foreach(var item in output)
      {
        item.Element.PurgeTalAttributes();
        if(item.Element.IsInNamespace(ZptConstants.Tal.Namespace))
        {
          item.Element.Omit();
        }
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

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.Tal.TalVisitor"/> class.
    /// </summary>
    static TalVisitor()
    {
      _logger = log4net.LogManager.GetLogger(typeof(TalVisitor));
    }

    #endregion
  }
}

