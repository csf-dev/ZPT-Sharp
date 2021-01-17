using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;


namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Implementation of <see cref="IGetsValueFromObject"/> which uses reflection to get values from
    /// public properties, methods or fields of objects.
    /// </summary>
    public class ReflectionObjectValueProvider : IGetsValueFromObject
    {
        readonly IGetsValueFromObject wrapped;
        readonly IGetsValueFromReflection valueProvider;

        /// <summary>
        /// <para>
        /// Attempts to get a value for a named reference, from the specified object.
        /// </para>
        /// <para>
        /// This will return a value by using the getter of a property, or by executing a
        /// method (which takes no parameters) or by accessing a field (checking for members in this same order).
        /// Only public members will be considered.
        /// </para>
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The value name.</param>
        /// <param name="object">The object from which to retrieve the value.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<GetValueResult> TryGetValueAsync(string name, object @object, CancellationToken cancellationToken = default)
        {
            if (ReferenceEquals(@object, null))
                return wrapped.TryGetValueAsync(name, @object, cancellationToken);

            // This method could totally benefit from some
            // caching & performance improvements.

            var objectType = @object.GetType();

            if (TryGetProperty(name, objectType, out var property))
                return Task.FromResult(GetValueResult.For(valueProvider.GetValue(property, @object)));

            if (TryGetMethod(name, objectType, out var method))
                return Task.FromResult(GetValueResult.For(valueProvider.GetValue(method, @object)));

            if (TryGetField(name, objectType, out var field))
                return Task.FromResult(GetValueResult.For(valueProvider.GetValue(field, @object)));

            return wrapped.TryGetValueAsync(name, @object, cancellationToken);
        }

        static bool TryGetProperty(string name, Type type, out PropertyInfo property)
        {
            property = type.GetProperty(name);
            return property != null && property.CanRead;
        }

        static bool TryGetMethod(string name, Type type, out MethodInfo method)
        {
            method = type.GetMethod(name, Type.EmptyTypes);
            return method != null && method.ReturnType != typeof(void);
        }

        static bool TryGetField(string name, Type type, out FieldInfo field)
        {
            field = type.GetField(name);
            return field != null;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ReflectionObjectValueProvider"/> class.
        /// </summary>
        /// <param name="wrapped">The wrapped service.</param>
        /// <param name="valueProvider">The reflection value provider</param>
        public ReflectionObjectValueProvider(IGetsValueFromObject wrapped, IGetsValueFromReflection valueProvider)
        {
            this.valueProvider = valueProvider ?? throw new ArgumentNullException(nameof(valueProvider));
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}
