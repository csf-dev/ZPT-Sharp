using System;
using System.Text.RegularExpressions;
using System.Linq;
using CSF.Zpt.Rendering;
using CSF.Zpt.Resources;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Implementation of <see cref="ITalAttributeHandler"/> which handles a <c>tal:define</c> attribute.
  /// </summary>
  public class TalDefineAttributeHandler : ITalAttributeHandler
  {
    #region constants

    private const string
      GLOBAL_SCOPE        = "global",
      DEFINITION_PATTERN  = "^(?:(local|global) )?([^ ]+) (.+)$",
      ITEM_PATTERN        = @"((?:[^;]|;;)+)\s*(?:;(?!;))?\s*";

    private static readonly Regex
      Definition          = new Regex(DEFINITION_PATTERN, RegexOptions.Compiled),
      Item                = new Regex(ITEM_PATTERN, RegexOptions.Compiled);

    #endregion

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

      var attrib = element.GetTalAttribute(ZptConstants.Tal.DefineAttribute);

      if(attrib != null)
      {
        var definitionText = attrib.Value;

        var items = (from match in Item.Matches(definitionText).Cast<Match>()
                     let def = Definition.Match(match.Groups[1].Value)
                     select new { Scope = def.Groups[1].Value,
                                  Name = def.Groups[2].Value,
                                  Expression = def.Groups[3].Value })
          .ToArray();

        foreach(var item in items)
        {
          ExpressionResult result;

          try
          {
            result = model.Evaluate(item.Expression);
          }
          catch(Exception ex)
          {
            string message = String.Format(ExceptionMessages.ExpressionEvaluationExceptionFormat,
                                           ZptConstants.Tal.DefaultPrefix,
                                           ZptConstants.Tal.DefineAttribute,
                                           item.Expression);
            throw new ModelEvaluationException(message, ex) {
              ExpressionText = item.Expression
            };
          }

          if(!result.CancelsAction())
          {
            if(item.Scope == GLOBAL_SCOPE)
            {
              model.AddGlobal(item.Name, result.GetResult());
            }
            else
            {
              model.AddLocal(item.Name, result.GetResult());
            }
          }
        }
      }

      return new[] { element };
    }

    #endregion
  }
}

