using System;
using System.Threading;
using NUnit.Framework;
using Options.Core;

namespace Options
{
    public static class EvaluationCounter
    {
        public static EvaluationCounter<Unit, Unit> Unit()
        {
            return FromAction<Unit>(_ => { });
        }
 
        public static EvaluationCounter<T, Unit> FromAction<T>(Action<T> action)
        {
            return new EvaluationCounter<T, Unit>(t =>
            {
                action(t);
                return Core.Unit.Default;
            });
        }
    }

    public class EvaluationCounter<T1,T2>
    {
        private readonly Func<T1,T2> _counted;
        private int _evaluatedCount;

        public EvaluationCounter(Func<T1,T2> counted)
        {
            if (ReferenceEquals(counted, null))
            {
                throw new ArgumentNullException("counted");
            }
            _counted = t1 =>
            {
                Interlocked.Increment(ref _evaluatedCount);
                return counted(t1);
            };
        }

        public Func<T1, T2> Counted
        {
            get { return _counted; }
        }

        public Action<T1> CountedAction
        {
            get { return t1 => Counted(t1); }
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
            _evaluatedCount = 0;
        }
    }
}