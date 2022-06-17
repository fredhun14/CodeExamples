namespace FreshPackManifest
{
    partial class DailyLogDisplay
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DailyLogDisplay));
            this.CloseButton = new System.Windows.Forms.Button();
            this.FreshDataGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.SerialEntryBox = new System.Windows.Forms.TextBox();
            this.PrintBacktiButton = new System.Windows.Forms.Button();
            this.ReloadData = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.FreshDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.CloseButton.Location = new System.Drawing.Point(671, 582);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(123, 60);
            this.CloseButton.TabIndex = 3;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // FreshDataGrid
            // 
            this.FreshDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FreshDataGrid.Location = new System.Drawing.Point(12, 12);
            this.FreshDataGrid.Name = "FreshDataGrid";
            this.FreshDataGrid.Size = new System.Drawing.Size(600, 630);
            this.FreshDataGrid.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.label1.Location = new System.Drawing.Point(618, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 26);
            this.label1.TabIndex = 7;
            this.label1.Text = "Serial:";
            // 
            // SerialEntryBox
            // 
            this.SerialEntryBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.SerialEntryBox.Location = new System.Drawing.Point(623, 102);
            this.SerialEntryBox.MaxLength = 8;
            this.SerialEntryBox.Name = "SerialEntryBox";
            this.SerialEntryBox.Size = new System.Drawing.Size(176, 32);
            this.SerialEntryBox.TabIndex = 6;
            // 
            // PrintBacktiButton
            // 
            this.PrintBacktiButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.PrintBacktiButton.Location = new System.Drawing.Point(671, 12);
            this.PrintBacktiButton.Name = "PrintBacktiButton";
            this.PrintBacktiButton.Size = new System.Drawing.Size(123, 60);
            this.PrintBacktiButton.TabIndex = 5;
            this.PrintBacktiButton.Text = "Print Bacti Sticker";
            this.PrintBacktiButton.UseVisualStyleBackColor = true;
            this.PrintBacktiButton.Click += new System.EventHandler(this.PrintBacktiButton_Click);
            // 
            // ReloadData
            // 
            this.ReloadData.Enabled = true;
            this.ReloadData.Interval = 30000;
            this.ReloadData.Tick += new System.EventHandler(this.ReloadData_Tick);
            // 
            // DailyLogDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(806, 654);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SerialEntryBox);
            this.Controls.Add(this.PrintBacktiButton);
            this.Controls.Add(this.FreshDataGrid);
            this.Controls.Add(this.CloseButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DailyLogDisplay";
            this.Text = "Daily Log Display";
            this.Load += new System.EventHandler(this.DailyLogDisplay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FreshDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.DataGridView FreshDataGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SerialEntryBox;
        private System.Windows.Forms.Button PrintBacktiButton;
        private System.Windows.Forms.Timer ReloadData;
    }
}