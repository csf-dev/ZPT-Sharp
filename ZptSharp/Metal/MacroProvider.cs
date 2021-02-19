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
        /// Gets the METAL macro referenced by the specified node's attribute, if such an attribute is present.
        /// </summary>
        /// <returns>The METAL macro, or a null reference if the <paramref name="node"/>
        /// has no attribute matching the <paramref name="attributeSpec"/>.</returns>
        /// <param name="node">The node from which to get the macro.</param>
        /// <param name="context">The current expression context.</param>
        /// <param name="attributeSpec">An attribute spec.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <exception cref="MacroNotFoundException">If the node does have an attribute matching
        /// the <paramref name="attributeSpec"/> but no macro could be resolved from the attribute's expression.</exception>
        public Task<MetalMacro> GetMacroAsync(INode node,
                                              ExpressionContext context,
                                              AttributeSpec attributeSpec,
                                              CancellationToken token = default)
            => GetMacroAsync(node, context, new[] { attributeSpec }, token);

        /// <summary>
        /// Gets the METAL macro referenced by the specified node's attribute, if such an attribute is present.
        /// </summary>
        /// <returns>The METAL macro, or a null reference if the <paramref name="node"/>
        /// has no attribute matching any of the <paramref name="attributeSpecs"/>.</returns>
        /// <param name="node">The node from which to get the macro.</param>
        /// <param name="context">The current expression context.</param>
        /// <param name="attributeSpecs">A collection of attribute specs.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <exception cref="T:ZptSharp.Metal.MacroNotFoundException">If the node does have an attribute matching
        /// the <paramref name="attributeSpecs"/> but no macro could be resolved from the attribute's expression.</exception>
        public Task<MetalMacro> GetMacroAsync(INode node,
                                              ExpressionContext context,
                                              IEnumerable<AttributeSpec> attributeSpecs,
                                              CancellationToken token = default)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (attributeSpecs == null)
                throw new ArgumentNullException(nameof(attributeSpecs));

            return GetMacroPrivateAsync(node, context, attributeSpecs, token);
        }

        async Task<MetalMacro> GetMacroPrivateAsync(INode node,
                                                    ExpressionContext context,
                                                    IEnumerable<AttributeSpec> attributeSpecs,
                                                    CancellationToken token)
        {
            var attribute = node.GetMatchingAttribute(attributeSpecs, out var attributeSpec);
            if (attribute == null)
            {
                if(logger.IsEnabled(LogLevel.Trace))
                    logger.LogTrace("No macro referenced by {node}", node);

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
                AssertMacroIsNotNull(macro, node, attribute.Value, attributeSpec, ex);
            }

            AssertMacroIsNotNull(macro, node, attribute.Value, attributeSpec);

            if (logger.IsEnabled(LogLevel.Trace))
                logger.LogTrace(@"An node references a METAL macro:
Node:{node} ({node_source})
  Macro:{macro} ({macro_source})",
                                node,
                                node.SourceInfo,
                                macro.Node,
                                macro.Node.SourceInfo);

            return macro.GetCopy();
        }

        void AssertMacroIsNotNull(MetalMacro macro,
                                  INode node,
                                  string macroExpression,
                                  AttributeSpec attributeSpec,
                                  Exception inner = null)
        {
            if (macro != null) return;

            var message = String.Format(Resources.ExceptionMessage.MacroNotFound,
                                        attributeSpec.Name,
                                        node,
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
