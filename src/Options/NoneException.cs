using System;
using System.Linq;

#if !NETFX_CORE
using System.Runtime.Serialization;
#endif

namespace Options
{
	///<summary>
	///	An <see cref = "Exception" /> thrown when an <see cref = "Option{TOption}" /> value is accessed where none exists
	///</summary>
#if !NETFX_CORE
    [Serializable]
#endif
	public class NoneException : Exception
	{
		///<summary>
		///	Initializes an instance of <see cref = "NoneException" /> with a default message
		///</summary>
		public NoneException() : base("Option does not contain a value.") {}

		///<summary>
		///	Initializes an instance of <see cref = "NoneException" />
		///</summary>
		///<param name = "message">The message of the exception</param>
		// ReSharper disable UnusedMember.Global
		public NoneException(string message) : base(message) {}

		// ReSharper restore UnusedMember.Global

		/// <summary>
		/// 	Initializes an instance of <see cref = "NoneException" />
		/// </summary>
		/// <param name = "message">The message of the exception</param>
		/// <param name = "innerException">The exception that caused the <see cref = "NoneException" /> to be thrown</param>
		// ReSharper disable UnusedMember.Global
		public NoneException(string message, Exception innerException) : base(message, innerException) {}

		// ReSharper restore UnusedMember.Global

#if !NETFX_CORE
		/// <summary>
		/// 	Initializes an instance of <see cref = "NoneException" />
		/// </summary>
		/// <param name = "info"></param>
		/// <param name = "context"></param>
		protected NoneException(SerializationInfo info, StreamingContext context) : base(info, context) {}
#endif
	}
}
