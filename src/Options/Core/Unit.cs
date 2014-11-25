using System;

namespace Options.Core
{
    /// <summary>
    ///     Represents no value
    /// </summary>
    /// <remarks>
    /// Cribbed from http://rx.codeplex.com/SourceControl/changeset/view/d56e5d1076bf2d6e5eacee2b03420a32fb354bbc#Rx.NET/Source/System.Reactive.Core/Reactive/Unit.cs
    /// </remarks>
    [Serializable]
    public struct Unit : IEquatable<Unit>
    {
        private static readonly Unit _default = default(Unit);

        /// <summary>
        /// Gets the single <see cref="Unit"/> value.
        /// </summary>
        public static Unit Default
        {
            get { return _default; }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <see langword="true"/>, every time.
        /// </returns>
        /// <param name="other">A <see cref="Unit"/>.</param>
        public bool Equals(Unit other)
        {
            return true;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <remarks>The hash code of <see cref="Unit"/> is always zero.</remarks>
        public override int GetHashCode()
        {
            return 0;
        }

        // ReSharper disable UnusedParameter.Global
        /// <summary>
        ///     Compares two <see cref="Unit" />s for inequality.
        /// </summary>
        /// <param name="left">A <see cref="Unit" /></param>
        /// <param name="right">A <see cref="Unit" /></param>
        /// <returns>
        ///     <see langword="false" />
        /// </returns>
        public static bool operator ==(Unit left, Unit right)
        {
            return true;
        }

        /// <summary>
        ///     Compares two <see cref="Unit" />s for inequality.
        /// </summary>
        /// <param name="left">A <see cref="Unit" /></param>
        /// <param name="right">A <see cref="Unit" /></param>
        /// <returns>
        ///     <see langword="false" />
        /// </returns>
        public static bool operator !=(Unit left, Unit right)
        {
            return false;
        }
        // ReSharper restore UnusedParameter.Global

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is <see cref="Unit"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <param name="obj">Another object to compare to.</param>
        public override bool Equals(object obj)
        {
            return obj is Unit;
        }

        /// <summary>
        /// Returns the string representation of <see cref="Unit"/>.
        /// </summary>
        /// <returns>A <see cref="string"/></returns>
        public override string ToString()
        {
            return "()";
        }
    }
}