using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnigmaOnForms
{
    class Selector : PictureBox
    {
		int selected;
        public int Selected
		{
			get
			{
				return selected;
			}
			set
			{
				selected = value;
			}
		}
		Font font = new Font("Arial", 14f);

		public delegate void SelectorChangedHandler(int i);
		public event SelectorChangedHandler OnSelectorChanged = delegate { };

        protected override void OnPaint(PaintEventArgs pe)
		{
			var g = pe.Graphics;
			DrawChars(g);
			g.TranslateTransform(Width / 2, Height / 2);
			g.RotateTransform(160);
			g.RotateTransform(45 * selected);
			g.TranslateTransform(-Width / 2 + 20, -Height / 2 + 20);
			g.DrawImage(Image, new Rectangle(0, 0, Width - 50, Height - 50));
		}

		void DrawChars(Graphics g)
		{
			var str = "ABCD";
			for (var i = 0; i < 4; i++)
			{
				var angle = Math.PI/180*45f;
				var rect = new Rectangle(0, 0, 32, 32);
				g.TranslateTransform((Width - font.Size) / 2 - 5, (Height - font.Height) / 2);
				g.TranslateTransform((float) -Math.Cos(angle * -i - Math.PI / 180f * 25) * (Width / 2 - 5), 
									 (float) Math.Sin(angle * -i - Math.PI / 180f * 25) * (Height / 2 - 5));
				g.DrawString(str[i].ToString(), font, Brushes.Black, rect);
				g.ResetTransform();
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
			{
				selected++;
			} else if(e.Button == MouseButtons.Right)
			{
				selected--;
			}
			if (selected < 0) selected = 3;
			if (selected > 3) selected = 0;
			OnSelectorChanged(selected);
			Refresh();
			base.OnMouseDown(e);
		}
    }
}
