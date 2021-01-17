using System.Reflection;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Implementation of <see cref="IGetsValueFromReflection"/> which uses plain/uncached reflection.
    /// </summary>
    public class ReflectionValueReader : IGetsValueFromReflection
    {
        /// <summary>
        /// Gets the value from the specified property and target object.
        /// </summary>
        /// <param name="property">The property to use in order to get the value.</param>
        /// <param name="target">The target object from which to get the value.</param>
        /// <returns>The property value.</returns>
        public object GetValue(PropertyInfo property, object target)
            => property.GetValue(target);

        /// <summary>
        /// Gets the value from the specified (parameterless) method and target object.
        /// </summary>
        /// <param name="method">The method to use in order to get the value.</param>
        /// <param name="target">The target object from which to get the value.</param>
        /// <returns>The method return value.</returns>
        public object GetValue(MethodInfo method, object target)
            => method.Invoke(target, new object[0]);

        /// <summary>
        /// Gets the value from the specified field and target object.
        /// </summary>
        /// <param name="field">The field to use in order to get the value.</param>
        /// <param name="target">The target object from which to get the value.</param>
        /// <returns>The field value.</returns>
        public object GetValue(FieldInfo field, object target)
            => field.GetValue(target);
    }
}