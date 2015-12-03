using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            File.ReadLines(_pathToFileWithWords)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(x => x.Trim());

    }
}
