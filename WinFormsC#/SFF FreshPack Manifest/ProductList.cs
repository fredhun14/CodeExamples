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
//   ╨ is ALT + 2256

namespace FreshPackManifest
{
    public partial class ProductList : Form
    {
        public ProductList()
        {
            InitializeComponent();
        }

        private void Cancelbut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OKbut_Click(object sender, EventArgs e)
        {
            string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\DescriptionDatabase.txt";
            path15 = path15.Replace("\r", "").Replace("\n", "");
            string[,] data = new string[TheGrid.Rows.Count, TheGrid.Columns.Count];

            for (int rows = 0; rows < TheGrid.Rows.Count; rows++)
            {
                for (int col = 0; col < TheGrid.Columns.Count; col++)
                {
                    if (TheGrid.Rows[rows].Cells[col].Value != null && TheGrid.Rows[rows].Cells[col].Value != DBNull.Value && !String.IsNullOrWhiteSpace(TheGrid.Rows[rows].Cells[col].Value.ToString()))
                    {
                        data[rows, col] = TheGrid.Rows[rows].Cells[col].Value.ToString();
                    }
                    else { data[rows, col] = ""; }
                }
            }
            File.WriteAllText(path15, "");
            for (int rows = 0; rows < TheGrid.Rows.Count - 1; rows++)
            {
                for (int col = 0; col < TheGrid.Columns.Count; col++)
                {
                    using (StreamWriter sw = File.AppendText(path15))
                    { sw.Write(data[rows, col] + "╨"); }
                }
            }
            var pf = Application.OpenForms.OfType<PrimaryForm>().First();
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(pf._User, "Modified Product descriptions");
            this.Close();
        }

        private void ProductList_Load(object sender, EventArgs e)
        {
            string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\DescriptionDatabase.txt";
            path15 = path15.Replace("\r", "").Replace("\n", "");
            string[] data = File.ReadAllText(path15).Split('╨');
            DataTable dt = new DataTable();
            DataRow row;
            DataColumn column1 = new DataColumn(), column2 = new DataColumn();
            column1.DataType = System.Type.GetType("System.String");
            column2.DataType = System.Type.GetType("System.String");
            column1.ColumnName = "Resource";
            column2.ColumnName = "Description";
            dt.Columns.Add(column1);
            dt.Columns.Add(column2);
            TheGrid.DataSource = dt;
            for (int f = 0; f < data.Length - 1; f = f + 2)
            {
                row = dt.NewRow();
                row["Resource"] = data[f];
                row["Description"] = data[f + 1];
                dt.Rows.Add(row);
            }
            TheGrid.Columns[0].Width = 100;
            TheGrid.Columns[1].Width = 500;

        }
    }
}
