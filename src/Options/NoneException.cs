using System;
using System.Runtime.Serialization;

namespace Options
{
	[Serializable]
	public class NoneException : Exception
	{
		public NoneException() : base("Option does not contain a value.") {}

		public NoneException(string message) : base(message) {}

		public NoneException(string message, Exception innerException) : base(message, innerException) {}

		protected NoneException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
}