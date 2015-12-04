using System.Collections.Generic;

namespace _03_design_hw.Loaders
{
    public class WordsLoader : IWordsLoader
    {
        private readonly string _pathToFileWithWords;

        public WordsLoader(Options options)
        {
            _pathToFileWithWords = options.PathToWords;
        }

        public IEnumerable<string> Words =>
            WordsListLoader.LoadFromFile(_pathToFileWithWords);
    }
}