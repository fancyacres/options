using System;

namespace Options
{
	/// <summary>
	/// A set of factory methods for creating <see cref="Either{TFirst,TSecond}"/> instances.
	/// </summary>
	public static class Either
	{
		/// <summary>
		/// Returns an <see cref="Either{TFirst,TSecond}"/> with the result of <paramref name="first"/> or <paramref name="second"/> based on the value of <paramref name="isFirst"/>.
		/// </summary>
		/// <param name="isFirst">If true, construct the <see cref="Either{TFirst,TSecond}"/> instance with <paramref name="first"/>, otherwise <paramref name="second"/>.</param>
		/// <param name="first">A function returning the value to use if <paramref name="isFirst"/> is true. Not called if <paramref name="isFirst"/> is false.</param>
		/// <param name="second">A function returning the value to use if <paramref name="isFirst"/> is false. Not called if <paramref name="isFirst"/> is true.</param>
		/// <typeparam name="TFirst">The return type of <paramref name="first"/>, and the first type of the returned <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <typeparam name="TSecond">The return type of <paramref name="second"/>, and the second type of the returned <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <returns>
		///		When <paramref name="isFirst"/> is true, an <see cref="Either{TFirst,TSecond}"/> with the result of <paramref name="first"/> as its value,
		///		otherwise an <see cref="Either{TFirst,TSecond}"/> with the result of <paramref name="second"/> as its value.
		/// </returns>
		public static Either<TFirst, TSecond> Create<TFirst, TSecond>(
			bool isFirst,
			Func<TFirst> first,
			Func<TSecond> second)
		{
			return isFirst
							? new Either<TFirst, TSecond>(first())
							: new Either<TFirst, TSecond>(second());
		}

		/// <summary>
		/// Returns a partially-constructed <see cref="Either{TFirst,TSecond}"/> which will have <typeparamref name="TFirst"/> as its first type.
		/// </summary>
		/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/> to be constructed.</typeparam>
		/// <returns>A partially-constructed <see cref="Either{TFirst,TSecond}"/> which will have <typeparamref name="TFirst"/> as its first type.</returns>
		public static IEitherNeedsSecondValue<TFirst> WithFirst<TFirst>()
		{
			return new EitherNeedsSecondValue<TFirst>();
		}

		/// <summary>
		/// Returns a partially-constructed <see cref="Either{TFirst,TSecond}"/> which will have <typeparamref name="TSecond"/> as its first type.
		/// </summary>
		/// <typeparam name="TSecond">The second type of the <see cref="Either{TFirst,TSecond}"/> to be constructed.</typeparam>
		/// <returns>A partially-constructed <see cref="Either{TFirst,TSecond}"/> which will have <typeparamref name="TSecond"/> as its first type.</returns>
		public static IEitherNeedsFirstValue<TSecond> WithSecond<TSecond>()
		{
			return new EitherNeedsFirstValue<TSecond>();
		}

		/// <summary>
		/// Returns a partially-constructed <see cref="Either{TFirst,TSecond}"/> which will have <paramref name="first"/> as its value and <typeparamref name="TFirst"/> as its first type.
		/// </summary>
		/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/> to be constructed.</typeparam>
		/// <returns>A partially-constructed <see cref="Either{TFirst,TSecond}"/> which will have <paramref name="first"/> as its value and <typeparamref name="TFirst"/> as its first type.</returns>
		public static IEitherNeedsSecondType<TFirst> WithFirst<TFirst>(TFirst first)
		{
			return new EitherNeedsSecondType<TFirst>(first);
		}

		/// <summary>
		/// Returns a partially-constructed <see cref="Either{TFirst,TSecond}"/> which will have <paramref name="second"/> as its value and <typeparamref name="TSecond"/> as its second type.
		/// </summary>
		/// <typeparam name="TSecond">The first type of the <see cref="Either{TFirst,TSecond}"/> to be constructed.</typeparam>
		/// <returns>A partially-constructed <see cref="Either{TFirst,TSecond}"/> which will have <paramref name="second"/> as its value and <typeparamref name="TSecond"/> as its second type.</returns>
		public static IEitherNeedsFirstType<TSecond> WithSecond<TSecond>(TSecond second)
		{
			return new EitherNeedsFirstType<TSecond>(second);
		}
	}
}
