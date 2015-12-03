using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _03_design_hw.Loaders
{
    public class BlackListLoader : IBlackListLoader
    {
        public BlackListLoader(Options options)
        {
            _pathToBlackList = options.PathToBlackList;
        }

        public HashSet<string> BlackList =>
            new HashSet<string>(
                File.ReadLines(_pathToBlackList)
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(x => x.Trim()));

        private readonly string _pathToBlackList;
    }
}
