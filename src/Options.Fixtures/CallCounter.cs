using System;
using System.Threading;
using NUnit.Framework;
using Options.Core;

namespace Options
{
    public static class CallCounter
    {
        public static CallCounter<Unit> Unit()
        {
            return new CallCounter<Unit>(() => Core.Unit.Default);
        } 
    }

    public class CallCounter<T>
    {
        private readonly Func<T> _counted;
        private int _evaluatedCount;

        public CallCounter(Func<T> counted)
        {
            if (ReferenceEquals(counted, null))
            {
                throw new ArgumentNullException("counted");
            }
            _counted = () =>
            {
                Interlocked.Increment(ref _evaluatedCount);
                return counted();
            };
        }

        public Func<T> Counted
        {
            get { return _counted; }
        }

        public Action CountedAction
        {
            get { return () => _counted(); }
        }

        public void AssertOnce()
        {
            Assert.That(_evaluatedCount, Is.EqualTo(1), "Function should have evaluated once.");
        }

        public void AssertNever()
        {
            Assert.That(_evaluatedCount, Is.EqualTo(0), "Function should not have evaluated.");
        }

        public void AssertMultiple(uint times)
        {
            Assert.That(_evaluatedCount, Is.EqualTo(times), "Function should have evaluated {0} times.", times);
        }

        public void AssertMultiple()
        {
            Assert.That(_evaluatedCount, Is.GreaterThan(0), "Function should have evaluated more than once.");
        }

        public void Reset()
        {
            Interlocked.Exchange(ref _evaluatedCount, 0);
        }
    }
}