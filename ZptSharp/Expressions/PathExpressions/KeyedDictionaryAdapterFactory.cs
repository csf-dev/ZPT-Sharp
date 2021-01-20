using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// This factory class gets a representation of an object as an
    /// <see cref="IDictionary{TKey, TValue}"/>, where the key-type is a specified generic type
    /// and the value-type is always <see cref="System.Object"/>.
    /// If required, this includes down-casting/boxing the value-type to object.
    /// </summary>
    internal class KeyedDictionaryAdapterFactory
    {
        /// <summary>
        /// <para>
        /// If it is possible, this method gets a representation of the <paramref name="object"/>
        /// in the form of an <c>IDictionary&lt;TKey,object&gt;</c>.
        /// </para>
        /// <para>
        /// If the object may be readily cast to that interface then this is done.  If the object
        /// implements any other <see cref="IDictionary{TKey, TValue}"/> interface where the key-type
        /// is <typeparamref name="TKey"/> then it is wrapped using an adapter class, allowing limited reading
        /// as if it were <c>IDictionary&lt;TKey,object&gt;</c>.  If neither of the previous scenarios
        /// apply, or if the object is null, then this method returns a null reference.
        /// </para>
        /// </summary>
        /// <returns>The <paramref name="object"/>, cast or wrapped as a <c>IDictionary&lt;TKey,object&gt;</c>,
        /// or a null reference.</returns>
        /// <param name="object">The object for which we will attempt to convert or wrap as a <c>IDictionary&lt;TKey,object&gt;</c>.</param>
        /// <typeparam name="TKey">The key-type of the desired dictionary.</typeparam>
        internal IDictionary<TKey,object> GetDictionary<TKey>(object @object)
        {
            if (ReferenceEquals(@object, null)) return null;

            // A small optimisation. There's no need to mess about if our object
            // already implements the desired interface directly.
            if (@object is IDictionary<TKey, object> alreadyTheRightType)
                return alreadyTheRightType;

            var dictionaryInterface = GetKeyedGenericDictionaryInterface<TKey>(@object);
            if (dictionaryInterface == null) return null;

            // The object does implement IDictionary<TKey,TValue>, but the generic type parameter TValue on that
            // interface isn't System.Object.  What we need to do is to wrap the object in an adapter,
            // so that we can access it (for limited read-related operations) as if it were
            // IDictionary<TKey,object>.  We do that by plugging 'whatever type TValue is' into
            // GenericDictionaryAdapter<TKey,TValue> and creating an instance of that, wrapping the object.

            var genericDictionaryAdapterType = typeof(GenericDictionaryAdapter<,>)
                .MakeGenericType(typeof(TKey), dictionaryInterface.GenericTypeArguments[1]);
            return (IDictionary<TKey, object>)Activator.CreateInstance(genericDictionaryAdapterType, @object);
        }

        /// <summary>
        /// <para>
        /// If the <paramref name="object"/> implements any <see cref="IDictionary{TKey, TValue}"/>
        /// interface, where the key-type is <typeparamref name="TKey"/>, then this method
        /// returns a reference to the first such interface type.
        /// </para>
        /// <para>
        /// If the object implements no such interface then it will return a null reference.
        /// </para>
        /// </summary>
        /// <returns>The string-keyed dictionary interface type, or a <see langword="null"/> reference.</returns>
        /// <param name="object">The object for which to get an interface.</param>
        Type GetKeyedGenericDictionaryInterface<TKey>(object @object)
        {
            return (from @interface in @object.GetType().GetInterfaces()
                    where @interface.IsGenericType
                    let genericInterface = @interface.GetGenericTypeDefinition()
                    where
                         genericInterface == typeof(IDictionary<,>)
                         && @interface.GenericTypeArguments[0] == typeof(TKey)
                    select @interface)
                .FirstOrDefault();
        }

        #region contained adapter type

        /// <summary>
        /// An adapter/wrapper for a generic dictionary of <typeparamref name="TKey"/> and <typeparamref name="TValue"/>.
        /// This provides very limited read-only functionality as if the dictionary was actually
        /// for <typeparamref name="TKey"/> and <see cref="object"/>.
        /// </summary>
        internal class GenericDictionaryAdapter<TKey, TValue> : IDictionary<TKey, object>
        {
            readonly IDictionary<TKey, TValue> wrapped;

            object IDictionary<TKey, object>.this[TKey key]
            {
                get => wrapped[key];
                set => throw new NotSupportedException();
            }

            bool IDictionary<TKey, object>.ContainsKey(TKey key) => wrapped.ContainsKey(key);

            public GenericDictionaryAdapter(IDictionary<TKey, TValue> wrapped)
            {
                this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            }

            // A big block of unsupported & unused functionality.
            // This is all irrelevant in the context in which this class is used.
            ICollection<TKey> IDictionary<TKey, object>.Keys => throw new NotSupportedException();
            ICollection<object> IDictionary<TKey, object>.Values => throw new NotSupportedException();
            int ICollection<KeyValuePair<TKey, object>>.Count => throw new NotSupportedException();
            bool ICollection<KeyValuePair<TKey, object>>.IsReadOnly => throw new NotSupportedException();
            void IDictionary<TKey, object>.Add(TKey key, object value) => throw new NotSupportedException();
            void ICollection<KeyValuePair<TKey, object>>.Add(KeyValuePair<TKey, object> item) => throw new NotSupportedException();
            void ICollection<KeyValuePair<TKey, object>>.Clear() => throw new NotSupportedException();
            bool ICollection<KeyValuePair<TKey, object>>.Contains(KeyValuePair<TKey, object> item) => throw new NotSupportedException();
            void ICollection<KeyValuePair<TKey, object>>.CopyTo(KeyValuePair<TKey, object>[] array, int arrayIndex) => throw new NotSupportedException();
            IEnumerator<KeyValuePair<TKey, object>> IEnumerable<KeyValuePair<TKey, object>>.GetEnumerator() => throw new NotSupportedException();
            bool IDictionary<TKey, object>.Remove(TKey key) => throw new NotSupportedException();
            bool ICollection<KeyValuePair<TKey, object>>.Remove(KeyValuePair<TKey, object> item) => throw new NotSupportedException();
            bool IDictionary<TKey, object>.TryGetValue(TKey key, out object value) => throw new NotSupportedException();
            IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
        }

        #endregion
    }
}
