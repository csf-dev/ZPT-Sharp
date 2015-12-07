using System;
using System.Text.RegularExpressions;
using CSF.Zpt.Rendering;
using CSF.Zpt.Resources;
using System.Collections;
using System.Linq;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Implementation of <see cref="IAttributeHandler"/> which handles a <c>tal:repeat</c> attribute.
  /// </summary>
  public class RepeatAttributeHandler : IAttributeHandler
  {
    #region constants

    private const string ATTRIBUTE_MATCH_PATTERN = "^([^ ]+) +(.+)$";

    private static readonly Regex AttributeMatcher = new Regex(ATTRIBUTE_MATCH_PATTERN, RegexOptions.Compiled);

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

      ZptElement[] output = null;
      var attribute = element.GetTalAttribute(ZptConstants.Tal.RepeatAttribute);

      if(attribute != null)
      {
        string repeatVariableName, expression;

        this.ParseAttributeValue(attribute, out repeatVariableName, out expression, element);

        var sequence = this.GetSequence(expression, model, element);

        if(sequence != null)
        {
          var repetitionInfos = this.GetRepetitions(sequence, element, repeatVariableName);

          output = this.HandleRepetitions(repetitionInfos, model, element);
        }
      }

      if(output == null)
      {
        output = new [] { element };
      }

      return output;
    }

    /// <summary>
    /// Parses the value of a 'tal:repeat' attribute and exposes the variable name and expression.
    /// </summary>
    /// <param name="attribute">Attribute.</param>
    /// <param name="variableName">Variable name.</param>
    /// <param name="expression">Expression.</param>
    /// <param name="element">The element which contains the attribute.</param>
    private void ParseAttributeValue(ZptAttribute attribute,
                                     out string variableName,
                                     out string expression,
                                     ZptElement element)
    {
      var attribMatch = AttributeMatcher.Match(attribute.Value);

      if(!attribMatch.Success)
      {
        string message = String.Format(ExceptionMessages.ZptAttributeParsingError,
                                       ZptConstants.Tal.DefaultPrefix,
                                       ZptConstants.Tal.RepeatAttribute,
                                       attribute.Value);
        throw new ParserException(message) {
          SourceAttributeName = ZptConstants.Tal.RepeatAttribute,
          SourceElementName = element.Name,
          SourceAttributeValue = attribute.Value
        };
      }

      variableName = attribMatch.Groups[1].Value;
      expression = attribMatch.Groups[2].Value;
    }

    /// <summary>
    /// Gets the sequence exposed by the given expression.
    /// </summary>
    /// <returns>The sequence.</returns>
    /// <param name="expression">Expression.</param>
    /// <param name="model">Model.</param>
    /// <param name="element">Element.</param>
    private IEnumerable GetSequence(string expression, Model model, ZptElement element)
    {
      ExpressionResult result;
      IEnumerable output;

      try
      {
        result = model.Evaluate(expression, element);
      }
      catch(Exception ex)
      {
        string message = String.Format(ExceptionMessages.ExpressionEvaluationException,
                                       ZptConstants.Tal.DefaultPrefix,
                                       ZptConstants.Tal.DefineAttribute,
                                       expression);
        throw new ModelEvaluationException(message, ex) {
          ExpressionText = expression
        };
      }

      if(result.CancelsAction())
      {
        output = null;
      }
      else
      {
        try
        {
          output = result.GetResult<IEnumerable>();
        }
        catch(InvalidCastException ex)
        {
          string message = String.Format(ExceptionMessages.TalRepeatExpressionMustEvaluateToIEnumerable,
                                         typeof(IEnumerable).FullName,
                                         expression);
          throw new ModelEvaluationException(message, ex) {
            ExpressionText = expression
          };
        }
      }

      return output;
    }

    /// <summary>
    /// Gets an array of <see cref="RepetitionInfo"/> which represents the repetitions of the element.
    /// </summary>
    /// <returns>The repetitions.</returns>
    /// <param name="sequence">The source sequence.</param>
    /// <param name="element">The source element.</param>
    /// <param name="repeatVariableName">Repeat variable name.</param>
    private RepetitionInfo[] GetRepetitions(IEnumerable sequence, ZptElement element, string repeatVariableName)
    {
      var sequenceArray = sequence
        .Cast<object>()
        .ToArray();
      
      return sequenceArray
        .Select(x => new RepetitionInfo(repeatVariableName,
                                        Array.IndexOf<object>(sequenceArray, x),
                                        sequenceArray.Length,
                                        x,
                                        element.Clone()))
        .ToArray();
    }

    /// <summary>
    /// Handles the collection of repetitions, adding the data to the model and inserting new elements, as appropriate.
    /// </summary>
    /// <returns>The collection of ZPT elements which were added to the DOM.</returns>
    /// <param name="repetitions">The repetitions.</param>
    /// <param name="model">The model.</param>
    /// <param name="element">The source ZPT element.</param>
    private ZptElement[] HandleRepetitions(RepetitionInfo[] repetitions, Model model, ZptElement element)
    {
      if(repetitions.Any())
      {
        model.AddRepetitionInfo(repetitions);
        var parent = element.GetParentElement();

        foreach(var item in repetitions)
        {
          parent.InsertBefore(element, item.AssociatedElement);
        }
      }

      element.Remove();

      return repetitions
        .Select(x => x.AssociatedElement)
        .ToArray();
    }

    #endregion
  }
}

