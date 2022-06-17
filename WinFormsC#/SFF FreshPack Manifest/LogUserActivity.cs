using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace FreshPackManifest
{
    class LogUserActivity
    {
        public void LogActivity(string User, string activity)
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            if (settings[2] != null && settings[2] != string.Empty && settings[2] != "" && settings[3] != null && settings[3] != string.Empty && settings[3] != "" && settings[18] != null && settings[18] != string.Empty && settings[18] != "" && settings[19] != null && settings[19] != string.Empty && settings[19] != "")
            {
                string connString = "Data Source=" + settings[18] + ";Initial Catalog=" + settings[2] + ";User ID=" + settings[19] + ";Password=" + settings[20] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString = "INSERT INTO " + settings[3] + " (Username,Activity_Description) VALUES (@Val0, @Val1)";
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandText = cmdString;

                            comm.Parameters.AddWithValue("@Val0", User);
                            comm.Parameters.AddWithValue("@Val1", activity);

                            conn.Open();
                            comm.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception f)
                { MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + f.ToString()); }
            }
        }
    }
}
