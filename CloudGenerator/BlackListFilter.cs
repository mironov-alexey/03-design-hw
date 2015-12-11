using System.Collections.Generic;
using _03_design_hw.Loaders;

namespace _03_design_hw.CloudGenerator
{
    internal class BlackListFilter : IWordsFilter
    {
        private readonly HashSet<string> _blackList;

        public BlackListFilter(IBlackListLoader blackListLoader)
        {
            _blackList = blackListLoader.BlackList;
        }

        public bool Filter(string word) =>
            !_blackList.Contains(word);
    }
}