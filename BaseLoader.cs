using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace _03_design_hw
{
    public abstract class BaseLoader
    {
        public string PathToWords{ get; }
        private string PathToBanned{ get; }
        private JObject JsonConfig{ get; }
        public abstract IEnumerable<string> GetWords();
         
        protected BaseLoader(string pathToConfig)
        {
            JsonConfig = JObject.Parse(File.ReadAllText(pathToConfig));
            PathToWords = JsonConfig["words"].ToString();
            PathToBanned = JsonConfig["blacklist"].ToString();
        }

        private HashSet<string> GetBannedWords()
        {
            return new HashSet<string>(
                File.ReadAllLines(PathToBanned)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(x => x.Trim()));
        }

        private Color[] GetColors()
        {
            return JsonConfig["colors"]
                .Select(item => (string)item)                
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(Color.FromName)
                .ToArray();
        }

        public string GetOutputPath()
        {
            return JsonConfig["output"].ToString();
        }

        public Dictionary<string, string> GetSpellingDictionaries()
        {
            return JsonConfig["dictionaries"]
                .ToDictionary(
                token => token[0].ToString(),
                token => token[1].ToString()
                );
        }

        public TagCloudSettings GetSettings()
        {
            var size = new Size((int)JsonConfig["size"][0], (int)JsonConfig["size"][1]);
            var fontName = JsonConfig["fontName"].ToString();
            var fontSizes = JsonConfig["fontSize"].Select(token => (int)token).ToArray();
            var minFontSize = fontSizes[0];
            var maxFontSize = fontSizes[1];
            var top = (int)JsonConfig["top"];
            return new TagCloudSettings(size, GetColors(), GetBannedWords(), fontName, top, minFontSize, maxFontSize);
        }
    }
}