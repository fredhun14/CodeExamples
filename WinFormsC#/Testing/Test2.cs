using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testing_app
{
    public partial class Test2 : Form
    {
        public Test2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var main = Application.OpenForms.OfType<Form1>().First();
            main._SecLevel = Int32.Parse(textBox1.Text);
            label1.Text = main._SecLevel.ToString();
        }
    }
}
