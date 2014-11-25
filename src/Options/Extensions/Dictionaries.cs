using System;
using System.Collections.Generic;
using Options.Core;

namespace Options.Extensions
{
    /// <summary>
    /// Functions to ease work with <see cref="IDictionary{TKey,TValue}"/>
    /// </summary>
    public static class Dictionaries
    {
        /// <summary>
        /// Get an <see cref="IOption{T}"/> containing the value for <paramref name="key"/> or no value, if not found or <see langword="null"/>.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/> that might contain a value for <paramref name="key"/>.</param>
        /// <param name="key"></param>
        /// <typeparam name="TKey">The type of keys in <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">The type of values in <paramref name="dictionary"/>.</typeparam>
        /// <returns>An <see cref="IOption{T}"/> containing the value for <paramref name="key"/> or no value, if not found or <see langword="null"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <see langword="null"/>.</exception>
        public static IOption<TValue> OptionGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (ReferenceEquals(dictionary, null))
            {
                throw new ArgumentNullException("dictionary");
            }
            TValue value;
            return dictionary.TryGetValue(key, out value) ? Linq.Option.Create(value) : Linq.Option.Create<TValue>();
        }

        /// <summary>
        /// Get an <see cref="IOption{T}"/> containing the value for <paramref name="key"/> or no value, if not found or <see langword="null"/>.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/> that might contain a value for <paramref name="key"/>.</param>
        /// <param name="key"></param>
        /// <typeparam name="TKey">The type of keys in <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">The type of values in <paramref name="dictionary"/>.</typeparam>
        /// <returns>An <see cref="IOption{T}"/> containing the value for <paramref name="key"/> or no value, if not found or <see langword="null"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <see langword="null"/>.</exception>
        public static IOption<TValue> OptionGetValue<TKey, TValue>(this IDictionary<TKey, TValue?> dictionary, TKey key)
            where TValue:struct 
        {
            if (ReferenceEquals(dictionary, null))
            {
                throw new ArgumentNullException("dictionary");
            }
            TValue? value;
            return dictionary.TryGetValue(key, out value) ? Linq.Option.Create(value) : Linq.Option.Create<TValue>();
        }
    }
}