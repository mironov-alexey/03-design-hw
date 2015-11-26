using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
namespace _03_design_hw
{
    [TestFixture]
    public class StatisticShould
    {
        private Mock<DictionaryLoader> _loader;
        private Mock<TagCloudSettings> _settings;

        [SetUp]
        public void SetUp()
        {
            _loader = new Mock<DictionaryLoader>("path");
            _settings = new Mock<TagCloudSettings>(_loader.Object);
            _settings.Setup(x => x.BlackList).Returns(new HashSet<string>());
            _settings.Setup(x => x.Top).Returns(3);
            _settings.Setup(x => x.Random).Returns(new Random());
        }
        [Test]
        public void ReturnCorrectWordsCount()
        {
            var statistic = new Statistic(_settings.Object, new List<string> {"a", "b", "b", "c", "c", "c", });
            var words = statistic.WordsWithFrequency;
            Assert.AreEqual(3, words.Count);
        }

        [Test]
        public void ReturnsCorrectWordCount_WithTopConstraint()
        {
            _settings.Setup(x => x.Top).Returns(2);
            var statistic = new Statistic(_settings.Object, new List<string> { "a", "b", "b", "c", "c", "c", });
            var words = statistic.WordsWithFrequency;
            Assert.AreEqual(2, words.Count);
        }
        [Test]
        public void CorrectlyCalculate_WordFrequency()
        {
            var results = new List<int>();
            var size = 1000;
            for (var i = 1; i <= size; i++)
            {
                var inputWords = string.Join(" ", Enumerable.Range(0, i).Select(_ => "a")).Split().ToList();
                var statistic = new Statistic(_settings.Object, inputWords);
                results.Add(statistic.WordsWithFrequency[0].Frequency);
            }
            CollectionAssert.AreEqual(Enumerable.Range(1, size).ToList(), results);
        }
        [Test]
        public void CorrectlyCalculate_MinWordCount()
        {
            var statistic = new Statistic(_settings.Object, new List<string> { "a", "b", "b", "c", "c", "c", });
            Assert.AreEqual(1, statistic.MinCount);
        }
        [Test]
        public void CorrectlyCalculate_MaxWordCount()
        {
            var statistic = new Statistic(_settings.Object, new List<string> { "a", "b", "b", "c", "c", "c", });
            Assert.AreEqual(3, statistic.MaxCount);
        }
    }
}
