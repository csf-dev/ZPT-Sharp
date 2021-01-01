using System;
using System.Collections.Generic;

namespace ZptSharp.Expressions.PythonExpressions
{
    /// <summary>
    /// An object which gets an executable script from an expression and
    /// a collection of variable definitions.  The format of that script
    /// is a python class definition.
    /// </summary>
    public interface IGetsClassDefinitionScript
    {
        /// <summary>
        /// Gets the python script, representing a class definition.
        /// </summary>
        /// <returns>The script.</returns>
        /// <param name="expression">Expression.</param>
        /// <param name="variableDefinitions">Variable definitions.</param>
        ClassDefinitionScript GetScript(string expression, IList<Variable> variableDefinitions);
    }
}
