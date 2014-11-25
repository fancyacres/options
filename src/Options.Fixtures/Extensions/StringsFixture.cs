using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Options.Core;

namespace Options.Extensions
{
    [TestFixture]
    public class StringsFixture
    {
        private static readonly IDictionary<string, string> _cases =
            new Dictionary<string, string>
            {
                {"null", null},
                {"empty", ""},
                {"spaces", "   "},
                {"tabs", "\t\t"},
                {"content", "Willis, yo!"}
            };

        [Test]
        [TestCaseSource("NoneIfNullOrWhiteSpaceCases")]
        public bool NoneIfNullOrWhiteSpace(string s)
        {
            return s.NoneIfNullOrWhiteSpace().Handle(_ => true, () => false);
        }

        [Test]
        [TestCaseSource("NotWhiteSpaceCases")]
        public bool NotWhiteSpace(IOption<string> option)
        {
            return option.NotWhiteSpace().Handle(_ => true, () => false);
        }

        [Test]
        [TestCaseSource("NoneIfNullOrEmptyCases")]
        public bool NoneIfNullOrEmpty(string s)
        {
            return s.NoneIfNullOrEmpty().Handle(_ => true, () => false);
        }

        [Test]
        [TestCaseSource("NotEmptyCases")]
        public bool NotEmpty(IOption<string> option)
        {
            return option.NotEmpty().Handle(_ => true, () => false);
        }

        private static IEnumerable<TestCaseData> NoneIfNullOrWhiteSpaceCases()
        {
            return _cases.Select(kvp => new TestCaseData(kvp.Value)
                .SetName(kvp.Key)
                .Returns(!string.IsNullOrWhiteSpace(kvp.Value)));
        }

        private static IEnumerable<TestCaseData> NotWhiteSpaceCases()
        {
            return _cases.Select(kvp => new TestCaseData(Linq.Option.Create(kvp.Value))
                .SetName(kvp.Key)
                .Returns(!string.IsNullOrWhiteSpace(kvp.Value)));
        }

        private static IEnumerable<TestCaseData> NoneIfNullOrEmptyCases()
        {
            return _cases.Select(kvp => new TestCaseData(kvp.Value)
                .SetName(kvp.Key)
                .Returns(!string.IsNullOrEmpty(kvp.Value)));
        }

        private static IEnumerable<TestCaseData> NotEmptyCases()
        {
            return _cases.Select(kvp => new TestCaseData(Linq.Option.Create(kvp.Value))
                .SetName(kvp.Key)
                .Returns(!string.IsNullOrEmpty(kvp.Value)));
        }
    }
}