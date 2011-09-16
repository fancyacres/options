using System;

namespace Options
{
	/// <summary>
	/// A type representing a choice between two options.
	/// </summary>
	/// <typeparam name="TFirst">The first of the two types.</typeparam>
	/// <typeparam name="TSecond">The second of the two types.</typeparam>
	public class Either<TFirst, TSecond> : IEquatable<Either<TFirst, TSecond>>
	{
		private readonly bool _isFirst;
		private readonly TFirst _first;
		private readonly TSecond _second;

		/// <summary>
		/// 	Initializes an <see cref = "Either{TFirst,TSecond}" /> instance with a value of type <typeparamref name="TFirst"/>.
		/// </summary>
		/// <param name = "first">The internal value of the <see cref = "Either{TFirst,TSecond}" /></param>
		/// <exception cref = "ArgumentNullException"><paramref name = "first" /> is null</exception>
		public Either(TFirst first)
		{
			if (ReferenceEquals(first, null))
			{
				throw new ArgumentNullException("first");
			}
			_isFirst = true;
			_first = first;
			_second = default(TSecond);
		}

		/// <summary>
		/// 	Initializes an <see cref = "Either{TFirst,TSecond}" /> instance with a value of type <typeparamref name="TSecond"/>.
		/// </summary>
		/// <param name = "second">The internal value of the <see cref = "Either{TFirst,TSecond}" /></param>
		/// <exception cref = "ArgumentNullException"><paramref name = "second" /> is null</exception>
		public Either(TSecond second)
		{
			if (ReferenceEquals(second, null))
			{
				throw new ArgumentNullException("second");
			}
			_isFirst = false;
			_first = default(TFirst);
			_second = second;
		}

		/// <summary>
		/// 	Executes the functions, varying whether on which value is contained in the <see cref = "Either{TFirst,TSecond}" />.
		/// </summary>
		/// <typeparam name = "TResult">The type returned by <paramref name = "ifFirst" /> and <paramref name = "ifSecond" /></typeparam>
		/// <param name = "ifFirst">The function to execute if the first value is present</param>
		/// <param name = "ifSecond">The function to execute if the second value is present</param>
		/// <returns>The value returned by <paramref name = "ifFirst" /> or <paramref name = "ifSecond" /></returns>
		/// <exception cref = "ArgumentNullException"><paramref name = "ifFirst" /> is null</exception>
		/// <exception cref = "ArgumentNullException"><paramref name = "ifSecond" /> is null</exception>
		public TResult Handle<TResult>(Func<TFirst, TResult> ifFirst, Func<TSecond, TResult> ifSecond)
		{
			if (ifFirst == null)
			{
				throw new ArgumentNullException("ifFirst");
			}
			if (ifSecond == null)
			{
				throw new ArgumentNullException("ifSecond");
			}

			return _isFirst
			       	? ifFirst(_first)
			       	: ifSecond(_second);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Either<TFirst, TSecond> other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other._isFirst.Equals(_isFirst) && Equals(other._first, _first) && Equals(other._second, _second);
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(Either<TFirst, TSecond>)) return false;
			return Equals((Either<TFirst, TSecond>) obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			unchecked
			{
				int result = _isFirst.GetHashCode();
				if (_isFirst)
				{
					result = (result * 397) ^ _first.GetHashCode();
				}
				else
				{
					result = (result*397) ^ _second.GetHashCode();
				}
				return result;
			}
		}

		/// <summary>
		/// 	Operator to test <see cref = "Either{TLeft,TRight}" /> equality
		/// </summary>
		/// <param name = "left">The <see cref = "Either{TLeft,TRight}" /> on the left side of the operator</param>
		/// <param name = "right">The <see cref = "Either{TLeft,TRight}" /> on the right side of the operator</param>
		/// <returns>True, if the instances are equal; false, if not.</returns>
		public static bool operator ==(Either<TFirst, TSecond> left, Either<TFirst, TSecond> right)
		{
			return Equals(left, right);
		}

		/// <summary>
		/// 	Operator to test <see cref = "Either{TLeft,TRight}" /> equality
		/// </summary>
		/// <param name = "left">The <see cref = "Either{TLeft,TRight}" /> on the left side of the operator</param>
		/// <param name = "right">The <see cref = "Either{TLeft,TRight}" /> on the right side of the operator</param>
		/// <returns>True, if the instances are not equal; false, if not.</returns>
		public static bool operator !=(Either<TFirst, TSecond> left, Either<TFirst, TSecond> right)
		{
			return !Equals(left, right);
		}
	}
}
