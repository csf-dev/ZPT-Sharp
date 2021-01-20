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
        /// Configures ZptSharp to use a number of expression-types which are part of the ZPT standard.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The expression evaluators (expression types) supported by this method are parts of the formal ZPT standard, thus
        /// they are very frequently used within ZPT document templates.
        /// Using this method is equivalent to using all three of the following:
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="AddZptPathExpressions"/></description></item>
        /// <item><description><see cref="AddZptStringExpressions"/></description></item>
        /// <item><description><see cref="AddZptNotExpressions"/></description></item>
        /// </list>
        /// </remarks>
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
        /// This method primarily registers the evaluator for "path" expressions.
        /// There are three lesser-used variants of path expressions which are also included in this method though.
        /// The expression evaluators and expression prefixes supported by this method are:
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="Expressions.PathExpressions.PathExpressionEvaluator"/>, using the prefix <see cref="WellKnownExpressionPrefix.Path"/>.</description></item>
        /// <item><description><see cref="Expressions.PathExpressions.LocalVariablesOnlyPathExpressionEvaluator"/>, using the prefix <see cref="WellKnownExpressionPrefix.LocalVariablePath"/>.</description></item>
        /// <item><description><see cref="Expressions.PathExpressions.GlobalVariablesOnlyPathExpressionEvaluator"/>, using the prefix <see cref="WellKnownExpressionPrefix.GlobalVariablePath"/>.</description></item>
        /// <item><description><see cref="Expressions.PathExpressions.DefinedVariablesOnlyPathExpressionEvaluator"/>, using the prefix <see cref="WellKnownExpressionPrefix.DefinedVariablePath"/>.</description></item>
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

        /// <summary>
        /// Configures ZptSharp to read and handle TALES "structure" expressions.
        /// </summary>
        /// <returns>The same builder instance, after setting it up.</returns>
        /// <param name="builder">The hosting builder.</param>
        public static IBuildsHostingEnvironment AddZptStructureExpressions(this IBuildsHostingEnvironment builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ServiceRegistry.ExpresionEvaluatorTypes.Add(WellKnownExpressionPrefix.Structure, typeof(Expressions.StructureExpressions.StructureExpressionEvaluator));

            return builder;
        }
    }
}