using ZptSharp.Expressions;
using ZptSharp.Hosting;

namespace ZptSharp
{
    /// <summary>
    /// Extension methods for <see cref="IBuildsSelfHostingEnvironment" />.
    /// </summary>
    public static class SelfHostingEnvironmentBuilderExtensions
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
        /// <returns>The self-hosting builder instance, after setting it up.</returns>
        /// <param name="builder">The self-hosting builder.</param>
        public static IBuildsSelfHostingEnvironment AddStandardZptExpressions(this IBuildsSelfHostingEnvironment builder)
        {
            builder.ServiceUsages.Add(provider => provider.UseStandardZptExpressions());
            return builder;
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
        /// <returns>The self-hosting builder instance, after setting it up.</returns>
        /// <param name="builder">The self-hosting builder.</param>
        public static IBuildsSelfHostingEnvironment AddZptPathExpressions(this IBuildsSelfHostingEnvironment builder)
        {
            builder.ServiceUsages.Add(provider => provider.UseZptPathExpressions());
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
        /// <returns>The self-hosting builder instance, after setting it up.</returns>
        /// <param name="builder">The self-hosting builder.</param>
        public static IBuildsSelfHostingEnvironment AddZptStringExpressions(this IBuildsSelfHostingEnvironment builder)
        {
            builder.ServiceUsages.Add(provider => provider.UseZptStringExpressions());
            return builder;
        }

        /// <summary>
        /// Configures ZptSharp to read and handle TALES "not" expressions.
        /// </summary>
        /// <returns>The self-hosting builder instance, after setting it up.</returns>
        /// <param name="builder">The self-hosting builder.</param>
        public static IBuildsSelfHostingEnvironment AddZptNotExpressions(this IBuildsSelfHostingEnvironment builder)
        {
            builder.ServiceUsages.Add(provider => provider.UseZptNotExpressions());
            return builder;
        }
    }
}