using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_design_hw
{
    public class Word
    {
        public string WordString{ get; }
        public int Frequency{ get; }

        public Word(string word, int frequency)
        {
            WordString = word;
            Frequency = frequency;
        }
    }
}
