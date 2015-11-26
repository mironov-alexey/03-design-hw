﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace _03_design_hw
{
    public class Statistic
    {
        private IEnumerable<string> Words{ get; }
        private Random Random{ get; }
        private int Top{ get; }
        private HashSet<string> BlackList{ get; }
        public Statistic(TagCloudSettings settings, List<string> words)
        {
            Words = words;
            Random = settings.Random;
            BlackList = settings.BlackList;
            Top = settings.Top;
        }

        public int MaxCount => WordsWithFrequency.Max(w => w.Frequency);
        public int MinCount => WordsWithFrequency.Min(w => w.Frequency);

        public List<Word> WordsWithFrequency =>
            Words
            .FilterBannedWords(BlackList)
            .GroupBy(w => w)
            .OrderByDescending(g => g.Count())
            .Take(Top)
            .OrderByDescending(g => Random.Next())
            .Select(g => new Word(g.First(), g.Count()))
            .ToList();
    }
}