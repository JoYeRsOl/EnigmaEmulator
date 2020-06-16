using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnigmaOnForms
{
    class KeyBoard
    {
		Dictionary<char, Button> buttons;
		Button pressedButton;

		public delegate void KeyDownHandler(char c);
        public event KeyDownHandler OnKeyDown = delegate {};
        public event KeyDownHandler OnKeyUp = delegate {};

        public KeyBoard(Button[] controlButtons) 
        {
			var dict = new Dictionary<Char, Button>();
            foreach(var button in controlButtons)
			{
				button.MouseDown += (sender, e) => 
				{
					var b = (Button) sender;
					var ch = char.ToUpper(b.Text[0]);
					KeyDown(ch);
				};
				button.MouseUp += (sender, e) => 
				{
					var b = (Button) sender;
					var ch = char.ToUpper(b.Text[0]);
					KeyUp(ch);
				};
				dict.Add(char.ToUpper(button.Text[0]), button);
			}
			buttons = dict;
		}

        public void KeyDown(char c) 
		{
			if (pressedButton != null) return;
			char ch = char.ToUpper(c);
			if (!buttons.ContainsKey(ch)) return;
			pressedButton = buttons[ch];
			OnKeyDown(ch);
			LightOn();
		}

        public void KeyUp(char c) 
		{
			if (pressedButton == null) return;
			char bc = char.ToUpper(c);
			if (!buttons.ContainsKey(bc)) return;
			if (pressedButton == buttons[bc]) LightOff();
			OnKeyUp(c);
		}

        void LightOn() 
		{
			pressedButton.BackColor = Color.FromArgb(200, 200, 0);
		}

        void LightOff() 
		{
			pressedButton.BackColor = Color.DarkGray;

			pressedButton = null;
		}
    }
}
