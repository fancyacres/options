using System;

namespace Options.GlobalExtensions
{
	/// <summary>
	/// Utility functions for creating <see cref="Either{TFirst,TSecond}">either instances</see> from any value.
	/// </summary>
	public static class EitherGlobalExtensions
	{
		/// <summary>
		/// Returns a partially-constructed <see cref="Either{TFirst,TSecond}"/> which will have <paramref name="first"/> as its value and <typeparamref name="TFirst"/> as its first type.
		/// </summary>
		/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/> to be constructed.</typeparam>
		/// <returns>A partially-constructed <see cref="Either{TFirst,TSecond}"/> which will have <paramref name="first"/> as its value and <typeparamref name="TFirst"/> as its first type.</returns>
		public static IEitherNeedsSecondType<TFirst> AsFirst<TFirst>(this TFirst first)
		{
			if (ReferenceEquals(first, null))
			{
				throw new ArgumentNullException("first");
			}

			return new EitherNeedsSecondType<TFirst>(first);
		}

		/// <summary>
		/// Returns a partially-constructed <see cref="Either{TFirst,TSecond}"/> which will have <paramref name="second"/> as its value and <typeparamref name="TSecond"/> as its second type.
		/// </summary>
		/// <typeparam name="TSecond">The first type of the <see cref="Either{TFirst,TSecond}"/> to be constructed.</typeparam>
		/// <returns>A partially-constructed <see cref="Either{TFirst,TSecond}"/> which will have <paramref name="second"/> as its value and <typeparamref name="TSecond"/> as its second type.</returns>
		public static IEitherNeedsFirstType<TSecond> AsSecond<TSecond>(this TSecond second)
		{
			if (ReferenceEquals(second, null))
			{
				throw new ArgumentNullException("second");
			}

			return new EitherNeedsFirstType<TSecond>(second);
		}
	}
}
