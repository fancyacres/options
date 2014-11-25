using System;

namespace Options.Core
{
    /// <summary>
    /// Implemented by types that may or may not eventually contain a value.
    /// </summary>
    /// <typeparam name="T">The type that may eventually be contained.</typeparam>
    public interface IDeferredOption<out T>
    {
        /// <summary>
        /// Projects the eventual value to a new form.
        /// </summary>
        /// <param name="selector">A function used to project the new value.</param>
        /// <typeparam name="TResult">The type that may eventually be contained by the returned <see cref="IDeferredOption{T}"/>.</typeparam>
        /// <returns>An <see cref="IDeferredOption{T}"/></returns>
        IDeferredOption<TResult> Project<TResult>(Func<IOption<T>, IOption<TResult>> selector);

        /// <summary>
        /// Evaluate the deferred computation and return an <see cref="IOption{T}"/>.
        /// </summary>
        /// <returns>An <see cref="IOption{T}"/>.</returns>
        IOption<T> Evaluate();
    }
}