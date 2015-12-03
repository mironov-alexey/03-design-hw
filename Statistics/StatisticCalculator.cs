using System;
using System.Collections.Generic;
using System.Linq;
using _03_design_hw.Loaders;

namespace _03_design_hw.Statistics
{
    public class StatisticCalculator
    {
        public StatisticCalculator(ILoader configLoader, IBlackListLoader blackListLoader)
        {
            Random = configLoader.Random;
            BlackList = blackListLoader.BlackList;
            Top = configLoader.TagsCount;
        }

        private Random Random{ get; }
        private int Top{ get; }
        private HashSet<string> BlackList{ get; }

        public Statistic Calculate(IEnumerable<string> words)
        {
            var wordsWithFreq = words
                .FilterBannedWords(BlackList)
                .GroupBy(w => w)
                .OrderByDescending(g => g.Count())
                .Take(Top)
                .OrderByDescending(g => Random.Next())
                .Select(g => new Word(g.First(), g.Count()))
                .ToList();
            return new Statistic(wordsWithFreq);
        }
    }
}