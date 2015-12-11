using System;
using System.Collections.Generic;
using System.Linq;
using _03_design_hw.CloudGenerator;
using _03_design_hw.Loaders;

namespace _03_design_hw.Statistics
{
    public class StatisticCalculator : IStatisticCalculator
    {
//        private readonly HashSet<string> _blackList;
        private readonly IWordsFilter _filter;
        private readonly Random _random;
        private readonly int _top;

        public StatisticCalculator(Settings settings, IWordsFilter filter)
        {
            _filter = filter;
            _random = new Random();
//            _blackList = blackListLoader.BlackList;
            _top = settings.TagsCount;
        }

        public Statistic Calculate(IEnumerable<string> words)
        {
            var wordsWithFreq = words
                .FilterBannedWords(_filter)
                .GroupBy(w => w)
                .OrderByDescending(g => g.Count())
                .Take(_top)
                .OrderByDescending(g => _random.Next())
                .Select(g => new Word(g.First(), g.Count()))
                .ToList();
            return new Statistic(wordsWithFreq);
        }
    }
}