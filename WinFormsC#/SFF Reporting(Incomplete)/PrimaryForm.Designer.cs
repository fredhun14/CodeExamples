
namespace SFF_Reporting
{
    partial class ClearSumby
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClearSumby));
            this.PortraitorLandscape = new System.Windows.Forms.ComboBox();
            this.FPKPKGGEN = new System.Windows.Forms.ComboBox();
            this.DateRangeSelector = new System.Windows.Forms.MonthCalendar();
            this.Column1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.columnslabel = new System.Windows.Forms.Label();
            this.Column2 = new System.Windows.Forms.ComboBox();
            this.Column3 = new System.Windows.Forms.ComboBox();
            this.Column4 = new System.Windows.Forms.ComboBox();
            this.Column5 = new System.Windows.Forms.ComboBox();
            this.Column6 = new System.Windows.Forms.ComboBox();
            this.Column7 = new System.Windows.Forms.ComboBox();
            this.totalcolumnslabel = new System.Windows.Forms.Label();
            this.TColumn1 = new System.Windows.Forms.ComboBox();
            this.TColumn2 = new System.Windows.Forms.ComboBox();
            this.TColumn3 = new System.Windows.Forms.ComboBox();
            this.TColumn6 = new System.Windows.Forms.ComboBox();
            this.TColumn5 = new System.Windows.Forms.ComboBox();
            this.TColumn4 = new System.Windows.Forms.ComboBox();
            this.PortraitBox = new System.Windows.Forms.PictureBox();
            this.LandscapeBox = new System.Windows.Forms.PictureBox();
            this.ReportTitle = new System.Windows.Forms.TextBox();
            this.ReportTitleLabel = new System.Windows.Forms.Label();
            this.sumcollumnslabel = new System.Windows.Forms.Label();
            this.SumColumnsBy = new System.Windows.Forms.ComboBox();
            this.orderbylabel = new System.Windows.Forms.Label();
            this.OrderBy = new System.Windows.Forms.ComboBox();
            this.ascendordescendlabel = new System.Windows.Forms.Label();
            this.AscorDesc = new System.Windows.Forms.ComboBox();
            this.PrintButton = new System.Windows.Forms.Button();
            this.CSVInsteadCheck = new System.Windows.Forms.CheckBox();
            this.ConfigureButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.PrinterSelect = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.LocationSelection = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.NoPalletSize1 = new System.Windows.Forms.CheckBox();
            this.OnlyPalletSize1 = new System.Windows.Forms.CheckBox();
            this.PreviewButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.CSVBox = new System.Windows.Forms.TextBox();
            this.ClearColumnSumBy = new System.Windows.Forms.Button();
            this.CSVSeparator = new System.Windows.Forms.TextBox();
            this.SeparatorLabel = new System.Windows.Forms.Label();
            this.SaveCSVDialog = new System.Windows.Forms.SaveFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PortraitBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LandscapeBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // PortraitorLandscape
            // 
            this.PortraitorLandscape.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.PortraitorLandscape.FormattingEnabled = true;
            this.PortraitorLandscape.Items.AddRange(new object[] {
            "Portrait",
            "Landscape"});
            this.PortraitorLandscape.Location = new System.Drawing.Point(6, 44);
            this.PortraitorLandscape.Name = "PortraitorLandscape";
            this.PortraitorLandscape.Size = new System.Drawing.Size(173, 28);
            this.PortraitorLandscape.TabIndex = 0;
            this.PortraitorLandscape.SelectedIndexChanged += new System.EventHandler(this.TopLevel_SelectedIndexChanged);
            // 
            // FPKPKGGEN
            // 
            this.FPKPKGGEN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.FPKPKGGEN.FormattingEnabled = true;
            this.FPKPKGGEN.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "CPM",
            "OPC Data Log"});
            this.FPKPKGGEN.Location = new System.Drawing.Point(204, 44);
            this.FPKPKGGEN.Name = "FPKPKGGEN";
            this.FPKPKGGEN.Size = new System.Drawing.Size(173, 28);
            this.FPKPKGGEN.TabIndex = 1;
            this.FPKPKGGEN.SelectedIndexChanged += new System.EventHandler(this.FPKPKGGEN_SelectedIndexChanged);
            // 
            // DateRangeSelector
            // 
            this.DateRangeSelector.Location = new System.Drawing.Point(584, 47);
            this.DateRangeSelector.MaxSelectionCount = 10000;
            this.DateRangeSelector.Name = "DateRangeSelector";
            this.DateRangeSelector.TabIndex = 2;
            // 
            // Column1
            // 
            this.Column1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Column1.FormattingEnabled = true;
            this.Column1.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.Column1.Location = new System.Drawing.Point(6, 318);
            this.Column1.Name = "Column1";
            this.Column1.Size = new System.Drawing.Size(200, 28);
            this.Column1.TabIndex = 3;
            this.Column1.Visible = false;
            this.Column1.SelectionChangeCommitted += new System.EventHandler(this.ColumnChange);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "Portrait or Landscape";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label2.Location = new System.Drawing.Point(200, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(193, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "FPK, PKG, or General";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label3.Location = new System.Drawing.Point(580, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "Date Range Selection";
            // 
            // columnslabel
            // 
            this.columnslabel.AutoSize = true;
            this.columnslabel.BackColor = System.Drawing.Color.Transparent;
            this.columnslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.columnslabel.Location = new System.Drawing.Point(6, 290);
            this.columnslabel.Name = "columnslabel";
            this.columnslabel.Size = new System.Drawing.Size(146, 24);
            this.columnslabel.TabIndex = 7;
            this.columnslabel.Text = "Report Columns";
            this.columnslabel.Visible = false;
            // 
            // Column2
            // 
            this.Column2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Column2.FormattingEnabled = true;
            this.Column2.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.Column2.Location = new System.Drawing.Point(212, 318);
            this.Column2.Name = "Column2";
            this.Column2.Size = new System.Drawing.Size(200, 28);
            this.Column2.TabIndex = 8;
            this.Column2.Visible = false;
            this.Column2.SelectionChangeCommitted += new System.EventHandler(this.ColumnChange);
            // 
            // Column3
            // 
            this.Column3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Column3.FormattingEnabled = true;
            this.Column3.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.Column3.Location = new System.Drawing.Point(418, 318);
            this.Column3.Name = "Column3";
            this.Column3.Size = new System.Drawing.Size(200, 28);
            this.Column3.TabIndex = 9;
            this.Column3.Visible = false;
            this.Column3.SelectionChangeCommitted += new System.EventHandler(this.ColumnChange);
            // 
            // Column4
            // 
            this.Column4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Column4.FormattingEnabled = true;
            this.Column4.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.Column4.Location = new System.Drawing.Point(624, 318);
            this.Column4.Name = "Column4";
            this.Column4.Size = new System.Drawing.Size(200, 28);
            this.Column4.TabIndex = 10;
            this.Column4.Visible = false;
            this.Column4.SelectionChangeCommitted += new System.EventHandler(this.ColumnChange);
            // 
            // Column5
            // 
            this.Column5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Column5.FormattingEnabled = true;
            this.Column5.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.Column5.Location = new System.Drawing.Point(6, 352);
            this.Column5.Name = "Column5";
            this.Column5.Size = new System.Drawing.Size(200, 28);
            this.Column5.TabIndex = 11;
            this.Column5.Visible = false;
            this.Column5.SelectionChangeCommitted += new System.EventHandler(this.ColumnChange);
            // 
            // Column6
            // 
            this.Column6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Column6.FormattingEnabled = true;
            this.Column6.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.Column6.Location = new System.Drawing.Point(212, 352);
            this.Column6.Name = "Column6";
            this.Column6.Size = new System.Drawing.Size(200, 28);
            this.Column6.TabIndex = 12;
            this.Column6.Visible = false;
            this.Column6.SelectionChangeCommitted += new System.EventHandler(this.ColumnChange);
            // 
            // Column7
            // 
            this.Column7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Column7.FormattingEnabled = true;
            this.Column7.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.Column7.Location = new System.Drawing.Point(418, 352);
            this.Column7.Name = "Column7";
            this.Column7.Size = new System.Drawing.Size(200, 28);
            this.Column7.TabIndex = 13;
            this.Column7.Visible = false;
            this.Column7.SelectionChangeCommitted += new System.EventHandler(this.ColumnChange);
            // 
            // totalcolumnslabel
            // 
            this.totalcolumnslabel.AutoSize = true;
            this.totalcolumnslabel.BackColor = System.Drawing.Color.Transparent;
            this.totalcolumnslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.totalcolumnslabel.Location = new System.Drawing.Point(6, 398);
            this.totalcolumnslabel.Name = "totalcolumnslabel";
            this.totalcolumnslabel.Size = new System.Drawing.Size(153, 24);
            this.totalcolumnslabel.TabIndex = 14;
            this.totalcolumnslabel.Text = "Totaled Columns";
            this.totalcolumnslabel.Visible = false;
            // 
            // TColumn1
            // 
            this.TColumn1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TColumn1.FormattingEnabled = true;
            this.TColumn1.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.TColumn1.Location = new System.Drawing.Point(6, 425);
            this.TColumn1.Name = "TColumn1";
            this.TColumn1.Size = new System.Drawing.Size(200, 28);
            this.TColumn1.TabIndex = 15;
            this.TColumn1.Visible = false;
            this.TColumn1.SelectedIndexChanged += new System.EventHandler(this.TColumn_SelectedIndexChanged);
            // 
            // TColumn2
            // 
            this.TColumn2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TColumn2.FormattingEnabled = true;
            this.TColumn2.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.TColumn2.Location = new System.Drawing.Point(212, 425);
            this.TColumn2.Name = "TColumn2";
            this.TColumn2.Size = new System.Drawing.Size(200, 28);
            this.TColumn2.TabIndex = 16;
            this.TColumn2.Visible = false;
            this.TColumn2.SelectedIndexChanged += new System.EventHandler(this.TColumn_SelectedIndexChanged);
            // 
            // TColumn3
            // 
            this.TColumn3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TColumn3.FormattingEnabled = true;
            this.TColumn3.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.TColumn3.Location = new System.Drawing.Point(418, 425);
            this.TColumn3.Name = "TColumn3";
            this.TColumn3.Size = new System.Drawing.Size(200, 28);
            this.TColumn3.TabIndex = 17;
            this.TColumn3.Visible = false;
            this.TColumn3.SelectedIndexChanged += new System.EventHandler(this.TColumn_SelectedIndexChanged);
            // 
            // TColumn6
            // 
            this.TColumn6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TColumn6.FormattingEnabled = true;
            this.TColumn6.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.TColumn6.Location = new System.Drawing.Point(418, 459);
            this.TColumn6.Name = "TColumn6";
            this.TColumn6.Size = new System.Drawing.Size(200, 28);
            this.TColumn6.TabIndex = 20;
            this.TColumn6.Visible = false;
            this.TColumn6.SelectedIndexChanged += new System.EventHandler(this.TColumn_SelectedIndexChanged);
            // 
            // TColumn5
            // 
            this.TColumn5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TColumn5.FormattingEnabled = true;
            this.TColumn5.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.TColumn5.Location = new System.Drawing.Point(212, 459);
            this.TColumn5.Name = "TColumn5";
            this.TColumn5.Size = new System.Drawing.Size(200, 28);
            this.TColumn5.TabIndex = 19;
            this.TColumn5.Visible = false;
            this.TColumn5.SelectedIndexChanged += new System.EventHandler(this.TColumn_SelectedIndexChanged);
            // 
            // TColumn4
            // 
            this.TColumn4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TColumn4.FormattingEnabled = true;
            this.TColumn4.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.TColumn4.Location = new System.Drawing.Point(6, 459);
            this.TColumn4.Name = "TColumn4";
            this.TColumn4.Size = new System.Drawing.Size(200, 28);
            this.TColumn4.TabIndex = 18;
            this.TColumn4.Visible = false;
            this.TColumn4.SelectedIndexChanged += new System.EventHandler(this.TColumn_SelectedIndexChanged);
            // 
            // PortraitBox
            // 
            this.PortraitBox.BackColor = System.Drawing.Color.Transparent;
            this.PortraitBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PortraitBox.Location = new System.Drawing.Point(956, 11);
            this.PortraitBox.Name = "PortraitBox";
            this.PortraitBox.Size = new System.Drawing.Size(425, 550);
            this.PortraitBox.TabIndex = 21;
            this.PortraitBox.TabStop = false;
            this.PortraitBox.Visible = false;
            this.PortraitBox.MouseHover += new System.EventHandler(this.PortraitBox_MouseHover);
            // 
            // LandscapeBox
            // 
            this.LandscapeBox.BackColor = System.Drawing.Color.Transparent;
            this.LandscapeBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LandscapeBox.Location = new System.Drawing.Point(831, 11);
            this.LandscapeBox.Name = "LandscapeBox";
            this.LandscapeBox.Size = new System.Drawing.Size(550, 425);
            this.LandscapeBox.TabIndex = 22;
            this.LandscapeBox.TabStop = false;
            this.LandscapeBox.Visible = false;
            // 
            // ReportTitle
            // 
            this.ReportTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.ReportTitle.Location = new System.Drawing.Point(6, 201);
            this.ReportTitle.Name = "ReportTitle";
            this.ReportTitle.Size = new System.Drawing.Size(371, 29);
            this.ReportTitle.TabIndex = 23;
            this.ReportTitle.Text = "Production Report";
            // 
            // ReportTitleLabel
            // 
            this.ReportTitleLabel.AutoSize = true;
            this.ReportTitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.ReportTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.ReportTitleLabel.Location = new System.Drawing.Point(6, 174);
            this.ReportTitleLabel.Name = "ReportTitleLabel";
            this.ReportTitleLabel.Size = new System.Drawing.Size(106, 24);
            this.ReportTitleLabel.TabIndex = 24;
            this.ReportTitleLabel.Text = "Report Title";
            // 
            // sumcollumnslabel
            // 
            this.sumcollumnslabel.AutoSize = true;
            this.sumcollumnslabel.BackColor = System.Drawing.Color.Transparent;
            this.sumcollumnslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.sumcollumnslabel.Location = new System.Drawing.Point(6, 233);
            this.sumcollumnslabel.Name = "sumcollumnslabel";
            this.sumcollumnslabel.Size = new System.Drawing.Size(154, 24);
            this.sumcollumnslabel.TabIndex = 26;
            this.sumcollumnslabel.Text = "Sum Columns by";
            this.sumcollumnslabel.Visible = false;
            // 
            // SumColumnsBy
            // 
            this.SumColumnsBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.SumColumnsBy.FormattingEnabled = true;
            this.SumColumnsBy.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.SumColumnsBy.Location = new System.Drawing.Point(6, 261);
            this.SumColumnsBy.Name = "SumColumnsBy";
            this.SumColumnsBy.Size = new System.Drawing.Size(173, 28);
            this.SumColumnsBy.TabIndex = 25;
            this.SumColumnsBy.Visible = false;
            this.SumColumnsBy.SelectedIndexChanged += new System.EventHandler(this.PossilbeReportChangeIndexChanged);
            // 
            // orderbylabel
            // 
            this.orderbylabel.AutoSize = true;
            this.orderbylabel.BackColor = System.Drawing.Color.Transparent;
            this.orderbylabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.orderbylabel.Location = new System.Drawing.Point(6, 507);
            this.orderbylabel.Name = "orderbylabel";
            this.orderbylabel.Size = new System.Drawing.Size(136, 24);
            this.orderbylabel.TabIndex = 28;
            this.orderbylabel.Text = "Order Rows by";
            this.orderbylabel.Visible = false;
            // 
            // OrderBy
            // 
            this.OrderBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.OrderBy.FormattingEnabled = true;
            this.OrderBy.Items.AddRange(new object[] {
            "Freshpack",
            "Packaging",
            "General"});
            this.OrderBy.Location = new System.Drawing.Point(6, 535);
            this.OrderBy.Name = "OrderBy";
            this.OrderBy.Size = new System.Drawing.Size(200, 28);
            this.OrderBy.TabIndex = 27;
            this.OrderBy.Visible = false;
            this.OrderBy.SelectedIndexChanged += new System.EventHandler(this.PossilbeReportChangeIndexChanged);
            // 
            // ascendordescendlabel
            // 
            this.ascendordescendlabel.AutoSize = true;
            this.ascendordescendlabel.BackColor = System.Drawing.Color.Transparent;
            this.ascendordescendlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.ascendordescendlabel.Location = new System.Drawing.Point(208, 507);
            this.ascendordescendlabel.Name = "ascendordescendlabel";
            this.ascendordescendlabel.Size = new System.Drawing.Size(178, 24);
            this.ascendordescendlabel.TabIndex = 30;
            this.ascendordescendlabel.Text = "Ascend or Descend";
            this.ascendordescendlabel.Visible = false;
            // 
            // AscorDesc
            // 
            this.AscorDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.AscorDesc.FormattingEnabled = true;
            this.AscorDesc.Items.AddRange(new object[] {
            "Asc",
            "Desc"});
            this.AscorDesc.Location = new System.Drawing.Point(212, 535);
            this.AscorDesc.Name = "AscorDesc";
            this.AscorDesc.Size = new System.Drawing.Size(200, 28);
            this.AscorDesc.TabIndex = 29;
            this.AscorDesc.Visible = false;
            this.AscorDesc.SelectedIndexChanged += new System.EventHandler(this.PossilbeReportChangeIndexChanged);
            // 
            // PrintButton
            // 
            this.PrintButton.Enabled = false;
            this.PrintButton.Location = new System.Drawing.Point(6, 638);
            this.PrintButton.Name = "PrintButton";
            this.PrintButton.Size = new System.Drawing.Size(127, 52);
            this.PrintButton.TabIndex = 31;
            this.PrintButton.Text = "Print";
            this.PrintButton.UseVisualStyleBackColor = true;
            this.PrintButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // CSVInsteadCheck
            // 
            this.CSVInsteadCheck.AutoSize = true;
            this.CSVInsteadCheck.BackColor = System.Drawing.Color.Transparent;
            this.CSVInsteadCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.CSVInsteadCheck.Location = new System.Drawing.Point(6, 143);
            this.CSVInsteadCheck.Name = "CSVInsteadCheck";
            this.CSVInsteadCheck.Size = new System.Drawing.Size(212, 28);
            this.CSVInsteadCheck.TabIndex = 32;
            this.CSVInsteadCheck.Text = "Export to CSV instead";
            this.CSVInsteadCheck.UseVisualStyleBackColor = false;
            this.CSVInsteadCheck.CheckedChanged += new System.EventHandler(this.CSVInsteadCheck_CheckedChanged);
            // 
            // ConfigureButton
            // 
            this.ConfigureButton.Location = new System.Drawing.Point(697, 638);
            this.ConfigureButton.Name = "ConfigureButton";
            this.ConfigureButton.Size = new System.Drawing.Size(127, 52);
            this.ConfigureButton.TabIndex = 33;
            this.ConfigureButton.Text = "Configure";
            this.ConfigureButton.UseVisualStyleBackColor = true;
            this.ConfigureButton.Click += new System.EventHandler(this.ConfigureButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label4.Location = new System.Drawing.Point(6, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 24);
            this.label4.TabIndex = 35;
            this.label4.Text = "Printer Selection";
            // 
            // PrinterSelect
            // 
            this.PrinterSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.PrinterSelect.FormattingEnabled = true;
            this.PrinterSelect.Items.AddRange(new object[] {
            "Portrait",
            "Landscape"});
            this.PrinterSelect.Location = new System.Drawing.Point(6, 109);
            this.PrinterSelect.Name = "PrinterSelect";
            this.PrinterSelect.Size = new System.Drawing.Size(371, 28);
            this.PrinterSelect.TabIndex = 34;
            this.PrinterSelect.SelectedIndexChanged += new System.EventHandler(this.PrinterSelect_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label5.Location = new System.Drawing.Point(399, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 24);
            this.label5.TabIndex = 37;
            this.label5.Text = "Location";
            // 
            // LocationSelection
            // 
            this.LocationSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.LocationSelection.FormattingEnabled = true;
            this.LocationSelection.Items.AddRange(new object[] {
            "Weston",
            "Garrett",
            "Both"});
            this.LocationSelection.Location = new System.Drawing.Point(403, 44);
            this.LocationSelection.Name = "LocationSelection";
            this.LocationSelection.Size = new System.Drawing.Size(173, 28);
            this.LocationSelection.TabIndex = 36;
            this.LocationSelection.SelectedIndexChanged += new System.EventHandler(this.TopLevel_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label7.Location = new System.Drawing.Point(185, 241);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(487, 72);
            this.label7.TabIndex = 38;
            this.label7.Text = "If nothing is selected in the sum by box every record\r\nfrom the selected date ran" +
    "ge will be displayed. Otherwise\r\nevery column that can be sumed will be.";
            // 
            // NoPalletSize1
            // 
            this.NoPalletSize1.AutoSize = true;
            this.NoPalletSize1.BackColor = System.Drawing.Color.Transparent;
            this.NoPalletSize1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.NoPalletSize1.Location = new System.Drawing.Point(433, 507);
            this.NoPalletSize1.Name = "NoPalletSize1";
            this.NoPalletSize1.Size = new System.Drawing.Size(239, 24);
            this.NoPalletSize1.TabIndex = 39;
            this.NoPalletSize1.Text = "Do not include Pallet Size of 1";
            this.NoPalletSize1.UseVisualStyleBackColor = false;
            this.NoPalletSize1.Visible = false;
            this.NoPalletSize1.CheckedChanged += new System.EventHandler(this.NoPalletSize1_CheckedChanged);
            // 
            // OnlyPalletSize1
            // 
            this.OnlyPalletSize1.AutoSize = true;
            this.OnlyPalletSize1.BackColor = System.Drawing.Color.Transparent;
            this.OnlyPalletSize1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.OnlyPalletSize1.Location = new System.Drawing.Point(433, 539);
            this.OnlyPalletSize1.Name = "OnlyPalletSize1";
            this.OnlyPalletSize1.Size = new System.Drawing.Size(222, 24);
            this.OnlyPalletSize1.TabIndex = 40;
            this.OnlyPalletSize1.Text = "Only include Pallet Size of 1";
            this.OnlyPalletSize1.UseVisualStyleBackColor = false;
            this.OnlyPalletSize1.Visible = false;
            this.OnlyPalletSize1.CheckedChanged += new System.EventHandler(this.OnlyPalletSize1_CheckedChanged);
            // 
            // PreviewButton
            // 
            this.PreviewButton.Location = new System.Drawing.Point(6, 580);
            this.PreviewButton.Name = "PreviewButton";
            this.PreviewButton.Size = new System.Drawing.Size(127, 52);
            this.PreviewButton.TabIndex = 41;
            this.PreviewButton.Text = "Preview";
            this.PreviewButton.UseVisualStyleBackColor = true;
            this.PreviewButton.Click += new System.EventHandler(this.PreviewButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(139, 580);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(127, 52);
            this.ClearButton.TabIndex = 42;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // CSVBox
            // 
            this.CSVBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.CSVBox.Location = new System.Drawing.Point(830, 8);
            this.CSVBox.Multiline = true;
            this.CSVBox.Name = "CSVBox";
            this.CSVBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.CSVBox.Size = new System.Drawing.Size(550, 553);
            this.CSVBox.TabIndex = 43;
            this.CSVBox.Visible = false;
            // 
            // ClearColumnSumBy
            // 
            this.ClearColumnSumBy.Location = new System.Drawing.Point(678, 241);
            this.ClearColumnSumBy.Name = "ClearColumnSumBy";
            this.ClearColumnSumBy.Size = new System.Drawing.Size(127, 52);
            this.ClearColumnSumBy.TabIndex = 44;
            this.ClearColumnSumBy.Text = "Clear Sum by";
            this.ClearColumnSumBy.UseVisualStyleBackColor = true;
            this.ClearColumnSumBy.Click += new System.EventHandler(this.ClearColumnSumBy_Click);
            // 
            // CSVSeparator
            // 
            this.CSVSeparator.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.CSVSeparator.Location = new System.Drawing.Point(326, 141);
            this.CSVSeparator.MaxLength = 1;
            this.CSVSeparator.Name = "CSVSeparator";
            this.CSVSeparator.Size = new System.Drawing.Size(39, 29);
            this.CSVSeparator.TabIndex = 45;
            this.CSVSeparator.Text = ",";
            this.CSVSeparator.Visible = false;
            // 
            // SeparatorLabel
            // 
            this.SeparatorLabel.AutoSize = true;
            this.SeparatorLabel.BackColor = System.Drawing.Color.Transparent;
            this.SeparatorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.SeparatorLabel.Location = new System.Drawing.Point(224, 144);
            this.SeparatorLabel.Name = "SeparatorLabel";
            this.SeparatorLabel.Size = new System.Drawing.Size(96, 24);
            this.SeparatorLabel.TabIndex = 46;
            this.SeparatorLabel.Text = "Separator:";
            this.SeparatorLabel.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(1206, 580);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(174, 110);
            this.pictureBox1.TabIndex = 47;
            this.pictureBox1.TabStop = false;
            // 
            // ClearSumby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1393, 702);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.SeparatorLabel);
            this.Controls.Add(this.CSVSeparator);
            this.Controls.Add(this.ClearColumnSumBy);
            this.Controls.Add(this.CSVBox);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.PreviewButton);
            this.Controls.Add(this.OnlyPalletSize1);
            this.Controls.Add(this.NoPalletSize1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.LocationSelection);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PrinterSelect);
            this.Controls.Add(this.ConfigureButton);
            this.Controls.Add(this.CSVInsteadCheck);
            this.Controls.Add(this.PrintButton);
            this.Controls.Add(this.ascendordescendlabel);
            this.Controls.Add(this.AscorDesc);
            this.Controls.Add(this.orderbylabel);
            this.Controls.Add(this.OrderBy);
            this.Controls.Add(this.sumcollumnslabel);
            this.Controls.Add(this.SumColumnsBy);
            this.Controls.Add(this.ReportTitleLabel);
            this.Controls.Add(this.ReportTitle);
            this.Controls.Add(this.LandscapeBox);
            this.Controls.Add(this.PortraitBox);
            this.Controls.Add(this.TColumn6);
            this.Controls.Add(this.TColumn5);
            this.Controls.Add(this.TColumn4);
            this.Controls.Add(this.TColumn3);
            this.Controls.Add(this.TColumn2);
            this.Controls.Add(this.TColumn1);
            this.Controls.Add(this.totalcolumnslabel);
            this.Controls.Add(this.Column7);
            this.Controls.Add(this.Column6);
            this.Controls.Add(this.Column5);
            this.Controls.Add(this.Column4);
            this.Controls.Add(this.Column3);
            this.Controls.Add(this.Column2);
            this.Controls.Add(this.columnslabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Column1);
            this.Controls.Add(this.DateRangeSelector);
            this.Controls.Add(this.FPKPKGGEN);
            this.Controls.Add(this.PortraitorLandscape);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClearSumby";
            this.Text = "SFF Reporting";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PrimaryForm_FormClosed);
            this.Load += new System.EventHandler(this.PrimaryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PortraitBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LandscapeBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox PortraitorLandscape;
        private System.Windows.Forms.ComboBox FPKPKGGEN;
        private System.Windows.Forms.MonthCalendar DateRangeSelector;
        private System.Windows.Forms.ComboBox Column1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label columnslabel;
        private System.Windows.Forms.ComboBox Column2;
        private System.Windows.Forms.ComboBox Column3;
        private System.Windows.Forms.ComboBox Column4;
        private System.Windows.Forms.ComboBox Column5;
        private System.Windows.Forms.ComboBox Column6;
        private System.Windows.Forms.ComboBox Column7;
        private System.Windows.Forms.Label totalcolumnslabel;
        private System.Windows.Forms.ComboBox TColumn1;
        private System.Windows.Forms.ComboBox TColumn2;
        private System.Windows.Forms.ComboBox TColumn3;
        private System.Windows.Forms.ComboBox TColumn6;
        private System.Windows.Forms.ComboBox TColumn5;
        private System.Windows.Forms.ComboBox TColumn4;
        private System.Windows.Forms.PictureBox PortraitBox;
        private System.Windows.Forms.PictureBox LandscapeBox;
        private System.Windows.Forms.TextBox ReportTitle;
        private System.Windows.Forms.Label ReportTitleLabel;
        private System.Windows.Forms.Label sumcollumnslabel;
        private System.Windows.Forms.ComboBox SumColumnsBy;
        private System.Windows.Forms.Label orderbylabel;
        private System.Windows.Forms.ComboBox OrderBy;
        private System.Windows.Forms.Label ascendordescendlabel;
        private System.Windows.Forms.ComboBox AscorDesc;
        private System.Windows.Forms.Button PrintButton;
        private System.Windows.Forms.CheckBox CSVInsteadCheck;
        private System.Windows.Forms.Button ConfigureButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox PrinterSelect;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox LocationSelection;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox NoPalletSize1;
        private System.Windows.Forms.CheckBox OnlyPalletSize1;
        private System.Windows.Forms.Button PreviewButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.TextBox CSVBox;
        private System.Windows.Forms.Button ClearColumnSumBy;
        private System.Windows.Forms.TextBox CSVSeparator;
        private System.Windows.Forms.Label SeparatorLabel;
        private System.Windows.Forms.SaveFileDialog SaveCSVDialog;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

