using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnigmaOnForms
{
    public class PlugLabel : Label
    {
		bool selected;
		public bool Selected { get
			{
				return selected;
			}
			set {
				selected = value;
				this.Refresh();
			} 
		}
		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);
			if (selected) {
				ControlPaint.DrawBorder(e.Graphics, this.DisplayRectangle,
										  Color.Black, 3, ButtonBorderStyle.Dashed,
										  Color.Black, 3, ButtonBorderStyle.Dashed,
										  Color.Black, 3, ButtonBorderStyle.Dashed,
										  Color.Black, 3, ButtonBorderStyle.Dashed);
			}
		}
    }
}
