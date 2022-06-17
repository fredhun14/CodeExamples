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

namespace FreshPackManifest
{
    public partial class VoidTicket : Form
    {
        public VoidTicket()
        {
            InitializeComponent();
        }

        private void VoidButton_Click(object sender, EventArgs e)
        {
            try
            {
                var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                LogUserActivity LUA = new LogUserActivity();
                LUA.LogActivity(pf._User, "Attempted to void " + ToBeVoidBox.Text + " from manifest.");

                #region Text Void
                string expath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports\\" + "PATTYN" + CodeReturnGenerator("ddd-yy", "0", 0) + ".txt";
                expath = expath.Replace("\r", "").Replace("\n", "");

                string[] log = File.ReadAllLines(expath);

                int voidline = -1;
                for(int i = 0; i < log.Length; i++)
                {
                    string[] line = log[i].Split(',');
                    if(line[7] == ToBeVoidBox.Text)
                    {
                        voidline = i;
                        break;
                    }
                }
                if (voidline != -1)
                {

                    string VoidedLog = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports" + @"\" + CodeReturnGenerator("dddyy", "0", 0) + "_VoidLog.txt";
                    VoidedLog = VoidedLog.Replace("\r", "").Replace("\n", "");
                    using (StreamWriter w = File.AppendText(VoidedLog))
                    {
                        w.WriteLine(log[voidline]);
                    }
                    File.WriteAllText(expath, "");
                    for(int i = 0; i < log.Length; i++)
                    {
                        if(i != voidline)
                        {
                            using (StreamWriter w = File.AppendText(VoidedLog))
                            {
                                w.WriteLine(log[i]);
                            }
                        }
                    }
                    MessageBox.Show("Ticket # " + ToBeVoidBox.Text + " Has been found and removed from the manifest text file.");
                }
                else
                {
                    MessageBox.Show("Ticket # " + ToBeVoidBox.Text + " was not found in the manifest text file, and therefore was not removed.");
                }
                #endregion
                #region SQL Void
                string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
                path11 = path11.Replace("\r", "").Replace("\n", "");
                string[] settings = File.ReadAllText(path11).Split(',');
                //Data Source=SFFNT8;Initial Catalog=ReplacementDB;Persist Security Info=True;User ID=software;Password=***********
                string connString = "Data Source=" + settings[18] + ";Initial Catalog=" + settings[21] + ";User ID=" + settings[19] + ";Password=" + settings[20] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString = "DELETE FROM " + settings[22] + " WHERE Serial_Number LIKE '" + ToBeVoidBox.Text + "'";

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

                            MessageBox.Show("Ticket # " + ToBeVoidBox.Text + " If it exsisted in the SQL table, has been found and deleted from " + settings[22]);
                        }
                        catch (Exception f)
                        {
                            string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\ErrorLog.txt";
                            path15 = path15.Replace("\r", "").Replace("\n", "");
                            using (StreamWriter w = File.AppendText(path15))
                            {
                                w.WriteLine(" .ToString(): " + f.ToString() + System.Environment.NewLine + " StackTrace: " + f.StackTrace.ToString() + System.Environment.NewLine + " MESSAGE: " + f.Message + System.Environment.NewLine + " Soruce: " + f.Source);
                            };
                            MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + f.ToString());
                        }
                    }
                }
                #endregion

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

        private void CloseButton_Click(object sender, EventArgs e)
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
    }
}
