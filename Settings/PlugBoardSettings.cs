using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaOnForms.Settings
{
    class PlugBoardSettings
    {
        public readonly Dictionary<char, char> links;

        public PlugBoardSettings(Dictionary<char, char> links)
        {
            this.links = new Dictionary<char, char>(links);
        }
    }
}
