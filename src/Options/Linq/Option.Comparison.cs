using System;
using System.Collections.Generic;
using System.Diagnostics;
using Options.Core;

namespace Options.Linq
{
    public static partial class Option
    {
        /// <summary>
        /// Create an <see cref="IEquatableOption{T}"/> from an <see cref="IOption{T}"/>.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}"/> that should be equatable.</param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option"/>.</typeparam>
        /// <returns>An <see cref="IEquatableOption{T}"/></returns>
        /// <remarks>
        /// <para>
        /// The returned <see cref="IEquatableOption{T}"/> will return <see langword="true"/> from
        /// <see cref="IEquatable{T}.Equals(T)"/> when both <see cref="IOption{T}"/>s contain
        /// an equal value or both contain no value.
        /// </para>
        /// </remarks>
        public static IEquatableOption<T> ToEquatable<T>(this IOption<T> option)
            where T : IEquatable<T>
        {
            return option.ToEquatable(new EquatableEqualityComparer<T>());
        }

        /// <summary>
        /// Create an <see cref="IEquatableOption{T}"/> from an <see cref="IOption{T}"/>.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}"/> that should be equatable.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> used for comparison.</param>
        /// <typeparam name="T">The type contained by <paramref name="option"/>.</typeparam>
        /// <returns>An <see cref="IEquatableOption{T}"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// <para>
        /// The returned <see cref="IEquatableOption{T}"/> will return <see langword="true"/> from
        /// <see cref="IEquatable{T}.Equals(T)"/> when both <see cref="IOption{T}"/>s contain
        /// an equal value or both contain no value.
        /// </para>
        /// </remarks>
        public static IEquatableOption<T> ToEquatable<T>(this IOption<T> option, IEqualityComparer<T> comparer)
        {
            if (ReferenceEquals(comparer, null))
            {
                throw new ArgumentNullException("comparer");
            }
            return new EquatableOption<T>(option.Safe(), comparer);
        }

        /// <summary>
        /// Create an <see cref="IComparableOption{T}"/> from an <see cref="IOption{T}"/>.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}"/> that should be comparable.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for comparison.</param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option"/>.</typeparam>
        /// <returns>An <see cref="IComparableOption{T}"/></returns>
        /// <remarks>
        /// <para>
        /// The returned <see cref="IComparableOption{T}"/> will return:
        /// </para>
        /// <para>The integer returned by comparing values, when both <see cref="IOption{T}"/>s contain values.</para>
        /// <para>-1, when containing no value and the other <see cref="IOption{T}"/> does.</para>
        /// <para>-1, when containing no value and the other <see cref="IOption{T}"/> does.</para>
        /// <para>0, when both <see cref="IOption{T}"/>s contain no value.</para>
        /// </remarks>
        public static IComparableOption<T> ToComparable<T>(this IOption<T> option, IComparer<T> comparer)
        {
            return new ComparableOption<T>(option.Safe(), comparer);
        }

        /// <summary>
        /// Create an <see cref="IComparableOption{T}"/> from an <see cref="IOption{T}"/>.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}"/> that should be comparable.</param>
        /// <typeparam name="T">The type potentially contained by <paramref name="option"/>.</typeparam>
        /// <returns>An <see cref="IComparableOption{T}"/></returns>
        /// <remarks>
        /// <para>
        /// The returned <see cref="IComparableOption{T}"/> will return:
        /// </para>
        /// <para>The integer returned by comparing values, when both <see cref="IOption{T}"/>s contain values.</para>
        /// <para>-1, when containing no value and the other <see cref="IOption{T}"/> does.</para>
        /// <para>-1, when containing no value and the other <see cref="IOption{T}"/> does.</para>
        /// <para>0, when both <see cref="IOption{T}"/>s contain no value.</para>
        /// </remarks>
        public static IComparableOption<T> ToComparable<T>(this IOption<T> option)
            where T : IComparable<T>
        {
            return option.ToComparable(new ComparableEqualityComparer<T>());
        }
 
        private class EquatableOption<T> : IEquatableOption<T>
        {
            private readonly IOption<T> _inner;
            private readonly IEqualityComparer<T> _comparer;

            internal EquatableOption(IOption<T> inner, IEqualityComparer<T> comparer)
            {
                Debug.Assert(inner != null, "Inner is null");
                Debug.Assert(comparer != null, "Comparer is null");

                _inner = inner;
                _comparer = comparer;
            }

            public TResult Handle<TResult>(Func<T, TResult> ifValue, Func<TResult> ifNone)
            {
                return _inner.Handle(ifValue, ifNone);
            }

            public bool Equals(IOption<T> other)
            {
                return Handle(
                    t => other.Safe().Handle(otherT => _comparer.Equals(t, otherT), () => false),
                    () => other.Safe().Handle(_ => false, () => true));
            }

            public override bool Equals(object obj)
            {
                return obj is IOption<T> && Equals((IOption<T>)obj);
            }

            public override int GetHashCode()
            {
                return _inner.Select(t => _comparer.GetHashCode(t)).GetValueOrDefault();
            }
        }

        private class EquatableEqualityComparer<T> : IEqualityComparer<T>
            where T:IEquatable<T>
        {
            public bool Equals(T x, T y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }

        private class ComparableOption<T> : IComparableOption<T>
        {
            private readonly IOption<T> _inner;
            private readonly IComparer<T> _comparer;

            internal ComparableOption(IOption<T> inner, IComparer<T> comparer)
            {
                Debug.Assert(inner != null, "Inner is null");
                Debug.Assert(comparer != null, "Comparer is null");

                _inner = inner;
                _comparer = comparer;
            }

            public TResult Handle<TResult>(Func<T, TResult> ifValue, Func<TResult> ifNone)
            {
                return _inner.Safe().Handle(ifValue, ifNone);
            }

            public int CompareTo(IOption<T> other)
            {
                return Handle(t => other.Handle(otherT => _comparer.Compare(t, otherT), () => 1),
                    () => other.Handle(_ => -1, () => 0));
            }
        }

        private class ComparableEqualityComparer<T> : IComparer<T>
            where T : IComparable<T>
        {
            public int Compare(T x, T y)
            {
                return x.CompareTo(y);
            }
        } 
    }
}