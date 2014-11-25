using System;

namespace Options.Core
{
    /// <summary>
    ///     Implemented by types that may or may not contain an <see cref="IEquatable{T}" /> value.
    /// </summary>
    /// <typeparam name="T">The type of value that may be present</typeparam>
    public interface IEquatableOption<T> : IOption<T>, IEquatable<IOption<T>>
    {
    }
}