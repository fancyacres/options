using System;

namespace Options
{
#pragma warning disable 618
    /// <summary>
    ///     A handy alternative to null.
    /// </summary>
    /// <typeparam name="TOption">The type of the value contained by an <see cref="Option{TOption}" /></typeparam>
    public struct Option<TOption> : IEquatable<Option<TOption>>, IEquatable<TOption>
    {
        private readonly bool _isSome;
        private readonly TOption _value;

        /// <summary>
        ///     Initializes an <see cref="Option{TOption}" /> instance with a value.
        /// </summary>
        /// <param name="value">The internal value of the <see cref="Option{TOption}" /></param>
        /// <remarks>
        ///     If <paramref name="value" /> is null, the <see cref="Option{TOption}" /> will not contain a value.
        /// </remarks>
        public Option(TOption value)
        {
            _isSome = !ReferenceEquals(null, value);
            _value = value;
        }

        /// <summary>
        /// Execute one of two functions, varying whether the <see cref="Option{T}"/> contains a value.
        /// </summary>
        /// <param name="ifSome">The <see cref="Func{T1,TResult}"/> to execute when a value is present.</param>
        /// <param name="ifNone">The <see cref="Func{TResult}"/> to execute when a value is not present.</param>
        /// <typeparam name="TResult">The result of executing <paramref name="ifSome"/> or <paramref name="ifNone"/>.</typeparam>
        /// <returns>A <typeparamref name="TResult"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="ifSome"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ifNone"/> is <see langword="null"/>.</exception>
        public TResult Handle<TResult>(Func<TOption, TResult> ifSome, Func<TResult> ifNone)
        {
            if (ifSome == null)
            {
                throw new ArgumentNullException("ifSome");
            }
            if (ifNone == null)
            {
                throw new ArgumentNullException("ifNone");
            }
            return _isSome ? ifSome(_value) : ifNone();
        }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the contained type.
        /// </summary>
        /// <returns>
        ///     If there is no value in the option and <paramref name="other" /> is null, true;
        ///     if there is no value in the option and <paramref name="other" /> is not null, false;
        ///     if the contained object is equal to the <paramref name="other" /> parameter according to Object.Equals, true;
        ///     otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(TOption other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (!_isSome)
            {
                return false;
            }
            return _value.Equals(other);
        }

        /// <summary>
        ///     Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        ///     true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (obj.GetType() == typeof (TOption))
            {
                return Equals((TOption) obj);
            }
            if (obj.GetType() != typeof (Option<TOption>))
            {
                return false;
            }
            return Equals((Option<TOption>) obj);
        }

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_isSome.GetHashCode()*397) ^ (_isSome ? _value.GetHashCode() : 0);
            }
        }

        /// <summary>
        ///     Operator to test <see cref="Option{TOption}" /> equality
        /// </summary>
        /// <param name="left">The <see cref="Option{TOption}" /> on the left side of the operator</param>
        /// <param name="right">The <see cref="Option{TOption}" /> on the right side of the operator</param>
        /// <returns>True, if the instances are equal; false, if not.</returns>
        public static bool operator ==(Option<TOption> left, Option<TOption> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Operator to test <see cref="Option{TOption}" /> equality
        /// </summary>
        /// <param name="left">The <see cref="Option{TOption}" /> on the left side of the operator</param>
        /// <param name="right">The <see cref="Option{TOption}" /> on the right side of the operator</param>
        /// <returns>False, if the instances are equal; true, if not.</returns>
        public static bool operator !=(Option<TOption> left, Option<TOption> right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Operator to test <see cref="Option{TOption}" /> equality
        /// </summary>
        /// <param name="left">The <see cref="Option{TOption}" /> on the left side of the operator</param>
        /// <param name="right">The <see cref="Option{TOption}" /> on the right side of the operator</param>
        /// <returns>True, if the instances are equal; false, if not.</returns>
        public static bool operator ==(Option<TOption> left, TOption right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Operator to test <see cref="Option{TOption}" /> equality
        /// </summary>
        /// <param name="left">The <see cref="Option{TOption}" /> on the left side of the operator</param>
        /// <param name="right">The <see cref="Option{TOption}" /> on the right side of the operator</param>
        /// <returns>False, if the instances are equal; true, if not.</returns>
        public static bool operator !=(Option<TOption> left, TOption right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Operator to test <see cref="Option{TOption}" /> equality
        /// </summary>
        /// <param name="left">The <see cref="Option{TOption}" /> on the left side of the operator</param>
        /// <param name="right">The <see cref="Option{TOption}" /> on the right side of the operator</param>
        /// <returns>True, if the instances are equal; false, if not.</returns>
        public static bool operator ==(TOption left, Option<TOption> right)
        {
            return right.Equals(left);
        }

        /// <summary>
        ///     Operator to test <see cref="Option{TOption}" /> equality
        /// </summary>
        /// <param name="left">The <see cref="Option{TOption}" /> on the left side of the operator</param>
        /// <param name="right">The <see cref="Option{TOption}" /> on the right side of the operator</param>
        /// <returns>False, if the instances are equal; true, if not.</returns>
        public static bool operator !=(TOption left, Option<TOption> right)
        {
            return !right.Equals(left);
        }

        #region IEquatable<Option<TOption>> Members

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An <see cref="Option{TOption}" /> to compare with this object.</param>
        public bool Equals(Option<TOption> other)
        {
            return !(_isSome || other._isSome)
                   || other._isSome.Equals(_isSome) && Equals(other._value, _value);
        }

        #endregion

        /// <summary>
        ///     Operator to provide implicit lifting into <see cref="Option{TOption}" /> values
        /// </summary>
        /// <param name="value">The value that will be returned in <see cref="Option{TOption}" /> form</param>
        /// <returns>
        ///     An <see cref="Option{TOption}" /> containing <paramref name="value" />.
        ///     If <paramref name="value" /> is <c>null</c>, the result will be none.
        /// </returns>
        public static implicit operator Option<TOption>(TOption value)
        {
            return new Option<TOption>(value);
        }


        /// <summary>
        ///     Operator to support <see cref="Option" />.None
        /// </summary>
        /// <param name="none">A placeholder value, typically returned by <see cref="Option.None" /></param>
        /// <returns>
        ///     A none valued <see cref="Option{TOption}" />
        /// </returns>
        public static implicit operator Option<TOption>(OptionNone none)
        {
            return new Option<TOption>();
        }
    }
#pragma warning restore 618
}