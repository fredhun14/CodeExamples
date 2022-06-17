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
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
namespace FreshPackManifest
{
    public partial class Export : Form
    {
        public Export()
        {
            InitializeComponent();
        }

        private void Export_Load(object sender, EventArgs e)
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            FreshPackPath.Text = settings[8];
        }

        private void Export_Click(object sender, EventArgs e)
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');

            string filename = @"\PATTYN" + CodeReturnGenerator("ddd-yy", "0", 0) + ".txt";
            string SourcePath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports";
            SourcePath = SourcePath.Replace("\r", "").Replace("\n", "");

            string TargetPath = settings[8];

            string SourceFile = SourcePath + filename;
            string DestFile = TargetPath + filename;

            #region Add locations to source file freshpack
            if (File.Exists(SourceFile))
            {
                string[] FreshPackLines = File.ReadAllLines(SourceFile);
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
                                #endregion
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
                File.WriteAllLines(SourceFile, FreshPackLines);
            }
            #endregion
            System.IO.Directory.CreateDirectory(TargetPath);
            Generatedailyreport();
            try
            {
                System.IO.File.Copy(SourceFile, DestFile, true);

                var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                LogUserActivity LUA = new LogUserActivity();
                LUA.LogActivity(pf._User, "Exported " + "PATTYN" + CodeReturnGenerator("ddd-yy", "0", 0) + ".txt");

                MessageBox.Show("Exported " + "PATTYN" + CodeReturnGenerator("ddd-yy", "0", 0) + ".txt" + "Successully");
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("Could not find file"))
                {
                    MessageBox.Show("PATTYN" + CodeReturnGenerator("ddd-yy", "0", 0) + ".txt" + " not found");
                }
                else
                {
                    MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + ex.ToString());
                }
            }
        }
        public class Locations
        {
            public string Serial { get; set; }
            public string PalletLocation { get; set; }
            public string Date_Time { get; set; }
        }
        private void ExportFromPreviousDayFreshPack_Click(object sender, EventArgs e)
        {
            try
            {
                string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
                path11 = path11.Replace("\r", "").Replace("\n", "");
                string[] settings = File.ReadAllText(path11).Split(',');

                //sets filename
                DateTime now = DateTime.Now;
                string filename = "";
                string filepath = "";
                OpenFileDialog Dialog = new OpenFileDialog();
                Dialog.InitialDirectory = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports";
                Dialog.InitialDirectory = Dialog.InitialDirectory.Replace("\r", "").Replace("\n", "");
                Dialog.Title = "Export Select";
                Dialog.DefaultExt = "txt";
                Dialog.Filter = "txt files (*.txt)|*.txt";
                Dialog.FilterIndex = 2;
                Dialog.CheckFileExists = true;
                Dialog.CheckPathExists = true;

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    filename = Dialog.SafeFileName;
                    filepath = Dialog.FileName;
                }
                else
                {
                    MessageBox.Show("Canceled export.");
                    return;
                }
                #region Add locations to source file freshpack
                if (File.Exists(filepath))
                {
                    string[] FreshPackLines = File.ReadAllLines(filepath);
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
                                    #endregion
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
                    File.WriteAllLines(filepath, FreshPackLines);
                }
                #endregion
                //destination path
                string TargetPath = settings[8];

                //destination file path
                string DestFile = TargetPath + "\\" + filename;

                // Create a new target folder.
                // If the directory already exists, this method does not create a new directory.
                System.IO.Directory.CreateDirectory(TargetPath);

                //Copy that day's file to the correct directory
                System.IO.File.Copy(filepath, DestFile, true);

                var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                LogUserActivity LUA = new LogUserActivity();
                LUA.LogActivity(pf._User, "Exported " + filename);

                MessageBox.Show("Exported " + filename + " Successfully");

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Could not find file"))
                {
                    MessageBox.Show("File not found");
                }
                else
                {
                    string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\ErrorLog.txt";
                    path15 = path15.Replace("\r", "").Replace("\n", "");
                    using (StreamWriter w = File.AppendText(path15))
                    {
                        w.WriteLine(" .ToString(): " + ex.ToString() + System.Environment.NewLine + " StackTrace: " + ex.StackTrace.ToString() + System.Environment.NewLine + " MESSAGE: " + ex.Message + System.Environment.NewLine + " Soruce: " + ex.Source);
                    };
                    MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + ex.ToString());
                }
            }
        }

        private void CloseBut_Click(object sender, EventArgs e)
        {
            this.Close();
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
        void Generatedailyreport()
        {
            try
            {
                string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
                path11 = path11.Replace("\r", "").Replace("\n", "");
                string[] settings = File.ReadAllText(path11).Split(',');
                DateTime now = DateTime.Now;
                DateTime Start = new DateTime(now.Year, now.Month, now.Day, Int32.Parse(settings[1]), 0, 0);
                DateTime End = new DateTime(now.Year, now.Month, now.Day, Int32.Parse(settings[1]), 0, 0);
                if (now.Hour < Int32.Parse(settings[1]))
                {
                    Start = Start.AddDays(-1);
                    End = End.AddMilliseconds(-1);
                }
                else
                {
                    End = End.AddDays(1);
                    End = End.AddMilliseconds(-1);
                }
                productionreportdaterange(Start, End);
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
        void productionreportdaterange(DateTime Start, DateTime End)
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            try
            {
                #region FPK side
                #region SQL stuff
                ProductionReport[] AllRecords2 = null;
                //Data Source=SFFNT8;Initial Catalog=ReplacementDB;Persist Security Info=True;User ID=software;Password=***********
                string connString2 = "Data Source=" + settings[18] + ";Initial Catalog=" + settings[21] + ";User ID=" + settings[19] + ";Password=" + settings[20] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString2 = @"SELECT SERIAL_NUMBER,Resource,Time_Date,Net_Weight,Description FROM " + settings[22] + " WHERE Time_Date >= @dtStart AND Time_Date <= @dtEnd;";

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
                            { list.Add(new ProductionReport { Serial = reader.GetString(0), resource = reader.GetString(1), Date_Time = reader.GetDateTime(2), Weight = reader.GetString(3), Description = reader.GetString(4) }); }
                            AllRecords2 = list.ToArray();
                        }
                    }
                }
                #endregion
                string[,] Totals2 = new string[AllRecords2.Length, 4];
                bool flag2 = false;
                for (int x = 0; x < AllRecords2.Length; x++)
                {
                    if (Int32.Parse(AllRecords2[x].Weight) >= 200)
                    {
                        for (int y = 0; y < AllRecords2.Length; y++)
                        {
                            if (AllRecords2[x].resource == Totals2[y, 0])
                            {
                                if (Totals2[y, 1] == null || Totals2[y, 1] == string.Empty || Totals2[y, 1] == "")
                                {
                                    Totals2[y, 1] = "1";
                                    Totals2[y, 2] = AllRecords2[x].Weight;
                                    Totals2[y, 3] = AllRecords2[x].Description;
                                }
                                else
                                {
                                    Totals2[y, 1] = (Int32.Parse(Totals2[y, 1]) + 1).ToString();
                                    Totals2[y, 2] = (Int32.Parse(Totals2[y, 2]) + Int32.Parse(AllRecords2[x].Weight)).ToString();
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
                                        Totals2[z, 2] = AllRecords2[x].Weight;
                                        Totals2[z, 3] = AllRecords2[x].Description;
                                    }
                                    else
                                    {
                                        Totals2[z, 1] = (Int32.Parse(Totals2[z, 1]) + 1).ToString();
                                        Totals2[z, 2] = (Int32.Parse(Totals2[z, 2]) + Int32.Parse(AllRecords2[x].Weight)).ToString();
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
                string prodpath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\Exports\ProductionData" + Start.DayOfYear.ToString().PadLeft(3, '0') + ".txt";
                prodpath = prodpath.Replace("\r", "").Replace("\n", "");

                using (var file = File.CreateText(prodpath))
                {
                    file.WriteLine("Resource".PadRight(19, ' ') + "Description".PadRight(39, ' ') + "Record Count".PadRight(19, ' ') + "Weight");
                    for (int x = 0; x < FPKToFile.Length; x++)
                    {
                        if (FPKToFile[x] == null) { break; }
                        file.WriteLine(FPKToFile[x]);
                    }
                    file.WriteLine("".PadRight(96, '-'));
                    file.WriteLine("Totals: ".PadRight(58, ' ') + Total_pallets2.PadRight(19, ' ') + Total_cases2.PadRight(19, ' '));
                }

                #endregion
                sendemails(prodpath);
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
        void sendemails(string attachpath)
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            try
            {
                var smtpClient = new SmtpClient(settings[9])//"smtp.gmail.com"
                {
                    Port = 25,
                    Credentials = new NetworkCredential(settings[10], settings[11]), //sending email/password
                    EnableSsl = false,
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("ProductionReports@smithfrozenfoods.com"),
                    Subject = "Tote Line Production report " + DateTime.Now.ToString(),
                    Body = "<h1>Attached: </h1>",
                    IsBodyHtml = true,
                };
                int r = 0;
                if (settings[12] != "" && settings[12] != null && settings[12] != string.Empty)
                {
                    mailMessage.To.Add(settings[12]);
                    r++;
                }
                if (settings[13] != "" && settings[13] != null && settings[13] != string.Empty)
                {
                    mailMessage.To.Add(settings[13]);
                    r++;
                }
                if (settings[14] != "" && settings[14] != null && settings[14] != string.Empty)
                {
                    mailMessage.To.Add(settings[14]);
                    r++;
                }
                var attachment = new Attachment(attachpath, MediaTypeNames.Text.Plain);
                mailMessage.Attachments.Add(attachment);
                if (r > 0)
                {
                    smtpClient.Send(mailMessage);
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
        public class ProductionReport
        {
            public string Serial { get; set; }
            public string resource { get; set; }
            public DateTime Date_Time { get; set; }
            public string Weight { get; set; }
            public string Description { get; set; }
        }
    }
}
