using System;
using System.Collections.Generic;
using System.Linq;
using _03_design_hw.Loaders;
using _03_design_hw.Word;

namespace _03_design_hw.Statistic
{
    public class Statistic
    {
        private IEnumerable<string> Words{ get; }
        private Random Random{ get; }
        private int Top{ get; }
        private HashSet<string> BlackList{ get; }
        public Statistic(ILoader loader)
        {
            Random = loader.Random;
            BlackList = loader.BlackList;
            Top = loader.Top;
            Words = loader.Words;
        }

        public int MaxCount => WordsWithFrequency.Max(w => w.Frequency);
        public int MinCount => WordsWithFrequency.Min(w => w.Frequency);

        public List<Word.Word> WordsWithFrequency =>
            Words
            .FilterBannedWords(BlackList)
            .GroupBy(w => w)
            .OrderByDescending(g => g.Count())
            .Take(Top)
            .OrderByDescending(g => Random.Next())
            .Select(g => new Word.Word(g.First(), g.Count()))
            .ToList();
    }
}