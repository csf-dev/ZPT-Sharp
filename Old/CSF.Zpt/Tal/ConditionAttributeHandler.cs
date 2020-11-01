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
    /// Handle the related attribute types which exist upon the element exposed by the given context, if any.
    /// </summary>
    /// <returns>A response type providing information about the result of this operation.</returns>
    /// <param name="context">The rendering context, which exposes a ZPT element.</param>
    public AttributeHandlingResult Handle(IRenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      AttributeHandlingResult output = new AttributeHandlingResult(new [] { context }, true);

      var attribute = context.GetTalAttribute(ZptConstants.Tal.ConditionAttribute);
      if(attribute != null)
      {
        ExpressionResult result;

        try
        {
          result = context.TalModel.Evaluate(attribute.Value, context);
        }
        catch(Exception ex)
        {
          string message = String.Format(ExceptionMessages.ExpressionEvaluationException,
                                         ZptConstants.Tal.Namespace,
                                         ZptConstants.Tal.ConditionAttribute,
                                         attribute.Value,
                                         context.Element.Name);
          throw new ModelEvaluationException(message, ex) {
            ExpressionText = attribute.Value,
            ElementName = context.Element.Name,
          };
        }

        var removeElement = result.CancelsAction? false : !result.GetValueAsBoolean();
        if(removeElement)
        {
          context.Element.Remove();
          output = new AttributeHandlingResult(new RenderingContext[0], false);
        }
      }

      return output;
    }

    #endregion
  }
}

