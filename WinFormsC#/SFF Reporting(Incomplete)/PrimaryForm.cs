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
using System.Data.SqlTypes;
using NiceLabel.SDK;
using System.Reflection;
using System.Globalization;

namespace SFF_Reporting
{
    public partial class ClearSumby : Form
    {
        public ClearSumby()
        {
            InitializeComponent();
        }
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
        #region Generate Neccesary Files
        void GenerateNeccesaryFiles()
        {
            string path11 = @"GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            if (!File.Exists(path11))
            {
                File.WriteAllText(path11, @",,,,,,,,,,,,,,,,,,7,6,");
            }
        }
        #endregion
        #region Primary Form Load/ Close
        private void PrimaryForm_Load(object sender, EventArgs e)
        {
            GenerateNeccesaryFiles();
            InitializePrintEngine();
            GetPrinters();
        }
        private void PrimaryForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PrintEngineFactory.PrintEngine.Shutdown();
        }
        #endregion
        #region Helper Methods
        void GetPrinters()
        {
            PrinterSelect.Items.Clear();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                PrinterSelect.Items.Add(printer);
            }
        }
        void TopLevelSelectedChanged()
        {
            if (CheckColumnsAvailble() && !CSVInsteadCheck.Checked)
            {
                AddColumns();
                if (PortraitorLandscape.SelectedIndex == 0)
                {
                    Column1.Visible = true;
                    Column2.Visible = true;
                    Column3.Visible = true;
                    Column4.Visible = true;
                    Column5.Visible = true;
                    Column6.Visible = false;
                    Column7.Visible = false;
                    Column6.SelectedIndex = -1;
                    Column7.SelectedIndex = -1;

                    TColumn1.Visible = true;
                    TColumn2.Visible = true;
                    TColumn3.Visible = true;
                    TColumn4.Visible = true;
                    TColumn5.Visible = false;
                    TColumn6.Visible = false;
                    TColumn5.SelectedIndex = -1;
                    TColumn6.SelectedIndex = -1;

                }
                if (PortraitorLandscape.SelectedIndex == 1)
                {
                    Column1.Visible = true;
                    Column2.Visible = true;
                    Column3.Visible = true;
                    Column4.Visible = true;
                    Column5.Visible = true;
                    Column6.Visible = true;
                    Column7.Visible = true;

                    TColumn1.Visible = true;
                    TColumn2.Visible = true;
                    TColumn3.Visible = true;
                    TColumn4.Visible = true;
                    TColumn5.Visible = true;
                    TColumn6.Visible = true;


                }
                OrderBy.Visible = true;
                AscorDesc.Visible = true;
                sumcollumnslabel.Visible = true;
                columnslabel.Visible = true;
                totalcolumnslabel.Visible = true;
                orderbylabel.Visible = true;
                ascendordescendlabel.Visible = true;
            }
            else if(CheckColumnsAvailble() && CSVInsteadCheck.Checked)
            {
                AddColumns();
                Column1.Visible = true;
                Column2.Visible = true;
                Column3.Visible = true;
                Column4.Visible = true;
                Column5.Visible = true;
                Column6.Visible = true;
                Column7.Visible = true;

                TColumn1.Visible = false;
                TColumn2.Visible = false;
                TColumn3.Visible = false;
                TColumn4.Visible = false;
                TColumn5.Visible = false;
                TColumn6.Visible = false;

                OrderBy.Visible = true;
                AscorDesc.Visible = true;
                sumcollumnslabel.Visible = true;
                columnslabel.Visible = true;
                totalcolumnslabel.Visible = false;
                orderbylabel.Visible = true;
                ascendordescendlabel.Visible = true;
            }
            else
            {
                Column1.Visible = false;
                Column2.Visible = false;
                Column3.Visible = false;
                Column4.Visible = false;
                Column5.Visible = false;
                Column6.Visible = false;
                Column7.Visible = false;

                TColumn1.Visible = false;
                TColumn2.Visible = false;
                TColumn3.Visible = false;
                TColumn4.Visible = false;
                TColumn5.Visible = false;
                TColumn6.Visible = false;

                OrderBy.Visible = false;
                AscorDesc.Visible = false;
                sumcollumnslabel.Visible = false;
                columnslabel.Visible = false;
                totalcolumnslabel.Visible = false;
                orderbylabel.Visible = false;
                ascendordescendlabel.Visible = false;
            }
        }
        void GeneratePrintPreview()
        {
            if (CSVInsteadCheck.Checked == false)
            {
                switch (PortraitorLandscape.SelectedIndex)
                {
                    case 0:
                        if (SumColumnsBy.SelectedIndex != -1)
                        { PortraitSumedPP(); }
                        else
                        {
                            PortraitPP();
                        }
                        break;
                    case 1:
                        if (SumColumnsBy.SelectedIndex != -1)
                        { LandscapeSumedPP(); }
                        else
                        {
                            LandscapePP();
                        }
                        break;
                }
            }
            else
            {
                if (SumColumnsBy.SelectedIndex != -1)
                { GenerateSummedCSV(); }
                else
                {
                    GenerateCSV();
                }
            }
        }
        void GenerateCSV()
        {
            CSVBox.Text = "";
            string path11 = @"GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            int dbsetting = 0, tbsetting = 0, starttimesetting = 0;
            string timedatename = "";
            int rowsperpage = 42;
            DateTime start = DateRangeSelector.SelectionStart;
            DateTime end = DateRangeSelector.SelectionEnd;
            string[] sumcolumns;
            switch (FPKPKGGEN.SelectedIndex)
            {
                case 0:
                    sumcolumns = new string[7] { "Metal_Present", "Tare_Weight", "Net_Weight", "Check_Weight", "Clips_Present", "Foil_Check", "Filling_To_Long" };
                    timedatename = "Time_Date";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 5;
                            tbsetting = 6;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 13;
                            tbsetting = 14;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 1:
                    sumcolumns = new string[2] { "PALLET_SIZE", "PALLET_WEIGHT" };
                    timedatename = "Date_Time";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 3;
                            tbsetting = 4;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 11;
                            tbsetting = 12;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 2:
                    timedatename = "Date_Time";
                    sumcolumns = new string[1] { "CPM" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 7;
                            tbsetting = 8;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 15;
                            tbsetting = 16;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 3:
                    timedatename = "Time_Date";
                    sumcolumns = new string[5] { "Poly_UseL1", "Poly_UseL2", "Job_Count", "Check_Weigher_Count", "Evo_Print_Count" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 9;
                            tbsetting = 10;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 17;
                            tbsetting = 18;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                default:
                    sumcolumns = new string[1] { "" };
                    dbsetting = 0;
                    tbsetting = 0;
                    break;
            }
            DateTime Start = fixstarttime(start, Int32.Parse(settings[starttimesetting]));
            DateTime End = fixendtime(end, Int32.Parse(settings[starttimesetting]));
            string[] columns = new string[7];
            int ColumnCount = 0, Tcolumncount = 0;
            bool selfsort = true;
            SQLData[] AllRecords = null;
            bool[] Summable = new bool[7];

            for (int x = 1; x < 8; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedIndex != -1 && cb.SelectedItem.ToString() != "Pallet Count" && cb.SelectedItem.ToString() != "Record Count")
                {
                    columns[x - 1] = cb.SelectedItem.ToString();
                    ColumnCount++;
                    for (int y = 0; y < sumcolumns.Length; y++)
                    {
                        if (cb.SelectedItem.ToString() == sumcolumns[y])
                        {
                            Summable[x] = true;
                        }
                    }
                }
            }
            if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
            {
                string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString = "SELECT ";
                for (int x = 0; x <= ColumnCount; x++)
                {
                    cmdString = cmdString + columns[x] + ", ";
                }

                for(int g = 0; g < 10; g++)
                {
                    if(cmdString.Substring(cmdString.Length - 1, 1) == " " || cmdString.Substring(cmdString.Length - 1, 1) == ",")
                    {
                        cmdString = cmdString.Remove(cmdString.Length - 1, 1);
                    }
                }

                cmdString = cmdString + " FROM " + settings[tbsetting] + " WHERE " + timedatename + " >= @dtStart AND " + timedatename + " <= @dtEnd";

                if (OnlyPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE = '1'";
                }
                else if (NoPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE != '1'";
                }

                if (OrderBy.SelectedIndex != -1 && AscorDesc.SelectedIndex != -1 && !selfsort)
                {
                    cmdString = cmdString + " ORDER BY " + OrderBy.SelectedItem + " " + AscorDesc.SelectedItem + ";";
                }
                else { cmdString = cmdString + ";"; }

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
                            var list = new List<SQLData>();
                            while (reader.Read())
                            {
                                list.Add(new SQLData());
                                int tcolumnstart = ColumnCount;
                                if (ColumnCount > 0)
                                { try { list[list.Count - 1].Col1 = reader.GetString(0); } catch (Exception t) { }; }
                                if (ColumnCount > 1)
                                { try { list[list.Count - 1].Col2 = reader.GetString(1); } catch (Exception t) { }; }
                                if (ColumnCount > 2)
                                { try { list[list.Count - 1].Col3 = reader.GetString(2); } catch (Exception t) { }; }
                                if (ColumnCount > 3)
                                { try { list[list.Count - 1].Col4 = reader.GetString(3); } catch (Exception t) { }; }
                                if (ColumnCount > 4)
                                { try { list[list.Count - 1].Col5 = reader.GetString(4); } catch (Exception t) { }; }
                                if (ColumnCount > 5)
                                { try { list[list.Count - 1].Col6 = reader.GetString(5); } catch (Exception t) { }; }
                                if (ColumnCount > 6)
                                { try { list[list.Count - 1].Col7 = reader.GetString(6); } catch (Exception t) { }; }

                                AllRecords = list.ToArray();
                            }
                        }
                    }
                }
                if (AllRecords != null)
                {
                    int RecordCount = 0;
                    string[,] Totals = new string[AllRecords.Length, 7];
                    for (int x = 0; x < AllRecords.Length; x++)
                    {
                        Totals[x, 0] = AllRecords[x].Col1;
                        Totals[x, 1] = AllRecords[x].Col2;
                        Totals[x, 2] = AllRecords[x].Col3;
                        Totals[x, 3] = AllRecords[x].Col4;
                        Totals[x, 4] = AllRecords[x].Col5;
                        Totals[x, 5] = AllRecords[x].Col6;
                        Totals[x, 6] = AllRecords[x].Col7;

                        RecordCount++;
                    }
                    #region sorting
                    int sortby = findsortby();
                    bool sortbyisSummable = false;
                    Control[] cts = this.Controls.Find("Column" + (sortby + 1).ToString(), true);
                    ComboBox cbs = cts[0] as ComboBox;
                    if (Summable[sortby] || cbs.SelectedItem.ToString() == "Pallet Count" || cbs.SelectedItem.ToString() == "Record Count")
                    { sortbyisSummable = true; }
                    if (AscorDesc.SelectedIndex == 0)
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length - 1; y++)
                            {
                                string[] holding = new string[7];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) < 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) < Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                { break; }
                            }
                        }
                    }
                    else
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length - 1; y++)
                            {
                                string[] holding = new string[7];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) > 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) > Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                { break; }
                            }
                        }
                    }
                    #endregion
                    int pagecount = 1;
                    bool done = true;
                    int rows = 0;

                    do
                    {
                        string records = "";

                        for (int x = (pagecount - 1) * rowsperpage; x <= RecordCount && x < pagecount * rowsperpage; x++)
                        {
                            if (Totals[x, 0] == null) { Totals[x, 0] = ""; }
                            if (Totals[x, 1] == null) { Totals[x, 1] = ""; }
                            if (Totals[x, 2] == null) { Totals[x, 2] = ""; }
                            if (Totals[x, 3] == null) { Totals[x, 3] = ""; }
                            if (Totals[x, 4] == null) { Totals[x, 4] = ""; }
                            if (Totals[x, 5] == null) { Totals[x, 5] = ""; }
                            if (Totals[x, 6] == null) { Totals[x, 6] = ""; }
                           
                            if (Totals[x, 0] != "")
                            { records = records + Totals[x, 0] + CSVSeparator.Text; }
                            if (Totals[x, 1] != "")
                            { records = records + Totals[x, 1] + CSVSeparator.Text; }
                            if (Totals[x, 2] != "")
                            { records = records + Totals[x, 2] + CSVSeparator.Text; }
                            if (Totals[x, 3] != "")
                            { records = records + Totals[x, 3] + CSVSeparator.Text; }
                            if (Totals[x, 4] != "")
                            { records = records + Totals[x, 4] + CSVSeparator.Text; }
                            if (Totals[x, 5] != "")
                            { records = records + Totals[x, 5] + CSVSeparator.Text; }
                            if (Totals[x, 6] != "")
                            { records = records + Totals[x, 6] + CSVSeparator.Text; }
                            records = records + "\r\n";
                            rows++;
                        }
                        if (rows >= RecordCount)
                        { done = true; }
                        CSVBox.AppendText(records);
                        CSVBox.Visible = true;
                        pagecount++;
                    }
                    while (!done);
                }
            }
        }
        void GenerateSummedCSV()
        {
            CSVBox.Text = "";
            string path11 = @"GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            int dbsetting = 0, tbsetting = 0, starttimesetting = 0;
            string[] sumcolumns;
            string timedatename = "";
            bool selfsort = false;
            int rowsperpage = 42;
            DateTime start = DateRangeSelector.SelectionStart;
            DateTime end = DateRangeSelector.SelectionEnd;
            SQLData[] AllRecords = null;

            switch (FPKPKGGEN.SelectedIndex)
            {
                case 0:
                    sumcolumns = new string[7] { "Metal_Present", "Tare_Weight", "Net_Weight", "Check_Weight", "Clips_Present", "Foil_Check", "Filling_To_Long" };
                    timedatename = "Time_Date";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 5;
                            tbsetting = 6;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 13;
                            tbsetting = 14;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 1:
                    sumcolumns = new string[2] { "PALLET_SIZE", "PALLET_WEIGHT" };
                    timedatename = "Date_Time";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 3;
                            tbsetting = 4;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 11;
                            tbsetting = 12;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 2:
                    timedatename = "Date_Time";
                    sumcolumns = new string[1] { "CPM" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 7;
                            tbsetting = 8;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 15;
                            tbsetting = 16;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 3:
                    timedatename = "Time_Date";
                    sumcolumns = new string[5] { "Poly_UseL1", "Poly_UseL2", "Job_Count", "Check_Weigher_Count", "Evo_Print_Count" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 9;
                            tbsetting = 10;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 17;
                            tbsetting = 18;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                default:
                    sumcolumns = new string[1] { "" };
                    dbsetting = 0;
                    tbsetting = 0;
                    break;
            }
            DateTime Start = fixstarttime(start, Int32.Parse(settings[starttimesetting]));
            DateTime End = fixendtime(end, Int32.Parse(settings[starttimesetting]));
            for (int x = 0; x < sumcolumns.Length; x++)
            {
                if (SumColumnsBy.SelectedIndex == -1 && OrderBy.SelectedItem.ToString() != sumcolumns[x])
                {
                    selfsort = false;
                }
                else { selfsort = true; break; }
            }
            string[] columns = new string[8];
            int sumbycolumn = 0;
            for (int x = 1; x < 8; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedItem.ToString() == SumColumnsBy.SelectedItem.ToString())
                {
                    columns[0] = cb.SelectedItem.ToString();
                    sumbycolumn = x;
                    break;
                }
            }
            int columncount = 1, Tcolumncount = 0;
            int skipped = 0;
            for (int x = 1; x < 8; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedIndex == -1 || x == sumbycolumn || cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                { skipped++; }
                else { columns[x - skipped] = cb.SelectedItem.ToString(); columncount++; }
            }

            bool[] Summable = new bool[7];
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < sumcolumns.Length; y++)
                {
                    if (columns[x] == sumcolumns[y]) { Summable[x] = true; break; }
                    else { Summable[x] = false; }
                }
            }
            if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
            {
                string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString = "SELECT ";
                //Change this creation of cmd string to be in a loop to iterate the number of columns
                //and then patch together the string one column at a time checking if it has a -1 selected index each time  or pallet count
                //this should allow to also check other things like for Pallet count and pivot later in the function to address that issue
                for (int x = 0; x <= columncount; x++)
                {
                    cmdString = cmdString + columns[x] + ", ";
                }

                for (int g = 0; g < 10; g++)
                {
                    if (cmdString.Substring(cmdString.Length - 1, 1) == " " || cmdString.Substring(cmdString.Length - 1, 1) == ",")
                    {
                        cmdString = cmdString.Remove(cmdString.Length - 1, 1);
                    }
                }
                
                cmdString = cmdString + " FROM " + settings[tbsetting] + " WHERE " + timedatename + " >= @dtStart AND " + timedatename + " <= @dtEnd";
                if (OnlyPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE = '1'";
                }
                else if (NoPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE != '1'";
                }
                if (OrderBy.SelectedIndex != -1 && AscorDesc.SelectedIndex != -1 && !selfsort)
                {
                    cmdString = cmdString + " ORDER BY " + OrderBy.SelectedItem + " " + AscorDesc.SelectedItem + ";";
                }
                else { cmdString = cmdString + ";"; }

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
                            var list = new List<SQLData>();
                            while (reader.Read())
                            {
                                //list.Add(new SQLData { Col1 = reader.GetString(0), Col2 = reader.GetString(1), Col3 = reader.GetString(2), Col4 = reader.GetString(3), Col5 = reader.GetString(4), Col6 = reader.GetString(5), Col7 = reader.GetString(6), TCol1 = reader.GetString(7), TCol2 = reader.GetString(8), TCol3 = reader.GetString(9), TCol4 = reader.GetString(10), TCol5 = reader.GetString(11), TCol6 = reader.GetString(12) });
                                list.Add(new SQLData());
                                int tcolumnstart = columncount;
                                if (columncount > 0)
                                { try { list[list.Count - 1].Col1 = reader.GetString(0); } catch (Exception t) { }; }
                                if (columncount > 1)
                                { try { list[list.Count - 1].Col2 = reader.GetString(1); } catch (Exception t) { }; }
                                if (columncount > 2)
                                { try { list[list.Count - 1].Col3 = reader.GetString(2); } catch (Exception t) { }; }
                                if (columncount > 3)
                                { try { list[list.Count - 1].Col4 = reader.GetString(3); } catch (Exception t) { }; }
                                if (columncount > 4)
                                { try { list[list.Count - 1].Col5 = reader.GetString(4); } catch (Exception t) { }; }
                                if (columncount > 5)
                                { try { list[list.Count - 1].Col6 = reader.GetString(5); } catch (Exception t) { }; }
                                if (columncount > 6)
                                { try { list[list.Count - 1].Col7 = reader.GetString(6); } catch (Exception t) { }; }


                                if (Tcolumncount > 0)
                                { try { list[list.Count - 1].TCol1 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 1)
                                { try { list[list.Count - 1].TCol2 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 2)
                                { try { list[list.Count - 1].TCol3 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 3)
                                { try { list[list.Count - 1].TCol4 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 4)
                                { try { list[list.Count - 1].TCol5 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 5)
                                { try { list[list.Count - 1].TCol6 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                AllRecords = list.ToArray();
                            }
                        }
                    }
                }

                if (AllRecords != null)
                {
                    int RecordCount = 0;
                    string[,] Totals = new string[AllRecords.Length, 8];
                    int uniquerecords = 0;
                    for (int x = 0; x < AllRecords.Length; x++)
                    {
                        RecordCount++;
                        bool found = false;
                        for (int y = 0; y < AllRecords.Length; y++)
                        {
                            if (AllRecords[x].Col1 == Totals[y, 0])
                            {
                                if (Summable[1]) { Totals[y, 1] = (Int32.Parse(Totals[y, 1]) + Int32.Parse(AllRecords[x].Col2)).ToString(); }
                                if (Summable[2]) { Totals[y, 2] = (Int32.Parse(Totals[y, 2]) + Int32.Parse(AllRecords[x].Col3)).ToString(); }
                                if (Summable[3]) { Totals[y, 3] = (Int32.Parse(Totals[y, 3]) + Int32.Parse(AllRecords[x].Col4)).ToString(); }
                                if (Summable[4]) { Totals[y, 4] = (Int32.Parse(Totals[y, 4]) + Int32.Parse(AllRecords[x].Col5)).ToString(); }
                                if (Summable[5]) { Totals[y, 5] = (Int32.Parse(Totals[y, 5]) + Int32.Parse(AllRecords[x].Col6)).ToString(); }
                                if (Summable[6]) { Totals[y, 6] = (Int32.Parse(Totals[y, 6]) + Int32.Parse(AllRecords[x].Col7)).ToString(); }
                                if (Column1.SelectedIndex != -1)
                                {
                                    if (Column1.SelectedItem.ToString() == "Pallet Count" || Column1.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 0] = (Int32.Parse(Totals[y, 0]) + 1).ToString(); }
                                }
                                if (Column2.SelectedIndex != -1)
                                {
                                    if (Column2.SelectedItem.ToString() == "Pallet Count" || Column2.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 1] = (Int32.Parse(Totals[y, 1]) + 1).ToString(); }
                                }
                                if (Column3.SelectedIndex != -1)
                                {
                                    if (Column3.SelectedItem.ToString() == "Pallet Count" || Column3.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 2] = (Int32.Parse(Totals[y, 2]) + 1).ToString(); }
                                }
                                if (Column4.SelectedIndex != -1)
                                {
                                    if (Column4.SelectedItem.ToString() == "Pallet Count" || Column4.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 3] = (Int32.Parse(Totals[y, 3]) + 1).ToString(); }
                                }
                                if (Column5.SelectedIndex != -1)
                                {
                                    if (Column5.SelectedItem.ToString() == "Pallet Count" || Column5.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 4] = (Int32.Parse(Totals[y, 4]) + 1).ToString(); }
                                }
                                if (Column6.SelectedIndex != -1)
                                {
                                    if (Column6.SelectedItem.ToString() == "Pallet Count" || Column6.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 5] = (Int32.Parse(Totals[y, 5]) + 1).ToString(); }
                                }
                                if (Column7.SelectedIndex != -1)
                                {
                                    if (Column7.SelectedItem.ToString() == "Pallet Count" || Column7.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 6] = (Int32.Parse(Totals[y, 6]) + 1).ToString(); }
                                }
                                Totals[y, 7] = (Int32.Parse(Totals[y, 7]) + 1).ToString();
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            for (int y = 0; y < AllRecords.Length; y++)
                            {
                                if (Totals[y, 0] == null || Totals[y, 0] == string.Empty || Totals[y, 0] == "")
                                {
                                    Totals[y, 0] = AllRecords[x].Col1;
                                    Totals[y, 1] = AllRecords[x].Col2;
                                    Totals[y, 2] = AllRecords[x].Col3;
                                    Totals[y, 3] = AllRecords[x].Col4;
                                    Totals[y, 4] = AllRecords[x].Col5;
                                    Totals[y, 5] = AllRecords[x].Col6;
                                    Totals[y, 6] = AllRecords[x].Col7;
                                    Totals[y, 7] = "1";
                                    if (Column1.SelectedIndex != -1)
                                    {
                                        if (Column1.SelectedItem.ToString() == "Pallet Count" || Column1.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 0] = "1"; }
                                    }
                                    if (Column2.SelectedIndex != -1)
                                    {
                                        if (Column2.SelectedItem.ToString() == "Pallet Count" || Column2.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 1] = "1"; }
                                    }
                                    if (Column3.SelectedIndex != -1)
                                    {
                                        if (Column3.SelectedItem.ToString() == "Pallet Count" || Column3.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 2] = "1"; }
                                    }
                                    if (Column4.SelectedIndex != -1)
                                    {
                                        if (Column4.SelectedItem.ToString() == "Pallet Count" || Column4.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 3] = "1"; }
                                    }
                                    if (Column5.SelectedIndex != -1)
                                    {
                                        if (Column5.SelectedItem.ToString() == "Pallet Count" || Column5.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 4] = "1"; }
                                    }
                                    if (Column6.SelectedIndex != -1)
                                    {
                                        if (Column6.SelectedItem.ToString() == "Pallet Count" || Column6.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 5] = "1"; }
                                    }
                                    if (Column7.SelectedIndex != -1)
                                    {
                                        if (Column7.SelectedItem.ToString() == "Pallet Count" || Column7.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 6] = "1"; }
                                    }
                                    uniquerecords++;
                                    break;
                                }
                            }
                        }
                    }
                    #region sorting
                    int sortby = findsortby();
                    bool sortbyisSummable = false;
                    Control[] cts = this.Controls.Find("Column" + (sortby + 1).ToString(), true);
                    ComboBox cbs = cts[0] as ComboBox;
                    if (Summable[sortby] || cbs.SelectedItem.ToString() == "Pallet Count" || cbs.SelectedItem.ToString() == "Record Count")
                    { sortbyisSummable = true; }
                    if (AscorDesc.SelectedIndex == 0)
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length; y++)
                            {
                                string[] holding = new string[8];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) < 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) < Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                { break; }
                            }
                        }
                    }
                    else
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length; y++)
                            {
                                string[] holding = new string[8];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) > 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) > Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                { break; }
                            }
                        }
                    }
                    #endregion
                    int pagecount = 1;
                    bool done = true;
                    int rows = 0;
                    for (int x = 1; x < 8; x++)
                    {
                        Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                        ComboBox cb = ct[0] as ComboBox;
                        if (cb.SelectedIndex != -1)
                        {
                            if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                            { columncount++; }
                        }
                    }
                    do
                    {
                        string records = "";

                        for (int x = (pagecount - 1) * rowsperpage; x <= uniquerecords && x < pagecount * rowsperpage; x++)
                        {
                            if (Totals[x, 0] == null) { Totals[x, 0] = ""; }
                            if (Totals[x, 1] == null) { Totals[x, 1] = ""; }
                            if (Totals[x, 2] == null) { Totals[x, 2] = ""; }
                            if (Totals[x, 3] == null) { Totals[x, 3] = ""; }
                            if (Totals[x, 4] == null) { Totals[x, 4] = ""; }
                            if (Totals[x, 5] == null) { Totals[x, 5] = ""; }
                            if (Totals[x, 6] == null) { Totals[x, 6] = ""; }

                            if (Totals[x, 0] != "")
                            { records = records + Totals[x, 0] + CSVSeparator.Text; }
                            if (Totals[x, 1] != "")
                            { records = records + Totals[x, 1] + CSVSeparator.Text; }
                            if (Totals[x, 2] != "")
                            { records = records + Totals[x, 2] + CSVSeparator.Text; }
                            if (Totals[x, 3] != "")
                            { records = records + Totals[x, 3] + CSVSeparator.Text; }
                            if (Totals[x, 4] != "")
                            { records = records + Totals[x, 4] + CSVSeparator.Text; }
                            if (Totals[x, 5] != "")
                            { records = records + Totals[x, 5] + CSVSeparator.Text; }
                            if (Totals[x, 6] != "")
                            { records = records + Totals[x, 6] + CSVSeparator.Text; }
                            records = records + "\r\n";
                            rows++;
                        }
                        if (rows >= uniquerecords)
                        { done = true; }
                        CSVBox.AppendText(records);
                        CSVBox.Visible = true;
                        pagecount++;
                    } while (!done);
                }
                else { MessageBox.Show("No data found in selected date range for selected report."); }
            }
            else { MessageBox.Show("Missing configs"); }
        }
        void ExportCSV()
        {
           if(SaveCSVDialog.ShowDialog() == DialogResult.OK)
            {
                string file = SaveCSVDialog.FileName;
                string path11 = @"GeneralSettings.txt";
                path11 = path11.Replace("\r", "").Replace("\n", "");
                string[] settings = File.ReadAllText(path11).Split(',');
                int dbsetting = 0, tbsetting = 0, starttimesetting = 0;
                string timedatename = "";
                int rowsperpage = 10000;
                DateTime start = DateRangeSelector.SelectionStart;
                DateTime end = DateRangeSelector.SelectionEnd;
                string[] sumcolumns;
                switch (FPKPKGGEN.SelectedIndex)
                {
                    case 0:
                        sumcolumns = new string[7] { "Metal_Present", "Tare_Weight", "Net_Weight", "Check_Weight", "Clips_Present", "Foil_Check", "Filling_To_Long" };
                        timedatename = "Time_Date";
                        switch (LocationSelection.SelectedIndex)
                        {
                            case 0:
                                dbsetting = 5;
                                tbsetting = 6;
                                starttimesetting = 19;
                                break;
                            case 1:
                                dbsetting = 13;
                                tbsetting = 14;
                                starttimesetting = 20;
                                break;
                        }
                        break;
                    case 1:
                        sumcolumns = new string[2] { "PALLET_SIZE", "PALLET_WEIGHT" };
                        timedatename = "Date_Time";
                        switch (LocationSelection.SelectedIndex)
                        {
                            case 0:
                                dbsetting = 3;
                                tbsetting = 4;
                                starttimesetting = 19;
                                break;
                            case 1:
                                dbsetting = 11;
                                tbsetting = 12;
                                starttimesetting = 20;
                                break;
                        }
                        break;
                    case 2:
                        timedatename = "Date_Time";
                        sumcolumns = new string[1] { "CPM" };
                        switch (LocationSelection.SelectedIndex)
                        {
                            case 0:
                                dbsetting = 7;
                                tbsetting = 8;
                                starttimesetting = 19;
                                break;
                            case 1:
                                dbsetting = 15;
                                tbsetting = 16;
                                starttimesetting = 20;
                                break;
                        }
                        break;
                    case 3:
                        timedatename = "Time_Date";
                        sumcolumns = new string[5] { "Poly_UseL1", "Poly_UseL2", "Job_Count", "Check_Weigher_Count", "Evo_Print_Count" };
                        switch (LocationSelection.SelectedIndex)
                        {
                            case 0:
                                dbsetting = 9;
                                tbsetting = 10;
                                starttimesetting = 19;
                                break;
                            case 1:
                                dbsetting = 17;
                                tbsetting = 18;
                                starttimesetting = 20;
                                break;
                        }
                        break;
                    default:
                        sumcolumns = new string[1] { "" };
                        dbsetting = 0;
                        tbsetting = 0;
                        break;
                }
                DateTime Start = fixstarttime(start, Int32.Parse(settings[starttimesetting]));
                DateTime End = fixendtime(end, Int32.Parse(settings[starttimesetting]));
                string[] columns = new string[7];
                int ColumnCount = 0, Tcolumncount = 0;
                bool selfsort = true;
                SQLData[] AllRecords = null;
                bool[] Summable = new bool[7];

                for (int x = 1; x < 8; x++)
                {
                    Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                    ComboBox cb = ct[0] as ComboBox;
                    if (cb.SelectedIndex != -1 && cb.SelectedItem.ToString() != "Pallet Count" && cb.SelectedItem.ToString() != "Record Count")
                    {
                        columns[x - 1] = cb.SelectedItem.ToString();
                        ColumnCount++;
                        for (int y = 0; y < sumcolumns.Length; y++)
                        {
                            if (cb.SelectedItem.ToString() == sumcolumns[y])
                            {
                                Summable[x] = true;
                            }
                        }
                    }
                }
                if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
                {
                    string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    string cmdString = "SELECT ";
                    for (int x = 0; x <= ColumnCount; x++)
                    {
                        cmdString = cmdString + columns[x] + ", ";
                    }

                    for (int g = 0; g < 10; g++)
                    {
                        if (cmdString.Substring(cmdString.Length - 1, 1) == " " || cmdString.Substring(cmdString.Length - 1, 1) == ",")
                        {
                            cmdString = cmdString.Remove(cmdString.Length - 1, 1);
                        }
                    }

                    cmdString = cmdString + " FROM " + settings[tbsetting] + " WHERE " + timedatename + " >= @dtStart AND " + timedatename + " <= @dtEnd";

                    if (OnlyPalletSize1.Checked == true)
                    {
                        cmdString = cmdString + " AND PALLET_SIZE = '1'";
                    }
                    else if (NoPalletSize1.Checked == true)
                    {
                        cmdString = cmdString + " AND PALLET_SIZE != '1'";
                    }

                    if (OrderBy.SelectedIndex != -1 && AscorDesc.SelectedIndex != -1 && !selfsort)
                    {
                        cmdString = cmdString + " ORDER BY " + OrderBy.SelectedItem + " " + AscorDesc.SelectedItem + ";";
                    }
                    else { cmdString = cmdString + ";"; }

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
                                var list = new List<SQLData>();
                                while (reader.Read())
                                {
                                    list.Add(new SQLData());
                                    int tcolumnstart = ColumnCount;
                                    if (ColumnCount > 0)
                                    { try { list[list.Count - 1].Col1 = reader.GetString(0); } catch (Exception t) { }; }
                                    if (ColumnCount > 1)
                                    { try { list[list.Count - 1].Col2 = reader.GetString(1); } catch (Exception t) { }; }
                                    if (ColumnCount > 2)
                                    { try { list[list.Count - 1].Col3 = reader.GetString(2); } catch (Exception t) { }; }
                                    if (ColumnCount > 3)
                                    { try { list[list.Count - 1].Col4 = reader.GetString(3); } catch (Exception t) { }; }
                                    if (ColumnCount > 4)
                                    { try { list[list.Count - 1].Col5 = reader.GetString(4); } catch (Exception t) { }; }
                                    if (ColumnCount > 5)
                                    { try { list[list.Count - 1].Col6 = reader.GetString(5); } catch (Exception t) { }; }
                                    if (ColumnCount > 6)
                                    { try { list[list.Count - 1].Col7 = reader.GetString(6); } catch (Exception t) { }; }

                                    AllRecords = list.ToArray();
                                }
                            }
                        }
                    }
                    if (AllRecords != null)
                    {
                        int RecordCount = 0;
                        string[,] Totals = new string[AllRecords.Length, 7];
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            Totals[x, 0] = AllRecords[x].Col1;
                            Totals[x, 1] = AllRecords[x].Col2;
                            Totals[x, 2] = AllRecords[x].Col3;
                            Totals[x, 3] = AllRecords[x].Col4;
                            Totals[x, 4] = AllRecords[x].Col5;
                            Totals[x, 5] = AllRecords[x].Col6;
                            Totals[x, 6] = AllRecords[x].Col7;

                            RecordCount++;
                        }
                        #region sorting
                        int sortby = findsortby();
                        bool sortbyisSummable = false;
                        Control[] cts = this.Controls.Find("Column" + (sortby + 1).ToString(), true);
                        ComboBox cbs = cts[0] as ComboBox;
                        if (Summable[sortby] || cbs.SelectedItem.ToString() == "Pallet Count" || cbs.SelectedItem.ToString() == "Record Count")
                        { sortbyisSummable = true; }
                        if (AscorDesc.SelectedIndex == 0)
                        {
                            for (int x = 0; x < AllRecords.Length; x++)
                            {
                                for (int y = 0; y < AllRecords.Length - 1; y++)
                                {
                                    string[] holding = new string[7];
                                    if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                    {
                                        if (!sortbyisSummable)
                                        {
                                            if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) < 0)
                                            {
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    holding[f] = Totals[y + 1, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y + 1, f] = Totals[y, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y, f] = holding[f];
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Int32.Parse(Totals[y, sortby]) < Int32.Parse(Totals[y + 1, sortby]))
                                            {
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    holding[f] = Totals[y + 1, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y + 1, f] = Totals[y, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y, f] = holding[f];
                                                }
                                            }
                                        }
                                    }
                                    else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                    { break; }
                                }
                            }
                        }
                        else
                        {
                            for (int x = 0; x < AllRecords.Length; x++)
                            {
                                for (int y = 0; y < AllRecords.Length - 1; y++)
                                {
                                    string[] holding = new string[7];
                                    if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                    {
                                        if (!sortbyisSummable)
                                        {
                                            if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) > 0)
                                            {
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    holding[f] = Totals[y + 1, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y + 1, f] = Totals[y, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y, f] = holding[f];
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Int32.Parse(Totals[y, sortby]) > Int32.Parse(Totals[y + 1, sortby]))
                                            {
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    holding[f] = Totals[y + 1, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y + 1, f] = Totals[y, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y, f] = holding[f];
                                                }
                                            }
                                        }
                                    }
                                    else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                    { break; }
                                }
                            }
                        }
                        #endregion
                        int pagecount = 1;
                        bool done = false;
                        int rows = 0;

                        do
                        {
                            string records = "";

                            for (int x = (pagecount - 1) * rowsperpage; x <= RecordCount && x < pagecount * rowsperpage; x++)
                            {
                                if (Totals[x, 0] == null) { Totals[x, 0] = ""; }
                                if (Totals[x, 1] == null) { Totals[x, 1] = ""; }
                                if (Totals[x, 2] == null) { Totals[x, 2] = ""; }
                                if (Totals[x, 3] == null) { Totals[x, 3] = ""; }
                                if (Totals[x, 4] == null) { Totals[x, 4] = ""; }
                                if (Totals[x, 5] == null) { Totals[x, 5] = ""; }
                                if (Totals[x, 6] == null) { Totals[x, 6] = ""; }

                                if (Totals[x, 0] != "")
                                { records = records + Totals[x, 0] + CSVSeparator.Text; }
                                if (Totals[x, 1] != "")
                                { records = records + Totals[x, 1] + CSVSeparator.Text; }
                                if (Totals[x, 2] != "")
                                { records = records + Totals[x, 2] + CSVSeparator.Text; }
                                if (Totals[x, 3] != "")
                                { records = records + Totals[x, 3] + CSVSeparator.Text; }
                                if (Totals[x, 4] != "")
                                { records = records + Totals[x, 4] + CSVSeparator.Text; }
                                if (Totals[x, 5] != "")
                                { records = records + Totals[x, 5] + CSVSeparator.Text; }
                                if (Totals[x, 6] != "")
                                { records = records + Totals[x, 6] + CSVSeparator.Text; }
                                records = records + "\r\n";
                                rows++;
                            }
                            if (rows >= RecordCount)
                            { done = true; }
                            using (StreamWriter w = File.AppendText(file))
                            {
                                w.WriteLine(records);
                            };
                            pagecount++;
                        }
                        while (!done);
                    }
                }
            }
        }
        void ExportSummedCSV()
        {
            if (SaveCSVDialog.ShowDialog() == DialogResult.OK)
            {
                string file = SaveCSVDialog.FileName;
                string path11 = @"GeneralSettings.txt";
                path11 = path11.Replace("\r", "").Replace("\n", "");
                string[] settings = File.ReadAllText(path11).Split(',');
                int dbsetting = 0, tbsetting = 0, starttimesetting = 0;
                string[] sumcolumns;
                string timedatename = "";
                bool selfsort = false;
                int rowsperpage = 10000;
                DateTime start = DateRangeSelector.SelectionStart;
                DateTime end = DateRangeSelector.SelectionEnd;
                SQLData[] AllRecords = null;

                switch (FPKPKGGEN.SelectedIndex)
                {
                    case 0:
                        sumcolumns = new string[7] { "Metal_Present", "Tare_Weight", "Net_Weight", "Check_Weight", "Clips_Present", "Foil_Check", "Filling_To_Long" };
                        timedatename = "Time_Date";
                        switch (LocationSelection.SelectedIndex)
                        {
                            case 0:
                                dbsetting = 5;
                                tbsetting = 6;
                                starttimesetting = 19;
                                break;
                            case 1:
                                dbsetting = 13;
                                tbsetting = 14;
                                starttimesetting = 20;
                                break;
                        }
                        break;
                    case 1:
                        sumcolumns = new string[2] { "PALLET_SIZE", "PALLET_WEIGHT" };
                        timedatename = "Date_Time";
                        switch (LocationSelection.SelectedIndex)
                        {
                            case 0:
                                dbsetting = 3;
                                tbsetting = 4;
                                starttimesetting = 19;
                                break;
                            case 1:
                                dbsetting = 11;
                                tbsetting = 12;
                                starttimesetting = 20;
                                break;
                        }
                        break;
                    case 2:
                        timedatename = "Date_Time";
                        sumcolumns = new string[1] { "CPM" };
                        switch (LocationSelection.SelectedIndex)
                        {
                            case 0:
                                dbsetting = 7;
                                tbsetting = 8;
                                starttimesetting = 19;
                                break;
                            case 1:
                                dbsetting = 15;
                                tbsetting = 16;
                                starttimesetting = 20;
                                break;
                        }
                        break;
                    case 3:
                        timedatename = "Time_Date";
                        sumcolumns = new string[5] { "Poly_UseL1", "Poly_UseL2", "Job_Count", "Check_Weigher_Count", "Evo_Print_Count" };
                        switch (LocationSelection.SelectedIndex)
                        {
                            case 0:
                                dbsetting = 9;
                                tbsetting = 10;
                                starttimesetting = 19;
                                break;
                            case 1:
                                dbsetting = 17;
                                tbsetting = 18;
                                starttimesetting = 20;
                                break;
                        }
                        break;
                    default:
                        sumcolumns = new string[1] { "" };
                        dbsetting = 0;
                        tbsetting = 0;
                        break;
                }
                DateTime Start = fixstarttime(start, Int32.Parse(settings[starttimesetting]));
                DateTime End = fixendtime(end, Int32.Parse(settings[starttimesetting]));
                for (int x = 0; x < sumcolumns.Length; x++)
                {
                    if (SumColumnsBy.SelectedIndex == -1 && OrderBy.SelectedItem.ToString() != sumcolumns[x])
                    {
                        selfsort = false;
                    }
                    else { selfsort = true; break; }
                }
                string[] columns = new string[8];
                int sumbycolumn = 0;
                for (int x = 1; x < 8; x++)
                {
                    Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                    ComboBox cb = ct[0] as ComboBox;
                    if (cb.SelectedItem.ToString() == SumColumnsBy.SelectedItem.ToString())
                    {
                        columns[0] = cb.SelectedItem.ToString();
                        sumbycolumn = x;
                        break;
                    }
                }
                int columncount = 1, Tcolumncount = 0;
                int skipped = 0;
                for (int x = 1; x < 8; x++)
                {
                    Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                    ComboBox cb = ct[0] as ComboBox;
                    if (cb.SelectedIndex == -1 || x == sumbycolumn || cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                    { skipped++; }
                    else { columns[x - skipped] = cb.SelectedItem.ToString(); columncount++; }
                }

                bool[] Summable = new bool[7];
                for (int x = 0; x < 7; x++)
                {
                    for (int y = 0; y < sumcolumns.Length; y++)
                    {
                        if (columns[x] == sumcolumns[y]) { Summable[x] = true; break; }
                        else { Summable[x] = false; }
                    }
                }
                if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
                {
                    string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    string cmdString = "SELECT ";
                    //Change this creation of cmd string to be in a loop to iterate the number of columns
                    //and then patch together the string one column at a time checking if it has a -1 selected index each time  or pallet count
                    //this should allow to also check other things like for Pallet count and pivot later in the function to address that issue
                    for (int x = 0; x <= columncount; x++)
                    {
                        cmdString = cmdString + columns[x] + ", ";
                    }

                    for (int g = 0; g < 10; g++)
                    {
                        if (cmdString.Substring(cmdString.Length - 1, 1) == " " || cmdString.Substring(cmdString.Length - 1, 1) == ",")
                        {
                            cmdString = cmdString.Remove(cmdString.Length - 1, 1);
                        }
                    }

                    cmdString = cmdString + " FROM " + settings[tbsetting] + " WHERE " + timedatename + " >= @dtStart AND " + timedatename + " <= @dtEnd";
                    if (OnlyPalletSize1.Checked == true)
                    {
                        cmdString = cmdString + " AND PALLET_SIZE = '1'";
                    }
                    else if (NoPalletSize1.Checked == true)
                    {
                        cmdString = cmdString + " AND PALLET_SIZE != '1'";
                    }
                    if (OrderBy.SelectedIndex != -1 && AscorDesc.SelectedIndex != -1 && !selfsort)
                    {
                        cmdString = cmdString + " ORDER BY " + OrderBy.SelectedItem + " " + AscorDesc.SelectedItem + ";";
                    }
                    else { cmdString = cmdString + ";"; }

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
                                var list = new List<SQLData>();
                                while (reader.Read())
                                {
                                    //list.Add(new SQLData { Col1 = reader.GetString(0), Col2 = reader.GetString(1), Col3 = reader.GetString(2), Col4 = reader.GetString(3), Col5 = reader.GetString(4), Col6 = reader.GetString(5), Col7 = reader.GetString(6), TCol1 = reader.GetString(7), TCol2 = reader.GetString(8), TCol3 = reader.GetString(9), TCol4 = reader.GetString(10), TCol5 = reader.GetString(11), TCol6 = reader.GetString(12) });
                                    list.Add(new SQLData());
                                    int tcolumnstart = columncount;
                                    if (columncount > 0)
                                    { try { list[list.Count - 1].Col1 = reader.GetString(0); } catch (Exception t) { }; }
                                    if (columncount > 1)
                                    { try { list[list.Count - 1].Col2 = reader.GetString(1); } catch (Exception t) { }; }
                                    if (columncount > 2)
                                    { try { list[list.Count - 1].Col3 = reader.GetString(2); } catch (Exception t) { }; }
                                    if (columncount > 3)
                                    { try { list[list.Count - 1].Col4 = reader.GetString(3); } catch (Exception t) { }; }
                                    if (columncount > 4)
                                    { try { list[list.Count - 1].Col5 = reader.GetString(4); } catch (Exception t) { }; }
                                    if (columncount > 5)
                                    { try { list[list.Count - 1].Col6 = reader.GetString(5); } catch (Exception t) { }; }
                                    if (columncount > 6)
                                    { try { list[list.Count - 1].Col7 = reader.GetString(6); } catch (Exception t) { }; }


                                    if (Tcolumncount > 0)
                                    { try { list[list.Count - 1].TCol1 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                    if (Tcolumncount > 1)
                                    { try { list[list.Count - 1].TCol2 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                    if (Tcolumncount > 2)
                                    { try { list[list.Count - 1].TCol3 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                    if (Tcolumncount > 3)
                                    { try { list[list.Count - 1].TCol4 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                    if (Tcolumncount > 4)
                                    { try { list[list.Count - 1].TCol5 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                    if (Tcolumncount > 5)
                                    { try { list[list.Count - 1].TCol6 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                    AllRecords = list.ToArray();
                                }
                            }
                        }
                    }

                    if (AllRecords != null)
                    {
                        int RecordCount = 0;
                        string[,] Totals = new string[AllRecords.Length, 8];
                        int uniquerecords = 0;
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            RecordCount++;
                            bool found = false;
                            for (int y = 0; y < AllRecords.Length; y++)
                            {
                                if (AllRecords[x].Col1 == Totals[y, 0])
                                {
                                    if (Summable[1]) { Totals[y, 1] = (Int32.Parse(Totals[y, 1]) + Int32.Parse(AllRecords[x].Col2)).ToString(); }
                                    if (Summable[2]) { Totals[y, 2] = (Int32.Parse(Totals[y, 2]) + Int32.Parse(AllRecords[x].Col3)).ToString(); }
                                    if (Summable[3]) { Totals[y, 3] = (Int32.Parse(Totals[y, 3]) + Int32.Parse(AllRecords[x].Col4)).ToString(); }
                                    if (Summable[4]) { Totals[y, 4] = (Int32.Parse(Totals[y, 4]) + Int32.Parse(AllRecords[x].Col5)).ToString(); }
                                    if (Summable[5]) { Totals[y, 5] = (Int32.Parse(Totals[y, 5]) + Int32.Parse(AllRecords[x].Col6)).ToString(); }
                                    if (Summable[6]) { Totals[y, 6] = (Int32.Parse(Totals[y, 6]) + Int32.Parse(AllRecords[x].Col7)).ToString(); }
                                    if (Column1.SelectedIndex != -1)
                                    {
                                        if (Column1.SelectedItem.ToString() == "Pallet Count" || Column1.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 0] = (Int32.Parse(Totals[y, 0]) + 1).ToString(); }
                                    }
                                    if (Column2.SelectedIndex != -1)
                                    {
                                        if (Column2.SelectedItem.ToString() == "Pallet Count" || Column2.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 1] = (Int32.Parse(Totals[y, 1]) + 1).ToString(); }
                                    }
                                    if (Column3.SelectedIndex != -1)
                                    {
                                        if (Column3.SelectedItem.ToString() == "Pallet Count" || Column3.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 2] = (Int32.Parse(Totals[y, 2]) + 1).ToString(); }
                                    }
                                    if (Column4.SelectedIndex != -1)
                                    {
                                        if (Column4.SelectedItem.ToString() == "Pallet Count" || Column4.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 3] = (Int32.Parse(Totals[y, 3]) + 1).ToString(); }
                                    }
                                    if (Column5.SelectedIndex != -1)
                                    {
                                        if (Column5.SelectedItem.ToString() == "Pallet Count" || Column5.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 4] = (Int32.Parse(Totals[y, 4]) + 1).ToString(); }
                                    }
                                    if (Column6.SelectedIndex != -1)
                                    {
                                        if (Column6.SelectedItem.ToString() == "Pallet Count" || Column6.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 5] = (Int32.Parse(Totals[y, 5]) + 1).ToString(); }
                                    }
                                    if (Column7.SelectedIndex != -1)
                                    {
                                        if (Column7.SelectedItem.ToString() == "Pallet Count" || Column7.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 6] = (Int32.Parse(Totals[y, 6]) + 1).ToString(); }
                                    }
                                    Totals[y, 7] = (Int32.Parse(Totals[y, 7]) + 1).ToString();
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                for (int y = 0; y < AllRecords.Length; y++)
                                {
                                    if (Totals[y, 0] == null || Totals[y, 0] == string.Empty || Totals[y, 0] == "")
                                    {
                                        Totals[y, 0] = AllRecords[x].Col1;
                                        Totals[y, 1] = AllRecords[x].Col2;
                                        Totals[y, 2] = AllRecords[x].Col3;
                                        Totals[y, 3] = AllRecords[x].Col4;
                                        Totals[y, 4] = AllRecords[x].Col5;
                                        Totals[y, 5] = AllRecords[x].Col6;
                                        Totals[y, 6] = AllRecords[x].Col7;
                                        Totals[y, 7] = "1";
                                        if (Column1.SelectedIndex != -1)
                                        {
                                            if (Column1.SelectedItem.ToString() == "Pallet Count" || Column1.SelectedItem.ToString() == "Record Count")
                                            { Totals[y, 0] = "1"; }
                                        }
                                        if (Column2.SelectedIndex != -1)
                                        {
                                            if (Column2.SelectedItem.ToString() == "Pallet Count" || Column2.SelectedItem.ToString() == "Record Count")
                                            { Totals[y, 1] = "1"; }
                                        }
                                        if (Column3.SelectedIndex != -1)
                                        {
                                            if (Column3.SelectedItem.ToString() == "Pallet Count" || Column3.SelectedItem.ToString() == "Record Count")
                                            { Totals[y, 2] = "1"; }
                                        }
                                        if (Column4.SelectedIndex != -1)
                                        {
                                            if (Column4.SelectedItem.ToString() == "Pallet Count" || Column4.SelectedItem.ToString() == "Record Count")
                                            { Totals[y, 3] = "1"; }
                                        }
                                        if (Column5.SelectedIndex != -1)
                                        {
                                            if (Column5.SelectedItem.ToString() == "Pallet Count" || Column5.SelectedItem.ToString() == "Record Count")
                                            { Totals[y, 4] = "1"; }
                                        }
                                        if (Column6.SelectedIndex != -1)
                                        {
                                            if (Column6.SelectedItem.ToString() == "Pallet Count" || Column6.SelectedItem.ToString() == "Record Count")
                                            { Totals[y, 5] = "1"; }
                                        }
                                        if (Column7.SelectedIndex != -1)
                                        {
                                            if (Column7.SelectedItem.ToString() == "Pallet Count" || Column7.SelectedItem.ToString() == "Record Count")
                                            { Totals[y, 6] = "1"; }
                                        }
                                        uniquerecords++;
                                        break;
                                    }
                                }
                            }
                        }
                        #region sorting
                        int sortby = findsortby();
                        bool sortbyisSummable = false;
                        Control[] cts = this.Controls.Find("Column" + (sortby + 1).ToString(), true);
                        ComboBox cbs = cts[0] as ComboBox;
                        if (Summable[sortby] || cbs.SelectedItem.ToString() == "Pallet Count" || cbs.SelectedItem.ToString() == "Record Count")
                        { sortbyisSummable = true; }
                        if (AscorDesc.SelectedIndex == 0)
                        {
                            for (int x = 0; x < AllRecords.Length; x++)
                            {
                                for (int y = 0; y < AllRecords.Length; y++)
                                {
                                    string[] holding = new string[8];
                                    if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                    {
                                        if (!sortbyisSummable)
                                        {
                                            if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) < 0)
                                            {
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    holding[f] = Totals[y + 1, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y + 1, f] = Totals[y, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y, f] = holding[f];
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Int32.Parse(Totals[y, sortby]) < Int32.Parse(Totals[y + 1, sortby]))
                                            {
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    holding[f] = Totals[y + 1, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y + 1, f] = Totals[y, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y, f] = holding[f];
                                                }
                                            }
                                        }
                                    }
                                    else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                    { break; }
                                }
                            }
                        }
                        else
                        {
                            for (int x = 0; x < AllRecords.Length; x++)
                            {
                                for (int y = 0; y < AllRecords.Length; y++)
                                {
                                    string[] holding = new string[8];
                                    if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                    {
                                        if (!sortbyisSummable)
                                        {
                                            if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) > 0)
                                            {
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    holding[f] = Totals[y + 1, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y + 1, f] = Totals[y, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y, f] = holding[f];
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Int32.Parse(Totals[y, sortby]) > Int32.Parse(Totals[y + 1, sortby]))
                                            {
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    holding[f] = Totals[y + 1, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y + 1, f] = Totals[y, f];
                                                }
                                                for (int f = 0; f < holding.Length; f++)
                                                {
                                                    Totals[y, f] = holding[f];
                                                }
                                            }
                                        }
                                    }
                                    else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                    { break; }
                                }
                            }
                        }
                        #endregion
                        int pagecount = 1;
                        bool done = false;
                        int rows = 0;
                        for (int x = 1; x < 8; x++)
                        {
                            Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (cb.SelectedIndex != -1)
                            {
                                if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                                { columncount++; }
                            }
                        }
                        do
                        {
                            string records = "";

                            for (int x = (pagecount - 1) * rowsperpage; x <= uniquerecords && x < pagecount * rowsperpage; x++)
                            {
                                if (Totals[x, 0] == null) { Totals[x, 0] = ""; }
                                if (Totals[x, 1] == null) { Totals[x, 1] = ""; }
                                if (Totals[x, 2] == null) { Totals[x, 2] = ""; }
                                if (Totals[x, 3] == null) { Totals[x, 3] = ""; }
                                if (Totals[x, 4] == null) { Totals[x, 4] = ""; }
                                if (Totals[x, 5] == null) { Totals[x, 5] = ""; }
                                if (Totals[x, 6] == null) { Totals[x, 6] = ""; }

                                if (Totals[x, 0] != "")
                                { records = records + Totals[x, 0] + CSVSeparator.Text; }
                                if (Totals[x, 1] != "")
                                { records = records + Totals[x, 1] + CSVSeparator.Text; }
                                if (Totals[x, 2] != "")
                                { records = records + Totals[x, 2] + CSVSeparator.Text; }
                                if (Totals[x, 3] != "")
                                { records = records + Totals[x, 3] + CSVSeparator.Text; }
                                if (Totals[x, 4] != "")
                                { records = records + Totals[x, 4] + CSVSeparator.Text; }
                                if (Totals[x, 5] != "")
                                { records = records + Totals[x, 5] + CSVSeparator.Text; }
                                if (Totals[x, 6] != "")
                                { records = records + Totals[x, 6] + CSVSeparator.Text; }
                                records = records + "\r\n";
                                rows++;
                            }
                            if (rows >= uniquerecords)
                            { done = true; }
                            using (StreamWriter w = File.AppendText(file))
                            {
                                w.WriteLine(records);
                            };
                            pagecount++;
                        } while (!done);
                    }
                    else { MessageBox.Show("No data found in selected date range for selected report."); }
                }
                else { MessageBox.Show("Missing configs"); }
            }
        }
        DateTime fixstarttime(DateTime start, int hour)
        {
            if (start.Hour < hour)
            {
                while (start.Hour < hour)
                { start = start.AddHours(1); }
            }
            else
            {
                while (start.Hour > hour)
                { start = start.AddHours(-1); }
            }
            if (start.Minute > 0)
            {
                while (start.Minute > 0)
                {
                    start = start.AddMinutes(-1);
                }
            }
            else
            {
                while (start.Minute < 0)
                {
                    start = start.AddMinutes(1);
                }
            }
            return start;
        }
        DateTime fixendtime(DateTime end, int hour)
        {
            end = end.AddDays(1);
            if (end.Hour < hour - 1)
            {
                while (end.Hour < hour - 1)
                { end = end.AddHours(1); }
            }
            else
            {
                while (end.Hour > hour - 1)
                { end = end.AddHours(-1); }
            }
            if (end.Minute > 59)
            {
                while (end.Minute > 59)
                {
                    end = end.AddMinutes(-1);
                }
            }
            else
            {
                while (end.Minute < 59)
                {
                    end = end.AddMinutes(1);
                }
            }
            return end;
        }
        int findsortby()
        {
            for (int x = 1; x < 8; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedIndex != -1)
                {
                    if (cb.SelectedItem.ToString() == OrderBy.SelectedItem.ToString())
                    { return x - 1; }
                }
            }
            return 0;
        }
        bool CheckPreviewAvailble()
        {
            if (!CSVInsteadCheck.Checked)
            {
                if (PortraitorLandscape.SelectedIndex != -1 && FPKPKGGEN.SelectedIndex != -1 && ReportTitle.Text != "" && Column1.SelectedIndex != -1 && OrderBy.SelectedIndex != -1 && AscorDesc.SelectedIndex != -1 && PrinterSelect.SelectedIndex != -1 && LocationSelection.SelectedIndex != -1)
                {
                    return true;
                }
            }
            else
            {
                if (PortraitorLandscape.SelectedIndex != -1 && FPKPKGGEN.SelectedIndex != -1 && Column1.SelectedIndex != -1 && OrderBy.SelectedIndex != -1 && AscorDesc.SelectedIndex != -1 && LocationSelection.SelectedIndex != -1)
                {
                    return true;
                }
            }
            return false;
        }
        bool CheckColumnsAvailble()
        {
            if(PortraitorLandscape.SelectedIndex != -1 && FPKPKGGEN.SelectedIndex != -1 && LocationSelection.SelectedIndex != -1)
            { return true; }
            return false;
        }
        void AddColumns()
        {
            #region Variables for fixing selected indexes
            string rcol1 = "";
            string rcol2 = "";
            string rcol3 = "";
            string rcol4 = "";
            string rcol5 = "";
            string rcol6 = "";
            string rcol7 = "";
            if (Column1.SelectedIndex != -1) { rcol1 = Column1.SelectedItem.ToString(); }
            if (Column2.SelectedIndex != -1) { rcol2 = Column2.SelectedItem.ToString(); }
            if (Column3.SelectedIndex != -1) { rcol3 = Column3.SelectedItem.ToString(); }
            if (Column4.SelectedIndex != -1) { rcol4 = Column4.SelectedItem.ToString(); }
            if (Column5.SelectedIndex != -1) { rcol5 = Column5.SelectedItem.ToString(); }
            if (Column6.SelectedIndex != -1) { rcol6 = Column6.SelectedItem.ToString(); }
            if (Column7.SelectedIndex != -1) { rcol7 = Column7.SelectedItem.ToString(); }
            string tcol1 = "";
            string tcol2 = "";
            string tcol3 = "";
            string tcol4 = "";
            string tcol5 = "";
            string tcol6 = "";
            if (TColumn1.SelectedIndex != -1) { tcol1 = TColumn1.SelectedItem.ToString(); }
            if (TColumn2.SelectedIndex != -1) { tcol2 = TColumn2.SelectedItem.ToString(); }
            if (TColumn3.SelectedIndex != -1) { tcol3 = TColumn3.SelectedItem.ToString(); }
            if (TColumn4.SelectedIndex != -1) { tcol4 = TColumn4.SelectedItem.ToString(); }
            if (TColumn5.SelectedIndex != -1) { tcol5 = TColumn5.SelectedItem.ToString(); }
            if (TColumn6.SelectedIndex != -1) { tcol6 = TColumn6.SelectedItem.ToString(); }
            #endregion
            string[] PKGtotalable = new string[3] { "PALLET_SIZE", "Pallet Count", "PALLET_WEIGHT" };
            string[] FPKtotalable = new string[8] { "Metal_Present", "Tare_Weight", "Net_Weight", "Check_Weight", "Clips_Present", "Foil_Check", "Filling_To_Long", "Pallet Count" };
            string[] CPMtotalable = new string[2] { "CPM", "Record Count" };
            string[] OPCtotalable = new string[6] { "Poly_UseL1", "Poly_UseL2", "Job_Count", "Check_Weigher_Count", "Evo_Print_Count" , "Record Count"};
            string path11 = @"GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            int dbsetting = 0, tbsetting = 0;
            switch (FPKPKGGEN.SelectedIndex)
            {
                case 0:
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 5;
                            tbsetting = 6;
                            break;
                        case 1:
                            dbsetting = 13;
                            tbsetting = 14;
                            break;
                    }
                    if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
                    {
                        string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                        string cmdString = "SELECT name FROM sys.columns WHERE object_id = OBJECT_ID('" + settings[tbsetting] + "')";
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                conn.Open();
                                using (var reader = comm.ExecuteReader())
                                {
                                    var list = new List<SQLColumns>();
                                    while (reader.Read())
                                    {
                                        list.Add(new SQLColumns { ColumnName = reader.GetString(0) });
                                    }
                                    SQLColumns[] columns = list.ToArray();
                                    string[] scolumns = new string[columns.Length + 1];
                                    for (int x = 0; x < columns.Length; x++)
                                    {
                                        scolumns[x] = columns[x].ColumnName;
                                    }
                                    scolumns[scolumns.Length - 1] = "Pallet Count";
                                    Column1.Items.Clear();
                                    Column1.Items.AddRange(scolumns);
                                    Column2.Items.Clear();
                                    Column2.Items.AddRange(scolumns);
                                    Column3.Items.Clear();
                                    Column3.Items.AddRange(scolumns);
                                    Column4.Items.Clear();
                                    Column4.Items.AddRange(scolumns);
                                    Column5.Items.Clear();
                                    Column5.Items.AddRange(scolumns);
                                    Column6.Items.Clear();
                                    Column6.Items.AddRange(scolumns);
                                    Column7.Items.Clear();
                                    Column7.Items.AddRange(scolumns);

                                    TColumn1.Items.Clear();
                                    TColumn1.Items.AddRange(FPKtotalable);
                                    TColumn2.Items.Clear();
                                    TColumn2.Items.AddRange(FPKtotalable);
                                    TColumn3.Items.Clear();
                                    TColumn3.Items.AddRange(FPKtotalable);
                                    TColumn4.Items.Clear();
                                    TColumn4.Items.AddRange(FPKtotalable);
                                    TColumn5.Items.Clear();
                                    TColumn5.Items.AddRange(FPKtotalable);
                                    TColumn6.Items.Clear();
                                    TColumn6.Items.AddRange(FPKtotalable);
                                }
                            }
                        }
                    }
                    else { MessageBox.Show("Missing configs"); }
                    break;
                case 1:
                    switch(LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 3;
                            tbsetting = 4;
                            break;
                        case 1:
                            dbsetting = 11;
                            tbsetting = 12;
                            break;
                    }
                    if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
                    {
                        string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                        string cmdString = "SELECT name FROM sys.columns WHERE object_id = OBJECT_ID('" + settings[tbsetting] + "')"; 
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                conn.Open();
                                using (var reader = comm.ExecuteReader())
                                {
                                    var list = new List<SQLColumns>();
                                    while(reader.Read())
                                    {
                                        list.Add(new SQLColumns { ColumnName = reader.GetString(0) });
                                    }
                                    SQLColumns[] columns = list.ToArray();
                                    string[] scolumns = new string[columns.Length + 1];
                                    for(int x = 0; x < columns.Length; x++)
                                    {
                                        scolumns[x] = columns[x].ColumnName;
                                    }
                                    scolumns[scolumns.Length - 1] = "Pallet Count";
                                    Column1.Items.Clear();
                                    Column1.Items.AddRange(scolumns);
                                    Column2.Items.Clear();
                                    Column2.Items.AddRange(scolumns);
                                    Column3.Items.Clear();
                                    Column3.Items.AddRange(scolumns);
                                    Column4.Items.Clear();
                                    Column4.Items.AddRange(scolumns);
                                    Column5.Items.Clear();
                                    Column5.Items.AddRange(scolumns);
                                    Column6.Items.Clear();
                                    Column6.Items.AddRange(scolumns);
                                    Column7.Items.Clear();
                                    Column7.Items.AddRange(scolumns);

                                    TColumn1.Items.Clear();
                                    TColumn1.Items.AddRange(PKGtotalable);
                                    TColumn2.Items.Clear();
                                    TColumn2.Items.AddRange(PKGtotalable);
                                    TColumn3.Items.Clear();
                                    TColumn3.Items.AddRange(PKGtotalable);
                                    TColumn4.Items.Clear();
                                    TColumn4.Items.AddRange(PKGtotalable);
                                    TColumn5.Items.Clear();
                                    TColumn5.Items.AddRange(PKGtotalable);
                                    TColumn6.Items.Clear();
                                    TColumn6.Items.AddRange(PKGtotalable);
                                }
                            }
                        }
                    }
                    else { MessageBox.Show("Missing configs"); }
                    break;
                case 2:
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 7;
                            tbsetting = 8;
                            break;
                        case 1:
                            dbsetting = 15;
                            tbsetting = 16;
                            break;
                    }
                    if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
                    {
                        string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                        string cmdString = "SELECT name FROM sys.columns WHERE object_id = OBJECT_ID('" + settings[tbsetting] + "')";
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                conn.Open();
                                using (var reader = comm.ExecuteReader())
                                {
                                    var list = new List<SQLColumns>();
                                    while (reader.Read())
                                    {
                                        list.Add(new SQLColumns { ColumnName = reader.GetString(0) });
                                    }
                                    SQLColumns[] columns = list.ToArray();
                                    string[] scolumns = new string[columns.Length + 1];
                                    for (int x = 0; x < columns.Length; x++)
                                    {
                                        scolumns[x] = columns[x].ColumnName;
                                    }
                                    scolumns[scolumns.Length - 1] = "Record Count";
                                    Column1.Items.Clear();
                                    Column1.Items.AddRange(scolumns);
                                    Column2.Items.Clear();
                                    Column2.Items.AddRange(scolumns);
                                    Column3.Items.Clear();
                                    Column3.Items.AddRange(scolumns);
                                    Column4.Items.Clear();
                                    Column4.Items.AddRange(scolumns);
                                    Column5.Items.Clear();
                                    Column5.Items.AddRange(scolumns);
                                    Column6.Items.Clear();
                                    Column6.Items.AddRange(scolumns);
                                    Column7.Items.Clear();
                                    Column7.Items.AddRange(scolumns);

                                    TColumn1.Items.Clear();
                                    TColumn1.Items.AddRange(CPMtotalable);
                                    TColumn2.Items.Clear();
                                    TColumn2.Items.AddRange(CPMtotalable);
                                    TColumn3.Items.Clear();
                                    TColumn3.Items.AddRange(CPMtotalable);
                                    TColumn4.Items.Clear();
                                    TColumn4.Items.AddRange(CPMtotalable);
                                    TColumn5.Items.Clear();
                                    TColumn5.Items.AddRange(CPMtotalable);
                                    TColumn6.Items.Clear();
                                    TColumn6.Items.AddRange(CPMtotalable);
                                }
                            }
                        }
                    }
                    else { MessageBox.Show("Missing configs"); }
                    break;
                case 3:
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 9;
                            tbsetting = 10;
                            break;
                        case 1:
                            dbsetting = 17;
                            tbsetting = 18;
                            break;
                    }
                    if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
                    {
                        string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                        string cmdString = "SELECT name FROM sys.columns WHERE object_id = OBJECT_ID('" + settings[tbsetting] + "')";
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                conn.Open();
                                using (var reader = comm.ExecuteReader())
                                {
                                    var list = new List<SQLColumns>();
                                    while (reader.Read())
                                    {
                                        list.Add(new SQLColumns { ColumnName = reader.GetString(0) });
                                    }
                                    SQLColumns[] columns = list.ToArray();
                                    string[] scolumns = new string[columns.Length + 1];
                                    for (int x = 0; x < columns.Length; x++)
                                    {
                                        scolumns[x] = columns[x].ColumnName;
                                    }
                                    scolumns[scolumns.Length - 1] = "Record Count";
                                    Column1.Items.Clear();
                                    Column1.Items.AddRange(scolumns);
                                    Column2.Items.Clear();
                                    Column2.Items.AddRange(scolumns);
                                    Column3.Items.Clear();
                                    Column3.Items.AddRange(scolumns);
                                    Column4.Items.Clear();
                                    Column4.Items.AddRange(scolumns);
                                    Column5.Items.Clear();
                                    Column5.Items.AddRange(scolumns);
                                    Column6.Items.Clear();
                                    Column6.Items.AddRange(scolumns);
                                    Column7.Items.Clear();
                                    Column7.Items.AddRange(scolumns);

                                    TColumn1.Items.Clear();
                                    TColumn1.Items.AddRange(OPCtotalable);
                                    TColumn2.Items.Clear();
                                    TColumn2.Items.AddRange(OPCtotalable);
                                    TColumn3.Items.Clear();
                                    TColumn3.Items.AddRange(OPCtotalable);
                                    TColumn4.Items.Clear();
                                    TColumn4.Items.AddRange(OPCtotalable);
                                    TColumn5.Items.Clear();
                                    TColumn5.Items.AddRange(OPCtotalable);
                                    TColumn6.Items.Clear();
                                    TColumn6.Items.AddRange(OPCtotalable);
                                }
                            }
                        }
                    }
                    else { MessageBox.Show("Missing configs"); }
                    break;
            }
            bool[] rfound = new bool[7] { false, false, false, false, false, false, false };
            bool[] tfound = new bool[6] { false, false, false, false, false, false };
            //Fix selected indexes after a change is made
            for (int x = 0; x < Column1.Items.Count; x++)
            {
                if (rcol1 == Column1.Items[x].ToString() && !rfound[0])
                { Column1.SelectedIndex = x; rfound[0] = true; }
                if (rcol2 == Column2.Items[x].ToString() && !rfound[1])
                { Column2.SelectedIndex = x; rfound[1] = true; }
                if (rcol3 == Column3.Items[x].ToString() && !rfound[2])
                { Column3.SelectedIndex = x; rfound[2] = true; }
                if (rcol4 == Column4.Items[x].ToString() && !rfound[3])
                { Column4.SelectedIndex = x; rfound[3] = true; }
                if (rcol5 == Column5.Items[x].ToString() && !rfound[4])
                { Column5.SelectedIndex = x; rfound[4] = true; }
                if (rcol6 == Column6.Items[x].ToString() && !rfound[5])
                { Column6.SelectedIndex = x; rfound[5] = true; }
                if (rcol7 == Column7.Items[x].ToString() && !rfound[6])
                { Column7.SelectedIndex = x; rfound[6] = true; }
            }
            for (int x = 0; x < TColumn1.Items.Count; x++)
            {
                if (tcol1 == TColumn1.Items[x].ToString() && !tfound[0])
                { TColumn1.SelectedIndex = x; tfound[0] = true; }
                if (tcol2 == TColumn2.Items[x].ToString() && !tfound[1])
                { TColumn2.SelectedIndex = x; tfound[1] = true; }
                if (tcol3 == TColumn3.Items[x].ToString() && !tfound[2])
                { TColumn3.SelectedIndex = x; tfound[2] = true; }
                if (tcol4 == TColumn4.Items[x].ToString() && !tfound[3])
                { TColumn4.SelectedIndex = x; tfound[3] = true; }
                if (tcol5 == TColumn5.Items[x].ToString() && !tfound[4])
                { TColumn5.SelectedIndex = x; tfound[4] = true; }
                if (tcol6 == TColumn6.Items[x].ToString() && !tfound[5])
                { TColumn6.SelectedIndex = x; tfound[5] = true; }
            }
            if(!rfound[0])
            {
                Column1.SelectedIndex = -1;
                Column1.Text = "";
            }
            if (!rfound[1])
            {
                Column2.SelectedIndex = -1;
                Column2.Text = "";
            }
            if (!rfound[2])
            {
                Column3.SelectedIndex = -1;
                Column3.Text = "";
            }
            if (!rfound[3])
            {
                Column4.SelectedIndex = -1;
                Column4.Text = "";
            }
            if (!rfound[4])
            {
                Column5.SelectedIndex = -1;
                Column5.Text = "";
            }
            if (!rfound[5])
            {
                Column6.SelectedIndex = -1;
                Column6.Text = "";
            }
            if (!rfound[6])
            {
                Column7.SelectedIndex = -1;
                Column7.Text = "";
            }
            if (!tfound[0])
            {
                TColumn1.SelectedIndex = -1;
                TColumn1.Text = "";
            }
            if (!tfound[1])
            {
                TColumn2.SelectedIndex = -1;
                TColumn2.Text = "";
            }
            if (!tfound[2])
            {
                TColumn3.SelectedIndex = -1;
                TColumn3.Text = "";
            }
            if (!tfound[3])
            {
                TColumn4.SelectedIndex = -1;
                TColumn4.Text = "";
            }
            if (!tfound[4])
            {
                TColumn5.SelectedIndex = -1;
                TColumn5.Text = "";
            }
            if (!tfound[5])
            {
                TColumn6.SelectedIndex = -1;
                TColumn6.Text = "";
            }

        }
        public class SQLData
        {
            public string Col1 { get; set; }
            public string Col2 { get; set; }
            public string Col3 { get; set; }
            public string Col4 { get; set; }
            public string Col5 { get; set; }
            public string Col6 { get; set; }
            public string Col7 { get; set; }
            public string TCol1 { get; set; }
            public string TCol2 { get; set; }
            public string TCol3 { get; set; }
            public string TCol4 { get; set; }
            public string TCol5 { get; set; }
            public string TCol6 { get; set; }
        }
        public class SQLColumns
        {
            public string ColumnName { get; set; }
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
        #endregion
        #region Triggers and Buttons
        private void ConfigureButton_Click(object sender, EventArgs e)
        {
            GeneralSettings gs = new GeneralSettings();
            var result = gs.ShowDialog();
            if (result == DialogResult.OK)
            {
            }
        }
        private void TopLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            TopLevelSelectedChanged();
        }

        private void PortraitBox_MouseHover(object sender, EventArgs e)
        {

        }

        private void ColumnChange(object sender, EventArgs e)
        {
            string sumby = "", order = "";
            if (SumColumnsBy.SelectedIndex != -1)
            {
                sumby = SumColumnsBy.SelectedItem.ToString();
            }
            if (OrderBy.SelectedIndex != -1)
            {
                order = OrderBy.SelectedItem.ToString();
            }

            SumColumnsBy.Items.Clear();
            OrderBy.Items.Clear();

            try { if (Column1.SelectedIndex != -1) { if (Column1.SelectedItem.ToString() != "Pallet Count" && Column1.SelectedItem.ToString() != "Record Count") SumColumnsBy.Items.Add(Column1.SelectedItem.ToString()); } } catch { }
            try { if (Column2.SelectedIndex != -1) { if (Column2.SelectedItem.ToString() != "Pallet Count" && Column2.SelectedItem.ToString() != "Record Count") SumColumnsBy.Items.Add(Column2.SelectedItem.ToString()); } } catch { }
            try { if (Column3.SelectedIndex != -1) { if (Column3.SelectedItem.ToString() != "Pallet Count" && Column3.SelectedItem.ToString() != "Record Count") SumColumnsBy.Items.Add(Column3.SelectedItem.ToString()); } } catch { }
            try { if (Column4.SelectedIndex != -1) { if (Column4.SelectedItem.ToString() != "Pallet Count" && Column4.SelectedItem.ToString() != "Record Count") SumColumnsBy.Items.Add(Column4.SelectedItem.ToString()); } } catch { }
            try { if (Column5.SelectedIndex != -1) { if (Column5.SelectedItem.ToString() != "Pallet Count" && Column5.SelectedItem.ToString() != "Record Count") SumColumnsBy.Items.Add(Column5.SelectedItem.ToString()); } } catch { }
            try { if (Column6.SelectedIndex != -1) { if (Column6.SelectedItem.ToString() != "Pallet Count" && Column6.SelectedItem.ToString() != "Record Count") SumColumnsBy.Items.Add(Column6.SelectedItem.ToString()); } } catch { }
            try { if (Column7.SelectedIndex != -1) { if (Column7.SelectedItem.ToString() != "Pallet Count" && Column7.SelectedItem.ToString() != "Record Count") SumColumnsBy.Items.Add(Column7.SelectedItem.ToString()); } } catch { }

            try { if (Column1.SelectedIndex != -1) OrderBy.Items.Add(Column1.SelectedItem.ToString()); } catch { }
            try { if (Column2.SelectedIndex != -1) OrderBy.Items.Add(Column2.SelectedItem.ToString()); } catch { }
            try { if (Column3.SelectedIndex != -1) OrderBy.Items.Add(Column3.SelectedItem.ToString()); } catch { }
            try { if (Column4.SelectedIndex != -1) OrderBy.Items.Add(Column4.SelectedItem.ToString()); } catch { }
            try { if (Column5.SelectedIndex != -1) OrderBy.Items.Add(Column5.SelectedItem.ToString()); } catch { }
            try { if (Column6.SelectedIndex != -1) OrderBy.Items.Add(Column6.SelectedItem.ToString()); } catch { }
            try { if (Column7.SelectedIndex != -1) OrderBy.Items.Add(Column7.SelectedItem.ToString()); } catch { }
            SumColumnsBy.Visible = true;

            for (int x = 0; x < SumColumnsBy.Items.Count; x++)
            {
                if (sumby == SumColumnsBy.Items[x].ToString())
                {
                    SumColumnsBy.SelectedIndex = x;
                    break;
                }
                else { SumColumnsBy.SelectedIndex = -1; SumColumnsBy.Text = ""; }
            }

            for (int x = 0; x < OrderBy.Items.Count; x++)
            {
                if (order == OrderBy.Items[x].ToString())
                {
                    OrderBy.SelectedIndex = x;
                    break;
                }
                else { OrderBy.SelectedIndex = -1; OrderBy.Text = ""; }
            }

        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            if (!CSVInsteadCheck.Checked)
            {
                switch (PortraitorLandscape.SelectedIndex)
                {
                    case 0:
                        if (SumColumnsBy.SelectedIndex != -1)
                        { PortraitSumedPrint(); }
                        else
                        {
                            PortraitPrint();
                        }
                        break;
                    case 1:
                        if (SumColumnsBy.SelectedIndex != -1)
                        { LandscapeSumedPrint(); }
                        else
                        {
                            LandscapePrint();
                        }
                        break;
                }
            }
            else
            {
                if (SumColumnsBy.SelectedIndex != -1)
                { ExportSummedCSV(); }
                else
                {
                    ExportCSV();
                }
            }
        }
        private void TColumn_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PrinterSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void PossilbeReportChangeIndexChanged(object sender, EventArgs e)
        {

        }

        private void NoPalletSize1_CheckedChanged(object sender, EventArgs e)
        {
            if (NoPalletSize1.Checked == true)
            {
                OnlyPalletSize1.Enabled = false;
            }
            else
            {
                OnlyPalletSize1.Enabled = true;
            }
        }

        private void OnlyPalletSize1_CheckedChanged(object sender, EventArgs e)
        {
            if (OnlyPalletSize1.Checked == true)
            {
                NoPalletSize1.Enabled = false;
            }
            else
            {
                NoPalletSize1.Enabled = true;
            }
        }

        private void FPKPKGGEN_SelectedIndexChanged(object sender, EventArgs e)
        {
            TopLevelSelectedChanged();
            if (FPKPKGGEN.SelectedIndex == 1)
            {
                OnlyPalletSize1.Visible = true;
                NoPalletSize1.Visible = true;
            }
            else
            {
                OnlyPalletSize1.Visible = false;
                NoPalletSize1.Visible = false;
                OnlyPalletSize1.Checked = false;
                NoPalletSize1.Checked = false;
            }
             
        }

        private void PreviewButton_Click(object sender, EventArgs e)
        {
            if (CheckPreviewAvailble())
            {
                GeneratePrintPreview();
                PrintButton.Enabled = true;
            }
        }
        private void ClearButton_Click(object sender, EventArgs e)
        {
            PortraitorLandscape.SelectedIndex = -1;
            PortraitorLandscape.Text = "";
            FPKPKGGEN.SelectedIndex = -1;
            FPKPKGGEN.Text = "";
            LocationSelection.SelectedIndex = -1;
            LocationSelection.Text = "";
            PrinterSelect.SelectedIndex = -1;
            PrinterSelect.Text = "";
            SumColumnsBy.SelectedIndex = -1;
            SumColumnsBy.Text = "";
            Column1.SelectedIndex = -1;
            Column1.Text = "";
            Column2.SelectedIndex = -1;
            Column2.Text = "";
            Column3.SelectedIndex = -1;
            Column3.Text = "";
            Column4.SelectedIndex = -1;
            Column4.Text = "";
            Column5.SelectedIndex = -1;
            Column5.Text = "";
            Column6.SelectedIndex = -1;
            Column6.Text = "";
            Column7.SelectedIndex = -1;
            Column7.Text = "";
            TColumn1.SelectedIndex = -1;
            TColumn1.Text = "";
            TColumn2.SelectedIndex = -1;
            TColumn2.Text = "";
            TColumn3.SelectedIndex = -1;
            TColumn3.Text = "";
            TColumn4.SelectedIndex = -1;
            TColumn4.Text = "";
            TColumn5.SelectedIndex = -1;
            TColumn5.Text = "";
            TColumn6.SelectedIndex = -1;
            TColumn6.Text = "";
            OrderBy.SelectedIndex = -1;
            OrderBy.Text = "";
            AscorDesc.SelectedIndex = -1;
            AscorDesc.Text = "";
            OnlyPalletSize1.Checked = false;
            NoPalletSize1.Checked = false;
            ReportTitle.Text = "";
            DateRangeSelector.SelectionStart = DateTime.Now;
            DateRangeSelector.SelectionEnd = DateTime.Now;
            PortraitBox.Visible = false;
            PortraitBox.Image = null;
            LandscapeBox.Visible = false;
            LandscapeBox.Image = null;
            CSVBox.Text = "";
            CSVBox.Visible = false;
            CSVInsteadCheck.Checked = false;
        }

        private void CSVInsteadCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (CSVInsteadCheck.Checked)
            {
                PrintButton.Text = "Export";
                SeparatorLabel.Visible = true;
                CSVSeparator.Visible = true;
                totalcolumnslabel.Visible = false;
                TColumn1.Visible = false;
                TColumn2.Visible = false;
                TColumn3.Visible = false;
                TColumn4.Visible = false;
                TColumn5.Visible = false;
                TColumn6.Visible = false;
                PrinterSelect.Visible = false;
                ReportTitleLabel.Visible = false;
                ReportTitle.Visible = false;
            }
            else
            {
                PrintButton.Text = "Print";
                SeparatorLabel.Visible = false;
                CSVSeparator.Visible = false;
                if (CheckColumnsAvailble())
                {
                    totalcolumnslabel.Visible = true;
                    TColumn1.Visible = true;
                    TColumn2.Visible = true;
                    TColumn3.Visible = true;
                    TColumn4.Visible = true;
                    TColumn5.Visible = true;
                    TColumn6.Visible = true;
                }
                PrinterSelect.Visible = true;
                ReportTitle.Visible = true;
                ReportTitleLabel.Visible = true;
            }
        }

        private void ClearColumnSumBy_Click(object sender, EventArgs e)
        {
            SumColumnsBy.SelectedIndex = -1;
            SumColumnsBy.Text = "";
        }
        #endregion
        #region print Preview
        void LandscapePP()
        {
            string path11 = @"GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            int dbsetting = 0, tbsetting = 0, starttimesetting = 0;
            string timedatename = "";
            int rowsperpage = 42;
            DateTime start = DateRangeSelector.SelectionStart;
            DateTime end = DateRangeSelector.SelectionEnd;
            string[] sumcolumns;
            switch (FPKPKGGEN.SelectedIndex)
            {
                case 0:
                    sumcolumns = new string[7] { "Metal_Present", "Tare_Weight", "Net_Weight", "Check_Weight", "Clips_Present", "Foil_Check", "Filling_To_Long" };
                    timedatename = "Time_Date";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 5;
                            tbsetting = 6;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 13;
                            tbsetting = 14;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 1:
                    sumcolumns = new string[2] { "PALLET_SIZE", "PALLET_WEIGHT" };
                    timedatename = "Date_Time";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 3;
                            tbsetting = 4;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 11;
                            tbsetting = 12;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 2:
                    timedatename = "Date_Time";
                    sumcolumns = new string[1] { "CPM" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 7;
                            tbsetting = 8;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 15;
                            tbsetting = 16;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 3:
                    timedatename = "Time_Date";
                    sumcolumns = new string[5] { "Poly_UseL1", "Poly_UseL2", "Job_Count", "Check_Weigher_Count", "Evo_Print_Count" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 9;
                            tbsetting = 10;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 17;
                            tbsetting = 18;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                default:
                    sumcolumns = new string[1] { "" };
                    dbsetting = 0;
                    tbsetting = 0;
                    break;
            }
            DateTime Start = fixstarttime(start, Int32.Parse(settings[starttimesetting]));
            DateTime End = fixendtime(end, Int32.Parse(settings[starttimesetting]));
            string[] columns = new string[7];
            int ColumnCount = 0, Tcolumncount = 0;
            bool selfsort = true;
            SQLData[] AllRecords = null;
            bool[] Summable = new bool[7];
            int skipped = 0;
            for (int x = 1; x < 8; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedIndex != -1 && cb.SelectedItem.ToString() != "Pallet Count" && cb.SelectedItem.ToString() != "Record Count")
                {
                    columns[x - 1 - skipped] = cb.SelectedItem.ToString();
                    ColumnCount++;
                    for (int y = 0; y < sumcolumns.Length; y++)
                    {
                        if (cb.SelectedItem.ToString() == sumcolumns[y])
                        {
                            Summable[x] = true;
                        }
                    }
                }
                else { skipped++; }
            }
            if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
            {
                string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString = "SELECT ";
                for (int x = 0; x <= ColumnCount; x++)
                {
                    cmdString = cmdString + columns[x] + ", ";
                }


                for (int g = 0; g < 10; g++)
                {
                    if (cmdString.Substring(cmdString.Length - 1, 1) == " " || cmdString.Substring(cmdString.Length - 1, 1) == ",")
                    {
                        cmdString = cmdString.Remove(cmdString.Length - 1, 1);
                    }
                }
                cmdString = cmdString + ", ";
                if (TColumn1.SelectedIndex != -1 && TColumn1.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn1.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn2.SelectedIndex != -1 && TColumn2.SelectedItem.ToString() != "Pallet Count" && TColumn2.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn2.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn3.SelectedIndex != -1 && TColumn3.SelectedItem.ToString() != "Pallet Count" && TColumn3.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn3.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn4.SelectedIndex != -1 && TColumn4.SelectedItem.ToString() != "Pallet Count" && TColumn4.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn4.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn5.SelectedIndex != -1 && TColumn5.SelectedItem.ToString() != "Pallet Count" && TColumn5.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn5.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn6.SelectedIndex != -1 && TColumn6.SelectedItem.ToString() != "Pallet Count" && TColumn6.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn6.SelectedItem.ToString(); Tcolumncount++; }

                for (int g = 0; g < 10; g++)
                {
                    if (cmdString.Substring(cmdString.Length - 1, 1) == " " || cmdString.Substring(cmdString.Length - 1, 1) == ",")
                    {
                        cmdString = cmdString.Remove(cmdString.Length - 1, 1);
                    }
                }

                cmdString = cmdString + " FROM " + settings[tbsetting] + " WHERE " + timedatename + " >= @dtStart AND " + timedatename + " <= @dtEnd";

                if (OnlyPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE = '1'";
                }
                else if (NoPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE != '1'";
                }

                if (OrderBy.SelectedIndex != -1 && AscorDesc.SelectedIndex != -1 && !selfsort)
                {
                    cmdString = cmdString + " ORDER BY " + OrderBy.SelectedItem + " " + AscorDesc.SelectedItem + ";";
                }
                else { cmdString = cmdString + ";"; }

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
                            var list = new List<SQLData>();
                            while (reader.Read())
                            {
                                list.Add(new SQLData());
                                int tcolumnstart = ColumnCount;
                                if (ColumnCount > 0)
                                { try { list[list.Count - 1].Col1 = reader.GetString(0); } catch (Exception t) { }; }
                                if (ColumnCount > 1)
                                { try { list[list.Count - 1].Col2 = reader.GetString(1); } catch (Exception t) { }; }
                                if (ColumnCount > 2)
                                { try { list[list.Count - 1].Col3 = reader.GetString(2); } catch (Exception t) { }; }
                                if (ColumnCount > 3)
                                { try { list[list.Count - 1].Col4 = reader.GetString(3); } catch (Exception t) { }; }
                                if (ColumnCount > 4)
                                { try { list[list.Count - 1].Col5 = reader.GetString(4); } catch (Exception t) { }; }
                                if (ColumnCount > 5)
                                { try { list[list.Count - 1].Col6 = reader.GetString(5); } catch (Exception t) { }; }
                                if (ColumnCount > 6)
                                { try { list[list.Count - 1].Col7 = reader.GetString(6); } catch (Exception t) { }; }


                                if (Tcolumncount > 0)
                                { try { list[list.Count - 1].TCol1 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 1)
                                { try { list[list.Count - 1].TCol2 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 2)
                                { try { list[list.Count - 1].TCol3 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 3)
                                { try { list[list.Count - 1].TCol4 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 4)
                                { try { list[list.Count - 1].TCol5 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 5)
                                { try { list[list.Count - 1].TCol6 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                AllRecords = list.ToArray();

                            }
                        }
                    }
                }
                if (AllRecords != null)
                {
                    int RecordCount = 0;
                    string[,] Totals = new string[AllRecords.Length, 8];
                    int[] tcoltotals = new int[6] { 0, 0, 0, 0, 0, 0 };
                    for (int x = 0; x < AllRecords.Length; x++)
                    {
                        Totals[x, 0] = AllRecords[x].Col1;
                        Totals[x, 1] = AllRecords[x].Col2;
                        Totals[x, 2] = AllRecords[x].Col3;
                        Totals[x, 3] = AllRecords[x].Col4;
                        Totals[x, 4] = AllRecords[x].Col5;
                        Totals[x, 5] = AllRecords[x].Col6;
                        Totals[x, 6] = AllRecords[x].Col7;
                        Totals[x, 7] = "1";

                        RecordCount++;

                        int sqlselector = 0;
                        for (int f = 0; f < 6; f++)
                        {
                            Control[] ct = this.Controls.Find("TColumn" + (f + 1).ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (cb.SelectedIndex != -1)
                            {
                                if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                                { tcoltotals[f]++; }
                                else
                                {
                                    if (sqlselector == 0)
                                    { if (AllRecords[x].TCol1 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol1); } }
                                    if (sqlselector == 1)
                                    { if (AllRecords[x].TCol2 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol2); } }
                                    if (sqlselector == 2)
                                    { if (AllRecords[x].TCol3 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol3); } }
                                    if (sqlselector == 3)
                                    { if (AllRecords[x].TCol4 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol4); } }
                                    if (sqlselector == 4)
                                    { if (AllRecords[x].TCol5 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol5); } }
                                    if (sqlselector == 5)
                                    { if (AllRecords[x].TCol6 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol6); } }
                                    sqlselector++;
                                }
                            }
                        }
                    }
                    #region sorting
                    int sortby = findsortby();
                    bool sortbyisSummable = false;
                    Control[] cts = this.Controls.Find("Column" + (sortby + 1).ToString(), true);
                    ComboBox cbs = cts[0] as ComboBox;
                    if (Summable[sortby] || cbs.SelectedItem.ToString() == "Pallet Count" || cbs.SelectedItem.ToString() == "Record Count")
                    { sortbyisSummable = true; }
                    if (cbs.SelectedItem.ToString() == "Pallet Count" || cbs.SelectedItem.ToString() == "Record Count")
                    { sortby = 7; }
                    if (AscorDesc.SelectedIndex == 0)
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length - 1; y++)
                            {
                                string[] holding = new string[7];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) < 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) < Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                { break; }
                            }
                        }
                    }
                    else
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length - 1; y++)
                            {
                                string[] holding = new string[7];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) > 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) > Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                { break; }
                            }
                        }
                    }
                    #endregion
                    int pagecount = 1;
                    bool done = true;
                    int rows = 0;
                    int charpercolumn = (105 / ColumnCount) - 1;
                    do
                    {
                        CultureInfo culture = CultureInfo.GetCultureInfo("en-US");
                        string dateheader = start.ToString("d", culture) + " - " + end.ToString("d", culture);
                        string Headers = "";
                        string Totalsbar = "Totals:";
                        string records = "";

                        for (int x = 0; x < 7; x++)
                        {
                            Control[] ct = this.Controls.Find("Column" + (x + 1).ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (columns[x] != null)
                            {
                                Headers = Headers + columns[x].PadRight(charpercolumn, ' ');
                            }
                            if (cb.SelectedIndex != -1)
                            {
                                if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                                {
                                    Headers = Headers + cb.SelectedItem.ToString().PadRight(charpercolumn, ' ');
                                }
                            }
                        }

                        for (int x = 1; x <= tcoltotals.Length; x++)
                        {
                            Control[] ct = this.Controls.Find("TColumn" + x.ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (cb.SelectedIndex != -1)
                            {
                                Totalsbar = Totalsbar + (cb.SelectedItem.ToString() + ": " + tcoltotals[x - 1]) + " ";
                            }
                        }

                        for (int x = (pagecount - 1) * rowsperpage; x <= RecordCount && x < pagecount * rowsperpage; x++)
                        {
                            if (Totals[x, 0] == null) { Totals[x, 0] = ""; }
                            if (Totals[x, 1] == null) { Totals[x, 1] = ""; }
                            if (Totals[x, 2] == null) { Totals[x, 2] = ""; }
                            if (Totals[x, 3] == null) { Totals[x, 3] = ""; }
                            if (Totals[x, 4] == null) { Totals[x, 4] = ""; }
                            if (Totals[x, 5] == null) { Totals[x, 5] = ""; }
                            if (Totals[x, 6] == null) { Totals[x, 6] = ""; }
                            if (Totals[x, 0].Length >= charpercolumn)
                            { Totals[x, 0] = Totals[x, 0].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 1].Length >= charpercolumn)
                            { Totals[x, 1] = Totals[x, 1].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 2].Length >= charpercolumn)
                            { Totals[x, 2] = Totals[x, 2].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 3].Length >= charpercolumn)
                            { Totals[x, 3] = Totals[x, 3].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 4].Length >= charpercolumn)
                            { Totals[x, 4] = Totals[x, 4].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 5].Length >= charpercolumn)
                            { Totals[x, 5] = Totals[x, 5].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 6].Length >= charpercolumn)
                            { Totals[x, 6] = Totals[x, 6].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 0] != "")
                            { records = records + Totals[x, 0].PadRight(charpercolumn, ' '); }
                            if (Totals[x, 1] != "")
                            { records = records + Totals[x, 1].PadRight(charpercolumn, ' '); }
                            if (Totals[x, 2] != "")
                            { records = records + Totals[x, 2].PadRight(charpercolumn, ' '); }
                            if (Totals[x, 3] != "")
                            { records = records + Totals[x, 3].PadRight(charpercolumn, ' '); }
                            if (Totals[x, 4] != "")
                            { records = records + Totals[x, 4].PadRight(charpercolumn, ' '); }
                            if (Totals[x, 5] != "")
                            { records = records + Totals[x, 5].PadRight(charpercolumn, ' '); }
                            if (Totals[x, 6] != "")
                            { records = records + Totals[x, 6].PadRight(charpercolumn, ' '); }
                            records = records + "\r\n";
                            rows++;
                        }
                        if (rows >= RecordCount)
                        { done = true; }
                        string labelpath = "ReportLandscape.nlbl";
                        ILabel label = PrintEngineFactory.PrintEngine.OpenLabel(labelpath);
                        label.PrintSettings.PrinterName = PrinterSelect.SelectedItem.ToString();

                        label.Variables["ReportTitle"].SetValue(ReportTitle.Text);
                        label.Variables["Page"].SetValue(pagecount.ToString());
                        label.Variables["Data"].SetValue(records);
                        label.Variables["ColumnHeads"].SetValue(Headers);
                        label.Variables["Totals"].SetValue(Totalsbar);
                        label.Variables["DateRange"].SetValue(dateheader);

                        ILabelPreviewSettings labelPreviewSettings = new LabelPreviewSettings();

                        labelPreviewSettings.PreviewToFile = false;                                    // if true, result will be the file name, if false, result will be a byte array.
                        labelPreviewSettings.ImageFormat = "jpg";                                      // file format of graphics.  Valid formats: JPG, PNG, BMP.
                        labelPreviewSettings.Width = this.LandscapeBox.Width;                            // Width of image to generate
                        labelPreviewSettings.Height = this.LandscapeBox.Height;                          // Height of image to generate
                                                                                                         //labelPreviewSettings.Destination = this.textBoxFileName.Text;                // If PrintToFile is true, this is the name of the file that will be generated.
                        labelPreviewSettings.FormatPreviewSide = FormatPreviewSide.FrontSide;          // Which label side(s) to generate the image for.  

                        // Generate Preview File
                        object imageObj = label.GetLabelPreview(labelPreviewSettings);

                        // Display image in UI
                        if (imageObj is byte[])
                        {
                            // When PrintToFiles = false
                            // Convert byte[] to Bitmap and set as image source for PictureBox control
                            PortraitBox.Visible = false;
                            CSVBox.Visible = false;
                            LandscapeBox.Visible = true;
                            LandscapeBox.Image = this.ByteToImage((byte[])imageObj);
                        }

                        pagecount++;
                    }
                    while (!done);
                }
            }
        }
        void LandscapeSumedPP()
        {
            string path11 = @"GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            int dbsetting = 0, tbsetting = 0, starttimesetting = 0;
            string[] sumcolumns;
            string timedatename = "";
            bool selfsort = false;
            int rowsperpage = 42;
            DateTime start = DateRangeSelector.SelectionStart;
            DateTime end = DateRangeSelector.SelectionEnd;
            SQLData[] AllRecords = null;

            switch (FPKPKGGEN.SelectedIndex)
            {
                case 0:
                    sumcolumns = new string[7] { "Metal_Present", "Tare_Weight", "Net_Weight", "Check_Weight", "Clips_Present", "Foil_Check", "Filling_To_Long" };
                    timedatename = "Time_Date";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 5;
                            tbsetting = 6;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 13;
                            tbsetting = 14;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 1:
                    sumcolumns = new string[2] { "PALLET_SIZE", "PALLET_WEIGHT" };
                    timedatename = "Date_Time";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 3;
                            tbsetting = 4;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 11;
                            tbsetting = 12;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 2:
                    timedatename = "Date_Time";
                    sumcolumns = new string[1] { "CPM" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 7;
                            tbsetting = 8;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 15;
                            tbsetting = 16;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 3:
                    timedatename = "Time_Date";
                    sumcolumns = new string[5] { "Poly_UseL1", "Poly_UseL2", "Job_Count", "Check_Weigher_Count", "Evo_Print_Count" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 9;
                            tbsetting = 10;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 17;
                            tbsetting = 18;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                default:
                    sumcolumns = new string[1] { "" };
                    dbsetting = 0;
                    tbsetting = 0;
                    break;
            }
            DateTime Start = fixstarttime(start, Int32.Parse(settings[starttimesetting]));
            DateTime End = fixendtime(end, Int32.Parse(settings[starttimesetting]));
            for (int x = 0; x < sumcolumns.Length; x++)
            {
                if (SumColumnsBy.SelectedIndex == -1 && OrderBy.SelectedItem.ToString() != sumcolumns[x])
                {
                    selfsort = false;
                }
                else { selfsort = true; break; }
            }
            string[] columns = new string[8];
            int sumbycolumn = 0;
            for (int x = 1; x < 8; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedItem.ToString() == SumColumnsBy.SelectedItem.ToString())
                {
                    columns[0] = cb.SelectedItem.ToString();
                    sumbycolumn = x;
                    break;
                }
            }
            int columncount = 1, Tcolumncount = 0;
            int skipped = 0;
            for (int x = 1; x < 8; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedIndex == -1 || x == sumbycolumn)
                { skipped++; }
                else { columns[x - skipped] = cb.SelectedItem.ToString(); columncount++; }
            }

            bool[] Summable = new bool[7];
            int prskip = 0;
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < sumcolumns.Length; y++)
                {
                    if (columns[x] == sumcolumns[y]) 
                    { Summable[x - prskip] = true; break; }
                    else if (columns[x] == "Pallet Count" || columns[x] == "Record Count")
                    { prskip++; break; }
                    else { Summable[x] = false; }
                }
            }
            if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
            {
                string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString = "SELECT ";
                //Change this creation of cmd string to be in a loop to iterate the number of columns
                //and then patch together the string one column at a time checking if it has a -1 selected index each time  or pallet count
                //this should allow to also check other things like for Pallet count and pivot later in the function to address that issue
                for (int x = 0; x <= columncount; x++)
                {
                    if (columns[x] != "Pallet Count" && columns[x] != "Record Count")
                    { cmdString = cmdString + columns[x] + ", "; }
                    else { columncount--; }
                }

                for (int g = 0; g < 10; g++)
                {
                    if (cmdString.Substring(cmdString.Length - 1, 1) == " " || cmdString.Substring(cmdString.Length - 1, 1) == ",")
                    {
                        cmdString = cmdString.Remove(cmdString.Length - 1, 1);
                    }
                }
                cmdString = cmdString + ", ";
                if (TColumn1.SelectedIndex != -1 && TColumn1.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn1.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn2.SelectedIndex != -1 && TColumn2.SelectedItem.ToString() != "Pallet Count" && TColumn2.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn2.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn3.SelectedIndex != -1 && TColumn3.SelectedItem.ToString() != "Pallet Count" && TColumn3.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn3.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn4.SelectedIndex != -1 && TColumn4.SelectedItem.ToString() != "Pallet Count" && TColumn4.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn4.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn5.SelectedIndex != -1 && TColumn5.SelectedItem.ToString() != "Pallet Count" && TColumn5.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn5.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn6.SelectedIndex != -1 && TColumn6.SelectedItem.ToString() != "Pallet Count" && TColumn6.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn6.SelectedItem.ToString(); Tcolumncount++; }

                for (int g = 0; g < 10; g++)
                {
                    if (cmdString.Substring(cmdString.Length - 1, 1) == " " || cmdString.Substring(cmdString.Length - 1, 1) == ",")
                    {
                        cmdString = cmdString.Remove(cmdString.Length - 1, 1);
                    }
                }

                cmdString = cmdString + " FROM " + settings[tbsetting] + " WHERE " + timedatename + " >= @dtStart AND " + timedatename + " <= @dtEnd";
                if (OnlyPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE = '1'";
                }
                else if (NoPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE != '1'";
                }
                if (OrderBy.SelectedIndex != -1 && AscorDesc.SelectedIndex != -1 && !selfsort)
                {
                    cmdString = cmdString + " ORDER BY " + OrderBy.SelectedItem + " " + AscorDesc.SelectedItem + ";";
                }
                else { cmdString = cmdString + ";"; }

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
                            var list = new List<SQLData>();
                            while (reader.Read())
                            {
                                //list.Add(new SQLData { Col1 = reader.GetString(0), Col2 = reader.GetString(1), Col3 = reader.GetString(2), Col4 = reader.GetString(3), Col5 = reader.GetString(4), Col6 = reader.GetString(5), Col7 = reader.GetString(6), TCol1 = reader.GetString(7), TCol2 = reader.GetString(8), TCol3 = reader.GetString(9), TCol4 = reader.GetString(10), TCol5 = reader.GetString(11), TCol6 = reader.GetString(12) });
                                list.Add(new SQLData());
                                int tcolumnstart = columncount;
                                if (columncount > 0)
                                { try { list[list.Count - 1].Col1 = reader.GetString(0); } catch (Exception t) { }; }
                                if (columncount > 1)
                                { try { list[list.Count - 1].Col2 = reader.GetString(1); } catch (Exception t) { }; }
                                if (columncount > 2)
                                { try { list[list.Count - 1].Col3 = reader.GetString(2); } catch (Exception t) { }; }
                                if (columncount > 3)
                                { try { list[list.Count - 1].Col4 = reader.GetString(3); } catch (Exception t) { }; }
                                if (columncount > 4)
                                { try { list[list.Count - 1].Col5 = reader.GetString(4); } catch (Exception t) { }; }
                                if (columncount > 5)
                                { try { list[list.Count - 1].Col6 = reader.GetString(5); } catch (Exception t) { }; }
                                if (columncount > 6)
                                { try { list[list.Count - 1].Col7 = reader.GetString(6); } catch (Exception t) { }; }


                                if (Tcolumncount > 0)
                                { try { list[list.Count - 1].TCol1 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 1)
                                { try { list[list.Count - 1].TCol2 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 2)
                                { try { list[list.Count - 1].TCol3 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 3)
                                { try { list[list.Count - 1].TCol4 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 4)
                                { try { list[list.Count - 1].TCol5 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 5)
                                { try { list[list.Count - 1].TCol6 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                AllRecords = list.ToArray();
                            }
                        }
                    }
                }

                if (AllRecords != null)
                {
                    int RecordCount = 0;
                    string[,] Totals = new string[AllRecords.Length, 8];
                    int uniquerecords = 0;
                    int[] tcoltotals = new int[6] { 0, 0, 0, 0, 0, 0 };
                    for (int x = 0; x < AllRecords.Length; x++)
                    {
                        RecordCount++;
                        bool found = false;
                        for (int y = 0; y < AllRecords.Length; y++)
                        {
                            if (AllRecords[x].Col1 == Totals[y, 0])
                            {
                                if (Summable[1]) { Totals[y, 1] = (Int32.Parse(Totals[y, 1]) + Int32.Parse(AllRecords[x].Col2)).ToString(); }
                                if (Summable[2]) { Totals[y, 2] = (Int32.Parse(Totals[y, 2]) + Int32.Parse(AllRecords[x].Col3)).ToString(); }
                                if (Summable[3]) { Totals[y, 3] = (Int32.Parse(Totals[y, 3]) + Int32.Parse(AllRecords[x].Col4)).ToString(); }
                                if (Summable[4]) { Totals[y, 4] = (Int32.Parse(Totals[y, 4]) + Int32.Parse(AllRecords[x].Col5)).ToString(); }
                                if (Summable[5]) { Totals[y, 5] = (Int32.Parse(Totals[y, 5]) + Int32.Parse(AllRecords[x].Col6)).ToString(); }
                                if (Summable[6]) { Totals[y, 6] = (Int32.Parse(Totals[y, 6]) + Int32.Parse(AllRecords[x].Col7)).ToString(); }
                                if (Column1.SelectedIndex != -1)
                                {
                                    if (Column1.SelectedItem.ToString() == "Pallet Count" || Column1.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 0] = (Int32.Parse(Totals[y, 0]) + 1).ToString(); }
                                }
                                if (Column2.SelectedIndex != -1)
                                {
                                    if (Column2.SelectedItem.ToString() == "Pallet Count" || Column2.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 1] = (Int32.Parse(Totals[y, 1]) + 1).ToString(); }
                                }
                                if (Column3.SelectedIndex != -1)
                                {
                                    if (Column3.SelectedItem.ToString() == "Pallet Count" || Column3.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 2] = (Int32.Parse(Totals[y, 2]) + 1).ToString(); }
                                }
                                if (Column4.SelectedIndex != -1)
                                {
                                    if (Column4.SelectedItem.ToString() == "Pallet Count" || Column4.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 3] = (Int32.Parse(Totals[y, 3]) + 1).ToString(); }
                                }
                                if (Column5.SelectedIndex != -1)
                                {
                                    if (Column5.SelectedItem.ToString() == "Pallet Count" || Column5.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 4] = (Int32.Parse(Totals[y, 4]) + 1).ToString(); }
                                }
                                if (Column6.SelectedIndex != -1)
                                {
                                    if (Column6.SelectedItem.ToString() == "Pallet Count" || Column6.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 5] = (Int32.Parse(Totals[y, 5]) + 1).ToString(); }
                                }
                                if (Column7.SelectedIndex != -1)
                                {
                                    if (Column7.SelectedItem.ToString() == "Pallet Count" || Column7.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 6] = (Int32.Parse(Totals[y, 6]) + 1).ToString(); }
                                }
                                Totals[y, 7] = (Int32.Parse(Totals[y, 7]) + 1).ToString();
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            for (int y = 0; y < AllRecords.Length; y++)
                            {
                                if (Totals[y, 0] == null || Totals[y, 0] == string.Empty || Totals[y, 0] == "")
                                {
                                    Totals[y, 0] = AllRecords[x].Col1;
                                    Totals[y, 1] = AllRecords[x].Col2;
                                    Totals[y, 2] = AllRecords[x].Col3;
                                    Totals[y, 3] = AllRecords[x].Col4;
                                    Totals[y, 4] = AllRecords[x].Col5;
                                    Totals[y, 5] = AllRecords[x].Col6;
                                    Totals[y, 6] = AllRecords[x].Col7;
                                    Totals[y, 7] = "1";
                                    if (Column1.SelectedIndex != -1)
                                    {
                                        if (Column1.SelectedItem.ToString() == "Pallet Count" || Column1.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 0] = "1"; }
                                    }
                                    if (Column2.SelectedIndex != -1)
                                    {
                                        if (Column2.SelectedItem.ToString() == "Pallet Count" || Column2.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 1] = "1"; }
                                    }
                                    if (Column3.SelectedIndex != -1)
                                    {
                                        if (Column3.SelectedItem.ToString() == "Pallet Count" || Column3.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 2] = "1"; }
                                    }
                                    if (Column4.SelectedIndex != -1)
                                    {
                                        if (Column4.SelectedItem.ToString() == "Pallet Count" || Column4.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 3] = "1"; }
                                    }
                                    if (Column5.SelectedIndex != -1)
                                    {
                                        if (Column5.SelectedItem.ToString() == "Pallet Count" || Column5.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 4] = "1"; }
                                    }
                                    if (Column6.SelectedIndex != -1)
                                    {
                                        if (Column6.SelectedItem.ToString() == "Pallet Count" || Column6.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 5] = "1"; }
                                    }
                                    if (Column7.SelectedIndex != -1)
                                    {
                                        if (Column7.SelectedItem.ToString() == "Pallet Count" || Column7.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 6] = "1"; }
                                    }
                                    uniquerecords++;
                                    break;
                                }
                            }
                        }
                        int sqlselector = 0;
                        for (int f = 0; f < 6; f++)
                        {

                            Control[] ct = this.Controls.Find("TColumn" + (f + 1).ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (cb.SelectedIndex != -1)
                            {
                                if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                                { tcoltotals[f]++; }
                                else
                                {
                                    if (sqlselector == 0)
                                    { if (AllRecords[x].TCol1 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol1); } }
                                    if (sqlselector == 1)
                                    { if (AllRecords[x].TCol2 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol2); } }
                                    if (sqlselector == 2)
                                    { if (AllRecords[x].TCol3 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol3); } }
                                    if (sqlselector == 3)
                                    { if (AllRecords[x].TCol4 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol4); } }
                                    if (sqlselector == 4)
                                    { if (AllRecords[x].TCol5 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol5); } }
                                    if (sqlselector == 5)
                                    { if (AllRecords[x].TCol6 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol6); } }
                                    sqlselector++;
                                }
                            }
                        }
                    }
                    #region sorting
                    int sortby = findsortby();
                    bool sortbyisSummable = false;
                    Control[] cts = this.Controls.Find("Column" + (sortby + 1).ToString(), true);
                    ComboBox cbs = cts[0] as ComboBox;
                    if (Summable[sortby] || cbs.SelectedItem.ToString() == "Pallet Count" || cbs.SelectedItem.ToString() == "Record Count")
                    { sortbyisSummable = true; }
                    if(cbs.SelectedItem.ToString() == "Pallet Count" || cbs.SelectedItem.ToString() == "Record Count")
                    { sortby = 7; }
                    if (AscorDesc.SelectedIndex == 0)
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length; y++)
                            {
                                string[] holding = new string[8];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) < 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) < Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                { break; }
                            }
                        }
                    }
                    else
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length; y++)
                            {
                                string[] holding = new string[8];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) > 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) > Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                { break; }
                            }
                        }
                    }
                    #endregion
                    int pagecount = 1;
                    bool done = true;
                    int rows = 0;
                    for (int x = 1; x < 8; x++)
                    {
                        Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                        ComboBox cb = ct[0] as ComboBox;
                        if (cb.SelectedIndex != -1)
                        {
                            if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                            { columncount++; }
                        }
                    }
                    int charpercolumn = (105 / columncount + prskip) - 1;
                    do
                    {
                        CultureInfo culture = CultureInfo.GetCultureInfo("en-US");
                        string dateheader = start.ToString("d", culture) + " - " + end.ToString("d", culture);
                        string Headers = "";
                        string Totalsbar = "Totals:";
                        string records = "";
                        for (int x = 0; x < 7; x++)
                        {
                            Control[] ct = this.Controls.Find("Column" + (x + 1).ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (columns[x] != null)
                            {
                                Headers = Headers + columns[x].PadRight(charpercolumn, ' ');
                            }
                        }
                        for (int x = 1; x <= tcoltotals.Length; x++)
                        {
                            Control[] ct = this.Controls.Find("TColumn" + x.ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (cb.SelectedIndex != -1)
                            {
                                Totalsbar = Totalsbar + (cb.SelectedItem.ToString() + ": " + tcoltotals[x - 1]) + " ";
                            }
                        }

                        for (int x = (pagecount - 1) * rowsperpage; x <= uniquerecords && x < pagecount * rowsperpage; x++)
                        {
                            if (Totals[x, 0] == null) { Totals[x, 0] = ""; }
                            if (Totals[x, 1] == null) { Totals[x, 1] = ""; }
                            if (Totals[x, 2] == null) { Totals[x, 2] = ""; }
                            if (Totals[x, 3] == null) { Totals[x, 3] = ""; }
                            if (Totals[x, 4] == null) { Totals[x, 4] = ""; }
                            if (Totals[x, 5] == null) { Totals[x, 5] = ""; }
                            if (Totals[x, 6] == null) { Totals[x, 6] = ""; }

                            if (Totals[x, 0].Length >= charpercolumn)
                            { Totals[x, 0] = Totals[x, 0].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 1].Length >= charpercolumn)
                            { Totals[x, 1] = Totals[x, 1].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 2].Length >= charpercolumn)
                            { Totals[x, 2] = Totals[x, 2].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 3].Length >= charpercolumn)
                            { Totals[x, 3] = Totals[x, 3].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 4].Length >= charpercolumn)
                            { Totals[x, 4] = Totals[x, 4].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 5].Length >= charpercolumn)
                            { Totals[x, 5] = Totals[x, 5].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 6].Length >= charpercolumn)
                            { Totals[x, 6] = Totals[x, 6].Substring(0, charpercolumn - 1); }

                            if (Totals[x, 0] != "")
                            { records = records + Totals[x, 0].PadRight(charpercolumn, ' '); }
                            
                            if (Totals[x, 1] != "")
                            { records = records + Totals[x, 1].PadRight(charpercolumn, ' '); }
                           
                            if (Totals[x, 2] != "")
                            { records = records + Totals[x, 2].PadRight(charpercolumn, ' '); }
                           
                            if (Totals[x, 3] != "")
                            { records = records + Totals[x, 3].PadRight(charpercolumn, ' '); }
                           
                            if (Totals[x, 4] != "")
                            { records = records + Totals[x, 4].PadRight(charpercolumn, ' '); }
                            
                            if (Totals[x, 5] != "")
                            { records = records + Totals[x, 5].PadRight(charpercolumn, ' '); }
                            
                            if (Totals[x, 6] != "")
                            { records = records + Totals[x, 6].PadRight(charpercolumn, ' '); }
                            records = records + "\r\n";
                            rows++;
                        }
                        if (rows >= uniquerecords)
                        { done = true; }
                        string labelpath = "ReportLandscape.nlbl";
                        ILabel label = PrintEngineFactory.PrintEngine.OpenLabel(labelpath);
                        label.PrintSettings.PrinterName = PrinterSelect.SelectedItem.ToString();

                        label.Variables["ReportTitle"].SetValue(ReportTitle.Text);
                        label.Variables["Page"].SetValue(pagecount.ToString());
                        label.Variables["Data"].SetValue(records);
                        label.Variables["ColumnHeads"].SetValue(Headers);
                        label.Variables["Totals"].SetValue(Totalsbar);
                        label.Variables["DateRange"].SetValue(dateheader);

                        ILabelPreviewSettings labelPreviewSettings = new LabelPreviewSettings();

                        labelPreviewSettings.PreviewToFile = false;                                    // if true, result will be the file name, if false, result will be a byte array.
                        labelPreviewSettings.ImageFormat = "jpg";                                      // file format of graphics.  Valid formats: JPG, PNG, BMP.
                        labelPreviewSettings.Width = this.LandscapeBox.Width;                            // Width of image to generate
                        labelPreviewSettings.Height = this.LandscapeBox.Height;                          // Height of image to generate
                                                                                                         //labelPreviewSettings.Destination = this.textBoxFileName.Text;                // If PrintToFile is true, this is the name of the file that will be generated.
                        labelPreviewSettings.FormatPreviewSide = FormatPreviewSide.FrontSide;          // Which label side(s) to generate the image for.  

                        // Generate Preview File
                        object imageObj = label.GetLabelPreview(labelPreviewSettings);

                        // Display image in UI
                        if (imageObj is byte[])
                        {
                            // When PrintToFiles = false
                            // Convert byte[] to Bitmap and set as image source for PictureBox control
                            PortraitBox.Visible = false;
                            CSVBox.Visible = false;
                            LandscapeBox.Visible = true;
                            LandscapeBox.Image = this.ByteToImage((byte[])imageObj);
                        }

                        pagecount++;
                    } while (!done);
                }
                else { MessageBox.Show("No data found in selected date range for selected report."); }
            }
            else { MessageBox.Show("Missing configs"); }
        }
        void PortraitSumedPP()
        {
            string path11 = @"GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            int dbsetting = 0, tbsetting = 0, starttimesetting = 0;
            string[] sumcolumns;
            string timedatename = "";
            bool selfsort = false;
            int rowsperpage = 61;
            DateTime start = DateRangeSelector.SelectionStart;
            DateTime end = DateRangeSelector.SelectionEnd;
            SQLData[] AllRecords = null;

            switch (FPKPKGGEN.SelectedIndex)
            {
                case 0:
                    sumcolumns = new string[7] { "Metal_Present", "Tare_Weight", "Net_Weight", "Check_Weight", "Clips_Present", "Foil_Check", "Filling_To_Long" };
                    timedatename = "Time_Date";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 5;
                            tbsetting = 6;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 13;
                            tbsetting = 14;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 1:
                    sumcolumns = new string[2] { "PALLET_SIZE", "PALLET_WEIGHT" };
                    timedatename = "Date_Time";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 3;
                            tbsetting = 4;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 11;
                            tbsetting = 12;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 2:
                    timedatename = "Date_Time";
                    sumcolumns = new string[1] { "CPM" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 7;
                            tbsetting = 8;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 15;
                            tbsetting = 16;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 3:
                    timedatename = "Time_Date";
                    sumcolumns = new string[5] { "Poly_UseL1", "Poly_UseL2", "Job_Count", "Check_Weigher_Count", "Evo_Print_Count" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 9;
                            tbsetting = 10;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 17;
                            tbsetting = 18;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                default:
                    sumcolumns = new string[1] { "" };
                    dbsetting = 0;
                    tbsetting = 0;
                    break;
            }
            DateTime Start = fixstarttime(start, Int32.Parse(settings[starttimesetting]));
            DateTime End = fixendtime(end, Int32.Parse(settings[starttimesetting]));
            for (int x = 0; x < sumcolumns.Length; x++)
            {
                if (SumColumnsBy.SelectedIndex == -1 && OrderBy.SelectedItem.ToString() != sumcolumns[x])
                {
                    selfsort = false;
                }
                else { selfsort = true; break; }
            }
            string[] columns = new string[6];
            int sumbycolumn = 0;
            for (int x = 1; x < 6; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedItem.ToString() == SumColumnsBy.SelectedItem.ToString())
                {
                    columns[0] = cb.SelectedItem.ToString();
                    sumbycolumn = x;
                    break;
                }
            }
            int columncount = 1, Tcolumncount = 0;
            int skipped = 0;
            for (int x = 1; x < 6; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedIndex == -1 || x == sumbycolumn)
                { skipped++; }
                else { columns[x - skipped] = cb.SelectedItem.ToString(); columncount++; }
            }

            bool[] Summable = new bool[5];
            int prskip = 0;
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < sumcolumns.Length; y++)
                {
                    if (columns[x] == sumcolumns[y]) 
                    { Summable[x - prskip] = true; break; }
                    else if (columns[x] == "Pallet Count" || columns[x] == "Record Count")
                    { prskip++; break; }
                    else { Summable[x] = false; }
                }
            }
            if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
            {
                string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString = "SELECT ";
                //Change this creation of cmd string to be in a loop to iterate the number of columns
                //and then patch together the string one column at a time checking if it has a -1 selected index each time  or pallet count
                //this should allow to also check other things like for Pallet count and pivot later in the function to address that issue
                for (int x = 0; x <= columncount; x++)
                {
                    if (columns[x] != "Pallet Count" && columns[x] != "Record Count")
                    { cmdString = cmdString + columns[x] + ", "; }
                    else { columncount--; }
                }

                for (int g = 0; g < 10; g++)
                {
                    if (cmdString.Substring(cmdString.Length - 1, 1) == " " || cmdString.Substring(cmdString.Length - 1, 1) == ",")
                    {
                        cmdString = cmdString.Remove(cmdString.Length - 1, 1);
                    }
                }

                cmdString = cmdString + ", ";
                if (TColumn1.SelectedIndex != -1 && TColumn1.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn1.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn2.SelectedIndex != -1 && TColumn2.SelectedItem.ToString() != "Pallet Count" && TColumn2.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn2.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn3.SelectedIndex != -1 && TColumn3.SelectedItem.ToString() != "Pallet Count" && TColumn3.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn3.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn4.SelectedIndex != -1 && TColumn4.SelectedItem.ToString() != "Pallet Count" && TColumn4.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn4.SelectedItem.ToString() + ", "; Tcolumncount++; }

                for (int g = 0; g < 10; g++)
                {
                    if (cmdString.Substring(cmdString.Length - 1, 1) == " " || cmdString.Substring(cmdString.Length - 1, 1) == ",")
                    {
                        cmdString = cmdString.Remove(cmdString.Length - 1, 1);
                    }
                }

                cmdString = cmdString + " FROM " + settings[tbsetting] + " WHERE " + timedatename + " >= @dtStart AND " + timedatename + " <= @dtEnd";
                if (OnlyPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE = '1'";
                }
                else if (NoPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE != '1'";
                }
                if (OrderBy.SelectedIndex != -1 && AscorDesc.SelectedIndex != -1 && !selfsort)
                {
                    cmdString = cmdString + " ORDER BY " + OrderBy.SelectedItem + " " + AscorDesc.SelectedItem + ";";
                }
                else { cmdString = cmdString + ";"; }

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
                            var list = new List<SQLData>();
                            while (reader.Read())
                            {
                                //list.Add(new SQLData { Col1 = reader.GetString(0), Col2 = reader.GetString(1), Col3 = reader.GetString(2), Col4 = reader.GetString(3), Col5 = reader.GetString(4), Col6 = reader.GetString(5), Col7 = reader.GetString(6), TCol1 = reader.GetString(7), TCol2 = reader.GetString(8), TCol3 = reader.GetString(9), TCol4 = reader.GetString(10), TCol5 = reader.GetString(11), TCol6 = reader.GetString(12) });
                                list.Add(new SQLData());
                                int tcolumnstart = columncount;
                                if (columncount > 0)
                                { try { list[list.Count - 1].Col1 = reader.GetString(0); } catch (Exception t) { }; }
                                if (columncount > 1)
                                { try { list[list.Count - 1].Col2 = reader.GetString(1); } catch (Exception t) { }; }
                                if (columncount > 2)
                                { try { list[list.Count - 1].Col3 = reader.GetString(2); } catch (Exception t) { }; }
                                if (columncount > 3)
                                { try { list[list.Count - 1].Col4 = reader.GetString(3); } catch (Exception t) { }; }
                                if (columncount > 4)
                                { try { list[list.Count - 1].Col5 = reader.GetString(4); } catch (Exception t) { }; }


                                if (Tcolumncount > 0)
                                { try { list[list.Count - 1].TCol1 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 1)
                                { try { list[list.Count - 1].TCol2 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 2)
                                { try { list[list.Count - 1].TCol3 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 3)
                                { try { list[list.Count - 1].TCol4 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }

                                AllRecords = list.ToArray();
                            }
                        }
                    }
                }

                if (AllRecords != null)
                {
                    int RecordCount = 0;
                    string[,] Totals = new string[AllRecords.Length, 6];
                    int uniquerecords = 0;
                    int[] tcoltotals = new int[4] { 0, 0, 0, 0 };
                    for (int x = 0; x < AllRecords.Length; x++)
                    {
                        RecordCount++;
                        bool found = false;
                        for (int y = 0; y < AllRecords.Length; y++)
                        {
                            if (AllRecords[x].Col1 == Totals[y, 0])
                            {
                                if (Summable[1]) { Totals[y, 1] = (Int32.Parse(Totals[y, 1]) + Int32.Parse(AllRecords[x].Col2)).ToString(); }
                                if (Summable[2]) { Totals[y, 2] = (Int32.Parse(Totals[y, 2]) + Int32.Parse(AllRecords[x].Col3)).ToString(); }
                                if (Summable[3]) { Totals[y, 3] = (Int32.Parse(Totals[y, 3]) + Int32.Parse(AllRecords[x].Col4)).ToString(); }
                                if (Summable[4]) { Totals[y, 4] = (Int32.Parse(Totals[y, 4]) + Int32.Parse(AllRecords[x].Col5)).ToString(); }

                                if (Column1.SelectedIndex != -1)
                                {
                                    if (Column1.SelectedItem.ToString() == "Pallet Count" || Column1.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 0] = (Int32.Parse(Totals[y, 0]) + 1).ToString(); }
                                }
                                if (Column2.SelectedIndex != -1)
                                {
                                    if (Column2.SelectedItem.ToString() == "Pallet Count" || Column2.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 1] = (Int32.Parse(Totals[y, 1]) + 1).ToString(); }
                                }
                                if (Column3.SelectedIndex != -1)
                                {
                                    if (Column3.SelectedItem.ToString() == "Pallet Count" || Column3.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 2] = (Int32.Parse(Totals[y, 2]) + 1).ToString(); }
                                }
                                if (Column4.SelectedIndex != -1)
                                {
                                    if (Column4.SelectedItem.ToString() == "Pallet Count" || Column4.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 3] = (Int32.Parse(Totals[y, 3]) + 1).ToString(); }
                                }
                                if (Column5.SelectedIndex != -1)
                                {
                                    if (Column5.SelectedItem.ToString() == "Pallet Count" || Column5.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 4] = (Int32.Parse(Totals[y, 4]) + 1).ToString(); }
                                }

                                Totals[y, 5] = (Int32.Parse(Totals[y, 5]) + 1).ToString();
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            for (int y = 0; y < AllRecords.Length; y++)
                            {
                                if (Totals[y, 0] == null || Totals[y, 0] == string.Empty || Totals[y, 0] == "")
                                {
                                    Totals[y, 0] = AllRecords[x].Col1;
                                    Totals[y, 1] = AllRecords[x].Col2;
                                    Totals[y, 2] = AllRecords[x].Col3;
                                    Totals[y, 3] = AllRecords[x].Col4;
                                    Totals[y, 4] = AllRecords[x].Col5;

                                    Totals[y, 5] = "1";
                                    if (Column1.SelectedIndex != -1)
                                    {
                                        if (Column1.SelectedItem.ToString() == "Pallet Count" || Column1.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 0] = "1"; }
                                    }
                                    if (Column2.SelectedIndex != -1)
                                    {
                                        if (Column2.SelectedItem.ToString() == "Pallet Count" || Column2.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 1] = "1"; }
                                    }
                                    if (Column3.SelectedIndex != -1)
                                    {
                                        if (Column3.SelectedItem.ToString() == "Pallet Count" || Column3.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 2] = "1"; }
                                    }
                                    if (Column4.SelectedIndex != -1)
                                    {
                                        if (Column4.SelectedItem.ToString() == "Pallet Count" || Column4.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 3] = "1"; }
                                    }
                                    if (Column5.SelectedIndex != -1)
                                    {
                                        if (Column5.SelectedItem.ToString() == "Pallet Count" || Column5.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 4] = "1"; }
                                    }

                                    uniquerecords++;
                                    break;
                                }
                            }
                        }
                        int sqlselector = 0;
                        for (int f = 0; f < 4; f++)
                        {

                            Control[] ct = this.Controls.Find("TColumn" + (f + 1).ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (cb.SelectedIndex != -1)
                            {
                                if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                                { tcoltotals[f]++; }
                                else
                                {
                                    if (sqlselector == 0)
                                    { if (AllRecords[x].TCol1 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol1); } }
                                    if (sqlselector == 1)
                                    { if (AllRecords[x].TCol2 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol2); } }
                                    if (sqlselector == 2)
                                    { if (AllRecords[x].TCol3 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol3); } }
                                    if (sqlselector == 3)
                                    { if (AllRecords[x].TCol4 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol4); } }

                                    sqlselector++;
                                }
                            }
                        }
                    }
                    #region sorting
                    int sortby = findsortby();
                    bool sortbyisSummable = false;
                    Control[] cts = this.Controls.Find("Column" + (sortby + 1).ToString(), true);
                    ComboBox cbs = cts[0] as ComboBox;
                    if (Summable[sortby] || cbs.SelectedItem.ToString() == "Pallet Count" || cbs.SelectedItem.ToString() == "Record Count")
                    { sortbyisSummable = true; }
                    if (cbs.SelectedItem.ToString() == "Pallet Count" || cbs.SelectedItem.ToString() == "Record Count")
                    { sortby = 5; }
                    if (AscorDesc.SelectedIndex == 0)
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length; y++)
                            {
                                string[] holding = new string[6];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) < 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) < Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null)
                                { break; }
                            }
                        }
                    }
                    else
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length; y++)
                            {
                                string[] holding = new string[6];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) > 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) > Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null)
                                { break; }
                            }
                        }
                    }
                    #endregion
                    int pagecount = 1;
                    bool done = true;
                    int rows = 0;
                    for (int x = 1; x < 6; x++)
                    {
                        Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                        ComboBox cb = ct[0] as ComboBox;
                        if (cb.SelectedIndex != -1)
                        {
                            if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                            { columncount++; }
                        }
                    }
                    int charpercolumn = (80 / columncount + prskip) - 1;
                    do
                    {
                        CultureInfo culture = CultureInfo.GetCultureInfo("en-US");
                        string dateheader = start.ToString("d", culture) + " - " + end.ToString("d", culture);
                        string Headers = "";
                        string Totalsbar = "Totals:";
                        string records = "";
                        for (int x = 0; x < 5; x++)
                        {
                            Control[] ct = this.Controls.Find("Column" + (x + 1).ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (columns[x] != null)
                            {
                                if (columns[x].Length > charpercolumn)
                                {
                                    Headers = Headers + (columns[x].Substring(0, charpercolumn - 1)).PadRight(charpercolumn, ' ');
                                }
                                else
                                {
                                    Headers = Headers + columns[x].PadRight(charpercolumn, ' ');
                                }
                            }
                        }
                        for (int x = 1; x <= tcoltotals.Length; x++)
                        {
                            Control[] ct = this.Controls.Find("TColumn" + x.ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (cb.SelectedIndex != -1)
                            {
                                Totalsbar = Totalsbar + (cb.SelectedItem.ToString() + ": " + tcoltotals[x - 1]) + " ";
                            }
                        }

                        for (int x = (pagecount - 1) * rowsperpage; x <= uniquerecords && x < pagecount * rowsperpage; x++)
                        {
                            if (Totals[x, 0] == null) { Totals[x, 0] = ""; }
                            if (Totals[x, 1] == null) { Totals[x, 1] = ""; }
                            if (Totals[x, 2] == null) { Totals[x, 2] = ""; }
                            if (Totals[x, 3] == null) { Totals[x, 3] = ""; }
                            if (Totals[x, 4] == null) { Totals[x, 4] = ""; }
                            if (Totals[x, 5] == null) { Totals[x, 5] = ""; }

                            if (Totals[x, 0].Length >= charpercolumn)
                            { Totals[x, 0] = Totals[x, 0].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 1].Length >= charpercolumn)
                            { Totals[x, 1] = Totals[x, 1].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 2].Length >= charpercolumn)
                            { Totals[x, 2] = Totals[x, 2].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 3].Length >= charpercolumn)
                            { Totals[x, 3] = Totals[x, 3].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 4].Length >= charpercolumn)
                            { Totals[x, 4] = Totals[x, 4].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 5].Length >= charpercolumn)
                            { Totals[x, 5] = Totals[x, 5].Substring(0, charpercolumn - 1); }

                            int f = 0;

                            //if (Column1.SelectedIndex != -1)
                            //{
                            //    if (Column1.SelectedItem.ToString() == "Pallet Count" || Column1.SelectedItem.ToString() == "Record Count")
                            //    { records = records + Totals[x, 5].PadRight(charpercolumn, ' '); }
                            //}
                            if (Totals[x, 0] != "")
                            { records = records + Totals[x, 0].PadRight(charpercolumn, ' '); }
                            //if (Column2.SelectedIndex != -1)
                            //{
                            //    if (Column2.SelectedItem.ToString() == "Pallet Count" || Column2.SelectedItem.ToString() == "Record Count")
                            //    { records = records + Totals[x, 5].PadRight(charpercolumn, ' '); }
                            //}
                            if (Totals[x, 1] != "")
                            { records = records + Totals[x, 1].PadRight(charpercolumn, ' '); }
                            //if (Column3.SelectedIndex != -1)
                            //{
                            //    if (Column3.SelectedItem.ToString() == "Pallet Count" || Column3.SelectedItem.ToString() == "Record Count")
                            //    { records = records + Totals[x, 5].PadRight(charpercolumn, ' '); }
                            //}
                            if (Totals[x, 2] != "")
                            { records = records + Totals[x, 2].PadRight(charpercolumn, ' '); }
                            //if (Column4.SelectedIndex != -1)
                            //{
                            //    if (Column4.SelectedItem.ToString() == "Pallet Count" || Column4.SelectedItem.ToString() == "Record Count")
                            //    { records = records + Totals[x, 5].PadRight(charpercolumn, ' '); }
                            //}
                            if (Totals[x, 3] != "")
                            { records = records + Totals[x, 3].PadRight(charpercolumn, ' '); }
                            //if (Column5.SelectedIndex != -1)
                            //{
                            //    if (Column5.SelectedItem.ToString() == "Pallet Count" || Column5.SelectedItem.ToString() == "Record Count")
                            //    { records = records + Totals[x, 5].PadRight(charpercolumn, ' '); }
                            //}
                            if (Totals[x, 4] != "")
                            { records = records + Totals[x, 4].PadRight(charpercolumn, ' '); }

                            records = records + "\r\n";
                            rows++;
                        }
                        if (rows >= uniquerecords)
                        { done = true; }
                        string labelpath = "ReportPortrait.nlbl";
                        ILabel label = PrintEngineFactory.PrintEngine.OpenLabel(labelpath);
                        label.PrintSettings.PrinterName = PrinterSelect.SelectedItem.ToString();

                        label.Variables["ReportTitle"].SetValue(ReportTitle.Text);
                        label.Variables["Page"].SetValue(pagecount.ToString());
                        label.Variables["Data"].SetValue(records);
                        label.Variables["ColumnHeads"].SetValue(Headers);
                        label.Variables["Totals"].SetValue(Totalsbar);
                        label.Variables["DateRange"].SetValue(dateheader);

                        ILabelPreviewSettings labelPreviewSettings = new LabelPreviewSettings();

                        labelPreviewSettings.PreviewToFile = false;                                    // if true, result will be the file name, if false, result will be a byte array.
                        labelPreviewSettings.ImageFormat = "jpg";                                      // file format of graphics.  Valid formats: JPG, PNG, BMP.
                        labelPreviewSettings.Width = this.PortraitBox.Width;                            // Width of image to generate
                        labelPreviewSettings.Height = this.PortraitBox.Height;                          // Height of image to generate
                                                                                                        //labelPreviewSettings.Destination = this.textBoxFileName.Text;                // If PrintToFile is true, this is the name of the file that will be generated.
                        labelPreviewSettings.FormatPreviewSide = FormatPreviewSide.FrontSide;          // Which label side(s) to generate the image for.  

                        // Generate Preview File
                        object imageObj = label.GetLabelPreview(labelPreviewSettings);

                        // Display image in UI
                        if (imageObj is byte[])
                        {
                            // When PrintToFiles = false
                            // Convert byte[] to Bitmap and set as image source for PictureBox control
                            LandscapeBox.Visible = false;
                            CSVBox.Visible = false;
                            PortraitBox.Visible = true;
                            PortraitBox.Image = this.ByteToImage((byte[])imageObj);
                        }

                        pagecount++;
                    } while (!done);
                }
                else { MessageBox.Show("No data found in selected date range for selected report."); }
            }
            else { MessageBox.Show("Missing configs"); }
        }
        void PortraitPP()
        {
            string path11 = @"GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            int dbsetting = 0, tbsetting = 0, starttimesetting = 0;
            string timedatename = "";
            int rowsperpage = 61;
            DateTime start = DateRangeSelector.SelectionStart;
            DateTime end = DateRangeSelector.SelectionEnd;
            string[] sumcolumns;
            switch (FPKPKGGEN.SelectedIndex)
            {
                case 0:
                    sumcolumns = new string[7] { "Metal_Present", "Tare_Weight", "Net_Weight", "Check_Weight", "Clips_Present", "Foil_Check", "Filling_To_Long" };
                    timedatename = "Time_Date";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 5;
                            tbsetting = 6;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 13;
                            tbsetting = 14;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 1:
                    sumcolumns = new string[2] { "PALLET_SIZE", "PALLET_WEIGHT" };
                    timedatename = "Date_Time";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 3;
                            tbsetting = 4;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 11;
                            tbsetting = 12;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 2:
                    timedatename = "Date_Time";
                    sumcolumns = new string[1] { "CPM" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 7;
                            tbsetting = 8;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 15;
                            tbsetting = 16;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 3:
                    timedatename = "Time_Date";
                    sumcolumns = new string[5] { "Poly_UseL1", "Poly_UseL2", "Job_Count", "Check_Weigher_Count", "Evo_Print_Count" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 9;
                            tbsetting = 10;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 17;
                            tbsetting = 18;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                default:
                    sumcolumns = new string[1] { "" };
                    dbsetting = 0;
                    tbsetting = 0;
                    break;
            }
            DateTime Start = fixstarttime(start, Int32.Parse(settings[starttimesetting]));
            DateTime End = fixendtime(end, Int32.Parse(settings[starttimesetting]));
            string[] columns = new string[7];
            int ColumnCount = 0, Tcolumncount = 0;
            bool selfsort = true;
            SQLData[] AllRecords = null;
            bool[] Summable = new bool[7];
            int skipped = 0;
            for (int x = 1; x < 6; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedIndex != -1 && cb.SelectedItem.ToString() != "Pallet Count" && cb.SelectedItem.ToString() != "Record Count")
                {
                    columns[x - 1 - skipped] = cb.SelectedItem.ToString();
                    ColumnCount++;
                    for (int y = 0; y < sumcolumns.Length; y++)
                    {
                        if (cb.SelectedItem.ToString() == sumcolumns[y])
                        {
                            Summable[x] = true;
                        }
                    }
                }
                else { skipped++; }
            }
            if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
            {
                string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString = "SELECT ";
                for (int x = 0; x <= ColumnCount; x++)
                {
                    cmdString = cmdString + columns[x] + ", ";
                }


                for (int g = 0; g < 10; g++)
                {
                    if (cmdString.Substring(cmdString.Length - 1, 1) == " " || cmdString.Substring(cmdString.Length - 1, 1) == ",")
                    {
                        cmdString = cmdString.Remove(cmdString.Length - 1, 1);
                    }
                }

                cmdString = cmdString + ", ";
                if (TColumn1.SelectedIndex != -1 && TColumn1.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn1.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn2.SelectedIndex != -1 && TColumn2.SelectedItem.ToString() != "Pallet Count" && TColumn2.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn2.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn3.SelectedIndex != -1 && TColumn3.SelectedItem.ToString() != "Pallet Count" && TColumn3.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn3.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn4.SelectedIndex != -1 && TColumn4.SelectedItem.ToString() != "Pallet Count" && TColumn4.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn4.SelectedItem.ToString() + ", "; Tcolumncount++; }

                for (int g = 0; g < 10; g++)
                {
                    if (cmdString.Substring(cmdString.Length - 1, 1) == " " || cmdString.Substring(cmdString.Length - 1, 1) == ",")
                    {
                        cmdString = cmdString.Remove(cmdString.Length - 1, 1);
                    }
                }

                cmdString = cmdString + " FROM " + settings[tbsetting] + " WHERE " + timedatename + " >= @dtStart AND " + timedatename + " <= @dtEnd";

                if (OnlyPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE = '1'";
                }
                else if (NoPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE != '1'";
                }

                if (OrderBy.SelectedIndex != -1 && AscorDesc.SelectedIndex != -1 && !selfsort)
                {
                    cmdString = cmdString + " ORDER BY " + OrderBy.SelectedItem + " " + AscorDesc.SelectedItem + ";";
                }
                else { cmdString = cmdString + ";"; }

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
                            var list = new List<SQLData>();
                            while (reader.Read())
                            {
                                list.Add(new SQLData());
                                int tcolumnstart = ColumnCount;
                                if (ColumnCount > 0)
                                { try { list[list.Count - 1].Col1 = reader.GetString(0); } catch (Exception t) { }; }
                                if (ColumnCount > 1)
                                { try { list[list.Count - 1].Col2 = reader.GetString(1); } catch (Exception t) { }; }
                                if (ColumnCount > 2)
                                { try { list[list.Count - 1].Col3 = reader.GetString(2); } catch (Exception t) { }; }
                                if (ColumnCount > 3)
                                { try { list[list.Count - 1].Col4 = reader.GetString(3); } catch (Exception t) { }; }
                                if (ColumnCount > 4)
                                { try { list[list.Count - 1].Col5 = reader.GetString(4); } catch (Exception t) { }; }

                                if (Tcolumncount > 0)
                                { try { list[list.Count - 1].TCol1 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 1)
                                { try { list[list.Count - 1].TCol2 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 2)
                                { try { list[list.Count - 1].TCol3 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 3)
                                { try { list[list.Count - 1].TCol4 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }

                                AllRecords = list.ToArray();
                            }
                        }
                    }
                }
                if (AllRecords != null)
                {
                    int RecordCount = 0;
                    string[,] Totals = new string[AllRecords.Length, 5];
                    int[] tcoltotals = new int[4] { 0, 0, 0, 0 };
                    for (int x = 0; x < AllRecords.Length; x++)
                    {
                        Totals[x, 0] = AllRecords[x].Col1;
                        Totals[x, 1] = AllRecords[x].Col2;
                        Totals[x, 2] = AllRecords[x].Col3;
                        Totals[x, 3] = AllRecords[x].Col4;
                        Totals[x, 4] = AllRecords[x].Col5;

                        RecordCount++;

                        int sqlselector = 0;
                        for (int f = 0; f < 4; f++)
                        {
                            Control[] ct = this.Controls.Find("TColumn" + (f + 1).ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (cb.SelectedIndex != -1)
                            {
                                if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                                { tcoltotals[f]++; }
                                else
                                {
                                    if (sqlselector == 0)
                                    { if (AllRecords[x].TCol1 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol1); } }
                                    if (sqlselector == 1)
                                    { if (AllRecords[x].TCol2 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol2); } }
                                    if (sqlselector == 2)
                                    { if (AllRecords[x].TCol3 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol3); } }
                                    if (sqlselector == 3)
                                    { if (AllRecords[x].TCol4 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol4); } }

                                    sqlselector++;
                                }
                            }
                        }
                    }
                    #region sorting
                    int sortby = findsortby();
                    bool sortbyisSummable = false;
                    Control[] cts = this.Controls.Find("Column" + (sortby + 1).ToString(), true);
                    ComboBox cbs = cts[0] as ComboBox;
                    if (Summable[sortby] || cbs.SelectedItem.ToString() == "Pallet Count" || cbs.SelectedItem.ToString() == "Record Count")
                    { sortbyisSummable = true; }
                    if (AscorDesc.SelectedIndex == 0)
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length - 1; y++)
                            {
                                string[] holding = new string[5];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) < 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) < Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                { break; }
                            }
                        }
                    }
                    else
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length - 1; y++)
                            {
                                string[] holding = new string[5];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) > 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) > Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                { break; }
                            }
                        }
                    }
                    #endregion
                    int pagecount = 1;
                    bool done = true;
                    int rows = 0;
                    int charpercolumn = (80 / ColumnCount) - 1;
                    do
                    {
                        CultureInfo culture = CultureInfo.GetCultureInfo("en-US");
                        string dateheader = start.ToString("d", culture) + " - " + end.ToString("d", culture);
                        string Headers = "";
                        string Totalsbar = "Totals:";
                        string records = "";

                        for (int x = 0; x < 5; x++)
                        {
                            Control[] ct = this.Controls.Find("Column" + (x + 1).ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (columns[x] != null)
                            {
                                if (columns[x].Length > charpercolumn)
                                {
                                    Headers = Headers + (columns[x].Substring(0, charpercolumn - 1)).PadRight(charpercolumn, ' ');
                                }
                                else
                                {
                                    Headers = Headers + columns[x].PadRight(charpercolumn, ' ');
                                }
                            }
                        }

                        for (int x = 1; x <= tcoltotals.Length; x++)
                        {
                            Control[] ct = this.Controls.Find("TColumn" + x.ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (cb.SelectedIndex != -1)
                            {
                                Totalsbar = Totalsbar + (cb.SelectedItem.ToString() + ": " + tcoltotals[x - 1]) + " ";
                            }
                        }
                        
                        for (int x = (pagecount - 1) * rowsperpage; x <= RecordCount && x < pagecount * rowsperpage; x++)
                        {
                            if (Totals[x, 0] == null) { Totals[x, 0] = ""; }
                            if (Totals[x, 1] == null) { Totals[x, 1] = ""; }
                            if (Totals[x, 2] == null) { Totals[x, 2] = ""; }
                            if (Totals[x, 3] == null) { Totals[x, 3] = ""; }
                            if (Totals[x, 4] == null) { Totals[x, 4] = ""; }

                            if (Totals[x, 0].Length >= charpercolumn)
                            { Totals[x, 0] = Totals[x, 0].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 1].Length >= charpercolumn)
                            { Totals[x, 1] = Totals[x, 1].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 2].Length >= charpercolumn)
                            { Totals[x, 2] = Totals[x, 2].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 3].Length >= charpercolumn)
                            { Totals[x, 3] = Totals[x, 3].Substring(0, charpercolumn - 1); }
                            if (Totals[x, 4].Length >= charpercolumn)
                            { Totals[x, 4] = Totals[x, 4].Substring(0, charpercolumn - 1); }
      
                            if (Totals[x, 0] != "")
                            { records = records + Totals[x, 0].PadRight(charpercolumn, ' '); }
                            if (Totals[x, 1] != "")
                            { records = records + Totals[x, 1].PadRight(charpercolumn, ' '); }
                            if (Totals[x, 2] != "")
                            { records = records + Totals[x, 2].PadRight(charpercolumn, ' '); }
                            if (Totals[x, 3] != "")
                            { records = records + Totals[x, 3].PadRight(charpercolumn, ' '); }
                            if (Totals[x, 4] != "")
                            { records = records + Totals[x, 4].PadRight(charpercolumn, ' '); }

                            records = records + "\r\n";
                            rows++;
                        }
                        if (rows >= RecordCount)
                        { done = true; }
                        string labelpath = "ReportPortrait.nlbl";
                        ILabel label = PrintEngineFactory.PrintEngine.OpenLabel(labelpath);
                        label.PrintSettings.PrinterName = PrinterSelect.SelectedItem.ToString();

                        label.Variables["ReportTitle"].SetValue(ReportTitle.Text);
                        label.Variables["Page"].SetValue(pagecount.ToString());
                        label.Variables["Data"].SetValue(records);
                        label.Variables["ColumnHeads"].SetValue(Headers);
                        label.Variables["Totals"].SetValue(Totalsbar);
                        label.Variables["DateRange"].SetValue(dateheader);

                        ILabelPreviewSettings labelPreviewSettings = new LabelPreviewSettings();

                        labelPreviewSettings.PreviewToFile = false;                                    // if true, result will be the file name, if false, result will be a byte array.
                        labelPreviewSettings.ImageFormat = "jpg";                                      // file format of graphics.  Valid formats: JPG, PNG, BMP.
                        labelPreviewSettings.Width = this.PortraitBox.Width;                            // Width of image to generate
                        labelPreviewSettings.Height = this.PortraitBox.Height;                          // Height of image to generate
                                                                                                        //labelPreviewSettings.Destination = this.textBoxFileName.Text;                // If PrintToFile is true, this is the name of the file that will be generated.
                        labelPreviewSettings.FormatPreviewSide = FormatPreviewSide.FrontSide;          // Which label side(s) to generate the image for.  

                        // Generate Preview File
                        object imageObj = label.GetLabelPreview(labelPreviewSettings);

                        // Display image in UI
                        if (imageObj is byte[])
                        {
                            // When PrintToFiles = false
                            // Convert byte[] to Bitmap and set as image source for PictureBox control
                            LandscapeBox.Visible = false;
                            CSVBox.Visible = false;
                            PortraitBox.Visible = true;
                            PortraitBox.Image = this.ByteToImage((byte[])imageObj);
                        }

                        pagecount++;
                    }
                    while (!done);
                }
            }
        }
        #endregion
        #region Print
        void PortraitSumedPrint()
        {
            string path11 = @"GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            int dbsetting = 0, tbsetting = 0, starttimesetting = 0;
            string[] sumcolumns;
            string timedatename = "";
            bool selfsort = false;
            int rowsperpage = 61;
            DateTime start = DateRangeSelector.SelectionStart;
            DateTime end = DateRangeSelector.SelectionEnd;
            SQLData[] AllRecords = null;

            switch (FPKPKGGEN.SelectedIndex)
            {
                case 0:
                    sumcolumns = new string[7] { "Metal_Present", "Tare_Weight", "Net_Weight", "Check_Weight", "Clips_Present", "Foil_Check", "Filling_To_Long" };
                    timedatename = "Time_Date";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 5;
                            tbsetting = 6;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 13;
                            tbsetting = 14;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 1:
                    sumcolumns = new string[2] { "PALLET_SIZE", "PALLET_WEIGHT" };
                    timedatename = "Date_Time";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 3;
                            tbsetting = 4;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 11;
                            tbsetting = 12;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 2:
                    timedatename = "Date_Time";
                    sumcolumns = new string[1] { "CPM" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 7;
                            tbsetting = 8;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 15;
                            tbsetting = 16;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 3:
                    timedatename = "Time_Date";
                    sumcolumns = new string[5] { "Poly_UseL1", "Poly_UseL2", "Job_Count", "Check_Weigher_Count", "Evo_Print_Count" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 9;
                            tbsetting = 10;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 17;
                            tbsetting = 18;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                default:
                    sumcolumns = new string[1] { "" };
                    dbsetting = 0;
                    tbsetting = 0;
                    break;
            }
            DateTime Start = fixstarttime(start, Int32.Parse(settings[starttimesetting]));
            DateTime End = fixendtime(end, Int32.Parse(settings[starttimesetting]));
            for (int x = 0; x < sumcolumns.Length; x++)
            {
                if (SumColumnsBy.SelectedIndex == -1 && OrderBy.SelectedItem.ToString() != sumcolumns[x])
                {
                    selfsort = false;
                }
                else { selfsort = true; break; }
            }
            string[] columns = new string[6];
            int sumbycolumn = 0;
            for (int x = 1; x < 6; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedItem.ToString() == SumColumnsBy.SelectedItem.ToString())
                {
                    columns[0] = cb.SelectedItem.ToString();
                    sumbycolumn = x;
                    break;
                }
            }
            int columncount = 1, Tcolumncount = 0;
            int skipped = 0;
            for (int x = 1; x < 6; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedIndex == -1 || x == sumbycolumn || cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                { skipped++; }
                else { columns[x - skipped] = cb.SelectedItem.ToString(); columncount++; }
            }

            bool[] Summable = new bool[5];
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < sumcolumns.Length; y++)
                {
                    if (columns[x] == sumcolumns[y]) { Summable[x] = true; break; }
                    else { Summable[x] = false; }
                }
            }
            if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
            {
                string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString = "SELECT ";
                //Change this creation of cmd string to be in a loop to iterate the number of columns
                //and then patch together the string one column at a time checking if it has a -1 selected index each time  or pallet count
                //this should allow to also check other things like for Pallet count and pivot later in the function to address that issue
                for (int x = 0; x <= columncount; x++)
                {
                    cmdString = cmdString + columns[x] + ", ";
                }
                if (cmdString[cmdString.Length - 2] == ' ')
                { cmdString = cmdString.Remove(cmdString.Length - 1, 1); }
                if (cmdString[cmdString.Length - 2] == ',')
                { cmdString = cmdString.Remove(cmdString.Length - 2, 1); }
                if (TColumn1.SelectedIndex != -1 && TColumn1.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn1.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn2.SelectedIndex != -1 && TColumn2.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn2.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn3.SelectedIndex != -1 && TColumn3.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn3.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn4.SelectedIndex != -1 && TColumn4.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn4.SelectedItem.ToString() + ", "; Tcolumncount++; }

                if (cmdString[cmdString.Length - 2] == ' ')
                { cmdString = cmdString.Remove(cmdString.Length - 1, 1); }
                if (cmdString[cmdString.Length - 2] == ',')
                { cmdString = cmdString.Remove(cmdString.Length - 2, 1); }
                cmdString = cmdString + " FROM " + settings[tbsetting] + " WHERE " + timedatename + " >= @dtStart AND " + timedatename + " <= @dtEnd";
                if (OnlyPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE = '1'";
                }
                else if (NoPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE != '1'";
                }
                if (OrderBy.SelectedIndex != -1 && AscorDesc.SelectedIndex != -1 && !selfsort)
                {
                    cmdString = cmdString + " ORDER BY " + OrderBy.SelectedItem + " " + AscorDesc.SelectedItem + ";";
                }
                else { cmdString = cmdString + ";"; }

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
                            var list = new List<SQLData>();
                            while (reader.Read())
                            {
                                //list.Add(new SQLData { Col1 = reader.GetString(0), Col2 = reader.GetString(1), Col3 = reader.GetString(2), Col4 = reader.GetString(3), Col5 = reader.GetString(4), Col6 = reader.GetString(5), Col7 = reader.GetString(6), TCol1 = reader.GetString(7), TCol2 = reader.GetString(8), TCol3 = reader.GetString(9), TCol4 = reader.GetString(10), TCol5 = reader.GetString(11), TCol6 = reader.GetString(12) });
                                list.Add(new SQLData());
                                int tcolumnstart = columncount;
                                if (columncount > 0)
                                { try { list[list.Count - 1].Col1 = reader.GetString(0); } catch (Exception t) { }; }
                                if (columncount > 1)
                                { try { list[list.Count - 1].Col2 = reader.GetString(1); } catch (Exception t) { }; }
                                if (columncount > 2)
                                { try { list[list.Count - 1].Col3 = reader.GetString(2); } catch (Exception t) { }; }
                                if (columncount > 3)
                                { try { list[list.Count - 1].Col4 = reader.GetString(3); } catch (Exception t) { }; }
                                if (columncount > 4)
                                { try { list[list.Count - 1].Col5 = reader.GetString(4); } catch (Exception t) { }; }
                                if (columncount > 5)

                                    if (Tcolumncount > 0)
                                    { try { list[list.Count - 1].TCol1 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 1)
                                { try { list[list.Count - 1].TCol2 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 2)
                                { try { list[list.Count - 1].TCol3 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 3)
                                { try { list[list.Count - 1].TCol4 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }

                                AllRecords = list.ToArray();
                            }
                        }
                    }
                }

                if (AllRecords != null)
                {
                    int RecordCount = 0;
                    string[,] Totals = new string[AllRecords.Length, 6];
                    int uniquerecords = 0;
                    int[] tcoltotals = new int[4] { 0, 0, 0, 0 };
                    for (int x = 0; x < AllRecords.Length; x++)
                    {
                        RecordCount++;
                        bool found = false;
                        for (int y = 0; y < AllRecords.Length; y++)
                        {
                            if (AllRecords[x].Col1 == Totals[y, 0])
                            {
                                if (Summable[1]) { Totals[y, 1] = (Int32.Parse(Totals[y, 1]) + Int32.Parse(AllRecords[x].Col2)).ToString(); }
                                if (Summable[2]) { Totals[y, 2] = (Int32.Parse(Totals[y, 2]) + Int32.Parse(AllRecords[x].Col3)).ToString(); }
                                if (Summable[3]) { Totals[y, 3] = (Int32.Parse(Totals[y, 3]) + Int32.Parse(AllRecords[x].Col4)).ToString(); }
                                if (Summable[4]) { Totals[y, 4] = (Int32.Parse(Totals[y, 4]) + Int32.Parse(AllRecords[x].Col5)).ToString(); }

                                if (Column1.SelectedIndex != -1)
                                {
                                    if (Column1.SelectedItem.ToString() == "Pallet Count" || Column1.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 0] = (Int32.Parse(Totals[y, 0]) + 1).ToString(); }
                                }
                                if (Column2.SelectedIndex != -1)
                                {
                                    if (Column2.SelectedItem.ToString() == "Pallet Count" || Column2.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 1] = (Int32.Parse(Totals[y, 1]) + 1).ToString(); }
                                }
                                if (Column3.SelectedIndex != -1)
                                {
                                    if (Column3.SelectedItem.ToString() == "Pallet Count" || Column3.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 2] = (Int32.Parse(Totals[y, 2]) + 1).ToString(); }
                                }
                                if (Column4.SelectedIndex != -1)
                                {
                                    if (Column4.SelectedItem.ToString() == "Pallet Count" || Column4.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 3] = (Int32.Parse(Totals[y, 3]) + 1).ToString(); }
                                }
                                if (Column5.SelectedIndex != -1)
                                {
                                    if (Column5.SelectedItem.ToString() == "Pallet Count" || Column5.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 4] = (Int32.Parse(Totals[y, 4]) + 1).ToString(); }
                                }

                                Totals[y, 5] = (Int32.Parse(Totals[y, 5]) + 1).ToString();
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            for (int y = 0; y < AllRecords.Length; y++)
                            {
                                if (Totals[y, 0] == null || Totals[y, 0] == string.Empty || Totals[y, 0] == "")
                                {
                                    Totals[y, 0] = AllRecords[x].Col1;
                                    Totals[y, 1] = AllRecords[x].Col2;
                                    Totals[y, 2] = AllRecords[x].Col3;
                                    Totals[y, 3] = AllRecords[x].Col4;
                                    Totals[y, 4] = AllRecords[x].Col5;

                                    Totals[y, 5] = "1";
                                    if (Column1.SelectedIndex != -1)
                                    {
                                        if (Column1.SelectedItem.ToString() == "Pallet Count" || Column1.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 0] = "1"; }
                                    }
                                    if (Column2.SelectedIndex != -1)
                                    {
                                        if (Column2.SelectedItem.ToString() == "Pallet Count" || Column2.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 1] = "1"; }
                                    }
                                    if (Column3.SelectedIndex != -1)
                                    {
                                        if (Column3.SelectedItem.ToString() == "Pallet Count" || Column3.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 2] = "1"; }
                                    }
                                    if (Column4.SelectedIndex != -1)
                                    {
                                        if (Column4.SelectedItem.ToString() == "Pallet Count" || Column4.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 3] = "1"; }
                                    }
                                    if (Column5.SelectedIndex != -1)
                                    {
                                        if (Column5.SelectedItem.ToString() == "Pallet Count" || Column5.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 4] = "1"; }
                                    }

                                    uniquerecords++;
                                    break;
                                }
                            }
                        }
                        int sqlselector = 0;
                        for (int f = 0; f < 4; f++)
                        {

                            Control[] ct = this.Controls.Find("TColumn" + (f + 1).ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (cb.SelectedIndex != -1)
                            {
                                if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                                { tcoltotals[f]++; }
                                else
                                {
                                    if (sqlselector == 0)
                                    { if (AllRecords[x].TCol1 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol1); } }
                                    if (sqlselector == 1)
                                    { if (AllRecords[x].TCol2 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol2); } }
                                    if (sqlselector == 2)
                                    { if (AllRecords[x].TCol3 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol3); } }
                                    if (sqlselector == 3)
                                    { if (AllRecords[x].TCol4 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol4); } }

                                    sqlselector++;
                                }
                            }
                        }
                    }
                    #region sorting
                    int sortby = findsortby();
                    bool sortbyisSummable = false;
                    Control[] cts = this.Controls.Find("Column" + (sortby + 1).ToString(), true);
                    ComboBox cbs = cts[0] as ComboBox;
                    if (Summable[sortby] || cbs.SelectedItem.ToString() == "Pallet Count" || cbs.SelectedItem.ToString() == "Record Count")
                    { sortbyisSummable = true; }
                    if (AscorDesc.SelectedIndex == 0)
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length; y++)
                            {
                                string[] holding = new string[6];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) < 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) < Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null)
                                { break; }
                            }
                        }
                    }
                    else
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length; y++)
                            {
                                string[] holding = new string[6];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) > 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) > Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null)
                                { break; }
                            }
                        }
                    }
                    #endregion
                    int pagecount = 1;
                    bool done = false;
                    int rows = 0;
                    for (int x = 1; x < 6; x++)
                    {
                        Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                        ComboBox cb = ct[0] as ComboBox;
                        if (cb.SelectedIndex != -1)
                        {
                            if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                            { columncount++; }
                        }
                    }
                    int charpercolumn = 80 / columncount;
                    int expectedpages = RecordCount / rowsperpage;
                    bool printOK = true;
                    if (expectedpages > 10)
                    {
                        string pagewarning = "Are you sure you want to print " + expectedpages.ToString() + " pages?";
                        AreYouSureYouWantToPrint aysywtp = new AreYouSureYouWantToPrint(pagewarning);
                        var result = aysywtp.ShowDialog();
                        if (result == DialogResult.Yes)
                        {
                            printOK = true;
                        }
                        else { printOK = false; }
                    }
                    if (printOK)
                    {
                        do
                        {
                            CultureInfo culture = CultureInfo.GetCultureInfo("en-US");
                            string dateheader = start.ToString("d", culture) + " - " + end.ToString("d", culture);
                            string Headers = "";
                            string Totalsbar = "Totals:";
                            string records = "";
                            for (int x = 0; x < 5; x++)
                            {
                                Control[] ct = this.Controls.Find("Column" + (x + 1).ToString(), true);
                                ComboBox cb = ct[0] as ComboBox;
                                if (columns[x] != null)
                                {
                                    if (columns[x].Length > charpercolumn)
                                    {
                                        Headers = Headers + (columns[x].Substring(0, charpercolumn - 1)).PadRight(charpercolumn, ' ');
                                    }
                                    else
                                    {
                                        Headers = Headers + columns[x].PadRight(charpercolumn, ' ');
                                    }
                                }
                                if (cb.SelectedIndex != -1)
                                {
                                    if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                                    {
                                        if (cb.SelectedItem.ToString().Length > charpercolumn)
                                        {
                                            Headers = Headers + (cb.SelectedItem.ToString().Substring(0, charpercolumn - 1)).PadRight(charpercolumn, ' ');
                                        }
                                        else
                                        {
                                            Headers = Headers + cb.SelectedItem.ToString().PadRight(charpercolumn, ' ');
                                        }
                                    }
                                }
                            }
                            for (int x = 1; x <= tcoltotals.Length; x++)
                            {
                                Control[] ct = this.Controls.Find("TColumn" + x.ToString(), true);
                                ComboBox cb = ct[0] as ComboBox;
                                if (cb.SelectedIndex != -1)
                                {
                                    Totalsbar = Totalsbar + (cb.SelectedItem.ToString() + ": " + tcoltotals[x - 1]) + " ";
                                }
                            }

                            for (int x = (pagecount - 1) * rowsperpage; x <= uniquerecords && x < pagecount * rowsperpage; x++)
                            {
                                if (Totals[x, 0] == null) { Totals[x, 0] = ""; }
                                if (Totals[x, 1] == null) { Totals[x, 1] = ""; }
                                if (Totals[x, 2] == null) { Totals[x, 2] = ""; }
                                if (Totals[x, 3] == null) { Totals[x, 3] = ""; }
                                if (Totals[x, 4] == null) { Totals[x, 4] = ""; }

                                if (Totals[x, 0].Length >= charpercolumn)
                                { Totals[x, 0] = Totals[x, 0].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 1].Length >= charpercolumn)
                                { Totals[x, 1] = Totals[x, 1].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 2].Length >= charpercolumn)
                                { Totals[x, 2] = Totals[x, 2].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 3].Length >= charpercolumn)
                                { Totals[x, 3] = Totals[x, 3].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 4].Length >= charpercolumn)
                                { Totals[x, 4] = Totals[x, 4].Substring(0, charpercolumn - 1); }

                                if (Totals[x, 0] != "")
                                { records = records + Totals[x, 0].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 1] != "")
                                { records = records + Totals[x, 1].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 2] != "")
                                { records = records + Totals[x, 2].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 3] != "")
                                { records = records + Totals[x, 3].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 4] != "")
                                { records = records + Totals[x, 4].PadRight(charpercolumn, ' '); }

                                records = records + "\r\n";
                                rows++;
                            }
                            if (rows >= uniquerecords)
                            { done = true; }
                            string labelpath = "ReportPortrait.nlbl";
                            ILabel label = PrintEngineFactory.PrintEngine.OpenLabel(labelpath);
                            label.PrintSettings.PrinterName = PrinterSelect.SelectedItem.ToString();

                            label.Variables["ReportTitle"].SetValue(ReportTitle.Text);
                            label.Variables["Page"].SetValue(pagecount.ToString());
                            label.Variables["Data"].SetValue(records);
                            label.Variables["ColumnHeads"].SetValue(Headers);
                            label.Variables["Totals"].SetValue(Totalsbar);
                            label.Variables["DateRange"].SetValue(dateheader);

                            label.Print(1);

                            pagecount++;
                        } while (!done);
                    }
                }
                else { MessageBox.Show("No data found in selected date range for selected report."); }
            }
            else { MessageBox.Show("Missing configs"); }
        }
        void PortraitPrint()
        {
            string path11 = @"GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            int dbsetting = 0, tbsetting = 0, starttimesetting = 0;
            string timedatename = "";
            int rowsperpage = 61;
            DateTime start = DateRangeSelector.SelectionStart;
            DateTime end = DateRangeSelector.SelectionEnd;
            string[] sumcolumns;
            switch (FPKPKGGEN.SelectedIndex)
            {
                case 0:
                    sumcolumns = new string[7] { "Metal_Present", "Tare_Weight", "Net_Weight", "Check_Weight", "Clips_Present", "Foil_Check", "Filling_To_Long" };
                    timedatename = "Time_Date";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 5;
                            tbsetting = 6;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 13;
                            tbsetting = 14;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 1:
                    sumcolumns = new string[2] { "PALLET_SIZE", "PALLET_WEIGHT" };
                    timedatename = "Date_Time";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 3;
                            tbsetting = 4;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 11;
                            tbsetting = 12;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 2:
                    timedatename = "Date_Time";
                    sumcolumns = new string[1] { "CPM" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 7;
                            tbsetting = 8;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 15;
                            tbsetting = 16;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 3:
                    timedatename = "Time_Date";
                    sumcolumns = new string[5] { "Poly_UseL1", "Poly_UseL2", "Job_Count", "Check_Weigher_Count", "Evo_Print_Count" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 9;
                            tbsetting = 10;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 17;
                            tbsetting = 18;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                default:
                    sumcolumns = new string[1] { "" };
                    dbsetting = 0;
                    tbsetting = 0;
                    break;
            }
            DateTime Start = fixstarttime(start, Int32.Parse(settings[starttimesetting]));
            DateTime End = fixendtime(end, Int32.Parse(settings[starttimesetting]));
            string[] columns = new string[7];
            int ColumnCount = 0, Tcolumncount = 0;
            bool selfsort = true;
            SQLData[] AllRecords = null;
            bool[] Summable = new bool[7];

            for (int x = 1; x < 6; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedIndex != -1)
                {
                    columns[x - 1] = cb.SelectedItem.ToString();
                    ColumnCount++;
                    for (int y = 0; y < sumcolumns.Length; y++)
                    {
                        if (cb.SelectedItem.ToString() == sumcolumns[y])
                        {
                            Summable[x] = true;
                        }
                    }
                }
            }
            if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
            {
                string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString = "SELECT ";
                for (int x = 0; x <= ColumnCount; x++)
                {
                    cmdString = cmdString + columns[x] + ", ";
                }


                if (cmdString[cmdString.Length - 2] == ' ')
                { cmdString = cmdString.Remove(cmdString.Length - 1, 1); }
                if (cmdString[cmdString.Length - 2] == ',')
                { cmdString = cmdString.Remove(cmdString.Length - 2, 1); }

                if (TColumn1.SelectedIndex != -1 && TColumn1.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn1.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn2.SelectedIndex != -1 && TColumn2.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn2.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn3.SelectedIndex != -1 && TColumn3.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn3.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn4.SelectedIndex != -1 && TColumn4.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn4.SelectedItem.ToString() + ", "; Tcolumncount++; }

                if (cmdString[cmdString.Length - 2] == ' ')
                { cmdString = cmdString.Remove(cmdString.Length - 1, 1); }
                if (cmdString[cmdString.Length - 2] == ',')
                { cmdString = cmdString.Remove(cmdString.Length - 2, 1); }

                cmdString = cmdString + " FROM " + settings[tbsetting] + " WHERE " + timedatename + " >= @dtStart AND " + timedatename + " <= @dtEnd";

                if (OnlyPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE = '1'";
                }
                else if (NoPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE != '1'";
                }

                if (OrderBy.SelectedIndex != -1 && AscorDesc.SelectedIndex != -1 && !selfsort)
                {
                    cmdString = cmdString + " ORDER BY " + OrderBy.SelectedItem + " " + AscorDesc.SelectedItem + ";";
                }
                else { cmdString = cmdString + ";"; }

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
                            var list = new List<SQLData>();
                            while (reader.Read())
                            {
                                list.Add(new SQLData());
                                int tcolumnstart = ColumnCount;
                                if (ColumnCount > 0)
                                { try { list[list.Count - 1].Col1 = reader.GetString(0); } catch (Exception t) { }; }
                                if (ColumnCount > 1)
                                { try { list[list.Count - 1].Col2 = reader.GetString(1); } catch (Exception t) { }; }
                                if (ColumnCount > 2)
                                { try { list[list.Count - 1].Col3 = reader.GetString(2); } catch (Exception t) { }; }
                                if (ColumnCount > 3)
                                { try { list[list.Count - 1].Col4 = reader.GetString(3); } catch (Exception t) { }; }
                                if (ColumnCount > 4)
                                { try { list[list.Count - 1].Col5 = reader.GetString(4); } catch (Exception t) { }; }

                                if (Tcolumncount > 0)
                                { try { list[list.Count - 1].TCol1 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 1)
                                { try { list[list.Count - 1].TCol2 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 2)
                                { try { list[list.Count - 1].TCol3 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 3)
                                { try { list[list.Count - 1].TCol4 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }

                                AllRecords = list.ToArray();
                            }
                        }
                    }
                }
                if (AllRecords != null)
                {
                    int RecordCount = 0;
                    string[,] Totals = new string[AllRecords.Length, 5];
                    int[] tcoltotals = new int[4] { 0, 0, 0, 0 };
                    for (int x = 0; x < AllRecords.Length; x++)
                    {
                        Totals[x, 0] = AllRecords[x].Col1;
                        Totals[x, 1] = AllRecords[x].Col2;
                        Totals[x, 2] = AllRecords[x].Col3;
                        Totals[x, 3] = AllRecords[x].Col4;
                        Totals[x, 4] = AllRecords[x].Col5;

                        RecordCount++;

                        int sqlselector = 0;
                        for (int f = 0; f < 4; f++)
                        {
                            Control[] ct = this.Controls.Find("TColumn" + (f + 1).ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (cb.SelectedIndex != -1)
                            {
                                if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                                { tcoltotals[f]++; }
                                else
                                {
                                    if (sqlselector == 0)
                                    { if (AllRecords[x].TCol1 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol1); } }
                                    if (sqlselector == 1)
                                    { if (AllRecords[x].TCol2 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol2); } }
                                    if (sqlselector == 2)
                                    { if (AllRecords[x].TCol3 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol3); } }
                                    if (sqlselector == 3)
                                    { if (AllRecords[x].TCol4 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol4); } }

                                    sqlselector++;
                                }
                            }
                        }
                    }
                    #region sorting
                    int sortby = findsortby();
                    bool sortbyisSummable = false;
                    Control[] cts = this.Controls.Find("Column" + (sortby + 1).ToString(), true);
                    ComboBox cbs = cts[0] as ComboBox;
                    if (Summable[sortby] || cbs.SelectedItem.ToString() == "Pallet Count" || cbs.SelectedItem.ToString() == "Record Count")
                    { sortbyisSummable = true; }
                    if (AscorDesc.SelectedIndex == 0)
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length - 1; y++)
                            {
                                string[] holding = new string[5];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) < 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) < Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                { break; }
                            }
                        }
                    }
                    else
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length - 1; y++)
                            {
                                string[] holding = new string[5];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) > 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) > Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                { break; }
                            }
                        }
                    }
                    #endregion
                    int pagecount = 1;
                    bool done = false;
                    int rows = 0;
                    int charpercolumn = 80 / ColumnCount;
                    int expectedpages = RecordCount / rowsperpage;
                    bool printOK = true;
                    if (expectedpages > 10)
                    {
                        string pagewarning = "Are you sure you want to print " + expectedpages.ToString() + " pages?";
                        AreYouSureYouWantToPrint aysywtp = new AreYouSureYouWantToPrint(pagewarning);
                        var result = aysywtp.ShowDialog();
                        if (result == DialogResult.Yes)
                        {
                            printOK = true;
                        }
                        else { printOK = false; }
                    }
                    if (printOK)
                    {
                        do
                        {
                            CultureInfo culture = CultureInfo.GetCultureInfo("en-US");
                            string dateheader = start.ToString("d", culture) + " - " + end.ToString("d", culture);
                            string Headers = "";
                            string Totalsbar = "Totals:";
                            string records = "";

                            for (int x = 0; x < 5; x++)
                            {
                                Control[] ct = this.Controls.Find("Column" + (x + 1).ToString(), true);
                                ComboBox cb = ct[0] as ComboBox;
                                if (columns[x] != null)
                                {
                                    if (columns[x].Length > charpercolumn)
                                    {
                                        Headers = Headers + (columns[x].Substring(0, charpercolumn - 1)).PadRight(charpercolumn, ' ');
                                    }
                                    else
                                    {
                                        Headers = Headers + columns[x].PadRight(charpercolumn, ' ');
                                    }
                                }
                                if (cb.SelectedIndex != -1)
                                {
                                    if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                                    {
                                        if (cb.SelectedItem.ToString().Length > charpercolumn)
                                        {
                                            Headers = Headers + (cb.SelectedItem.ToString().Substring(0, charpercolumn - 1)).PadRight(charpercolumn, ' ');
                                        }
                                        else
                                        {
                                            Headers = Headers + cb.SelectedItem.ToString().PadRight(charpercolumn, ' ');
                                        }
                                    }
                                }
                            }

                            for (int x = 1; x <= tcoltotals.Length; x++)
                            {
                                Control[] ct = this.Controls.Find("TColumn" + x.ToString(), true);
                                ComboBox cb = ct[0] as ComboBox;
                                if (cb.SelectedIndex != -1)
                                {
                                    Totalsbar = Totalsbar + (cb.SelectedItem.ToString() + ": " + tcoltotals[x - 1]) + " ";
                                }
                            }

                            for (int x = (pagecount - 1) * rowsperpage; x <= RecordCount && x < pagecount * rowsperpage; x++)
                            {
                                if (Totals[x, 0] == null) { Totals[x, 0] = ""; }
                                if (Totals[x, 1] == null) { Totals[x, 1] = ""; }
                                if (Totals[x, 2] == null) { Totals[x, 2] = ""; }
                                if (Totals[x, 3] == null) { Totals[x, 3] = ""; }
                                if (Totals[x, 4] == null) { Totals[x, 4] = ""; }

                                if (Totals[x, 0].Length >= charpercolumn)
                                { Totals[x, 0] = Totals[x, 0].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 1].Length >= charpercolumn)
                                { Totals[x, 1] = Totals[x, 1].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 2].Length >= charpercolumn)
                                { Totals[x, 2] = Totals[x, 2].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 3].Length >= charpercolumn)
                                { Totals[x, 3] = Totals[x, 3].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 4].Length >= charpercolumn)
                                { Totals[x, 4] = Totals[x, 4].Substring(0, charpercolumn - 1); }

                                if (Totals[x, 0] != "")
                                { records = records + Totals[x, 0].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 1] != "")
                                { records = records + Totals[x, 1].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 2] != "")
                                { records = records + Totals[x, 2].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 3] != "")
                                { records = records + Totals[x, 3].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 4] != "")
                                { records = records + Totals[x, 4].PadRight(charpercolumn, ' '); }

                                records = records + "\r\n";
                                rows++;
                            }
                            if (rows >= RecordCount)
                            { done = true; }
                            string labelpath = "ReportPortrait.nlbl";
                            ILabel label = PrintEngineFactory.PrintEngine.OpenLabel(labelpath);
                            label.PrintSettings.PrinterName = PrinterSelect.SelectedItem.ToString();

                            label.Variables["ReportTitle"].SetValue(ReportTitle.Text);
                            label.Variables["Page"].SetValue(pagecount.ToString());
                            label.Variables["Data"].SetValue(records);
                            label.Variables["ColumnHeads"].SetValue(Headers);
                            label.Variables["Totals"].SetValue(Totalsbar);
                            label.Variables["DateRange"].SetValue(dateheader);

                            label.Print(1);

                            pagecount++;
                        }
                        while (!done);
                    }
                }
            }
        }
        void LandscapeSumedPrint()
        {
            string path11 = @"GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            int dbsetting = 0, tbsetting = 0, starttimesetting = 0;
            string[] sumcolumns;
            string timedatename = "";
            bool selfsort = false;
            int rowsperpage = 42;
            DateTime start = DateRangeSelector.SelectionStart;
            DateTime end = DateRangeSelector.SelectionEnd;
            SQLData[] AllRecords = null;

            switch (FPKPKGGEN.SelectedIndex)
            {
                case 0:
                    sumcolumns = new string[7] { "Metal_Present", "Tare_Weight", "Net_Weight", "Check_Weight", "Clips_Present", "Foil_Check", "Filling_To_Long" };
                    timedatename = "Time_Date";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 5;
                            tbsetting = 6;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 13;
                            tbsetting = 14;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 1:
                    sumcolumns = new string[2] { "PALLET_SIZE", "PALLET_WEIGHT" };
                    timedatename = "Date_Time";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 3;
                            tbsetting = 4;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 11;
                            tbsetting = 12;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 2:
                    timedatename = "Date_Time";
                    sumcolumns = new string[1] { "CPM" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 7;
                            tbsetting = 8;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 15;
                            tbsetting = 16;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 3:
                    timedatename = "Time_Date";
                    sumcolumns = new string[5] { "Poly_UseL1", "Poly_UseL2", "Job_Count", "Check_Weigher_Count", "Evo_Print_Count" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 9;
                            tbsetting = 10;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 17;
                            tbsetting = 18;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                default:
                    sumcolumns = new string[1] { "" };
                    dbsetting = 0;
                    tbsetting = 0;
                    break;
            }
            DateTime Start = fixstarttime(start, Int32.Parse(settings[starttimesetting]));
            DateTime End = fixendtime(end, Int32.Parse(settings[starttimesetting]));
            for (int x = 0; x < sumcolumns.Length; x++)
            {
                if (SumColumnsBy.SelectedIndex != -1 && OrderBy.SelectedItem.ToString() != sumcolumns[x])
                {
                    selfsort = false;
                }
                else { selfsort = true; break; }
            }
            string[] columns = new string[8];
            int sumbycolumn = 0;
            for (int x = 1; x < 8; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedItem.ToString() == SumColumnsBy.SelectedItem.ToString())
                {
                    columns[0] = cb.SelectedItem.ToString();
                    sumbycolumn = x;
                    break;
                }
            }
            int columncount = 1, Tcolumncount = 0;
            int skipped = 0;
            for (int x = 1; x < 8; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedIndex == -1 || x == sumbycolumn || cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                { skipped++; }
                else { columns[x - skipped] = cb.SelectedItem.ToString(); columncount++; }
            }

            bool[] Summable = new bool[7];
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < sumcolumns.Length; y++)
                {
                    if (columns[x] == sumcolumns[y]) { Summable[x] = true; break; }
                    else { Summable[x] = false; }
                }
            }
            if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
            {
                string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString = "SELECT ";
                //Change this creation of cmd string to be in a loop to iterate the number of columns
                //and then patch together the string one column at a time checking if it has a -1 selected index each time  or pallet count
                //this should allow to also check other things like for Pallet count and pivot later in the function to address that issue
                for (int x = 0; x <= columncount; x++)
                {
                    cmdString = cmdString + columns[x] + ", ";
                }
                if (cmdString[cmdString.Length - 2] == ' ')
                { cmdString = cmdString.Remove(cmdString.Length - 1, 1); }
                if (cmdString[cmdString.Length - 2] == ',')
                { cmdString = cmdString.Remove(cmdString.Length - 2, 1); }
                if (TColumn1.SelectedIndex != -1 && TColumn1.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn1.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn2.SelectedIndex != -1 && TColumn2.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn2.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn3.SelectedIndex != -1 && TColumn3.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn3.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn4.SelectedIndex != -1 && TColumn4.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn4.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn5.SelectedIndex != -1 && TColumn5.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn5.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn6.SelectedIndex != -1 && TColumn6.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn6.SelectedItem.ToString(); Tcolumncount++; }
                if (cmdString[cmdString.Length - 2] == ' ')
                { cmdString = cmdString.Remove(cmdString.Length - 1, 1); }
                if (cmdString[cmdString.Length - 2] == ',')
                { cmdString = cmdString.Remove(cmdString.Length - 2, 1); }
                cmdString = cmdString + " FROM " + settings[tbsetting] + " WHERE " + timedatename + " >= @dtStart AND " + timedatename + " <= @dtEnd";
                if (OnlyPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE = '1'";
                }
                else if (NoPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE != '1'";
                }
                if (OrderBy.SelectedIndex != -1 && AscorDesc.SelectedIndex != -1 && selfsort)
                {
                    cmdString = cmdString + " ORDER BY " + OrderBy.SelectedItem + " " + AscorDesc.SelectedItem + ";";
                }
                else { cmdString = cmdString + ";"; }

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
                            var list = new List<SQLData>();
                            while (reader.Read())
                            {
                                //list.Add(new SQLData { Col1 = reader.GetString(0), Col2 = reader.GetString(1), Col3 = reader.GetString(2), Col4 = reader.GetString(3), Col5 = reader.GetString(4), Col6 = reader.GetString(5), Col7 = reader.GetString(6), TCol1 = reader.GetString(7), TCol2 = reader.GetString(8), TCol3 = reader.GetString(9), TCol4 = reader.GetString(10), TCol5 = reader.GetString(11), TCol6 = reader.GetString(12) });
                                list.Add(new SQLData());
                                int tcolumnstart = columncount;
                                if (columncount > 0)
                                { try { list[list.Count - 1].Col1 = reader.GetString(0); } catch (Exception t) { }; }
                                if (columncount > 1)
                                { try { list[list.Count - 1].Col2 = reader.GetString(1); } catch (Exception t) { }; }
                                if (columncount > 2)
                                { try { list[list.Count - 1].Col3 = reader.GetString(2); } catch (Exception t) { }; }
                                if (columncount > 3)
                                { try { list[list.Count - 1].Col4 = reader.GetString(3); } catch (Exception t) { }; }
                                if (columncount > 4)
                                { try { list[list.Count - 1].Col5 = reader.GetString(4); } catch (Exception t) { }; }
                                if (columncount > 5)
                                { try { list[list.Count - 1].Col6 = reader.GetString(5); } catch (Exception t) { }; }
                                if (columncount > 6)
                                { try { list[list.Count - 1].Col7 = reader.GetString(6); } catch (Exception t) { }; }


                                if (Tcolumncount > 0)
                                { try { list[list.Count - 1].TCol1 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 1)
                                { try { list[list.Count - 1].TCol2 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 2)
                                { try { list[list.Count - 1].TCol3 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 3)
                                { try { list[list.Count - 1].TCol4 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 4)
                                { try { list[list.Count - 1].TCol5 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 5)
                                { try { list[list.Count - 1].TCol6 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                AllRecords = list.ToArray();
                            }
                        }
                    }
                }

                if (AllRecords != null)
                {
                    int RecordCount = 0;
                    string[,] Totals = new string[AllRecords.Length, 8];
                    int uniquerecords = 0;
                    int[] tcoltotals = new int[6] { 0, 0, 0, 0, 0, 0 };
                    for (int x = 0; x < AllRecords.Length; x++)
                    {
                        RecordCount++;
                        bool found = false;
                        for (int y = 0; y < AllRecords.Length; y++)
                        {
                            if (AllRecords[x].Col1 == Totals[y, 0])
                            {
                                if (Summable[1]) { Totals[y, 1] = (Int32.Parse(Totals[y, 1]) + Int32.Parse(AllRecords[x].Col2)).ToString(); }
                                if (Summable[2]) { Totals[y, 2] = (Int32.Parse(Totals[y, 2]) + Int32.Parse(AllRecords[x].Col3)).ToString(); }
                                if (Summable[3]) { Totals[y, 3] = (Int32.Parse(Totals[y, 3]) + Int32.Parse(AllRecords[x].Col4)).ToString(); }
                                if (Summable[4]) { Totals[y, 4] = (Int32.Parse(Totals[y, 4]) + Int32.Parse(AllRecords[x].Col5)).ToString(); }
                                if (Summable[5]) { Totals[y, 5] = (Int32.Parse(Totals[y, 5]) + Int32.Parse(AllRecords[x].Col6)).ToString(); }
                                if (Summable[6]) { Totals[y, 6] = (Int32.Parse(Totals[y, 6]) + Int32.Parse(AllRecords[x].Col7)).ToString(); }
                                if (Column1.SelectedIndex != -1)
                                {
                                    if (Column1.SelectedItem.ToString() == "Pallet Count" || Column1.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 0] = (Int32.Parse(Totals[y, 0]) + 1).ToString(); }
                                }
                                if (Column2.SelectedIndex != -1)
                                {
                                    if (Column2.SelectedItem.ToString() == "Pallet Count" || Column2.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 1] = (Int32.Parse(Totals[y, 1]) + 1).ToString(); }
                                }
                                if (Column3.SelectedIndex != -1)
                                {
                                    if (Column3.SelectedItem.ToString() == "Pallet Count" || Column3.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 2] = (Int32.Parse(Totals[y, 2]) + 1).ToString(); }
                                }
                                if (Column4.SelectedIndex != -1)
                                {
                                    if (Column4.SelectedItem.ToString() == "Pallet Count" || Column4.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 3] = (Int32.Parse(Totals[y, 3]) + 1).ToString(); }
                                }
                                if (Column5.SelectedIndex != -1)
                                {
                                    if (Column5.SelectedItem.ToString() == "Pallet Count" || Column5.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 4] = (Int32.Parse(Totals[y, 4]) + 1).ToString(); }
                                }
                                if (Column6.SelectedIndex != -1)
                                {
                                    if (Column6.SelectedItem.ToString() == "Pallet Count" || Column6.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 5] = (Int32.Parse(Totals[y, 5]) + 1).ToString(); }
                                }
                                if (Column7.SelectedIndex != -1)
                                {
                                    if (Column7.SelectedItem.ToString() == "Pallet Count" || Column7.SelectedItem.ToString() == "Record Count")
                                    { Totals[y, 6] = (Int32.Parse(Totals[y, 6]) + 1).ToString(); }
                                }
                                Totals[y, 7] = (Int32.Parse(Totals[y, 7]) + 1).ToString();
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            for (int y = 0; y < AllRecords.Length; y++)
                            {
                                if (Totals[y, 0] == null || Totals[y, 0] == string.Empty || Totals[y, 0] == "")
                                {
                                    Totals[y, 0] = AllRecords[x].Col1;
                                    Totals[y, 1] = AllRecords[x].Col2;
                                    Totals[y, 2] = AllRecords[x].Col3;
                                    Totals[y, 3] = AllRecords[x].Col4;
                                    Totals[y, 4] = AllRecords[x].Col5;
                                    Totals[y, 5] = AllRecords[x].Col6;
                                    Totals[y, 6] = AllRecords[x].Col7;
                                    Totals[y, 7] = "1";
                                    if (Column1.SelectedIndex != -1)
                                    {
                                        if (Column1.SelectedItem.ToString() == "Pallet Count" || Column1.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 0] = "1"; }
                                    }
                                    if (Column2.SelectedIndex != -1)
                                    {
                                        if (Column2.SelectedItem.ToString() == "Pallet Count" || Column2.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 1] = "1"; }
                                    }
                                    if (Column3.SelectedIndex != -1)
                                    {
                                        if (Column3.SelectedItem.ToString() == "Pallet Count" || Column3.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 2] = "1"; }
                                    }
                                    if (Column4.SelectedIndex != -1)
                                    {
                                        if (Column4.SelectedItem.ToString() == "Pallet Count" || Column4.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 3] = "1"; }
                                    }
                                    if (Column5.SelectedIndex != -1)
                                    {
                                        if (Column5.SelectedItem.ToString() == "Pallet Count" || Column5.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 4] = "1"; }
                                    }
                                    if (Column6.SelectedIndex != -1)
                                    {
                                        if (Column6.SelectedItem.ToString() == "Pallet Count" || Column6.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 5] = "1"; }
                                    }
                                    if (Column7.SelectedIndex != -1)
                                    {
                                        if (Column7.SelectedItem.ToString() == "Pallet Count" || Column7.SelectedItem.ToString() == "Record Count")
                                        { Totals[y, 6] = "1"; }
                                    }
                                    uniquerecords++;
                                    break;
                                }
                            }
                        }
                        int sqlselector = 0;
                        for (int f = 0; f < 6; f++)
                        {

                            Control[] ct = this.Controls.Find("TColumn" + (f + 1).ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (cb.SelectedIndex != -1)
                            {
                                if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                                { tcoltotals[f]++; }
                                else
                                {
                                    if (sqlselector == 0)
                                    { if (AllRecords[x].TCol1 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol1); } }
                                    if (sqlselector == 1)
                                    { if (AllRecords[x].TCol2 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol2); } }
                                    if (sqlselector == 2)
                                    { if (AllRecords[x].TCol3 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol3); } }
                                    if (sqlselector == 3)
                                    { if (AllRecords[x].TCol4 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol4); } }
                                    if (sqlselector == 4)
                                    { if (AllRecords[x].TCol5 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol5); } }
                                    if (sqlselector == 5)
                                    { if (AllRecords[x].TCol6 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol6); } }
                                    sqlselector++;
                                }
                            }
                        }
                    }
                    int pagecount = 1;
                    bool done = false;
                    int rows = 0;
                    for (int x = 1; x < 8; x++)
                    {
                        Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                        ComboBox cb = ct[0] as ComboBox;
                        if (cb.SelectedIndex != -1)
                        {
                            if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                            { columncount++; }
                        }
                    }
                    int charpercolumn = 105 / columncount;
                    int expectedpages = RecordCount / rowsperpage;
                    bool printOK = true;
                    if (expectedpages > 10)
                    {
                        string pagewarning = "Are you sure you want to print " + expectedpages.ToString() + " pages?";
                        AreYouSureYouWantToPrint aysywtp = new AreYouSureYouWantToPrint(pagewarning);
                        var result = aysywtp.ShowDialog();
                        if (result == DialogResult.Yes)
                        {
                            printOK = true;
                        }
                        else { printOK = false; }
                    }
                    if (printOK)
                    {
                        do
                        {
                            CultureInfo culture = CultureInfo.GetCultureInfo("en-US");
                            string dateheader = start.ToString("d", culture) + " - " + end.ToString("d", culture);
                            string Headers = "";
                            string Totalsbar = "Totals:";
                            string records = "";
                            for (int x = 0; x < 7; x++)
                            {
                                Control[] ct = this.Controls.Find("Column" + (x + 1).ToString(), true);
                                ComboBox cb = ct[0] as ComboBox;
                                if (columns[x] != null)
                                {
                                    Headers = Headers + columns[x].PadRight(charpercolumn, ' ');
                                }
                                if (cb.SelectedIndex != -1)
                                {
                                    if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                                    {
                                        Headers = Headers + cb.SelectedItem.ToString().PadRight(charpercolumn, ' ');
                                    }
                                }
                            }
                            for (int x = 1; x <= tcoltotals.Length; x++)
                            {
                                Control[] ct = this.Controls.Find("TColumn" + x.ToString(), true);
                                ComboBox cb = ct[0] as ComboBox;
                                if (cb.SelectedIndex != -1)
                                {
                                    Totalsbar = Totalsbar + (cb.SelectedItem.ToString() + ": " + tcoltotals[x - 1]) + " ";
                                }
                            }

                            for (int x = (pagecount - 1) * rowsperpage; x <= uniquerecords && x < pagecount * rowsperpage; x++)
                            {
                                if (Totals[x, 0] == null) { Totals[x, 0] = ""; }
                                if (Totals[x, 1] == null) { Totals[x, 1] = ""; }
                                if (Totals[x, 2] == null) { Totals[x, 2] = ""; }
                                if (Totals[x, 3] == null) { Totals[x, 3] = ""; }
                                if (Totals[x, 4] == null) { Totals[x, 4] = ""; }
                                if (Totals[x, 5] == null) { Totals[x, 5] = ""; }
                                if (Totals[x, 6] == null) { Totals[x, 6] = ""; }
                                if (Totals[x, 0].Length >= charpercolumn)
                                { Totals[x, 0] = Totals[x, 0].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 1].Length >= charpercolumn)
                                { Totals[x, 1] = Totals[x, 1].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 2].Length >= charpercolumn)
                                { Totals[x, 2] = Totals[x, 2].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 3].Length >= charpercolumn)
                                { Totals[x, 3] = Totals[x, 3].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 4].Length >= charpercolumn)
                                { Totals[x, 4] = Totals[x, 4].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 5].Length >= charpercolumn)
                                { Totals[x, 5] = Totals[x, 5].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 6].Length >= charpercolumn)
                                { Totals[x, 6] = Totals[x, 6].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 0] != "")
                                { records = records + Totals[x, 0].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 1] != "")
                                { records = records + Totals[x, 1].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 2] != "")
                                { records = records + Totals[x, 2].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 3] != "")
                                { records = records + Totals[x, 3].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 4] != "")
                                { records = records + Totals[x, 4].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 5] != "")
                                { records = records + Totals[x, 5].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 6] != "")
                                { records = records + Totals[x, 6].PadRight(charpercolumn, ' '); }
                                records = records + "\r\n";
                                rows++;
                            }
                            if (rows >= uniquerecords)
                            { done = true; }
                            string labelpath = "ReportLandscape.nlbl";
                            ILabel label = PrintEngineFactory.PrintEngine.OpenLabel(labelpath);
                            label.PrintSettings.PrinterName = PrinterSelect.SelectedItem.ToString();

                            label.Variables["ReportTitle"].SetValue(ReportTitle.Text);
                            label.Variables["Page"].SetValue(pagecount.ToString());
                            label.Variables["Data"].SetValue(records);
                            label.Variables["ColumnHeads"].SetValue(Headers);
                            label.Variables["Totals"].SetValue(Totalsbar);
                            label.Variables["DateRange"].SetValue(dateheader);

                            label.Print(1);

                            pagecount++;
                        } while (!done);
                    }
                }
                else { MessageBox.Show("No data found in selected date range for selected report."); }
            }
            else { MessageBox.Show("Missing configs"); }
        }
        void LandscapePrint()
        {
            string path11 = @"GeneralSettings.txt";
            path11 = path11.Replace("\r", "").Replace("\n", "");
            string[] settings = File.ReadAllText(path11).Split(',');
            int dbsetting = 0, tbsetting = 0, starttimesetting = 0;
            string timedatename = "";
            int rowsperpage = 42;
            DateTime start = DateRangeSelector.SelectionStart;
            DateTime end = DateRangeSelector.SelectionEnd;
            string[] sumcolumns;
            switch (FPKPKGGEN.SelectedIndex)
            {
                case 0:
                    sumcolumns = new string[7] { "Metal_Present", "Tare_Weight", "Net_Weight", "Check_Weight", "Clips_Present", "Foil_Check", "Filling_To_Long" };
                    timedatename = "Time_Date";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 5;
                            tbsetting = 6;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 13;
                            tbsetting = 14;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 1:
                    sumcolumns = new string[2] { "PALLET_SIZE", "PALLET_WEIGHT" };
                    timedatename = "Date_Time";
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 3;
                            tbsetting = 4;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 11;
                            tbsetting = 12;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 2:
                    timedatename = "Date_Time";
                    sumcolumns = new string[1] { "CPM" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 7;
                            tbsetting = 8;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 15;
                            tbsetting = 16;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                case 3:
                    timedatename = "Time_Date";
                    sumcolumns = new string[5] { "Poly_UseL1", "Poly_UseL2", "Job_Count", "Check_Weigher_Count", "Evo_Print_Count" };
                    switch (LocationSelection.SelectedIndex)
                    {
                        case 0:
                            dbsetting = 9;
                            tbsetting = 10;
                            starttimesetting = 19;
                            break;
                        case 1:
                            dbsetting = 17;
                            tbsetting = 18;
                            starttimesetting = 20;
                            break;
                    }
                    break;
                default:
                    sumcolumns = new string[1] { "" };
                    dbsetting = 0;
                    tbsetting = 0;
                    break;
            }
            DateTime Start = fixstarttime(start, Int32.Parse(settings[starttimesetting]));
            DateTime End = fixendtime(end, Int32.Parse(settings[starttimesetting]));
            string[] columns = new string[7];
            int ColumnCount = 0, Tcolumncount = 0;
            bool selfsort = true;
            SQLData[] AllRecords = null;
            bool[] Summable = new bool[7];

            for (int x = 1; x < 8; x++)
            {
                Control[] ct = this.Controls.Find("Column" + x.ToString(), true);
                ComboBox cb = ct[0] as ComboBox;
                if (cb.SelectedIndex != -1)
                {
                    columns[x - 1] = cb.SelectedItem.ToString();
                    ColumnCount++;
                    for (int y = 0; y < sumcolumns.Length; y++)
                    {
                        if (cb.SelectedItem.ToString() == sumcolumns[y])
                        {
                            Summable[x] = true;
                        }
                    }
                }
            }
            if (settings[0] != null && settings[0] != "" && settings[0] != string.Empty && settings[1] != null && settings[1] != "" && settings[1] != string.Empty && settings[2] != null && settings[2] != "" && settings[2] != string.Empty && settings[dbsetting] != null && settings[dbsetting] != "" && settings[dbsetting] != string.Empty && settings[tbsetting] != null && settings[tbsetting] != "" && settings[tbsetting] != string.Empty)
            {
                string connString = "Data Source=" + settings[0] + ";Initial Catalog=" + settings[dbsetting] + ";User ID=" + settings[1] + ";Password=" + settings[2] + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                string cmdString = "SELECT ";
                for (int x = 0; x <= ColumnCount; x++)
                {
                    cmdString = cmdString + columns[x] + ", ";
                }


                if (cmdString[cmdString.Length - 2] == ' ')
                { cmdString = cmdString.Remove(cmdString.Length - 1, 1); }
                if (cmdString[cmdString.Length - 2] == ',')
                { cmdString = cmdString.Remove(cmdString.Length - 2, 1); }

                if (TColumn1.SelectedIndex != -1 && TColumn1.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn1.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn2.SelectedIndex != -1 && TColumn2.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn2.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn3.SelectedIndex != -1 && TColumn3.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn3.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn4.SelectedIndex != -1 && TColumn4.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn4.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn5.SelectedIndex != -1 && TColumn5.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn5.SelectedItem.ToString() + ", "; Tcolumncount++; }
                if (TColumn6.SelectedIndex != -1 && TColumn6.SelectedItem.ToString() != "Pallet Count" && TColumn1.SelectedItem.ToString() != "Record Count")
                { cmdString = cmdString + TColumn6.SelectedItem.ToString(); Tcolumncount++; }

                if (cmdString[cmdString.Length - 2] == ' ')
                { cmdString = cmdString.Remove(cmdString.Length - 1, 1); }
                if (cmdString[cmdString.Length - 2] == ',')
                { cmdString = cmdString.Remove(cmdString.Length - 2, 1); }

                cmdString = cmdString + " FROM " + settings[tbsetting] + " WHERE " + timedatename + " >= @dtStart AND " + timedatename + " <= @dtEnd";

                if (OnlyPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE = '1'";
                }
                else if (NoPalletSize1.Checked == true)
                {
                    cmdString = cmdString + " AND PALLET_SIZE != '1'";
                }

                if (OrderBy.SelectedIndex != -1 && AscorDesc.SelectedIndex != -1 && !selfsort)
                {
                    cmdString = cmdString + " ORDER BY " + OrderBy.SelectedItem + " " + AscorDesc.SelectedItem + ";";
                }
                else { cmdString = cmdString + ";"; }

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
                            var list = new List<SQLData>();
                            while (reader.Read())
                            {
                                list.Add(new SQLData());
                                int tcolumnstart = ColumnCount;
                                if (ColumnCount > 0)
                                { try { list[list.Count - 1].Col1 = reader.GetString(0); } catch (Exception t) { }; }
                                if (ColumnCount > 1)
                                { try { list[list.Count - 1].Col2 = reader.GetString(1); } catch (Exception t) { }; }
                                if (ColumnCount > 2)
                                { try { list[list.Count - 1].Col3 = reader.GetString(2); } catch (Exception t) { }; }
                                if (ColumnCount > 3)
                                { try { list[list.Count - 1].Col4 = reader.GetString(3); } catch (Exception t) { }; }
                                if (ColumnCount > 4)
                                { try { list[list.Count - 1].Col5 = reader.GetString(4); } catch (Exception t) { }; }
                                if (ColumnCount > 5)
                                { try { list[list.Count - 1].Col6 = reader.GetString(5); } catch (Exception t) { }; }
                                if (ColumnCount > 6)
                                { try { list[list.Count - 1].Col7 = reader.GetString(6); } catch (Exception t) { }; }


                                if (Tcolumncount > 0)
                                { try { list[list.Count - 1].TCol1 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 1)
                                { try { list[list.Count - 1].TCol2 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 2)
                                { try { list[list.Count - 1].TCol3 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 3)
                                { try { list[list.Count - 1].TCol4 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 4)
                                { try { list[list.Count - 1].TCol5 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                if (Tcolumncount > 5)
                                { try { list[list.Count - 1].TCol6 = reader.GetString(tcolumnstart); tcolumnstart++; } catch (Exception t) { }; }
                                AllRecords = list.ToArray();
                            }
                        }
                    }
                }
                if (AllRecords != null)
                {
                    int RecordCount = 0;
                    string[,] Totals = new string[AllRecords.Length, 7];
                    int[] tcoltotals = new int[6] { 0, 0, 0, 0, 0, 0 };
                    for (int x = 0; x < AllRecords.Length; x++)
                    {
                        Totals[x, 0] = AllRecords[x].Col1;
                        Totals[x, 1] = AllRecords[x].Col2;
                        Totals[x, 2] = AllRecords[x].Col3;
                        Totals[x, 3] = AllRecords[x].Col4;
                        Totals[x, 4] = AllRecords[x].Col5;
                        Totals[x, 5] = AllRecords[x].Col6;
                        Totals[x, 6] = AllRecords[x].Col7;

                        RecordCount++;

                        int sqlselector = 0;
                        for (int f = 0; f < 6; f++)
                        {
                            Control[] ct = this.Controls.Find("TColumn" + (f + 1).ToString(), true);
                            ComboBox cb = ct[0] as ComboBox;
                            if (cb.SelectedIndex != -1)
                            {
                                if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                                { tcoltotals[f]++; }
                                else
                                {
                                    if (sqlselector == 0)
                                    { if (AllRecords[x].TCol1 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol1); } }
                                    if (sqlselector == 1)
                                    { if (AllRecords[x].TCol2 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol2); } }
                                    if (sqlselector == 2)
                                    { if (AllRecords[x].TCol3 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol3); } }
                                    if (sqlselector == 3)
                                    { if (AllRecords[x].TCol4 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol4); } }
                                    if (sqlselector == 4)
                                    { if (AllRecords[x].TCol5 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol5); } }
                                    if (sqlselector == 5)
                                    { if (AllRecords[x].TCol6 != null) { tcoltotals[f] = tcoltotals[f] + Int32.Parse(AllRecords[x].TCol6); } }
                                    sqlselector++;
                                }
                            }
                        }
                    }
                    #region sorting
                    int sortby = findsortby();
                    bool sortbyisSummable = false;
                    Control[] cts = this.Controls.Find("Column" + (sortby + 1).ToString(), true);
                    ComboBox cbs = cts[0] as ComboBox;
                    if (Summable[sortby] || cbs.SelectedItem.ToString() == "Pallet Count" || cbs.SelectedItem.ToString() == "Record Count")
                    { sortbyisSummable = true; }
                    if (AscorDesc.SelectedIndex == 0)
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length - 1; y++)
                            {
                                string[] holding = new string[7];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) < 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) < Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                { break; }
                            }
                        }
                    }
                    else
                    {
                        for (int x = 0; x < AllRecords.Length; x++)
                        {
                            for (int y = 0; y < AllRecords.Length - 1; y++)
                            {
                                string[] holding = new string[7];
                                if (Totals[y, sortby] != null && Totals[y + 1, sortby] != null)
                                {
                                    if (!sortbyisSummable)
                                    {
                                        if (Totals[y, sortby].CompareTo(Totals[y + 1, sortby]) > 0)
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Int32.Parse(Totals[y, sortby]) > Int32.Parse(Totals[y + 1, sortby]))
                                        {
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                holding[f] = Totals[y + 1, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y + 1, f] = Totals[y, f];
                                            }
                                            for (int f = 0; f < holding.Length; f++)
                                            {
                                                Totals[y, f] = holding[f];
                                            }
                                        }
                                    }
                                }
                                else if (Totals[y, 0] == null && Totals[y, 1] == null && Totals[y, 2] == null && Totals[y, 3] == null && Totals[y, 4] == null && Totals[y, 5] == null && Totals[y, 6] == null && Totals[y, 7] == null)
                                { break; }
                            }
                        }
                    }
                    #endregion
                    int pagecount = 1;
                    bool done = false;
                    int rows = 0;
                    int charpercolumn = 105 / ColumnCount;
                    int expectedpages = RecordCount / rowsperpage;
                    bool printOK = true;
                    if (expectedpages > 10)
                    {
                        string pagewarning = "Are you sure you want to print " + expectedpages.ToString() + " pages?";
                        AreYouSureYouWantToPrint aysywtp = new AreYouSureYouWantToPrint(pagewarning);
                        var result = aysywtp.ShowDialog();
                        if (result == DialogResult.Yes)
                        {
                            printOK = true;
                        }
                        else { printOK = false; }
                    }
                    if (printOK)
                    {
                        do
                        {
                            CultureInfo culture = CultureInfo.GetCultureInfo("en-US");
                            string dateheader = start.ToString("d", culture) + " - " + end.ToString("d", culture);
                            string Headers = "";
                            string Totalsbar = "Totals:";
                            string records = "";

                            for (int x = 0; x < 7; x++)
                            {
                                Control[] ct = this.Controls.Find("Column" + (x + 1).ToString(), true);
                                ComboBox cb = ct[0] as ComboBox;
                                if (columns[x] != null)
                                {
                                    Headers = Headers + columns[x].PadRight(charpercolumn, ' ');
                                }
                                if (cb.SelectedIndex != -1)
                                {
                                    if (cb.SelectedItem.ToString() == "Pallet Count" || cb.SelectedItem.ToString() == "Record Count")
                                    {
                                        Headers = Headers + cb.SelectedItem.ToString().PadRight(charpercolumn, ' ');
                                    }
                                }
                            }

                            for (int x = 1; x <= tcoltotals.Length; x++)
                            {
                                Control[] ct = this.Controls.Find("TColumn" + x.ToString(), true);
                                ComboBox cb = ct[0] as ComboBox;
                                if (cb.SelectedIndex != -1)
                                {
                                    Totalsbar = Totalsbar + (cb.SelectedItem.ToString() + ": " + tcoltotals[x - 1]) + " ";
                                }
                            }

                            for (int x = (pagecount - 1) * rowsperpage; x <= RecordCount && x < pagecount * rowsperpage; x++)
                            {
                                if (Totals[x, 0] == null) { Totals[x, 0] = ""; }
                                if (Totals[x, 1] == null) { Totals[x, 1] = ""; }
                                if (Totals[x, 2] == null) { Totals[x, 2] = ""; }
                                if (Totals[x, 3] == null) { Totals[x, 3] = ""; }
                                if (Totals[x, 4] == null) { Totals[x, 4] = ""; }
                                if (Totals[x, 5] == null) { Totals[x, 5] = ""; }
                                if (Totals[x, 6] == null) { Totals[x, 6] = ""; }
                                if (Totals[x, 0].Length >= charpercolumn)
                                { Totals[x, 0] = Totals[x, 0].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 1].Length >= charpercolumn)
                                { Totals[x, 1] = Totals[x, 1].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 2].Length >= charpercolumn)
                                { Totals[x, 2] = Totals[x, 2].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 3].Length >= charpercolumn)
                                { Totals[x, 3] = Totals[x, 3].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 4].Length >= charpercolumn)
                                { Totals[x, 4] = Totals[x, 4].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 5].Length >= charpercolumn)
                                { Totals[x, 5] = Totals[x, 5].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 6].Length >= charpercolumn)
                                { Totals[x, 6] = Totals[x, 6].Substring(0, charpercolumn - 1); }
                                if (Totals[x, 0] != "")
                                { records = records + Totals[x, 0].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 1] != "")
                                { records = records + Totals[x, 1].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 2] != "")
                                { records = records + Totals[x, 2].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 3] != "")
                                { records = records + Totals[x, 3].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 4] != "")
                                { records = records + Totals[x, 4].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 5] != "")
                                { records = records + Totals[x, 5].PadRight(charpercolumn, ' '); }
                                if (Totals[x, 6] != "")
                                { records = records + Totals[x, 6].PadRight(charpercolumn, ' '); }
                                records = records + "\r\n";
                                rows++;
                            }
                            if (rows >= RecordCount)
                            { done = true; }
                            string labelpath = "ReportLandscape.nlbl";
                            ILabel label = PrintEngineFactory.PrintEngine.OpenLabel(labelpath);
                            label.PrintSettings.PrinterName = PrinterSelect.SelectedItem.ToString();

                            label.Variables["ReportTitle"].SetValue(ReportTitle.Text);
                            label.Variables["Page"].SetValue(pagecount.ToString());
                            label.Variables["Data"].SetValue(records);
                            label.Variables["ColumnHeads"].SetValue(Headers);
                            label.Variables["Totals"].SetValue(Totalsbar);
                            label.Variables["DateRange"].SetValue(dateheader);

                            label.Print(1);

                            pagecount++;
                        }
                        while (!done);
                    }
                }
            }
        }
        #endregion
    }
}
