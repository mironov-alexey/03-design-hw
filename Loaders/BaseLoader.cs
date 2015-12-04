using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace _03_design_hw.Loaders
{
    public class BaseLoader : ILoader
    {
        public BaseLoader(Options options)
        {
             _jsonConfig = JObject.Parse(File.ReadAllText(options.PathToConfig));
        }

        private readonly JObject _jsonConfig;

        public Color[] Colors => 
            _jsonConfig["colors"]
            .Select(item => (string)item)                
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(ColorTranslator.FromHtml)
            .ToArray();

        public Dictionary<string, string> SpellingDictionaries => 
            _jsonConfig["dictionaries"]
            .ToDictionary(
            token => token[0].ToString(),
            token => token[1].ToString());

        public string FontName => _jsonConfig["fontName"].ToString();

        public int TagsCount => (int) _jsonConfig["tagsCount"];

        public int MinFontSize => _jsonConfig["fontSize"].Select(token => (int)token).ToArray()[0];

        public int MaxFontSize => _jsonConfig["fontSize"].Select(token => (int) token).ToArray()[1];

        public int Width => _jsonConfig["size"].Select(token => (int)token).ToArray()[0];

        public int Height => _jsonConfig["size"].Select(token => (int)token).ToArray()[1];
    }
}