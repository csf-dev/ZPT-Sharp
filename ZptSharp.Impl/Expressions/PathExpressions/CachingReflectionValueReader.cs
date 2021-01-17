using System;
using System.Reflection;
using System.Runtime.Caching;
using System.Linq.Expressions;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Implementation of <see cref="IGetsValueFromReflection"/> which uses caching for
    /// property and method access.  Field access is not cached.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Probably, on the first call alone, speed is not improved.  However, because of the caching,
    /// all subsequent usages of reflection will offer speeds similar to native/non-reflection usage.
    /// </para>
    /// <para>
    /// Field access is not cached as there are no 'big wins' to be had.
    /// </para>
    /// </remarks>
    public class CachingReflectionValueReader : IGetsValueFromReflection
    {
        readonly ObjectCache cache;

        /// <summary>
        /// Gets the value from the specified property and target object.
        /// </summary>
        /// <param name="property">The property to use in order to get the value.</param>
        /// <param name="target">The target object from which to get the value.</param>
        /// <returns>The property value.</returns>
        public object GetValue(PropertyInfo property, object target)
        {
            if (LooksLikeAPropertyOfAnAnonymousType(property)) return property.GetValue(target);
            
            return GetValue(property.GetGetMethod(), target);
        }

        /// <summary>
        /// Gets the value from the specified (parameterless) method and target object.
        /// </summary>
        /// <param name="method">The method to use in order to get the value.</param>
        /// <param name="target">The target object from which to get the value.</param>
        /// <returns>The method return value.</returns>
        public object GetValue(MethodInfo method, object target)
        {
            var methodDelegate = GetDelegate(method);
            return methodDelegate(target);
        }

        /// <summary>
        /// Gets the value from the specified field and target object.
        /// </summary>
        /// <param name="field">The field to use in order to get the value.</param>
        /// <param name="target">The target object from which to get the value.</param>
        /// <returns>The field value.</returns>
        public object GetValue(FieldInfo field, object target) => field.GetValue(target);

        static string GetCacheKey(MethodInfo method)
            => $"Method_{method.DeclaringType.FullName}.{method.Name}";

        Func<object, object> GetDelegate(MethodInfo method)
        {
            var key = GetCacheKey(method);
            var cachedDelegate = cache.Get(key) as Func<object, object>;
            if (cachedDelegate != null) return cachedDelegate;

            var createdDelegate = CreateDelegate(method);
            cache.Add(key, createdDelegate, DateTimeOffset.MaxValue);
            return createdDelegate;
        }

        static Func<object, object> CreateDelegate(MethodInfo method)
        {
            var param = Expression.Parameter(method.DeclaringType);
            var callExpression = Expression.Call(param, method);
            var resultExpression = Expression.Convert(callExpression, typeof(object));
            var compiledDelegate = Expression.Lambda(resultExpression, param).Compile();
            return obj => compiledDelegate.DynamicInvoke(obj);
        }

        static bool LooksLikeAPropertyOfAnAnonymousType(PropertyInfo property)
        {
            if (!property.DeclaringType.IsSealed) return false;
            if (property.DeclaringType.BaseType != typeof(object)) return false;
            if (property.DeclaringType.GetCustomAttribute<System.Runtime.CompilerServices.CompilerGeneratedAttribute>() == null) return false;
            return true;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CachingReflectionValueReader"/>.
        /// </summary>
        public CachingReflectionValueReader()
        {
            cache = new MemoryCache(typeof(CachingReflectionValueReader).FullName);
        }
    }
}