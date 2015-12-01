using System;
using System.Linq;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Visitor type which is used to work upon an <see cref="ZptElement"/> and perform TAL-related functionality.
  /// </summary>
  public class TalVisitor : ElementVisitor
  {
    #region fields

    private ITalAttributeHandler[] _handlers;
    private ITalAttributeHandler _errorHandler;

    #endregion

    #region methods

    /// <summary>
    /// Visit the given element and perform modifications as required.
    /// </summary>
    /// <returns>A reference to the element which has been visited.  This might be the input <paramref name="element"/> or a replacement.</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="model">The object model provided as context to the visitor.</param>
    public override ZptElement[] Visit(ZptElement element, Model model)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(model == null)
      {
        throw new ArgumentNullException("model");
      }


      var output = new [] { element };

      foreach(var handler in _handlers)
      {
        var elementsAfterProcessing = (from ele in output
                                       from handled in handler.Handle(ele, model)
                                       select handled)
          .ToArray();

        output = elementsAfterProcessing;
      }

      return output;
    }

    /// <summary>
    /// Visits the given element, and then recursively visits all of its child elements.
    /// </summary>
    /// <returns>The recursively.</returns>
    /// <param name="element">Element.</param>
    /// <param name="model">Model.</param>
    public override ZptElement[] VisitRecursively(ZptElement element, Model model)
    {
      ZptElement[] output;

      try
      {
        output = base.VisitRecursively(element, model);
      }
      catch(RenderingException ex)
      {
        output = new [] { element };

        if(element.GetTalAttribute(Tal.OnErrorAttribute) != null)
        {
          model.AddError(ex);
          _errorHandler.Handle(element, model);
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
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.TalVisitor"/> class.
    /// </summary>
    /// <param name="options">Rendering options.</param>
    /// <param name="handlers">A collection of <see cref="ITalAttributeHandler"/> to process.</param>
    /// <param name="errorHandler">An <see cref="ITalAttributeHandler"/> which should be used to handle any errors.</param>
    public TalVisitor(RenderingOptions options = null,
                      ITalAttributeHandler[] handlers = null,
                      ITalAttributeHandler errorHandler = null) : base(options: options)
    {
      _handlers = handlers?? new ITalAttributeHandler[] {
        new TalDefineAttributeHandler(),
        new TalConditionAttributeHandler(),
        new TalRepeatAttributeHandler(),
        new TalContentOrReplaceAttributeHandler(),
        new TalAttributesAttributeHandler(),
        new TalOmitTagAttributeHandler(),
      };

      _errorHandler = errorHandler?? new TalOnErrorAttributeHandler();
    }

    #endregion
  }
}

