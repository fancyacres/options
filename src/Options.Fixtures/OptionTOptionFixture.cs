using System;
using System.Diagnostics;
using System.Linq;

using NUnit.Framework;

namespace Options.Fixtures
{
	[TestFixture]
	public class OptionTOptionFixture
	{
		[Test]
		[Category("Fast")]
		public void ConstructorWithValueContainsValue([Random(int.MinValue, int.MaxValue, 1)] int random)
		{
			var actual = new Option<int>(random);
			actual.AssertSomeAnd(Is.EqualTo(random));
		}

		[Test]
		[Category("Fast")]
		public void CreatedWithNullHasNoValue()
		{
			string nullString = null;
			var actual = new Option<string>(nullString);
			actual.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void DefaultConstructorContainsNoValue()
		{
			var actual = new Option<int>();
			actual.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void GetHashCodeWorksWithInternalNull()
		{
			string nullString = null;
			new Option<string>(nullString).GetHashCode();
		}

		[Test]
		[Category("Fast")]
		public void HatesNulls()
		{
			Assert.That(() => { new Option<int>().Handle(null, () => true); },
			            Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("ifSome"));
			Assert.That(() => { new Option<int>().Handle(v => true, null); },
			            Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("ifNone"));
		}

		[Test]
		[Category("Fast")]
		public void TwoNoneOptionsAreEqual()
		{
			var actual = new Option<int>();
			var expected = new Option<int>();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		[Category("Fast")]
		public void TwoNoneOptionsHaveSameHashCode()
		{
			var actual = new Option<int>().GetHashCode();
			var expected = new Option<int>().GetHashCode();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		[Category("Fast")]
		public void TwoOptionsWithDifferentValuesAreNotEqual([Random(int.MinValue + 1, int.MaxValue, 1)] int random)
		{
			var actual = new Option<int>(random - 1);
			var expected = new Option<int>(random);
			Assert.That(actual, Is.Not.EqualTo(expected));
		}

		[Test]
		[Category("Fast")]
		public void TwoOptionsWithSameValueAreEqual([Random(int.MinValue, int.MaxValue, 1)] int random)
		{
			var actual = new Option<int>(random);
			var expected = new Option<int>(random);
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		[Category("Fast")]
		public void TwoOptionsWithSameValueHaveSameHashCode([Random(int.MinValue, int.MaxValue, 1)] int random)
		{
			var actual = new Option<int>(random).GetHashCode();
			var expected = new Option<int>(random).GetHashCode();
			Assert.That(actual, Is.EqualTo(expected));
		}

	    [Test]
	    [Category("Fast")]
	    public void ImplicitSyntax()
	    {
	        Func<decimal, decimal, Option<Decimal>> divide = (divisor, dividend) =>
	        {
	            if (dividend != 0)
	            {
	                return divisor / dividend;
	            }
	            else
	            {
	                return Option.None();
	            }
	        };

	        divide(2, 1).AssertSomeAnd(Is.EqualTo(2));
            divide(2, 0).AssertNone();
	    }
	}
}
