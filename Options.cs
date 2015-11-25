using System.Collections.Generic;
using CommandLine;

namespace _03_design_hw
{
    public class Options
    {
        [Option('c', "config", DefaultValue = "config.json",
            HelpText = "Path to config file.")]
        public string ConfigPath{ get; set; }
    }
}