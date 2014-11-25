using System;
using NUnit.Framework;
using Options.Core;
using Options.Fixtures;

namespace Options.Linq
{
    [TestFixture(Description = "Tests the various IOption<T> utility functions in Options.Linq.Option.")]
    public class UtilityFixture
    {
        [Test]
        public void GetValueOrDefault_None_NoParameter_ReturnsTypeDefault()
        {
            const int expectedInt = default(int);
            var actualInt = Option.Create<int>().GetValueOrDefault();
            Assert.That(actualInt, Is.EqualTo(expectedInt));

            var expectedObj = default(object);
            var actualObj = Option.Create<object>().GetValueOrDefault();
            Assert.That(actualObj, Is.EqualTo(expectedObj));
        }

        [Test]
        public void GetValueOrDefault_None_ExecutesSelector()
        {
            var expected = new Object();
            var actual = Option.Create<object>().GetValueOrDefault(() => expected);
            Assert.That(actual, Is.SameAs(expected));
        }

        [Test]
        public void Act_CallsCorrectAction()
        {
            var somethingCounter = EvaluationCounter.Unit();
            var nothingCounter = CallCounter.Unit();

            Option.Create(Unit.Default).Act(somethingCounter.Counted, nothingCounter.Counted);
            somethingCounter.AssertOnce();
            nothingCounter.AssertNever();
            somethingCounter.Reset();
            nothingCounter.Reset();

            Option.Create<Unit>().Act(somethingCounter.Counted, nothingCounter.Counted);
            somethingCounter.AssertNever();
            nothingCounter.AssertOnce();
            somethingCounter.Reset();
            nothingCounter.Reset();

            Option.Create(Unit.Default).Act(somethingCounter.Counted);
            somethingCounter.AssertOnce();
            somethingCounter.Reset();

            Option.Create<Unit>().Act(somethingCounter.Counted);
            somethingCounter.AssertNever();

            Option.Create(Unit.Default).Act(nothingCounter.Counted);
            nothingCounter.AssertNever();

            Option.Create<Unit>().Act(nothingCounter.Counted);
            nothingCounter.AssertOnce();
            nothingCounter.Reset();

            Option.Create(Unit.Default).Act(somethingCounter.CountedAction, nothingCounter.CountedAction);
            somethingCounter.AssertOnce();
            nothingCounter.AssertNever();
            somethingCounter.Reset();
            nothingCounter.Reset();

            Option.Create<Unit>().Act(somethingCounter.CountedAction, nothingCounter.CountedAction);
            somethingCounter.AssertNever();
            nothingCounter.AssertOnce();
            somethingCounter.Reset();
            nothingCounter.Reset();

            Option.Create(Unit.Default).Act(somethingCounter.CountedAction);
            somethingCounter.AssertOnce();
            somethingCounter.Reset();

            Option.Create<Unit>().Act(somethingCounter.CountedAction);
            somethingCounter.AssertNever();

            Option.Create(Unit.Default).Act(nothingCounter.CountedAction);
            nothingCounter.AssertNever();

            Option.Create<Unit>().Act(nothingCounter.CountedAction);
            nothingCounter.AssertOnce();
            nothingCounter.Reset();
        }

        [Test]
        public void SafeReturnsNotNullFromNull()
        {
            var actual = ((IOption<object>)null).Safe();
            Assert.That(actual, Is.Not.Null);
        }

        [Test]
        public void SafeFromNullEvaluatesToNone()
        {
            var actual = ((IOption<object>)null).Safe();
            actual.AssertNone();
        }
    }
}