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

namespace FreshPackManifest
{
    public partial class DayCodeGenerator : Form
    {
        public DayCodeGenerator()
        {
            InitializeComponent();
        }
        #region Form Load
        //Loads all the saved day codes from the text file and triggers the automatic example outputs for each
        private void DayCodeGenerator_Load(object sender, EventArgs e)
        {
            string path4 = "";
            path4 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\DateCodes" + ".txt";
            path4 = path4.Replace("\r", "").Replace("\n", "");
            string[] data = File.ReadAllText(path4).Split(',');
            DataTable dt = new DataTable();

            DataColumn column1 = new DataColumn(), column2 = new DataColumn(), column3 = new DataColumn();
            DataRow row;
            column1.DataType = System.Type.GetType("System.String");
            column2.DataType = System.Type.GetType("System.String");
            column3.DataType = System.Type.GetType("System.String");
            column1.ColumnName = "Name";
            column2.ColumnName = "Symbology";
            column3.ColumnName = "Result";
            dt.Columns.Add(column1);
            dt.Columns.Add(column2);
            dt.Columns.Add(column3);
            DayCodeGrid.DataSource = dt;
            for (int f = 0; f < data.Length-1; f = f +2)
            {
                row = dt.NewRow();
                row["Name"] = data[f];
                row["Symbology"] = data[f + 1];
                row["Result"] = CodeReturnGenerator(data[f + 1]);
                dt.Rows.Add(row);
            }
            DayCodeGrid.AutoResizeColumn(0);
            DayCodeGrid.AutoResizeColumn(1);
            DayCodeGrid.AutoResizeColumn(2);
            
        }
        #endregion
        #region Triggers/Buttons
        //closes the form without saving any changes made to the date codes
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //This will close the form after saving any changes made to the date codes to the text file
        private void Ok_Click(object sender, EventArgs e)
        {
            string path4 = "";
            path4 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\DateCodes" + ".txt";
            path4 = path4.Replace("\r", "").Replace("\n", "");
            string[,] data = new string[DayCodeGrid.Rows.Count, DayCodeGrid.Columns.Count];

            for (int rows = 0; rows < DayCodeGrid.Rows.Count; rows++)
            {
                for (int col = 0; col < DayCodeGrid.Columns.Count; col++)
                {
                    if (DayCodeGrid.Rows[rows].Cells[col].Value != null && DayCodeGrid.Rows[rows].Cells[col].Value != DBNull.Value && !String.IsNullOrWhiteSpace(DayCodeGrid.Rows[rows].Cells[col].Value.ToString()))
                    {
                        data[rows, col] = DayCodeGrid.Rows[rows].Cells[col].Value.ToString();
                    }
                    else { data[rows, col] = ""; }
                }
            }
            File.WriteAllText(path4, "");
            for (int rows = 0; rows < DayCodeGrid.Rows.Count; rows++)
            {
                for (int col = 0; col < DayCodeGrid.Columns.Count - 1; col++)
                {
                    using (StreamWriter sw = File.AppendText(path4))
                    { sw.Write(data[rows, col] + ","); }
                }
            }
            //removes any white space or extra lines.
            var lines = File.ReadAllLines(path4).Where(arg => !string.IsNullOrWhiteSpace(arg));
            File.WriteAllLines(path4, lines);

            var pf = Application.OpenForms.OfType<PrimaryForm>().First();
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(pf._User, "Modified Daycodes");

            this.Close();
        }
        //Triggers the example date code to be re-evaluated anytime one of the cell in the data grid is changed then resizes the columns automatically 
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            for (int f = 0; f < DayCodeGrid.Rows.Count; f++)
            {
                if (DayCodeGrid.Rows[f].Cells[1].Value != null && DayCodeGrid.Rows[f].Cells[1].Value != DBNull.Value && !String.IsNullOrWhiteSpace(DayCodeGrid.Rows[f].Cells[1].Value.ToString()))
                {
                    DayCodeGrid.Rows[f].Cells[2].Value = CodeReturnGenerator(DayCodeGrid.Rows[f].Cells[1].Value.ToString());
                }
            }
            DayCodeGrid.AutoResizeColumn(0);
            DayCodeGrid.AutoResizeColumn(1);
            DayCodeGrid.AutoResizeColumn(2);
        }
        #endregion
        #region datecode generation
        //returns the full daycode based on the string it is sent calling helper methods when appropriate
        public string CodeReturnGenerator(string s)
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
                            thereturn = thereturn + twohouralpha();
                        }
                        #endregion
                        #region Line Number
                        else if (code[f] == 'L')
                        {
                            if (f + 1 < code.Length)
                            {
                                if (code[f + 1] == 'L')
                                {
                                    thereturn = thereturn + LineNumber();
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
                        DateTime now = DateTime.Now.AddMonths(24);
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
                                                    thereturn = thereturn + DateTime.Now.AddMonths(24).ToString("yyyy");
                                                    f = f + 3;
                                                }
                                                #endregion
                                                else
                                                {
                                                    thereturn = thereturn + DateTime.Now.AddMonths(24).ToString("yyy").Substring(DateTime.Now.AddMonths(24).ToString("yyy").Length - 3);
                                                    f = f + 2;
                                                }
                                            }
                                            else
                                            {
                                                thereturn = thereturn + DateTime.Now.AddMonths(24).ToString("yyy").Substring(DateTime.Now.AddMonths(24).ToString("yyy").Length - 3);
                                                f = f + 2;
                                            }
                                        }
                                        #endregion
                                        else
                                        {
                                            thereturn = thereturn + DateTime.Now.AddMonths(24).ToString("yy");
                                            f = f + 1;
                                        }

                                    }
                                    else
                                    {
                                        thereturn = thereturn + DateTime.Now.AddMonths(24).ToString("yy");
                                        f = f + 1;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    thereturn = thereturn + DateTime.Now.AddMonths(24).ToString("y").Substring(DateTime.Now.AddMonths(24).ToString("y").Length - 1);
                                }
                            }
                            else
                            {
                                thereturn = thereturn + DateTime.Now.AddMonths(24).ToString("y").Substring(DateTime.Now.AddMonths(24).ToString("y").Length - 1);
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
                            thereturn = thereturn + twohouralpha();
                        }
                        #endregion
                        #region Line Number
                        else if (code[f] == 'L')
                        {
                            if (f + 1 < code.Length)
                            {
                                if (code[f + 1] == 'L')
                                {
                                    thereturn = thereturn + LineNumber();
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
        public string LineNumber()
        {
            string path = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\PrinterConfigFile.txt";
            path = path.Replace("\r", "").Replace("\n", "");
            string[] printers = File.ReadAllText(path).Split(',');

            return printers[3];
        }
        // A helper method ment to return the correct Julian date
        public string JulianDateGenerator(DateTime now)
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            string Julian = "";
            int add;
            if (now.Hour <= Int32.Parse(settings[1]))
            { now = now.AddDays(-1); }
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
            return Julian;
        }
        // A helper method ment to return the correct 2 letter abreviation for the month
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
        // A helper method ment to return the Two hour alpha character relevant to the start time for the facility and the current time A-M skipping I 
        //for every two hours
        public string twohouralpha()
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            string thereturn = "";
            char letter = 'A';
            int starttime = Int32.Parse(settings[1]);
            DateTime now = DateTime.Now;
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
        #region for debug
        //used to test the datecode generation
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            if (textBox1.Text != null)
               label1.Text = CodeReturnGenerator(textBox1.Text);
            label2.Text = "";
            label3.Text = "";
            label4.Text = twocharmonth(now);
            label5.Text = twohouralpha();
        }
        #endregion

    }
}