using NUnit.Framework;
using Options.Core;

namespace Options.Linq
{
    [TestFixture]
    public class PublicationFixture
    {
        [Test]
        public void PublishedOnlyEvaluatedOnce()
        {
            var counter = CallCounter.Unit();
            var published = Option.Generate(counter.Counted).Publish();
            published.Evaluate();
            published.Evaluate();
            counter.AssertOnce();
        }

        [Test]
        public void PublicationDoesNotForceEvaluation()
        {
            var counter = CallCounter.Unit();
            Option.Generate(counter.Counted).Publish();
            counter.AssertNever();
        }

        [Test]
        public void PublicationOnlyEvaluatedOnceEvenWhenWrappedInGenerated()
        {
            var publishedCounter = CallCounter.Unit();
            var generatedCounter = CallCounter.Unit();
            var optionThing =
                from _ in Option.Generate(publishedCounter.Counted)
                    .Publish()
                from __ in Option.Generate(generatedCounter.Counted)
                select __;
            optionThing.Evaluate();
            optionThing.Evaluate();
            publishedCounter.AssertOnce();
            generatedCounter.AssertMultiple();
        }

        [Test]
        public void PublicationWrapsAllGenerated()
        {
            var firstCounter = CallCounter.Unit();
            var secondCounter = CallCounter.Unit();
            var optionThing =
                (from _ in Option.Generate(firstCounter.Counted)
                    from __ in Option.Generate(secondCounter.Counted)
                    select __).Publish();
            optionThing.Evaluate();
            optionThing.Evaluate();
            firstCounter.AssertOnce();
            secondCounter.AssertOnce();
        }

        [Test]
        public void PublicationDoesNotModifySourceDeferred()
        {
            var counter = CallCounter.Unit();
            var deferred = Option.Generate(counter.Counted);
            var published = deferred.Publish();
            published.Evaluate();
            published.Evaluate();
            deferred.Evaluate();
            deferred.Evaluate();

            // Once for the two calls to published.Evaluate and twice for the
            // two calls to deferred.Evaluate
            counter.AssertMultiple(3);
        }

        [Test]
        public void AsOptionDoesNotForceEvaluation()
        {
            var counter = CallCounter.Unit();
            Option.Generate(counter.Counted).Publish().AsOption();
            counter.AssertNever();
        }

        [Test]
        public void AsDeferredDoesNotUndoPublication()
        {
            var counter = CallCounter.Unit();
            var deferred = Option.Generate(counter.Counted).Publish().AsDeferred();
            deferred.Evaluate();
            deferred.Evaluate();
            counter.AssertOnce();
        }

        [Test]
        public void HasEvaluatedIsCorrect()
        {
            var published = Option.Constant(Unit.Default).Publish();
            Assert.That(published.HasEvaluated, Is.False);
            published.Evaluate();
            Assert.That(published.HasEvaluated, Is.True);
        }
    }
}