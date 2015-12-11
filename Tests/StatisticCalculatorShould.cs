using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using _03_design_hw.CloudGenerator;
using _03_design_hw.Loaders;
using _03_design_hw.Statistics;

namespace _03_design_hw.Tests
{
    [TestFixture]
    public class StatisticCalculatorShould
    {
        [SetUp]
        public void SetUp()
        {
            _settings = new Settings
            {
                TagsCount = 3
            };
            _wordsLoader = new Mock<IWordsLoader>();
            _blackListFilter = new Mock<IWordsFilter>();
            _blackListFilter.Setup(x => x.Filter(It.IsAny<string>())).Returns(true);
            _wordsLoader.Setup(x => x.Words).Returns(new List<string> {"a", "b", "b", "c", "c", "c"});

        }

        private Settings _settings;
        private Mock<IWordsLoader> _wordsLoader;
        private Mock<IWordsFilter> _blackListFilter;

        [Test]
        public void CorrectlyCalculate_MaxWordCount()
        {
            var statistic =
                new StatisticCalculator(_settings, _blackListFilter.Object).Calculate(_wordsLoader.Object.Words);
            Assert.AreEqual(3, statistic.MaxCount);
        }

        [Test]
        public void CorrectlyCalculate_MinWordCount()
        {
            var statistic =
                new StatisticCalculator(_settings, _blackListFilter.Object).Calculate(_wordsLoader.Object.Words);
            Assert.AreEqual(1, statistic.MinCount);
        }

        [Test]
        public void CorrectlyCalculate_WordFrequency()
        {
            var actualFrequencies = new List<int>();
            var size = 1000;
            for (var i = 1; i <= size; i++)
            {
                var inputWords = string.Join(" ", Enumerable.Range(0, i).Select(_ => "a")).Split().ToList();
                var statistic = new StatisticCalculator(_settings, _blackListFilter.Object).Calculate(inputWords);
                actualFrequencies.Add(statistic.WordsWithFrequency[0].Frequency);
            }
            var expectedFrequencies = Enumerable.Range(1, size).ToList();
            CollectionAssert.AreEqual(expectedFrequencies, actualFrequencies);
        }

        [Test]
        public void ReturnCorrectWordsCount()
        {
            var statistic =
                new StatisticCalculator(_settings, _blackListFilter.Object).Calculate(_wordsLoader.Object.Words);
            var words = statistic.WordsWithFrequency;
            Assert.AreEqual(3, words.Count);
        }

        [Test]
        public void ReturnsCorrectWordCount_WithTopConstraint()
        {
            _settings.TagsCount = 2;
            var statistic =
                new StatisticCalculator(_settings, _blackListFilter.Object).Calculate(_wordsLoader.Object.Words);
            var words = statistic.WordsWithFrequency;
            Assert.AreEqual(2, words.Count);
        }
    }
}