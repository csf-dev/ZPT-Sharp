using System;
using Microsoft.Scripting.Hosting;

namespace ZptSharp.Expressions.PythonExpressions
{
    /// <summary>
    /// An object which provides a <see cref="ScriptEngine"/> instance.
    /// </summary>
    public interface IGetsScriptEngine
    {
        /// <summary>
        /// Gets the script engine.
        /// </summary>
        /// <value>The script engine.</value>
        ScriptEngine ScriptEngine { get; }
    }
}
