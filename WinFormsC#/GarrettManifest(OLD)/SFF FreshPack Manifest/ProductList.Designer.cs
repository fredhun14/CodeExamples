
namespace FreshPackManifest
{
    partial class ProductList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductList));
            this.TheGrid = new System.Windows.Forms.DataGridView();
            this.OKbut = new System.Windows.Forms.Button();
            this.Cancelbut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TheGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // TheGrid
            // 
            this.TheGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.TheGrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.TheGrid.Location = new System.Drawing.Point(12, 12);
            this.TheGrid.Name = "TheGrid";
            this.TheGrid.Size = new System.Drawing.Size(643, 658);
            this.TheGrid.TabIndex = 0;
            // 
            // OKbut
            // 
            this.OKbut.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKbut.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OKbut.Location = new System.Drawing.Point(668, 12);
            this.OKbut.Name = "OKbut";
            this.OKbut.Size = new System.Drawing.Size(115, 32);
            this.OKbut.TabIndex = 1;
            this.OKbut.Text = "OK";
            this.OKbut.UseVisualStyleBackColor = true;
            this.OKbut.Click += new System.EventHandler(this.OKbut_Click);
            // 
            // Cancelbut
            // 
            this.Cancelbut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancelbut.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancelbut.Location = new System.Drawing.Point(668, 638);
            this.Cancelbut.Name = "Cancelbut";
            this.Cancelbut.Size = new System.Drawing.Size(115, 32);
            this.Cancelbut.TabIndex = 2;
            this.Cancelbut.Text = "Cancel";
            this.Cancelbut.UseVisualStyleBackColor = true;
            this.Cancelbut.Click += new System.EventHandler(this.Cancelbut_Click);
            // 
            // ProductList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(795, 682);
            this.Controls.Add(this.Cancelbut);
            this.Controls.Add(this.OKbut);
            this.Controls.Add(this.TheGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductList";
            this.Text = "Product List";
            this.Load += new System.EventHandler(this.ProductList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TheGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView TheGrid;
        private System.Windows.Forms.Button OKbut;
        private System.Windows.Forms.Button Cancelbut;
    }
}