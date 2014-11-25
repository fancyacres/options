using System;
using Options.Core;

namespace Options.Linq
{
    /// <summary>
    ///     Functions to support LINQ bindings and simplify working with <see cref="IOption{T}" />
    /// </summary>
    public static partial class Option
    {
        private static T Identity<T>(T t)
        {
            return t;
        }

        /// <summary>
        ///     Create an <see cref="IOption{T}" /> with no value.
        /// </summary>
        /// <typeparam name="T">The type of value the <see cref="IOption{T}" /> could have had.</typeparam>
        /// <returns>An empty <see cref="IOption{T}" /></returns>
        public static IOption<T> Create<T>()
        {
            return new OptionImpl<T>();
        }

        /// <summary>
        ///     Create an <see cref="IOption{T}" /> from a value.
        /// </summary>
        /// <param name="value">The value contained by the <see cref="IOption{T}" /></param>
        /// <typeparam name="T">The type of value the <see cref="IOption{T}" /> contains.</typeparam>
        /// <returns>An <see cref="IOption{T}" /></returns>
        /// <remarks>
        ///     If <paramref name="value" /> is <see langword="null" />, the <see cref="IOption{T}" /> will not contain a
        ///     value.
        /// </remarks>
        public static IOption<T> Create<T>(T value)
        {
            return new OptionImpl<T>(value);
        }

        /// <summary>
        ///     Create an <see cref="IOption{T}" /> from a <see cref="Nullable{T}" />.
        /// </summary>
        /// <param name="value">The value contained by the <see cref="IOption{T}" /></param>
        /// <typeparam name="T">The value type <paramref name="value" /> might contain.</typeparam>
        /// <returns>An <see cref="IOption{T}" /></returns>
        /// <remarks>
        ///     If <paramref name="value" /> is <see langword="null" />, the <see cref="IOption{T}" /> will not contain a
        ///     value.
        /// </remarks>
        public static IOption<T> Create<T>(T? value)
            where T : struct
        {
            return value.HasValue ? Create(value.Value) : Create<T>();
        }

        /// <summary>
        ///     Get an <see cref="IOption{T}" /> that will never be <see langword="null" />.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}" /> that could be <see langword="null" />.</param>
        /// <typeparam name="T">The type of value the <see cref="IOption{T}" /> contains.</typeparam>
        /// <returns>An <see cref="IOption{T}" /> that is never <see langword="null" /></returns>
        public static IOption<T> Safe<T>(this IOption<T> option)
        {
            return option ?? Create<T>();
        }

        /// <summary>
        ///     Gets the value contained by <paramref name="option" /> or the result of executing
        ///     <paramref name="defaultFactory" />.
        /// </summary>
        /// <param name="option">An <see cref="IOption{T}" /></param>
        /// <param name="defaultFactory">A factory function to execute when <paramref name="option" /> has no value.</param>
        /// <typeparam name="T">
        ///     The type contained by <paramref name="option" /> and returned by <paramref name="defaultFactory" />
        ///     .
        /// </typeparam>
        /// <returns>
        ///     The value contained by <paramref name="option" /> or the result of executing
        ///     <paramref name="defaultFactory" />.
        /// </returns>
        public static T GetValueOrDefault<T>(this IOption<T> option, Func<T> defaultFactory)
        {
            if (ReferenceEquals(defaultFactory, null))
            {
                throw new ArgumentNullException("defaultFactory");
            }
            return option.Safe().Handle(Identity, defaultFactory);
        }

        /// <summary>
        ///     Gets the value contained by <paramref name="option" /> or the default value of <typeparamref name="T" />.
        /// </summary>
        /// <param name="option">An <see cref="IOption{T}" /></param>
        /// <typeparam name="T">The type contained by <paramref name="option" />.</typeparam>
        /// <returns>The value contained by <paramref name="option" /> or the default value of <typeparamref name="T" />.</returns>
        public static T GetValueOrDefault<T>(this IOption<T> option)
        {
            return option.GetValueOrDefault(() => default(T));
        }

        /// <summary>
        ///     Perform one of two <see cref="Action" />s, varying whether <paramref name="option" /> contains a value.
        /// </summary>
        /// <param name="option">An <see cref="IOption{T}" /> used to act.</param>
        /// <param name="valueAction">The action executed when <paramref name="option" /> contains a value.</param>
        /// <param name="nothingAction">The action executed when <paramref name="option" /> contains a value.</param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option" />.</typeparam>
        /// <returns>
        ///     <see cref="Unit" />
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueAction" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="nothingAction" /> is <see langword="null" />.</exception>
        public static Unit Act<T>(
            this IOption<T> option,
            Func<T, Unit> valueAction,
            Func<Unit> nothingAction)
        {
            if (ReferenceEquals(valueAction, null))
            {
                throw new ArgumentNullException("valueAction");
            }
            if (ReferenceEquals(nothingAction, null))
            {
                throw new ArgumentNullException("nothingAction");
            }
            return option.Safe().Handle(valueAction, nothingAction);
        }

        /// <summary>
        ///     Perform an action, if an <see cref="IOption{T}" /> contains a value.
        /// </summary>
        /// <param name="option">An <see cref="IOption{T}" /> used to act.</param>
        /// <param name="valueAction">The action executed when <paramref name="option" /> contains a value.</param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option" />.</typeparam>
        /// <return>
        ///     <see cref="Unit" />
        /// </return>
        /// <exception cref="ArgumentNullException"><paramref name="valueAction" /> is <see langword="null" />.</exception>
// ReSharper disable once UnusedMethodReturnValue.Global
        public static Unit Act<T>(
            this IOption<T> option,
            Func<T, Unit> valueAction)
        {
            if (ReferenceEquals(valueAction, null))
            {
                throw new ArgumentNullException("valueAction");
            }
            return option.Act(valueAction, () => Unit.Default);
        }

        /// <summary>
        ///     Perform an <see cref="Action" />s, if <paramref name="option" /> contains no value.
        /// </summary>
        /// <param name="option">An <see cref="IOption{T}" /> used to act.</param>
        /// <param name="nothingAction">The action executed when <paramref name="option" /> contains a value.</param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option" />.</typeparam>
        /// <returns>Unit</returns>
        /// <exception cref="ArgumentNullException"><paramref name="nothingAction" /> is <see langword="null" />.</exception>
// ReSharper disable once UnusedMethodReturnValue.Global
        public static Unit Act<T>(
            this IOption<T> option,
            Func<Unit> nothingAction)
        {
            if (ReferenceEquals(nothingAction, null))
            {
                throw new ArgumentNullException("nothingAction");
            }
            return option.Act(_ => Unit.Default, nothingAction);
        }

        /// <summary>
        ///     Perform one of two <see cref="Action" />s, varying whether <paramref name="option" /> contains a value.
        /// </summary>
        /// <param name="option">An <see cref="IOption{T}" /> used to act.</param>
        /// <param name="valueAction">The action executed when <paramref name="option" /> contains a value.</param>
        /// <param name="nothingAction">The action executed when <paramref name="option" /> contains a value.</param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option" />.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="valueAction" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="nothingAction" /> is <see langword="null" />.</exception>
        public static void Act<T>(
            this IOption<T> option,
            Action<T> valueAction,
            Action nothingAction)
        {
            if (ReferenceEquals(valueAction, null))
            {
                throw new ArgumentNullException("valueAction");
            }
            if (ReferenceEquals(nothingAction, null))
            {
                throw new ArgumentNullException("nothingAction");
            }
            option.Safe().Act(valueAction.MakeFunc(), nothingAction.MakeFunc());
        }

        /// <summary>
        ///     Perform an action, if an <see cref="IOption{T}" /> contains a value.
        /// </summary>
        /// <param name="option">An <see cref="IOption{T}" /> used to act.</param>
        /// <param name="valueAction">The action executed when <paramref name="option" /> contains a value.</param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option" />.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="valueAction" /> is <see langword="null" />.</exception>
        public static void Act<T>(
            this IOption<T> option,
            Action<T> valueAction)
        {
            if (ReferenceEquals(valueAction, null))
            {
                throw new ArgumentNullException("valueAction");
            }
            option.Safe().Act(valueAction.MakeFunc());
        }

        /// <summary>
        ///     Perform an <see cref="Action" />s, if <paramref name="option" /> contains no value.
        /// </summary>
        /// <param name="option">An <see cref="IOption{T}" /> used to act.</param>
        /// <param name="nothingAction">The action executed when <paramref name="option" /> contains a value.</param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option" />.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="nothingAction" /> is <see langword="null" />.</exception>
        public static void Act<T>(
            this IOption<T> option,
            Action nothingAction)
        {
            if (ReferenceEquals(nothingAction, null))
            {
                throw new ArgumentNullException("nothingAction");
            }
            option.Safe().Act(nothingAction.MakeFunc());
        }

        /// <summary>
        ///     Create a concrete <see cref="IOption{T}" /> implementation from <paramref name="option" />.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}" /> to convert.</param>
        /// <typeparam name="T">The type contained by <paramref name="option" />.</typeparam>
        /// <returns>A <see cref="ValueOption{T}" /></returns>
        /// <remarks>
        ///     <para>
        ///         This function can be used to get an <see cref="IOption{T}" /> that is never <see langword="null" />.
        ///     </para>
        /// </remarks>
        public static ValueOption<T> ToConcrete<T>(this IOption<T> option)
        {
            return new ValueOption<T>(option);
        }
    }
}