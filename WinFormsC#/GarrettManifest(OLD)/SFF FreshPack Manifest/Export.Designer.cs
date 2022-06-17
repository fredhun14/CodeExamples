
namespace FreshPackManifest
{
    partial class Export
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Export));
            this.CloseBut = new System.Windows.Forms.Button();
            this.ExportFromPreviousDayFreshPack = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.FreshPackPath = new System.Windows.Forms.Label();
            this.ExportBoth = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CloseBut
            // 
            this.CloseBut.Location = new System.Drawing.Point(337, 56);
            this.CloseBut.Name = "CloseBut";
            this.CloseBut.Size = new System.Drawing.Size(84, 69);
            this.CloseBut.TabIndex = 17;
            this.CloseBut.Text = "Close";
            this.CloseBut.UseVisualStyleBackColor = true;
            this.CloseBut.Click += new System.EventHandler(this.CloseBut_Click);
            // 
            // ExportFromPreviousDayFreshPack
            // 
            this.ExportFromPreviousDayFreshPack.Location = new System.Drawing.Point(178, 56);
            this.ExportFromPreviousDayFreshPack.Name = "ExportFromPreviousDayFreshPack";
            this.ExportFromPreviousDayFreshPack.Size = new System.Drawing.Size(153, 69);
            this.ExportFromPreviousDayFreshPack.TabIndex = 16;
            this.ExportFromPreviousDayFreshPack.Text = "Export From Previous Day";
            this.ExportFromPreviousDayFreshPack.UseVisualStyleBackColor = true;
            this.ExportFromPreviousDayFreshPack.Click += new System.EventHandler(this.ExportFromPreviousDayFreshPack_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 26);
            this.label1.TabIndex = 14;
            this.label1.Text = "Destination:";
            // 
            // FreshPackPath
            // 
            this.FreshPackPath.AutoSize = true;
            this.FreshPackPath.BackColor = System.Drawing.Color.Transparent;
            this.FreshPackPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FreshPackPath.Location = new System.Drawing.Point(145, 9);
            this.FreshPackPath.Name = "FreshPackPath";
            this.FreshPackPath.Size = new System.Drawing.Size(148, 26);
            this.FreshPackPath.TabIndex = 12;
            this.FreshPackPath.Text = "freshpackpath";
            // 
            // ExportBoth
            // 
            this.ExportBoth.Location = new System.Drawing.Point(19, 56);
            this.ExportBoth.Name = "ExportBoth";
            this.ExportBoth.Size = new System.Drawing.Size(153, 69);
            this.ExportBoth.TabIndex = 10;
            this.ExportBoth.Text = "Export";
            this.ExportBoth.UseVisualStyleBackColor = true;
            this.ExportBoth.Click += new System.EventHandler(this.Export_Click);
            // 
            // Export
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(438, 140);
            this.Controls.Add(this.CloseBut);
            this.Controls.Add(this.ExportFromPreviousDayFreshPack);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FreshPackPath);
            this.Controls.Add(this.ExportBoth);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Export";
            this.Text = "Export";
            this.Load += new System.EventHandler(this.Export_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CloseBut;
        private System.Windows.Forms.Button ExportFromPreviousDayFreshPack;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label FreshPackPath;
        private System.Windows.Forms.Button ExportBoth;
    }
}