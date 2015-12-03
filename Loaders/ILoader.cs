using System;
using System.Collections.Generic;
using System.Drawing;

namespace _03_design_hw.Loaders
{
    public interface ILoader
    {
        string FontName{ get; }
        int TagsCount{ get; }
        int MinFontSize { get; }
        int MaxFontSize{ get; }
        int Width{ get; }
        int Height{ get; }
        Random Random{ get; }
        Dictionary<string, string> SpellingDictionaries{ get; }
        Color[] Colors{ get; }
    }
}