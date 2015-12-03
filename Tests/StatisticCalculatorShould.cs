using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using _03_design_hw.Loaders;
using _03_design_hw.Statistics;

namespace _03_design_hw.Tests
{
    [TestFixture]
    public class StatisticCalculatorShould
    {
        private Mock<ILoader> _loader;
        private Mock<IWordsLoader> _wordsLoader;
        private Mock<IBlackListLoader> _blackListLoader;

        [SetUp]
        public void SetUp()
        {
            _loader = new Mock<ILoader>();
            _wordsLoader = new Mock<IWordsLoader>();
            _blackListLoader = new Mock<IBlackListLoader>();
            _blackListLoader.Setup(x => x.BlackList).Returns(new HashSet<string>());
            _loader.Setup(x => x.TagsCount).Returns(3);
            _loader.Setup(x => x.Random).Returns(new Random());
            _wordsLoader.Setup(x => x.Words).Returns(new List<string> { "a", "b", "b", "c", "c", "c", });
        }
        [Test]
        public void ReturnCorrectWordsCount()
        {
            var statistic = new StatisticCalculator(_loader.Object, _blackListLoader.Object).Calculate(_wordsLoader.Object.Words);
            var words = statistic.WordsWithFrequency;
            Assert.AreEqual(3, words.Count);
        }

        [Test]
        public void ReturnsCorrectWordCount_WithTopConstraint()
        {
            _loader.Setup(x => x.TagsCount).Returns(2);
            var statistic = new StatisticCalculator(_loader.Object, _blackListLoader.Object).Calculate(_wordsLoader.Object.Words);
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
                var statistic = new StatisticCalculator(_loader.Object, _blackListLoader.Object).Calculate(inputWords);
                results.Add(statistic.WordsWithFrequency[0].Frequency);
            }
            CollectionAssert.AreEqual(Enumerable.Range(1, size).ToList(), results);
        }
        [Test]
        public void CorrectlyCalculate_MinWordCount()
        {
            var statistic = new StatisticCalculator(_loader.Object, _blackListLoader.Object).Calculate(_wordsLoader.Object.Words);
            Assert.AreEqual(1, statistic.MinCount);
        }
        [Test]
        public void CorrectlyCalculate_MaxWordCount()
        {
            var statistic = new StatisticCalculator(_loader.Object, _blackListLoader.Object).Calculate(_wordsLoader.Object.Words);
            Assert.AreEqual(3, statistic.MaxCount);
        }
    }
}
