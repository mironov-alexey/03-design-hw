using System.Collections.Generic;
using NUnit.Framework;
using _03_design_hw.Word;

namespace _03_design_hw.Tests
{
    [TestFixture]
    public class WordExtenstionsTest
    {

        [Test]
        public void FilterBannedWords()
        {
            var words = new List<string> {"a", "b", "c", "d", "e", "d", "e", "d", "e", "b", "c", "b", "c", "a", "b", "f"};
            var bannedWords = new HashSet<string> {"b", "c", "d"};
            CollectionAssert.AreEquivalent(new List<string> {"a", "e", "e", "e", "a", "f"}, words.FilterBannedWords(bannedWords));
        }
    }
}
