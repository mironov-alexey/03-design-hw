using System.Collections.Generic;
using System.Drawing;

namespace _03_design_hw.Loaders
{
    public class Settings
    {
        public Color[] Colors{ get; set; }

        public Dictionary<string, string> SpellingDictionaries{ get; set; }

        public string FontName{ get; set; }

        public int TagsCount{ get; set; }

        public int MinFontSize{ get; set; }

        public int MaxFontSize{ get; set; }

        public int Width{ get; set; }

        public int Height{ get; set; }
    }
}