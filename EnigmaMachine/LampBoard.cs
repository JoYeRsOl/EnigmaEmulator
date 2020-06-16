using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnigmaOnForms
{
    class LampBoard
    {
        Dictionary<Char, Label> lamps;
		Label lastLamp;

        public LampBoard(Label[] labels)
        {
            var dict = new Dictionary<Char, Label>();
            foreach (Label label in labels)
            {
                dict.Add(char.ToUpper(label.Text[0]), label);
            }
			lamps = dict;
        }

		public void Enable(char c) 
        {
			var bc = char.ToUpper(c);

			if (lamps.ContainsKey(bc))
                LightOn(lamps[bc]);
		}

		public void Disable() => LightOff();
        
		void LightOn(Label l) 
        {
			LightOff();
			lastLamp = l;
			l.BackColor = Color.FromArgb(200, 200, 0);
		}

        void LightOff() 
        {
			if (lastLamp == null) return;
			lastLamp.BackColor = Color.DarkGray;
		}
    }
}
