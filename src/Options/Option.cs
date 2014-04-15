using System;

#if !NETFX_CORE && !WINDOWS_PHONE
using Microsoft.FSharp.Core;
#endif

namespace Options
{
	/// <summary>
	/// 	Methods to assist in the creation of <see cref = "Option{TOption}" />s
	/// </summary>
	public static class Option
	{
		/// <summary>
		/// 	Creates an <see cref = "Option{TOption}" />.
		/// </summary>
		/// <typeparam name = "TOption">The type of the <see cref = "Option{TOption}" />'s internal value</typeparam>
		/// <param name = "value">The <typeparamref name = "TOption" /> which will be represented by the created <see cref = "Option{TOption}" /></param>
		/// <returns>An <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> type. Will be None, if <paramref name = "value" /> is null.</returns>
		public static Option<TOption> Create<TOption>(TOption value)
		{
			return new Option<TOption>(value);
		}

		/// <summary>
		/// 	Creates an <see cref = "Option{TOption}" /> from a nullable value type.
		/// </summary>
		/// <typeparam name = "TOption">The type of the <see cref = "Option{TOption}" />'s internal value</typeparam>
		/// <param name = "value">The <typeparamref name = "TOption" /> which will be represented by the created <see cref = "Option{TOption}" /></param>
		/// <returns>An <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> type. Will be None, if <paramref name = "value" /> is null.</returns>
		public static Option<TOption> Create<TOption>(TOption? value)
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
		public static Option<TOption> Create<TOption>()
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

#if !NETFX_CORE && !WINDOWS_PHONE
		///<summary>
		/// Converts an <see cref="FSharpOption{T}"/> to an equivalent <see cref="Option{TOption}"/>.
		///</summary>
		///<param name="fSharpOption">The <see cref="FSharpOption{T}"/> to convert</param>
		///<typeparam name="TOption">The internal value of <paramref name="fSharpOption"/></typeparam>
		///<returns>An <see cref="Option{TOption}"/> equivalent to <paramref name="fSharpOption"/></returns>
		public static Option<TOption> FromFSharp<TOption>(FSharpOption<TOption> fSharpOption)
		{
			if (fSharpOption == null || FSharpOption<TOption>.get_IsNone(fSharpOption))
			{
				return Create<TOption>();
			}
			return new Option<TOption>(fSharpOption.Value);
		}
#endif

        /// <summary>
		/// 	Creates an <see cref = "Option{TOption}" /> from a reference type, given a non-null value.
		/// </summary>
		/// <typeparam name = "TOption">The type of the <see cref = "Option{TOption}" />'s internal value</typeparam>
		/// <param name = "value">The <typeparamref name = "TOption" /> which will be represented by the created <see cref = "Option{TOption}" /></param>
		/// <exception cref="System.ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
		/// <returns>An <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> type, containing <paramref name="value"/>.</returns>
		public static Option<TOption> Some<TOption>(TOption? value)
			where TOption : struct
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return new Option<TOption>(value.Value);
		}

		/// <summary>
		/// 	Creates an <see cref = "Option{TOption}" /> from a reference type, given a non-null value.
		/// </summary>
		/// <typeparam name = "TOption">The type of the <see cref = "Option{TOption}" />'s internal value</typeparam>
		/// <param name = "value">The <typeparamref name = "TOption" /> which will be represented by the created <see cref = "Option{TOption}" /></param>
		/// <exception cref="System.ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
		/// <returns>An <see cref = "Option{TOption}" /> of <typeparamref name = "TOption" /> type, containing <paramref name="value"/>.</returns>
		public static Option<TOption> Some<TOption>(TOption value)
			where TOption : class
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return new Option<TOption>(value);
		}

        /// <summary>
        /// Returns a value that will be implicitly coerced into the appropriate <see cref="Option{TOption}"/> type.
        /// </summary>
        /// <returns>A value that will be implicitly coerced into the appropriate <see cref="Option{TOption}"/> type.</returns>
	    public static OptionNone None()
	    {
	        return new OptionNone();
	    }
	}
}
