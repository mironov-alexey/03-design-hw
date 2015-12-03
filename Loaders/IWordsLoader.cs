using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_design_hw.Loaders
{
    public interface IWordsLoader
    {
        IEnumerable<string> Words{ get; } 
    }
}
