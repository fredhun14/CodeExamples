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
using System.Data.SqlClient;

namespace SFFPKGLineStatusDisplay
{
    public partial class PrimaryForm : Form
    {
        public PrimaryForm()
        {
            InitializeComponent();
        }

        private void PrimaryForm_Load(object sender, EventArgs e)
        {
            GenerateNeccesaryFiles();
            InititializeControlsData();
        }
        void InititializeControlsData()
        {
            UpdateStatus();
            StatusUpdateTimer.Enabled = true;
        }
        void GenerateNeccesaryFiles()
        {
            if (!File.Exists(Application.StartupPath + @"\ConfigurationPathFile.txt"))
            {
                string filename = "C:";
                using (var fbd = new FolderBrowserDialog())

                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        filename = fbd.SelectedPath.ToString();
                    }

                using (StreamWriter w = File.AppendText(Application.StartupPath + @"\ConfigurationPathFile.txt"))
                {
                    w.WriteLine(filename);
                };
            }
            string path1 = Application.StartupPath + @"\settings.txt";
            if (!File.Exists(path1))
            {
                using (StreamWriter w = File.AppendText(path1))
                {
                    w.WriteLine("SFF_Label_N_Manifest,DBO.LineStatus,SFFVSSQL,software,!Mk!03625");
                };
            }
        }

        private void StatusUpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
        }
        void UpdateStatus()
        {
            string path1 = Application.StartupPath + @"\settings.txt";
            if (File.Exists(path1))
            {
                string[] settings = File.ReadAllText(path1).Split(',');
                for (int x = 0; x < 10; x++)
                {
                    if (settings.Length > 1)
                    {
                        if (settings[0] != null && settings[0] != string.Empty && settings[0] != "" && settings[1] != null && settings[1] != string.Empty && settings[1] != "")
                        {
                            Control[] ct = this.Controls.Find("Panel" + x.ToString(), true);
                            if (ct.Length > 0)
                            {
                                Control[] ct1 = this.Controls.Find("Resource" + x.ToString(), true);
                                Label Resource = ct1[0] as Label;
                                Control[] ct2 = this.Controls.Find("CaseCount" + x.ToString(), true);
                                Label CaseCount = ct2[0] as Label;
                                Control[] ct3 = this.Controls.Find("CPM" + x.ToString(), true);
                                Label CPM = ct3[0] as Label;
                                Control[] ct4 = this.Controls.Find("Labeler" + x.ToString(), true);
                                Label Labeler = ct4[0] as Label;
                                Control[] ct5 = this.Controls.Find("Manifest" + x.ToString(), true);
                                Label Manifest = ct5[0] as Label;
                                Control[] ct6 = this.Controls.Find("JobCount" + x.ToString(), true);
                                Label JobCount = ct6[0] as Label;
                                Control[] ct7 = this.Controls.Find("LastUpdated" + x.ToString(), true);
                                Label LastUpdated = ct7[0] as Label;

                                try
                                {
                                    SqlConnection connString = new SqlConnection("Data Source=" + settings[2] + ";Initial Catalog=" + settings[0] + ";User ID=" + settings[3] + ";Password=" + settings[4] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                                    connString.Open();
                                    SqlCommand cmdString = new SqlCommand("SELECT Current_Case_Count, Current_CPM, Current_Resource, Current_Manifest, Current_Labeler, Current_Job_Count, Last_Updated FROM " + settings[1] + " WHERE Line = '" + x.ToString() + "'", connString);

                                    using (SqlDataReader reader = cmdString.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            Resource.Text = reader["Current_Resource"].ToString();
                                            CaseCount.Text = reader["Current_Case_Count"].ToString();
                                            CPM.Text = reader["Current_CPM"].ToString();
                                            Labeler.Text = reader["Current_Labeler"].ToString();
                                            Manifest.Text = reader["Current_Manifest"].ToString();
                                            JobCount.Text = reader["Current_Job_Count"].ToString();
                                            LastUpdated.Text = reader["Last_Updated"].ToString();
                                        }
                                    }
                                    connString.Close();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + ex.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

