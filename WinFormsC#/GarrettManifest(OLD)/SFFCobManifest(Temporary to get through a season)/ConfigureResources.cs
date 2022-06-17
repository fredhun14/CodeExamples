using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GarrettManifester
{
    public partial class ConfigureResources : Form
    {
        public ConfigureResources()
        {
            InitializeComponent();
        }

        private void DropButton_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Application.StartupPath + "\\resources.txt";
                path = path.Replace("\r", "").Replace("\n", "");
                string[] resources = File.ReadAllLines(path);
                resources[Int32.Parse(SectionBox.SelectedItem.ToString())-1] = resources[Int32.Parse(SectionBox.SelectedItem.ToString())-1] + "," + DropText.Text;

                File.WriteAllLines(path, resources);
                MessageBox.Show("Successfully added!");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + ex.ToString());
            }
        }

        private void OkButt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CancelButt_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
