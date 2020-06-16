using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading.Tasks;
using EnigmaOnForms.Settings;
using System.Threading;

namespace EnigmaOnForms
{
    public partial class Form1 : Form
    {
        EnigmaMachine machine;
        LampBoard lampBoard;
        KeyBoard keyBoard;
        PlugBoard plugBoard;
        EnigmaSettings machineSettings;
        EnigmaSettings settingsToReset;
        List<RotorSettings> rotorsSettings;
		List<RotorSettings> reflectors;

        int[] wheels;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Label[] labels = {Lamp_Q, Lamp_W, Lamp_E, Lamp_R, Lamp_T, Lamp_Y, Lamp_U, Lamp_I, Lamp_O, Lamp_P,
                              Lamp_A, Lamp_S, Lamp_D, Lamp_F, Lamp_G, Lamp_H, Lamp_J, Lamp_K, Lamp_L,
                              Lamp_Z, Lamp_X, Lamp_C, Lamp_V, Lamp_B, Lamp_N, Lamp_M };
            Button[] buttons = {Button_Q, Button_W, Button_E, Button_R, Button_T, Button_Y, Button_U, Button_I, Button_O, Button_P,
                                Button_A, Button_S, Button_D, Button_F, Button_G, Button_H, Button_J, Button_K, Button_L,
                                Button_Z, Button_X, Button_C, Button_V, Button_B, Button_N, Button_M };
            Label[] Plugs = {Plug_Q, Plug_W, Plug_E, Plug_R, Plug_T, Plug_Y, Plug_U, Plug_I, Plug_O, Plug_P,
                             Plug_A, Plug_S, Plug_D, Plug_F, Plug_G, Plug_H, Plug_J, Plug_K, Plug_L,
                             Plug_Z, Plug_X, Plug_C, Plug_V, Plug_B, Plug_N, Plug_M };

            rotorsSettings = new List<RotorSettings>()
            {
                new RotorSettings("EKMFLGDQVZNTOWYHXUSPAIBRCJ", 'Q', 0), new RotorSettings("AJDKSIRUXBLHWTMCQGZNPYFVOE", 'E', 0),
                new RotorSettings("BDFHJLCPRTXVZNYEIWGAKMUSQO", 'V', 0), new RotorSettings("ESOVPZJAYQUIRHXLNFTGKDCMWB", 'J', 0),
                new RotorSettings("VZBRGITYUPSDNHLXAWMJQOFECK", 'Z', 0), new RotorSettings("JPGVOUMFYQBENHZRDKASXLICTW", 'Z', 0),
                new RotorSettings("NZJHGRCXMYSWBOUFAIVLPEKQDT", 'Z', 0), new RotorSettings("FKQHTLXOCBJSPDZRAMEWNIUYGV", 'Z', 0)
            };

            reflectors = new List<RotorSettings>()
            {
                new RotorSettings("EJMZALYXVBWFCRQUONTSPIKHGD", 'A', 0), //A
                new RotorSettings("YRUHQSLDPXNGOKMIEBFZCWVJAT", 'A', 0), //B
                new RotorSettings("FVPJIAOYEDRZXWGCTKUQSBNMHL", 'A', 0), //C
                new RotorSettings("ENKQAUYWJICOPBLMDXZVFTHRGS", 'A', 0) //B Thin
			};

            lampBoard = new LampBoard(labels);
            keyBoard = new KeyBoard(buttons);
            plugBoard = new PlugBoard(Plugs);
            wheels = new int[3];

            var rotorI = new Rotor(rotorsSettings[0]);
            var rotorII = new Rotor(rotorsSettings[1]);
            var rotorIII = new Rotor(rotorsSettings[2]);
            machine = new EnigmaMachine(plugBoard);

            machine.OnRotorRotated += (i, ch) =>
            {
                switch (i)
                {
                    case 0:
                        r1.Text = ch.ToString();
                        break;
                    case 1:
                        r2.Text = ch.ToString();
                        break;
                    case 2:
                        r3.Text = ch.ToString();
                        break;
                }
            };
            machine.Refresh();

            r1.MouseWheel += textBoxR_Wheel;
            r2.MouseWheel += textBoxR_Wheel;
            r3.MouseWheel += textBoxR_Wheel;


            keyBoard.OnKeyDown += (c) => { var ch = machine.Encrypt(c); boxOut.Text += ch; boxIn.Text += c; lampBoard.Enable(ch); };
            keyBoard.OnKeyUp += (c) => { lampBoard.Disable(); };

            machineSettings = new EnigmaSettings(machine.GetSettings());
            settingsToReset = new EnigmaSettings(machine.GetSettings());

			boxIn.KeyDown += KeyDown_Delete_Back;
			boxOut.KeyDown += KeyDown_Delete_Back;
			selector.OnSelectorChanged += (i) => {
				Console.WriteLine(i);
                machine.SetReflector(reflectors[i]);
            };
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            boxIn.Text = "";
            boxOut.Text = "";
            machine.SetSettings(settingsToReset);
            machine.SetReflector(reflectors[selector.Selected]);
        }
        private void button3_Click(object sender, EventArgs e)
        {

            machineSettings = machine.GetSettings();
            wheels[0] = comboBox1.SelectedIndex;
            wheels[1] = comboBox2.SelectedIndex;
            wheels[2] = comboBox3.SelectedIndex;
        }

        private void boxIn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Control) return;

            if (e.KeyCode == Keys.V && e.Control)
            {
                if (Clipboard.ContainsText())
                {
                    var str = Clipboard.GetText().ToUpper();
                    foreach (var ch in str.Where(x => x >= 'A' && x <= 'Z'))
                    {
                        keyBoard.KeyDown(ch);
                        keyBoard.KeyUp(ch);
                    }
                }
            }
            else if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                keyBoard.KeyDown(Enum.GetName(typeof(Keys), e.KeyCode)[0]);
            }
        }

        private void boxIn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                keyBoard.KeyUp(Enum.GetName(typeof(Keys), e.KeyCode)[0]);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            machine.SetSettings(machineSettings);
            machine.SetRotor(0, rotorsSettings[wheels[0]]);
            machine.SetRotor(1, rotorsSettings[wheels[1]]);
            machine.SetRotor(2, rotorsSettings[wheels[2]]);
			machine.SetReflector(reflectors[selector.Selected]);
		}

        private void textBoxR_Wheel(object sender, MouseEventArgs e)
        {
            if (sender is TextBox textbox)
            {
                var i = 0;
                if (e.Delta > 0) i = 1;
                else
                    if (e.Delta < 0) i = -1;

                switch (textbox.Name)
                {
                    case "r1":
                        machine.GetRotor(0).Rotate(i);
                        break;
                    case "r2":
                        machine.GetRotor(1).Rotate(i);
                        break;
                    case "r3":
                        machine.GetRotor(2).Rotate(i);
                        break;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(sender is ComboBox comboBox)
            {
               machine.SetRotor(0, rotorsSettings[comboBox.SelectedIndex]);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                machine.SetRotor(1, rotorsSettings[comboBox.SelectedIndex]);
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                machine.SetRotor(2, rotorsSettings[comboBox.SelectedIndex]);
            }
        }

        private void KeyDown_Delete_Back(object sender, KeyEventArgs e)
        {
            if(sender is TextBox tb)
			{
				if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
				{
					if (tb.Text.Length == 0) return;
					var i = tb.SelectionStart;

					if (!string.IsNullOrEmpty(tb.SelectedText))
					{
						tb.Text = tb.Text.Remove(tb.SelectionStart, tb.SelectionLength);
						tb.SelectionStart = i;
						return;
					}
					else if (e.KeyCode == Keys.Back)
					{
						if (tb.SelectionStart == 0) return;
						StringBuilder sb = new StringBuilder(tb.Text);
						sb.Remove(tb.SelectionStart - 1, 1);
						tb.Text = sb.ToString();
						tb.SelectionStart = i - 1;
					}
					else if (e.KeyCode == Keys.Delete)
					{
						if (tb.SelectionStart == tb.Text.Length) return;
						StringBuilder sb = new StringBuilder(tb.Text);
						sb.Remove(tb.SelectionStart, 1);
						tb.Text = sb.ToString();
						tb.SelectionStart = i;
					}
				}
			}
        }

        private void rotor_KeyDown(object sender, KeyEventArgs e)
        {
			if (e.KeyCode < Keys.A || e.KeyCode > Keys.Z) return;

			var ch = Enum.GetName(typeof(Keys), e.KeyCode)[0];

			if (sender is TextBox textbox)
            {
                switch(textbox.Name)
                {
                    case "r1":
						machine.GetRotor(0).SetPositionChar(ch);
                        break;
                    case "r2":
						machine.GetRotor(1).SetPositionChar(ch);
						break;
                    case "r3":
						machine.GetRotor(2).SetPositionChar(ch);
						break;
                }
            }
        }
    }
}