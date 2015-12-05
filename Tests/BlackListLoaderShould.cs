using System.IO;
using NUnit.Framework;
using _03_design_hw.Loaders;

namespace _03_design_hw.Tests
{
    public class BlackListLoaderShould
    {
        private const string TestFileName = "loader_test.txt";
        private BlackListLoader _blackListLoader;
        private Options _options;

        [SetUp]
        public void SetUp()
        {
            _options = new Options {PathToBlackList = TestFileName};
            _blackListLoader = new BlackListLoader(_options);
        }

        [Test]
        public void LoadWordsFromFile()
        {
            File.WriteAllLines(TestFileName, new[] {"a", "a", "a", "a", "b", "b", "b", "c", "c", "c", "d"});
            var actualWords = _blackListLoader.BlackList;
            CollectionAssert.AreEqual(actualWords, new[] {"a", "b", "c", "d"});
        }
    }
}