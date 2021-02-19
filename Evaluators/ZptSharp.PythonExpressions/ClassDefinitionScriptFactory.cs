using System;
using System.Collections.Generic;
using System.Linq;

namespace ZptSharp.Expressions.PythonExpressions
{
    /// <summary>
    /// Implementation of <see cref="IGetsClassDefinitionScript"/> which generates a <see cref="ClassDefinitionScript"/>.
    /// </summary>
    public class ClassDefinitionScriptFactory : IGetsClassDefinitionScript
    {
        const string pythonClassName = "PythonExpression";

        static Func<dynamic, IList<object>, object> Evaluator
            => (instance, vars) => instance.evaluate(vars);

        /// <summary>
        /// Gets the python script, representing a class definition.
        /// </summary>
        /// <returns>The script.</returns>
        /// <param name="expression">Expression.</param>
        /// <param name="variableDefinitions">Variable definitions.</param>
        public ClassDefinitionScript GetScript(string expression, IList<Variable> variableDefinitions)
        {
            var variableNames = variableDefinitions.Select(x => x.Name).ToList();
            var script = GetScriptBody(expression, variableNames);
            return new ClassDefinitionScript(pythonClassName, script, Evaluator);
        }

        /// <summary>
        /// The script body is created from a very small class.  The two lines which are written dynamically
        /// are a variable-assignment line, and the evaluation (and return) of the expression itself.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The variable assignment allows the configuration of the contextual environment within which
        /// the expression is executed.  This is done by passing-in an ordered list of variable values
        /// via the <c>__params</c> parameter.  It is important that the values passed-in here are in the
        /// same order as the corresponding <paramref name="variableNames"/>.
        /// </para>
        /// <para>
        /// Depending upon how many variable assignments are required, this might use Python list-unpacking
        /// in order to assign many variables at once from a single collection parameter.
        /// See https://stackoverflow.com/a/34308407/6221779 for an example of the technique.
        /// </para>
        /// </remarks>
        /// <returns>The script body.</returns>
        /// <param name="expression">Expression.</param>
        /// <param name="variableNames">Variable names.</param>
        static string GetScriptBody(string expression, IList<string> variableNames)
        {
            var variableAssignments = GetVariableAssignments(variableNames);

            return $@"
class PythonExpression:
    def evaluate(self, __params):
        {variableAssignments}
        return {expression}";
        }

        static string GetVariableAssignments(IList<string> variableNames)
        {
            if (variableNames.Count == 0) return String.Empty;
            if (variableNames.Count == 1)
                return $"{variableNames.Single()} = __params[0]";

            return $"{String.Join(", ", variableNames)} = __params";
        }
    }
}
