using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaOnForms.Settings
{
    class RotorSettings
    {
        public readonly string wiring;
        public readonly char notch;
        public readonly int offset;

        public RotorSettings(string wiring, char notch, int offset)
        {
            this.wiring = wiring;
            this.notch = notch;
            this.offset = offset;
        }
    }
}
