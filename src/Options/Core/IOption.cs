using System;

namespace Options.Core
{
    /// <summary>
    /// Implemented by types that may or may not contain a <typeparamref name="T"/> value.
    /// </summary>
    /// <typeparam name="T">The type of value that may be present</typeparam>
    public interface IOption<out T>
    {
        /// <summary>
        /// Execute one of two functions, varying whether the <see cref="IOption{T}"/> contains a value.
        /// </summary>
        /// <param name="ifValue">The <see cref="Func{T1,TResult}"/> to execute when a value is present.</param>
        /// <param name="ifNone">The <see cref="Func{TResult}"/> to execute when a value is not present.</param>
        /// <typeparam name="TResult">The result of executing <paramref name="ifValue"/> or <paramref name="ifNone"/>.</typeparam>
        /// <returns>A <typeparamref name="TResult"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="ifValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ifNone"/> is <see langword="null"/>.</exception>
        TResult Handle<TResult>(Func<T, TResult> ifValue, Func<TResult> ifNone);
    }
}