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

namespace FreshPackManifest
{
    public partial class DailyLogDisplay : Form
    {
        public DailyLogDisplay()
        {
            InitializeComponent();
        }
        public class Locations
        {
            public string Serial { get; set; }
            public string PalletLocation { get; set; }
            public string Date_Time { get; set; }
        }
        void LoadData()
        {
            try
            {
                string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
                path11 = path11.Replace("\r", "").Replace("\n", "");
                string[] settings = File.ReadAllText(path11).Split(',');
                string expath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports\\" + "PATTYN" + CodeReturnGenerator("ddd-yy", "0", 0) + ".txt";
                expath = expath.Replace("\r", "").Replace("\n", "");
                if (File.Exists(expath))
                {
                    string[] Lines = File.ReadAllLines(expath);

                    #region Add locations to source file freshpack
                    string[] FreshPackLines = File.ReadAllLines(expath);
                    for (int x = 0; x < FreshPackLines.Length; x++)
                    {
                        string[] Pallet = FreshPackLines[x].Split(',');
                        #region SQL stuff
                        Locations[] AllRecords = null;
                        //Data Source=SFFNT8;Initial Catalog=ReplacementDB;Persist Security Info=True;User ID=software;Password=***********
                        string connString = "Data Source=" + settings[18] + ";Initial Catalog=" + settings[15] + ";User ID=" + settings[19] + ";Password=" + settings[20] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                        string cmdString = "SELECT * FROM " + settings[16] + " WHERE SERIAL_NUMBER = '" + Pallet[7] + "'";
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
                    }
                    File.WriteAllLines(expath, FreshPackLines);
                    #endregion
                    #endregion
                    Lines = File.ReadAllLines(expath);

                    DataTable dt = new DataTable();
                    DataRow row;
                    DataColumn column1 = new DataColumn(), column2 = new DataColumn(), column3 = new DataColumn(), column5 = new DataColumn(), column6 = new DataColumn(), column7 = new DataColumn(), column8 = new DataColumn();
                    column1.DataType = System.Type.GetType("System.String");
                    column2.DataType = System.Type.GetType("System.String");
                    column3.DataType = System.Type.GetType("System.String");
                    column5.DataType = System.Type.GetType("System.String");
                    column6.DataType = System.Type.GetType("System.String");
                    column7.DataType = System.Type.GetType("System.String");
                    column8.DataType = System.Type.GetType("System.String");

                    column1.ReadOnly = true;
                    column2.ReadOnly = true;
                    column3.ReadOnly = true;
                    column5.ReadOnly = true;
                    column6.ReadOnly = true;
                    column7.ReadOnly = true;
                    column8.ReadOnly = true;

                    column1.ColumnName = "Serial Number";
                    column2.ColumnName = "Tunnel";
                    column3.ColumnName = "Resource";
                    column5.ColumnName = "Daycode";
                    column6.ColumnName = "Time";
                    column7.ColumnName = "Weight";
                    column8.ColumnName = "Location";

                    dt.Columns.Add(column1);
                    dt.Columns.Add(column2);
                    dt.Columns.Add(column3);
                    dt.Columns.Add(column5);
                    dt.Columns.Add(column6);
                    dt.Columns.Add(column7);
                    dt.Columns.Add(column8);
                    FreshDataGrid.DataSource = dt;

                    for (int x = 0; x < Lines.Length; x++)
                    {
                        string[] Data = Lines[x].Split(',');
                        row = dt.NewRow();

                        row["Serial Number"] = Data[7].Replace("\"", "");
                        row["Tunnel"] = Data[5].Replace("\"", "");
                        row["Resource"] = Data[2].Replace("\"", "");
                        row["Daycode"] = Data[6].Replace("\"", "");
                        row["Time"] = Data[3].Replace("\"", "");
                        row["Weight"] = Data[0].Replace("\"", "");
                        row["Location"] = Data[13].Replace("\"", "");
                        dt.Rows.Add(row);
                    }
                    FreshDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    FreshDataGrid.Sort(this.FreshDataGrid.Columns["Tunnel"], ListSortDirection.Ascending);
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
        private void DailyLogDisplay_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void PrintBacktiButton_Click(object sender, EventArgs e)
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            string Path3 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Formats\\" + "BactiSticker" + ".nlbl";
            Path3 = Path3.Replace("\r", "").Replace("\n", "");

            ILabel label = PrintEngineFactory.PrintEngine.OpenLabel(Path3);
            label.PrintSettings.PrinterName = settings[17];

            string expath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports\\" + "PATTYN" + CodeReturnGenerator("ddd-yy", "0", 0) + ".txt";
            expath = expath.Replace("\r", "").Replace("\n", "");

            if (File.Exists(expath))
            {
                string[] Lines = File.ReadAllLines(expath);

                bool found = false; 
                for (int x = 0; x < Lines.Length; x++)
                {
                    string[] Data = Lines[x].Split(',');
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
                        label.Variables["SERIAL_NUMBER"].SetValue(Data[7].Replace("\"", "").Replace(" ", ""));
                        label.Variables["Line"].SetValue(Data[5].Replace("\"", "").Replace(" ", ""));
                        label.Variables["Resource"].SetValue(Data[2].Replace("\"", "").Replace(" ", ""));
                        label.Variables["Description"].SetValue(Description);
                        label.Variables["Daycode"].SetValue(Data[6].Replace("\"", "").Replace(" ", ""));
                        label.Variables["Time"].SetValue(Data[3].Replace("\"", "").Replace(" ", ""));

                        var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                        LogUserActivity LUA = new LogUserActivity();
                        LUA.LogActivity(pf._User, "Printed bacti sticker for: " + SerialEntryBox.Text);
                        found = true;
                        label.Print(4);
                        break;
                    }
                    
                }
                if(!found)
                {  MessageBox.Show("Could not find " + SerialEntryBox.Text + " in the list");  }
            }
        }

        #region datecode generation
        //returns the smnith code with the alpha code on the front
        string daycodeGenerator(string LineNumber)
        {
            string thereturn = "";
            DateTime now = DateTime.Now;
            thereturn = twohouralpha(now);
            thereturn = thereturn + JulianDateGenerator(now);
            thereturn = thereturn + DateTime.Now.ToString("y").Substring(DateTime.Now.ToString("y").Length - 1);
            thereturn = thereturn + LineNumber;
            return thereturn;
        }
        //returns the smith date code (dddyLL) with no leading alpha for the lot
        string NoAlphaDaycode(int LineNumber)
        {
            string thereturn = "";
            DateTime now = DateTime.Now;
            thereturn = thereturn + JulianDateGenerator(now);
            thereturn = thereturn + DateTime.Now.ToString("y").Substring(DateTime.Now.ToString("y").Length - 1);
            thereturn = thereturn + "0" + LineNumber.ToString();
            return thereturn;
        }
        //Accepts a string such as "dddyyyy" or a dd/mm/yyE and the postmonths which can be 0 and returns the appropriately formatted daycode using supporting 
        //functions
        public string CodeReturnGenerator(string s, string exp, int printer)
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            string thereturn = "";
            char[] code = s.ToCharArray();
            if (s != "")
            {
                if (s.Substring(s.Length - 1) != "E")
                {
                    for (int f = 0; f < code.Length; f++)
                    {
                        DateTime now = DateTime.Now;
                        if (now.Hour <= Int32.Parse(settings[1]))
                        { now = now.AddDays(-1); }
                        #region Days
                        if (code[f] == 'd')
                        {
                            #region two d two digit day
                            if (f + 1 < code.Length)
                            {
                                if (code[f + 1] == 'd')
                                {
                                    #region Three d three digit day
                                    if (f + 2 < code.Length)
                                    {
                                        if (code[f + 2] == 'd')
                                        {
                                            thereturn = thereturn + JulianDateGenerator(now);
                                            f = f + 2;
                                        }
                                        #endregion
                                        else
                                        {
                                            if (now.Day.ToString().Length > 1)
                                            { thereturn = thereturn + now.Day.ToString(); }
                                            else { thereturn = thereturn + "0" + now.Day.ToString(); }
                                            f = f + 1;
                                        }
                                    }
                                    else
                                    {
                                        if (now.Day.ToString().Length > 1)
                                        { thereturn = thereturn + now.Day.ToString(); }
                                        else { thereturn = thereturn + "0" + now.Day.ToString(); }
                                        f = f + 1;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    thereturn = thereturn + DateTime.Now.ToString("dd").Substring(DateTime.Now.ToString("dd").Length - 1);
                                }
                            }
                            else
                            {
                                thereturn = thereturn + DateTime.Now.ToString("dd").Substring(DateTime.Now.ToString("dd").Length - 1);
                            }
                        }
                        #endregion
                        #region Years
                        else if (code[f] == 'y')
                        {
                            #region two y two digit year
                            if (f + 1 < code.Length)
                            {
                                if (code[f + 1] == 'y')
                                {
                                    #region Three y three digit year
                                    if (f + 2 < code.Length)
                                    {
                                        if (code[f + 2] == 'y')
                                        {
                                            #region four y four digit year
                                            if (f + 3 < code.Length)
                                            {
                                                if (code[f + 3] == 'y')
                                                {
                                                    thereturn = thereturn + DateTime.Now.ToString("yyyy");
                                                    f = f + 3;
                                                }
                                                #endregion
                                                else
                                                {
                                                    thereturn = thereturn + DateTime.Now.ToString("yyy").Substring(DateTime.Now.ToString("yyy").Length - 3);
                                                    f = f + 2;
                                                }
                                            }
                                            else
                                            {
                                                thereturn = thereturn + DateTime.Now.ToString("yyy").Substring(DateTime.Now.ToString("yyy").Length - 3);
                                                f = f + 2;
                                            }
                                        }
                                        #endregion
                                        else
                                        {
                                            thereturn = thereturn + DateTime.Now.ToString("yy");
                                            f = f + 1;
                                        }

                                    }
                                    else
                                    {
                                        thereturn = thereturn + DateTime.Now.ToString("yy");
                                        f = f + 1;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    thereturn = thereturn + DateTime.Now.ToString("y").Substring(DateTime.Now.ToString("y").Length - 1);
                                }
                            }
                            else
                            {
                                thereturn = thereturn + DateTime.Now.ToString("y").Substring(DateTime.Now.ToString("y").Length - 1);
                            }
                        }
                        #endregion
                        #region DayOfWeek
                        else if (code[f] == 'w')
                        {
                            #region Abbreviated day of the week
                            if (f + 1 < code.Length)
                            {
                                if (code[f + 1] == 'w')
                                {
                                    thereturn = thereturn + now.DayOfWeek.ToString().Substring(0, 3);
                                    f = f + 1;
                                }
                                #endregion
                                else
                                {
                                    thereturn = thereturn + now.DayOfWeek.ToString();
                                }
                            }
                            else
                            {
                                thereturn = thereturn + now.DayOfWeek.ToString();
                            }

                        }
                        #endregion
                        #region Month
                        #region Single digit month
                        else if (code[f] == 'm')
                        {
                            #region 2 digit month
                            if (f + 1 < code.Length)
                            {
                                if (code[f + 1] == 'm')
                                {
                                    #region 3 char abbreviation
                                    if (f + 2 < code.Length)
                                    {
                                        if (code[f + 2] == 'm')
                                        {
                                            thereturn = thereturn + now.ToString("MMM");
                                            f = f + 2;
                                        }
                                        #endregion
                                        else
                                        {
                                            if (now.Month.ToString().Length > 1)
                                            { thereturn = thereturn + now.Month.ToString(); }
                                            else { thereturn = thereturn + "0" + now.Month.ToString(); }
                                            f = f + 1;
                                        }
                                    }
                                    else
                                    {
                                        if (now.Month.ToString().Length > 1)
                                        { thereturn = thereturn + now.Month.ToString(); }
                                        else { thereturn = thereturn + "0" + now.Month.ToString(); }
                                        f = f + 1;
                                    }
                                }
                                #endregion
                                else
                                {
                                    thereturn = thereturn + now.Month.ToString().Substring(now.Month.ToString().Length - 1);
                                }
                            }
                            else
                            {
                                thereturn = thereturn + now.Month.ToString().Substring(now.Month.ToString().Length - 1);
                            }
                        }
                        #endregion
                        #region Two Char Month 
                        else if (code[f] == 'M')
                        {
                            thereturn = thereturn + twocharmonth(now);
                        }
                        #endregion
                        #endregion
                        #region  Two hour alpha code
                        else if (code[f] == 'a')
                        {
                            thereturn = thereturn + twohouralpha(now);
                        }
                        #endregion
                        #region Line Number
                        else if (code[f] == 'L')
                        {
                            if (f + 1 < code.Length)
                            {
                                if (code[f + 1] == 'L')
                                {
                                    thereturn = thereturn + LineNumber(printer);
                                    f = f + 1;
                                }
                            }
                            else
                            {
                                thereturn = thereturn + 'L';
                            }
                        }
                        #endregion
                        else if (s[f] != 'E')
                        {
                            thereturn = thereturn + s[f];
                        }
                    }
                }
                else if (s.Substring(s.Length - 1) == "E")
                {
                    for (int f = 0; f < code.Length; f++)
                    {
                        DateTime now = DateTime.Now.AddMonths(Int32.Parse(exp));
                        if (now.Hour <= Int32.Parse(settings[1]))
                        { now = now.AddDays(-1); }
                        #region Days
                        if (code[f] == 'd')
                        {
                            #region two d two digit day
                            if (f + 1 < code.Length)
                            {
                                if (code[f + 1] == 'd')
                                {
                                    #region Three d three digit day
                                    if (f + 2 < code.Length)
                                    {
                                        if (code[f + 2] == 'd')
                                        {
                                            thereturn = thereturn + JulianDateGenerator(now);
                                            f = f + 2;
                                        }
                                        #endregion
                                        else
                                        {
                                            if (now.Day.ToString().Length > 1)
                                            { thereturn = thereturn + now.Day.ToString(); }
                                            else { thereturn = thereturn + "0" + now.Day.ToString(); }
                                            f = f + 1;
                                        }
                                    }
                                    else
                                    {
                                        if (now.Day.ToString().Length > 1)
                                        { thereturn = thereturn + now.Day.ToString(); }
                                        else { thereturn = thereturn + "0" + now.Day.ToString(); }
                                        f = f + 1;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    thereturn = thereturn + DateTime.Now.ToString("dd").Substring(DateTime.Now.ToString("dd").Length - 1);
                                }
                            }
                            else
                            {
                                thereturn = thereturn + DateTime.Now.ToString("dd").Substring(DateTime.Now.ToString("dd").Length - 1);
                            }
                        }
                        #endregion
                        #region Years
                        else if (code[f] == 'y')
                        {
                            #region two y two digit year
                            if (f + 1 < code.Length)
                            {
                                if (code[f + 1] == 'y')
                                {
                                    #region Three y three digit year
                                    if (f + 2 < code.Length)
                                    {
                                        if (code[f + 2] == 'y')
                                        {
                                            #region four y four digit year
                                            if (f + 3 < code.Length)
                                            {
                                                if (code[f + 3] == 'y')
                                                {
                                                    thereturn = thereturn + now.ToString("yyyy");
                                                    f = f + 3;
                                                }
                                                #endregion
                                                else
                                                {
                                                    thereturn = thereturn + now.ToString("yyy").Substring(now.ToString("yyy").Length - 3);
                                                    f = f + 2;
                                                }
                                            }
                                            else
                                            {
                                                thereturn = thereturn + now.ToString("yyy").Substring(now.ToString("yyy").Length - 3);
                                                f = f + 2;
                                            }
                                        }
                                        #endregion
                                        else
                                        {
                                            thereturn = thereturn + now.ToString("yy");
                                            f = f + 1;
                                        }

                                    }
                                    else
                                    {
                                        thereturn = thereturn + now.ToString("yy");
                                        f = f + 1;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    thereturn = thereturn + now.ToString("y").Substring(now.ToString("y").Length - 1);
                                }
                            }
                            else
                            {
                                thereturn = thereturn + now.ToString("y").Substring(now.ToString("y").Length - 1);
                            }
                        }
                        #endregion
                        #region DayOfWeek
                        else if (code[f] == 'w')
                        {
                            #region Abbreviated day of the week
                            if (f + 1 < code.Length)
                            {
                                if (code[f + 1] == 'w')
                                {
                                    thereturn = thereturn + now.DayOfWeek.ToString().Substring(0, 3);
                                    f = f + 1;
                                }
                                #endregion
                                else
                                {
                                    thereturn = thereturn + now.DayOfWeek.ToString();
                                }
                            }
                            else
                            {
                                thereturn = thereturn + now.DayOfWeek.ToString();
                            }

                        }
                        #endregion
                        #region Month
                        #region Single digit month
                        else if (code[f] == 'm')
                        {
                            #region 2 digit month
                            if (f + 1 < code.Length)
                            {
                                if (code[f + 1] == 'm')
                                {
                                    #region 3 char abbreviation
                                    if (f + 2 < code.Length)
                                    {
                                        if (code[f + 2] == 'm')
                                        {
                                            thereturn = thereturn + now.ToString("MMM");
                                            f = f + 2;
                                        }
                                        #endregion
                                        else
                                        {
                                            if (now.Month.ToString().Length > 1)
                                            { thereturn = thereturn + now.Month.ToString(); }
                                            else { thereturn = thereturn + "0" + now.Month.ToString(); }
                                            f = f + 1;
                                        }
                                    }
                                    else
                                    {
                                        if (now.Month.ToString().Length > 1)
                                        { thereturn = thereturn + now.Month.ToString(); }
                                        else { thereturn = thereturn + "0" + now.Month.ToString(); }
                                        f = f + 1;
                                    }
                                }
                                #endregion
                                else
                                {
                                    thereturn = thereturn + now.Month.ToString().Substring(now.Month.ToString().Length - 1);
                                }
                            }
                            else
                            {
                                thereturn = thereturn + now.Month.ToString().Substring(now.Month.ToString().Length - 1);
                            }
                        }
                        #endregion
                        #region Two Char Month 
                        else if (code[f] == 'M')
                        {
                            thereturn = thereturn + twocharmonth(now);
                        }
                        #endregion
                        #endregion
                        #region  Two hour alpha code
                        else if (code[f] == 'a')
                        {
                            thereturn = thereturn + twohouralpha(now);
                        }
                        #endregion
                        #region Line Number
                        else if (code[f] == 'L')
                        {
                            if (f + 1 < code.Length)
                            {
                                if (code[f + 1] == 'L')
                                {
                                    thereturn = thereturn + LineNumber(printer);
                                    f = f + 1;
                                }
                            }
                            else
                            {
                                thereturn = thereturn + 'L';
                            }
                        }
                        #endregion
                        else if (s[f] != 'E')
                        {
                            thereturn = thereturn + s[f];
                        }
                    }
                }

            }
            return thereturn;
        }
        //collects and returns the line number associated with the printer that the request is coming from.
        public string LineNumber(int x)
        {
            string path = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\PrinterConfigFile.txt";
            path = path.Replace("\r", "").Replace("\n", "");
            string[] printers = File.ReadAllText(path).Split(',');

            return printers[x * 4 + 3].PadLeft(2, '0');
        }
        //This is my custom Julian date return simply returns a correctly formatted string containg the current Julian date
        public string JulianDateGenerator(DateTime now)
        {
            string Julian = "";
            int add;
            #region Not a leap Year
            if (DateTime.IsLeapYear(now.Year) == false)
            {
                if (now.Month == 1)
                {
                    Julian = now.Day.ToString();
                }
                else if (now.Month == 2)
                {
                    add = 31 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 3)
                {
                    add = 59 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 4)
                {
                    add = 90 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 5)
                {
                    add = 120 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 6)
                {
                    add = 151 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 7)
                {
                    add = 181 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 8)
                {
                    add = 212 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 9)
                {
                    add = 243 + now.Day;
                    Julian = add.ToString();

                }
                else if (now.Month == 10)
                {
                    add = 273 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 11)
                {
                    add = 304 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 12)
                {
                    add = 334 + now.Day;
                    Julian = add.ToString();
                }
            }
            #endregion
            #region Is a Leap Year
            else if (DateTime.IsLeapYear(now.Year) == true)
            {
                if (now.Month == 1)
                {
                    Julian = now.Day.ToString();
                }
                else if (now.Month == 2)
                {
                    add = 31 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 3)
                {
                    add = 60 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 4)
                {
                    add = 91 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 5)
                {
                    add = 121 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 6)
                {
                    add = 152 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 7)
                {
                    add = 182 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 8)
                {
                    add = 213 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 9)
                {
                    add = 244 + now.Day;
                    Julian = add.ToString();

                }
                else if (now.Month == 10)
                {
                    add = 274 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 11)
                {
                    add = 305 + now.Day;
                    Julian = add.ToString();
                }
                else if (now.Month == 12)
                {
                    add = 335 + now.Day;
                    Julian = add.ToString();
                }
            }
            #endregion
            return Julian.PadLeft(3, '0'); ;
        }
        //Returns a string for the abbreviated two character month
        public string twocharmonth(DateTime now)
        {
            string thereturn = "";
            if (now.Month == 1)
            {
                thereturn = "JA";
            }
            else if (now.Month == 2)
            {
                thereturn = "FE";
            }
            else if (now.Month == 3)
            {
                thereturn = "MR";
            }
            else if (now.Month == 4)
            {
                thereturn = "AL";
            }
            else if (now.Month == 5)
            {
                thereturn = "MA";
            }
            else if (now.Month == 6)
            {
                thereturn = "JN";
            }
            else if (now.Month == 7)
            {
                thereturn = "JL";
            }
            else if (now.Month == 8)
            {
                thereturn = "AU";
            }
            else if (now.Month == 9)
            {
                thereturn = "SE";
            }
            else if (now.Month == 10)
            {
                thereturn = "OC";
            }
            else if (now.Month == 11)
            {
                thereturn = "NO";
            }
            else if (now.Month == 12)
            {
                thereturn = "DE";
            }

            return thereturn;
        }
        //Returns a single Alpha character to represent the hours since production startup skipping 'I' A-M for the whole day at Weston Start time is 7 
        //and Garrett it is 6 but any time can be entered in general settings to make this function change outputs accordingly
        public string twohouralpha(DateTime now)
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            string thereturn = "";
            char letter = 'A';
            int starttime = Int32.Parse(settings[1]);
            int difference = now.Hour - starttime;
            if (difference > -1)
            {
                for (int a = 1; a < difference; a = a + 2)
                {
                    letter++;
                    if (letter == 'I') { letter++; }
                }
            }
            if (difference < 0)
            {
                letter = 'M';
                for (int a = -1; a > difference; a = a - 2)
                {
                    letter--;
                    if (letter == 'I') { letter--; }
                }
            }
            thereturn = letter.ToString();
            #region old way
            //if (now.Hour == 7 || now.Hour == 8)
            //{
            //    thereturn = "A";
            //}
            //else if (now.Hour == 9 || now.Hour == 10)
            //{
            //    thereturn = "B";
            //}
            //else if (now.Hour == 11 || now.Hour == 12)
            //{
            //    thereturn = "C";
            //}
            //else if (now.Hour == 13 || now.Hour == 14)
            //{
            //    thereturn = "D";
            //}
            //else if (now.Hour == 15 || now.Hour == 16)
            //{
            //    thereturn = "E";
            //}
            //else if (now.Hour == 17 || now.Hour == 18)
            //{
            //    thereturn = "F";
            //}
            //else if (now.Hour == 19 || now.Hour == 20)
            //{
            //    thereturn = "G";
            //}
            //else if (now.Hour == 21 || now.Hour == 22)
            //{
            //    thereturn = "H";
            //}
            //else if (now.Hour == 23 || now.Hour == 0)
            //{
            //    thereturn = "J";
            //}
            //else if (now.Hour == 1 || now.Hour == 2)
            //{
            //    thereturn = "K";
            //}
            //else if (now.Hour == 3 || now.Hour == 4)
            //{
            //    thereturn = "L";
            //}
            //else if (now.Hour == 5 || now.Hour == 6)
            //{
            //    thereturn = "M";
            //}
            #endregion
            return thereturn;
        }



        #endregion

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReloadData_Tick(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
