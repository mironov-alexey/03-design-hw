using System.IO;
using NUnit.Framework;
using _03_design_hw.Loaders;

namespace _03_design_hw.Tests
{
    [TestFixture]
    public class WordsLoaderShould
    {
        [SetUp]
        public void SetUp()
        {
            _options = new Options {PathToWords = TestFileName};
            _wordsLoader = new WordsLoader(_options);
        }

        private const string TestFileName = "loader_test.txt";
        private WordsLoader _wordsLoader;
        private Options _options;

        [Test]
        public void LoadWordsFromFile()
        {
            File.WriteAllLines(TestFileName, new[] {"a", "b", "c", "d"});
            var actualWords = _wordsLoader.Words;
            CollectionAssert.AreEqual(actualWords, new[] {"a", "b", "c", "d"});
        }

        [Test]
        public void ThrowException_IfFileNotFound()
        {
            _options = new Options {PathToWords = "doesn'texist.txt"};
            _wordsLoader = new WordsLoader(_options);
            Assert.Throws<FileNotFoundException>(() => { var words = _wordsLoader.Words; });
        }
    }
}