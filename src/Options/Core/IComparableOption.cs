using System;

namespace Options.Core
{
    /// <summary>
    ///     Implemented by types that may or may not contain an <see cref="IComparable{T}" /> value.
    /// </summary>
    /// <typeparam name="T">The type of value that may be present</typeparam>
    public interface IComparableOption<T> : IOption<T>, IComparable<IOption<T>>
    {
    }
}