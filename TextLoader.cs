using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DesignHomeWork
{
    public static class TextLoader
    {
        public static IEnumerable<string> GetWordsFromDictionary(string filePath)
        {
            return File.ReadAllLines(filePath)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(x => x.Trim());
        }

        public static IEnumerable<string> GetWordsFromText(string filePath)
        {
            return null;
        } 
    }
}