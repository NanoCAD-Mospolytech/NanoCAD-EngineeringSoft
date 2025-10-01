using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySample1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            height.Text = "150";
            width.Text = "100";
            radius.Text = "45";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Parametric.height = Convert.ToInt32(height.Text);
            Parametric.width = Convert.ToInt32(width.Text);
            Parametric.radius = Convert.ToInt32(radius.Text);

            if ( Parametric.width > 2*Parametric.radius)
            {
                Parametric.radius = Convert.ToInt32(radius.Text);
            }
            else
            {
                Parametric.radius = (Parametric.width / 2) - 1;
            }
            Sample3d.cmd_Loft();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
