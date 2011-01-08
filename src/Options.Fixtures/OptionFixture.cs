using NUnit.Framework;

namespace Options.Fixtures
{
	[TestFixture]
	public class OptionFixture
	{
		[Test]
		[Category("Fast")]
		public void AvoidNullReturnsNoneForNull()
		{
			var actual = Option.AvoidNull((string)null);
			actual.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void AvoidNullReturnsSomeForNonNull()
		{
			var value = new object();
			var actual = Option.AvoidNull(value);
			actual.AssertSomeAnd(Is.SameAs(value));
		}

		[Test]
		[Category("Fast")]
		public void AvoidNullReturnsNoneForNullNullable()
		{
			var actual = Option.AvoidNull((int?)null);
			actual.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void AvoidNullReturnsSomeForNonNullNullable([Random(1)] double random)
		{
			var actual = Option.AvoidNull((double?)random);
			actual.AssertSomeAnd(Is.EqualTo(random));
		}

		[Test]
		[Category("Fast")]
		public void NoneReturnsNone()
		{
			Option.None<int>().AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void GuardProtectsAgainstNulls()
		{
			var guarded = Option.Guard(ReturnsNull);
			guarded().AssertNone();
		}

		public static string ReturnsNull()
		{
			return null;
		}
	}
}