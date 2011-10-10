using System;
using Microsoft.FSharp.Core;

using NUnit.Framework;

namespace Options.Fixtures
{
	[TestFixture]
	public class OptionExtensionsFixture
	{
		[Test]
		[Category("Fast")]
		public void AsNullableReturnsNullForNone()
		{
			var option = new Option<int>();
			var actual = option.AsNullable();
			Assert.That(actual, Is.Null);
		}

		[Test]
		[Category("Fast")]
		public void AsNullableReturnsValueForSome([Random(0, int.MaxValue, 1)] int value)
		{
			var option = new Option<int>(value);
			var actual = option.AsNullable();
			Assert.That(actual, Is.EqualTo(value));
		}

		[Test]
		[Category("Fast")]
		public void AsUnprotectedReturnsNullForNone()
		{
			var option = new Option<string>();
			var actual = option.AsUnprotected();
			Assert.That(actual, Is.Null);
		}

		[Test]
		[Category("Fast")]
		public void AsUnprotectedReturnsValueForSome()
		{
			const string expected = "Willis";
			var option = new Option<string>(expected);
			var actual = option.AsUnprotected();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		[Category("Fast")]
		public void CoalesceReturnsFirstSome()
		{
			var options = new[]
			              {
			              	new Option<int>(),
			              	new Option<int>(1),
			              };

			var actual = options.Coalesce();
			actual.AssertSomeAnd(Is.EqualTo(1));
		}

		[Test]
		[Category("Fast")]
		public void CoalesceReturnsNoneIfAllNone()
		{
			var options = new[]
			              {
			              	new Option<int>(),
			              	new Option<int>(),
			              };

			var actual = options.Coalesce();
			actual.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void GetValueOrDefaultReturnsDefaultIfNone([Random(0, int.MaxValue, 1)] int defaultValue)
		{
			var option = new Option<int>();
			option.AssertNone();
			var actual = option.GetValueOrDefault(defaultValue);
			Assert.That(actual, Is.EqualTo(defaultValue));
		}

		[Test]
		[Category("Fast")]
		public void GetValueOrDefaultReturnsValueIfSome([Random(int.MinValue, -1, 1)] int value,
		                                                [Random(0, int.MaxValue, 1)] int defaultValue)
		{
			var option = new Option<int>(value);
			var actual = option.GetValueOrDefault(defaultValue);
			Assert.That(actual, Is.EqualTo(value));
		}

		[Test]
		[Category("Fast")]
		public void GetValueOrThrowThrowsException()
		{
			var option = new Option<int>();
			Assert.That(() => { option.GetValueOrThrow(() => new InvalidOperationException("Willis")); },
			            Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("Willis"));
		}

		[Test]
		[Category("Fast")]
		public void GetValueOrThrowThrowsNoneException()
		{
			var option = new Option<int>();
			Assert.That(() => { option.GetValueOrThrow(); },
			            Throws.TypeOf<NoneException>());
		}

		[Test]
		[Category("Fast")]
		public void IntersectReturnsNoneIfEitherNone()
		{
			var firstNone = new Option<int>().Intersect(new Option<int>(1));
			firstNone.AssertNone();
			var secondNone = new Option<int>(1).Intersect(new Option<int>());
			secondNone.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void IntersectReturnsSomeIfBothSome()
		{
			var actual = new Option<int>(1).Intersect(new Option<int>(1));
			actual.AssertSomeAnd(Is.EqualTo(Tuple.Create(1, 1)));
		}

		[Test]
		[Category("Fast")]
		public void LiftReturnsNoneIfNone()
		{
			var actual = new Option<int>().SelectMany(i => new Option<int>(i));
			actual.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void LiftReturnsSelectedOption()
		{
			var actual = from i in new Option<int>(1)
			             from s in Option.Create(i.ToString())
			             select s;
			actual.AssertSomeAnd(Is.EqualTo(1.ToString()));
		}

		[Test]
		[Category("Fast")]
		public void TransformReturnsNoneIfFuncReturnsNull()
		{
			var actual = new Option<int>(1).Select(i => (string)null);
			actual.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void TransformToleratesNull()
		{
			var actual = new Option<int>().Select<int, string>(null);
			actual.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void TransformTransformsNone()
		{
			var actual = new Option<int>().Select(v => v == 1);
			actual.AssertNone();
			Assert.That(actual, Is.TypeOf<Option<bool>>());
		}

		[Test]
		[Category("Fast")]
		public void TransformTransformsSome()
		{
			var actual = from v in new Option<int>(1)
			             select v == 1;
			actual.AssertSomeAnd(Is.True);
		}

		[Test]
		[Category("Fast")]
		public void FSharpConversion([Random(int.MinValue, int.MaxValue, 1)] int random)
		{
			var ourSome = new Option<int>(random);
			var fSharpSome = ourSome.ToFSharp();
			Assert.That(FSharpOption<int>.get_IsSome(fSharpSome), "F# option should be some");
			Assert.That(fSharpSome.Value, Is.EqualTo(random));

			var ourNone = Option.Create<int>();
			var fSharpNone = ourNone.ToFSharp();
			Assert.That(FSharpOption<int>.get_IsNone(fSharpNone), "F# option should be None");
		}

		[Test]
		[Category("Fast")]
		public void ActOneArgumentCallsIfSomeWithValueIfOptionHasValue()
		{
			var expected = 5;
			var target = Option.Create(expected);
			target.Act(actual => Assert.That(actual, Is.EqualTo(expected)));
		}

		[Test]
		[Category("Fast")]
		public void ActOneArgumentDoesNotCallIfSomeWithValueIfOptionHasNoValue()
		{
			var target = Option.Create<int>();
			target.Act(actual => Assert.Fail("ifSome called. value: " + actual));
		}

		[Test]
		[Category("Fast")]
		public void ActTwoArgumentsCallsIfSomeWithValueIfOptionHasValue()
		{
			var expected = 5;
			var target = Option.Create(expected);
			target.Act(actual => Assert.That(actual, Is.EqualTo(expected)), () => {});
		}

		[Test]
		[Category("Fast")]
		public void ActTwoArgumentsDoesNotCallIfNoneIfOptionHasValue()
		{
			var target = Option.Create(5);
			target.Act(actual => { }, () => Assert.Fail("ifNone called."));
		}

		[Test]
		[Category("Fast")]
		public void ActTwoArgumentsCallsIfNoneIfOptionHasNoValue()
		{
			var ifNoneCalled = false;
			var target = Option.Create<int>();
			target.Act(actual => { }, () => { ifNoneCalled = true; });
			Assert.That(ifNoneCalled);
		}

		[Test]
		[Category("Fast")]
		public void ActTwoArgumentsDoesNotCallIfSomeIfOptionHasNoValue()
		{
			var target = Option.Create<int>();
			target.Act(actual => Assert.Fail("ifSome called. value: " + actual), () => {});
		}

		[Test]
		[Category("Fast")]
		public void NoneIfNullOrWhitespaceReturnsNoneIfGivenNull()
		{
			string target = null;
			target.NoneIfNullOrWhiteSpace().AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void NoneIfNullOrWhitespaceReturnsNoneIfGivenEmptyString()
		{
			var target = "";
			target.NoneIfNullOrWhiteSpace().AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void NoneIfNullOrWhitespaceReturnsNoneIfGivenWhitespace()
		{
			var target = " \r\n\t";
			target.NoneIfNullOrWhiteSpace().AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void NoneIfNullOrWhitespaceReturnsValueIfGivenContent()
		{
			var target = "a";
			target.NoneIfNullOrWhiteSpace().AssertSomeAnd(Is.EqualTo("a"));
		}

		[Test]
		[Category("Fast")]
		public void WhereFiltersFalseReturnFromPredicate()
		{
			var nine = Option.Create(9).Where(i => i > 9);
			nine.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void WherePassesThroughTrueReturnFromPredicate()
		{
			var nine = Option.Create(9).Where(i => i >= 9);
			nine.AssertSomeAnd(Is.EqualTo(9));
		}

		[Test]
		[Category("LINQ")]
		public void LinqSyntax()
		{
			// Simple select
			(from i in Option.Create(10) select i + 1).AssertSomeAnd(Is.EqualTo(11));
			(from i in Option.Create<int>() select i + 1).AssertNone();

			// Select many
			(from first in Option.Create(1) from second in Option.Create(2) select second > first).AssertSomeAnd(Is.True);
			(from first in Option.Create<int>() from second in Option.Create(2) select second > first).AssertNone();
			(from first in Option.Create(1) from second in Option.Create<int>() select second > first).AssertNone();

			// Where
			(from i in Option.Create(10) where i > 9 select i).AssertSomeAnd(Is.EqualTo(10));
			(from i in Option.Create(10) where i < 9 select i + 1).AssertNone();
		}
	}
}
