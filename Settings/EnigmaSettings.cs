using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaOnForms.Settings
{
    class EnigmaSettings
    {
        public readonly RotorSettings[] rotorSettings;
        public readonly RotorSettings reflectorSettings;
        public readonly PlugBoardSettings plugBoardSettings;

        public EnigmaSettings(RotorSettings[] rotorSettings, RotorSettings reflectorSettings, PlugBoardSettings plugBoardSettings)
        {
            this.rotorSettings = new RotorSettings[rotorSettings.Length];
            this.rotorSettings = rotorSettings;
            this.reflectorSettings = reflectorSettings;
            this.plugBoardSettings = plugBoardSettings;
        }

        public EnigmaSettings(EnigmaSettings enigmaSettings)
        {
            this.rotorSettings = new RotorSettings[enigmaSettings.rotorSettings.Length];
            this.rotorSettings = enigmaSettings.rotorSettings;
            this.reflectorSettings = enigmaSettings.reflectorSettings;
            this.plugBoardSettings = enigmaSettings.plugBoardSettings;
        }
    }
}
