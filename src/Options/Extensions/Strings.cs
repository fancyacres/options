using Options.Core;

namespace Options.Extensions
{
    /// <summary>
    /// Functions to ease work with <see cref="string"/> and <see cref="IOption{T}"/> of <see cref="string"/>
    /// </summary>
    public static class Strings
    {
        /// <summary>
        /// Create an <see cref="IOption{T}"/>, that will never contain a white space value.
        /// </summary>
        /// <param name="s">The <see cref="string"/> used to create the <see cref="IOption{T}"/>.</param>
        /// <returns>An <see cref="INonWhiteSpaceOption"/></returns>
        public static INonWhiteSpaceOption NoneIfNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s) ? new VerifiedStringOption() : new VerifiedStringOption(s);
        }

        /// <summary>
        /// Create an <see cref="IOption{T}"/>, that will never contain an empty value.
        /// </summary>
        /// <param name="s">The <see cref="string"/> used to create the <see cref="IOption{T}"/>.</param>
        /// <returns>An <see cref="INonEmptyOption"/></returns>
        public static INonEmptyOption NoneIfNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s) ? new VerifiedStringOption() : new VerifiedStringOption(s);
        }

        /// <summary>
        /// Converts <paramref name="option"/> to an <see cref="INonWhiteSpaceOption"/>.
        /// </summary>
        /// <param name="option">An <see cref="IOption{T}"/>.</param>
        /// <returns>An <see cref="INonWhiteSpaceOption"/></returns>
        public static INonWhiteSpaceOption NotWhiteSpace(this IOption<string> option)
        {
            return option.Handle(NoneIfNullOrWhiteSpace, () => new VerifiedStringOption());
        }

        /// <summary>
        /// Converts <paramref name="option"/> to an <see cref="INonEmptyOption"/>.
        /// </summary>
        /// <param name="option">An <see cref="IOption{T}"/>.</param>
        /// <returns>An <see cref="INonEmptyOption"/></returns>
        public static INonEmptyOption NotEmpty(this IOption<string> option)
        {
            return option.Handle(NoneIfNullOrEmpty, () => new VerifiedStringOption());
        }

        private class VerifiedStringOption : OptionImpl<string>, INonEmptyOption, INonWhiteSpaceOption
        {
            internal VerifiedStringOption()
            {
            }

            internal VerifiedStringOption(string value) : base(value)
            {
            }
        }
    }
}