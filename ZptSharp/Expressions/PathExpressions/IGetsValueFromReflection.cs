using System.Reflection;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// An object which can get values from a target object by use of reflection.
    /// </summary>
    public interface IGetsValueFromReflection
    {
        /// <summary>
        /// Gets the value from the specified property and target object.
        /// </summary>
        /// <param name="property">The property to use in order to get the value.</param>
        /// <param name="target">The target object from which to get the value.</param>
        /// <returns>The property value.</returns>
        object GetValue(PropertyInfo property, object target);

        /// <summary>
        /// Gets the value from the specified (parameterless) method and target object.
        /// </summary>
        /// <param name="method">The method to use in order to get the value.</param>
        /// <param name="target">The target object from which to get the value.</param>
        /// <returns>The method return value.</returns>
        object GetValue(MethodInfo method, object target);

        /// <summary>
        /// Gets the value from the specified field and target object.
        /// </summary>
        /// <param name="field">The field to use in order to get the value.</param>
        /// <param name="target">The target object from which to get the value.</param>
        /// <returns>The field value.</returns>
        object GetValue(FieldInfo field, object target);
    }
}