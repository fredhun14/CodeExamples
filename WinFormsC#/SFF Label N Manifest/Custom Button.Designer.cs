
namespace SFFLabelNManifest
{
    partial class Custom_Button
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Custom_Button));
            this.ButtonLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ButtonLabel
            // 
            this.ButtonLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ButtonLabel.BackColor = System.Drawing.Color.Transparent;
            this.ButtonLabel.Location = new System.Drawing.Point(26, 20);
            this.ButtonLabel.Name = "ButtonLabel";
            this.ButtonLabel.Size = new System.Drawing.Size(484, 253);
            this.ButtonLabel.TabIndex = 0;
            // 
            // Custom_Button
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.ButtonLabel);
            this.Name = "Custom_Button";
            this.Size = new System.Drawing.Size(535, 298);
            this.Load += new System.EventHandler(this.Custom_Button_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ButtonLabel;
    }
}
