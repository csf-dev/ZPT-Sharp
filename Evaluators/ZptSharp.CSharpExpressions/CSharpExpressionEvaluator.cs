using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// An implementation of <see cref="IEvaluatesExpression"/> which evaluates TALES 'csharp' expressions.
    /// </summary>
    public class CSharpExpressionEvaluator : IEvaluatesExpression
    {
        internal const string ExpressionPrefix = "csharp";

        readonly IGetsAllVariablesFromContext allValuesProvider;
        readonly ICachesCSharpExpressions expressionCache;
        readonly ICreatesCSharpExpressions expressionFactory;
        readonly IGetsExpressionDescription identityFactory;
        readonly IConfiguresCSharpExpressionGlobals globalConfig;

        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <param name="expression">The expression string.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<object> EvaluateExpressionAsync(string expression,
                                                    ExpressionContext context,
                                                    CancellationToken cancellationToken = default)
        {
            if (expression is null)
                throw new System.ArgumentNullException(nameof(expression));
            if (context is null)
                throw new System.ArgumentNullException(nameof(context));

            return EvaluateExpressionPrivateAsync(expression, context, cancellationToken);
        }

        async Task<object> EvaluateExpressionPrivateAsync(string expression,
                                                          ExpressionContext context,
                                                          CancellationToken cancellationToken)
        {
            var allTalesValues = await allValuesProvider.GetAllVariablesAsync(context)
                .ConfigureAwait(false);
            var description = identityFactory.GetDescription(expression,
                                                             allTalesValues,
                                                             globalConfig.GlobalAssemblyReferences.ToList(),
                                                             globalConfig.GlobalUsingNamespaces.ToList());
            var compiledExpression = await GetExpressionAsync(description, context, cancellationToken)
                .ConfigureAwait(false);

            try
            {
                return compiledExpression(allTalesValues);
            }
            catch(Exception ex)
            {
                var message = String.Format(Resources.ExceptionMessage.ErrorEvaluatingCSharpExpression, description.Expression, context.CurrentNode);
                throw new CSharpEvaluationException(message, ex);
            }
        }

        /// <summary>
        /// Gets the C# expression, from the cache if available, otherwise by using
        /// <see cref="CreateExpressionAsync(ExpressionDescription, ExpressionContext, CancellationToken)" />.
        /// </summary>
        /// <param name="description">The expression description.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The C# expression, ready to execute.</returns>
        async Task<CSharpExpression> GetExpressionAsync(ExpressionDescription description,
                                                        ExpressionContext context,
                                                        CancellationToken cancellationToken)
        {
            var cachedExpression = expressionCache.GetExpression(description);
            if(cachedExpression != null) return cachedExpression;

            var compiledExpression = await CreateExpressionAsync(description, context, cancellationToken)
                .ConfigureAwait(false);
            expressionCache.AddExpression(description, compiledExpression);
            return compiledExpression;
        }

        async Task<CSharpExpression> CreateExpressionAsync(ExpressionDescription description,
                                                           ExpressionContext context,
                                                           CancellationToken cancellationToken)
        {
            try
            {
                return await expressionFactory.GetExpressionAsync(description, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                var message = String.Format(Resources.ExceptionMessage.ErrorCompilingCSharpExpression, description.Expression, context.CurrentNode);
                throw new CSharpEvaluationException(message, ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CSharpExpressionEvaluator" />,
        /// </summary>
        /// <param name="allValuesProvider">A provider for all in-scope values in an expression context.</param>
        /// <param name="expressionCache">A cache for compiled C# expressions.</param>
        /// <param name="expressionFactory">A factory for compiling new C# expressions.</param>
        /// <param name="identityFactory">A factory for expression identity objects.</param>
        /// <param name="globalConfig">Global configuration for C# expressions.</param>
        public CSharpExpressionEvaluator(IGetsAllVariablesFromContext allValuesProvider,
                                         ICachesCSharpExpressions expressionCache,
                                         ICreatesCSharpExpressions expressionFactory,
                                         IGetsExpressionDescription identityFactory,
                                         IConfiguresCSharpExpressionGlobals globalConfig)
        {
            this.allValuesProvider = allValuesProvider ?? throw new System.ArgumentNullException(nameof(allValuesProvider));
            this.expressionCache = expressionCache ?? throw new System.ArgumentNullException(nameof(expressionCache));
            this.expressionFactory = expressionFactory ?? throw new System.ArgumentNullException(nameof(expressionFactory));
            this.identityFactory = identityFactory ?? throw new ArgumentNullException(nameof(identityFactory));
            this.globalConfig = globalConfig ?? throw new ArgumentNullException(nameof(globalConfig));
        }
    }
}