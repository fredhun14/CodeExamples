
namespace GarrettManifester
{
    partial class ConfigureResources
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
            this.DropText = new System.Windows.Forms.TextBox();
            this.DropButton = new System.Windows.Forms.Button();
            this.SectionBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.OkButt = new System.Windows.Forms.Button();
            this.CancelButt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DropText
            // 
            this.DropText.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DropText.Location = new System.Drawing.Point(17, 41);
            this.DropText.Name = "DropText";
            this.DropText.Size = new System.Drawing.Size(159, 32);
            this.DropText.TabIndex = 0;
            // 
            // DropButton
            // 
            this.DropButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DropButton.Location = new System.Drawing.Point(43, 79);
            this.DropButton.Name = "DropButton";
            this.DropButton.Size = new System.Drawing.Size(96, 38);
            this.DropButton.TabIndex = 2;
            this.DropButton.Text = "Add";
            this.DropButton.UseVisualStyleBackColor = true;
            this.DropButton.Click += new System.EventHandler(this.DropButton_Click);
            // 
            // SectionBox
            // 
            this.SectionBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SectionBox.FormattingEnabled = true;
            this.SectionBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7"});
            this.SectionBox.Location = new System.Drawing.Point(230, 41);
            this.SectionBox.Name = "SectionBox";
            this.SectionBox.Size = new System.Drawing.Size(121, 33);
            this.SectionBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "To be Added:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(225, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 26);
            this.label2.TabIndex = 4;
            this.label2.Text = "Section Number";
            // 
            // OkButt
            // 
            this.OkButt.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButt.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OkButt.Location = new System.Drawing.Point(12, 152);
            this.OkButt.Name = "OkButt";
            this.OkButt.Size = new System.Drawing.Size(96, 38);
            this.OkButt.TabIndex = 5;
            this.OkButt.Text = "Ok";
            this.OkButt.UseVisualStyleBackColor = true;
            this.OkButt.Click += new System.EventHandler(this.OkButt_Click);
            // 
            // CancelButt
            // 
            this.CancelButt.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButt.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelButt.Location = new System.Drawing.Point(306, 152);
            this.CancelButt.Name = "CancelButt";
            this.CancelButt.Size = new System.Drawing.Size(96, 38);
            this.CancelButt.TabIndex = 6;
            this.CancelButt.Text = "Cancel";
            this.CancelButt.UseVisualStyleBackColor = true;
            this.CancelButt.Click += new System.EventHandler(this.CancelButt_Click);
            // 
            // ConfigureResources
            // 
            this.AcceptButton = this.OkButt;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButt;
            this.ClientSize = new System.Drawing.Size(414, 202);
            this.Controls.Add(this.CancelButt);
            this.Controls.Add(this.OkButt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SectionBox);
            this.Controls.Add(this.DropButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DropText);
            this.Name = "ConfigureResources";
            this.Text = "ConfigureResources";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DropText;
        private System.Windows.Forms.Button DropButton;
        private System.Windows.Forms.ComboBox SectionBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button OkButt;
        private System.Windows.Forms.Button CancelButt;
    }
}