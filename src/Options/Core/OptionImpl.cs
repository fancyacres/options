using System;

namespace Options.Core
{
    internal class OptionImpl<T> : IOption<T>
    {
        private readonly bool _isValueSet;
        private readonly T _value;

        internal OptionImpl()
        {
            _isValueSet = false;
            _value = default(T);
        }

        internal OptionImpl(T value)
        {
            _isValueSet = !ReferenceEquals(value, null);
            _value = value;
        }

        public TResult Handle<TResult>(Func<T, TResult> ifValue, Func<TResult> ifNone)
        {
            if (ReferenceEquals(ifValue, null))
            {
                throw new ArgumentNullException("ifValue");
            }
            if (ReferenceEquals(ifNone, null))
            {
                throw new ArgumentNullException("ifNone");
            }
            return _isValueSet ? ifValue(_value) : ifNone();
        }
    }
}