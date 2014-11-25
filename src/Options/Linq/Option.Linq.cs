using System;
using Options.Core;

namespace Options.Linq
{
    public static partial class Option
    {
        #region Plain ole' Option

        /// <summary>
        ///     Projects the value potentially contained by <paramref name="option" /> to a new form.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}" /> to transform.</param>
        /// <param name="selector">The <see cref="Func{T,TResult}" /> used to transform the value.</param>
        /// <typeparam name="T">The type of value potentially contained by <paramref name="option" />.</typeparam>
        /// <typeparam name="TResult">The return type of <paramref name="selector" />.</typeparam>
        /// <returns>An <see cref="IOption{T}" /> of <typeparamref name="TResult" /></returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector" /> is <see langword="null" />.</exception>
        public static IOption<TResult> Select<T, TResult>(
            this IOption<T> option,
            Func<T, TResult> selector)
        {
            if (ReferenceEquals(selector, null))
            {
                throw new ArgumentNullException("selector");
            }
            return option.Safe().Handle(
                t => Create(selector(t)),
                Create<TResult>);
        }

        /// <summary>
        ///     Filters the value potentially contained by <paramref name="option" />.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}" /> to filter.</param>
        /// <param name="predicate">The <see cref="Func{T,TResult}" /> used to filter the value.</param>
        /// <typeparam name="T">The type of value potentially contained by <paramref name="option" />.</typeparam>
        /// <returns>An <see cref="IOption{T}" /> of <typeparamref name="T" /></returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate" /> is <see langword="null" />.</exception>
        public static IOption<T> Where<T>(
            this IOption<T> option,
            Func<T, bool> predicate)
        {
            if (ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException("predicate");
            }
            return option.Safe().Handle(
                t => predicate(t) ? Create(t) : Create<T>(),
                Create<T>);
        }

        /// <summary>
        ///     Projects the value potentially contained by an <see cref="IOption{T}" /> to a new form.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}" /> to transform.</param>
        /// <param name="intermediateSelector">
        ///     A function accepting the potential value of <paramref name="option" /> and returning
        ///     an <see cref="IOption{T}" />.
        /// </param>
        /// <param name="resultSelector">
        ///     A function accepting the potential values of <paramref name="option" /> and the
        ///     <see cref="IOption{T}" /> returned by <paramref name="intermediateSelector" /> and returning
        ///     <typeparamref name="TResult" />.
        /// </param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option" />.</typeparam>
        /// <typeparam name="TIntermediate">The type potentially returned by <paramref name="intermediateSelector" />.</typeparam>
        /// <typeparam name="TResult">The type returned by <paramref name="resultSelector" />.</typeparam>
        /// <returns>An <see cref="IOption{T}" /> of <typeparamref name="TResult" /></returns>
        /// <exception cref="ArgumentNullException"><paramref name="intermediateSelector" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector" /> is <see langword="null" />.</exception>
        public static IOption<TResult> SelectMany<T, TIntermediate, TResult>(
            this IOption<T> option,
            Func<T, IOption<TIntermediate>> intermediateSelector,
            Func<T, TIntermediate, TResult> resultSelector)
        {
            if (ReferenceEquals(intermediateSelector, null))
            {
                throw new ArgumentNullException("intermediateSelector");
            }
            if (ReferenceEquals(resultSelector, null))
            {
                throw new ArgumentNullException("resultSelector");
            }
            return option.Safe().Handle(
                t => intermediateSelector(t).Handle(ti => Create(resultSelector(t, ti)),
                    Create<TResult>),
                Create<TResult>);
        }

        /// <summary>
        ///     Projects the value potentially contained by an <see cref="IOption{T}" /> to a new form.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}" /> to transform.</param>
        /// <param name="selector">
        ///     A function accepting the potential value of <paramref name="option" /> and returning an
        ///     <see cref="IOption{T}" />.
        /// </param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option" />.</typeparam>
        /// <typeparam name="TResult">The type potentially returned by <paramref name="selector" />.</typeparam>
        /// <returns>An <see cref="IOption{T}" /> of <typeparamref name="TResult" /></returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector" /> is <see langword="null" />.</exception>
        public static IOption<TResult> SelectMany<T, TResult>(
            this IOption<T> option,
            Func<T, IOption<TResult>> selector)
        {
            if (ReferenceEquals(selector, null))
            {
                throw new ArgumentNullException("selector");
            }
            return option.Safe().Handle(selector, Create<TResult>);
        }

        #endregion

        #region Deferred

        /// <summary>
        ///     Projects the value potentially contained by <paramref name="option" /> to a new form.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}" /> to transform.</param>
        /// <param name="selector">The <see cref="Func{T,TResult}" /> used to transform the value.</param>
        /// <typeparam name="T">The type of value potentially contained by <paramref name="option" />.</typeparam>
        /// <typeparam name="TResult">The return type of <paramref name="selector" />.</typeparam>
        /// <returns>An <see cref="IOption{T}" /> of <typeparamref name="TResult" /></returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector" /> is <see langword="null" />.</exception>
        public static IDeferredOption<TResult> Select<T, TResult>(
            this IDeferredOption<T> option,
            Func<T, TResult> selector)
        {
            if (ReferenceEquals(selector, null))
            {
                throw new ArgumentNullException("selector");
            }
            return option.Safe().Project(o => o.Select(selector));
        }

        /// <summary>
        ///     Filters the value potentially contained by <paramref name="option" />.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}" /> to filter.</param>
        /// <param name="predicate">The <see cref="Func{T,TResult}" /> used to filter the value.</param>
        /// <typeparam name="T">The type of value potentially contained by <paramref name="option" />.</typeparam>
        /// <returns>An <see cref="IOption{T}" /> of <typeparamref name="T" /></returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate" /> is <see langword="null" />.</exception>
        public static IDeferredOption<T> Where<T>(
            this IDeferredOption<T> option,
            Func<T, bool> predicate)
        {
            if (ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException("predicate");
            }
            return option.Safe().Project(o => o.Where(predicate));
        }

        /// <summary>
        ///     Projects the value potentially contained by an <see cref="IOption{T}" /> to a new form.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}" /> to transform.</param>
        /// <param name="intermediateSelector">
        ///     A function accepting the potential value of <paramref name="option" /> and returning
        ///     an <see cref="IOption{T}" />.
        /// </param>
        /// <param name="resultSelector">
        ///     A function accepting the potential values of <paramref name="option" /> and the
        ///     <see cref="IOption{T}" /> returned by <paramref name="intermediateSelector" /> and returning
        ///     <typeparamref name="TResult" />.
        /// </param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option" />.</typeparam>
        /// <typeparam name="TIntermediate">The type potentially returned by <paramref name="intermediateSelector" />.</typeparam>
        /// <typeparam name="TResult">The type returned by <paramref name="resultSelector" />.</typeparam>
        /// <returns>An <see cref="IOption{T}" /> of <typeparamref name="TResult" /></returns>
        /// <exception cref="ArgumentNullException"><paramref name="intermediateSelector" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector" /> is <see langword="null" />.</exception>
        public static IDeferredOption<TResult> SelectMany<T, TIntermediate, TResult>(
            this IDeferredOption<T> option,
            Func<T, IOption<TIntermediate>> intermediateSelector,
            Func<T, TIntermediate, TResult> resultSelector)
        {
            if (ReferenceEquals(intermediateSelector, null))
            {
                throw new ArgumentNullException("intermediateSelector");
            }
            if (ReferenceEquals(resultSelector, null))
            {
                throw new ArgumentNullException("resultSelector");
            }
            return option.Safe().Project(o => o.SelectMany(intermediateSelector, resultSelector));
        }

        /// <summary>
        ///     Projects the value potentially contained by an <see cref="IOption{T}" /> to a new form.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}" /> to transform.</param>
        /// <param name="intermediateSelector">
        ///     A function accepting the potential value of <paramref name="option" /> and returning
        ///     an <see cref="IOption{T}" />.
        /// </param>
        /// <param name="resultSelector">
        ///     A function accepting the potential values of <paramref name="option" /> and the
        ///     <see cref="IOption{T}" /> returned by <paramref name="intermediateSelector" /> and returning
        ///     <typeparamref name="TResult" />.
        /// </param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option" />.</typeparam>
        /// <typeparam name="TIntermediate">The type potentially returned by <paramref name="intermediateSelector" />.</typeparam>
        /// <typeparam name="TResult">The type returned by <paramref name="resultSelector" />.</typeparam>
        /// <returns>An <see cref="IOption{T}" /> of <typeparamref name="TResult" /></returns>
        /// <exception cref="ArgumentNullException"><paramref name="intermediateSelector" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector" /> is <see langword="null" />.</exception>
        public static IDeferredOption<TResult> SelectMany<T, TIntermediate, TResult>(
            this IDeferredOption<T> option,
            Func<T, IDeferredOption<TIntermediate>> intermediateSelector,
            Func<T, TIntermediate, TResult> resultSelector)
        {
            if (ReferenceEquals(intermediateSelector, null))
            {
                throw new ArgumentNullException("intermediateSelector");
            }
            if (ReferenceEquals(resultSelector, null))
            {
                throw new ArgumentNullException("resultSelector");
            }
            return option.Safe().Project(o => o.SelectMany(t => intermediateSelector(t).Evaluate(), resultSelector));
        }

        /// <summary>
        ///     Projects the value potentially contained by an <see cref="IOption{T}" /> to a new form.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}" /> to transform.</param>
        /// <param name="selector">
        ///     A function accepting the potential value of <paramref name="option" /> and returning an
        ///     <see cref="IOption{T}" />.
        /// </param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option" />.</typeparam>
        /// <typeparam name="TResult">The type potentially returned by <paramref name="selector" />.</typeparam>
        /// <returns>An <see cref="IOption{T}" /> of <typeparamref name="TResult" /></returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector" /> is <see langword="null" />.</exception>
        public static IDeferredOption<TResult> SelectMany<T, TResult>(
            this IDeferredOption<T> option,
            Func<T, IOption<TResult>> selector)
        {
            if (ReferenceEquals(selector, null))
            {
                throw new ArgumentNullException("selector");
            }
            return option.Safe().Project(o => o.SelectMany(selector));
        }

        /// <summary>
        ///     Projects the value potentially contained by an <see cref="IOption{T}" /> to a new form.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}" /> to transform.</param>
        /// <param name="selector">
        ///     A function accepting the potential value of <paramref name="option" /> and returning an
        ///     <see cref="IOption{T}" />.
        /// </param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option" />.</typeparam>
        /// <typeparam name="TResult">The type potentially returned by <paramref name="selector" />.</typeparam>
        /// <returns>An <see cref="IOption{T}" /> of <typeparamref name="TResult" /></returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector" /> is <see langword="null" />.</exception>
        public static IDeferredOption<TResult> SelectMany<T, TResult>(
            this IDeferredOption<T> option,
            Func<T, IDeferredOption<TResult>> selector)
        {
            if (ReferenceEquals(selector, null))
            {
                throw new ArgumentNullException("selector");
            }
            return option.Safe().Project(o => o.SelectMany(t => selector(t).Evaluate()));
        }

        #endregion
    }
}