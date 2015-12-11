using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using _03_design_hw.CloudGenerator;
using _03_design_hw.Loaders;

namespace _03_design_hw.Tests
{
    [TestFixture]
    public class WordExtenstionsTest
    {
        [Test]
        public void FilterBannedWords()
        {
            var words = new List<string>
            {
                "a",
                "b",
                "c",
                "d",
                "e",
                "d",
                "e",
                "d",
                "e",
                "b",
                "c",
                "b",
                "c",
                "a",
                "b",
                "f"
            };
            var wordsFilter = new Mock<IWordsFilter>();
            wordsFilter.Setup(x => x.Filter(It.IsNotIn("b", "c", "d"))).Returns(true);
            var filteredWords = words.FilterBannedWords(wordsFilter.Object);
            CollectionAssert.AreEquivalent(new List<string> {"a", "e", "e", "e", "a", "f"}, filteredWords);
        }
    }
}