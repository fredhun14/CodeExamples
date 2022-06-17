
namespace SFF_Reporting
{
    partial class AreYouSureYouWantToPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AreYouSureYouWantToPrint));
            this.PagesLabel = new System.Windows.Forms.Label();
            this.YesButton = new System.Windows.Forms.Button();
            this.NoButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PagesLabel
            // 
            this.PagesLabel.BackColor = System.Drawing.Color.Transparent;
            this.PagesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.PagesLabel.Location = new System.Drawing.Point(34, 9);
            this.PagesLabel.Name = "PagesLabel";
            this.PagesLabel.Size = new System.Drawing.Size(222, 68);
            this.PagesLabel.TabIndex = 0;
            this.PagesLabel.Text = "Are you sure you want to print XXXX pages?";
            // 
            // YesButton
            // 
            this.YesButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.YesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.YesButton.Location = new System.Drawing.Point(12, 80);
            this.YesButton.Name = "YesButton";
            this.YesButton.Size = new System.Drawing.Size(95, 40);
            this.YesButton.TabIndex = 1;
            this.YesButton.Text = "Yes!";
            this.YesButton.UseVisualStyleBackColor = true;
            // 
            // NoButton
            // 
            this.NoButton.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.NoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.NoButton.Location = new System.Drawing.Point(191, 80);
            this.NoButton.Name = "NoButton";
            this.NoButton.Size = new System.Drawing.Size(95, 40);
            this.NoButton.TabIndex = 2;
            this.NoButton.Text = "No!";
            this.NoButton.UseVisualStyleBackColor = true;
            // 
            // AreYouSureYouWantToPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(298, 132);
            this.Controls.Add(this.NoButton);
            this.Controls.Add(this.YesButton);
            this.Controls.Add(this.PagesLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AreYouSureYouWantToPrint";
            this.Text = "Are YouSure You Want To Print?";
            this.Load += new System.EventHandler(this.AreYouSureYouWantToPrint_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label PagesLabel;
        private System.Windows.Forms.Button YesButton;
        private System.Windows.Forms.Button NoButton;
    }
}