using System.Collections.Generic;

using NUnit.Framework;

namespace Options.Fixtures
{
    [TestFixture]
    public class DictionaryExtensions
    {
        [Test]
        public void FoundElementContainsValue()
        {
            const string key = "Whatchu tawkin' 'bout";
            const string expected = "Willis";

            var dictionary = new Dictionary<string, string>
                             {
                                 {key, expected}
                             };
            var actual = dictionary.OptionGetValue(key);
            actual.AssertSomeAnd(Is.EqualTo(expected));
        }

        [Test]
        public void NotFoundElementContainsNoValue()
        {
            const string key = "Whatchu tawkin' 'bout";
            const string value = "Willis";

            var dictionary = new Dictionary<string, string>
                             {
                                 {key, value}
                             };
            var actual = dictionary.OptionGetValue("Arnold");
            actual.AssertNone();
        }

        [Test]
        public void FoundNullElementContainsNoValue()
        {
            const string key = "Whatchu tawkin' 'bout";
            const string value = null;

            var dictionary = new Dictionary<string, string>
                             {
                                 {key, value}
                             };
            var actual = dictionary.OptionGetValue(key);
            actual.AssertNone();
        }

        [Test]
        public void FoundNullableElementContainsValue()
        {
            const string key = "Whatchu tawkin' 'bout";
            const int expected = 1;

            var dictionary = new Dictionary<string, int?>
                             {
                                 {key, expected}
                             };
            var actual = dictionary.OptionGetValue(key);
            actual.AssertSomeAnd(Is.EqualTo(expected));
        }

        [Test]
        public void NotFoundNullableElementContainsNoValue()
        {
            const string key = "Whatchu tawkin' 'bout";
            const int value = 1;

            var dictionary = new Dictionary<string, int?>
                             {
                                 {key, value}
                             };
            var actual = dictionary.OptionGetValue("Arnold");
            actual.AssertNone();
        }

        [Test]
        public void FoundNullNullableElementContainsNoValue()
        {
            const string key = "Whatchu tawkin' 'bout";
            int? value = null;

            var dictionary = new Dictionary<string, int?>
                             {
                                 // ReSharper disable once ExpressionIsAlwaysNull
                                 {key, value}
                             };
            var actual = dictionary.OptionGetValue(key);
            actual.AssertNone();
        }
    }
}