using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Options.Fixtures
{
	[TestFixture]
	public class EnumerableExtensionsFixture
	{
		[Test]
		[Category("Fast")]
		public void HatesNulls()
		{
			Assert.That(() => { ((IEnumerable<int>)null).OptionFirst(i => true); },
			            Throws.TypeOf<ArgumentNullException>());
			Assert.That(() => { ((IEnumerable<int>)null).OptionFirst(); },
						Throws.TypeOf<ArgumentNullException>());
			Assert.That(() => { new [] { 1 }.OptionFirst(null); },
						Throws.TypeOf<ArgumentNullException>());
			Assert.That(() => { ((IEnumerable<int>)null).OptionSingle(i => true); },
						Throws.TypeOf<ArgumentNullException>());
			Assert.That(() => { ((IEnumerable<int>)null).OptionSingle(); },
						Throws.TypeOf<ArgumentNullException>());
			Assert.That(() => { new[] { 1 }.OptionSingle(null); },
						Throws.TypeOf<ArgumentNullException>());
		}

		[Test]
		[Category("Fast")]
		public void OptionFirstReturnsOptionWithNoValueIfNoMatch()
		{
			var actual = Enumerable.Range(1, 10).OptionFirst(i => i > 100);
			actual.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void OptionFirstReturnsFirstItemWithValueThatMatches()
		{
			var actual = Enumerable.Range(1, 10).OptionFirst(i => i > 5);
			actual.AssertSomeAnd(Is.EqualTo(6));
		}

		[Test]
		[Category("Fast")]
		public void OptionFirstWithNoPredicateReturnsFirstNonNullValue()
		{
			var source = new[] { null, null, "Willis", null };
			var actual = source.OptionFirst();
			actual.AssertSomeAnd(Is.EqualTo("Willis"));
		}

		[Test]
		[Category("Fast")]
		public void OptionFirstWithNoPredicateReturnsOptionWithNoValueIfAllNull()
		{
			var source = new string[] { null, null, null };
			var actual = source.OptionFirst();
			actual.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void OptionFirstWithNoPredicateReturnsOptionWithNoValueIfEmpty()
		{
			var source = new int[] {};
			var actual = source.OptionFirst();
			actual.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void OptionSingleReturnsOptionWithNoValueIfNoMatch()
		{
			var actual = Enumerable.Range(1, 10).OptionSingle(i => i > 100);
			actual.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void OptionSingleReturnsFirstItemWithValueThatMatches()
		{
			var actual = Enumerable.Range(1, 10).OptionSingle(i => i == 6);
			actual.AssertSomeAnd(Is.EqualTo(6));
		}

		[Test]
		[Category("Fast")]
		public void OptionSingleWithNoPredicateReturnsFirstNonNullValue()
		{
			var source = new[] { null, null, "Willis", null };
			var actual = source.OptionSingle();
			actual.AssertSomeAnd(Is.EqualTo("Willis"));
		}

		[Test]
		[Category("Fast")]
		public void OptionSingleWithNoPredicateReturnsOptionWithNoValueIfAllNull()
		{
			var source = new string[] { null, null, null };
			var actual = source.OptionSingle();
			actual.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void OptionSingleWithNoPredicateReturnsOptionWithNoValueIfEmpty()
		{
			var source = new int[] { };
			var actual = source.OptionFirst();
			actual.AssertNone();
		}

		[Test]
		[Category("Fast")]
		public void OptionSingleThrowsExceptionIfMoreThanOneMatch()
		{
			var source = Enumerable.Range(1, 10);
			Assert.That(() => { source.OptionSingle(i => i > 1); },
			            Throws.TypeOf<InvalidOperationException>());
		}
	}
}