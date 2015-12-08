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
    /// <returns>A response type providing information about the result of this operation.</returns>
    /// <param name="element">Element.</param>
    /// <param name="model">Model.</param>
    public AttributeHandlingResult Handle(ZptElement element, Model model)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(model == null)
      {
        throw new ArgumentNullException("model");
      }

      AttributeHandlingResult output = new AttributeHandlingResult(new [] { element }, true);

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
          string message = String.Format(ExceptionMessages.ExpressionEvaluationException,
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
          output = new AttributeHandlingResult(new ZptElement[0], false);
        }
      }

      return output;
    }

    #endregion
  }
}

