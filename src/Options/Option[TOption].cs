using System;
using System.Linq;

namespace Options
{
	public struct Option<TOption> : IEquatable<Option<TOption>>
	{
		private readonly bool _isSome;
		private readonly TOption _value;

		public Option(TOption value)
		{
			_isSome = !ReferenceEquals(null, value);
			_value = value;
		}

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

		public override int GetHashCode()
		{
			unchecked
			{
				return (_isSome.GetHashCode() * 397) ^ (_isSome ? _value.GetHashCode() : 0);
			}
		}

		public static bool operator ==(Option<TOption> left, Option<TOption> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Option<TOption> left, Option<TOption> right)
		{
			return !left.Equals(right);
		}

		#region IEquatable<Option<TOption>> Members

		public bool Equals(Option<TOption> other)
		{
			return !(_isSome || other._isSome)
				|| other._isSome.Equals(_isSome) && Equals(other._value, _value);
		}

		#endregion
	}
}
