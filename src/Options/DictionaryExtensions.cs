using System;
using System.Collections.Generic;

namespace Options
{
    ///<summary>
    ///	Provides extensions to <see cref = "IDictionary{TKey,TValue}" />
    ///</summary>
    public static class DictionaryExtensions
    {
        ///<summary>
        /// Returns an <see cref="Option{TOption}"/> containing the element of <paramref name="key"/> or no value, if there is no element for <paramref name="key"/>.
        ///</summary>
        ///<param name="dictionary">The <see cref = "IDictionary{TKey,TValue}" /> to use for lookup.</param>
        ///<param name="key">The key of the element to get.</param>
        ///<typeparam name="TKey">The type of keys in <paramref name="dictionary" />.</typeparam>
        ///<typeparam name="TValue">The type of values in the <paramref name="dictionary" />.</typeparam>
        ///<returns>An <see cref="Option{TOption}"/> of <typeparamref name="TValue"/> internal type</returns>
        ///<exception cref="ArgumentNullException"><paramref name="dictionary"/> is null.</exception>
        public static Option<TValue> OptionGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            TValue value;
            return dictionary.TryGetValue(key, out value) ?
                new Option<TValue>(value) :
                new Option<TValue>();
        }
    }
}