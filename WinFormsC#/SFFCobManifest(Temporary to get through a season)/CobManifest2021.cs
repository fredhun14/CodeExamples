//Written by Michael Hunsaker Waiting for deployment and testing 12/22/2020
//To replace the old Manifest program currently running on a Windows 2000 Computer
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
    public partial class CobManifest2021 : Form
    {
        public CobManifest2021()
        {
            InitializeComponent();
        }
        #region For Security
        // this variable is used to determine which controls appear and are therefore usable by the user. It is assigined when the user logs in
        private int SecLevel = 0;
        public int _SecLevel
        {
            get
            {
                return SecLevel;
            }
            set
            {
                if (SecLevel != value)
                    SecLevel = value;
            }
        }
        #endregion
        #region Primary Form Load
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
                {
                    MessageBox.Show("The program is already running");
                    this.Close();
                }
                if (!File.Exists("SerialNumber.txt"))
                {
                    using (StreamWriter w = File.AppendText("SerialNumber.txt"))
                    {
                        w.WriteLine("00000000");
                    };
                }
                String SerialNumber = System.IO.File.ReadAllText("SerialNumber.txt");
                SerialNumberLabel.Text = SerialNumber;
                ABC.SelectedIndex = 0;
                D.SelectedIndex = 0;
                E.SelectedIndex = 0;
                FGH.SelectedIndex = 0;
                I.SelectedIndex = 0;
                J.SelectedIndex = 0;
                K.SelectedIndex = 0;
                string path7 = Application.StartupPath + "\\security.txt";
                path7 = path7.Replace("\r", "").Replace("\n", "");
                if (!File.Exists(path7))
                {
                    using (StreamWriter w = File.AppendText(path7))
                    {
                        //Password defaults
                        //admin,Admin
                        //user.user
                        //manager,manager
                        w.WriteLine("firns,e3afed0047b08059d0fada10f400c1e5,10");//Admin
                        w.WriteLine(",zxjw,ee11cbb19052e40b07aac0ca060c23ee,1");//user
                        w.WriteLine(",rfsfljw,1d0258c2440a8d19e716292b231e3190,5");//manager
                    }
                }
                string path = Application.StartupPath + "\\resources.txt";
                path = path.Replace("\r", "").Replace("\n", "");
                if (!File.Exists(path))
                {
                    using (StreamWriter w = File.AppendText(path))
                    {
                        w.WriteLine("416,417,423,424,433,435,474,483,485,488,234,230");
                        w.WriteLine("1,3,4,6,7,1");
                        w.WriteLine("0,1,3,5,7,8,9");
                        w.WriteLine("1003,1012,1035,1042,1233,1133");
                        w.WriteLine("0,1,4,7");
                        w.WriteLine("0,1,2");
                        w.WriteLine("0,2,4,7,8,9,A,B,G,H,J,K,L,M,O,R,U,Y,Z,X");
                    }
                }
                LoadResources();
                if (File.Exists("SaveTemp.txt"))
                {
                    load();
                }
                daycodeGenerator();
                login();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tell the IT department you have seen this error\n" + ex.ToString());
            }
        }
        #endregion
        #region Print
        private void Line1PrintButton_Click(object sender, EventArgs e)
        {
            try
            {
                //Disable the print buttons for a few seconds.
                DisablePrint();
                ButtonTimoutTimer.Enabled = true;
                daycodeGenerator();
                //Set the time
                DateTime now = DateTime.Now;
                //Creates the file/appends information to the dates file
                string path = DaycodeReturn() + "CobCorn" + ".txt";
                string newline = "";

                newline = WeightTextBox.Value.ToString().PadLeft(5, ' ') + ",00000," + ResNoTextBox.Text.PadRight(16, ' ') + ",\"" + now.ToString("HH:mm") + " " + now.ToString("tt").ToLower() + "\",\"" + now.ToString("MM/dd/yy") + "\"," + LineNumber.Text + ",\"" + DayCodeTextBox.Text + "\"," + SerialNumberLabel.Text + ",\"\",\"TOTECO    \",\"" + Remarks1TextBox.Text + "\",\"" + Remarks2TextBox.Text + "\",\"" + Remarks3TextBox.Text + "\",\"" + "\",\"\"";

                //This will append to an exsisting file or create it if it doesnt exsist and right the first line
                using (StreamWriter w = File.AppendText(path))
                {
                    w.WriteLine(newline);
                };
                //*************************************************************************************Start Print Function********************************************************

                // the path to the print database and the correct data in the correct format for bartender to use it as a datasource
                string path2 = "PrintDatabase.txt";
                string newline2 = "\"" + DayCodeTextBox.Text + "\",\"" + now.ToString("HH:mm") + "\",\"" + now.ToString("MM/dd/yy") + "\",\"" + BuyerTextBox.Text + "\",\"" +
                    ResNoTextBox.Text + "\",\"" + DescriptionTextBox.Text + "\"," + CasesTextBox.Text + "," + WeightTextBox.Text + "," + OrigSerNoTextBox.Text + "," +
                    CompSerNoTextBox.Text + ",\"" + Remarks1TextBox.Text + "\",\"" + Remarks2TextBox.Text + "\",\"" + Remarks3TextBox.Text + "\",\"" + SerialNumberLabel.Text + "\"";

                //if the file exsists overwrite it with newline if not create it and then overwrite it with new 
                //Instantly (fast enough you never see the file in file explorer) Commander will send the data along and delete the file

                FileStream fcreate = File.Open(path2, FileMode.Create);
                fcreate.Close();
                System.IO.File.WriteAllText(path2, newline2);

                //**********************************************************************************************End Print function
                //Increment the serial Number for each printed label
                int serialnumber = int.Parse(SerialNumberLabel.Text);
                SerialNumberLabel.Text = (serialnumber + 1).ToString();
                //Ensure the Serial number stays a consistant length currently 9 must be changed on the if and the while if needed to be changed.
                do
                {
                    if (SerialNumberLabel.Text.Length < 8) { SerialNumberLabel.Text = '0' + SerialNumberLabel.Text; }
                }
                while (SerialNumberLabel.Text.Length < 8);
                //Overwrite the serial number file so that each time the program is opened it starts with the next serial number in line.
                System.IO.File.WriteAllText("SerialNumber.txt", SerialNumberLabel.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tell the IT department you have seen this error\n" + ex.ToString());
            }
        }
        #endregion
        #region Export
        private void ExportButton_Click(object sender, EventArgs e)
        {
            try
            {
                //sets filename
                DateTime now = DateTime.Now;
                string filename = ExportDaycodeReturn() + "CobCorn" + ".txt";
                //makes sure file exsists
                if (!File.Exists(filename))
                {
                    using (StreamWriter w = File.AppendText(filename)) { w.WriteLine(""); };
                }
                #region Sorting
                //SORTING
                //string[] lines = File.ReadAllLines(filename);
                //string[,] sortthis = new string[File.ReadLines(filename).Count(), 20];
                //string[] split = new string[20];
                //for (int r = 0; r < lines.Length; r++)
                //{
                //    split = lines[r].Split(',');
                //    for (int c = 0; c < 20; c++)
                //    {
                //        sortthis[r, c] = split[c];
                //    }
                //}
                //for (int x = 0; x < lines.Length; x++)
                //{
                //    for (int s = 0; s < lines.Length - 1; s++)
                //    {
                //        if (sortthis[s, 12].CompareTo(sortthis[s + 1, 12]) < 0)
                //        {
                //            for (int f = 0; f < 20; f++)
                //            {
                //                split[f] = sortthis[s + 1, f];
                //            }
                //            for (int f = 0; f < 20; f++)
                //            {
                //                sortthis[s + 1, f] = sortthis[s, f];
                //            }
                //            for (int f = 0; f < 20; f++)
                //            {
                //                sortthis[s, f] = split[f];
                //            }
                //        }

                //    }
                //}
                //for (int l = 0; l < lines.Length; l++)
                //{
                //    lines[l] = sortthis[l, 0] + "," + sortthis[l, 1] + "," + sortthis[l, 2] + "," + sortthis[l, 3] + "," + sortthis[l, 4] + "," + sortthis[l, 5] +
                //        "," + sortthis[l, 6] + "," + sortthis[l, 7] + "," + sortthis[l, 8] + "," + sortthis[l, 9] + "," + sortthis[l, 10] + "," +
                //        sortthis[l, 11] + "," + sortthis[l, 12] + "," + sortthis[l, 13] + "," + sortthis[l, 14] + "," + sortthis[l, 15] + "," + sortthis[l, 16] +
                //        "," + sortthis[l, 17] + "," + sortthis[l, 18] + "," + sortthis[l, 19];
                //}

                //File.WriteAllText(filename, String.Empty);
                //for (int l = 0; l < lines.Length; l++)
                //{
                //    using (StreamWriter w = File.AppendText(filename))
                //    {
                //        w.WriteLine(lines[l]);
                //    };
                //}

                //END SORTING
                #endregion
                //source and destination paths
                string SourcePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string TargetPath = @"\\sffvsfs\fill";

                //source and destination file paths
                String SourceFile = System.IO.Path.Combine(SourcePath, filename);
                string DestFile = System.IO.Path.Combine(TargetPath, filename);

                // Create a new target folder.
                // If the directory already exists, this method does not create a new directory.
                System.IO.Directory.CreateDirectory(TargetPath);

                //Copy that day's file to the correct directory
                System.IO.File.Copy(SourceFile, DestFile, true);
                MessageBox.Show("Exported Successfully: " + DaycodeReturn());
                ExportLabel.Text = "Last Exported: " + DaycodeReturn();
                save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tell the IT department you have seen this error\n" + ex.ToString());
            }
        }
        private void exportFromPreviousDayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //sets filename
                DateTime now = DateTime.Now;
                string filename = "";
                OpenFileDialog Dialog = new OpenFileDialog();
                Dialog.InitialDirectory = @"C:";
                Dialog.Title = "Export Select";
                Dialog.DefaultExt = "txt";
                Dialog.Filter = "txt files (*.txt)|*.txt";
                Dialog.FilterIndex = 2;
                Dialog.CheckFileExists = true;
                Dialog.CheckPathExists = true;

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    filename = Dialog.SafeFileName;
                }
                #region Sorting
                //SORTING
                //string[] lines = File.ReadAllLines(filename);
                //string[,] sortthis = new string[File.ReadLines(filename).Count(), 20];
                //string[] split = new string[20];
                //for (int r = 0; r < lines.Length; r++)
                //{
                //    split = lines[r].Split(',');
                //    for (int c = 0; c < 20; c++)
                //    {
                //        sortthis[r, c] = split[c];
                //    }
                //}
                //for (int x = 0; x < lines.Length; x++)
                //{
                //    for (int s = 0; s < lines.Length - 1; s++)
                //    {
                //        if (sortthis[s, 12].CompareTo(sortthis[s + 1, 12]) < 0)
                //        {
                //            for (int f = 0; f < 20; f++)
                //            {
                //                split[f] = sortthis[s + 1, f];
                //            }
                //            for (int f = 0; f < 20; f++)
                //            {
                //                sortthis[s + 1, f] = sortthis[s, f];
                //            }
                //            for (int f = 0; f < 20; f++)
                //            {
                //                sortthis[s, f] = split[f];
                //            }
                //        }

                //    }
                //}
                //for (int l = 0; l < lines.Length; l++)
                //{
                //    lines[l] = sortthis[l, 0] + "," + sortthis[l, 1] + "," + sortthis[l, 2] + "," + sortthis[l, 3] + "," + sortthis[l, 4] + "," + sortthis[l, 5] +
                //        "," + sortthis[l, 6] + "," + sortthis[l, 7] + "," + sortthis[l, 8] + "," + sortthis[l, 9] + "," + sortthis[l, 10] + "," +
                //        sortthis[l, 11] + "," + sortthis[l, 12] + "," + sortthis[l, 13] + "," + sortthis[l, 14] + "," + sortthis[l, 15] + "," + sortthis[l, 16] +
                //        "," + sortthis[l, 17] + "," + sortthis[l, 18] + "," + sortthis[l, 19];
                //}

                //File.WriteAllText(filename, String.Empty);
                //for (int l = 0; l < lines.Length; l++)
                //{
                //    using (StreamWriter w = File.AppendText(filename))
                //    {
                //        w.WriteLine(lines[l]);
                //    };
                //}

                //END SORTING
                #endregion
                //source and destination paths
                string SourcePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string TargetPath = @"\\sffvsfs\fill";

                //source and destination file paths
                String SourceFile = System.IO.Path.Combine(SourcePath, filename);
                string DestFile = System.IO.Path.Combine(TargetPath, filename);

                // Create a new target folder.
                // If the directory already exists, this method does not create a new directory.
                System.IO.Directory.CreateDirectory(TargetPath);

                //Copy that day's file to the correct directory
                System.IO.File.Copy(SourceFile, DestFile, true);
                MessageBox.Show("Exported Successfully: " + DaycodeReturn());
                ExportLabel.Text = "Last Exported: " + DaycodeReturn();
                save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tell the IT department you have seen this error\n" + ex.ToString());
            }
        }
        #endregion
        #region Save and load
        private void save()
        {
            try
            {
                string savepath = "SaveTemp.txt";
                if (!File.Exists(savepath))
                {
                    using (StreamWriter w = File.AppendText(savepath))
                    {
                        w.WriteLine(DayCodeTextBox.Text + "," + TypeComboBox.Text + "," + Remarks1TextBox.Text + "," + DescriptionTextBox.Text + "," + BuyerTextBox.Text + "," + Remarks2TextBox.Text + "," + ResNoTextBox.Text + "," + Remarks3TextBox.Text + "," + OrigSerNoTextBox.Text + "," + CompSerNoTextBox.Text + "," + ExportLabel.Text + "," + ABC.SelectedIndex.ToString() + "," + D.SelectedIndex.ToString() + "," + E.SelectedIndex.ToString() + "," + FGH.SelectedIndex.ToString() + "," + I.SelectedIndex.ToString() + "," + J.SelectedIndex.ToString() + "," + K.SelectedIndex.ToString());
                    };
                }
                else
                {
                    string savethis = DayCodeTextBox.Text + "," + TypeComboBox.Text + "," + Remarks1TextBox.Text + "," + DescriptionTextBox.Text + "," + BuyerTextBox.Text + "," + Remarks2TextBox.Text + "," + ResNoTextBox.Text + "," + Remarks3TextBox.Text + "," + OrigSerNoTextBox.Text + "," + CompSerNoTextBox.Text + "," + ExportLabel.Text + "," + ABC.SelectedIndex.ToString() + "," + D.SelectedIndex.ToString() + "," + E.SelectedIndex.ToString() + "," + FGH.SelectedIndex.ToString() + "," + I.SelectedIndex.ToString() + "," + J.SelectedIndex.ToString() + "," + K.SelectedIndex.ToString();
                    FileStream fcreate = File.Open(savepath, FileMode.Create);
                    fcreate.Close();
                    System.IO.File.WriteAllText(savepath, savethis);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tell the IT department you have seen this error\n" + ex.ToString());
            }
        }
        private void load()
        {
            try
            {
                string savepath = "SaveTemp.txt";
                if (File.Exists(savepath))
                {
                    String SavedData = System.IO.File.ReadAllText(savepath);
                    string[] Splitdata = SavedData.Split(',');
                    //line1
                    DayCodeTextBox.Text = Splitdata[0];
                    TypeComboBox.Text = Splitdata[1];
                    Remarks1TextBox.Text = Splitdata[2];
                    DescriptionTextBox.Text = Splitdata[3];
                    BuyerTextBox.Text = Splitdata[4];
                    Remarks2TextBox.Text = Splitdata[5];
                    ResNoTextBox.Text = Splitdata[6];
                    Remarks3TextBox.Text = Splitdata[7];
                    OrigSerNoTextBox.Text = Splitdata[8];
                    CompSerNoTextBox.Text = Splitdata[9];
                    ExportLabel.Text = Splitdata[10];
                    ABC.SelectedIndex = Int32.Parse(Splitdata[11]);
                    D.SelectedIndex = Int32.Parse(Splitdata[12]);
                    E.SelectedIndex = Int32.Parse(Splitdata[13]);
                    FGH.SelectedIndex = Int32.Parse(Splitdata[14]);
                    I.SelectedIndex = Int32.Parse(Splitdata[15]);
                    J.SelectedIndex = Int32.Parse(Splitdata[16]);
                    K.SelectedIndex = Int32.Parse(Splitdata[17]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tell the IT department you have seen this error\n" + ex.ToString());
            }
        }
        #region save triggers
        private void LineTextBox_TextChanged(object sender, EventArgs e)
        {
            save();
        }

        private void DayCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            save();
        }

        private void TypeComboBox_TextChanged(object sender, EventArgs e)
        {
            save();
        }

        private void Remarks1TextBox_TextChanged(object sender, EventArgs e)
        {
            save();
        }

        private void DescriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            save();
        }

        private void BuyerTextBox_TextChanged(object sender, EventArgs e)
        {
            save();
        }

        private void Remarks2TextBox_TextChanged(object sender, EventArgs e)
        {
            save();
        }

        private void TotalWeightTextBox_TextChanged(object sender, EventArgs e)
        {
            save();
        }

        private void TotalCasesTextBox_TextChanged(object sender, EventArgs e)
        {
            save();
        }

        private void ResNoTextBox_TextChanged(object sender, EventArgs e)
        {
            save();
        }

        private void YUMProdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            save();
        }

        private void Remarks3TextBox_TextChanged(object sender, EventArgs e)
        {
            save();
        }

        private void CasesTextBox_ValueChanged(object sender, EventArgs e)
        {
            save();
        }

        private void WeightTextBox_ValueChanged(object sender, EventArgs e)
        {
            save();
        }

        private void OrigSerNoTextBox_TextChanged(object sender, EventArgs e)
        {
            save();
        }

        private void CompSerNoTextBox_TextChanged(object sender, EventArgs e)
        {
            save();
        }
        #endregion
        #endregion
        #region Print Button Timeout
        private void DisablePrint()
        {
            try
            {
                Line1PrintButtonMain.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tell the IT department you have seen this error\n" + ex.ToString());
            }
        }
        private void EnablePrint()
        {
            try
            {
                Line1PrintButtonMain.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tell the IT department you have seen this error\n" + ex.ToString());
            }
        }

        private void ButtonTimoutTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                EnablePrint();
                ButtonTimoutTimer.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tell the IT department you have seen this error\n" + ex.ToString());
            }
        }
        #endregion
        #region Daycode
        string DaycodeReturn()
        {
            //NOTE THERE IS NO ALPHA CODE HERE
            string thereturn = "";
            thereturn = thereturn + JulianDateGenerator();
            thereturn = thereturn + DateTime.Now.ToString("y").Substring(DateTime.Now.ToString("y").Length - 1);
            thereturn = thereturn + LineNumber.Text;
            return thereturn;
        }
        string ExportDaycodeReturn()
        {
            //NOTE THERE IS NO ALPHA CODE HERE
            string thereturn = "";
            thereturn = thereturn + ExportJulianDateGenerator();
            thereturn = thereturn + DateTime.Now.ToString("y").Substring(DateTime.Now.ToString("y").Length - 1);
            thereturn = thereturn + LineNumber.Text;
            return thereturn;
        }
        void daycodeGenerator()
        {
            string thereturn = "";
            thereturn = twohouralpha();
            thereturn = thereturn + JulianDateGenerator();
            thereturn = thereturn + DateTime.Now.ToString("y").Substring(DateTime.Now.ToString("y").Length - 1);
            thereturn = thereturn + LineNumber.Text;
            DayCodeTextBox.Text = thereturn;
        }
        public string ExportJulianDateGenerator()
        {
            string Julian = "";
            int add;
            DateTime now = DateTime.Now;
            if (now.Hour <= 8)
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
        public string JulianDateGenerator()
        {
            string Julian = "";
            int add;
            DateTime now = DateTime.Now;
            if (now.Hour <= 7)
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
        public string twohouralpha()
        {
            string thereturn = "";
            DateTime now = DateTime.Now;
            if (now.Hour == 7 || now.Hour == 8)
            {
                thereturn = "A";
            }
            else if (now.Hour == 9 || now.Hour == 10)
            {
                thereturn = "B";
            }
            else if (now.Hour == 11 || now.Hour == 12)
            {
                thereturn = "C";
            }
            else if (now.Hour == 13 || now.Hour == 14)
            {
                thereturn = "D";
            }
            else if (now.Hour == 15 || now.Hour == 16)
            {
                thereturn = "E";
            }
            else if (now.Hour == 17 || now.Hour == 18)
            {
                thereturn = "F";
            }
            else if (now.Hour == 19 || now.Hour == 20)
            {
                thereturn = "G";
            }
            else if (now.Hour == 21 || now.Hour == 22)
            {
                thereturn = "H";
            }
            else if (now.Hour == 23 || now.Hour == 0)
            {
                thereturn = "J";
            }
            else if (now.Hour == 1 || now.Hour == 2)
            {
                thereturn = "K";
            }
            else if (now.Hour == 3 || now.Hour == 4)
            {
                thereturn = "L";
            }
            else if (now.Hour == 5 || now.Hour == 6)
            {
                thereturn = "M";
            }

            return thereturn;
        }
        #endregion

        #region Resource Number
        private void resourcechanged(object sender, EventArgs e)
        {
            try
            {
                if (ABC.SelectedIndex != -1 && D.SelectedIndex != -1 && E.SelectedIndex != -1 && FGH.SelectedIndex != -1 && I.SelectedIndex != -1 && J.SelectedIndex != -1 && K.SelectedIndex != -1)
                {
                    ResNoTextBox.Text = ABC.SelectedItem.ToString() + "-" + D.SelectedItem.ToString() + E.SelectedItem.ToString() + "-" + FGH.SelectedItem.ToString() + "-" + I.SelectedItem.ToString() + J.SelectedItem.ToString() + K.SelectedItem.ToString();
                    save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tell the IT department you have seen this error\n" + ex.ToString());
            }
        }

        #endregion

        void LoadResources()
        {
            string path = Application.StartupPath + "\\resources.txt";
            path = path.Replace("\r", "").Replace("\n", "");

            string[] resources = File.ReadAllLines(path);

            for(int x = 0; x < resources.Length; x++)
            {
                string[] section = resources[x].Split(',');
                switch (x)
                {
                    case 0:
                        ABC.Items.Clear();
                        ABC.Items.AddRange(section);
                        break;
                    case 1:
                        D.Items.Clear();
                        D.Items.AddRange(section);
                        break;
                    case 2:
                        E.Items.Clear();
                        E.Items.AddRange(section);
                        break;
                    case 3:
                        FGH.Items.Clear();
                        FGH.Items.AddRange(section);
                        break;
                    case 4:
                        I.Items.Clear();
                        I.Items.AddRange(section);
                        break;
                    case 5:
                        J.Items.Clear();
                        J.Items.AddRange(section);
                        break;
                    case 6:
                        K.Items.Clear();
                        K.Items.AddRange(section);
                        break;
                }
            }
        }
        void loginsettings()
        {
            switch (SecLevel)
            {
                case 10:
                    //file
                    logoutToolStripMenuItem.Visible = true;
                    //options
                    optionsToolStripMenuItem.Visible = true;
                    manageUsersToolStripMenuItem.Visible = true;
                    configureResourcesToolStripMenuItem.Visible = true;
                    break;
                case 5:
                    //file
                    logoutToolStripMenuItem.Visible = true;
                    //options
                    optionsToolStripMenuItem.Visible = true;
                    manageUsersToolStripMenuItem.Visible = true;
                    configureResourcesToolStripMenuItem.Visible = true;
                    break;
                case 1:
                    //file
                    logoutToolStripMenuItem.Visible = true;
                    //options
                    optionsToolStripMenuItem.Visible = false;
                    manageUsersToolStripMenuItem.Visible = false;
                    configureResourcesToolStripMenuItem.Visible = false;
                    break;
                case 0:
                    //file
                    logoutToolStripMenuItem.Visible = false;
                    //options
                    optionsToolStripMenuItem.Visible = false;
                    manageUsersToolStripMenuItem.Visible = false;
                    configureResourcesToolStripMenuItem.Visible = false;
                    break;
            }

        }
        private void login()
        {
            Login login = new Login();
            var dialogresult = login.ShowDialog();
            loginsettings();
        }
        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            var dialogresult = login.ShowDialog();
            if (dialogresult == DialogResult.OK)
            {
                loginsettings();
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SecLevel = 0;
            loginsettings();
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageUsers mnguser = new ManageUsers();
            mnguser.Show();
        }

        private void configureResourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigureResources config = new ConfigureResources();
            var dialogresult = config.ShowDialog();
            if (dialogresult == DialogResult.OK)
            {
                LoadResources();
            }
        }
    }
}