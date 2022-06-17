using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace GarrettManifester
{
    public partial class ManageUsers : Form
    {
        public ManageUsers()
        {
            InitializeComponent();
        }
        #region Form Load
        //loads the users from the text file they are saved decrypts the username and translates the saved user priviledge into the combobox
        //also disables access to modify Admin accounts if the user currently logged in is not an admin
        private void ManageUsers_Load(object sender, EventArgs e)
        {
            var pf = Application.OpenForms.OfType<CobManifest2021>().First();
            string path7 = Application.StartupPath + "\\security.txt";
            path7 = path7.Replace("\r", "").Replace("\n", "");
            string fullfile = File.ReadAllText(path7);
            string[] split = fullfile.Split(',');
            for(int n = 0; n < split.Length; n++)
            {
                split[n] = split[n].Replace("\r", "").Replace("\n", "");
            }
            for (int f = 0; f < split.Length; f = f + 3)
            {
                if(f + 2 < split.Length)
                {
                    Control[] cs = this.Controls.Find("Privilege" + f / 3, true);
                    ComboBox pv = cs[0] as ComboBox;
                    this.Controls["Username" + f / 3].Text = Decryptusername(split[f]);
                    this.Controls["Password" + f / 3].Text = split[f + 1];
                    switch(split[f+2])
                    {
                        case "10":
                            pv.SelectedIndex = 2;
                            if (pf._SecLevel != 10)
                            {
                                pv.Enabled = false;
                                this.Controls["Username" + f / 3].Enabled = false;
                                this.Controls["Password" + f / 3].Enabled = false;
                            }
                            break;
                        case "5":
                            pv.SelectedIndex = 1;
                            break;
                        case "1":
                            pv.SelectedIndex = 0;
                            break;
                    }
                }
            }


        }
        #endregion
        #region Button Clicks
        //Closes the form without saving any changes made
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Closes the form saving all changes made to the list of users
        private void Accept_Click(object sender, EventArgs e)
        {
            string path7 = Application.StartupPath + "\\security.txt";
            path7 = path7.Replace("\r", "").Replace("\n", "");
            string user = "";
            File.WriteAllText(path7, "");
            for(int f = 0; f < 25; f ++)
            {
                Control[] cs = this.Controls.Find("Privilege" + f, true);
                ComboBox pv = cs[0] as ComboBox;
                user = Encryptusername(this.Controls["Username" + f].Text) + ",";
                user = user + this.Controls["Password" + f].Text + ",";
                switch (pv.SelectedIndex)
                {
                    case 2:
                        user = user + "10,";
                        break;
                    case 1:
                        user = user + "5,";
                        break;
                    case 0:
                        user = user + "1,";
                        break;
                    default:
                        user = user + ",";
                        break;
                }
                using (StreamWriter w = File.AppendText(path7))
                {
                    w.WriteLine(user);
                };
            }
            this.Close();
        }
        #endregion
        #region Triggers
        //Once the password is comitted it is Encrypted immediatly so that when it is saved it is encrypted and we do not have to keep track of 
        //which passwords were modified
        //Set to minimum password length of 5 characters and no blank passwords no other complexity rules
        //Verifies there is a username for the password entered else it will set the password to ""
        private void Password_Comitted(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            Control[] ct = this.Controls.Find("Username" + this.ActiveControl.Tag.ToString(), true);
            TextBox tb2 = ct[0] as TextBox;
            Control[] ct2 = this.Controls.Find("Privilege" + this.ActiveControl.Tag.ToString(), true);
            ComboBox cb = ct2[0] as ComboBox;
            if (tb2.Text != "")
            {
                bool flag = true;
                if (tb.Text == "" || tb.Text.Length < 5)
                {
                    MessageBox.Show("Passowrd is not long enough");
                    flag = false;
                    tb.Focus();
                }
                if (flag)
                {
                    tb.Text = Encryptpassword(tb.Text);
                }
            }
            else { tb.Text = ""; cb.SelectedIndex = -1; }
        }
        //Empties the password box so the user does not need to and so that they know they need to fully enter a new password 
        //if they intend on saving their changes
        private void Password_Entered(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = "";
        }
        //this is to ensure that the user can not elevate an account to admin if they are not logged into an admin account it will immiditly change the 
        //privilege to a user if it is attempted by a non-admin
        private void Changed_user_Privilege(object sender, EventArgs e)
        {
            var pf = Application.OpenForms.OfType<CobManifest2021>().First();
            ComboBox cb = (ComboBox)sender;
            if (pf._SecLevel != 10 && cb.SelectedIndex == 2)
            {
                cb.SelectedIndex = 0;
            }
        }
        //Used to verify that a username is not already in use to prevent priviledge issues
        private void Username_Comitted(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            bool flag = false;
            for (int f = 0;f < 25; f++)
            {
                if (tb.Text == this.Controls["Username" + f.ToString()].Text && tb.Text != "" && tb.Name != this.Controls["Username" + f.ToString()].Name)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                tb.Text = "";
                MessageBox.Show("Username already in use");
                tb.Focus();
            }
            if (tb.Text == "" || tb.Text == string.Empty || tb.Text == null )
            {
                Control[] ct = this.Controls.Find("Password" + this.ActiveControl.Tag.ToString(), true);
                TextBox tb2 = ct[0] as TextBox;
                Control[] ct2 = this.Controls.Find("Privilege" + this.ActiveControl.Tag.ToString(), true);
                ComboBox cb = ct2[0] as ComboBox;
                tb2.Text = "";
                cb.SelectedIndex = -1;
            }
        }
        #endregion
        #region Encryption/Decryption
        //Uses MD5 algorithm to hash the password for storage
        string Encryptpassword(string password)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
        //Encrypts the username for storage
        string Encryptusername(string username)
        {
            username.ToLower();
            char[] cusername = username.ToCharArray();
            for (int f = 0; f < cusername.Length; f++)
            {
                cusername[f] += (char)5;
            }
            string eusername = new string(cusername);
            return eusername;
        }
        //Decrypts the username for viewing
        string Decryptusername(string username)
        {
            char[] cusername = username.ToCharArray();
            for (int f = 0; f < cusername.Length; f++)
            {
                cusername[f] -= (char)5;
            }
            string dusername = new string(cusername);
            return dusername;
        }
        #endregion
    }
}
