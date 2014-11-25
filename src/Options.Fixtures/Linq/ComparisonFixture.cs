using System.Collections.Generic;
using NUnit.Framework;
using Options.Core;

namespace Options.Linq
{
    [TestFixture]
    public class ComparisonFixture
    {
        [Test]
        [TestCaseSource("NaturallyEquatableCases")]
        public bool NaturallyEquatable(IOption<int> left, IOption<int> right)
        {
            return left.ToEquatable().Equals(right);
        }

        [Test]
        [TestCaseSource("NaturallyComparableCases")]
        public int NaturallyComparable(IOption<int> left, IOption<int> right)
        {
            return left.ToComparable().CompareTo(right);
        }

        [Test]
        [TestCaseSource("EqualityComparerProvidedCases")]
        public bool EqualityComparerProvided(IOption<int> left, IOption<int> right, IEqualityComparer<int> comparer)
        {
            return left.ToEquatable(comparer).Equals(right);
        }

        [Test]
        [TestCaseSource("ComparerProvidedCases")]
        public int ComparerProvided(IOption<int> left, IOption<int> right, IComparer<int> comparer)
        {
            return left.ToComparable(comparer).CompareTo(right);
        }

        private static IEnumerable<TestCaseData> NaturallyEquatableCases()
        {
            return new[]
            {
                new TestCaseData(Linq.Option.Create<int>(), Linq.Option.Create<int>())
                    .SetName("Both None")
                    .Returns(true),
                new TestCaseData(Linq.Option.Create(1), Linq.Option.Create<int>())
                    .SetName("Right None")
                    .Returns(false),
                new TestCaseData(Linq.Option.Create<int>(), Linq.Option.Create(1))
                    .SetName("Left None")
                    .Returns(false),
                new TestCaseData(Linq.Option.Create(1), Linq.Option.Create(0))
                    .SetName("Both Some Not Equal")
                    .Returns(false),
                new TestCaseData(Linq.Option.Create(1), Linq.Option.Create(1))
                    .SetName("Both Some Equal")
                    .Returns(true)
            };
        }

        private static IEnumerable<TestCaseData> NaturallyComparableCases()
        {
            return new[]
            {
                new TestCaseData(Linq.Option.Create<int>(), Linq.Option.Create<int>())
                    .SetName("Both None")
                    .Returns(0),
                new TestCaseData(Linq.Option.Create(1), Linq.Option.Create<int>())
                    .SetName("Right None")
                    .Returns(1),
                new TestCaseData(Linq.Option.Create<int>(), Linq.Option.Create(1))
                    .SetName("Left None")
                    .Returns(-1),
                new TestCaseData(Linq.Option.Create(0), Linq.Option.Create(1))
                    .SetName("Both Some Less Than")
                    .Returns(-1),
                new TestCaseData(Linq.Option.Create(1), Linq.Option.Create(0))
                    .SetName("Both Some Greater Than")
                    .Returns(1),
                new TestCaseData(Linq.Option.Create(1), Linq.Option.Create(1))
                    .SetName("Both Some Equal")
                    .Returns(0)
            };
        }

        private static IEnumerable<TestCaseData> EqualityComparerProvidedCases()
        {
            var comparer = new AllIntsAreCreatedEqualComparer();
            return new[]
            {
                new TestCaseData(Linq.Option.Create<int>(), Linq.Option.Create<int>(), comparer)
                    .SetName("Both None")
                    .Returns(true),
                new TestCaseData(Linq.Option.Create(1), Linq.Option.Create<int>(), comparer)
                    .SetName("Right None")
                    .Returns(false),
                new TestCaseData(Linq.Option.Create<int>(), Linq.Option.Create(1), comparer)
                    .SetName("Left None")
                    .Returns(false),
                new TestCaseData(Linq.Option.Create(1), Linq.Option.Create(0), comparer)
                    .SetName("Both Some Not Equal")
                    .Returns(true),
                new TestCaseData(Linq.Option.Create(1), Linq.Option.Create(1), comparer)
                    .SetName("Both Some Equal")
                    .Returns(true)
            };
        }

        private static IEnumerable<TestCaseData> ComparerProvidedCases()
        {
            var comparer = new ReverseIntComparer();
            return new[]
            {
                new TestCaseData(Linq.Option.Create<int>(), Linq.Option.Create<int>(), comparer)
                    .SetName("Both None")
                    .Returns(0),
                new TestCaseData(Linq.Option.Create(1), Linq.Option.Create<int>(), comparer)
                    .SetName("Right None")
                    .Returns(1),
                new TestCaseData(Linq.Option.Create<int>(), Linq.Option.Create(1), comparer)
                    .SetName("Left None")
                    .Returns(-1),
                new TestCaseData(Linq.Option.Create(0), Linq.Option.Create(1), comparer)
                    .SetName("Both Some Less Than")
                    .Returns(1),
                new TestCaseData(Linq.Option.Create(1), Linq.Option.Create(0), comparer)
                    .SetName("Both Some Greater Than")
                    .Returns(-1),
                new TestCaseData(Linq.Option.Create(1), Linq.Option.Create(1), comparer)
                    .SetName("Both Some Equal")
                    .Returns(0)
            };
        }

        private class AllIntsAreCreatedEqualComparer: IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return true;
            }

            public int GetHashCode(int obj)
            {
                return 1;
            }
        }

        private class ReverseIntComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return y.CompareTo(x);
            }
        }
    }
}