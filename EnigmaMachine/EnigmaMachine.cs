using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnigmaOnForms.Settings;

namespace EnigmaOnForms
{
	class EnigmaMachine
	{
		Rotor[] rotors;
		Rotor reflector;
		PlugBoard plugBoard;

		public delegate void RotorRotatedHandler(int index, char ch);
		public event RotorRotatedHandler OnRotorRotated = delegate{ };
		public EnigmaMachine(PlugBoard plugBoard) 
		{
			var rotor = new Rotor("EKMFLGDQVZNTOWYHXUSPAIBRCJ", 'Q');
			var rotor2 = new Rotor("AJDKSIRUXBLHWTMCQGZNPYFVOE", 'E');
			var rotor3 = new Rotor("BDFHJLCPRTXVZNYEIWGAKMUSQO", 'V');
			rotors = new[] { rotor, rotor2, rotor3 };
			foreach (var rtr in rotors) 
			{
				rtr.OnRotated += (sender, ch) => 
				{
					OnRotorRotated(Array.IndexOf(rotors, sender), ch);
				};
			}
			reflector = new Rotor("EJMZALYXVBWFCRQUONTSPIKHGD", 'D');
			this.plugBoard = plugBoard;
		}

		public Rotor GetRotor(int ID)
		{
			return rotors[ID];
		}

		public void Refresh() 
		{
			foreach (var rtr in rotors) 
			{
				rtr.Refresh();
			}
		}

		void RotateRotors() 
		{
			if (rotors[1].IsTurnOver())
			{
				rotors[1].Rotate();
				rotors[0].Rotate();
			}
			else if (rotors[2].IsTurnOver())
			{
				rotors[1].Rotate();
			}
			rotors[2].Rotate();
		}

		public char Encrypt(char ch) 
		{
			
			RotateRotors();

			ch = plugBoard.Encrypt(ch);

			for (int i = rotors.Length - 1; i >= 0; i--) 
			{
				ch = rotors[i].Encrypt(ch);
			}

			ch = reflector.Encrypt(ch);

			for (int i = 0; i < rotors.Length; i++) 
			{
				ch = rotors[i].Encrypt(ch, true);
			}

			ch = plugBoard.Encrypt(ch);

			return ch;
		}

		public void SetRotor(int n, RotorSettings rotorSettings)
		{
			if (n < rotors.Length)
			{
				rotors[n].SetSettings(rotorSettings);
			}
			rotors[n].Refresh();
		}

		public void SetReflector(RotorSettings reflector)
		{
			this.reflector.SetSettings(reflector);
		}

		public EnigmaSettings GetSettings()
		{
			var rotorSettings = new RotorSettings[this.rotors.Length];
			for (int i = 0; i < this.rotors.Length; i++)
			{
				rotorSettings[i] = this.rotors[i].GetSettings();
			}

			var enigmaSettings = new EnigmaSettings(rotorSettings, this.reflector.GetSettings(), this.plugBoard.GetSettings());
			return enigmaSettings;
		}

		public void SetSettings(EnigmaSettings enigmaSettings)
		{
			for (int i = 0; i < this.rotors.Length; i++)
			{
				this.rotors[i].SetSettings(enigmaSettings.rotorSettings[i]);
			}

			this.reflector.SetSettings(enigmaSettings.reflectorSettings);
			this.plugBoard.SetSettings(enigmaSettings.plugBoardSettings);
			Refresh();
		}

		public int GetCountOfRotors()
		{
			return this.rotors.Length;
		}
	}
}
