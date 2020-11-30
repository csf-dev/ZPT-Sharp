using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Metal
{
    /// <summary>
    /// Implementation of <see cref="IGetsMacro"/> which uses an expression evaluator
    /// to get macros from attributes.
    /// </summary>
    public class MacroProvider : IGetsMacro
    {
        readonly IEvaluatesExpression expressionEvaluator;
        readonly ILogger logger;

        /// <summary>
        /// Gets the METAL macro referenced by the specified element's attribute, if such an attribute is present.
        /// </summary>
        /// <returns>The METAL macro, or a null reference if the <paramref name="element"/>
        /// has no attribute matching the <paramref name="attributeSpec"/>.</returns>
        /// <param name="element">The element from which to get the macro.</param>
        /// <param name="context">The current expression context.</param>
        /// <param name="attributeSpec">An attribute spec.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <exception cref="MacroNotFoundException">If the element does have an attribute matching
        /// the <paramref name="attributeSpec"/> but no macro could be resolved from the attribute's expression.</exception>
        public Task<MetalMacro> GetMacroAsync(INode element,
                                              ExpressionContext context,
                                              AttributeSpec attributeSpec,
                                              CancellationToken token = default)
            => GetMacroAsync(element, context, new[] { attributeSpec }, token);

        /// <summary>
        /// Gets the METAL macro referenced by the specified element's attribute, if such an attribute is present.
        /// </summary>
        /// <returns>The METAL macro, or a null reference if the <paramref name="element"/>
        /// has no attribute matching any of the <paramref name="attributeSpecs"/>.</returns>
        /// <param name="element">The element from which to get the macro.</param>
        /// <param name="context">The current expression context.</param>
        /// <param name="attributeSpecs">A collection of attribute specs.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <exception cref="T:ZptSharp.Metal.MacroNotFoundException">If the element does have an attribute matching
        /// the <paramref name="attributeSpecs"/> but no macro could be resolved from the attribute's expression.</exception>
        public Task<MetalMacro> GetMacroAsync(INode element,
                                              ExpressionContext context,
                                              IEnumerable<AttributeSpec> attributeSpecs,
                                              CancellationToken token = default)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (attributeSpecs == null)
                throw new ArgumentNullException(nameof(attributeSpecs));

            return GetMacroPrivateAsync(element, context, attributeSpecs, token);
        }

        async Task<MetalMacro> GetMacroPrivateAsync(INode element,
                                                    ExpressionContext context,
                                                    IEnumerable<AttributeSpec> attributeSpecs,
                                                    CancellationToken token)
        {
            var attribute = element.GetMatchingAttribute(attributeSpecs, out var attributeSpec);
            if (attribute == null)
            {
                if(logger.IsEnabled(LogLevel.Trace))
                    logger.LogTrace("No macro referenced by {element}", element);

                return null;
            }

            MetalMacro macro = null;
            try
            {
                macro = await expressionEvaluator.EvaluateExpressionAsync<MetalMacro>(attribute.Value, context, token)
                    .ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                AssertMacroIsNotNull(macro, element, attribute.Value, attributeSpec, ex);
            }

            AssertMacroIsNotNull(macro, element, attribute.Value, attributeSpec);

            if (logger.IsEnabled(LogLevel.Trace))
                logger.LogTrace(@"An element references a METAL macro:
Element:{element} ({element_source})
  Macro:{macro} ({macro_source})",
                                element,
                                element.SourceInfo,
                                macro.Element,
                                macro.Element.SourceInfo);

            return macro.GetCopy();
        }

        void AssertMacroIsNotNull(MetalMacro macro,
                                  INode element,
                                  string macroExpression,
                                  AttributeSpec attributeSpec,
                                  Exception inner = null)
        {
            if (macro != null) return;

            var message = String.Format(Resources.ExceptionMessage.MacroNotFound,
                                        attributeSpec.Name,
                                        element,
                                        macroExpression);

            if(inner != null)
                throw new MacroNotFoundException(message, inner);

            throw new MacroNotFoundException(message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroProvider"/> class.
        /// </summary>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="logger">A logger.</param>
        public MacroProvider(IEvaluatesExpression expressionEvaluator,
                             ILogger<MacroProvider> logger)
        {
            this.expressionEvaluator = expressionEvaluator ?? throw new ArgumentNullException(nameof(expressionEvaluator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

    }
}
