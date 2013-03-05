using System;
using System.Collections.Generic;
using System.Linq;

#if !NETFX_CORE && !WINDOWS_PHONE
using Microsoft.FSharp.Core;
#endif

namespace Options
{
	///<summary>
	///	Extension methods designed to ease the pain of working with <see cref = "Option{TOption}" />.
	///</summary>
	public static class OptionExtensions
	{
		///<summary>
		///	Projects the value of an option into a new form.
		///</summary>
		///<param name = "source">The optional value to invoke a transform function on.</param>
		///<param name = "selector">A transform function to apply to the optional value.</param>
		///<typeparam name = "TSource">The type of the value of <paramref name = "source" />.</typeparam>
		///<typeparam name = "TResult">The type of the value returned by <paramref name = "selector" /></typeparam>
		///<returns>
		/// An <see cref = "Option{T}"/> whose value is the result of invoking the transform function on the value of <paramref name = "source"/>.
		///</returns>
		///<remarks>
		///	If <paramref name = "selector" /> is null, the returned <see cref = "Option{TOption}" /> will never contain a value.
		///</remarks>
		public static Option<TResult> Select<TSource, TResult>(this Option<TSource> source, Func<TSource, TResult> selector)
		{
			var funcOption = Option.Create(selector);

			return funcOption.Handle(f => source.Handle(v => new Option<TResult>(f(v)),
			                                            () => new Option<TResult>()),
			                         () => new Option<TResult>());
		}

		///<summary>
		///	Projects the value of an option to an <see cref = "Option{T}" />,  and invokes a result selector function on the value therein.
		///</summary>
		///<param name = "option">An optional value to project.</param>
		///<param name = "resultSelector">A transform function to apply to the value of the intermediate option.</param>
		///<typeparam name = "TSource">The type of the elements of source.</typeparam>
		///<typeparam name = "TResult">The type of the value of the resulting option.</typeparam>
		///<returns>An <see cref = "Option{T}" /> whose value is the result of mapping the result value and its corresponding source value to a result value. If the source or intermediate option has no value, return an empty option of type <typeparamref name="TResult"/>.</returns>
		///<exception cref = "ArgumentNullException"><paramref name="resultSelector"/> is null.</exception>
		public static Option<TResult> SelectMany<TSource, TResult>(this Option<TSource> option, Func<TSource, Option<TResult>> resultSelector)
		{
			return resultSelector == null
							? Option.Create<TResult>()
							: option.Select(resultSelector).GetValueOrDefault(Option.Create<TResult>());
		}

		///<summary>
		///	Projects the value of an option to an <see cref = "Option{T}" />, intersects the source and resulting options, and invokes a result selector function on the value therein.
		///</summary>
		///<param name = "option">An optional value to project.</param>
		///<param name = "optionSelector">A transform function to apply to the value of the input option.</param>
		///<param name = "resultSelector">A transform function to apply to the value of the intermediate option.</param>
		///<typeparam name = "TSource">The type of the elements of source.</typeparam>
		///<typeparam name = "TIntermediate">The type of the intermediate value returned by optionSelector.</typeparam>
		///<typeparam name = "TResult">The type of the value of the resulting option.</typeparam>
		///<returns>An <see cref = "Option{T}" /> whose value is the result of invoking the transform function optionSelector on the value of source and then mapping the result value and its corresponding source value to a result value. If the source or intermediate option has no value, return an empty option of type <typeparamref name="TResult"/>.</returns>
		///<exception cref = "ArgumentNullException"><paramref name="optionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
		public static Option<TResult> SelectMany<TSource, TIntermediate, TResult>(this Option<TSource> option, Func<TSource, Option<TIntermediate>> optionSelector, Func<TSource, TIntermediate, TResult> resultSelector)
		{
			if (optionSelector == null)
			{
				throw new ArgumentNullException("optionSelector");
			}
			if (resultSelector == null)
			{
				throw new ArgumentNullException("resultSelector");
			}
			return option.Intersect(option.SelectMany(optionSelector)).Select(pair => resultSelector(pair.Item1, pair.Item2));
		}

		/// <summary>
		/// Filters an option based on a predicate.
		/// </summary>
		/// <param name="option">The <see cref="Option{TOption}"/> to filter.</param>
		/// <param name="predicate">A <see cref="Predicate{TResult}"/> used to filter the option</param>
		/// <typeparam name="TSource">The type of the value contained by <paramref name="option"/></typeparam>
		/// <returns><paramref name="option"/>, if <paramref name="option"/> has a value and <paramref name="predicate"/> returns true; an empty <see cref="Option{TOption}"/>, otherwise.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
		public static Option<TSource> Where<TSource>(this Option<TSource> option, Func<TSource, bool> predicate)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return option.Handle(predicate, () => false)
			       	? option
			       	: Option.Create<TSource>();
		}

		///<summary>
		///	Retrieves the value contained in <paramref name = "option" /> or a supplied default.
		///</summary>
		///<param name = "option">An <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> internal type</param>
		///<param name = "defaultValue">The <typeparamref name = "TOption" /> value to return, if <paramref name = "option" /> does not contain a value</param>
		///<typeparam name = "TOption">The internal type of <paramref name = "option" /></typeparam>
		///<returns>A <typeparamref name = "TOption" /> value. The internal value of <paramref name = "option" /> or <paramref name = "defaultValue" /></returns>
		public static TOption GetValueOrDefault<TOption>(this Option<TOption> option, TOption defaultValue)
		{
			return option.Handle(v => v, () => defaultValue);
		}

		///<summary>
		///	Retrieves the value contained in <paramref name = "option" /> or the default value of the type.
		///</summary>
		///<param name = "option">An <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> internal type</param>
		///<typeparam name = "TOption">The internal type of <paramref name = "option" /></typeparam>
		///<returns>A <typeparamref name = "TOption" /> value. The internal value of <paramref name = "option" /> or the default value of <typeparamref name="TOption"/></returns>
		public static TOption GetValueOrDefault<TOption>(this Option<TOption> option)
			where TOption : struct
		{
			return option.GetValueOrDefault(default(TOption));
		}

		///<summary>
		///	Retrieves the value contained in <paramref name = "option" /> or throws an exception.
		///</summary>
		///<param name = "option">An <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> internal type</param>
		///<param name = "createException">A function used to create an <typeparamref name = "TException" /> to throw</param>
		///<typeparam name = "TOption">The internal type of <paramref name = "option" /></typeparam>
		///<typeparam name = "TException">The type of exception to throw</typeparam>
		///<returns>The internal value of <paramref name = "option" /></returns>
		///<exception><paramref name = "option" /> contains no value</exception>
		///<exception cref = "ArgumentNullException"><paramref name = "createException" /> is null</exception>
		public static TOption GetValueOrThrow<TOption, TException>(this Option<TOption> option,
		                                                           Func<TException> createException)
			where TException : Exception
		{
			if (createException == null)
			{
				throw new ArgumentNullException("createException");
			}
			return option.Handle(v => v, () => { throw createException(); });
		}

		///<summary>
		///	Retrieves the value contained in <paramref name = "option" /> or throws a <see cref = "NoneException" />.
		///</summary>
		///<param name = "option">An <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> internal type</param>
		///<typeparam name = "TOption">The internal type of <paramref name = "option" /></typeparam>
		///<returns>The internal value of <paramref name = "option" /></returns>
		///<exception cref = "NoneException"><paramref name = "option" /> contains no value</exception>
		public static TOption GetValueOrThrow<TOption>(this Option<TOption> option)
		{
			return option.Handle(v => v, () => { throw new NoneException(); });
		}

		///<summary>
		///	Returns a <see cref = "bool" /> indicating whether <paramref name = "option" /> contains a value.
		///</summary>
		///<param name = "option">The <see cref = "Option{TOption}" /> to test</param>
		///<typeparam name = "TOption">The internal type of <paramref name = "option" /></typeparam>
		///<returns>True, if <paramref name = "option" /> contains a value; false, if not.</returns>
		public static bool IsSome<TOption>(this Option<TOption> option)
		{
			return option.Select(v => true).GetValueOrDefault(false);
		}

		///<summary>
		///	Converts an <see cref = "Option{TOption}" /> to a nullable value type.
		///</summary>
		///<param name = "option">The <see cref = "Option{TOption}" /> to convert to a nullable <typeparamref name = "TOption" /></param>
		///<typeparam name = "TOption">The internal type of <paramref name = "option" /></typeparam>
		///<returns>A nullable <typeparamref name = "TOption" /> which will contain a value if <paramref name = "option" /> does.</returns>
		public static TOption? AsNullable<TOption>(this Option<TOption> option)
			where TOption : struct
		{
			return option.Select(v => (TOption?)v).GetValueOrDefault(null);
		}

		///<summary>
		///	Converts an <see cref = "Option{TOption}" /> to a reference type.
		///</summary>
		///<param name = "option">The <see cref = "Option{TOption}" /> to convert to a nullable <typeparamref name = "TOption" /></param>
		///<typeparam name = "TOption">The internal type of <paramref name = "option" /></typeparam>
		///<returns>A(n) <typeparamref name = "TOption" /> which will be null if <paramref name = "option" /> does not contain a value.</returns>
		public static TOption AsUnprotected<TOption>(this Option<TOption> option)
			where TOption : class
		{
			return option.GetValueOrDefault(null);
		}

		///<summary>
		///	Combines 2 <see cref = "Option{TOption}" />s into an <see cref = "Option{TOption}" /> that contains a <see cref = "Tuple" /> of <typeparamref name = "T1" /> and <typeparamref name = "T2" />
		///</summary>
		///<param name = "first">The first <see cref = "Option{TOption}" /></param>
		///<param name = "second">The second <see cref = "Option{TOption}" /></param>
		///<typeparam name = "T1">The internal type of <paramref name = "first" /></typeparam>
		///<typeparam name = "T2">The internal type of <paramref name = "second" /></typeparam>
		///<returns>An <see cref = "Option{TOption}" /></returns>
		///<remarks>
		///	The <see cref = "Option{TOption}" /> returned will only contain a value if both parameters contain a value.
		///</remarks>
		public static Option<Tuple<T1, T2>> Intersect<T1, T2>(this Option<T1> first, Option<T2> second)
		{
			return first.Handle(v => second.Select(v2 => Tuple.Create(v, v2)),
			                    () => new Option<Tuple<T1, T2>>());
		}

		///<summary>
		///	Returns the first <see cref = "Option{TOption}" /> that contains a value from <paramref name = "options" />.
		///</summary>
		///<param name = "options">An enumerable of <see cref = "Option{TOption}" /></param>
		///<typeparam name = "TOption">The internal type of the <see cref = "Option{TOption}" />s</typeparam>
		///<returns>The first non-empty <see cref = "Option{TOption}" /> yielded, or an empty <see cref = "Option{TOption}" /></returns>
		public static Option<TOption> Coalesce<TOption>(this IEnumerable<Option<TOption>> options)
		{
			return options == null
			       	? Option.Create<TOption>()
			       	: options.FirstOrDefault(o => o.Select(v => true).GetValueOrDefault(false));
		}

#if !NETFX_CORE && !WINDOWS_PHONE
		///<summary>
		/// Converts an <see cref="Option{TOption}"/> to an equivalent <see cref="FSharpOption{T}"/>
		///</summary>
		///<param name="option">The <see cref="Option{TOption}"/> to convert</param>
		///<typeparam name = "TOption">The internal type of <paramref name = "option" /></typeparam>
		///<returns>An <see cref="FSharpOption{T}"/> equivalent to <paramref name="option"/></returns>
		public static FSharpOption<TOption> ToFSharp<TOption>(this Option<TOption> option)
		{
			return option.Handle(FSharpOption<TOption>.Some, () => FSharpOption<TOption>.None);
		}
#endif

        /// <summary>
		/// Runs one of the given actions based on whether the given <see cref="Option{TOption}"/> has a value.
		/// </summary>
		///<typeparam name = "TOption">The internal type of <paramref name = "option" /></typeparam>
		/// <param name="option">The <see cref="Option{TOption}"/> to act on.</param>
		/// <param name="ifSome">The action that is run when the option has a value.</param>
		/// <param name="ifNone">The action that is run when the option has no value.</param>
		public static void Act<TOption>(this Option<TOption> option, Action<TOption> ifSome, Action ifNone)
		{
			if (ifSome == null)
			{
				throw new ArgumentNullException("ifSome");
			}
			if (ifNone == null)
			{
				throw new ArgumentNullException("ifNone");
			}
			if (option.IsSome())
			{
				ifSome(option.GetValueOrThrow());
			}
			else
			{
				ifNone();
			}
		}

		/// <summary>
		/// Runs one an action on the value of the given <see cref="Option{TOption}"/>, if it has a value.
		/// </summary>
		///<typeparam name = "TOption">The internal type of <paramref name = "option" /></typeparam>
		/// <param name="option">The <see cref="Option{TOption}"/> to act on.</param>
		/// <param name="ifSome">The action that is run when the option has a value.</param>
		public static void Act<TOption>(this Option<TOption> option, Action<TOption> ifSome)
		{
			if (ifSome == null)
			{
				throw new ArgumentNullException("ifSome");
			}
			option.Act(ifSome, () => { });
		}

		/// <summary>
		/// Returns an empty <see cref="Option{TOption}"/> if the given <see cref="System.String"/> is null, empty, or only whitespace.
		/// </summary>
		/// <param name="value">The given <see cref="System.String"/></param>
		/// <returns>An empty <see cref="Option{TOption}"/> if the given <see cref="System.String"/> is null, empty, or only whitespace; otherwise an <see cref="Option{TOption}"/> containing the given string.</returns>
		public static Option<string> NoneIfNullOrWhiteSpace(this string value)
		{
			return string.IsNullOrWhiteSpace(value) ? Option.Create<string>() : Option.Create(value);
		}

		/// <summary>
		/// 	Executes the functions, varying whether a value is contained in the <see cref = "Option{TOption}" />.
		/// </summary>
		/// <typeparam name="T1">The type of the first item in the <see cref="Tuple"/> contained by <paramref name="option"/></typeparam>
		/// <typeparam name="T2">The type of the second item in the <see cref="Tuple"/> contained by <paramref name="option"/></typeparam>
		/// <typeparam name = "TResult">The type returned by <paramref name = "ifSome" /> and <paramref name = "ifNone" /></typeparam>
		/// <param name="option">The <see cref="Option{TOption}"/> to be handled</param>
		/// <param name = "ifSome">The function to execute if a value is present</param>
		/// <param name = "ifNone">The function to execute if no value is present</param>
		/// <returns>The value returned by <paramref name = "ifSome" /> or <paramref name = "ifNone" /></returns>
		/// <exception cref = "ArgumentNullException"><paramref name = "ifSome" /> is null</exception>
		/// <exception cref = "ArgumentNullException"><paramref name = "ifNone" /> is null</exception>
		public static TResult Handle<T1, T2, TResult>(this Option<Tuple<T1, T2>> option, Func<T1, T2, TResult> ifSome, Func<TResult> ifNone)
		{
			if (ifSome == null)
			{
				throw new ArgumentNullException("ifSome");
			}
			if (ifNone == null)
			{
				throw new ArgumentNullException("ifNone");
			}
			return option.Handle(tuple => ifSome(tuple.Item1, tuple.Item2), ifNone);
		}
	}
}
