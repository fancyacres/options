using System;

namespace Options
{
	/// <summary>
	/// A set of utility functions for the <see cref="Either{TFirst,TSecond}"/> type.
	/// </summary>
	public static class EitherExtensions
	{
		/// <summary>
		///		Invokes an <see cref="Action{T}"/> based on which value is present in an <see cref="Either{TFirst,TSecond}"/>.
		/// </summary>
		/// <param name="either">An <see cref="Either{TFirst,TSecond}"/>.</param>
		/// <param name="ifFirst">An <see cref="Action{T}"/> to invoke if <paramref name="either"/> has a first value.</param>
		/// <param name="ifSecond">An <see cref="Action{T}"/> to invoke if <paramref name="either"/> has a second value.</param>
		/// <typeparam name="TFirst">The first type of <paramref name="either"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of <paramref name="either"/>.</typeparam>
		/// <exception cref="ArgumentNullException"><paramref name="either"/>, <paramref name="ifFirst"/>, or <paramref name="ifSecond"/> is null.</exception>
		public static void Act<TFirst, TSecond>(this Either<TFirst, TSecond> either, Action<TFirst> ifFirst, Action<TSecond> ifSecond)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}
			if (ifFirst == null)
			{
				throw new ArgumentNullException("ifFirst");
			}
			if (ifSecond == null)
			{
				throw new ArgumentNullException("ifSecond");
			}

			either.Handle(f =>
			              	{
			              		ifFirst(f);
			              		return true;
			              	},
			              s =>
			              	{
			              		ifSecond(s);
			              		return true;
			              	});
		}

		/// <summary>
		///		Invokes an <see cref="Action{T}"/> based on which value is present in an <see cref="Either{TFirst,TSecond}"/>.
		/// </summary>
		/// <param name="either">An <see cref="Either{TFirst,TSecond}"/>.</param>
		/// <param name="ifFirst">An <see cref="Action{T}"/> to invoke if <paramref name="either"/> has a first value.</param>
		/// <typeparam name="TFirst">The first type of <paramref name="either"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of <paramref name="either"/>.</typeparam>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> or <paramref name="ifFirst"/> is null.</exception>
		public static void Act<TFirst, TSecond>(this Either<TFirst, TSecond> either, Action<TFirst> ifFirst)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}
			if (ifFirst == null)
			{
				throw new ArgumentNullException("ifFirst");
			}

			either.Handle(f =>
			              	{
			              		ifFirst(f);
			              		return true;
			              	},
			              _ => true);
		}

		/// <summary>
		///		Invokes an <see cref="Action{T}"/> based on which value is present in an <see cref="Either{TFirst,TSecond}"/>.
		/// </summary>
		/// <param name="either">An <see cref="Either{TFirst,TSecond}"/>.</param>
		/// <param name="ifSecond">An <see cref="Action{T}"/> to invoke if <paramref name="either"/> has a second value.</param>
		/// <typeparam name="TFirst">The first type of <paramref name="either"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of <paramref name="either"/>.</typeparam>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> or <paramref name="ifSecond"/> is null.</exception>
		public static void Act<TFirst, TSecond>(this Either<TFirst, TSecond> either, Action<TSecond> ifSecond)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}
			if (ifSecond == null)
			{
				throw new ArgumentNullException("ifSecond");
			}

			either.Handle(_ => true,
			              s =>
			              	{
			              		ifSecond(s);
			              		return true;
			              	});
		}

		/// <summary>
		/// Returns <c>true</c> if <paramref name="either"/> has a value for its first type; <c>false</c> otherwise.
		/// </summary>
		/// <param name="either">A value of type <see cref="Either{TFirst,TSecond}"/>.</param>
		/// <typeparam name="TFirst">The first type of <paramref name="either"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of <paramref name="either"/>.</typeparam>
		/// <returns><c>true</c> if <paramref name="either"/> has a value for its first type; <c>false</c> otherwise.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> is null.</exception>
		public static bool IsFirst<TFirst, TSecond>(this Either<TFirst, TSecond> either)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}
			return either.Handle(_ => true, _ => false);
		}

		/// <summary>
		/// Returns <c>true</c> if <paramref name="either"/> has a value for its second type; <c>false</c> otherwise.
		/// </summary>
		/// <param name="either">A value of type <see cref="Either{TFirst,TSecond}"/>.</param>
		/// <typeparam name="TFirst">The first type of <paramref name="either"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of <paramref name="either"/>.</typeparam>
		/// <returns><c>true</c> if <paramref name="either"/> has a value for its second type; <c>false</c> otherwise.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> is null.</exception>
		public static bool IsSecond<TFirst, TSecond>(this Either<TFirst, TSecond> either)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}
			return !either.IsFirst();
		}

		/// <summary>
		/// Returns <paramref name="either"/> with its type arguments reversed and the same value.
		/// </summary>
		/// <param name="either">A value of type <see cref="Either{TFirst,TSecond}"/>.</param>
		/// <typeparam name="TFirst">The first type of <paramref name="either"/>, and the second type of the result.</typeparam>
		/// <typeparam name="TSecond">The second type of <paramref name="either"/>, and the first type of the result.</typeparam>
		/// <returns><paramref name="either"/> with its type arguments reversed and the same value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> is null.</exception>
		public static Either<TSecond, TFirst> Swap<TFirst, TSecond>(this Either<TFirst, TSecond> either)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}

			return
				either
					.Handle(
						f => new Either<TSecond, TFirst>(f),
						s => new Either<TSecond, TFirst>(s));
		}

		/// <summary>
		/// Returns a <typeparamref name="TFirst"/> value from an <see cref="Either{TFirst,TSecond}"/> if it has one. If not, it will throw an exception.
		/// </summary>
		/// <param name="either">The <see cref="Either{TFirst,TSecond}"/> to get a value from.</param>
		/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <returns>The first value of <paramref name="either"/>, if it has a first value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> is null.</exception>
		/// <exception cref="InvalidOperationException"><paramref name="either"/> does not have a first value.</exception>
		public static TFirst GetFirstOrThrow<TFirst, TSecond>(this Either<TFirst, TSecond> either)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}

			return
				either.Handle(f => f,
				              _ => { throw new InvalidOperationException("Tried to get the first item out of an either that does not have one."); }
					);
		}

		/// <summary>
		/// Returns a <typeparamref name="TSecond"/> value from an <see cref="Either{TFirst,TSecond}"/> if it has one. If not, it will throw an exception.
		/// </summary>
		/// <param name="either">The <see cref="Either{TFirst,TSecond}"/> to get a value from.</param>
		/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <returns>The second value of <paramref name="either"/>, if it has a second value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> is null.</exception>
		/// <exception cref="InvalidOperationException"><paramref name="either"/> does not have a second value.</exception>
		public static TSecond GetSecondOrThrow<TFirst, TSecond>(this Either<TFirst, TSecond> either)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}

			return
				either.Handle(_ => { throw new InvalidOperationException("Tried to get the second item out of an either that does not have one."); },
											s => s
					);
		}

		/// <summary>
		/// Returns the first value from an <see cref="Either{TFirst,TSecond}"/>, or <paramref name="defaultValue"/> if it does not have a first value.
		/// </summary>
		/// <param name="either">The <see cref="Either{TFirst,TSecond}"/> to get a value from.</param>
		/// <param name="defaultValue">The alternate value to return if <paramref name="either"/> does not have a first value.</param>
		/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <returns>The first value from <paramref name="either"/> if it has one, or <paramref name="defaultValue"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> is null.</exception>
		public static TFirst GetFirstOrDefault<TFirst, TSecond>(this Either<TFirst, TSecond> either, TFirst defaultValue)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}

			return either.Handle(f => f, _ => defaultValue);
		}

		/// <summary>
		/// Returns the second value from an <see cref="Either{TFirst,TSecond}"/>, or <paramref name="defaultValue"/> if it does not have a second value.
		/// </summary>
		/// <param name="either">The <see cref="Either{TFirst,TSecond}"/> to get a value from.</param>
		/// <param name="defaultValue">The alternate value to return if <paramref name="either"/> does not have a second value.</param>
		/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <returns>The second value from <paramref name="either"/> if it has one, or <paramref name="defaultValue"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> is null.</exception>
		public static TSecond GetSecondOrDefault<TFirst, TSecond>(this Either<TFirst, TSecond> either, TSecond defaultValue)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}

			return either.Handle(_ => defaultValue, s => s);
		}

		/// <summary>
		/// Returns the first value from an <see cref="Either{TFirst,TSecond}"/>, or the result of <paramref name="getDefaultValue"/> if it does not have a first value.
		/// </summary>
		/// <param name="either">The <see cref="Either{TFirst,TSecond}"/> to get a value from.</param>
		/// <param name="getDefaultValue">A function to provide an alternate value to return if <paramref name="either"/> does not have a first value.</param>
		/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <returns>The first value from <paramref name="either"/> if it has one, or the result of <paramref name="getDefaultValue"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> is null.</exception>
		/// <remarks><paramref name="getDefaultValue"/> will not be called unless the result cannot be obtained from <paramref name="either"/>.</remarks>
		public static TFirst GetFirstOrDefault<TFirst, TSecond>(this Either<TFirst, TSecond> either, Func<TFirst> getDefaultValue)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}

			return either.Handle(f => f, _ => getDefaultValue());
		}

		/// <summary>
		/// Returns the second value from an <see cref="Either{TFirst,TSecond}"/>, or the result of <paramref name="getDefaultValue"/> if it does not have a second value.
		/// </summary>
		/// <param name="either">The <see cref="Either{TFirst,TSecond}"/> to get a value from.</param>
		/// <param name="getDefaultValue">A function to provide an alternate value to return if <paramref name="either"/> does not have a second value.</param>
		/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <returns>The second value from <paramref name="either"/> if it has one, or the result of <paramref name="getDefaultValue"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> is null.</exception>
		/// <remarks><paramref name="getDefaultValue"/> will not be called unless the result cannot be obtained from <paramref name="either"/>.</remarks>
		public static TSecond GetSecondOrDefault<TFirst, TSecond>(this Either<TFirst, TSecond> either, Func<TSecond> getDefaultValue)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}

			return either.Handle(_ => getDefaultValue(), s => s);
		}

		/// <summary>
		/// Returns the first value from an <see cref="Either{TFirst,TSecond}"/>, or calls <paramref name="ifSecond"/> to obtain a result of the same type from the second value.
		/// </summary>
		/// <param name="either">The <see cref="Either{TFirst,TSecond}"/>.</param>
		/// <param name="ifSecond">A function to transform a <typeparamref name="TSecond"/> into a <typeparamref name="TFirst"/>.</param>
		/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <returns>the first value from <paramref name="either"/>, or the result of <paramref name="ifSecond"/> invoked on the second value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> or <paramref name="ifSecond"/> is null.</exception>
		public static TFirst Handle<TFirst, TSecond>(this Either<TFirst, TSecond> either, Func<TSecond, TFirst> ifSecond)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}
			if (ifSecond == null)
			{
				throw new ArgumentNullException("ifSecond");
			}

			return either.Handle(f => f, ifSecond);
		}

		/// <summary>
		/// Returns the second value from an <see cref="Either{TFirst,TSecond}"/>, or calls <paramref name="ifFirst"/> to obtain a result of the same type from the first value.
		/// </summary>
		/// <param name="either">The <see cref="Either{TFirst,TSecond}"/>.</param>
		/// <param name="ifFirst">A function to transform a <typeparamref name="TFirst"/> into a <typeparamref name="TSecond"/>.</param>
		/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <returns>the second value from <paramref name="either"/>, or the result of <paramref name="ifFirst"/> invoked on the first value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> or <paramref name="ifFirst"/> is null.</exception>
		public static TSecond Handle<TFirst, TSecond>(this Either<TFirst, TSecond> either, Func<TFirst, TSecond> ifFirst)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}
			if (ifFirst == null)
			{
				throw new ArgumentNullException("ifFirst");
			}


			return either.Handle(ifFirst, s => s);
		}

		/// <summary>
		/// Creates a <see cref="Tuple{T1,T2}"/> from an <see cref="Either{TFirst,TSecond}"/>, filling in the missing value by invoking the appropriate given function.
		/// </summary>
		/// <param name="either">The <see cref="Either{TFirst,TSecond}"/>.</param>
		/// <param name="ifSecond">A function to provide the first value when it is absent.</param>
		/// <param name="ifFirst">A function to provide the second value when it is absent.</param>
		/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <returns>A <see cref="Tuple{T1,T2}"/> with one value from <paramref name="either"/>, and the other from <paramref name="ifSecond"/> or <paramref name="ifFirst"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="either"/>, <paramref name="ifSecond"/>, or <paramref name="ifFirst"/> is null.</exception>
		public static Tuple<TFirst, TSecond> ToTuple<TFirst, TSecond>(this Either<TFirst, TSecond> either, Func<TFirst> ifSecond, Func<TSecond> ifFirst)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}
			if (ifSecond == null)
			{
				throw new ArgumentNullException("ifSecond");
			}
			if (ifFirst == null)
			{
				throw new ArgumentNullException("ifFirst");
			}

			return either.Handle(
				f => new Tuple<TFirst, TSecond>(f, ifFirst()),
				s => new Tuple<TFirst, TSecond>(ifSecond(), s)
				);
		}

		/// <summary>
		/// Creates an <see cref="Option{TOption}"/> with the first value of an <see cref="Either{TFirst,TSecond}"/>, or no value.
		/// </summary>
		/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <param name="either">The <see cref="Either{TFirst,TSecond}"/>.</param>
		/// <returns>An <see cref="Option{TOption}"/> with the first value of <paramref name="either"/>, or no value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> is null.</exception>
		public static Option<TFirst> OptionFirst<TFirst, TSecond>(this Either<TFirst, TSecond> either)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}

			return either.Handle(Option.Create, _ => Option.Create<TFirst>());
		}

		/// <summary>
		/// Creates an <see cref="Option{TOption}"/> with the second value of an <see cref="Either{TFirst,TSecond}"/>, or no value.
		/// </summary>
		/// <typeparam name="TFirst">The first type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <typeparam name="TSecond">The second type of the <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <param name="either">The <see cref="Either{TFirst,TSecond}"/>.</param>
		/// <returns>An <see cref="Option{TOption}"/> with the second value of <paramref name="either"/>, or no value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="either"/> is null.</exception>
		public static Option<TSecond> OptionSecond<TFirst, TSecond>(this Either<TFirst, TSecond> either)
		{
			if (either == null)
			{
				throw new ArgumentNullException("either");
			}

			return either.Handle(_ => Option.Create<TSecond>(), Option.Create);
		}

		/// <summary>
		///		Projects the first value of an <see cref="Either{TFirst,TSecond}"/> into a new form.
		/// </summary>
		/// <typeparam name="TFirstSource">The type of the first value of <paramref name="source"/>.</typeparam>
		/// <typeparam name="TFirstResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
		/// <typeparam name="TSecond">The type of the second value of <paramref name="source"/>.</typeparam>
		/// <param name="source">An <see cref="Either{TFirst,TSecond}"/> to invoke a transform function on.</param>
		/// <param name="selector">A transform function to apply to the first value, if it exists.</param>
		/// <returns>
		///		An <see cref="Either{TFirst,TSecond}"/> whose first value is the result of invoking the transform function
		///		on the first value of <paramref name="source"/>, if <paramref name="source"/> has a first value; otherwise
		///		an <see cref="Either{TFirst,TSecond}"/> whose second value is the second value of <paramref name="source"/>.
		///	</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		public static Either<TFirstResult, TSecond> Select<TFirstSource, TFirstResult, TSecond>(
			this Either<TFirstSource, TSecond> source,
			Func<TFirstSource, TFirstResult> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}

			return source.Handle(
				first => new Either<TFirstResult, TSecond>(selector(first)),
				second => new Either<TFirstResult, TSecond>(second)
				);
		}

		/// <summary>
		///		Projects the first value of an <see cref="Either{TFirst,TSecond}"/> to another <see cref="Either{TFirst,TSecond}"/> and invokes a result selector function on the first values, or returns the second value.
		/// </summary>
		/// <param name="source">An <see cref="Either{TFirst,TSecond}"/> to project.</param>
		/// <param name="eitherSelector">A transform function to apply to the first value of the input <see cref="Either{TFirst,TSecond}"/>.</param>
		/// <param name="resultSelector">A transform function to apply to the first values of the source and intermediate <see cref="Either{TFirst,TSecond}">eithers</see>.</param>
		/// <typeparam name="TFirstSource">The type of the first value of <paramref name="source"/>.</typeparam>
		/// <typeparam name="TFirstEither">The type of the first value of the result of <paramref name="eitherSelector"/>.</typeparam>
		/// <typeparam name="TFirstResult">The type of the first value of the resulting <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <typeparam name="TSecond">The type of the second value of the resulting <see cref="Either{TFirst,TSecond}"/>.</typeparam>
		/// <returns>
		///		An <see cref="Either{TFirst,TSecond}"/> whose first value is the result of invoking the transform function <paramref name="eitherSelector"/> on the first value of <paramref name="source"/>,
		///		then invoking <paramref name="resultSelector"/> on the first value of the intermediate result, if <paramref name="source"/> and the intermediate <see cref="Either{TFirst,TSecond}"/>
		///		have a first value; otherwise, the second value of <paramref name="source"/> or the intermediate result, based on which has a second value.
		/// </returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static Either<TFirstResult, TSecond> SelectMany<TFirstSource, TFirstEither, TFirstResult, TSecond>(
			this Either<TFirstSource, TSecond> source,
			Func<TFirstSource, Either<TFirstEither, TSecond>> eitherSelector,
			Func<TFirstSource, TFirstEither, TFirstResult> resultSelector
			)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (eitherSelector == null)
			{
				throw new ArgumentNullException("eitherSelector");
			}
			if (resultSelector == null)
			{
				throw new ArgumentNullException("resultSelector");
			}

			return source.Handle(
				first => eitherSelector(first)
				         	.Handle(intermediate => new Either<TFirstResult, TSecond>(resultSelector(first, intermediate)),
				         	        second => new Either<TFirstResult, TSecond>(second)),
				second => new Either<TFirstResult, TSecond>(second));
		}
	}
}