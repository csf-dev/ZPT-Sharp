using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ZptSharp.Expressions.PythonExpressions
{
    /// <summary>
    /// An evaluator class which deals with the logic required in order to execute a python expression within
    /// a 'context' of a number of variable definitions.  This uses an injected
    /// <see cref="Microsoft.Scripting.Hosting.ScriptEngine"/> to perform its work.
    /// </summary>
    public class ScriptEngineEvaluator : IEvaluatesPythonExpression
    {
        readonly IGetsScriptEngine engineContainer;
        readonly IGetsClassDefinitionScript scriptProvider;
        readonly ILogger logger;

        Microsoft.Scripting.Hosting.ScriptEngine Engine
            => engineContainer.ScriptEngine;

        /// <summary>
        /// Evaluates the python expression and returns the result.
        /// </summary>
        /// <returns>The evaluated expression.</returns>
        /// <param name="expression">Expression.</param>
        /// <param name="variableDefinitions">Variable definitions.</param>
        /// <param name="token">Token.</param>
        public Task<object> EvaluateExpressionAsync(string expression, IList<Variable> variableDefinitions, CancellationToken token = default)
        {
            var classDefinitionScript = scriptProvider.GetScript(expression, variableDefinitions);
            var classInstance = GetInstanceOfClass(classDefinitionScript);

            if(logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace(@"Evaluating a python script
Source expression:{expression}
   Variable names:{variable_names}

Script body
-----------
{script_body}",
                                expression,
                                variableDefinitions.Select(x => x.Name).ToList(),
                                classDefinitionScript.ScriptBody);
            }

            object result = classDefinitionScript.Evaluate(classInstance, variableDefinitions);
            return Task.FromResult(result);
        }

        /// <summary>
        /// <para>
        /// Where the <paramref name="classDefinitionScript"/> defines the body of a python class
        /// (and exposes its name), this method executes that script body and proceeds to create
        /// an instance of that python class.
        /// </para>
        /// <para>
        /// This is a <c>dynamic</c> object, because IronPython (&amp; the MS Script Host Engine abstraction)
        /// return dynamic types.
        /// </para>
        /// </summary>
        /// <returns>The instance of the python class, created using the definition script.</returns>
        /// <param name="classDefinitionScript">Class definition script.</param>
        dynamic GetInstanceOfClass(ClassDefinitionScript classDefinitionScript)
        {
            var scope = Engine.CreateScope();
            Engine.Execute(classDefinitionScript.ScriptBody, scope);
            var classDefinition = scope.GetVariable(classDefinitionScript.ClassName);
            return Engine.Operations.CreateInstance(classDefinition);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ScriptEngineEvaluator"/> class.
        /// </summary>
        /// <param name="engineContainer">Engine container.</param>
        /// <param name="scriptProvider">Script provider.</param>
        /// <param name="logger">Logger</param>
        public ScriptEngineEvaluator(IGetsScriptEngine engineContainer,
                                          IGetsClassDefinitionScript scriptProvider,
                                          ILogger<ScriptEngineEvaluator> logger)
        {
            this.engineContainer = engineContainer ?? throw new ArgumentNullException(nameof(engineContainer));
            this.scriptProvider = scriptProvider ?? throw new ArgumentNullException(nameof(scriptProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
