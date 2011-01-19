using System;
using System.Linq;

using Microsoft.FSharp.Core;

using NUnit.Framework;

namespace Options.Fixtures
{
	[TestFixture]
	public class OptionFixture
	{
		private static string ReturnsNull()
		{
			return null;
		}

		[Test]
		[Category("Fast")]
		public void AvoidNullReturnsNoneForNull()
		{
			var actual = Option.AvoidNull((string)null);
			actual.AssertNone();
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
		public void AvoidNullReturnsSomeForNonNull()
		{
			var value = new object();
			var actual = Option.AvoidNull(value);
			actual.AssertSomeAnd(Is.SameAs(value));
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
		public void GuardProtectsAgainstNulls()
		{
			var guarded = Option.Guard(ReturnsNull);
			guarded().AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void NoneReturnsNone()
		{
			Option.None<int>().AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void FSharpConversion([Random(1)] double random)
		{
			var fSharpSome = FSharpOption<double>.Some(random);
			var ourSome = Option.FromFSharp(fSharpSome);
			ourSome.AssertSomeAnd(Is.EqualTo(random));

			var fSharpNone = FSharpOption<double>.None;
			var ourNone = Option.FromFSharp(fSharpNone);
			ourNone.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void ReferenceTypeSomeReturnsOptionWithValueIfGivenNonNullValue()
		{
			var value = new object();
			var actual = Option.Some(value);
			actual.AssertSomeAnd(Is.SameAs(value));
		}

		[Test]
		[Category("Fast")]
		public void ReferenceTypeSomeThrowsArgumentNullExceptionIfGivenNull()
		{
			object value = null;
			Assert.Throws<ArgumentNullException>(() => { Option.Some(value); });
		}

		[Test]
		[Category("Fast")]
		public void NullableValueTypeSomeReturnsOptionWithValueIfGivenNonNullValue()
		{
			int? value = 1;
			var actual = Option.Some(value);
			actual.AssertSomeAnd(Is.EqualTo(value));
		}

		[Test]
		[Category("Fast")]
		public void NullableValueTypeSomeThrowsArgumentNullExceptionIfGivenNull()
		{
			int? value = null;
			Assert.Throws<ArgumentNullException>(() => { Option.Some(value); });
		}

	}
}
