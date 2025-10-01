using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loft
{
    public partial class LoftForm : Form
    {
        public LoftForm()
        {
            InitializeComponent();
        }

        private void buttonBuild_Click(object sender, EventArgs e)
        {
            Parametric.height = Convert.ToInt32(height.Text);
            Parametric.width = Convert.ToInt32(width.Text);
            Parametric.radius = Convert.ToInt32(radius.Text);

            if (Parametric.width > 2 * Parametric.radius)
            {
                Parametric.radius = Convert.ToInt32(radius.Text);
            }
            else
            {
                Parametric.radius = (Parametric.width / 2) - 1;
            }
            Loft.Commands.cmd_Loft();
            this.Close();
        }
    }
}
