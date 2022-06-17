
namespace SFFLabelNManifest
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.AboutLabel = new System.Windows.Forms.Label();
            this.OKbut = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AboutLabel
            // 
            this.AboutLabel.AutoSize = true;
            this.AboutLabel.BackColor = System.Drawing.Color.Transparent;
            this.AboutLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AboutLabel.Location = new System.Drawing.Point(12, 9);
            this.AboutLabel.Name = "AboutLabel";
            this.AboutLabel.Size = new System.Drawing.Size(275, 60);
            this.AboutLabel.TabIndex = 0;
            this.AboutLabel.Text = "Program written by: Michael Hunsaker\r\nVersion: 2.3.018\r\nVersion release date: 06/" +
    "13/2022\r\n";
            this.AboutLabel.Click += new System.EventHandler(this.AboutLabel_Click);
            // 
            // OKbut
            // 
            this.OKbut.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKbut.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OKbut.Location = new System.Drawing.Point(16, 358);
            this.OKbut.Name = "OKbut";
            this.OKbut.Size = new System.Drawing.Size(120, 37);
            this.OKbut.TabIndex = 1;
            this.OKbut.Text = "OK";
            this.OKbut.UseVisualStyleBackColor = true;
            this.OKbut.Click += new System.EventHandler(this.OKbut_Click);
            // 
            // About
            // 
            this.AcceptButton = this.OKbut;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(454, 419);
            this.Controls.Add(this.OKbut);
            this.Controls.Add(this.AboutLabel);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.Text = "About";
            this.Load += new System.EventHandler(this.About_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AboutLabel;
        private System.Windows.Forms.Button OKbut;
    }
}