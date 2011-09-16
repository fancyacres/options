namespace Options
{
	/// <summary>
	/// Represents a partially-constructed either, with a pre-provided second value.
	/// </summary>
	/// <typeparam name="TSecond">The type of the second value.</typeparam>
	public interface IEitherNeedsFirstType<TSecond>
	{
		/// <summary>
		/// Returns an either value with the pre-provided second value, and <typeparamref name="TFirst"/> for the first type.
		/// </summary>
		/// <typeparam name="TFirst">The first type of the either.</typeparam>
		/// <returns>An either value with the pre-provided second value, and <typeparamref name="TFirst"/> for the first type.</returns>
		Either<TFirst, TSecond> WithFirst<TFirst>();
	}

	class EitherNeedsFirstType<TSecond> : IEitherNeedsFirstType<TSecond>
	{
		private readonly TSecond _second;

		public EitherNeedsFirstType(TSecond second)
		{
			_second = second;
		}

		public Either<TFirst, TSecond> WithFirst<TFirst>()
		{
			return new Either<TFirst, TSecond>(_second);
		}
	}

	/// <summary>
	/// Represents a partially-constructed <see cref="Either{TFirst,TSecond}"/>, with a pre-provided first value.
	/// </summary>
	/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/> to be constructed.</typeparam>
	public interface IEitherNeedsSecondType<TFirst>
	{
		/// <summary>
		/// Returns an <see cref="Either{TFirst,TSecond}"/> instance with the pre-provided first value, and <typeparamref name="TSecond"/> for the second type.
		/// </summary>
		/// <typeparam name="TSecond">The first type of the <see cref="Either{TFirst,TSecond}"/> to be constructed.</typeparam>
		/// <returns>An <see cref="Either{TFirst,TSecond}"/> instance with the pre-provided first value, and <typeparamref name="TSecond"/> for the second type.</returns>
		Either<TFirst, TSecond> WithSecond<TSecond>();
	}

	internal class EitherNeedsSecondType<TFirst> : IEitherNeedsSecondType<TFirst>
	{
		private readonly TFirst _first;

		public EitherNeedsSecondType(TFirst first)
		{
			_first = first;
		}

		public Either<TFirst, TSecond> WithSecond<TSecond>()
		{
			return new Either<TFirst, TSecond>(_first);
		}
	}

	/// <summary>
	/// Represents a partially-constructed <see cref="Either{TFirst,TSecond}"/>, with a pre-provided second type.
	/// </summary>
	/// <typeparam name="TSecond">The second type of the <see cref="Either{TFirst,TSecond}"/> instance to be constructed.</typeparam>
	public interface IEitherNeedsFirstValue<TSecond>
	{
		/// <summary>
		/// Returns an <see cref="Either{TFirst,TSecond}"/> instance with the pre-provided second type, and <paramref name="first"/> for the first value.
		/// </summary>
		/// <typeparam name="TFirst">The first type of the returned <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <returns>An <see cref="Either{TFirst,TSecond}"/> instance with <typeparamref name="TSecond"/> for the second type, and <paramref name="first"/> for the first value.</returns>
		Either<TFirst, TSecond> WithFirst<TFirst>(TFirst first);
	}

	internal class EitherNeedsFirstValue<TSecond> : IEitherNeedsFirstValue<TSecond>
	{
		public Either<TFirst, TSecond> WithFirst<TFirst>(TFirst first)
		{
			return new Either<TFirst, TSecond>(first);
		}
	}

	/// <summary>
	/// Represents a partially-constructed <see cref="Either{TFirst,TSecond}"/>, with a pre-provided first value.
	/// </summary>
	/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/> instance to be constructed.</typeparam>
	public interface IEitherNeedsSecondValue<TFirst>
	{
		/// <summary>
		/// Returns an <see cref="Either{TFirst,TSecond}"/> instance with the pre-provided first type, and <paramref name="second"/> for the second value.
		/// </summary>
		/// <typeparam name="TSecond">The second type of the returned <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <returns>An <see cref="Either{TFirst,TSecond}"/> instance with <typeparamref name="TFirst"/> for the second type, and <paramref name="second"/> for the second value.</returns>
		Either<TFirst, TSecond> WithSecond<TSecond>(TSecond second);
	}

	internal class EitherNeedsSecondValue<TFirst> : IEitherNeedsSecondValue<TFirst>
	{
		public Either<TFirst, TSecond> WithSecond<TSecond>(TSecond second)
		{
			return new Either<TFirst, TSecond>(second);
		}
	}
}
