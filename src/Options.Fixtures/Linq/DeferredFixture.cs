using System;
using System.Threading;
using NUnit.Framework;
using Options.Core;
using Options.Fixtures;

namespace Options.Linq
{
    [TestFixture]
    public class DeferredFixture
    {
        private static IDeferredOption<Unit> LinqItUp<T>(IDeferredOption<T> source)
        {
            // Just to execise the LINQ bindings
            return
                from _ in source
                from willis in Option.Create("Willis")
                from unit in Option.Generate(() => Unit.Default)
                where willis.Length != 0
                select unit;
        }

        [Test]
        public void GenerateDoesNotExecuteIfNotEvaluated()
        {
            var counter = CallCounter.Unit();
            Option.Generate(counter.Counted);
            counter.AssertNever();
        }

        [Test]
        public void GenerateExecutesIfEvaluated()
        {
            var counter = CallCounter.Unit();
            Option.Generate(counter.Counted).Evaluate();
            counter.AssertOnce();
        }

        [Test]
        public void GenerateExecutesLikeEveryTime()
        {
            var counter = CallCounter.Unit();
            var deferred = Option.Generate(counter.Counted);
            deferred.Evaluate();
            deferred.Evaluate();
            counter.AssertMultiple(2);
        }

        [Test]
        public void DoExecutesEveryTime()
        {
            var marker = 0;
            // Make a source deferred that sometimes generates a value and sometimes doesn't.
            var source = Option.Generate(() =>
                (Interlocked.Increment(ref marker)%2 == 0)
                    ? Option.Create(Unit.Default)
                    : Option.Create<Unit>());

            var actual = 0;
            // Either way, we increment the counter
            var done = source.Do(_ => Interlocked.Increment(ref actual),
                () => Interlocked.Increment(ref actual));
            done.Evaluate();
            done.Evaluate();
            done.Evaluate();
            done.Evaluate();

            Assert.That(actual, Is.EqualTo(4), "Should have incremented four times.");
        }

        [Test]
        public void DoPermutations()
        {
            var valueCounter = EvaluationCounter.Unit();
            var noValueCounter = CallCounter.Unit();

            Option.Constant<Unit>()
                .Do(valueCounter.Counted, noValueCounter.Counted)
                .Do(valueCounter.Counted)
                .Do(noValueCounter.Counted)
                .Do(valueCounter.CountedAction, noValueCounter.CountedAction)
                .Do(valueCounter.CountedAction)
                .Do(noValueCounter.CountedAction)
                .Evaluate();

            valueCounter.AssertNever();
            noValueCounter.AssertMultiple(4);
            noValueCounter.Reset();

            Option.Constant(Unit.Default)
                .Do(valueCounter.Counted, noValueCounter.Counted)
                .Do(valueCounter.Counted)
                .Do(noValueCounter.Counted)
                .Do(valueCounter.CountedAction, noValueCounter.CountedAction)
                .Do(valueCounter.CountedAction)
                .Do(noValueCounter.CountedAction)
                .Evaluate();

            valueCounter.AssertMultiple(4);
            noValueCounter.AssertNever();
        }

        [Test]
        public void SelectAndSelectManyDoNotForceEvaluation()
        {
            var counter = CallCounter.Unit();
            Option.Generate(counter.Counted)
                .SelectMany(_ => Option.Generate(counter.Counted))
                .SelectMany(_ => Option.Generate(counter.Counted),
                    Tuple.Create)
                .Select(tuple => tuple.Item1);
            counter.AssertNever();
        }

        [Test]
        public void WhereDoesNotForceEvaluation()
        {
            var counter = CallCounter.Unit();
            Option.Generate(counter.Counted)
                .Where(_ => true);
            counter.AssertNever();
        }

        [Test]
        public void SelectAndSelectManyEvaluateTheProperNumberOfTimes()
        {
            var counter = CallCounter.Unit();
            Option.Generate(counter.Counted)
                .SelectMany(_ => Option.Generate(counter.Counted))
                .SelectMany(_ => Option.Generate(counter.Counted),
                    Tuple.Create)
                .Select(tuple => tuple.Item1)
                .Evaluate();
            counter.AssertMultiple(3);
        }

        [Test]
        public void WhereShortCircuitsAsExpected()
        {
            var before = CallCounter.Unit();
            var after = CallCounter.Unit();
            Option.Generate(before.Counted)
                .Where(_ => false)
                .SelectMany(_ => Option.Generate(after.Counted))
                .Evaluate();
            before.AssertOnce();
            after.AssertNever();
        }

        [Test]
        public void ConstantNullableWithValueCreatedCorrectly()
        {
            const int expected = 1;
            var actual = Option.Constant((int?) expected).Evaluate();
            actual.AssertSomeAnd(Is.EqualTo(expected));
        }

        [Test]
        public void ConstantNullableWithoutValueCreatedCorrectly()
        {
            var actual = Option.Constant((int?)null).Evaluate();
            actual.AssertNone();
        }

        [Test]
        public void SafeReturnsNotNullFromNull()
        {
            var actual = ((IDeferredOption<object>) null).Safe();
            Assert.That(actual, Is.Not.Null);
        }

        [Test]
        public void SafeFromNullEvaluatesToNone()
        {
            var actual = ((IDeferredOption<object>)null).Safe();
            actual.Evaluate().AssertNone();
        }
    }
}