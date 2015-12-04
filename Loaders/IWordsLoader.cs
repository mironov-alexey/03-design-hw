using System.Collections.Generic;

namespace _03_design_hw.Loaders
{
    public interface IWordsLoader
    {
        IEnumerable<string> Words{ get; }
    }
}