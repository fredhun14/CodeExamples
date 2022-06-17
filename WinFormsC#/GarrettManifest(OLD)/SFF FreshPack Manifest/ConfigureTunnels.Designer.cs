
namespace FreshPackManifest
{
    partial class ConfigureTunnels
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureTunnels));
            this.Tunnel = new System.Windows.Forms.ComboBox();
            this.AddBut = new System.Windows.Forms.Button();
            this.RemoveBut = new System.Windows.Forms.Button();
            this.EntryBox = new System.Windows.Forms.TextBox();
            this.OKBut = new System.Windows.Forms.Button();
            this.CancelBut = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Tunnel
            // 
            this.Tunnel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.Tunnel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.Tunnel.DropDownHeight = 200;
            this.Tunnel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Tunnel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Tunnel.FormattingEnabled = true;
            this.Tunnel.IntegralHeight = false;
            this.Tunnel.Location = new System.Drawing.Point(12, 72);
            this.Tunnel.Name = "Tunnel";
            this.Tunnel.Size = new System.Drawing.Size(128, 33);
            this.Tunnel.TabIndex = 10044;
            // 
            // AddBut
            // 
            this.AddBut.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.AddBut.Location = new System.Drawing.Point(183, 111);
            this.AddBut.Name = "AddBut";
            this.AddBut.Size = new System.Drawing.Size(104, 52);
            this.AddBut.TabIndex = 10045;
            this.AddBut.Text = "Add";
            this.AddBut.UseVisualStyleBackColor = true;
            this.AddBut.Click += new System.EventHandler(this.AddBut_Click);
            // 
            // RemoveBut
            // 
            this.RemoveBut.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.RemoveBut.Location = new System.Drawing.Point(183, 15);
            this.RemoveBut.Name = "RemoveBut";
            this.RemoveBut.Size = new System.Drawing.Size(104, 52);
            this.RemoveBut.TabIndex = 10046;
            this.RemoveBut.Text = "Remove";
            this.RemoveBut.UseVisualStyleBackColor = true;
            this.RemoveBut.Click += new System.EventHandler(this.RemoveBut_Click);
            // 
            // EntryBox
            // 
            this.EntryBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.EntryBox.Location = new System.Drawing.Point(183, 73);
            this.EntryBox.MaxLength = 2;
            this.EntryBox.Name = "EntryBox";
            this.EntryBox.Size = new System.Drawing.Size(104, 32);
            this.EntryBox.TabIndex = 10047;
            // 
            // OKBut
            // 
            this.OKBut.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBut.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.OKBut.Location = new System.Drawing.Point(12, 199);
            this.OKBut.Name = "OKBut";
            this.OKBut.Size = new System.Drawing.Size(104, 52);
            this.OKBut.TabIndex = 10048;
            this.OKBut.Text = "OK";
            this.OKBut.UseVisualStyleBackColor = true;
            this.OKBut.Click += new System.EventHandler(this.OKBut_Click);
            // 
            // CancelBut
            // 
            this.CancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBut.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.CancelBut.Location = new System.Drawing.Point(214, 199);
            this.CancelBut.Name = "CancelBut";
            this.CancelBut.Size = new System.Drawing.Size(104, 52);
            this.CancelBut.TabIndex = 10049;
            this.CancelBut.Text = "Cancel";
            this.CancelBut.UseVisualStyleBackColor = true;
            this.CancelBut.Click += new System.EventHandler(this.CancelBut_Click);
            // 
            // ConfigureTunnels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(330, 263);
            this.Controls.Add(this.CancelBut);
            this.Controls.Add(this.OKBut);
            this.Controls.Add(this.EntryBox);
            this.Controls.Add(this.RemoveBut);
            this.Controls.Add(this.AddBut);
            this.Controls.Add(this.Tunnel);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigureTunnels";
            this.Text = "ConfigureTunnels";
            this.Load += new System.EventHandler(this.ConfigureTunnels_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Tunnel;
        private System.Windows.Forms.Button AddBut;
        private System.Windows.Forms.Button RemoveBut;
        private System.Windows.Forms.TextBox EntryBox;
        private System.Windows.Forms.Button OKBut;
        private System.Windows.Forms.Button CancelBut;
    }
}