using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapey.Objects
{
    class Configuration
    {
        public List<ConfigurationPath> ProductFiles { get; set; }
    }

    internal class ConfigurationPath
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string OutputFile { get; set; }
    }
}
