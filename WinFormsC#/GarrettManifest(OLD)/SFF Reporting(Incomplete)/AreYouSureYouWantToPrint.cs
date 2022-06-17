using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SFF_Reporting
{
    public partial class AreYouSureYouWantToPrint : Form
    {
        public AreYouSureYouWantToPrint(string pages)
        {
            InitializeComponent();
            PAGES = pages;
        }
        string PAGES;
        private void AreYouSureYouWantToPrint_Load(object sender, EventArgs e)
        {
            PagesLabel.Text = PAGES;
        }
    }
}
