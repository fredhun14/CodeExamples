using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Security.Cryptography;
using System.IO.Ports;
using System.IO;
using OpcLabs.EasyOpc.UA;
using OpcLabs.EasyOpc.UA.OperationModel;
namespace testing_app
{

    public partial class Form1 : Form
    {
        EasyUAClient UAClient;
        EasyUAClient SubClient1;
        EasyUAClient SubClient2;
        EasyUAClient SubClient3;
        EasyUAClient SubClient4;
        EasyUAClient SubClient5;
        EasyUAClient SubClient6;
        EasyUAClient SubClient7;
        UAEndpointDescriptor EndpointDescriptor;
        UANodeDescriptor nodeDescriptor;



        public Form1()
        {
            InitializeComponent();
            _shouldStop = false;
            UAClient = new EasyUAClient();
            SubClient1 = new EasyUAClient();
            SubClient2 = new EasyUAClient();
            SubClient3 = new EasyUAClient();
            SubClient4 = new EasyUAClient();
            SubClient5 = new EasyUAClient();
            SubClient6 = new EasyUAClient();
            SubClient7 = new EasyUAClient();
        }
        #region Irrelevant
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
        private volatile bool _shouldStop;
        public void RequestStop()
        {
            _shouldStop = true;
        }
        public void ResetStop()
        {
            _shouldStop = false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value++;
            new Thread(delegate ()
            {
                threadloop(Convert.ToInt32(numericUpDown1.Value));
            }).Start();
        }
        public void threadloop(int threadcount)
        {
            Thread.CurrentThread.IsBackground = true;
            while (!_shouldStop)
            {
                wait(500);
                Invoke(new Action(() => { textBox1.AppendText("Loop ran in thread: " + threadcount.ToString()); }));
            }
        }
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
        private void button2_Click(object sender, EventArgs e)
        {
            RequestStop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ResetStop();
        }

        private void Encrypt_Click(object sender, EventArgs e)
        {
            string password = textBox2.Text;
            EncryptedTextBox.Text = Encryptpassword(password); 
        }
        string Encryptpassword(string password)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
        string Encryptusername(string username)
        {
            username.ToLower();
            char[] cusername = username.ToCharArray();
            for (int f = 0; f < cusername.Length; f++)
            {
                cusername[f] += (char)5;
            }
            string eusername = new string(cusername);
            return eusername;
        }
        string Decryptusername(string username)
        {
            char[] cusername = username.ToCharArray();
            for (int f = 0; f < cusername.Length; f++)
            {
                cusername[f] -= (char)5;
            }
            string dusername = new string(cusername);
            return dusername;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Test2 login = new Test2();
            var dialogresult = login.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label1.Text = SecLevel.ToString();
        }
        string DaycodeReturn()
        {
            //NOTE THERE IS NO ALPHA CODE HERE
            string thereturn = "";
            thereturn = thereturn + JulianDateGenerator();
            thereturn = thereturn + DateTime.Now.ToString("y").Substring(DateTime.Now.ToString("y").Length - 1);
            return thereturn;
        }
        public string JulianDateGenerator()
        {
            string Julian = "";
            int add;
            DateTime now = DateTime.Now;
            if (now.Hour < 10)
            { now = now.AddDays(1); }
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

        private void button6_Click(object sender, EventArgs e)
        {
            label2.Text = DaycodeReturn();
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            Invoke(new Action(() =>
            {
                textBox3.AppendText(indata);
            }));
            
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            //OPCTestBox.Text = easyUAClient1.ReadValue("opc.tcp://10.10.1.50:49320", "nsu=TOP Server;s=Channel1.Device1.Tag1").ToString();
            EndpointDescriptor = "opc.tcp://127.0.0.1:49320";
            nodeDescriptor = "nsu=KEPServerEX ;ns=2;s=Channel1.Device1.Tag1";
            EndpointDescriptor.UserName = "administrator";
            EndpointDescriptor.Password = "U43T2ufiYk4YcrexOCmiA8jJiX6iFn";
            EndpointDescriptor.UserIdentity = OpcLabs.BaseLib.IdentityModel.User.UserIdentity.CreateUserNameIdentity("administrator", "U43T2ufiYk4YcrexOCmiA8jJiX6iFn");
            OPCTagBox.Text = nodeDescriptor.ToString();

            makerectangles();
            

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;
            toolTip1.ForeColor = System.Drawing.Color.Red;
            toolTip1.BackColor = System.Drawing.Color.Black;
            toolTip1.OwnerDraw = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.panel1, "Image 1 tip");
            toolTip1.SetToolTip(this.panel2, "Image 2 tip");
            createbinary();
            readbinary();
            
            RandomTimer.Enabled = true;
        }
        #region old
        #region irrelevant
        private void button7_Click(object sender, EventArgs e)
        {
            string password = textBox2.Text;
            EncryptedTextBox.Text = Decryptusername(password);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            label3.Text = (numericUpDown2.Value % numericUpDown3.Value).ToString();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string path8 = @"\\sffnt4\fill\228106CobCorn.txt";
            path8 = path8.Replace("\r", "").Replace("\n", "");
            string[] lines = File.ReadAllLines(path8);
            string[] items;
            string[,] data = new string[lines.Length, 8];
            for (int c = 0; c < lines.Length; c++)
            {
                items = lines[c].Split('\t');
                for (int r = 0; r < 8; r++)
                {
                    data[c, r] = items[r];
                }
            }
            path8 = "";
            string newline = "";
            for (int x = 0; x <lines.Length; x++)
            {
                newline = data[x,0].PadLeft(5, ' ') + ",00000," + data[x, 1].PadRight(16, ' ') + ",\"" + data[x, 2].PadLeft(8,'0').ToLower() + "\",\"" +"08/16/21" + "\"," + data[x, 4].PadLeft(2,'0') + ",\"" + data[x, 5] + "\"," + data[x, 6].PadLeft(8, '0') + ",\"\",\"" + data[x, 7].PadRight(10,' ') + "\",\"" + /*data[x, 8]*/"" + "\",\"" + "\",\"" + "\",\"" + /*data[x, 9]*/"" + "\",\"" + /*data[x, 10]*/"" + "\"";
                string exppath = @"C:\Users\mikeh.SMITHFROZENFOOD\Desktop\cobtestdata.txt";
                using (StreamWriter w = File.AppendText(exppath))
                {
                    w.WriteLine(newline);
                };
            }
        }

        private void Concat_Click(object sender, EventArgs e)
        {
            string[] one = textBox4.Text.Split(','), two = textBox5.Text.Split(','), three = textBox6.Text.Split(',');
            string concat = "";
            for (int x = 0; x < one.Length; x++)
                for (int y = 0; y < two.Length; y++)
                    for (int z = 0; z < three.Length; z++)
                    {
                        concat = one[x] + two[y] + three[z] + ",";
                        using (StreamWriter w = File.AppendText("Concat.txt"))
                        {
                            w.Write(concat);
                        }
                    }
        }
        #endregion
        private void button10_Click(object sender, EventArgs e)
        {
            //Read Value Method that outputs value opctestbox
            //Errors are caught and displayed in message box
            try
            {
                nodeDescriptor = OPCTagBox.Text;
                OPCTestBox.Text = UAClient.ReadValue(EndpointDescriptor, nodeDescriptor).ToString();
            }
            catch (UAException uaException)
            {
               MessageBox.Show("Read Failed: " + uaException.GetBaseException().Message);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //Set the UA Endpoint property of the UADataDialog control so it knows which OPC UA Server to browse
            uaDataDialog1.EndpointDescriptor = EndpointDescriptor;
            //Output the selected UA Node to the uaItemTextbox
            if (uaDataDialog1.ShowDialog() == DialogResult.OK)
            {
                nodeDescriptor = uaDataDialog1.NodeDescriptor.NodeId;
                OPCTagBox.Text = uaDataDialog1.NodeDescriptor.NodeId;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                nodeDescriptor = OPCTagBox.Text;
                UAClient.WriteValue(EndpointDescriptor, nodeDescriptor, OPCTestBox.Text);
            }
            catch (UAException uaException)
            {
                MessageBox.Show("Read Failed: " + uaException.GetBaseException().Message);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            UAClient.DataChangeNotification += UAClient_DataChangeNotification;
            UAClient.SubscribeDataChange(EndpointDescriptor, nodeDescriptor, 1000);
        }
        private void UAClient_DataChangeNotification(object sender, EasyUADataChangeNotificationEventArgs e)
        {
            //Output the value of the OnDataChange event to the lowCodeSubscribeTextbox
            //Errors are caught and displayed in eventLog ListBox
            if (e.Succeeded)
                OPCTestBox.Text = e.AttributeData.DisplayValue();
            else
                MessageBox.Show("Failure: " + e.Exception.Message);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            nodeDescriptor = OPCTagBox.Text;
            SubClient2.DataChangeNotification += UAClient_DataChangeNotification2;
            SubClient2.SubscribeDataChange(EndpointDescriptor, nodeDescriptor, 1000);
        }
        private void UAClient_DataChangeNotification2(object sender, EasyUADataChangeNotificationEventArgs e)
        {
            //Output the value of the OnDataChange event to the lowCodeSubscribeTextbox
            //Errors are caught and displayed in eventLog ListBox
            if (e.Succeeded)
                OPCTestBox2.Text = e.AttributeData.DisplayValue();
            else
                MessageBox.Show("Failure: " + e.Exception.Message);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook excelworkBook;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet;
            Microsoft.Office.Interop.Excel.Range excelCellrange;
            DataTable table = new DataTable();
            table.Columns.Add("column1", typeof(string));
            table.Columns.Add("column2", typeof(string));
            table.Rows.Add(Excel1.Text, excel2.Text);

            // Start Excel and get Application object.  
            excel = new Microsoft.Office.Interop.Excel.Application();
            // for making Excel visible  
            excel.Visible = false;
            excel.DisplayAlerts = false;
            // Creation a new Workbook  
            excelworkBook = excel.Workbooks.Add(Type.Missing);
            // Workk sheet  
            excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
            excelSheet.Name = "Test work sheet";

            excelSheet.Cells[1, 1] = "Sample test data";
            excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();

            // now we resize the columns  
            excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[2, 2]];
            excelCellrange.EntireColumn.AutoFit();
            Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
            border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            border.Weight = 2d;
            FormattingExcelCells(excelCellrange, "White", System.Drawing.Color.Black, false);
            
        }
        public void FormattingExcelCells(Microsoft.Office.Interop.Excel.Range range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
        {
            range.Interior.Color = System.Drawing.ColorTranslator.FromHtml(HTMLcolorCode);
            range.Font.Color = System.Drawing.ColorTranslator.ToOle(fontColor);
            if (IsFontbool == true)
            {
                range.Font.Bold = IsFontbool;
            }
        }

#endregion


        private Point start;
        private Point starting_Location;
        private string currently_being_dragged;
        private string currently_being_over;
        Rectangle rectangle1;
        Rectangle rectangle2;
        Rectangle rectangle3;
        void makerectangles()
        {
            rectangle1 = new Rectangle(panel1.Location, panel1.Size);
            rectangle2 = new Rectangle(panel2.Location, panel2.Size);
            rectangle3 = new Rectangle(panel3.Location, panel3.Size);
        }
        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            Control ThePanel = (Control)sender;
            if (e.Button == MouseButtons.Left && ThePanel.BackgroundImage != null)
            {
                start = e.Location;
                ThePanel.BringToFront();
                starting_Location = ThePanel.Location;
                currently_being_dragged = ThePanel.Name;

                ThePanel.MouseUp += new MouseEventHandler(panel_MouseUp_Dragging);
                ThePanel.MouseMove += new MouseEventHandler(panel_MouseMove);

                Cursor.Clip = new Rectangle(this.Location,this.Size);
            }
        }

        private void panel_MouseUp_Dragging(object sender, MouseEventArgs e)
        {
            Control ThePanel = (Control)sender;
            var relativePoint = panel4.PointToClient(Cursor.Position);
            if (rectangle1.Contains(relativePoint) && currently_being_dragged != panel1.Name)
            {
                currently_being_over = panel1.Name;
            }
            if (rectangle2.Contains(relativePoint) && currently_being_dragged != panel2.Name)
            {
                currently_being_over = panel2.Name;
            }
            if (rectangle3.Contains(relativePoint) && currently_being_dragged != panel3.Name)
            {
                currently_being_over = panel3.Name;
            }
            if (currently_being_over != null)
            {
                Control[] ct = this.Controls.Find(currently_being_over, true);
                Image oldimage = ct[0].BackgroundImage;
                string oldtip = toolTip1.GetToolTip(ct[0]);
                toolTip1.SetToolTip(ct[0], toolTip1.GetToolTip(ThePanel));
                toolTip1.SetToolTip(ThePanel, oldtip);
                ct[0].BackgroundImage = ThePanel.BackgroundImage;
                ThePanel.BackgroundImage = oldimage;
            }
            ThePanel.MouseMove -= new MouseEventHandler(panel_MouseMove);
            ThePanel.MouseUp -= new MouseEventHandler(panel_MouseUp_Dragging);

            Cursor.Clip = new Rectangle();
            ThePanel.Location = starting_Location;
            currently_being_dragged = null;
            currently_being_over = null;
        }
        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            Control ThePanel = (Control)sender;
            ThePanel.Location = new Point(ThePanel.Location.X - (start.X - e.X), ThePanel.Location.Y - (start.Y - e.Y));
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            MessageBox.Show(sender.ToString() + "   :   " + e.KeyChar);
        }

        private void FocusBox_Leave(object sender, EventArgs e)
        {
           
            FocusBox.Focus();
        }

        private void FocusBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Return || e.KeyData == Keys.Escape || e.KeyData == Keys.Right || e.KeyData == Keys.Left || e.KeyData == Keys.Down || e.KeyData == Keys.Up || e.KeyData == Keys.Tab)
            {
                if (e.KeyData == Keys.Tab)
                {
                    if(panel4.Visible == false) { panel4.Visible = true; }
                    else { panel4.Visible = false; }
                }
                else
                {
                    MessageBox.Show(sender.ToString() + "   :   " + e.KeyData);
                }
            }
        }

        private void toolTip1_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            e.DrawBorder();
            e.DrawText();
        }
        void createbinary()
        {
            FileStream fs = new FileStream("C:\\Users\\mikeh.SMITHFROZENFOOD\\source\\repos\\testing app\\testing app\\bin\\Debug\\test.bin", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(true);  //writing bool value
            bw.Write(Convert.ToByte('a')); //writing byte
            bw.Write('a');                 //writing character
            bw.Write("string");            //string
            bw.Write(123);                 //number
            bw.Write(123.12);              // double value
            bw.Write(false);
            bw.Close();
        }
        void readbinary()
        {
            FileStream fs1 = new FileStream("C:\\Users\\mikeh.SMITHFROZENFOOD\\source\\repos\\testing app\\testing app\\bin\\Debug\\test.bin", FileMode.Open);
            BinaryReader br = new BinaryReader(fs1);
            bool b = br.ReadBoolean();
            byte _byte = br.ReadByte();
            char _char = br.ReadChar();
            string _string = br.ReadString();
            int _int = br.ReadInt16();
            double _dbl = br.ReadDouble();
            bool f = br.ReadBoolean();
            string[] s = new string[100];
            for (int x = 0; x<50; x++)
            {
                s[x] = br.Read().ToString();
            }
            for (int x = 0; x < 50; x++)
            {
                BinaryOutbox.AppendText(s[x] + System.Environment.NewLine);
            }
            BinaryOutbox.AppendText(b.ToString());
            BinaryOutbox.AppendText(_byte.ToString());
            BinaryOutbox.AppendText(_char.ToString());
            BinaryOutbox.AppendText(_string.ToString());
            BinaryOutbox.AppendText(_int.ToString());
            BinaryOutbox.AppendText(_dbl.ToString());
            BinaryOutbox.AppendText(f.ToString());
            Console.WriteLine(b);
            Console.WriteLine(_byte);
            Console.WriteLine(_char);
            Console.WriteLine(_string);
            Console.WriteLine(_int);
            Console.WriteLine(_dbl);
            fs1.Close();
        }

        private void RandomTimer_Tick(object sender, EventArgs e)
        {
            Random rand = new Random();
            int chancemod1 = 2;
            int chancemod2 = 20;
            int chancemod3 = 200;
            int chancemod4 = 2000;
            int rand1 = rand.Next(0, 10);
            int rand2 = rand.Next(0, 100);
            int rand3 = rand.Next(0, 1000);
            int rand4 = rand.Next(0, 10000);
            RandomLabel1.Text = rand1.ToString();
            RandomLabel2.Text = rand2.ToString();
            RandomLabel3.Text = rand3.ToString();
            RandomLabel4.Text = rand4.ToString();
            if (rand1 < 5 - chancemod1) { RandomLabel1.BackColor = System.Drawing.Color.Red; }
            else { RandomLabel1.BackColor = System.Drawing.Color.Green; }
            if (rand2 < 50 - chancemod2) { RandomLabel2.BackColor = System.Drawing.Color.Red; }
            else { RandomLabel2.BackColor = System.Drawing.Color.Green; }
            if (rand3 < 500 - chancemod3) { RandomLabel3.BackColor = System.Drawing.Color.Red; }
            else { RandomLabel3.BackColor = System.Drawing.Color.Green; }
            if (rand4 < 5000 - chancemod4) { RandomLabel4.BackColor = System.Drawing.Color.Red; }
            else { RandomLabel4.BackColor = System.Drawing.Color.Green; }
        }

        void RainbowTableGenerator()
        {
            char[] CHashMe = new char[5];
            CHashMe[0] = (char)33;
            CHashMe[1] = (char)33;
            CHashMe[2] = (char)33;
            CHashMe[3] = (char)33;
            CHashMe[4] = (char)33;
            string HashMe;
            string Rainbow1;
            string Rainbow2;
            for (double x = 0; x < 7339040224; x++) 
            {
                if (CHashMe[0] < 127) { CHashMe[0]++; }
                else if(CHashMe[1] < 127)
                { CHashMe[0] = (char)33; CHashMe[1]++; }
                else if (CHashMe[2] < 127) 
                { CHashMe[1] = (char)33; CHashMe[2]++; }
                else if (CHashMe[3] < 127) 
                { CHashMe[2] = (char)33; CHashMe[3]++; }
                else if (CHashMe[4] < 127) 
                { CHashMe[3] = (char)33; CHashMe[4]++; }
                else 
                { break; }
                Rainbow1 = CHashMe[0].ToString() + CHashMe[1].ToString() + CHashMe[2].ToString() + CHashMe[3].ToString() + CHashMe[4].ToString();

                MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(Rainbow1);
                byte[] hash = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }

                Rainbow2 = sb.ToString();

                using (StreamWriter w = File.AppendText("Rainbow1.txt"))
                {
                    w.WriteLine(Rainbow1 + "╨" + Rainbow2);
                };
            }
        }

        private void RainbowGenerate_Click(object sender, EventArgs e)
        {
            RainbowTableGenerator();
        }

        string returnsastring()
        {
            return "1234.5678";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string adouble = "12345.12345";
            StringLabel.Text = String.Format("{0:0}",double.Parse(adouble));
        }

        private void TimeCheck_Click(object sender, EventArgs e)
        {
            DateTime date1 = DateTime.Now;
            DateTime date2 = DateTime.Now;
            date2 = date2.AddMinutes(-10);
            if(date1.AddMinutes(-60) < date2)
            {
                TimeCheck.Text = "<";
            }
            else { TimeCheck.Text = ">"; }

        }

        private void button18_Click(object sender, EventArgs e)
        {
            bool ToWrite = true;
            UANodeDescriptor NodeDescriptor = "nsu=KEPServerEX ;ns=2;s=Channel2.Device1.BoolTest";
            UAClient.WriteValue(EndpointDescriptor, NodeDescriptor, ToWrite);
        }
    }
}
