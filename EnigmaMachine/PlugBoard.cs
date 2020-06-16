using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnigmaOnForms.Settings;

namespace EnigmaOnForms
{
	class PlugBoard
	{
		Dictionary<char, Label> plugs;
		Dictionary<char, char> links;
		Label currentPlug;

		public PlugBoard(Label[] labels)
		{
			links = new Dictionary<char, char>();
			plugs = new Dictionary<char, Label>();
			foreach (Label label in labels)
			{
				label.MouseClick += (sender, e) =>
				{
					switch (e.Button)
					{
						case MouseButtons.Left:
							LeftButton((Label)sender);
							break;
						case MouseButtons.Right:
							RightButton((Label)sender);
							break;
					}
				};
				plugs.Add(char.ToUpper(label.Text[0]), label);
			}
		}

		void AddLink(char curr, char key)
		{
			links.Add(curr, key);
			links.Add(key, curr);
			var color = GetAvailableColor();
			plugs[curr].BackColor = color;
			plugs[key].BackColor = color;
		}

		void RemoveLink(char key)
		{
			ResetAvailableColor(plugs[links[key]].BackColor);
			plugs[links[key]].BackColor = Color.White;
			plugs[key].BackColor = Color.White;
			links.Remove(links[key]);
			links.Remove(key);
		}

		void LeftButton(Label label)
		{
			var key = char.ToUpper(label.Text[0]);
			if (links.ContainsKey(key)) return;
			if (currentPlug == null)
			{
				currentPlug = label;
				SelectLabel(currentPlug);
			}
			if (currentPlug != null && key == GetCharFromLabel(currentPlug))
			{
				return;
			}
			else
			{
				AddLink(GetCharFromLabel(currentPlug), key);
				UnSelectLabel(currentPlug);
				currentPlug = null;

			}
		}
		void RightButton(Label label)
		{
			var key = char.ToUpper(label.Text[0]);

			if (links.ContainsKey(key))
			{
				RemoveLink(key);
			}
		}

		List<Color> availableColors = new List<Color>()
		{
			Color.Red, Color.Green, Color.FromArgb(100,100,255), Color.Gray, Color.Orange, Color.Cyan, Color.DarkGray,
			Color.Yellow, Color.Lime, Color.Brown, Color.Pink, Color.DarkViolet, Color.Magenta
		};

		Color GetAvailableColor()
		{
			var color = availableColors.First();
			availableColors.Remove(color);
			return color;
		}

		void ResetAvailableColor(Color color)
		{
			availableColors.Add(color);
		}

        char GetCharFromLabel(Label label)
		{
			return char.ToUpper(label.Text[0]);
		}
        void SelectLabel(Label label)
		{
			((PlugLabel) label).Selected = true;
		}
		void UnSelectLabel(Label label)
		{
			((PlugLabel) label).Selected = false;
		}

		public char Encrypt(char ch) 
		{
			if (links.ContainsKey(ch)) 
			{
				return links[ch];
			}
			return ch;
		}

		public PlugBoardSettings GetSettings()
		{
			var plugBoardSettings = new PlugBoardSettings(links);
			return plugBoardSettings;
		}

		public void SetSettings(PlugBoardSettings plugBoardSettings)
		{
			this.links = plugBoardSettings.links;
		}
	}
}
