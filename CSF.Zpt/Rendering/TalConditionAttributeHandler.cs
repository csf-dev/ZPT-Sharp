using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Implementation of <see cref="ITalAttributeHandler"/> which handles a <c>tal:condition</c> attribute.
  /// </summary>
  public class TalConditionAttributeHandler : ITalAttributeHandler
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

      var attribute = element.GetTalAttribute(Tal.ConditionAttribute);
      if(attribute != null)
      {
        var value = model.Evaluate(attribute.Value);
        bool includeElement = (value.EvaluationSuccess && !value.CancelsAction())? value.GetResultAsBoolean() : true;

        if(!includeElement)
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

