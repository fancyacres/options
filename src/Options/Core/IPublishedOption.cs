namespace Options.Core
{
    /// <summary>
    /// An <see cref="IDeferredOption{T}"/> that will only evaluate once.
    /// </summary>
    /// <typeparam name="T">The type that will eventually be contained.</typeparam>
    /// <remarks>Because published options will only evaluate once, they can safely be treated as regular <see cref="IOption{T}"/>a</remarks>
    public interface IPublishedOption<out T> : IOption<T>, IDeferredOption<T>
    {
        /// <summary>
        /// Gets whether the published option has evaluated to an <see cref="IOption{T}"/>.
        /// </summary>
        bool HasEvaluated { get; }
    }
}