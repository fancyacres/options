using NUnit.Framework;

namespace Options.Fixtures
{
	[TestFixture]
	public class ObjectExtensionsFixture
	{
		[Test]
		[Category("Fast")]
		public void NonNullValueYieldsOptionWithValue()
		{
			1.AsOption().AssertSomeAnd(Is.EqualTo(1));
			"Willis".AsOption().AssertSomeAnd(Is.EqualTo("Willis"));
		}

		[Test]
		[Category("Fast")]
		public void NullValueYieldsOptionWithNoValue()
		{
			((string)null).AsOption().AssertNone();
		}
	}
}