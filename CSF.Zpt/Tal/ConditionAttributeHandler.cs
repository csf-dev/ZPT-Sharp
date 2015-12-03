using System;
using CSF.Zpt.Rendering;
using CSF.Zpt.Resources;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Implementation of <see cref="IAttributeHandler"/> which handles a <c>tal:condition</c> attribute.
  /// </summary>
  public class ConditionAttributeHandler : IAttributeHandler
  {
    #region methods

    /// <summary>
    /// Handle the related attribute types which exist upon the element, if any.
    /// </summary>
    /// <returns>A collection of elements which are present in the DOM after this handler has completed its work.</returns>
    /// <param name="element">Element.</param>
    /// <param name="model">Model.</param>
    public ZptElement[] Handle(ZptElement element, Model model)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(model == null)
      {
        throw new ArgumentNullException("model");
      }

      ZptElement[] output = new [] { element };

      var attribute = element.GetTalAttribute(ZptConstants.Tal.ConditionAttribute);
      if(attribute != null)
      {
        ExpressionResult result;

        try
        {
          result = model.Evaluate(attribute.Value, element);
        }
        catch(Exception ex)
        {
          string message = String.Format(ExceptionMessages.ExpressionEvaluationExceptionFormat,
                                         ZptConstants.Tal.DefaultPrefix,
                                         ZptConstants.Tal.ConditionAttribute,
                                         attribute.Value);
          throw new ModelEvaluationException(message, ex) {
            ExpressionText = attribute.Value
          };
        }

        var removeElement = result.CancelsAction()? false : !result.GetResultAsBoolean();
        if(removeElement)
        {
          element.Remove();
          output = new ZptElement[0];
        }
      }

      return output;
    }

    #endregion
  }
}

