using System.Collections.Generic;

namespace _03_design_hw.Loaders
{
    public interface IBlackListLoader
    {
        HashSet<string> BlackList{ get; }
    }
}