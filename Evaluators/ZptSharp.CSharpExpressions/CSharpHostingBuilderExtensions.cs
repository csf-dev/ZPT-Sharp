using System;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Expressions.CSharpExpressions;
using ZptSharp.Hosting;

namespace ZptSharp
{
    /// <summary>
    /// Extension methods related to the use of ZPT Python expressions.
    /// </summary>
    public static class CSharpHostingBuilderExtensions
    {
        /// <summary>
        /// Adds service registrations to the <paramref name="builder"/> in order
        /// to enable the use of ZPT "csharp" expressions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// As well as the primary "csharp" expression, there are three other expression types which are activated by this method.
        /// In total, the expression evaluators registered by this type are:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description><see cref="AssemblyReferenceEvaluator"/> using the prefix <c>assemblyref</c></description>
        /// </item>
        /// <item>
        /// <description><see cref="UsingNamespaceEvaluator"/> using the prefix <c>using</c></description>
        /// </item>
        /// <item>
        /// <description><see cref="VariableTypeEvaluator"/> using the prefix <c>type</c></description>
        /// </item>
        /// <item>
        /// <description><see cref="CSharpExpressionEvaluator"/> using the prefix <c>csharp</c></description>
        /// </item>
        /// </list>
        /// <para>
        /// Additionally, an optional configuration action/callback may be supplied.
        /// If used, the <see cref="IConfiguresCSharpExpressionGlobals"/> may be used to add-to the lists of assembly references
        /// and 'using' namespaces which are available to all "csharp" expressions, whether they have been added explicitly via
        /// "assemblyref" or "using" expressions or not.
        /// </para>
        /// </remarks>
        /// <param name="builder">The self-hosting builder.</param>
        /// <returns>The self-hosting builder instance, after setting it up.</returns>
        /// <param name="configAction">An optional action permitting some global configuration of the C# expression evaluation.</param>
        public static IBuildsHostingEnvironment AddZptCSharpExpressions(this IBuildsHostingEnvironment builder,
                                                                        Action<IConfiguresCSharpExpressionGlobals> configAction = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ServiceCollection.AddSingleton<ICachesCSharpExpressions, ExpressionCache>();

            builder.ServiceCollection.AddTransient<CSharpExpressionEvaluator>();
            builder.ServiceCollection.AddTransient<IGetsExpressionDescription, ExpressionDescriptionFactory>();
            builder.ServiceCollection.AddTransient<ICreatesCSharpExpressions, ExpressionCompiler>();
            builder.ServiceCollection.AddTransient<IGetsScriptBody, ScriptBodyFactory>();
            builder.ServiceCollection.AddTransient<AssemblyReferenceEvaluator>();
            builder.ServiceCollection.AddTransient<UsingNamespaceEvaluator>();
            builder.ServiceCollection.AddTransient<VariableTypeEvaluator>();

            builder.ServiceRegistry.ExpresionEvaluatorTypes.Add(AssemblyReferenceEvaluator.ExpressionPrefix, typeof(AssemblyReferenceEvaluator));
            builder.ServiceRegistry.ExpresionEvaluatorTypes.Add(UsingNamespaceEvaluator.ExpressionPrefix, typeof(UsingNamespaceEvaluator));
            builder.ServiceRegistry.ExpresionEvaluatorTypes.Add(VariableTypeEvaluator.ExpressionPrefix, typeof(VariableTypeEvaluator));
            builder.ServiceRegistry.ExpresionEvaluatorTypes.Add(CSharpExpressionEvaluator.ExpressionPrefix, typeof(CSharpExpressionEvaluator));

            var expressionConfig = new GlobalExpressionConfigStore();
            builder.ServiceCollection.AddSingleton<IConfiguresCSharpExpressionGlobals>(expressionConfig);
            configAction?.Invoke(expressionConfig);

            return builder;
        }
    }
}
