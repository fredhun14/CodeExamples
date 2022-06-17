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

namespace SFF_Reporting
{
    public partial class GeneralSettings : Form
    {
        public GeneralSettings()
        {
            InitializeComponent();
        }

        private void GeneralSettings_Load(object sender, EventArgs e)
        {
            string path11 = @"GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            SQLServerName.Text = settings[0];
            SQLUserID.Text = settings[1];
            SQLUserPassword.Text = settings[2];
            SQLWestonPKGManifestDBName.Text = settings[3];
            SQLWestonPKGManifestTBName.Text = settings[4];
            SQLWestonFPKManifestDBName.Text = settings[5];
            SQLWestonFPKManifestTBName.Text = settings[6];
            SQLWestonCPMDBName.Text = settings[7];
            SQLWestonCPMTBName.Text = settings[8];
            SQLWestonOPCDataLogDBName.Text = settings[9];
            SQLWestonOPCDataLogTBName.Text = settings[10];
            SQLGarrettPKGManifestDBName.Text = settings[11];
            SQLGarrettPKGManifestTBName.Text = settings[12];
            SQLGarrettFPKManifestDBName.Text = settings[13];
            SQLGarrettFPKManifestTBName.Text = settings[14];
            SQLGarrettCPMDBName.Text = settings[15];
            SQLGarrettCPMTBName.Text = settings[16];
            SQLGarrettOPCDataLogDBName.Text = settings[17];
            SQLGarrettOPCDataLogTBName.Text = settings[18];
            GenWestenStartTimeUpDown.Value = Int32.Parse(settings[19]);
            GenGarrettStartTime.Value = Int32.Parse(settings[20]);
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            string path11 = @"GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string newline = SQLServerName.Text + "," + SQLUserID.Text + "," + SQLUserPassword.Text + "," + SQLWestonPKGManifestDBName.Text + "," + SQLWestonPKGManifestTBName.Text + "," + SQLWestonFPKManifestDBName.Text + "," + SQLWestonFPKManifestTBName.Text + "," + SQLWestonCPMDBName.Text + "," + SQLWestonCPMTBName.Text + "," + SQLWestonOPCDataLogDBName.Text + "," + SQLWestonOPCDataLogTBName.Text + "," +SQLGarrettPKGManifestDBName.Text + "," + SQLGarrettPKGManifestTBName.Text + "," + SQLGarrettFPKManifestDBName.Text + "," + SQLGarrettFPKManifestTBName.Text + "," + SQLGarrettCPMDBName.Text + "," + SQLGarrettCPMTBName.Text + "," + SQLGarrettOPCDataLogDBName.Text + "," + SQLGarrettOPCDataLogTBName.Text + "," + GenWestenStartTimeUpDown.Value.ToString() + "," + GenGarrettStartTime.Value.ToString() + ",";
            //expand the above line for each item add a comma to the end should be simple to extend and add setting like this.
            //Remember to adjust the default settings in generateneccesaryfiles() in the primary form when adding items to the list so that things don't break.
            //Item List:
            /* 0 SQL Server name
             * 1 SQL username
             * 2 SQL password
             * 3 SQL Weston pkg database name
             * 4 SQL Weston pkg table name
             * 5 SQL Weston fpk database name
             * 6 SQL Weston fpk table name
             * 7 SQL Weston CPM Log database name
             * 8 SQL Weston CPM Log table name
             * 9 SQL Weston OPC Data log database name
             * 10 SQL Weston OPC Data log table name
             * 11 SQL Garrett pkg database name
             * 12 SQL Garrett pkg table name
             * 13 SQL Garrett fpk database name
             * 14 SQL Garrett fpk table name
             * 15 SQL Garrett CPM Log database name
             * 16 SQL Garrett CPM Log table name
             * 17 SQL Garrett OPC Data log database name
             * 18 SQL Garrett OPC Data log table name
             * 19 GEN Weston start time
             * 20 GEN Garrett start time
             * 21
             */

            File.WriteAllText(path11, newline);
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
