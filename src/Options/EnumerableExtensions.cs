using System;
using System.Collections.Generic;
using System.Linq;

namespace Options
{
	///<summary>
	///	Provides extensions to <see cref = "IEnumerable{T}" />
	///</summary>
	public static class EnumerableExtensions
	{
		///<summary>
		/// Returns an <see cref="Option{TOption}"/> containing the first value that matches or no value, if none match.
		///</summary>
		///<param name="source">The <see cref="IEnumerable{T}"/> to search.</param>
		///<param name="predicate">A <see cref="Predicate{T}"/> used to test the values yielded by <paramref name="source"/></param>
		///<typeparam name="TOption">The type of values yielded by <paramref name="source"/></typeparam>
		///<returns>An <see cref="Option{TOption}"/> of <typeparamref name="TOption"/> internal type</returns>
		///<exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		///<exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
		public static Option<TOption> OptionFirst<TOption>(this IEnumerable<TOption> source, Predicate<TOption> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return source
				.Select(o => new Option<TOption>(o))
				.FirstOrDefault(option => option.Transform(o => predicate(o)).GetValueOrDefault(false));
		}

		///<summary>
		/// Returns an <see cref="Option{TOption}"/> containing the first value from <paramref name="source"/> or no value, if none exist.
		///</summary>
		///<param name="source">The <see cref="IEnumerable{T}"/> to search.</param>
		///<typeparam name="TOption">The type of values yielded by <paramref name="source"/></typeparam>
		///<returns>An <see cref="Option{TOption}"/> of <typeparamref name="TOption"/> internal type</returns>
		///<exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static Option<TOption> OptionFirst<TOption>(this IEnumerable<TOption> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return source.OptionFirst(o => true);
		}

		///<summary>
		/// Returns an <see cref="Option{TOption}"/> containing the only value that matches or no value, if none match.
		///</summary>
		///<param name="source">The <see cref="IEnumerable{T}"/> to search.</param>
		///<param name="predicate">A <see cref="Predicate{T}"/> used to test the values yielded by <paramref name="source"/></param>
		///<typeparam name="TOption">The type of values yielded by <paramref name="source"/></typeparam>
		///<returns>An <see cref="Option{TOption}"/> of <typeparamref name="TOption"/> internal type</returns>
		///<exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		///<exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
		public static Option<TOption> OptionSingle<TOption>(this IEnumerable<TOption> source, Predicate<TOption> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return source
				.Select(o => new Option<TOption>(o))
				.SingleOrDefault(option => option.Transform(o => predicate(o)).GetValueOrDefault(false));
		}

		///<summary>
		/// Returns an <see cref="Option{TOption}"/> containing the only value from <paramref name="source"/> or no value, if none exist.
		///</summary>
		///<param name="source">The <see cref="IEnumerable{T}"/> to search.</param>
		///<typeparam name="TOption">The type of values yielded by <paramref name="source"/></typeparam>
		///<returns>An <see cref="Option{TOption}"/> of <typeparamref name="TOption"/> internal type</returns>
		///<exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static Option<TOption> OptionSingle<TOption>(this IEnumerable<TOption> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return source.OptionSingle(o => true);
		}
	}
}
