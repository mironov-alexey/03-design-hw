using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _03_design_hw
{
    public class DictionaryLoader : BaseLoader
    {
        public DictionaryLoader(string configPath)
            : base(configPath)
        {
        }
        public new virtual IEnumerable<string> Words =>
            File.ReadAllLines(InputPath)
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(x => x.Trim());
    }
}