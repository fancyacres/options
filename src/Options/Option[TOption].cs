using System;
using System.Linq;

namespace Options
{
	/// <summary>
	/// A handy alternative to null.
	/// </summary>
	/// <typeparam name="TOption">The type of the value contained by an <see cref="Option{TOption}"/></typeparam>
	public struct Option<TOption> : IEquatable<Option<TOption>>
	{
		private readonly bool _isSome;
		private readonly TOption _value;

		/// <summary>
		/// Initializes an <see cref="Option{TOption}"/> instance with a value.
		/// </summary>
		/// <param name="value">The internal value of the <see cref="Option{TOption}"/></param>
		/// <remarks>
		/// If <paramref name="value"/> is null, the <see cref="Option{TOption}"/> will not contain a value.
		/// </remarks>
		public Option(TOption value)
		{
			_isSome = !ReferenceEquals(null, value);
			_value = value;
		}

		/// <summary>
		/// Executes the functions, varying whether a value is contained in the <see cref="Option{TOption}"/>.
		/// </summary>
		/// <typeparam name="TResult">The type returned by <paramref name="ifSome"/> and <paramref name="ifNone"/></typeparam>
		/// <param name="ifSome">The function to execute if a value is present</param>
		/// <param name="ifNone">The function to execute if no value is present</param>
		/// <returns>The value returned by <paramref name="ifSome"/> or <paramref name="ifNone"/></returns>
		/// <exception cref="ArgumentNullException"><paramref name="ifSome"/> is null</exception>
		/// <exception cref="ArgumentNullException"><paramref name="ifNone"/> is null</exception>
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
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <returns>
		/// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		/// <param name="obj">Another object to compare to. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (obj.GetType() != typeof(Option<TOption>))
			{
				return false;
			}
			return Equals((Option<TOption>)obj);
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
			unchecked
			{
				return (_isSome.GetHashCode() * 397) ^ (_isSome ? _value.GetHashCode() : 0);
			}
		}

		/// <summary>
		/// Operator to test <see cref="Option{TOption}"/> equality
		/// </summary>
		/// <param name="left">The <see cref="Option{TOption}"/> on the left side of the operator</param>
		/// <param name="right">The <see cref="Option{TOption}"/> on the right side of the operator</param>
		/// <returns>True, if the instances are equal; false, if not.</returns>
		public static bool operator ==(Option<TOption> left, Option<TOption> right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Operator to test <see cref="Option{TOption}"/> equality
		/// </summary>
		/// <param name="left">The <see cref="Option{TOption}"/> on the left side of the operator</param>
		/// <param name="right">The <see cref="Option{TOption}"/> on the right side of the operator</param>
		/// <returns>False, if the instances are equal; true, if not.</returns>
		public static bool operator !=(Option<TOption> left, Option<TOption> right)
		{
			return !left.Equals(right);
		}

		#region IEquatable<Option<TOption>> Members

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An <see cref="Option{TOption}"/> to compare with this object.</param>
		public bool Equals(Option<TOption> other)
		{
			return !(_isSome || other._isSome)
				|| other._isSome.Equals(_isSome) && Equals(other._value, _value);
		}

		#endregion
	}
}
