using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace _03_design_hw.Loaders
{
    public class BaseSettingsLoader : ISettingsLoader
    {
        private readonly JObject _jsonConfig;

        public BaseSettingsLoader(Options options)
        {
            _jsonConfig = JObject.Parse(File.ReadAllText(options.PathToConfig));
        }

        private Color[] Colors =>
            _jsonConfig["colors"]
                .Select(item => (string) item)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(ColorTranslator.FromHtml)
                .ToArray();

        private Dictionary<string, string> SpellingDictionaries =>
            _jsonConfig["dictionaries"]
                .ToDictionary(
                    token => token[0].ToString(),
                    token => token[1].ToString());

        private string FontName => _jsonConfig["fontName"].ToString();

        private int TagsCount => (int) _jsonConfig["tagsCount"];

        private int MinFontSize => _jsonConfig["fontSize"].Select(token => (int) token).ToArray()[0];

        private int MaxFontSize => _jsonConfig["fontSize"].Select(token => (int) token).ToArray()[1];

        private int Width => _jsonConfig["size"].Select(token => (int) token).ToArray()[0];

        private int Height => _jsonConfig["size"].Select(token => (int) token).ToArray()[1];

        public Settings Load()
        {
            return new Settings
            {
                Colors = Colors,
                Height = Height,
                Width = Width,
                MinFontSize = MinFontSize,
                MaxFontSize = MaxFontSize,
                FontName = FontName,
                TagsCount = TagsCount,
                SpellingDictionaries = SpellingDictionaries
            };
        }
    }
}