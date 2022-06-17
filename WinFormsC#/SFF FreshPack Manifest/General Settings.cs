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
using OpcLabs.EasyOpc.UA;
using OpcLabs.EasyOpc.UA.OperationModel;

namespace FreshPackManifest
{
    public partial class General_Settings : Form
    {

        //for OPC R/W buttons
        EasyUAClient UAClient;
        public General_Settings()
        {
            InitializeComponent();
            UAClient = new EasyUAClient();
        }
        //used to tell wether we have changed the serial number in this settings menu so that we do not create duplicate serial numbers from saving a then
        //outdated serial number
        bool Serialnumberchanged = false;
        //If a new setting is added both the OK_Button click and the form load here need to have it added accordingly but also the Generate Neccesary files
        //function in the PrimaryForm.cs also needs to have the setting added to the list of defaults for the general settings text file else havoc may ensue
        //It is also reccomended that anytime new settings are added that the general settings textfile is deleted and the program loaded allowing the generate
        //neccesary files function to create it properly again
        #region Button clicks
        //Saves the settings controls details to a single string/line in the general setting text file contains list of which settings are in which order and
        //the number of which to access them if they are split into a string array based on a comma as intended
        //then closes the form
        private void OKbut_Click(object sender, EventArgs e)
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string path10 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\SerialNumber.txt";
            path10 = path10.Replace("\r", "").Replace("\n", "");
            var pf = Application.OpenForms.OfType<PrimaryForm>().First();
            string newline = GenFacilityUPDown.Value.ToString() + "," + GenStartTimeUpDown.Value.ToString() + "," + SQLUALDBName.Text + "," + SQLUALTBName.Text + "," + OPCServerString.Text + "," + OPCServerUsername.Text + "," + OPCServerPassword.Text + "," + OPCErrorFlag.Text + "," + GenExportDest.Text + "," + DESMTPClient.Text + "," + DESendingEmail.Text + "," + DESendingPassword.Text + "," + DERecipient1.Text + "," + DERecipient2.Text + "," + DERecipient3.Text + "," + SQLLocationLogDBName.Text + "," + SQLLocationLogTBName.Text + "," + MPBactiPrinterName.Text + "," + SQLServerName.Text + "," + SQLUserID.Text + "," + SQLUserPassword.Text + "," + SQLDatabaseName.Text + "," + SQLTableName.Text + "," + MPName.Text + "," + LPName.Text + "," + OPCL1License.Text + "," +OPCL1MetalPresent.Text + "," + OPCL1Tare.Text + "," + OPCL1Net.Text + "," + OPCL1DayTime.Text + "," + OPCL1Check.Text + "," + OPCL1Foil.Text + "," + OPCL1FillingToLong.Text + "," + OPCL1LineNumber.Text + ","; 
            //expand the above line for each item add a comma to the end should be simple to extend and add setting like this.
            //Remember to adjust the default settings in generateneccesaryfiles() in the primary form when adding items to the list so that things don't break.
            //Item List:
            /* 0 GEN Facility number
             * 1 GEN Facility Start Time
             * GEN Pallet Serial Number Saved elsewhere
             * 2 SQL User activity log db name
             * 3 SQL User activity log tb name
             * 4 OPC Server String
             * 5 OPC Server username
             * 6 OPC Server Password
             * 7 OPC Error Flag
             * 8 GEN Export Destination
             * 9 DE SMTP Client
             * 10 DE Sending Email
             * 11 DE Sending password
             * 12 DE Recipient 1
             * 13 DE Recipient 2
             * 14 DE Recipient 3
             * 15 SQL Locations DB name
             * 16 SQL Locations TB name
             * 17 MP Bacti Printer Name
             * 18 SQL Server Name
             * 19 SQL User ID
             * 20 SQL User Password
             * 21 SQL Database Name
             * 22 SQL Table Name
             * 23 MP Manifest printer name
             * 24 MP License printer name
             * 25 OPC L1 License Plate Number
             * 26 OPC L1 Metal Present
             * 27 OPC L1 Tare Weight
             * 28 OPC L1 Net Weight
             * 29 OPC L1 Day Time
             * 30 OPC L1 Check Weight
             * 31 OPC L1 Foil Check
             * 32 OPC L1 Filling Too Long
             * 33 OPC L1 Line Number
             * 
             */
            if (Serialnumberchanged)
            {
                File.WriteAllText(path10, GenSerial.Text);
                pf._SerialNumber = GenSerial.Text;
            }
            File.WriteAllText(path11, newline);
            
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(pf._User, "Modified General Settings");
            this.Close();
        }
        //Just closes the form without saving any of the data that has changed or otherwise
        private void Cancelbut_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //All the buttons associates with the two OPC tag tabs including browse and R/W buttons
        private void BrowseServerButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (OPCServerUsername.Text != string.Empty && OPCServerUsername.Text != null && OPCServerString.Text != "" && OPCServerUsername.Text != string.Empty && OPCServerUsername.Text != null && OPCServerUsername.Text != "" && OPCServerPassword.Text != string.Empty && OPCServerPassword.Text != null && OPCServerPassword.Text != "")
                {
                    UAEndpointDescriptor EndpointDescriptor = OPCServerString.Text;
                    EndpointDescriptor.UserName = OPCServerUsername.Text;
                    EndpointDescriptor.Password = OPCServerPassword.Text;
                    EndpointDescriptor.UserIdentity = OpcLabs.BaseLib.IdentityModel.User.UserIdentity.CreateUserNameIdentity(OPCServerUsername.Text, OPCServerPassword.Text);
                    BrowseDialog.EndpointDescriptor = EndpointDescriptor;
                    BrowseDialog.ShowDialog();
                }
                else
                {
                    MessageBox.Show("One or more required strings was missing (Server name, Password, Username)");
                }
            }
            catch (UAException uaException)
            {
                MessageBox.Show("Browse Failed: " + uaException.GetBaseException().Message);
            }
        }
        

        #endregion
        #region Form Load
        // Loads the settings from the text file and assigns their values to the appropriate control
        private void General_Settings_Load(object sender, EventArgs e)
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            var pf = Application.OpenForms.OfType<PrimaryForm>().First();

            GenFacilityUPDown.Value = Int32.Parse(settings[0]);
            GenStartTimeUpDown.Value = Int32.Parse(settings[1]);
            SQLUALDBName.Text = settings[2];
            SQLUALTBName.Text = settings[3];
            OPCServerString.Text = settings[4];
            OPCServerUsername.Text = settings[5];
            OPCServerPassword.Text = settings[6];
            OPCErrorFlag.Text = settings[7];
            GenExportDest.Text = settings[8];
            DESMTPClient.Text = settings[9];
            DESendingEmail.Text = settings[10];
            DESendingPassword.Text = settings[11];
            DERecipient1.Text = settings[12];
            DERecipient2.Text = settings[13];
            DERecipient3.Text = settings[14];
            SQLLocationLogDBName.Text = settings[15];
            SQLLocationLogTBName.Text = settings[16];
            MPBactiPrinterName.Text = settings[17];
            GenSerial.Text = pf._SerialNumber;
            SQLServerName.Text = settings[18];
            SQLUserID.Text = settings[19];
            SQLUserPassword.Text = settings[20];
            SQLDatabaseName.Text = settings[21];
            SQLTableName.Text = settings[22];
            MPName.Text = settings[23];
            if (settings.Length >= 24)
            {

                LPName.Text = settings[24];
            }
            if (settings.Length >= 33)
            {
                OPCL1License.Text = settings[25];
                OPCL1MetalPresent.Text = settings[26];
                OPCL1Tare.Text = settings[27];
                OPCL1Net.Text = settings[28];
                OPCL1DayTime.Text = settings[29];
                OPCL1Check.Text = settings[30];
                OPCL1Foil.Text = settings[31];
                OPCL1FillingToLong.Text = settings[32];
                OPCL1LineNumber.Text = settings[33];
            }
        }
        #endregion
        #region Triggers
        //Validates a serial number is long enough and that the serial number does not include any characters that are not 0-9
        private void GenSerial_Validated(object sender, EventArgs e)
        {
            char[] serial = GenSerial.Text.ToCharArray();
            var pf = Application.OpenForms.OfType<PrimaryForm>().First();
            bool flag = false;
            for (int x = 0; x < serial.Length && flag == false; x++)
            {
                if (serial[x] < '0' || serial[x] > '9')
                {
                    flag = true;
                    MessageBox.Show("Invalid Serial Number!");
                    GenSerial.Text = pf._SerialNumber;
                    return;
                }
            }
            if (serial.Length < 8 && flag == false)
            {
                flag = true;
                MessageBox.Show("Invalid Serial Number!");
                GenSerial.Text = pf._SerialNumber;
                return;
            }
            Serialnumberchanged = true;
        }


        #endregion

        private void OPCReadButton_Click(object sender, EventArgs e)
        {
            try
            {
                string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
                path11 = path11.Replace("\r", "").Replace("\n", "");
                string[] settings = File.ReadAllText(path11).Split(',');
                string[] Tag = new string[] { "25", "26", "27", "28", "29", "30", "31", "32", "33"};
                string[] Names = new string[] { "OPCL1License", "OPCL1MetalPresent", "OPCL1Tare", "OPCL1Net", "OPCL1DayTime", "OPCL1Check", "OPCL1Foil", "OPCL1FillingToLong", "OPCL1LineNumber" };
                UAEndpointDescriptor EndpointDescriptor = OPCServerString.Text;
                Button s = (Button)sender;
                string TagNumber =s.Tag.ToString();
                string controlname = "";
                for(int x = 0; x <= Tag.Length; x++)
                {
                    if(TagNumber == Tag[x])
                    {
                        controlname = Names[x];
                        break;
                    }
                }
                Control[] ct = this.Controls.Find(controlname, true);
                TextBox tb = ct[0] as TextBox;
                if (OPCServerUsername.Text != "" && OPCServerUsername.Text != string.Empty && OPCServerUsername.Text != null && OPCServerPassword.Text != "" && OPCServerPassword.Text != string.Empty && OPCServerPassword.Text != null)
                {
                    EndpointDescriptor.UserName = OPCServerUsername.Text;
                    EndpointDescriptor.Password = OPCServerPassword.Text;
                    EndpointDescriptor.UserIdentity = OpcLabs.BaseLib.IdentityModel.User.UserIdentity.CreateUserNameIdentity(OPCServerUsername.Text, OPCServerPassword.Text);
                }
                UANodeDescriptor NodeDescriptor = tb.Text;
                if (tb.Text != "" && tb.Text != string.Empty && tb.Text != null && OPCServerString.Text != "" && OPCServerString.Text != string.Empty && OPCServerString.Text != null)
                {
                    MessageBox.Show(UAClient.ReadValue(EndpointDescriptor, NodeDescriptor).ToString());
                }
            }
            catch (Exception exe)
            {
                string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\ErrorLog.txt";
                path15 = path15.Replace("\r", "").Replace("\n", "");
                using (StreamWriter w = File.AppendText(path15))
                {
                    w.WriteLine(" .ToString(): " + exe.ToString() + System.Environment.NewLine + " StackTrace: " + exe.StackTrace.ToString() + System.Environment.NewLine + " MESSAGE: " + exe.Message + System.Environment.NewLine + " Soruce: " + exe.Source);
                };
                MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + exe.ToString());
                MessageBox.Show("OPC Failed");
            }
        }

        private void VNCTest_Click(object sender, EventArgs e)
        {

        }
    }
}