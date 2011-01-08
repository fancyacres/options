using System;
using System.Linq;

namespace Options
{
	public static class Option
	{
		public static Option<TOption> AvoidNull<TOption>(TOption value)
			where TOption : class
		{
			return new Option<TOption>(value);
		}

		public static Option<TOption> AvoidNull<TOption>(TOption? value)
			where TOption : struct
		{
			return value.HasValue
			       	? new Option<TOption>(value.Value)
			       	: new Option<TOption>();
		}

		public static Option<TOption> None<TOption>()
		{
			return new Option<TOption>();
		}

		public static Func<Option<TOption>> Guard<TOption>(Func<TOption> guarded)
		{
			if (guarded == null)
			{
				throw new ArgumentNullException("guarded");
			}
			return () => new Option<TOption>(guarded());
		}
	}
}
