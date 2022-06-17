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
using NiceLabel.SDK;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SFFLabelNManifest
{
    public partial class DailyLogDisplay : Form
    {
        public DailyLogDisplay()
        {
            InitializeComponent();
        }
        #region Form Load
        //The form load in this case runs two similar functions one to load the contents of the packaging manifest and the other too load the freshpack 
        //manifest from the manifest text files.
        private void DailyLogDisplay_Load(object sender, EventArgs e)
        {
            LoadPackaging();
            LoadFreshpack();
        }
        public class Locations
        {
            public string Serial { get; set; }
            public string PalletLocation { get; set; }
            public string Date_Time { get; set; }
        }
        //Loads the packaging manifest from the text file onto the datagrid on the packaging tab
        void LoadPackaging()
        {
            DateCodeGeneration DCG = new DateCodeGeneration();
            string exppath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports" + @"\" + DCG.CodeReturnGenerator("dddyy", "0", "0", 0, 0) + "_REPACK.txt";
            exppath = exppath.Replace("\r", "").Replace("\n", "");
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');

            #region Locations
            if (settings[76] != null && settings[76] != string.Empty && settings[76] != "" && settings[77] != null && settings[76] != string.Empty && settings[77] != "")
            {
                #region Add locations to source file Repack
                if (File.Exists(exppath))
                {
                    string[] RepackLines = File.ReadAllLines(exppath);
                    for (int x = 0; x < RepackLines.Length; x++)
                    {
                        string[] Pallet = RepackLines[x].Split(',');
                        #region SQL stuff
                        Locations[] AllRecords = null;
                        //Data Source=SFFNT8;Initial Catalog=ReplacementDB;Persist Security Info=True;User ID=software;Password=***********
                        string connString = "Data Source=" + settings[1] + ";Initial Catalog=" + settings[76] + ";User ID=" + settings[4] + ";Password=" + settings[5] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                        string cmdString = "SELECT * FROM " + settings[77] + " WHERE SERIAL_NUMBER = '" + Pallet[14] + "'";
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;

                                try
                                {
                                    conn.Open();
                                    comm.ExecuteNonQuery();
                                    using (var reader = comm.ExecuteReader())
                                    {
                                        var list = new List<Locations>();
                                        if (reader != null)
                                        {
                                            while (reader.Read())
                                            { list.Add(new Locations { Serial = reader.GetString(1), PalletLocation = reader.GetString(2), Date_Time = reader.GetString(3) }); }
                                            AllRecords = list.ToArray();
                                        }

                                    }
                                    if (AllRecords.Length != 0)
                                    {
                                        Pallet[19] = "\"" + AllRecords[AllRecords.Length - 1].PalletLocation.PadRight(5, ' ') + "\"";
                                        RepackLines[x] = "";
                                        for (int y = 0; y < Pallet.Length; y++)
                                        {
                                            RepackLines[x] = RepackLines[x] + Pallet[y] + ",";
                                        }
                                        RepackLines[x] = RepackLines[x].Remove(RepackLines[x].Length - 1, 1);
                                    }
                                }
                                catch (Exception f)
                                {
                                    if (f.Message.Contains("Could not find file"))
                                    {

                                    }
                                    else
                                    {
                                        MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + f.ToString());
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    //write all the updated lines to the file with their locations
                    File.WriteAllLines(exppath, RepackLines);
                }
                #endregion
            }
            #endregion

            try
            {
                if (File.Exists(exppath))
                {
                    string[] Lines = File.ReadAllLines(exppath);

                    DataTable dt = new DataTable();
                    DataRow row;
                    DataColumn column1 = new DataColumn(), column2 = new DataColumn(), column3 = new DataColumn(), column4 = new DataColumn(), column5 = new DataColumn(), column6 = new DataColumn(), column7 = new DataColumn();
                    column1.DataType = System.Type.GetType("System.String");
                    column2.DataType = System.Type.GetType("System.String");
                    column3.DataType = System.Type.GetType("System.String");
                    column4.DataType = System.Type.GetType("System.String");
                    column5.DataType = System.Type.GetType("System.String");
                    column6.DataType = System.Type.GetType("System.String");
                    column7.DataType = System.Type.GetType("System.String");

                    column1.ReadOnly = true;
                    column2.ReadOnly = true;
                    column3.ReadOnly = true;
                    column4.ReadOnly = true;
                    column5.ReadOnly = true;
                    column6.ReadOnly = true;
                    column7.ReadOnly = true;

                    column1.ColumnName = "Serial Number";
                    column2.ColumnName = "Line";
                    column3.ColumnName = "Resource";
                    column4.ColumnName = "Description";
                    column5.ColumnName = "Daycode";
                    column6.ColumnName = "Time";
                    column7.ColumnName = "Location";

                    dt.Columns.Add(column1);
                    dt.Columns.Add(column2);
                    dt.Columns.Add(column3);
                    dt.Columns.Add(column4);
                    dt.Columns.Add(column5);
                    dt.Columns.Add(column6);
                    dt.Columns.Add(column7);
                    RepackDataGrid.DataSource = dt;

                    for (int x = 0; x < Lines.Length; x++)
                    {
                        string[] Data = Lines[x].Split(',');
                        row = dt.NewRow();

                        row["Serial Number"] = Data[14].Replace("\"", "");
                        row["Line"] = Data[0].Replace("\"", "");
                        row["Resource"] = Data[12].Replace("\"", "");
                        row["Description"] = Data[5].Replace("\"", "");
                        row["Daycode"] = Data[4].Replace("\"", "");
                        row["Time"] = Data[10].Replace("\"", "");
                        row["Location"] = Data[19].Replace("\"", "");
                        dt.Rows.Add(row);
                    }
                    RepackDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
                RPKLoadCaseCounts();
            }
            catch (Exception ef)
            {
                string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\ErrorLog.txt";
                path15 = path15.Replace("\r", "").Replace("\n", "");
                using (StreamWriter w = File.AppendText(path15))
                {
                    w.WriteLine(" .ToString(): " + ef.ToString() + System.Environment.NewLine + " StackTrace: " + ef.StackTrace.ToString() + System.Environment.NewLine + " MESSAGE: " + ef.Message + System.Environment.NewLine + " Soruce: " + ef.Source);
                };
                MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + ef.ToString());
            }
        }
        //Loads the freshpack manifest from the text file onto the datagrid on the fresh pack tab
        void LoadFreshpack()
        {
            DateCodeGeneration DCG = new DateCodeGeneration();
            string exppath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports" + @"\" + DCG.CodeReturnGenerator("dddyy", "0", "0", 0, 0) + "_FPK.txt";
            exppath = exppath.Replace("\r", "").Replace("\n", "");
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');

            #region Locations
            if (settings[76] != null && settings[76] != string.Empty && settings[76] != "" && settings[77] != null && settings[76] != string.Empty && settings[77] != "")
            {
                #region Add locations to source file freshpack
                if (File.Exists(exppath))
                {
                    string[] FreshPackLines = File.ReadAllLines(exppath);
                    for (int x = 0; x < FreshPackLines.Length; x++)
                    {
                        string[] Pallet = FreshPackLines[x].Split(',');
                        #region SQL stuff
                        Locations[] AllRecords = null;
                        //Data Source=SFFNT8;Initial Catalog=ReplacementDB;Persist Security Info=True;User ID=software;Password=***********
                        string connString = "Data Source=" + settings[1] + ";Initial Catalog=" + settings[76] + ";User ID=" + settings[4] + ";Password=" + settings[5] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                        string cmdString = "SELECT * FROM " + settings[77] + " WHERE SERIAL_NUMBER = '" + Pallet[7] + "'";
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;

                                try
                                {
                                    conn.Open();
                                    comm.ExecuteNonQuery();
                                    using (var reader = comm.ExecuteReader())
                                    {
                                        var list = new List<Locations>();
                                        if (reader != null)
                                        {
                                            while (reader.Read())
                                            { list.Add(new Locations { Serial = reader.GetString(1), PalletLocation = reader.GetString(2), Date_Time = reader.GetString(3) }); }
                                            AllRecords = list.ToArray();
                                        }

                                    }
                                    if (AllRecords.Length != 0)
                                    {
                                        Pallet[13] = "\"" + AllRecords[0].PalletLocation + "\"";
                                        FreshPackLines[x] = "";
                                        for (int y = 0; y < Pallet.Length; y++)
                                        {
                                            FreshPackLines[x] = FreshPackLines[x] + Pallet[y] + ",";
                                        }
                                        FreshPackLines[x] = FreshPackLines[x].Remove(FreshPackLines[x].Length - 1, 1);
                                    }
                                }
                                catch (Exception f)
                                {
                                    if (f.Message.Contains("Could not find file"))
                                    {

                                    }
                                    else
                                    {
                                        MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + f.ToString());
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    //write all the updated lines to the file with their locations
                    File.WriteAllLines(exppath, FreshPackLines);
                }
                #endregion

            }
            #endregion

            try
            {
                if (File.Exists(exppath))
                {
                    string[] Lines = File.ReadAllLines(exppath);

                    DataTable dt = new DataTable();
                    DataRow row;
                    DataColumn column1 = new DataColumn(), column2 = new DataColumn(), column3 = new DataColumn(), column5 = new DataColumn(), column6 = new DataColumn(), column7 = new DataColumn();
                    column1.DataType = System.Type.GetType("System.String");
                    column2.DataType = System.Type.GetType("System.String");
                    column3.DataType = System.Type.GetType("System.String");
                    column5.DataType = System.Type.GetType("System.String");
                    column6.DataType = System.Type.GetType("System.String");
                    column7.DataType = System.Type.GetType("System.String");

                    column1.ReadOnly = true;
                    column2.ReadOnly = true;
                    column3.ReadOnly = true;
                    column5.ReadOnly = true;
                    column6.ReadOnly = true;
                    column7.ReadOnly = true;

                    column1.ColumnName = "Serial Number";
                    column2.ColumnName = "Line";
                    column3.ColumnName = "Resource";
                    column5.ColumnName = "Daycode";
                    column6.ColumnName = "Time";
                    column7.ColumnName = "Location";

                    dt.Columns.Add(column1);
                    dt.Columns.Add(column2);
                    dt.Columns.Add(column3);
                    dt.Columns.Add(column5);
                    dt.Columns.Add(column6);
                    dt.Columns.Add(column7);
                    FreshDataGrid.DataSource = dt;

                    for (int x = 0; x < Lines.Length; x++)
                    {
                        string[] Data = Lines[x].Split(',');
                        row = dt.NewRow();

                        row["Serial Number"] = Data[7].Replace("\"", "");
                        row["Line"] = Data[5].Replace("\"", "");
                        row["Resource"] = Data[2].Replace("\"", "");
                        row["Daycode"] = Data[6].Replace("\"", "");
                        row["Time"] = Data[3].Replace("\"", "");
                        row["Location"] = Data[13].Replace("\"", "");
                        dt.Rows.Add(row);
                    }
                    FreshDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
                FPKLoadCaseCounts();
                CobWeightLoad();
            }
            catch (Exception ef)
            {
                string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\ErrorLog.txt";
                path15 = path15.Replace("\r", "").Replace("\n", "");
                using (StreamWriter w = File.AppendText(path15))
                {
                    w.WriteLine(" .ToString(): " + ef.ToString() + System.Environment.NewLine + " StackTrace: " + ef.StackTrace.ToString() + System.Environment.NewLine + " MESSAGE: " + ef.Message + System.Environment.NewLine + " Soruce: " + ef.Source);
                };
                MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + ef.ToString());
            }
        }
        void CobWeightLoad()
        {
            DateCodeGeneration DCG = new DateCodeGeneration();
            string exppath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports" + @"\" + DCG.CodeReturnGenerator("dddyy", "0", "0", 0, 0) + "_FPK.txt";
            exppath = exppath.Replace("\r", "").Replace("\n", "");
            try
            {
                if (File.Exists(exppath))
                {
                    string[] Lines = File.ReadAllLines(exppath);
                    int Total = 0;
                    for (int x = 0; x < Lines.Length; x++)
                    {
                        string[] theline = Lines[x].Split(',');
                        if (theline[5] == "06")
                        {
                            Total = Total + Int32.Parse(theline[0]);
                        }
                    }
                    CobWeight.Text = Total.ToString();
                }
                else { CobWeight.Text = "0"; }
            }
            catch (Exception ef)
            {
                string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\ErrorLog.txt";
                path15 = path15.Replace("\r", "").Replace("\n", "");
                using (StreamWriter w = File.AppendText(path15))
                {
                    w.WriteLine(" .ToString(): " + ef.ToString() + System.Environment.NewLine + " StackTrace: " + ef.StackTrace.ToString() + System.Environment.NewLine + " MESSAGE: " + ef.Message + System.Environment.NewLine + " Soruce: " + ef.Source);
                };
                MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + ef.ToString());
            }
        }
        void FPKLoadCaseCounts()
        {
            DateCodeGeneration DCG = new DateCodeGeneration();
            string exppath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports" + @"\" + DCG.CodeReturnGenerator("dddyy", "0", "0", 0, 0) + "_FPK.txt";
            exppath = exppath.Replace("\r", "").Replace("\n", "");
            string path = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\PrinterConfigFile.txt";
            path = path.Replace("\r", "").Replace("\n", "");
            string[] printers = File.ReadAllText(path).Split(',');
            int pc = CountLines(path);
            try
            {
                if (File.Exists(exppath))
                {
                    string[] Lines = File.ReadAllLines(exppath);
                    int[] Totals = new int[4];
                    Totals[0] = 0;
                    Totals[1] = 0;
                    Totals[2] = 0;
                    Totals[3] = 0;
                    for (int x = 0; x < Lines.Length; x++)
                    {
                        string[] theline = Lines[x].Split(',');
                        if (pc > 0)
                        {
                            if (theline[5] == printers[0 * 6 + 5])
                            {
                                Totals[0] = Totals[0] + Int32.Parse(theline[0]);
                            }
                        }
                        if (pc > 1)
                        {
                            if (theline[5] == printers[1 * 6 + 5])
                            {
                                Totals[1] = Totals[1] + Int32.Parse(theline[0]);
                            }
                        }
                        if (pc > 2)
                        {
                            if (theline[5] == printers[2 * 6 + 5])
                            {
                                Totals[2] = Totals[2] + Int32.Parse(theline[0]);
                            }
                        }
                        if (pc > 3)
                        {
                            if (theline[5] == printers[3 * 6 + 5])
                            {
                                Totals[3] = Totals[3] + Int32.Parse(theline[0]);
                            }
                        }
                    }
                    FPKCaseCountL1.Text = Totals[0].ToString();
                    FPKCaseCountL2.Text = Totals[1].ToString();
                    FPKCaseCountL3.Text = Totals[2].ToString();
                    FPKCaseCountL4.Text = Totals[3].ToString();
                }
                else
                {
                    FPKCaseCountL1.Text = "0";
                    FPKCaseCountL2.Text = "0";
                    FPKCaseCountL3.Text = "0";
                    FPKCaseCountL4.Text = "0";
                }
            }
            catch(Exception ef)
            {
                string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\ErrorLog.txt";
                path15 = path15.Replace("\r", "").Replace("\n", "");
                using (StreamWriter w = File.AppendText(path15))
                {
                    w.WriteLine(" .ToString(): " + ef.ToString() + System.Environment.NewLine + " StackTrace: " + ef.StackTrace.ToString() + System.Environment.NewLine + " MESSAGE: " + ef.Message + System.Environment.NewLine + " Soruce: " + ef.Source);
                };
                MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + ef.ToString());
            }

        }
        //counts the number of lines in a file
        public int CountLines(string file)
        {
            int count = 0;
            count = File.ReadLines(file).Count();
            return count;
        }
        void RPKLoadCaseCounts()
        {
            DateCodeGeneration DCG = new DateCodeGeneration();
            string exppath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports" + @"\" + DCG.CodeReturnGenerator("dddyy", "0", "0", 0, 0) + "_REPACK.txt";
            exppath = exppath.Replace("\r", "").Replace("\n", "");
            string path = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\PrinterConfigFile.txt";
            path = path.Replace("\r", "").Replace("\n", "");
            string[] printers = File.ReadAllText(path).Split(',');
            int pc = CountLines(path);
            try
            {
                if (File.Exists(exppath))
                {
                    string[] Lines = File.ReadAllLines(exppath);
                    int[] Totals = new int[4];
                    Totals[0] = 0;
                    Totals[1] = 0;
                    Totals[2] = 0;
                    Totals[3] = 0;
                    for (int x = 0; x < Lines.Length; x++)
                    {
                        string[] theline = Lines[x].Split(',');
                        if (pc > 0)
                        {
                            if (theline[0] == "\"Line " + printers[0 * 6 + 3] + "\"" || theline[0] == "\"Line " + printers[0 * 6 + 3] + " \"")
                            {
                                Totals[0] = Totals[0] + Int32.Parse(theline[3]);
                            }
                        }
                        if (pc > 1)
                        {
                            if (theline[0] == "\"Line " + printers[1 * 6 + 3] + "\"" || theline[0] == "\"Line " + printers[1 * 6 + 3] + " \"")
                            {
                                Totals[1] = Totals[1] + Int32.Parse(theline[3]);
                            }
                        }
                        if (pc > 2)
                        {
                            if (theline[0] == "\"Line " + printers[2 * 6 + 3] + "\"" || theline[0] == "\"Line " + printers[2 * 6 + 3] + " \"")
                            {
                                Totals[2] = Totals[2] + Int32.Parse(theline[3]);
                            }
                        }
                        if (pc > 3)
                        {
                            if (theline[0] == "\"Line " + printers[3 * 6 + 3] + "\"" || theline[0] == "\"Line " + printers[3 * 6 + 3] + " \"")
                            {
                                Totals[3] = Totals[3] + Int32.Parse(theline[3]);
                            }
                        }
                    }
                    CaseCountL1.Text = Totals[0].ToString();
                    CaseCountL2.Text = Totals[1].ToString();
                    CaseCountL3.Text = Totals[2].ToString();
                    CaseCountL4.Text = Totals[3].ToString();
                }
                else
                {
                    CaseCountL1.Text = "0";
                    CaseCountL2.Text = "0";
                    CaseCountL3.Text = "0";
                    CaseCountL4.Text = "0";
                }
            }
            catch (Exception ef)
            {
                string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\ErrorLog.txt";
                path15 = path15.Replace("\r", "").Replace("\n", "");
                using (StreamWriter w = File.AppendText(path15))
                {
                    w.WriteLine(" .ToString(): " + ef.ToString() + System.Environment.NewLine + " StackTrace: " + ef.StackTrace.ToString() + System.Environment.NewLine + " MESSAGE: " + ef.Message + System.Environment.NewLine + " Soruce: " + ef.Source);
                };
                MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + ef.ToString());
            }
        }
        #endregion
        #region Buttons
        //Simply closes the form
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Will print a bacti sticker for any manifested ticket that was created the same day regardless of which file it is from.
        private void PrintBacktiButton_Click(object sender, EventArgs e)
        {
            //bacti printer is general settings 80
            try 
            {
                DateCodeGeneration DCG = new DateCodeGeneration();
                string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
                path11 = path11.Replace("\r", "").Replace("\n", "");
                string[] settings = File.ReadAllText(path11).Split(',');
                string Path3 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Formats\\" + "BactiSticker" + ".nlbl";
                Path3 = Path3.Replace("\r", "").Replace("\n", "");

                ILabel label = PrintEngineFactory.PrintEngine.OpenLabel(Path3);
                bool found = false;
                if (settings.Length > 80)
                {
                    label.PrintSettings.PrinterName = settings[80];

                    string exppath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports" + @"\" + DCG.CodeReturnGenerator("dddyy", "0", "0", 0, 0) + "_REPACK.txt";
                    exppath = exppath.Replace("\r", "").Replace("\n", "");
                    string exppath2 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports" + @"\" + DCG.CodeReturnGenerator("dddyy", "0", "0", 0, 0) + "_FPK.txt";
                    exppath2 = exppath2.Replace("\r", "").Replace("\n", "");
                    if (File.Exists(exppath))
                    {

                        string[] Lines = File.ReadAllLines(exppath);


                        for (int x = 0; x < Lines.Length; x++)
                        {
                            string[] Data = Lines[x].Split(',');
                            if (Data[14].Replace("\"", "").Replace(" ", "") == SerialEntryBox.Text)
                            {
                                found = true;
                                label.Variables["SERIAL_NUMBER"].SetValue(Data[14].Replace("\"", "").Replace(" ", ""));
                                label.Variables["Line"].SetValue(Data[0].Replace("\"", "").Replace(" ", ""));
                                label.Variables["Resource"].SetValue(Data[12].Replace("\"", "").Replace(" ", ""));
                                label.Variables["Description"].SetValue(Data[5].Replace("\"", "").Replace(" ", ""));
                                label.Variables["Daycode"].SetValue(Data[9].Replace("\"", "").Replace(" ", ""));
                                label.Variables["Time"].SetValue(Data[10].Replace("\"", "").Replace(" ", ""));
                                break;
                            }
                        }
                    }

                    if (File.Exists(exppath2))
                    {
                        if (found == false)
                        {
                            string[] Lines2 = File.ReadAllLines(exppath2);
                            for (int x = 0; x < Lines2.Length; x++)
                            {
                                string[] Data = Lines2[x].Split(',');
                                if (Data[7].Replace("\"", "").Replace(" ", "") == SerialEntryBox.Text)
                                {
                                    #region for description
                                    string path17 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\DescriptionDatabase.txt";
                                    path17 = path17.Replace("\r", "").Replace("\n", "");
                                    string[] descdata = File.ReadAllText(path17).Split('╨');
                                    string[] resource = Data[2].Split('-');
                                    resource[0] = resource[0].TrimEnd();
                                    string Description = "Unknown Description";
                                    for (int f = 0; f < descdata.Length - 1; f = f + 2)
                                    {
                                        if (resource[0] == descdata[f])
                                        {
                                            Description = descdata[f + 1];
                                            break;
                                        }
                                    }
                                    #endregion
                                    found = true;
                                    label.Variables["SERIAL_NUMBER"].SetValue(Data[7].Replace("\"", "").Replace(" ", ""));
                                    label.Variables["Line"].SetValue(Data[5].Replace("\"", "").Replace(" ", ""));
                                    label.Variables["Resource"].SetValue(Data[2].Replace("\"", "").Replace(" ", ""));
                                    label.Variables["Description"].SetValue(Description);
                                    label.Variables["Daycode"].SetValue(Data[6].Replace("\"", "").Replace(" ", ""));
                                    label.Variables["Time"].SetValue(Data[3].Replace("\"", "").Replace(" ", ""));
                                    break;
                                }
                            }
                        }

                    }
                    if (found)
                    {
                        var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                        LogUserActivity LUA = new LogUserActivity();
                        LUA.LogActivity(pf._User, "Printed bacti sticker for: " + SerialEntryBox.Text);
                        label.Print(4);
                    }
                    else { MessageBox.Show("Serial Not found"); }
                }
            }
            catch(Exception ef)
            {
                string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\ErrorLog.txt";
                path15 = path15.Replace("\r", "").Replace("\n", "");
                using (StreamWriter w = File.AppendText(path15))
                {
                    w.WriteLine(" .ToString(): " + ef.ToString() + System.Environment.NewLine + " StackTrace: " + ef.StackTrace.ToString() + System.Environment.NewLine + " MESSAGE: " + ef.Message + System.Environment.NewLine + " Soruce: " + ef.Source);
                };
                MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + ef.ToString());

            }
        }

        private void GenerateDailyProdTotalsText_Click(object sender, EventArgs e)
        {
            DateSelection ds = new DateSelection();
            var dialogresult = ds.ShowDialog();
            if (dialogresult == DialogResult.OK)
            { productionreportdaterange(ds.Start, ds.End); }
        }
        void productionreportdaterange(DateTime Start, DateTime End)
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            try
            {
                #region PKG side
                #region SQL stuff
                ProductionReport[] AllRecords = null;
                //Data Source=SFFNT8;Initial Catalog=ReplacementDB;Persist Security Info=True;User ID=software;Password=***********
                string connString = "Data Source=" + settings[1] + ";Initial Catalog=" + settings[2] + ";User ID=" + settings[4] + ";Password=" + settings[5] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString = @"SELECT SERIAL_NUMBER,RES_NUMBER,Date_Time,Pallet_Size,AS400_DESCRIPTION FROM " + settings[3] + " WHERE Date_Time >= @dtStart AND Date_Time <= @dtEnd; ";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Parameters.Add("@dtStart", SqlDbType.DateTime).Value = Start;
                        comm.Parameters.Add("@dtEnd", SqlDbType.DateTime).Value = End;
                        comm.Connection = conn;
                        comm.CommandText = cmdString;
                        conn.Open();
                        using (var reader = comm.ExecuteReader())
                        {
                            var list = new List<ProductionReport>();
                            while (reader.Read())
                            { list.Add(new ProductionReport { Serial = reader.GetString(0), resource = reader.GetString(1), Date_Time = reader.GetDateTime(2), CaseCount = reader.GetString(3), Description = reader.GetString(4) }); }
                            AllRecords = list.ToArray();
                        }
                    }
                }
                #endregion
                string[,] Totals = new string[AllRecords.Length, 4];
                bool flag = false;
                for (int x = 0; x < AllRecords.Length; x++)
                {
                    if (Int32.Parse(AllRecords[x].CaseCount) <= 200)
                    {
                        for (int y = 0; y < AllRecords.Length; y++)
                        {
                            if (AllRecords[x].resource == Totals[y, 0])
                            {
                                if (Totals[y, 1] == null || Totals[y, 1] == string.Empty || Totals[y, 1] == "")
                                {
                                    Totals[y, 1] = "1";
                                    Totals[y, 2] = AllRecords[x].CaseCount;
                                    Totals[y, 3] = AllRecords[x].Description;
                                }
                                else
                                {
                                    Totals[y, 1] = (Int32.Parse(Totals[y, 1]) + 1).ToString();
                                    Totals[y, 2] = (Int32.Parse(Totals[y, 2]) + Int32.Parse(AllRecords[x].CaseCount)).ToString();
                                }
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            for (int z = 0; z < AllRecords.Length; z++)
                            {
                                if (Totals[z, 0] == null || Totals[z, 0] == string.Empty || Totals[z, 0] == "")
                                {
                                    Totals[z, 0] = AllRecords[x].resource;
                                    if (Totals[z, 1] == null || Totals[z, 1] == string.Empty || Totals[z, 1] == "")
                                    {
                                        Totals[z, 1] = "1";
                                        Totals[z, 2] = AllRecords[x].CaseCount;
                                        Totals[z, 3] = AllRecords[x].Description;
                                    }
                                    else
                                    {
                                        Totals[z, 1] = (Int32.Parse(Totals[z, 1]) + 1).ToString();
                                        Totals[z, 2] = (Int32.Parse(Totals[z, 2]) + Int32.Parse(AllRecords[x].CaseCount)).ToString();
                                    }
                                    break;
                                }
                            }
                        }
                        flag = false;
                    }
                }

                string Total_cases = "0", Total_pallets = "0";

                for (int z = 0; z < Totals.Length / 3; z++)
                {
                    if (Totals[z, 2] == null || Totals[z, 1] == null) { break; }
                    Total_cases = (Int32.Parse(Total_cases) + Int32.Parse(Totals[z, 2])).ToString();
                    Total_pallets = (Int32.Parse(Total_pallets) + Int32.Parse(Totals[z, 1])).ToString();
                }
                string[] ToFile2 = new string[AllRecords.Length];
                for (int z = 0; z < AllRecords.Length; z++)
                {
                    if (Totals[z, 2] == null || Totals[z, 1] == null) { break; }
                    ToFile2[z] = Totals[z, 0].PadRight(19, '.') + Totals[z, 3].PadRight(39, '.') + Totals[z, 1].PadRight(19, '.') + Totals[z, 2].PadRight(19, '.');
                }
                End = End.AddHours(-(Int32.Parse(settings[11]) + 1));
                string prodpath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\Exports\ProductionData" + Start.DayOfYear.ToString().PadLeft(3, '0') + ".txt";
                prodpath = prodpath.Replace("\r", "").Replace("\n", "");
                File.WriteAllText(prodpath, "");
                using (var file = File.CreateText(prodpath))
                {
                    file.WriteLine(Start.DayOfYear.ToString().PadLeft(3, '0') + "-" + End.DayOfYear.ToString().PadLeft(3, '0') + " " + End.Year.ToString());
                    file.WriteLine("Resource           Description                            Pallet Count       Case Count");
                    for (int x = 0; x < ToFile2.Length; x++)
                    {
                        if (ToFile2[x] == null) { break; }
                        file.WriteLine(ToFile2[x]);
                    }

                    file.WriteLine("Total PKG".PadRight(58, ' ') + Total_pallets.PadRight(19, ' ') + Total_cases.PadRight(19, ' '));
                }
                #endregion
                #region FPK side
                #region SQL stuff
                ProductionReport[] AllRecords2 = null;
                //Data Source=SFFNT8;Initial Catalog=ReplacementDB;Persist Security Info=True;User ID=software;Password=***********
                string connString2 = "Data Source=" + settings[1] + ";Initial Catalog=" + settings[69] + ";User ID=" + settings[4] + ";Password=" + settings[5] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString2 = @"SELECT SERIAL_NUMBER,Resource,Time_Date,Net_Weight,Description FROM " + settings[70] + " WHERE Time_Date >= @dtStart AND Time_Date <= @dtEnd;";

                using (SqlConnection conn = new SqlConnection(connString2))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Parameters.Add("@dtStart", SqlDbType.DateTime).Value = Start;
                        comm.Parameters.Add("@dtEnd", SqlDbType.DateTime).Value = End;
                        comm.Connection = conn;
                        comm.CommandText = cmdString2;
                        conn.Open();
                        using (var reader = comm.ExecuteReader())
                        {
                            var list = new List<ProductionReport>();
                            while (reader.Read())
                            { list.Add(new ProductionReport { Serial = reader.GetString(0), resource = reader.GetString(1), Date_Time = reader.GetDateTime(2), CaseCount = reader.GetString(3), Description = reader.GetString(4) }); }
                            AllRecords2 = list.ToArray();
                        }
                    }
                }
                #endregion
                string[,] Totals2 = new string[AllRecords2.Length, 4];
                bool flag2 = false;
                for (int x = 0; x < AllRecords2.Length; x++)
                {
                    if (Int32.Parse(AllRecords2[x].CaseCount) <= 200)
                    {
                        for (int y = 0; y < AllRecords2.Length; y++)
                        {
                            if (AllRecords2[x].resource == Totals2[y, 0])
                            {
                                if (Totals2[y, 1] == null || Totals2[y, 1] == string.Empty || Totals2[y, 1] == "")
                                {
                                    Totals2[y, 1] = "1";
                                    Totals2[y, 2] = AllRecords2[x].CaseCount;
                                    Totals2[y, 3] = AllRecords2[x].Description;
                                }
                                else
                                {
                                    Totals2[y, 1] = (Int32.Parse(Totals2[y, 1]) + 1).ToString();
                                    Totals2[y, 2] = (Int32.Parse(Totals2[y, 2]) + Int32.Parse(AllRecords2[x].CaseCount)).ToString();
                                }
                                flag2 = true;
                                break;
                            }
                        }
                        if (!flag2)
                        {
                            for (int z = 0; z < AllRecords2.Length; z++)
                            {
                                if (Totals2[z, 0] == null || Totals2[z, 0] == string.Empty || Totals2[z, 0] == "")
                                {
                                    Totals2[z, 0] = AllRecords2[x].resource;
                                    if (Totals2[z, 1] == null || Totals2[z, 1] == string.Empty || Totals2[z, 1] == "")
                                    {
                                        Totals2[z, 1] = "1";
                                        Totals2[z, 2] = AllRecords2[x].CaseCount;
                                        Totals2[z, 3] = AllRecords2[x].Description;
                                    }
                                    else
                                    {
                                        Totals2[z, 1] = (Int32.Parse(Totals2[z, 1]) + 1).ToString();
                                        Totals2[z, 2] = (Int32.Parse(Totals2[z, 2]) + Int32.Parse(AllRecords2[x].CaseCount)).ToString();
                                    }
                                    break;
                                }
                            }
                        }
                        flag2 = false;
                    }
                }

                string Total_cases2 = "0", Total_pallets2 = "0";
                for (int z = 0; z < Totals2.Length / 3; z++)
                {
                    if (Totals2[z, 2] == null || Totals2[z, 1] == null) { break; }
                    Total_cases2 = (Int32.Parse(Total_cases2) + Int32.Parse(Totals2[z, 2])).ToString();
                    Total_pallets2 = (Int32.Parse(Total_pallets2) + Int32.Parse(Totals2[z, 1])).ToString();
                }
                string[] FPKToFile = new string[AllRecords2.Length];
                for (int z = 0; z < AllRecords2.Length; z++)
                {
                    if (Totals2[z, 2] == null || Totals2[z, 1] == null) { break; }
                    FPKToFile[z] = Totals2[z, 0].PadRight(19, '.') + Totals2[z, 3].PadRight(39, '.') + Totals2[z, 1].PadRight(19, '.') + Totals2[z, 2].PadRight(19, '.');
                }
                string gtotalpallet = (Int32.Parse(Total_pallets) + Int32.Parse(Total_pallets2)).ToString();
                string gtotalcase = (Int32.Parse(Total_cases) + Int32.Parse(Total_cases2)).ToString();
                using (var file = File.AppendText(prodpath))
                {
                    for (int x = 0; x < FPKToFile.Length; x++)
                    {
                        if (FPKToFile[x] == null) { break; }
                        file.WriteLine(FPKToFile[x]);
                    }

                    file.WriteLine("Total FPK".PadRight(58, ' ') + Total_pallets2.PadRight(19, ' ') + Total_cases2.PadRight(19, ' '));
                    file.WriteLine("Totals:".PadRight(58, ' ') + gtotalpallet.PadRight(19, ' ') + gtotalcase.PadRight(19, ' '));
                }

                #endregion
                Process.Start(prodpath);
            }
            catch(Exception ef)
            {
                string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\ErrorLog.txt";
                path15 = path15.Replace("\r", "").Replace("\n", "");
                using (StreamWriter w = File.AppendText(path15))
                {
                    w.WriteLine(" .ToString(): " + ef.ToString() + System.Environment.NewLine + " StackTrace: " + ef.StackTrace.ToString() + System.Environment.NewLine + " MESSAGE: " + ef.Message + System.Environment.NewLine + " Soruce: " + ef.Source);
                };
                MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + ef.ToString());
            }
        }
        public class ProductionReport
        {
            public string Serial { get; set; }
            public string resource { get; set; }
            public DateTime Date_Time { get; set; }
            public string CaseCount { get; set; }
            public string Description { get; set; }
        }
        #endregion

        private void SerialEntryBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
