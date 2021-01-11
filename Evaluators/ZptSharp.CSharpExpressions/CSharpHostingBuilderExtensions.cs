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
