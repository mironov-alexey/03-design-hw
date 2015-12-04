using System.Collections.Generic;

namespace _03_design_hw.Loaders
{
    public class BlackListLoader : IBlackListLoader
    {
        private readonly string _pathToBlackList;

        public BlackListLoader(Options options)
        {
            _pathToBlackList = options.PathToBlackList;
        }

        public HashSet<string> BlackList =>
            new HashSet<string>(WordsListLoader.LoadFromFile(_pathToBlackList));
    }
}