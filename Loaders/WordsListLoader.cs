using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _03_design_hw.Loaders
{
    public static class WordsListLoader
    {
        public static IEnumerable<string> LoadFromFile(string path) =>
            File.ReadLines(path)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(x => x.Trim());
    }
}