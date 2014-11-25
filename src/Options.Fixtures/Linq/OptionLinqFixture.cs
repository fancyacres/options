using NUnit.Framework;
using Options.Fixtures;

namespace Options.Linq
{
    [TestFixture]
    public class OptionLinqFixture
    {
        [Test]
        public void Where_None_WhenFalse()
        {
            var actual =
                from i in Option.Create(5)
                where i > 5
                select i;
            actual.AssertNone();
        }

        [Test]
        public void Where_Some_WhenTrue()
        {
            const int expected = 6;
            var actual =
                from i in Option.Create(expected)
                where i > 5
                select i;
            actual.AssertSomeAnd(Is.EqualTo(expected));
        }

        [Test]
        public void SelectMany_None_WhenFirstNone()
        {
            var actual =
                from i in Option.Create<int>()
                from j in Option.Create(5)
                select i + j;
            actual.AssertNone();
        }

        [Test]
        public void SelectMany_None_WhenSecondNone()
        {
            var actual =
                from i in Option.Create(5)
                from j in Option.Create<int>()
                select i + j;
            actual.AssertNone();
        }

        [Test]
        public void SelectMany_Some_WhenBothSome()
        {
            const int first = 5;
            const int second = 6;
            const int expected = first + second;

            var actual =
                from i in Option.Create(first)
                from j in Option.Create(second)
                select i + j;

            actual.AssertSomeAnd(Is.EqualTo(expected));
        }

        [Test]
        public void SelectMany_Simple_None_WhenFirstNone()
        {
            var actual =
                Option.Create<int>().SelectMany(i => Option.Create(5));
            actual.AssertNone();
        }

        [Test]
        public void SelectMany_Simple_None_WhenSecondNone()
        {
            var actual =
                Option.Create(5).SelectMany(i => Option.Create<int>());
            actual.AssertNone();
        }

        [Test]
        public void SelectMany_Simple_Some_WhenBothSome()
        {
            const int first = 5;
            const int second = 6;
            const int expected = first + second;

            var actual =
                Option.Create(first).SelectMany(i => Option.Create(i + second));

            actual.AssertSomeAnd(Is.EqualTo(expected));
        }

        [Test]
        public void Select_None_WhenNone()
        {
            var actual =
                from i in Option.Create<int>()
                select i;
            actual.AssertNone();
        }

        [Test]
        public void Select_Some_WhenSome()
        {
            const int expected = 1;
            var actual =
                from i in Option.Create(expected)
                select i;
            actual.AssertSomeAnd(Is.EqualTo(expected));
        }
    }
}