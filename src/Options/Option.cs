using System;
using System.Linq;

namespace Options
{
	/// <summary>
	/// 	Methods to assist in the creation of <see cref = "Option{TOption}" />s
	/// </summary>
	public static class Option
	{
		/// <summary>
		/// 	Creates an <see cref = "Option{TOption}" /> from a reference type.
		/// </summary>
		/// <typeparam name = "TOption">The type of the <see cref = "Option{TOption}" />'s internal value</typeparam>
		/// <param name = "value">The <typeparamref name = "TOption" /> which will be represented by the created <see cref = "Option{TOption}" /></param>
		/// <returns>An <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> type. Will be None, if <paramref name = "value" /> is null.</returns>
		public static Option<TOption> AvoidNull<TOption>(TOption value)
			where TOption : class
		{
			return new Option<TOption>(value);
		}

		/// <summary>
		/// 	Creates an <see cref = "Option{TOption}" /> from a nullable value type.
		/// </summary>
		/// <typeparam name = "TOption">The type of the <see cref = "Option{TOption}" />'s internal value</typeparam>
		/// <param name = "value">The <typeparamref name = "TOption" /> which will be represented by the created <see cref = "Option{TOption}" /></param>
		/// <returns>An <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> type. Will be None, if <paramref name = "value" /> is null.</returns>
		public static Option<TOption> AvoidNull<TOption>(TOption? value)
			where TOption : struct
		{
			return value.HasValue
			       	? new Option<TOption>(value.Value)
			       	: new Option<TOption>();
		}

		/// <summary>
		/// 	Creates an <see cref = "Option{TOption}" /> with no value.
		/// </summary>
		/// <typeparam name = "TOption">The internal type of the <see cref = "Option{TOption}" /></typeparam>
		/// <returns>An <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> type with no value.</returns>
		public static Option<TOption> None<TOption>()
		{
			return new Option<TOption>();
		}

		/// <summary>
		/// 	Creates a function which will never return null.
		/// </summary>
		/// <typeparam name = "TOption">The type normally returned by <paramref name = "guarded" /></typeparam>
		/// <param name = "guarded">A function which will be wrapped to prevent a null return</param>
		/// <returns>A function, which will return an option containing a value if <paramref name = "guarded" /> returns non-null.</returns>
		/// <remarks>
		/// 	If <paramref name = "guarded" /> is null, the function created will always return an <see cref = "Option{TOption}" /> with no value.
		/// </remarks>
		public static Func<Option<TOption>> Guard<TOption>(Func<TOption> guarded)
		{
			return guarded == null
			       	? (Func<Option<TOption>>)(() => new Option<TOption>())
			       	: (() => new Option<TOption>(guarded()));
		}
	}
}
