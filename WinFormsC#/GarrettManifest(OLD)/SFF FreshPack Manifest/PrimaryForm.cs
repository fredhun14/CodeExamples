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
using System.Threading;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Reflection;
using System.Globalization;
using OpcLabs.EasyOpc.UA;
using OpcLabs.EasyOpc.UA.OperationModel;


namespace FreshPackManifest
{
    public partial class PrimaryForm : Form
    {
        int skipfirst = 0;
        public PrimaryForm()
        {
            InitializeComponent();
            //to stop background image from causing flickering during loading.
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            panel1, new object[] { true });
            //to stop background image from causing flickering during loading.
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            panel2, new object[] { true });
            //to stop background image from causing flickering during loading.
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            panel3, new object[] { true });
            //to stop background image from causing flickering during loading.
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            panel4, new object[] { true });
            //to stop background image from causing flickering during loading.
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            panel6, new object[] { true });
            //to stop background image from causing flickering during loading.
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            panel7, new object[] { true });
        }
        #region OPC
        #region OPC Clients
        //declaring OPC Ua Clients globally so that they can be accessed anywhere if neccesary at least one of which is being used in many functions
        //the others need to remain live as long as the program is running so that they keep the subscription the related OPC tag running
        EasyUAClient UAClient;
        EasyUAClient SubClient1;
        EasyUAClient SubClient2;
        EasyUAClient SubClient3;
        EasyUAClient SubClient4;
        EasyUAClient SubClient5;
        EasyUAClient SubClient6;
        EasyUAClient SubClient7;
        void CreateOPCClients()
        {
            UAClient = new EasyUAClient();
            SubClient1 = new EasyUAClient();
        }
        #endregion
        #region Subscribe OPC
        void OPCSubscribe()
        {
            try
            {
                string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
                path11 = path11.Replace("\r", "").Replace("\n", "");
                string[] settings = File.ReadAllText(path11).Split(',');
                UAEndpointDescriptor EndpointDescriptor = settings[4];
                if (settings[5] != "" && settings[5] != string.Empty && settings[5] != null && settings[6] != "" && settings[6] != string.Empty && settings[6] != null)
                {
                    EndpointDescriptor.UserName = settings[5];
                    EndpointDescriptor.Password = settings[6];
                    EndpointDescriptor.UserIdentity = OpcLabs.BaseLib.IdentityModel.User.UserIdentity.CreateUserNameIdentity(settings[5], settings[6]);
                }
                UANodeDescriptor NodeDescriptor = settings[29];
                if (settings[29] != "" && settings[29] != string.Empty && settings[29] != null && settings[4] != "" && settings[4] != string.Empty && settings[4] != null)
                {
                    SubClient1.DataChangeNotification += SubClient1ChangeNotification;
                    SubClient1.SubscribeDataChange(EndpointDescriptor, NodeDescriptor, 1000);
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
            }
        }
        private void SubClient1ChangeNotification(object sender, EasyUADataChangeNotificationEventArgs e)
        {
            if (e.Succeeded)
            {
                if (skipfirst != 0)
                {
                    new Thread(delegate ()
                    {
                        Manifest(OPCRead(25), OPCRead(26), OPCRead(27), OPCRead(28), OPCRead(29), OPCRead(30), OPCRead(31), OPCRead(32), Int32.Parse(OPCRead(33)));
                    }).Start();

                }
                else { skipfirst++; }
            }
            else
            {
                string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\ErrorLog.txt";
                path15 = path15.Replace("\r", "").Replace("\n", "");
                using (StreamWriter w = File.AppendText(path15))
                {
                    w.WriteLine(" .ToString(): " + e.ToString() + System.Environment.NewLine);
                };
                MessageBox.Show("Failure: " + e.Exception.Message);
            }
        }
        #endregion
        #region Read OPC
        string OPCRead(int TagNumber)
        {
            try
            {
                string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
                path11 = path11.Replace("\r", "").Replace("\n", "");
                string[] settings = File.ReadAllText(path11).Split(',');
                UAEndpointDescriptor EndpointDescriptor = settings[4];
                if (settings[5] != "" && settings[5] != string.Empty && settings[5] != null && settings[6] != "" && settings[6] != string.Empty && settings[6] != null)
                {
                    EndpointDescriptor.UserName = settings[5];
                    EndpointDescriptor.Password = settings[6];
                    EndpointDescriptor.UserIdentity = OpcLabs.BaseLib.IdentityModel.User.UserIdentity.CreateUserNameIdentity(settings[5], settings[6]);
                }
                UANodeDescriptor NodeDescriptor = settings[TagNumber];
                if (settings[TagNumber] != "" && settings[TagNumber] != string.Empty && settings[TagNumber] != null && settings[4] != "" && settings[4] != string.Empty && settings[4] != null)
                {
                    return UAClient.ReadValue(EndpointDescriptor, NodeDescriptor).ToString();
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
                return "OPC Failed";
            }
            return "OPC Failed";
        }
        #endregion
        #region OPC Write
        void OPCWrite(string ToWrite, int TagNumber)
        {
            try
            {
                string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
                path11 = path11.Replace("\r", "").Replace("\n", "");
                string[] settings = File.ReadAllText(path11).Split(',');
                UAEndpointDescriptor EndpointDescriptor = settings[4];
                if (settings[5] != "" && settings[5] != string.Empty && settings[5] != null && settings[6] != "" && settings[6] != string.Empty && settings[6] != null)
                {
                    EndpointDescriptor.UserName = settings[5];
                    EndpointDescriptor.Password = settings[6];
                    EndpointDescriptor.UserIdentity = OpcLabs.BaseLib.IdentityModel.User.UserIdentity.CreateUserNameIdentity(settings[5], settings[6]);
                }
                UANodeDescriptor NodeDescriptor = settings[TagNumber];
                if (settings[TagNumber] != "" && settings[TagNumber] != string.Empty && settings[TagNumber] != null && settings[4] != "" && settings[4] != string.Empty && settings[4] != null)
                {
                    UAClient.WriteValue(EndpointDescriptor, NodeDescriptor, ToWrite);
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
            }
        }
        #endregion
        #endregion
        #region for NiceLabel
        //starts the nice Label print engine which is needed to print and create previews
        private void InitializePrintEngine()
        {
            try
            {
                // this is needed anytime the file is loaded not on the IDE computer it tells the software where to look for the .dll file
                string sdkFilesPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\SDK.NET.dll";
                if (Directory.Exists(sdkFilesPath))
                {
                    PrintEngineFactory.SDKFilesPath = sdkFilesPath;
                }

                PrintEngineFactory.PrintEngine.Initialize();
            }
            catch (SDKException exception)
            {
                MessageBox.Show("Initialization of the SDK failed." + Environment.NewLine + Environment.NewLine + exception.ToString());
                Application.Exit();
            }
        }
        #endregion
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
        //This variable is used for logging purposes only; when an event is triggered that has a logging trigger attatched to it this variable is used
        //to log which user is currently logged in at the time of the event.
        private string User = "";
        public string _User
        {
            get
            {
                return User;
            }
            set
            {
                if (User != value)
                    User = value;
            }
        }
        #endregion
        #region Pallet Serial#
        // A serial number asigned whenever a Pallet is completed according to the counter in read loop or when printing a manual ticket
        // it is saved in a text file and asigned originally in the primary form load
        private string SerialNumber = "0";
        public string _SerialNumber
        {
            get
            {
                return SerialNumber;
            }
            set
            {
                if (SerialNumber != value)
                    SerialNumber = value;
            }
        }
        #endregion
        #region Form Load
        private void PrimaryForm_Load(object sender, EventArgs e)
        {
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                MessageBox.Show("The program is already running");
                this.Close();
            }
            GenerateNesecaryFiles();
            InitializePrintEngine();
            //PrintLicense();
            LoadTunnels();
            LoadFPResources();
            Load_Saved();
            InitializeSerial();
            LoadTotals();
            CreateOPCClients();
            OPCSubscribe();

            login();
        }

        private void PrimaryForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PrintEngineFactory.PrintEngine.Shutdown();
        }
        #endregion
        #region Menu strip actions
        private void generateHourlyReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Print_Hourly_Production_Report_Manual();
        }
        private void displayDailyTicketLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User, "Viewed Daily Ticket Log");
            DailyLogDisplay dlog = new DailyLogDisplay();
            dlog.Show();
        }
        private void voidTicketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User,"Opened Void Ticket Dialog");
            VoidTicket voidticket = new VoidTicket();
            voidticket.Show();
        }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User, "Opened Export Dialog");
            Export ex = new Export();
            ex.Show();
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User, "Opened About Dialog");
            About ab = new About();
            ab.Show();
        }
        private void configureTunnelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User,"Opened Configure tunnel dialog");
            ConfigureTunnels ct = new ConfigureTunnels();
            DialogResult result = ct.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadTunnels();
            }
        }
        private void resourceDescriptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User,"Opened resource description dialog");
            ProductList plist = new ProductList();
            plist.Show();
        }
        private void generalSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User,"Opened General settings dialog");
            General_Settings gsettings = new General_Settings();
            gsettings.Show();
        }
        //Opens the Manage users form which allows the user's credentials and access level to be modified
        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User,"Opened Manage users dialog");
            ManageUsers mnguser = new ManageUsers();
            mnguser.Show();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User, "Closed the software");
            this.Close();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User, "User: " + User + " Logged out.");
            SecLevel = 0;
            User = "";
            loginsettings();
            UserLabel.Text = "";
            login();
        }
        private void configureDayCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User,"Opened configure daycodes dialog");
            DayCodeGenerator daycodegen = new DayCodeGenerator();
            daycodegen.Show();
        }

        private void resendLicensePrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User,"Resent License print job");
            PrintLicense();
        }
        private void configureResourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigureResources fpr = new ConfigureResources();
            var dialogresult = fpr.ShowDialog();
            if (dialogresult == DialogResult.OK) { LoadFPResources(); }
        }
        #endregion
        #region Generate Neccesary Files
        //Simply generates the neccesary files for basic operation of the program and fills them with some default and basic data so the program can be ran
        //and modified in the gui to the correct settings
        //This also triggers the configuration path selection if none is found allowing the user to select the path on first start
        void GenerateNesecaryFiles()
        {
            //Creates the file that points to the configuration folder and has the user select that location
            //This should only happen the first time the program is run unless the file is deleted or the exe is moved

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
            //creates a folder to store and look for the bartender label formates
            string Path3 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Formats";
            Path3 = Path3.Replace("\r", "").Replace("\n", "");
            if (!System.IO.Directory.Exists(Path3))
            {
                System.IO.Directory.CreateDirectory(Path3);
            }
            //creates a file to save datecodes in
            string path4 = "";
            path4 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\DateCodes" + ".txt";
            path4 = path4.Replace("\r", "").Replace("\n", "");
            if (!File.Exists(path4))
            {
                using (StreamWriter w = File.AppendText(path4))
                {
                    w.WriteLine("01-SmithCode, adddyy,");
                }
            }
            string path5 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports";
            path5 = path5.Replace("\r", "").Replace("\n", "");
            if (!System.IO.Directory.Exists(path5))
            {
                System.IO.Directory.CreateDirectory(path5);
            }
            string path18 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports\\Hourly";
            path18 = path18.Replace("\r", "").Replace("\n", "");
            if (!System.IO.Directory.Exists(path18))
            {
                System.IO.Directory.CreateDirectory(path18);
            }
            //creates a folder to store and to look for images that will be used in the bartender labels
            string Path6 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Images";
            Path6 = Path6.Replace("\r", "").Replace("\n", "");
            if (!System.IO.Directory.Exists(Path6))
            {
                System.IO.Directory.CreateDirectory(Path6);
            }
            string path7 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\security.txt";
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
                for (int f = 0; f < 122; f++)
                {
                    using (StreamWriter x = File.AppendText(path7))
                    {
                        x.WriteLine(",,,");
                    }
                }
            }
            string path8 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Tunnels.txt";
            path8 = path8.Replace("\r", "").Replace("\n", "");
            if (!File.Exists(path8))
            {
                using (StreamWriter w = File.AppendText(path8))
                {
                    w.WriteLine("01╨02");
                }
            }
            string path9 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\ManifestPrinterConfigFile.txt";
            path9 = path9.Replace("\r", "").Replace("\n", "");
            if (!File.Exists(path9))
            {
                using (StreamWriter w = File.AppendText(path9))
                {
                    w.WriteLine(",,,");
                };
            }
            string path10 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\SerialNumber.txt";
            path10 = path10.Replace("\r", "").Replace("\n", "");
            if (!File.Exists(path10))
            {
                File.WriteAllText(path10, "0000000");
            }
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            if (!File.Exists(path11))
            {
                File.WriteAllText(path11, ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
            }
            string path12 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\Totals.txt";
            path12 = path12.Replace("\r", "").Replace("\n", "");
            if (!File.Exists(path12))
            {
                File.WriteAllText(path12, "0,0,0,0");
            }
            string path13 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\SavedData.txt";
            path13 = path13.Replace("\r", "").Replace("\n", "");
            if (!File.Exists(path13))
            {
                File.WriteAllText(path13, ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
            }
            string path14 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\PacketLog.txt";
            path14 = path14.Replace("\r", "").Replace("\n", "");
            if (!File.Exists(path14))
            {
                File.WriteAllText(path14, "");
            }
            string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\DescriptionDatabase.txt";
            path15 = path15.Replace("\r", "").Replace("\n", "");
            if (!File.Exists(path15))
            {
                File.WriteAllText(path15, ",");
            }
            string path16 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\LicenseCounter.txt";
            path16 = path16.Replace("\r", "").Replace("\n", "");
            if (!File.Exists(path16))
            {
                File.WriteAllText(path16, "0,0");
            }
            string path17 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\FreshpackResources.txt";
            path17 = path17.Replace("\r", "").Replace("\n", "");
            if (!File.Exists(path17))
            {
                File.WriteAllText(path17, "");
                using (StreamWriter w = File.AppendText(path17))
                {
                    w.WriteLine("416");
                    w.WriteLine("1");
                    w.WriteLine("0");
                    w.WriteLine("1003");
                    w.WriteLine("0");
                    w.WriteLine("0");
                    w.WriteLine("0");
                };
            }
        }
        #endregion
        #region Helper Methods
        private void MinuteTimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            if (now.Minute == 0)
            { Print_Hourly_Production_Report(); }
        }
        void Print_Hourly_Production_Report_Manual()
        {
            DateTime now = DateTime.Now;
            string expath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports\\" + "PATTYN" + CodeReturnGenerator("ddd-yy", "0", 0) + ".txt";
            expath = expath.Replace("\r", "").Replace("\n", "");
            string Hourly = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports\\Hourly\\" + "Hourly" + CodeReturnGenerator("ddd-yy", "0", 0) + (now.Hour - 1).ToString() + ".txt";
            Hourly = Hourly.Replace("\r", "").Replace("\n", "");

            string CollumnHeaders = "Resource".PadRight(20, ' ') + "Weight".PadRight(10, ' ') + "Time".PadRight(10, ' ') + "LotCode".PadRight(8, ' ') + System.Environment.NewLine;
            File.WriteAllText(Hourly, CollumnHeaders);
            if (File.Exists(expath))
            {
                int numberoftunnels = 0;
                int[] TotalWeight = new int[CountLines(expath)];
                for (int i = 0; i < TotalWeight.Length; i++)
                {
                    TotalWeight[i] = 0;
                }
                string[] tunnels = new string[CountLines(expath)];
                for (int j = 0; j < tunnels.Length; j++)
                {
                    tunnels[j] = "";
                }
                string[] Lines = File.ReadAllLines(expath);
                foreach (string Line in Lines)
                {
                    bool foundtunnel = false;
                    string[] linesplit = Line.Split(',');
                    for (int j = 0; j < tunnels.Length; j++) //tunnel is in slot 5
                    {
                        if (tunnels[j] == linesplit[5])
                        {
                            foundtunnel = true;
                            break;
                        }
                    }
                    if (!foundtunnel)
                    {
                        for (int j = 0; j < tunnels.Length; j++) //tunnel is in slot 5
                        {
                            if (tunnels[j] == "")
                            {
                                tunnels[j] = linesplit[5];
                                numberoftunnels++;
                                break;
                            }
                        }
                    }
                }
                foreach (string Line in Lines)
                {
                    string[] linesplit = Line.Split(',');
                    if (linesplit.Length > 2)
                    {
                        //time is line[3] weight is line[0]
                        linesplit[3] = linesplit[3].Substring(1);
                        int time = int.Parse(linesplit[3].Split(':')[0]);
                        string ampm = linesplit[3].Split(' ')[1].Substring(0, 2);
                        if (time == int.Parse(now.ToString("hh")) && ampm == now.ToString("tt").ToLower())
                        {
                            for (int j = 0; j < tunnels.Length; j++)
                            {
                                if (tunnels[j] == linesplit[5])
                                {
                                    TotalWeight[j] = TotalWeight[j] + Int32.Parse(linesplit[0]);
                                }
                            }
                            string newline = linesplit[2].PadRight(20, ' ') + linesplit[0].PadRight(10, ' ') + linesplit[3].PadRight(10, ' ') + linesplit[6].PadRight(8, ' ');
                            using (StreamWriter w = File.AppendText(Hourly))
                            {
                                w.WriteLine(newline);
                            };
                        }
                    }
                }
                using (StreamWriter w = File.AppendText(Hourly))
                {
                    for (int j = 0; j < numberoftunnels; j++)
                        w.WriteLine("Total Weight for tunnel " + tunnels[j] + ": " + TotalWeight[j].ToString());
                };
                System.Diagnostics.Process.Start(Hourly);
            }
        }
        void Print_Hourly_Production_Report()
        {
            DateTime now = DateTime.Now;
            now = now.AddHours(-1);
            string expath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports\\" + "PATTYN" + CodeReturnGenerator("ddd-yy", "0", 0) + ".txt";
            expath = expath.Replace("\r", "").Replace("\n", "");
            string Hourly = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports\\Hourly\\" + "Hourly" + CodeReturnGenerator("ddd-yy", "0", 0) + (now.Hour -1).ToString() + ".txt";
            Hourly = Hourly.Replace("\r", "").Replace("\n", "");

            string CollumnHeaders = "Resource".PadRight(20, ' ') + "Weight".PadRight(10, ' ') + "Time".PadRight(10, ' ') + "LotCode".PadRight(8, ' ') + System.Environment.NewLine;
            File.WriteAllText(Hourly, CollumnHeaders);
            if (File.Exists(expath))
            {
                int numberoftunnels = 0;
                int[] TotalWeight = new int[CountLines(expath)];
                for (int i = 0; i < TotalWeight.Length; i++)
                {
                    TotalWeight[i] = 0;
                }
                string[] tunnels = new string[CountLines(expath)];
                for (int j = 0; j < tunnels.Length; j++)
                {
                    tunnels[j] = "";
                }
                string[] Lines = File.ReadAllLines(expath);
                foreach (string Line in Lines)
                {
                    bool foundtunnel = false;
                    string[] linesplit = Line.Split(',');
                    for (int j = 0; j < tunnels.Length; j++) //tunnel is in slot 5
                    {
                        if (tunnels[j] == linesplit[5])
                        {
                            foundtunnel = true;
                            break;
                        }
                    }
                    if(!foundtunnel)
                    {
                        for (int j = 0; j < tunnels.Length; j++) //tunnel is in slot 5
                        {
                            if(tunnels[j] == "")
                            {
                                tunnels[j] = linesplit[5];
                                numberoftunnels++;
                                break;
                            }
                        }
                    }
                }
                foreach (string Line in Lines)
                {
                    string[] linesplit = Line.Split(',');
                    if (linesplit.Length > 2)
                    {
                        //time is line[3] weight is line[0]
                        linesplit[3] = linesplit[3].Substring(1);
                        int time = int.Parse(linesplit[3].Split(':')[0]);
                        string ampm = linesplit[3].Split(' ')[1].Substring(0, 2);
                        if (time == int.Parse(now.ToString("hh")) && ampm == now.ToString("tt").ToLower())
                        {
                            for (int j = 0; j < tunnels.Length; j++)
                            {
                                if(tunnels[j] == linesplit[5])
                                {
                                    TotalWeight[j] = TotalWeight[j] + Int32.Parse(linesplit[0]);
                                }
                            }
                            string newline = linesplit[2].PadRight(20, ' ') + linesplit[0].PadRight(10, ' ') + linesplit[3].PadRight(10, ' ') + linesplit[6].PadRight(8, ' ');
                            using (StreamWriter w = File.AppendText(Hourly))
                            {
                                w.WriteLine(newline);
                            };
                        }
                    }
                }
                using (StreamWriter w = File.AppendText(Hourly))
                {
                    for(int j = 0; j < numberoftunnels; j++)
                    w.WriteLine("Total Weight for tunnel " + tunnels[j] + ": " + TotalWeight[j].ToString());
                };
                System.Diagnostics.Process.Start(Hourly);
            }
        }
        //Recieves any freshpack resource numbers and converts the digits where it should if the product contains metal.
        string MetalFreshResourceConverter(string Resource)
        {
            string[] resourcesplit = Resource.Split('-');
            char[] changesection1 = resourcesplit[1].ToCharArray();
            char[] changesection2 = resourcesplit[3].ToCharArray();
            changesection1[1] = '4';
            changesection2[2] = 'M';
            string charstring1 = new string(changesection1);
            string charstring2 = new string(changesection2);
            string thereturn = resourcesplit[0] + "-" + charstring1 + "-" + resourcesplit[2] + "-" + charstring2;
            return thereturn;
        }
        string FillingToLongFreshResourceConverter(string Resource)
        {
            string[] resourcesplit = Resource.Split('-');
            char[] changesection1 = resourcesplit[1].ToCharArray();
            if (changesection1[1] != '7' && changesection1[1] != '8' && changesection1[1] != '9')
            {
                changesection1[1] = '3';
            }
            string charstring1 = new string(changesection1);

            string thereturn = resourcesplit[0] + "-" + charstring1 + "-" + resourcesplit[2] + "-" + resourcesplit[3];
            return thereturn;
        }
        void Manifest(string License, string metal, string tare, string net, string daytime, string check, string foil, string fillingtolong, int Line)
        {
            try
            {
                if (Line != 0)
                {
                    Thread.CurrentThread.IsBackground = true;
                    string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
                    path11 = path11.Replace("\r", "").Replace("\n", "");
                    string[] settings = File.ReadAllText(path11).Split(',');
                    string path10 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\SerialNumber.txt";
                    path10 = path10.Replace("\r", "").Replace("\n", "");
                    string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\DescriptionDatabase.txt";
                    path15 = path15.Replace("\r", "").Replace("\n", "");
                    string[] descriptions = File.ReadAllText(path15).Split('╨');

                    double igross = double.Parse(TotalGross.Text) + double.Parse(check);
                    double itare = double.Parse(TotalTare.Text) + double.Parse(tare);
                    double inet = double.Parse(TotalNet.Text) + double.Parse(net);
                    int icount = int.Parse(PalletCount.Text);
                    icount++;

                    tare = String.Format("{0:0}", double.Parse(tare));
                    check = String.Format("{0:0}", double.Parse(check));
                    net = String.Format("{0:0}", double.Parse(net));

                    string Resource = "";
                    string Description = "None Available";
                    string thetunnel = "99";
                    string remark1 = "", remark2 = "", remark3 = "";

                    Invoke(new Action(() =>
                    {
                        PalletCount.Text = icount.ToString();
                        TotalGross.Text = String.Format("{0:0}", igross);
                        TotalTare.Text = String.Format("{0:0}", itare);
                        TotalNet.Text = String.Format("{0:0}", inet);

                        Control[] ct = this.Controls.Find("Serial" + Line.ToString(), true);
                        Label serial = ct[0] as Label;
                        Control[] ct2 = this.Controls.Find("Weight" + Line.ToString(), true);
                        Label weight = ct2[0] as Label;
                        Control[] ct3 = this.Controls.Find("Time" + Line.ToString(), true);
                        Label time = ct3[0] as Label;
                        Control[] ct4 = this.Controls.Find("Resource" + Line.ToString(), true);
                        Label rs = ct4[0] as Label;
                        Control[] ct5 = this.Controls.Find("Remark" + Line.ToString() + "1", true);
                        TextBox rm1 = ct5[0] as TextBox;
                        Control[] ct6 = this.Controls.Find("Remark" + Line.ToString() + "2", true);
                        TextBox rm2 = ct6[0] as TextBox;
                        Control[] ct7 = this.Controls.Find("Remark" + Line.ToString() + "3", true);
                        TextBox rm3 = ct7[0] as TextBox;

                        string[] theresourcesplit = rs.Text.Split('-');

                        for (int x = 0; x < descriptions.Length; x = x + 2)
                        {
                            if (descriptions[x] == theresourcesplit[0]) { Description = descriptions[x + 1]; break; }
                        }
                        serial.Text = SerialNumber;
                        weight.Text = net;
                        time.Text = daytime;
                        Resource = rs.Text;
                        remark1 = rm1.Text;
                        remark2 = rm2.Text;
                        remark3 = rm3.Text;

                        if (metal != "False") { Resource = MetalFreshResourceConverter(Resource); }
                        else if (fillingtolong != "False") { Resource = FillingToLongFreshResourceConverter(Resource); }
                        Control[] tn = this.Controls.Find("Tunnel" + Line.ToString(), true);
                        ComboBox tunnel = tn[0] as ComboBox;

                        if (tunnel.SelectedItem != null)
                        {
                            thetunnel = tunnel.SelectedItem.ToString();
                        }
                    }));
                    if (metal == "1") { Resource = MetalFreshResourceConverter(Resource); }

                    DateTime now = DateTime.Now;

                    #region Text file manifest
                    string newline = net.ToString().PadLeft(5, ' ') + ",00000," + Resource.PadRight(16, ' ') + ",\"" + now.ToString("hh:mm") + " " + now.ToString("tt").ToLower() + "\",\"" + now.ToString("MM/dd/yy") + "\"," + thetunnel.PadLeft(2, '0') + ",\"" + daycodeGenerator(thetunnel.PadLeft(2, '0')) + "\"," + SerialNumber.PadLeft(8, '0') + ",\"\",\"TOTECO    \",\"" + "" + "\",\"" + "" + "\",\"" + "" + "\",\"" + "\",\"\"";
                    //fix if possible the remarks remarks.       --->                                                                                                                                                                                                                                                                                                                        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

                    string expath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports\\" + "PATTYN" + CodeReturnGenerator("ddd-yy", "0", 0) + ".txt";
                    expath = expath.Replace("\r", "").Replace("\n", "");
                    using (StreamWriter w = File.AppendText(expath))
                    {
                        w.WriteLine(newline);
                    };
                    #endregion
                    #region SQL Manifest
                    if (settings[18] != null && settings[18] != string.Empty && settings[18] != "" && settings[21] != null && settings[21] != string.Empty && settings[21] != "" && settings[22] != null && settings[22] != string.Empty && settings[22] != "")
                    {

                        //Data Source=SFFNT8;Initial Catalog=ReplacementDB;Persist Security Info=True;User ID=software;Password=***********
                        string connString = "Data Source=" + settings[18] + ";Initial Catalog=" + settings[21] + ";User ID=" + settings[19] + ";Password=" + settings[20] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                        string cmdString = "INSERT INTO " + settings[22] + " (Serial_Number, Line_Number, Metal_Present, Tare_Weight, Net_Weight, Check_Weight, License, Foil_Check, Filling_To_Long, Resource, Description) VALUES (@val0,@val1,@val2,@val3,@val4,@val5,@val6,@val7,@val8,@val9,@val10)";
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;

                                comm.Parameters.AddWithValue("@val0", SerialNumber);
                                comm.Parameters.AddWithValue("@val1", thetunnel);
                                comm.Parameters.AddWithValue("@val2", metal);
                                comm.Parameters.AddWithValue("@val3", tare);
                                comm.Parameters.AddWithValue("@val4", net);
                                comm.Parameters.AddWithValue("@val5", check);
                                comm.Parameters.AddWithValue("@val6", License);
                                comm.Parameters.AddWithValue("@val7", foil);
                                comm.Parameters.AddWithValue("@val8", fillingtolong);
                                comm.Parameters.AddWithValue("@val9", Resource);
                                comm.Parameters.AddWithValue("@val10", Description);

                                conn.Open();
                                comm.ExecuteNonQuery();
                            }
                        }
                    }
                    #endregion
                    #region Print Manifest Label
                    string Path3 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Formats\\" + "Manifest" + ".nlbl";
                    Path3 = Path3.Replace("\r", "").Replace("\n", "");

                    ILabel label = PrintEngineFactory.PrintEngine.OpenLabel(Path3);
                    label.PrintSettings.PrinterName = settings[23];

                    label.Variables["LineNumber"].SetValue(thetunnel);
                    label.Variables["MetalPresent"].SetValue(metal);
                    label.Variables["TareWT"].SetValue(tare);
                    label.Variables["NetWT"].SetValue(net);
                    label.Variables["Date_Time"].SetValue(now.ToString("g"));
                    label.Variables["CheckWT"].SetValue(check);
                    label.Variables["FoilCheck"].SetValue(foil);
                    label.Variables["FillingtoLong"].SetValue(fillingtolong);
                    label.Variables["SerialNumber"].SetValue(SerialNumber);
                    label.Variables["ResourceNumber"].SetValue(Resource);
                    label.Variables["Description"].SetValue(Description);
                    label.Variables["Remark1"].SetValue(remark1);
                    label.Variables["Remark2"].SetValue(remark2);
                    label.Variables["Remark3"].SetValue(remark3);
                    label.Variables["Tunnel"].SetValue(thetunnel);
                    label.Variables["LotCode"].SetValue(daycodeGenerator(thetunnel.PadLeft(2, '0')));
                    //add more variables later to fill out label with other information like resource.

                    //What actually sends the print command:
                    label.Print(1);

                    ILabelPreviewSettings labelPreviewSettings = new LabelPreviewSettings();

                    labelPreviewSettings.PreviewToFile = false;                                    // if true, result will be the file name, if false, result will be a byte array.
                    labelPreviewSettings.ImageFormat = "jpg";                                      // file format of graphics.  Valid formats: JPG, PNG, BMP.
                    labelPreviewSettings.Width = this.ManifestPreview.Width;                            // Width of image to generate
                    labelPreviewSettings.Height = this.ManifestPreview.Height;                          // Height of image to generate
                                                                                                        //labelPreviewSettings.Destination = this.textBoxFileName.Text;                // If PrintToFile is true, this is the name of the file that will be generated.
                    labelPreviewSettings.FormatPreviewSide = FormatPreviewSide.FrontSide;          // Which label side(s) to generate the image for.  

                    // Generate Preview File
                    object imageObj = label.GetLabelPreview(labelPreviewSettings);

                    // Display image in UI
                    if (imageObj is byte[])
                    {
                        // When PrintToFiles = false
                        // Convert byte[] to Bitmap and set as image source for PictureBox control
                        Invoke(new Action(() =>
                        {
                            ManifestPreview.Image = this.ByteToImage((byte[])imageObj);
                        }));
                    }
                    Invoke(new Action(() =>
                    {
                        OutputTestBox.AppendText(SerialNumber + ", " + License + ", " + metal + ", " + tare + ", " + net + ", " + daytime + ", " + check + ", " + foil + ", " + fillingtolong + ", " + thetunnel + ", " + Line + System.Environment.NewLine);
                        SerialNumber = (Int32.Parse(SerialNumber) + 1).ToString().PadLeft(8, '0');
                        File.WriteAllText(path10, SerialNumber);
                        Serial0.Text = SerialNumber;
                    }));
                    #endregion
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
            }
        }
        void LoadTunnels()
        {
            string path8 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Tunnels.txt";
            path8 = path8.Replace("\r", "").Replace("\n", "");
            string[] tunnels = File.ReadAllText(path8).Split('╨');

            for (int x = 0; x < tunnels.Length; x++)
            {
                tunnels[x] = tunnels[x].Replace("\r", "").Replace("\n", "");
            }

            Tunnel1.Items.Clear();
            Tunnel2.Items.Clear();
            Tunnel3.Items.Clear();
            Tunnel4.Items.Clear();
            Tunnel5.Items.Clear();
            Tunnel6.Items.Clear();
            Tunnel7.Items.Clear();

            Tunnel1.Items.AddRange(tunnels);
            Tunnel2.Items.AddRange(tunnels);
            Tunnel3.Items.AddRange(tunnels);
            Tunnel4.Items.AddRange(tunnels);
            Tunnel5.Items.AddRange(tunnels);
            Tunnel6.Items.AddRange(tunnels);
            Tunnel7.Items.AddRange(tunnels);
        }

        private void Tunnel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Save();
            LogUserActivity LUA = new LogUserActivity();
            ComboBox tun = sender as ComboBox;
            LUA.LogActivity(User, "Changed " + tun.Name + " to " + tun.SelectedItem.ToString());
        }
        void PrintLicense()
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            string Path3 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Formats\\" + "License" + ".nlbl";
            Path3 = Path3.Replace("\r", "").Replace("\n", "");
            CheckLicenseCounter();
            string path16 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\LicenseCounter.txt";
            path16 = path16.Replace("\r", "").Replace("\n", "");
            string[] counter = File.ReadAllText(path16).Split(',');

            ILabel label = PrintEngineFactory.PrintEngine.OpenLabel(Path3);
            label.PrintSettings.PrinterName = settings[24];

            label.Variables["Counter"].SetValue(counter[0].PadLeft(6, '0'));

            label.Print(1);
        }
        void CheckLicenseCounter()
        {
            string path16 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\LicenseCounter.txt";
            path16 = path16.Replace("\r", "").Replace("\n", "");
            string[] counter = File.ReadAllText(path16).Split(',');
            DateTime now = DateTime.Now;
            if (counter[1] != JulianDateGenerator(now))
            {
                File.WriteAllText(path16, "0," + JulianDateGenerator(now));
            }
        }

        /// Helper method to convert Bytes to image.
        /// </summary>
        /// <param name="bytes">The image as a byte array.</param>
        /// <returns>Bitmap of the image.</returns>
        private Bitmap ByteToImage(byte[] bytes)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(bytes, 0, Convert.ToInt32(bytes.Length));
            Bitmap bm = new Bitmap(memoryStream, false);
            memoryStream.Dispose();
            return bm;
        }
        //login calls the login form which forces the user to enter credentials before continueing 
        private void login()
        {
            Login login = new Login();
            login.StartPosition = FormStartPosition.Manual;
            login.Location = new Point(1010, 300);
            var dialogresult = login.ShowDialog();
            loginsettings();
            UserLabel.Text = User;
        }
        void loginsettings()
        {
            switch (SecLevel)
            {
                case 10:
                    for(int x = 1; x < 8; x++)
                    {
                        Control [] ct1 = this.Controls.Find("ResourceLock" + x.ToString(), true);
                        CheckBox locker = ct1[0] as CheckBox;
                        Control[] ct2 = this.Controls.Find("Tunnel" + x.ToString(), true);
                        ComboBox tunn = ct2[0] as ComboBox;

                        locker.Checked = true;
                        locker.Enabled = true;
                        tunn.Enabled = true;
                    }
                    ClearTotals.Enabled = true;
                    //manifest
                    manifestToolStripMenuItem.Visible = true;
                    exportToolStripMenuItem.Visible = true;
                    voidTicketToolStripMenuItem.Visible = true;
                    generateHourlyReportToolStripMenuItem.Visible = true;
                    //options
                    optionsToolStripMenuItem.Visible = true;
                    manageUsersToolStripMenuItem.Visible = true;
                    configureDayCodesToolStripMenuItem.Visible = true;
                    generalSettingsToolStripMenuItem.Visible = true;
                    resourceDescriptionsToolStripMenuItem.Visible = true;
                    configureTunnelsToolStripMenuItem.Visible = true;
                    configureResourcesToolStripMenuItem.Visible = true;
                    //debugging
                    OutputTestBox.Visible = true;
                    OutputTestBox.Enabled = true;
                    break;
                case 5:
                    for (int x = 1; x < 8; x++)
                    {
                        Control[] ct1 = this.Controls.Find("ResourceLock" + x.ToString(), true);
                        CheckBox locker = ct1[0] as CheckBox;
                        Control[] ct2 = this.Controls.Find("Tunnel" + x.ToString(), true);
                        ComboBox tunn = ct2[0] as ComboBox;

                        locker.Checked = true;
                        locker.Enabled = true;
                        tunn.Enabled = true;
                    }
                    ClearTotals.Enabled = true;
                    //manifest
                    manifestToolStripMenuItem.Visible = true;
                    exportToolStripMenuItem.Visible = true;
                    voidTicketToolStripMenuItem.Visible = true;
                    generateHourlyReportToolStripMenuItem.Visible = true;
                    //options
                    optionsToolStripMenuItem.Visible = true;
                    manageUsersToolStripMenuItem.Visible = true;
                    configureDayCodesToolStripMenuItem.Visible = true;
                    generalSettingsToolStripMenuItem.Visible = false;
                    resourceDescriptionsToolStripMenuItem.Visible = true;
                    configureTunnelsToolStripMenuItem.Visible = true;
                    configureResourcesToolStripMenuItem.Visible = true;
                    //debugging
                    OutputTestBox.Visible = true;
                    OutputTestBox.Enabled = false;
                    break;
                case 3:
                    for (int x = 1; x < 8; x++)
                    {
                        Control[] ct1 = this.Controls.Find("ResourceLock" + x.ToString(), true);
                        CheckBox locker = ct1[0] as CheckBox;
                        Control[] ct2 = this.Controls.Find("Tunnel" + x.ToString(), true);
                        ComboBox tunn = ct2[0] as ComboBox;

                        locker.Checked = true;
                        locker.Enabled = true;
                        tunn.Enabled = true;
                    }
                    ClearTotals.Enabled = true;
                    //manifest
                    manifestToolStripMenuItem.Visible = true;
                    exportToolStripMenuItem.Visible = true;
                    voidTicketToolStripMenuItem.Visible = true;
                    generateHourlyReportToolStripMenuItem.Visible = true;
                    //options
                    optionsToolStripMenuItem.Visible = true;
                    manageUsersToolStripMenuItem.Visible = true;
                    configureDayCodesToolStripMenuItem.Visible = false;
                    generalSettingsToolStripMenuItem.Visible = false;
                    resourceDescriptionsToolStripMenuItem.Visible = true;
                    configureTunnelsToolStripMenuItem.Visible = true;
                    configureResourcesToolStripMenuItem.Visible = true;
                    //debugging
                    OutputTestBox.Visible = true;
                    OutputTestBox.Enabled = false;
                    break;
                case 1:
                    for (int x = 1; x < 8; x++)
                    {
                        Control[] ct1 = this.Controls.Find("ResourceLock" + x.ToString(), true);
                        CheckBox locker = ct1[0] as CheckBox;
                        Control[] ct2 = this.Controls.Find("Tunnel" + x.ToString(), true);
                        ComboBox tunn = ct2[0] as ComboBox;

                        locker.Checked = true;
                        locker.Enabled = true;
                        tunn.Enabled = true;
                    }
                    ClearTotals.Enabled = false;
                    //manifest
                    manifestToolStripMenuItem.Visible = true;
                    exportToolStripMenuItem.Visible = true;
                    voidTicketToolStripMenuItem.Visible = false;
                    generateHourlyReportToolStripMenuItem.Visible = true;
                    //options
                    optionsToolStripMenuItem.Visible = false;
                    manageUsersToolStripMenuItem.Visible = false;
                    configureDayCodesToolStripMenuItem.Visible = false;
                    generalSettingsToolStripMenuItem.Visible = false;
                    resourceDescriptionsToolStripMenuItem.Visible = false;
                    configureTunnelsToolStripMenuItem.Visible = false;
                    configureResourcesToolStripMenuItem.Visible = false;
                    //debugging
                    OutputTestBox.Visible = false;
                    OutputTestBox.Enabled = false;
                    break;
                case 0:
                    for (int x = 1; x < 8; x++)
                    {
                        Control[] ct1 = this.Controls.Find("ResourceLock" + x.ToString(), true);
                        CheckBox locker = ct1[0] as CheckBox;
                        Control[] ct2 = this.Controls.Find("Tunnel" + x.ToString(), true);
                        ComboBox tunn = ct2[0] as ComboBox;

                        locker.Checked = true;
                        locker.Enabled = false;
                        tunn.Enabled = false;
                    }
                    ClearTotals.Enabled = false;
                    //manifest
                    manifestToolStripMenuItem.Visible = false;
                    exportToolStripMenuItem.Visible = false;
                    voidTicketToolStripMenuItem.Visible = false;
                    generateHourlyReportToolStripMenuItem.Visible = false;
                    //options
                    optionsToolStripMenuItem.Visible = false;
                    manageUsersToolStripMenuItem.Visible = false;
                    configureDayCodesToolStripMenuItem.Visible = false;
                    generalSettingsToolStripMenuItem.Visible = false;
                    resourceDescriptionsToolStripMenuItem.Visible = false;
                    configureTunnelsToolStripMenuItem.Visible = false;
                    //debugging
                    OutputTestBox.Visible = false;
                    OutputTestBox.Enabled = false;
                    break;
            }

        }
        //counts the number of lines in a file
        public int CountLines(string file)
        {
            int count = 0;
            count = File.ReadLines(file).Count();
            return count;
        }
        //waits a number of milliseconds 
        public void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }
        string GetTunnel(int Line)
        {
            Control[] ct = this.Controls.Find("Tunnel" + Line.ToString(), true);
            ComboBox tunnel = ct[0] as ComboBox;
            string thetunnel = "99";
            if (tunnel.SelectedItem != null)
            {
                thetunnel = tunnel.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("NO TUNNEL SELECTED LINE: " + Line);
            }
            return thetunnel;
        }
        private void ResourceLock_Click(object sender, EventArgs e)
        {
            CheckBox send = (CheckBox)sender;
            int p = Int32.Parse(this.ActiveControl.Name[this.ActiveControl.Name.Length - 1].ToString());
            Control[] cs1 = this.Controls.Find("ABC" + p.ToString(), true);
            ComboBox ABC = cs1[0] as ComboBox;
            Control[] cs2 = this.Controls.Find("D" + p.ToString(), true);
            ComboBox D = cs2[0] as ComboBox;
            Control[] cs3 = this.Controls.Find("E" + p.ToString(), true);
            ComboBox E = cs3[0] as ComboBox;
            Control[] cs4 = this.Controls.Find("FGH" + p.ToString(), true);
            ComboBox FGH = cs4[0] as ComboBox;
            Control[] cs5 = this.Controls.Find("I" + p.ToString(), true);
            ComboBox I = cs5[0] as ComboBox;
            Control[] cs6 = this.Controls.Find("J" + p.ToString(), true);
            ComboBox J = cs6[0] as ComboBox;
            Control[] cs7 = this.Controls.Find("K" + p.ToString(), true);
            ComboBox K = cs7[0] as ComboBox;
            if (send.Checked == true)
            {
                ABC.Enabled = false;
                D.Enabled = false;
                E.Enabled = false;
                FGH.Enabled = false;
                I.Enabled = false;
                J.Enabled = false;
                K.Enabled = false;
            }
            else if (send.Checked == false)
            {
                ABC.Enabled = true;
                D.Enabled = true;
                E.Enabled = true;
                FGH.Enabled = true;
                I.Enabled = true;
                J.Enabled = true;
                K.Enabled = true;
            }
        }
        #endregion
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
        #region Old Pattyn IO management
        /*
        static

        
        void PattynThreadStart()
        {
            new Thread(delegate ()
            {
                PattynConnect();
            }).Start();
        }
        void PattynConnect()
        {
            Thread.CurrentThread.IsBackground = true;
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            string host = settings[2];
            int port = Int32.Parse(settings[3]);


            try { 
                TcpListener listener = new TcpListener(System.Net.IPAddress.Parse(host), port);
                listener.Start();
                Byte[] bit = new byte[256];
                TcpClient tclient = listener.AcceptTcpClient();
                //int g = tclient.GetStream().Read(bit, 0, bit.Length);
                string FirstPacket = tclient.GetStream().ToString();
                Invoke(new Action(() =>
                {
                    OutputTestBox.AppendText("Pattyn Connected..." + System.Environment.NewLine);
                    OutputTestBox.AppendText(FirstPacket + " " + System.Environment.NewLine);
                }));
                if (!tclient.Connected)
                {
                    //fail
                    MessageBox.Show("Pattyn Failed to connect!");
                }
                else
                {
                    //success

                    NetworkStream netstream = tclient.GetStream();
                    netstream.ReadTimeout = 200;
                    string responseData = string.Empty;
                    string newline = null;
                    Byte[] bytes = new Byte[256];
                    int i;
                    while (!_PattynshouldStop)
                    {
                        try
                        {
                            while ((i = netstream.Read(bytes, 0, bytes.Length)) != 0 && !_PattynshouldStop)
                            {
                                newline = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                                newline = newline.Replace("\r", ",");
                                Invoke(new Action(() =>
                                {
                                    OutputTestBox.AppendText(newline);
                                    OutputTestBox.AppendText(System.Environment.NewLine);
                                    ManifestPacket(newline);
                                }));
                            }

                        }
                        catch (Exception e)
                        {
                            if (e.Message.Contains("Unable to read data from the transport connection: A connection attempt failed because the connected party did not properly respond after a period of time,"))
                            { }
                            else { MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + e.ToString()); }
                        }
                    }

                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + ex.ToString());
            }
           
        }
       
        void ManifestPacket(string packet)
        {
            Thread.CurrentThread.IsBackground = true;
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            string path10 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\SerialNumber.txt";
            path10 = path10.Replace("\r", "").Replace("\n", "");
            string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\DescriptionDatabase.txt";
            path15 = path15.Replace("\r", "").Replace("\n", "");
            string[] descriptions = File.ReadAllText(path15).Split('╥');

             //* 0 LineNumber
             //* 1 Metal Present
             //* 2 Tare WT
             //* 3 Net  WT
             //* 4 Date/Time yyyy/mm/dd HH:MM
             //* 5 Check Weight
             //* 6 Clips Present
             //* 7 foil check
             //* 8 filling to long
             //* 00,0,0000,0000,1970/01/01 00:00,0000,0,0,0
             //* 03,0,0111,1453,2021/11/17 12:14,1565,0,0,0
             
            string[] SPacket = packet.Split(',');

            if(packet!= "00,0,0000,0000,1970/01/01 00:00,0000,0,0,0" && SPacket[0]!= "00" && SPacket[0] != "0" && SPacket.Length > 1)
            {
                string path14 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\PacketLog.txt";
                path14 = path14.Replace("\r", "").Replace("\n", "");
                using (StreamWriter w = File.AppendText(path14))
                {
                    w.WriteLine(packet);
                };

                Invoke(new Action(() =>
                {
                    int gross = Int32.Parse(TotalGross.Text) + Int32.Parse(SPacket[5]);
                    int tare = Int32.Parse(TotalTare.Text) + Int32.Parse(SPacket[2]);
                    int net = Int32.Parse(TotalNet.Text) + Int32.Parse(SPacket[3]);
                    int count = Int32.Parse(PalletCount.Text);
                    count++;
                    PalletCount.Text = count.ToString();
                    TotalGross.Text = gross.ToString();
                    TotalTare.Text = tare.ToString();
                    TotalNet.Text = net.ToString();
                    int line = int.Parse(SPacket[0]);
                    Control[] ct = this.Controls.Find("Serial" + line.ToString(), true);
                    Label serial = ct[0] as Label;
                    Control[] ct2 = this.Controls.Find("Weight" + line.ToString(), true);
                    Label weight = ct2[0] as Label;
                    Control[] ct3 = this.Controls.Find("Time" + line.ToString(), true);
                    Label time = ct3[0] as Label;
                    Control[] ct4 = this.Controls.Find("Resource" + line.ToString(), true);
                    Label rs = ct4[0] as Label;
                    string[] theresourcesplit = rs.Text.Split('-');
                    string Description = "None Available";
                    for(int x = 0; x < descriptions.Length; x = x + 2)
                    {
                        if (descriptions[x] == theresourcesplit[0]) { Description = descriptions[x + 1]; break; }
                    }
                    serial.Text = SerialNumber;
                    weight.Text = SPacket[3];
                    time.Text = SPacket[4].Substring(SPacket[4].Length - 5);
                    string Resource = rs.Text;

                    DateTime now = DateTime.Now;
                    string newline = SPacket[3].ToString().PadLeft(5, ' ') + ",00000," + Resource.PadRight(16, ' ') + ",\"" + now.ToString("HH:mm") + " " + now.ToString("tt").ToLower() + "\",\"" + now.ToString("MM/dd/yy") + "\"," + GetTunnel(SPacket[0]).PadLeft(2,'0') + ",\"" + daycodeGenerator(GetTunnel(SPacket[0]).PadLeft(2, '0')) + "\"," + SerialNumber.PadLeft(8, '0') + ",\"\",\"TOTECO    \",\"" + "" + "\",\"" + "" + "\",\"" + "" + "\",\"" + "\",\"\"";
                    //fix if possible remarks.       --->                                                                                                                                                                                                                                                                                                                                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

                    string expath = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Exports\\" + "PATTYN" + CodeReturnGenerator("ddd-yy","0",0) + ".txt";
                    expath = expath.Replace("\r", "").Replace("\n", "");
                    using (StreamWriter w = File.AppendText(expath))
                    {
                        w.WriteLine(newline);
                    };
                    #region SQL stuff
                    if (settings[18] != null && settings[18] != string.Empty && settings[18] != "" && settings[21] != null && settings[21] != string.Empty && settings[21] != "" && settings[22] != null && settings[22] != string.Empty && settings[22] != "")
                    {

                        //Data Source=SFFNT8;Initial Catalog=ReplacementDB;Persist Security Info=True;User ID=software;Password=***********
                        string connString = "Data Source=" + settings[18] + ";Initial Catalog=" + settings[21] + ";User ID=" + settings[19] + ";Password=" + settings[20] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                        string cmdString = "INSERT INTO " + settings[22] + " (Serial_Number, Line_Number, Metal_Present, Tare_Weight, Net_Weight, Check_Weight, Clips_Present, Foil_Check, Filling_To_Long, Resource, Description) VALUES (@val0,@val1,@val2,@val3,@val4,@val5,@val6,@val7,@val8,@val9,@val10)"; //needs correct comlumns/count and to be assinged before sending the command
                        

                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;

                                comm.Parameters.AddWithValue("@val0", SerialNumber);
                                comm.Parameters.AddWithValue("@val1", SPacket[0]);
                                comm.Parameters.AddWithValue("@val2", SPacket[1]);
                                comm.Parameters.AddWithValue("@val3", SPacket[2]);
                                comm.Parameters.AddWithValue("@val4", SPacket[3]);
                                comm.Parameters.AddWithValue("@val5", SPacket[5]);
                                comm.Parameters.AddWithValue("@val6", SPacket[6]);
                                comm.Parameters.AddWithValue("@val7", SPacket[7]);
                                comm.Parameters.AddWithValue("@val8", SPacket[8]);
                                comm.Parameters.AddWithValue("@val9", Resource);
                                comm.Parameters.AddWithValue("@val10", Description);

                                conn.Open();
                                comm.ExecuteNonQuery();
                            }
                        }
                    }
                    #endregion
                    string Path3 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Formats\\" + "Manifest" + ".nlbl";
                    Path3 = Path3.Replace("\r", "").Replace("\n", "");

                    ILabel label = PrintEngineFactory.PrintEngine.OpenLabel(Path3);
                    label.PrintSettings.PrinterName = settings[23];

                    label.Variables["LineNumber"].SetValue(SPacket[0]);
                    label.Variables["MetalPresent"].SetValue(SPacket[1]);
                    label.Variables["TareWT"].SetValue(SPacket[2]);
                    label.Variables["NetWT"].SetValue(SPacket[3]);
                    label.Variables["Date_Time"].SetValue(now.ToString("g"));
                    label.Variables["CheckWT"].SetValue(SPacket[5]);
                    label.Variables["ClipsPresent"].SetValue(SPacket[6]);
                    label.Variables["FoilCheck"].SetValue(SPacket[7]);
                    label.Variables["FillingtoLong"].SetValue(SPacket[8]);
                    label.Variables["SerialNumber"].SetValue(SerialNumber);
                    label.Variables["ResourceNumber"].SetValue(Resource);
                    label.Variables["Description"].SetValue(Description);

                    //add more variables later to fill out label with other information like resource.

                    //What actually sends the print command:
                    label.Print(1);

                    ILabelPreviewSettings labelPreviewSettings = new LabelPreviewSettings();

                    labelPreviewSettings.PreviewToFile = false;                                    // if true, result will be the file name, if false, result will be a byte array.
                    labelPreviewSettings.ImageFormat = "jpg";                                      // file format of graphics.  Valid formats: JPG, PNG, BMP.
                    labelPreviewSettings.Width = this.ManifestPreview.Width;                            // Width of image to generate
                    labelPreviewSettings.Height = this.ManifestPreview.Height;                          // Height of image to generate
                    //labelPreviewSettings.Destination = this.textBoxFileName.Text;                // If PrintToFile is true, this is the name of the file that will be generated.
                    labelPreviewSettings.FormatPreviewSide = FormatPreviewSide.FrontSide;          // Which label side(s) to generate the image for.  
                    
                    // Generate Preview File
                    object imageObj = label.GetLabelPreview(labelPreviewSettings);

                    // Display image in UI
                    if (imageObj is byte[])
                    {
                        // When PrintToFiles = false
                        // Convert byte[] to Bitmap and set as image source for PictureBox control
                        ManifestPreview.Image = this.ByteToImage((byte[])imageObj);
                    }

                    SerialNumber = (Int32.Parse(SerialNumber) + 1).ToString().PadLeft(8, '0');
                    File.WriteAllText(path10, SerialNumber);
                    Serial0.Text = SerialNumber;
                }));
            }
        }
        #region For Pattyn Loop
        //when _shouldstop is asigned true the readloops on the completion of their next iteration will break out of their otherwise continuos loop
        // Reset Stop needs to be called before reconnecting to the printers otherwise it will skip over the while loop and immidiatly stop working.
        private volatile bool _PattynshouldStop;
        public void PattynRequestStop()
        {
            _PattynshouldStop = true;
        }
        public void PattynResetStop()
        {
            _PattynshouldStop = false;
        }
        #endregion
        #endregion
        #region Process Scanner Packet
        void ScannerConnect()
        {
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            try
            {
                for (int x = 0; x < 7; x++)
                {
                    string host = settings[4 + x];
                    if (host != string.Empty && host != null && host != "")
                    {
                        int port = Int32.Parse(settings[11 + x]);

                        TcpClient tcpClient;
                        tcpClient = new TcpClient();
                        //tcpClient.Connect(host, port);
                        if (!tcpClient.ConnectAsync(host, port).Wait(1500))
                        {
                            //fail
                            MessageBox.Show("License scanner " + x.ToString() + " Failed to connect!");
                        }
                        else
                        {
                            //success
                            new Thread(delegate ()
                            {
                                NetworkStream netstream = tcpClient.GetStream();

                            //Thread start another function to perma loop
                            ScannerLoop(new StreamReader(netstream), x);
                            }).Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + ex.ToString());
            }
        }
        void ScannerLoop(StreamReader reader,int Scanner)
        {
            Thread.CurrentThread.IsBackground = true;
            reader.BaseStream.ReadTimeout = 200;
            string path11 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");

            string responseData = string.Empty;
            string newline = string.Empty;

            while (!_ScannershouldStop)
            {
                string[] settings = File.ReadAllText(path11).Split(',');

                try { newline = reader.ReadLine(); }
                catch (Exception e)
                {
                    if (e.Message.Contains("Unable to read data from the transport connection: A connection attempt failed because the connected party did not properly respond after a period of time,"))
                    { }
                    else { MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + e.ToString()); }
                }

                if (newline != responseData)
                {
                    responseData = newline;
                    if (responseData != null)
                    {



                        Invoke(new Action(() =>
                        {
                            OutputTestBox.AppendText(responseData + System.Environment.NewLine);
                        }));
                    }
                }
            }

        }

        #region For ScannerLoop
        //when _shouldstop is asigned true the readloops on the completion of their next iteration will break out of their otherwise continuos loop
        // Reset Stop needs to be called before reconnecting to the printers otherwise it will skip over the while loop and immidiatly stop working.
        private volatile bool _ScannershouldStop;
        public void ScannerRequestStop()
        {
            _ScannershouldStop = true;
        }
        public void ScannerResetStop()
        {
            _ScannershouldStop = false;
        }

        #endregion
        #endregion
        */
        #endregion
        #region Totals
        private void Totals_TextChanged(object sender, EventArgs e)
        {
            string totals = PalletCount.Text + "," + TotalGross.Text + "," + TotalTare.Text + "," + TotalNet.Text;

            string path12 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\Totals.txt";
            path12 = path12.Replace("\r", "").Replace("\n", "");

            File.WriteAllText(path12, totals);
        }

        void LoadTotals()
        {
            string path12 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\Totals.txt";
            path12 = path12.Replace("\r", "").Replace("\n", "");

            string[] totals = File.ReadAllText(path12).Split(',');
            PalletCount.Text = totals[0];
            TotalGross.Text = totals[1];
            TotalTare.Text = totals[2];
            TotalNet.Text = totals[3];
        }

        private void ClearTotals_Click(object sender, EventArgs e)
        {
            string path12 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\Totals.txt";
            path12 = path12.Replace("\r", "").Replace("\n", "");

            File.WriteAllText(path12, "0,0,0,0");

            PalletCount.Text = "0";
            TotalGross.Text = "0";
            TotalTare.Text = "0";
            TotalNet.Text = "0";

            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User, "Cleared Totals");
        }
        #endregion
        #region Save and Load
        void LoadFPResources()
        {
            string path13 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\FreshpackResources.txt";
            path13 = path13.Replace("\r", "").Replace("\n", "");

            string[] resources = File.ReadAllLines(path13);
            string[] abc = resources[0].Split(',');
            string[] d = resources[1].Split(',');
            string[] e = resources[2].Split(',');
            string[] fghi = resources[3].Split(',');
            string[] j = resources[4].Split(',');
            string[] k = resources[5].Split(',');
            string[] l = resources[6].Split(',');
            for (int i = 1; i < 8; i++)
            {
                Control[] ct1 = this.Controls.Find("ABC" + i.ToString(), true);
                ComboBox ABC = ct1[0] as ComboBox;
                Control[] ct2 = this.Controls.Find("D" + i.ToString(), true);
                ComboBox D = ct2[0] as ComboBox;
                Control[] ct3 = this.Controls.Find("E" + i.ToString(), true);
                ComboBox E = ct3[0] as ComboBox;
                Control[] ct4 = this.Controls.Find("FGH" + i.ToString(), true);
                ComboBox FGHI = ct4[0] as ComboBox;
                Control[] ct5 = this.Controls.Find("I" + i.ToString(), true);
                ComboBox J = ct5[0] as ComboBox;
                Control[] ct6 = this.Controls.Find("J" + i.ToString(), true);
                ComboBox K = ct6[0] as ComboBox;
                Control[] ct7 = this.Controls.Find("K" + i.ToString(), true);
                ComboBox L = ct7[0] as ComboBox;

                ABC.Items.Clear();
                ABC.Items.AddRange(abc);
                D.Items.Clear();
                D.Items.AddRange(d);
                E.Items.Clear();
                E.Items.AddRange(e);
                FGHI.Items.Clear();
                FGHI.Items.AddRange(fghi);
                J.Items.Clear();
                J.Items.AddRange(j);
                K.Items.Clear();
                K.Items.AddRange(k);
                L.Items.Clear();
                L.Items.AddRange(l);
            }
        }
        void InitializeSerial()
        {
            string path10 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\SerialNumber.txt";
            path10 = path10.Replace("\r", "").Replace("\n", "");
            SerialNumber = File.ReadAllText(path10);
            Serial0.Text = SerialNumber;
        }
        private void Resource_Changed(object sender, EventArgs e)
        {
            string whichline = this.ActiveControl.Name[this.ActiveControl.Name.Length - 1].ToString();
            Control[] ct1 = this.Controls.Find("ABC" + whichline, true);
            ComboBox ABC = ct1[0] as ComboBox;
            Control[] ct2 = this.Controls.Find("D" + whichline, true);
            ComboBox D = ct2[0] as ComboBox;
            Control[] ct3 = this.Controls.Find("E" + whichline, true);
            ComboBox E = ct3[0] as ComboBox;
            Control[] ct4 = this.Controls.Find("FGH" + whichline, true);
            ComboBox FGH = ct4[0] as ComboBox;
            Control[] ct5 = this.Controls.Find("I" + whichline, true);
            ComboBox I = ct5[0] as ComboBox;
            Control[] ct6 = this.Controls.Find("J" + whichline, true);
            ComboBox J = ct6[0] as ComboBox;
            Control[] ct7 = this.Controls.Find("K" + whichline, true);
            ComboBox K = ct7[0] as ComboBox;
            Control[] ct8 = this.Controls.Find("Resource" + whichline, true);
            Label ResourceBox = ct8[0] as Label;

            try
            {
                if (ABC.SelectedIndex != -1 && D.SelectedIndex != -1 && E.SelectedIndex != -1 && FGH.SelectedIndex != -1 && I.SelectedIndex != -1 && J.SelectedIndex != -1 && K.SelectedIndex != -1)
                {
                    ResourceBox.Text = ABC.SelectedItem.ToString() + "-" + D.SelectedItem.ToString() + E.SelectedItem.ToString() + "-" + FGH.SelectedItem.ToString() + "-" + I.SelectedItem.ToString() + J.SelectedItem.ToString() + K.SelectedItem.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tell the IT department you have seen this error\n" + ex.ToString());
            }
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User, "Changed line " + whichline + " resource to " + ResourceBox.Text);
        }
        void Save()
        {
            string path13 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\SavedData.txt";
            path13 = path13.Replace("\r", "").Replace("\n", "");
            string currentdata = Resource1.Text + "," + Weight1.Text + "," + Time1.Text + "," + Serial1.Text + "," +
                                 Resource2.Text + "," + Weight2.Text + "," + Time2.Text + "," + Serial2.Text + "," +
                                 Resource3.Text + "," + Weight3.Text + "," + Time3.Text + "," + Serial3.Text + "," +
                                 Resource4.Text + "," + Weight4.Text + "," + Time4.Text + "," + Serial4.Text + "," +
                                 Resource5.Text + "," + Weight5.Text + "," + Time5.Text + "," + Serial5.Text + "," +
                                 Resource6.Text + "," + Weight6.Text + "," + Time6.Text + "," + Serial6.Text + "," +
                                 Resource7.Text + "," + Weight7.Text + "," + Time7.Text + "," + Serial7.Text + "," +
                                 Tunnel1.SelectedIndex + "," + Tunnel2.SelectedIndex + "," + Tunnel3.SelectedIndex + "," +
                                 Tunnel4.SelectedIndex + "," + Tunnel5.SelectedIndex + "," + Tunnel6.SelectedIndex + "," +
                                 Tunnel7.SelectedIndex + "," +
                                 Remark11.Text + "," + Remark12.Text + "," + Remark13.Text + "," +
                                 Remark21.Text + "," + Remark22.Text + "," + Remark23.Text + "," +
                                 Remark31.Text + "," + Remark32.Text + "," + Remark33.Text + "," +
                                 Remark41.Text + "," + Remark42.Text + "," + Remark43.Text + "," +
                                 Remark51.Text + "," + Remark52.Text + "," + Remark53.Text + "," +
                                 Remark61.Text + "," + Remark62.Text + "," + Remark63.Text + "," +
                                 Remark71.Text + "," + Remark72.Text + "," + Remark73.Text + ",";
            File.WriteAllText(path13, currentdata);
        }
        void Load_Saved()
        {
            string path13 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\SavedData.txt";
            path13 = path13.Replace("\r", "").Replace("\n", "");
            string[] SavedData = File.ReadAllText(path13).Split(',');
            Resource1.Text = SavedData[0]; Weight1.Text = SavedData[1]; Time1.Text = SavedData[2]; Serial1.Text = SavedData[3];
            Resource2.Text = SavedData[4]; Weight2.Text = SavedData[5]; Time2.Text = SavedData[6]; Serial2.Text = SavedData[7];
            Resource3.Text = SavedData[8]; Weight3.Text = SavedData[9]; Time3.Text = SavedData[10]; Serial3.Text = SavedData[11];
            Resource4.Text = SavedData[12]; Weight4.Text = SavedData[13]; Time4.Text = SavedData[14]; Serial4.Text = SavedData[15];
            Resource5.Text = SavedData[16]; Weight5.Text = SavedData[17]; Time5.Text = SavedData[18]; Serial5.Text = SavedData[19];
            Resource6.Text = SavedData[20]; Weight6.Text = SavedData[21]; Time6.Text = SavedData[22]; Serial6.Text = SavedData[23];
            Resource7.Text = SavedData[24]; Weight7.Text = SavedData[25]; Time7.Text = SavedData[26]; Serial7.Text = SavedData[27];
            Tunnel1.SelectedIndex = Int32.Parse(SavedData[28]);
            Tunnel2.SelectedIndex = Int32.Parse(SavedData[29]);
            Tunnel3.SelectedIndex = Int32.Parse(SavedData[30]);
            Tunnel4.SelectedIndex = Int32.Parse(SavedData[31]);
            Tunnel5.SelectedIndex = Int32.Parse(SavedData[32]);
            Tunnel6.SelectedIndex = Int32.Parse(SavedData[33]);
            Tunnel7.SelectedIndex = Int32.Parse(SavedData[34]);
            Remark11.Text = SavedData[35];
            Remark12.Text = SavedData[36];
            Remark13.Text = SavedData[37];
            Remark21.Text = SavedData[38];
            Remark22.Text = SavedData[39];
            Remark23.Text = SavedData[40];
            Remark31.Text = SavedData[41];
            Remark32.Text = SavedData[42];
            Remark33.Text = SavedData[43];
            Remark41.Text = SavedData[44];
            Remark42.Text = SavedData[45];
            Remark43.Text = SavedData[46];
            Remark51.Text = SavedData[47];
            Remark52.Text = SavedData[48];
            Remark53.Text = SavedData[49];
            Remark61.Text = SavedData[50];
            Remark62.Text = SavedData[51];
            Remark63.Text = SavedData[52];
            Remark71.Text = SavedData[53];
            Remark72.Text = SavedData[54];
            Remark73.Text = SavedData[55];
        }
        private void Primary_TextChanged(object sender, EventArgs e)
        {
            Save();
        }
        private void Remark_Text_Changed(object sender, EventArgs e)
        {
            Save();
        }

        private void RemarkValidated(object sender, EventArgs e)
        {
            TextBox ct = (TextBox)sender;
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(User, "Changed " + ct.Name + " to " + ct.Text);
        }

        #endregion


    }
}
