using System;
using ZptSharp.Expressions;
using ZptSharp.Hosting;

namespace ZptSharp
{
    /// <summary>
    /// Extension methods for <see cref="IBuildsHostingEnvironment"/> instances.
    /// </summary>
    public static class ZptSharpHostingBuilderExtensions
    {
        /// <summary>
        /// <para>
        /// Configures ZptSharp to use the standard/out-of-the-box expressions which are shipped with this package.
        /// This is equivalent to using all of the following methods:
        /// </para>
        /// <list type="bullet">
        /// <item><see cref="AddZptPathExpressions"/></item>
        /// <item><see cref="AddZptStringExpressions"/></item>
        /// <item><see cref="AddZptNotExpressions"/></item>
        /// </list>
        /// </summary>
        /// <returns>The same builder instance, after setting it up.</returns>
        /// <param name="builder">The hosting builder.</param>
        public static IBuildsHostingEnvironment AddStandardZptExpressions(this IBuildsHostingEnvironment builder)
        {
            return builder
                .AddZptPathExpressions()
                .AddZptStringExpressions()
                .AddZptNotExpressions();
        }

        /// <summary>
        /// Configures ZptSharp to read and handle TALES "path" expressions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method registers all four of:
        /// </para>
        /// <list type="bullet">
        /// <item><see cref="Expressions.PathExpressions.PathExpressionEvaluator"/>, using the prefix <see cref="WellKnownExpressionPrefix.Path"/>.</item>
        /// <item><see cref="Expressions.PathExpressions.LocalVariablesOnlyPathExpressionEvaluator"/>, using the prefix <see cref="WellKnownExpressionPrefix.LocalVariablePath"/>.</item>
        /// <item><see cref="Expressions.PathExpressions.GlobalVariablesOnlyPathExpressionEvaluator"/>, using the prefix <see cref="WellKnownExpressionPrefix.GlobalVariablePath"/>.</item>
        /// <item><see cref="Expressions.PathExpressions.DefinedVariablesOnlyPathExpressionEvaluator"/>, using the prefix <see cref="WellKnownExpressionPrefix.DefinedVariablePath"/>.</item>
        /// </list>
        /// </remarks>
        /// <returns>The same builder instance, after setting it up.</returns>
        /// <param name="builder">The hosting builder.</param>
        public static IBuildsHostingEnvironment AddZptPathExpressions(this IBuildsHostingEnvironment builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ServiceRegistry.ExpresionEvaluatorTypes.Add(WellKnownExpressionPrefix.Path, typeof(Expressions.PathExpressions.PathExpressionEvaluator));
            builder.ServiceRegistry.ExpresionEvaluatorTypes.Add(WellKnownExpressionPrefix.LocalVariablePath, typeof(Expressions.PathExpressions.LocalVariablesOnlyPathExpressionEvaluator));
            builder.ServiceRegistry.ExpresionEvaluatorTypes.Add(WellKnownExpressionPrefix.GlobalVariablePath, typeof(Expressions.PathExpressions.GlobalVariablesOnlyPathExpressionEvaluator));
            builder.ServiceRegistry.ExpresionEvaluatorTypes.Add(WellKnownExpressionPrefix.DefinedVariablePath, typeof(Expressions.PathExpressions.DefinedVariablesOnlyPathExpressionEvaluator));

            return builder;
        }

        /// <summary>
        /// Configures ZptSharp to read and handle TALES "string" expressions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method registers the <see cref="Expressions.StringExpressions.StringExpressionEvaluator"/> under both of the
        /// prefixes <see cref="WellKnownExpressionPrefix.String"/> &amp; <see cref="WellKnownExpressionPrefix.ShortStringAlias"/>.
        /// </para>
        /// </remarks>
        /// <returns>The same builder instance, after setting it up.</returns>
        /// <param name="builder">The hosting builder.</param>
        public static IBuildsHostingEnvironment AddZptStringExpressions(this IBuildsHostingEnvironment builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ServiceRegistry.ExpresionEvaluatorTypes.Add(WellKnownExpressionPrefix.String, typeof(Expressions.StringExpressions.StringExpressionEvaluator));
            builder.ServiceRegistry.ExpresionEvaluatorTypes.Add(WellKnownExpressionPrefix.ShortStringAlias, typeof(Expressions.StringExpressions.StringExpressionEvaluator));

            return builder;
        }

        /// <summary>
        /// Configures ZptSharp to read and handle TALES "not" expressions.
        /// </summary>
        /// <returns>The same builder instance, after setting it up.</returns>
        /// <param name="builder">The hosting builder.</param>
        public static IBuildsHostingEnvironment AddZptNotExpressions(this IBuildsHostingEnvironment builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ServiceRegistry.ExpresionEvaluatorTypes.Add(WellKnownExpressionPrefix.Not, typeof(Expressions.NotExpressions.NotExpressionEvaluator));

            return builder;
        }

        /// <summary>
        /// Configures ZptSharp to read and handle TALES "pipe" expressions.
        /// </summary>
        /// <returns>The same builder instance, after setting it up.</returns>
        /// <param name="builder">The hosting builder.</param>
        public static IBuildsHostingEnvironment AddZptPipeExpressions(this IBuildsHostingEnvironment builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ServiceRegistry.ExpresionEvaluatorTypes.Add(WellKnownExpressionPrefix.Pipe, typeof(Expressions.PipeExpressions.PipeExpressionEvaluator));

            return builder;
        }
    }
}