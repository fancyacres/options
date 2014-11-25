using System;
using Options.Linq;

namespace Options.Core
{
    /// <summary>
    /// A concrete implementation of <see cref="IOption{T}"/>
    /// </summary>
    /// <typeparam name="T">The type contained by instances</typeparam>
    public struct ValueOption<T> : IOption<T>, IEquatable<ValueOption<T>>, IEquatable<IOption<T>>
    {
        private readonly bool _hasValue;
        private readonly T _value;

        /// <summary>
        /// Create a new instance of <see cref="ValueOption{T}"/>.
        /// </summary>
        /// <param name="option">The <see cref="IOption{T}"/> used to drive this instance's behavior.</param>
        public ValueOption(IOption<T> option)
        {
            var safe = option.Safe();
            _hasValue = safe.Handle(_ => true, () => false);
            _value = safe.Handle(t => t, () => default(T));
        }

        /// <summary>
        /// Execute one of two functions, varying whether the <see cref="IOption{T}"/> contains a value.
        /// </summary>
        /// <param name="ifValue">The <see cref="Func{T1,TResult}"/> to execute when a value is present.</param>
        /// <param name="ifNone">The <see cref="Func{TResult}"/> to execute when a value is not present.</param>
        /// <typeparam name="TResult">The result of executing <paramref name="ifValue"/> or <paramref name="ifNone"/>.</typeparam>
        /// <returns>A <typeparamref name="TResult"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="ifValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ifNone"/> is <see langword="null"/>.</exception>
        public TResult Handle<TResult>(Func<T, TResult> ifValue, Func<TResult> ifNone)
        {
            return _hasValue ? ifValue(_value) : ifNone();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <see langword="false"/>.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ValueOption<T> other)
        {
            return (_hasValue && other._hasValue && Equals(_value, other._value))
                || (!_hasValue && !other._hasValue);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another <see cref="IOption{T}"/>.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to <paramref name="other"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <param name="other">An <see cref="IOption{T}"/> to compare with this object.</param>
        public bool Equals(IOption<T> other)
        {
            return Equals(other.ToConcrete());
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, <see langword="false"/>.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ValueOption<T> && Equals((ValueOption<T>) obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return _hasValue ? _value.GetHashCode() : 0;
        }

        /// <summary>
        /// Compare <paramref name="left"/> and <paramref name="right"/> for equality.
        /// </summary>
        /// <param name="left">A <see cref="ValueOption{T}"/>.</param>
        /// <param name="right">A <see cref="ValueOption{T}"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; <see langword="false"/> if not.</returns>
        public static bool operator ==(ValueOption<T> left, ValueOption<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compare <paramref name="left"/> and <paramref name="right"/> for inequality.
        /// </summary>
        /// <param name="left">A <see cref="ValueOption{T}"/>.</param>
        /// <param name="right">A <see cref="ValueOption{T}"/>.</param>
        /// <returns><see langword="false"/> if <paramref name="left"/> and <paramref name="right"/> are equal; <see langword="true"/> if not.</returns>
        public static bool operator !=(ValueOption<T> left, ValueOption<T> right)
        {
            return !left.Equals(right);
        }
    }
}