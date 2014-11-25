using System;
using Options.Core;

namespace Options.Linq
{
    public static partial class Option
    {
        /// <summary>
        ///     Convert <paramref name="deferred" /> to an <see cref="IPublishedOption{T}" />.
        /// </summary>
        /// <param name="deferred">An <see cref="IDeferredOption{T}" />.</param>
        /// <typeparam name="T">The type contained when <paramref name="deferred" /> is evaluated.</typeparam>
        /// <returns>An <see cref="IPublishedOption{T}" /> that will evaluate <paramref name="deferred" /> only once.</returns>
        /// <remarks>
        ///     <para>
        ///         The evaluation of <paramref name="deferred" /> will be thread-safe.
        ///     </para>
        /// </remarks>
        public static IPublishedOption<T> Publish<T>(this IDeferredOption<T> deferred)
        {
            return deferred.Publish(true);
        }

        /// <summary>
        ///     Convert <paramref name="deferred" /> to an <see cref="IPublishedOption{T}" />.
        /// </summary>
        /// <param name="deferred">An <see cref="IDeferredOption{T}" />.</param>
        /// <param name="isThreadSafe">
        ///     Indicates whether the one-time evaluation of <paramref name="deferred" /> should be
        ///     protected against race conditions.
        /// </param>
        /// <typeparam name="T">The type contained when <paramref name="deferred" /> is evaluated.</typeparam>
        /// <returns>An <see cref="IPublishedOption{T}" /> that will evaluate <paramref name="deferred" /> only once.</returns>
        public static IPublishedOption<T> Publish<T>(this IDeferredOption<T> deferred, bool isThreadSafe)
        {
            return new PublishedOption<T>(deferred, isThreadSafe);
        }

        /// <summary>
        ///     Return <paramref name="published" /> as an <see cref="IDeferredOption{T}" />.
        /// </summary>
        /// <param name="published">The <see cref="IPublishedOption{T}" /> to treat as <see cref="IDeferredOption{T}" />.</param>
        /// <typeparam name="T">The type eventually contained by <paramref name="published" />.</typeparam>
        /// <returns>An <see cref="IDeferredOption{T}" /></returns>
        /// <remarks>
        ///     <para>
        ///         This function does not &quot;undo&quot; publication. The returned <see cref="IDeferredOption{T}" />
        ///         will still only evaluate once.
        ///     </para>
        ///     The return value of this function is <i>not</i> a new reference. It is a
        ///     convenience method to allow <paramref name="published" /> to be supplied as a
        ///     parameter to functions that accept both <see cref="IDeferredOption{T}" /> and
        ///     <see cref="IOption{T}" />.
        /// </remarks>
        public static IDeferredOption<T> AsDeferred<T>(this IPublishedOption<T> published)
        {
            return published;
        }

        /// <summary>
        ///     Return the published option as an option.
        /// </summary>
        /// <param name="published">The published option to treat as evaluated.</param>
        /// <typeparam name="T">The type eventually contained by <paramref name="published" />.</typeparam>
        /// <returns>An <see cref="IOption{T}" /></returns>
        /// <remarks>
        ///     <para>This function does <i>not</i> force evaluation of <paramref name="published" />.</para>
        ///     <para>
        ///         The return value of this function is <i>not</i> a new reference. It is a
        ///         convenience method to allow <paramref name="published" /> to be supplied as a
        ///         parameter to functions that accept both <see cref="IDeferredOption{T}" /> and
        ///         <see cref="IOption{T}" />.
        ///     </para>
        /// </remarks>
        public static IOption<T> AsOption<T>(this IPublishedOption<T> published)
        {
            return published;
        }

        private class PublishedOption<T> : IPublishedOption<T>
        {
            private readonly Lazy<IOption<T>> _lazy;

            internal PublishedOption(IDeferredOption<T> deferred, bool isThreadSafe)
            {
                _lazy = new Lazy<IOption<T>>(() => deferred.Safe().Evaluate(), isThreadSafe);
            }

            public bool HasEvaluated
            {
                get { return _lazy.IsValueCreated; }
            }

            public IDeferredOption<TResult> Project<TResult>(Func<IOption<T>, IOption<TResult>> selector)
            {
                return new GenerateOption<TResult>(() => selector(_lazy.Value.Safe()));
            }

            public IOption<T> Evaluate()
            {
                return _lazy.Value.Safe();
            }

            public TResult Handle<TResult>(Func<T, TResult> ifValue, Func<TResult> ifNone)
            {
                return _lazy.Value.Safe().Handle(ifValue, ifNone);
            }
        }
    }
}