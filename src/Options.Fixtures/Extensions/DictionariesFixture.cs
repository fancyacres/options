using System;
using System.Collections.Generic;
using NUnit.Framework;
using Options.Fixtures;

namespace Options.Extensions
{
    [TestFixture]
    public class DictionariesFixture
    {
        [Test]
        public void OptionGetValueNoneWhenNotPresent()
        {
            var dictionary = new Dictionary<string, int>();
            dictionary.OptionGetValue("Nobody's home").AssertNone();
        }

        [Test]
        public void OptionGetValueNoneWhenNull()
        {
            const string key = "Nobody's home";
            var dictionary = new Dictionary<string, object>()
            {
                { key, null }
            };
            dictionary.OptionGetValue(key).AssertNone();
        }

        [Test]
        public void OptionGetValueNoneWhenNullableNoValue()
        {
            const string key = "Nobody's home";
            var dictionary = new Dictionary<string, int?>()
            {
                { key, null }
            };
            dictionary.OptionGetValue(key).AssertNone();
        }

        [Test]
        public void OptionGetValueSomeWhenNotNull()
        {
            const string key = "Nobody's home";
            var expected = new Object();
            var dictionary = new Dictionary<string, object>()
            {
                { key, expected }
            };
            dictionary.OptionGetValue(key).AssertSomeAnd(Is.SameAs(expected));
        }

        [Test]
        public void OptionGetValueSomeWhenNullableWithValue()
        {
            const string key = "Nobody's home";
            const int expected = 1;
            var dictionary = new Dictionary<string, int?>()
            {
                { key, expected }
            };
            dictionary.OptionGetValue(key).AssertSomeAnd(Is.EqualTo(expected));
        }
    }
}