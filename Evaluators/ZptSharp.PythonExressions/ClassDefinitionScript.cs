using System;
using System.Collections.Generic;
using System.Linq;

namespace ZptSharp.Expressions.PythonExpressions
{
    /// <summary>
    /// Represents an executable python script which represents a python class definition.
    /// </summary>
    public sealed class ClassDefinitionScript : IEquatable<ClassDefinitionScript>
    {
        readonly Func<dynamic, IList<object>, object> evaluator;

        /// <summary>
        /// Gets the python class name, within the <see cref="ScriptBody"/>.
        /// </summary>
        /// <value>The name of the class.</value>
        public string ClassName { get; }

        /// <summary>
        /// Gets the body of the python script.
        /// </summary>
        /// <value>The script body.</value>
        public string ScriptBody { get; }

        /// <summary>
        /// Executes the evaluation function contained within the specified <paramref name="instance"/>
        /// of a class (which was created via this script object) and returns the result.
        /// </summary>
        /// <returns>The result of executing the evaluation function.</returns>
        /// <param name="instance">The class instance.</param>
        /// <param name="variables">The variable values.</param>
        public object Evaluate(dynamic instance, IList<Variable> variables)
            => evaluator(instance, variables.Select(x => x.Value).ToList());

        /// <summary>
        /// Determines whether the specified <see cref="ClassDefinitionScript"/> is equal
        /// to the current <see cref="ClassDefinitionScript"/>.
        /// </summary>
        /// <param name="other">The <see cref="ClassDefinitionScript"/> to compare with the current <see cref="ClassDefinitionScript"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="ClassDefinitionScript"/> is equal to
        /// the current <see cref="ClassDefinitionScript"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(ClassDefinitionScript other)
        {
            if (other == null) return false;
            if (ReferenceEquals(other, this)) return true;

            return String.Equals(ClassName, other.ClassName, StringComparison.InvariantCulture)
                && String.Equals(ScriptBody, other.ScriptBody, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="ClassDefinitionScript"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="ClassDefinitionScript"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="ClassDefinitionScript"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => Equals(obj as ClassDefinitionScript);

        /// <summary>
        /// Serves as a hash function for a <see cref="ClassDefinitionScript"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() => ClassName.GetHashCode() ^ ScriptBody.GetHashCode();

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassDefinitionScript"/> class.
        /// </summary>
        /// <param name="className">Class name.</param>
        /// <param name="scriptBody">Script body.</param>
        /// <param name="evaluator">A function which is executed to execute the function (of the class) which evaluates the expression.</param>
        public ClassDefinitionScript(string className,
                                     string scriptBody,
                                     Func<dynamic,IList<object>, object> evaluator)
        {
            ClassName = className ?? throw new ArgumentNullException(nameof(className));
            ScriptBody = scriptBody ?? throw new ArgumentNullException(nameof(scriptBody));
            this.evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
        }
    }
}
