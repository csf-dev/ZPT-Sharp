using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace ZptSharp.Expressions.PythonExpressions
{
    /// <summary>
    /// A container for a <see cref="ScriptEngine"/> instance which may be used
    /// for evaluatting python scripts.  This is the only type which directly
    /// references IronPython.
    /// </summary>
    public class ScriptEngineContainer : IGetsScriptEngine
    {
        /// <summary>
        /// Gets the <see cref="ScriptEngine"/> instance.
        /// </summary>
        /// <value>The script engine.</value>
        public ScriptEngine ScriptEngine { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptEngineContainer"/> class.
        /// </summary>
        public ScriptEngineContainer()
        {
            ScriptEngine = Python.CreateEngine();
        }
    }
}
