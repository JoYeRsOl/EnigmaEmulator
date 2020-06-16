using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnigmaOnForms.Settings;

namespace EnigmaOnForms
{
	

	class Rotor 
	{
		const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		string wiring;
		char notch;
		int offset;

		public delegate void RotateHandler(Rotor sender, char ch);
		public event RotateHandler OnRotated = delegate { };

		public Rotor(string wiring, char notch) 
		{
			this.wiring = wiring;
			this.notch = notch;
		}

		public Rotor(RotorSettings rotorSettings)
		{
			this.wiring = rotorSettings.wiring;
			this.notch = rotorSettings.notch;
		}
		public void Refresh()
		{
			OnRotated(this, GetPositionChar());
		}

		public char Encrypt(char ch, bool reverse = false) 
		{
			var pos = CutNumber(alphabet.IndexOf(ch) + offset);
			if (reverse) 
			{
				ch = alphabet[wiring.IndexOf(alphabet[pos])];
			} 
			else 
			{
				ch = wiring[pos];
			}
			ch = alphabet[CutNumber(alphabet.IndexOf(ch) - offset)];
			return ch;
		}

		public char GetPositionChar() 
		{
			return alphabet[offset];
		}

		public void SetPositionChar(char ch)
		{
			offset = alphabet.IndexOf(ch);
			Refresh();
		}

		public void Rotate(int delta = 1)
		{
			offset = CutNumber(offset + delta);
			OnRotated(this, GetPositionChar());
		}

		public bool IsTurnOver() 
		{
			return alphabet[offset] == notch;
		}

		int CutNumber(int i) 
		{
			if (i > 25) i -= 26;
			if (i < 0) i += 26;
			return i;
		}

		public RotorSettings GetSettings()
		{
			var rotorSetting = new RotorSettings(this.wiring, this.notch, this.offset);
			return rotorSetting;
		}

		public void SetSettings(RotorSettings rotorSettings)
		{
			this.wiring = rotorSettings.wiring;
			this.notch = rotorSettings.notch;
			this.offset = rotorSettings.offset;

		}
	}
}
