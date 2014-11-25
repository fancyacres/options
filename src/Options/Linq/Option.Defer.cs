using System;
using System.Diagnostics;
using Options.Core;

namespace Options.Linq
{
    public static partial class Option
    {
        /// <summary>
        /// Create an <see cref="IDeferredOption{T}"/> that reevaluates every time the option is handled.
        /// </summary>
        /// <param name="factory">The function used to generate the <see cref="IOption{T}"/> value.</param>
        /// <typeparam name="T">The type that may be contained.</typeparam>
        /// <returns>An <see cref="IDeferredOption{T}"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is <see langword="null"/></exception>
        public static IDeferredOption<T> Generate<T>(Func<IOption<T>> factory)
        {
            if (ReferenceEquals(factory, null))
            {
                throw new ArgumentNullException("factory");
            }
            return new GenerateOption<T>(factory);
        }

        /// <summary>
        /// Create an <see cref="IDeferredOption{T}"/> that reevaluates every time the option is handled.
        /// </summary>
        /// <param name="factory">The function used to generate the option's value.</param>
        /// <typeparam name="T">The type that may be contained.</typeparam>
        /// <returns>An <see cref="IDeferredOption{T}"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is <see langword="null"/></exception>
        public static IDeferredOption<T> Generate<T>(Func<T> factory)
        {
            if (ReferenceEquals(factory, null))
            {
                throw new ArgumentNullException("factory");
            }
            return new GenerateOption<T>(() => Create(factory()));
        }

        /// <summary>
        /// Create an <see cref="IDeferredOption{T}"/> that always evaluates to an option with <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value that the evaluated option will always contain.</param>
        /// <typeparam name="T">The type that may be contained.</typeparam>
        /// <returns>An <see cref="IDeferredOption{T}"/></returns>
        /// <remarks>The evaluated option will not contain a value, if <paramref name="value"/> is <see langword="null"/></remarks>
        public static IDeferredOption<T> Constant<T>(T value)
        {
            return new GenerateOption<T>(() => Create(value));
        }

        /// <summary>
        /// Create an <see cref="IDeferredOption{T}"/> that always evaluates to an option with <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value that the evaluated option will always contain.</param>
        /// <typeparam name="T">The type that may be contained.</typeparam>
        /// <returns>An <see cref="IDeferredOption{T}"/></returns>
        /// <remarks>
        /// The evaluated option will not contain a value, if <paramref name="value"/>'s
        /// <see cref="Nullable{T}.HasValue"/> property is <see langword="false"/>.
        /// </remarks>
        public static IDeferredOption<T> Constant<T>(T? value)
            where T:struct
        {
            return new GenerateOption<T>(() => Create(value));
        }

        /// <summary>
        /// Create an <see cref="IDeferredOption{T}"/> that always evaluates to an option with no value.
        /// </summary>
        /// <typeparam name="T">The type contained by the evaluated <see cref="IOption{T}"/>.</typeparam>
        /// <returns>An <see cref="IDeferredOption{T}"/></returns>
        public static IDeferredOption<T> Constant<T>()
        {
            return new GenerateOption<T>(Create<T>);
        }

        /// <summary>
        ///     Get an <see cref="IDeferredOption{T}" /> that will never be <see langword="null" />.
        /// </summary>
        /// <param name="option">The <see cref="IDeferredOption{T}" /> that could be <see langword="null" />.</param>
        /// <typeparam name="T">The type of value the <see cref="IDeferredOption{T}" /> contains.</typeparam>
        /// <returns>An <see cref="IDeferredOption{T}" /> that is never <see langword="null" /></returns>
        public static IDeferredOption<T> Safe<T>(this IDeferredOption<T> option)
        {
            return option ?? new GenerateOption<T>();
        }

        /// <summary>
        /// Perform an action every time <paramref name="deferred"/> is evaluated.
        /// </summary>
        /// <param name="deferred">An <see cref="IDeferredOption{T}"/></param>
        /// <param name="doValue">The action to perform, if the evaluation yields a value.</param>
        /// <param name="doNone">The action to perform, if the evaluation yields no value.</param>
        /// <typeparam name="T">The type eventually contained by <paramref name="deferred"/>.</typeparam>
        /// <returns>An <see cref="IDeferredOption{T}"/>.</returns>
        public static IDeferredOption<T> Do<T>(this IDeferredOption<T> deferred, Func<T, Unit> doValue, Func<Unit> doNone)
        {
            return deferred.Safe().Project(
                o =>
                {
                    o.Safe().Handle(doValue, doNone);
                    return o;
                });
        }

        /// <summary>
        /// Perform an action every time <paramref name="deferred"/> is evaluated.
        /// </summary>
        /// <param name="deferred">An <see cref="IDeferredOption{T}"/></param>
        /// <param name="doValue">The action to perform, if the evaluation yields a value.</param>
        /// <typeparam name="T">The type eventually contained by <paramref name="deferred"/>.</typeparam>
        /// <returns>An <see cref="IDeferredOption{T}"/>.</returns>
        public static IDeferredOption<T> Do<T>(this IDeferredOption<T> deferred, Func<T, Unit> doValue)
        {
            return deferred.Do(doValue, () => Unit.Default);
        }

        /// <summary>
        /// Perform an action every time <paramref name="deferred"/> is evaluated.
        /// </summary>
        /// <param name="deferred">An <see cref="IDeferredOption{T}"/></param>
        /// <param name="doNone">The action to perform, if the evaluation yields a value.</param>
        /// <typeparam name="T">The type eventually contained by <paramref name="deferred"/>.</typeparam>
        /// <returns>An <see cref="IDeferredOption{T}"/>.</returns>
        public static IDeferredOption<T> Do<T>(this IDeferredOption<T> deferred, Func<Unit> doNone)
        {
            return deferred.Do(t => Unit.Default, doNone);
        }

        /// <summary>
        /// Perform an action every time <paramref name="deferred"/> is evaluated.
        /// </summary>
        /// <param name="deferred">An <see cref="IDeferredOption{T}"/></param>
        /// <param name="doValue">The action to perform, if the evaluation yields a value.</param>
        /// <param name="doNone">The action to perform, if the evaluation yields no value.</param>
        /// <typeparam name="T">The type eventually contained by <paramref name="deferred"/>.</typeparam>
        /// <returns>An <see cref="IDeferredOption{T}"/>.</returns>
        public static IDeferredOption<T> Do<T>(this IDeferredOption<T> deferred, Action<T> doValue, Action doNone)
        {
            return deferred.Do(doValue.MakeFunc(), doNone.MakeFunc());
        }

        /// <summary>
        /// Perform an action every time <paramref name="deferred"/> is evaluated.
        /// </summary>
        /// <param name="deferred">An <see cref="IDeferredOption{T}"/></param>
        /// <param name="doValue">The action to perform, if the evaluation yields a value.</param>
        /// <typeparam name="T">The type eventually contained by <paramref name="deferred"/>.</typeparam>
        /// <returns>An <see cref="IDeferredOption{T}"/>.</returns>
        public static IDeferredOption<T> Do<T>(this IDeferredOption<T> deferred, Action<T> doValue)
        {
            return deferred.Do(doValue, () => { });
        }

        /// <summary>
        /// Perform an action every time <paramref name="deferred"/> is evaluated.
        /// </summary>
        /// <param name="deferred">An <see cref="IDeferredOption{T}"/></param>
        /// <param name="doNone">The action to perform, if the evaluation yields a value.</param>
        /// <typeparam name="T">The type eventually contained by <paramref name="deferred"/>.</typeparam>
        /// <returns>An <see cref="IDeferredOption{T}"/>.</returns>
        public static IDeferredOption<T> Do<T>(this IDeferredOption<T> deferred, Action doNone)
        {
            return deferred.Do(t => { }, doNone);
        } 
 
        private class GenerateOption<T> : IDeferredOption<T>
        {
            private readonly Func<IOption<T>> _factory;

            internal GenerateOption() : this (Create<T>)
            {
            }

            internal GenerateOption(Func<IOption<T>> factory)
            {
                Debug.Assert(factory != null, "Factory is null.");
                _factory = factory;
            }

            public IDeferredOption<TResult> Project<TResult>(Func<IOption<T>, IOption<TResult>> selector)
            {
                return new GenerateOption<TResult>(() => selector(_factory()));
            }

            public IOption<T> Evaluate()
            {
                return _factory();
            }
        }
    }
}