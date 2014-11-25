using System;

namespace Options.Core
{
    internal static class Funcs
    {
        internal static Func<Unit> MakeFunc(this Action action)
        {
            return () =>
            {
                action();
                return Unit.Default;
            };
        }

        internal static Func<T, Unit> MakeFunc<T>(this Action<T> action)
        {
            return t =>
            {
                action(t);
                return Unit.Default;
            };
        }
    }
}