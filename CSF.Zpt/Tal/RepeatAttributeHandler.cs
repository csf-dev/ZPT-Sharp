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
    /// Handle the related attribute types which exist upon the element exposed by the given context, if any.
    /// </summary>
    /// <returns>A response type providing information about the result of this operation.</returns>
    /// <param name="context">The rendering context, which exposes a ZPT element.</param>
    public AttributeHandlingResult Handle(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      RenderingContext[] output = null;
      var attribute = context.GetTalAttribute(ZptConstants.Tal.RepeatAttribute);

      if(attribute != null)
      {
        string repeatVariableName, expression;

        this.ParseAttributeValue(attribute, out repeatVariableName, out expression, context.Element);

        var sequence = this.GetSequence(expression, context.TalModel, context);

        if(sequence != null)
        {
          var repetitionInfos = this.GetRepetitions(sequence, context.Element, repeatVariableName);

          output = this.HandleRepetitions(repetitionInfos, context);
        }
      }

      if(output == null)
      {
        output = new [] { context };
      }

      return new AttributeHandlingResult(output, true);
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
                                       ZptConstants.Tal.Namespace,
                                       ZptConstants.Tal.RepeatAttribute,
                                       attribute.Value,
                                       element.Name);
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
    /// <param name="context">The rendering context.</param>
    private IEnumerable GetSequence(string expression, IModel model, RenderingContext context)
    {
      ExpressionResult result;
      IEnumerable output;

      try
      {
        result = model.Evaluate(expression, context);
      }
      catch(Exception ex)
      {
        string message = String.Format(ExceptionMessages.ExpressionEvaluationException,
                                       ZptConstants.Tal.Namespace,
                                       ZptConstants.Tal.DefineAttribute,
                                       expression,
                                       context.Element.Name);
        throw new ModelEvaluationException(message, ex) {
          ExpressionText = expression,
          ElementName = context.Element.Name,
        };
      }

      if(result.CancelsAction)
      {
        output = null;
      }
      else
      {
        try
        {
          output = result.GetValue<IEnumerable>();
        }
        catch(InvalidCastException ex)
        {
          string message = String.Format(ExceptionMessages.TalRepeatExpressionMustEvaluateToIEnumerable,
                                         typeof(IEnumerable).FullName,
                                         expression,
                                         context.Element.Name);
          throw new ModelEvaluationException(message, ex) {
            ExpressionText = expression,
            ElementName = context.Element.Name,
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
    private IRepetitionInfo[] GetRepetitions(IEnumerable sequence, ZptElement element, string repeatVariableName)
    {
      var sequenceArray = sequence
        .Cast<object>()
        .ToArray();

      int i = 0;
      return sequenceArray
        .Select(x => new RepetitionInfo(repeatVariableName,
                                        i++,
                                        sequenceArray.Length,
                                        x,
                                        element.Clone()))
        .Cast<IRepetitionInfo>()
        .ToArray();
    }

    /// <summary>
    /// Handles the collection of repetitions, adding the data to the model and inserting new elements, as appropriate.
    /// </summary>
    /// <returns>The collection of new rendering contexts which represent new elements added to the DOM.</returns>
    /// <param name="repetitions">The repetitions.</param>
    /// <param name="context">The source rendering context.</param>
    private RenderingContext[] HandleRepetitions(IRepetitionInfo[] repetitions, RenderingContext context)
    {
      if(repetitions.Any())
      {
        context.TalModel.AddRepetitionInfo(repetitions);
        var parent = context.Element.GetParentElement();

        foreach(var item in repetitions)
        {
          item.AssociatedElement.RemoveAttribute(ZptConstants.Tal.Namespace,
                                                 ZptConstants.Tal.RepeatAttribute);
          parent.InsertBefore(context.Element, item.AssociatedElement);
        }
      }

      context.Element.Remove();

      return repetitions
        .Select(x => context.CreateSiblingContext(x.AssociatedElement))
        .ToArray();
    }

    #endregion
  }
}

