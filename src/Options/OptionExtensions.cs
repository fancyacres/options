using System;
using System.Collections.Generic;
using System.Linq;

namespace Options
{
	public static class OptionExtensions
	{
		public static Option<TResult> Transform<TOption, TResult>(this Option<TOption> option, Func<TOption, TResult> func)
		{
			if (func == null)
			{
				throw new ArgumentNullException("func");
			}
			return option.Handle(v => new Option<TResult>(func(v)),
			                     () => new Option<TResult>());
		}

		public static TOption GetValueOrDefault<TOption>(this Option<TOption> option, TOption defaultValue)
		{
			return option.Handle(v => v, () => defaultValue);
		}

		public static TOption GetValueOrThrow<TOption, TException>(this Option<TOption> option,
		                                                           Func<TException> createException)
			where TException : Exception
		{
			return option.Handle(v => v, () => { throw createException(); });
		}

		public static TOption GetValueOrThrow<TOption>(this Option<TOption> option)
		{
			return option.Handle(v => v, () => { throw new NoneException(); });
		}

		public static bool IsSome<TOption>(this Option<TOption> option)
		{
			return option.Transform(v => true).GetValueOrDefault(false);
		}

		public static TOption? AsNullable<TOption>(this Option<TOption> option)
			where TOption : struct
		{
			return option.Transform(v => (TOption?)v).GetValueOrDefault(null);
		}

		public static TOption AsUnprotected<TOption>(this Option<TOption> option)
			where TOption : class
		{
			return option.GetValueOrDefault(null);
		}

		public static Option<Tuple<T1, T2>> Intersect<T1, T2>(this Option<T1> option, Option<T2> second)
		{
			return option.Handle(v => second.Transform(v2 => Tuple.Create(v, v2)),
			                     () => new Option<Tuple<T1, T2>>());
		}

		public static Option<TOption> Coalese<TOption>(this IEnumerable<Option<TOption>> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			return options.FirstOrDefault(o => o.Transform(v => true).GetValueOrDefault(false));
		}
	}
}
