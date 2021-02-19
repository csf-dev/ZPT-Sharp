using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using ZptSharp.Expressions;
using ZptSharp.Dom;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Implementation of <see cref="IGetsRepetitionContexts"/> which gets and prepares
    /// the repetition contexts.
    /// </summary>
    public class RepetitionContextProvider : IGetsRepetitionContexts
    {
        readonly IGetsTalAttributeSpecs specProvider;
        readonly IGetsAlphabeticValueForNumber alphabeticValueProvider;
        readonly IGetsRomanNumeralForNumber romanNumeralProvider;

        /// <summary>
        /// Gets the repetition contexts for the specified <paramref name="expressionResult"/>.
        /// </summary>
        /// <returns>The repetition contexts.</returns>
        /// <param name="expressionResult">Expression result.</param>
        /// <param name="sourceContext">Source context.</param>
        /// <param name="repeatVariableName">Repeat variable name.</param>
        public IList<ExpressionContext> GetRepetitionContexts(object expressionResult, ExpressionContext sourceContext, string repeatVariableName)
        {
            if (expressionResult == null)
                throw new ArgumentNullException(nameof(expressionResult));
            if (sourceContext == null)
                throw new ArgumentNullException(nameof(sourceContext));
            if (repeatVariableName == null)
                throw new ArgumentNullException(nameof(repeatVariableName));

            var enumerable = GetEnumerable(expressionResult, sourceContext);
            var repetitions = GetRepetitions(enumerable, repeatVariableName, sourceContext);
            return GetContexts(repetitions, sourceContext);
        }

        /// <summary>
        /// Gets the sequence which is represented by the <paramref name="expressionResult"/>, or
        /// raises an exception if the result does not implement <see cref="IEnumerable"/>.
        /// </summary>
        /// <returns>The enumerable sequence.</returns>
        /// <param name="expressionResult">Expression result.</param>
        /// <param name="context">Context.</param>
        static IList<object> GetEnumerable(object expressionResult, ExpressionContext context)
        {
            try
            {
                return ((IEnumerable) expressionResult).Cast<object>().ToList();
            }
            catch (InvalidCastException ex)
            {
                var message = String.Format(Resources.ExceptionMessage.RepeatExpressionResultMustBeEnumerable,
                                            typeof(IEnumerable).FullName,
                                            context.CurrentNode,
                                            expressionResult.GetType().FullName);
                throw new EvaluationException(message, ex);
            }
            catch (Exception ex)
            {
                var message = String.Format(Resources.ExceptionMessage.UnexpectedExceptionEnumeratingRepetitions,
                                            context.CurrentNode);
                throw new EvaluationException(message, ex);
            }
        }

        /// <summary>
        /// Maps a list of objects into a list of <see cref="RepetitionInfo"/> instances,
        /// including the repeated DOM node and supporting information.
        /// </summary>
        /// <returns>The list of repetition objects.</returns>
        /// <param name="sequence">Sequence.</param>
        /// <param name="variableName">Variable name.</param>
        /// <param name="context">Context.</param>
        IList<RepetitionInfo> GetRepetitions(IList<object> sequence, string variableName, ExpressionContext context)
        {
            var itemCount = sequence.Count;

            return (from index in Enumerable.Range(0, itemCount)
                    let item = sequence[index]
                    select new RepetitionInfo(alphabeticValueProvider, romanNumeralProvider)
                    {
                        Count = itemCount,
                        CurrentIndex = index,
                        CurrentValue = item,
                        Node = context.CurrentNode.GetCopy(),
                        Name = variableName
                    })
                .ToList();
        }

        /// <summary>
        /// Maps a collection of <see cref="RepetitionInfo"/> to a collection of
        /// <see cref="ExpressionContext"/>, including the setup of those context objects.
        /// </summary>
        /// <returns>The expression contexts.</returns>
        /// <param name="repetitions">Repetitions.</param>
        /// <param name="sourceContext">Source context.</param>
        IList<ExpressionContext> GetContexts(IList<RepetitionInfo> repetitions, ExpressionContext sourceContext)
        {
            var parent = sourceContext.CurrentNode.ParentNode;
            var indexOnParent = parent.ChildNodes.IndexOf(sourceContext.CurrentNode);

            var contexts = repetitions
                .Select(repetition => GetContext(repetition, sourceContext))
                .ToList();

            contexts = GetWhitespaceSeparatedContexts(contexts, sourceContext, repetitions.FirstOrDefault()?.Name).ToList();

            AddContextsToParent(contexts, parent, indexOnParent);

            return contexts;
        }

        /// <summary>
        /// Maps a single <see cref="RepetitionInfo"/> (and supporting information) to
        /// an <see cref="ExpressionContext"/>.  This includes some manipulation of that
        /// context, appropriate for the functionality of a TAL repeat attribute.
        /// </summary>
        /// <returns>The context.</returns>
        /// <param name="repetition">Repetition.</param>
        /// <param name="sourceContext">Source context.</param>
        ExpressionContext GetContext(RepetitionInfo repetition, ExpressionContext sourceContext)
        {
            var context = sourceContext.CreateChild(repetition.Node);

            // Set up the repetition variable on the context
            context.Repetitions.Add(repetition.Name, repetition);
            context.LocalDefinitions.Add(repetition.Name, repetition.CurrentValue);

            // The copied node will have a 'repeat' attribute, but
            // we don't want to process it again, so we remove it.
            var duplicateRepeatAttribute = context.CurrentNode.Attributes
                .FirstOrDefault(x => x.Matches(specProvider.Repeat));
            if(duplicateRepeatAttribute != null)
                context.CurrentNode.Attributes.Remove(duplicateRepeatAttribute);

            return context;
        }

        /// <summary>
        /// Contexts returned as a result of a repeat attribute need to be separated by whitespace, so that
        /// they are not bunched up.  This doesn't usually matter for nodes, which are generally
        /// whitespace-neutral anyway, but it's important if we are omitting the node and just using
        /// text nodes.
        /// </summary>
        /// <returns>The whitespace separated contexts.</returns>
        /// <param name="contexts">Contexts.</param>
        /// <param name="originalContext">Context.</param>
        /// <param name="variableName">Repetition variable name</param>
        static IEnumerable<ExpressionContext> GetWhitespaceSeparatedContexts(IList<ExpressionContext> contexts,
                                                                      ExpressionContext originalContext,
                                                                      string variableName)
        {
            for (var i = 0; i < contexts.Count; i++)
            {
                yield return contexts[i];
                if (i == contexts.Count - 1) continue;

                var whitespace = originalContext.CreateChild(originalContext.CurrentNode.CreateTextNode(Environment.NewLine));
                whitespace.LocalDefinitions.Add(variableName, null);
                yield return whitespace;
            }
        }

        /// <summary>
        /// Adds the context node nodes to the parent node.
        /// </summary>
        /// <remarks>
        /// <para>
        /// We reverse the list of contexts before adding them, so that (when adding them
        /// all at the same index) they are naturally restored to the original order.
        /// </para>
        /// </remarks>
        /// <param name="contexts">Contexts.</param>
        /// <param name="parent">Parent.</param>
        /// <param name="indexOnParent">Index on parent.</param>
        static void AddContextsToParent(List<ExpressionContext> contexts, INode parent, int indexOnParent)
        {
            var reversedContexts = new List<ExpressionContext>(contexts);
            reversedContexts.Reverse();

            foreach (var context in reversedContexts)
                parent.ChildNodes.Insert(indexOnParent, context.CurrentNode);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepetitionContextProvider"/> class.
        /// </summary>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="alphabeticValueProvider">A service which gets alphabetic versions of strings.</param>
        /// <param name="romanNumeralProvider">A service which gets roman numerals.</param>
        public RepetitionContextProvider(IGetsTalAttributeSpecs specProvider,
                                         IGetsAlphabeticValueForNumber alphabeticValueProvider,
                                         IGetsRomanNumeralForNumber romanNumeralProvider)
        {
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.alphabeticValueProvider = alphabeticValueProvider ?? throw new ArgumentNullException(nameof(alphabeticValueProvider));
            this.romanNumeralProvider = romanNumeralProvider ?? throw new ArgumentNullException(nameof(romanNumeralProvider));
        }
    }
}
