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

namespace FreshPackManifest
{
    public partial class ManageUsers : Form
    {
        public ManageUsers()
        {
            InitializeComponent();
        }
        public int page = 1;

        string[] split;
        #region Form Load
        //loads the users from the text file they are saved decrypts the username and translates the saved user priviledge into the combobox
        //also disables access to modify Admin accounts if the user currently logged in is not an admin
        private void ManageUsers_Load(object sender, EventArgs e)
        {
            PageNumber.Text = page.ToString();
            CheckIfNextAvailable();
            CheckIfPreviousAvailable();
            var pf = Application.OpenForms.OfType<PrimaryForm>().First();
            string path7 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\security.txt";
            path7 = path7.Replace("\r", "").Replace("\n", "");
            split = File.ReadAllText(path7).Split(',');
            LoadPage();
            #region old load
            /*for (int n = 0; n < 75; n++)
            {
                split[n] = split[n].Replace("\r", "").Replace("\n", "");
            }
            for (int f = 0; f < 75; f = f + 3)
            {
                if (f + 2 < 75)
                {
                    Control[] cs = this.Controls.Find("Privilege" + f / 3, true);
                    ComboBox pv = cs[0] as ComboBox;
                    this.Controls["Username" + f / 3].Text = Decryptusername(split[f]);
                    this.Controls["Password" + f / 3].Text = split[f + 1];
                    switch (split[f + 2])
                    {
                        case "10":
                            pv.SelectedIndex = 5;
                            if (pf._SecLevel != 10)
                            {
                                pv.Enabled = false;
                                this.Controls["Username" + f / 3].Enabled = false;
                                this.Controls["Password" + f / 3].Enabled = false;
                            }
                            break;
                        case "5":
                            if(pf._SecLevel != 10 && pf._SecLevel != 5)
                            {
                                pv.Enabled = false;
                                this.Controls["Username" + f / 3].Enabled = false;
                                this.Controls["Password" + f / 3].Enabled = false;
                            }
                            pv.SelectedIndex = 4;
                            break;
                        case "4":
                            pv.SelectedIndex = 3;
                            break;
                        case "3":
                            pv.SelectedIndex = 2;
                            break;
                        case "2":
                            pv.SelectedIndex = 1;
                            break;
                        case "1":
                            pv.SelectedIndex = 0;
                            break;
                    }
                }
              }
            */
            #endregion
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
            string path7 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\security.txt";
            path7 = path7.Replace("\r", "").Replace("\n", "");
            File.WriteAllText(path7, "");

            for (int f = 0; f < split.Length; f = f + 3)
            {
                if (split.Length < f + 2) { break; }
                string newline = split[f] + "," + split[f + 1] + "," + split[f + 2] + ",";
                using (StreamWriter w = File.AppendText(path7))
                {
                    w.WriteLine(newline);
                };
            }
            #region OLD Save
            /*
            string user = "";
            for (int f = 0; f < 25; f++)
            {
                Control[] cs = this.Controls.Find("Privilege" + f, true);
                ComboBox pv = cs[0] as ComboBox;
                user = Encryptusername(this.Controls["Username" + f].Text) + ",";
                user = user + this.Controls["Password" + f].Text + ",";
                switch (pv.SelectedIndex)
                {
                    case 5:
                        user = user + "10,";
                        break;
                    case 4:
                        user = user + "5,";
                        break;
                    case 3:
                        user = user + "4,";
                        break;
                    case 2:
                        user = user + "3,";
                        break;
                    case 1:
                        user = user + "2,";
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
            */
            #endregion
            var pf = Application.OpenForms.OfType<PrimaryForm>().First();
            LogUserActivity LUA = new LogUserActivity();
            LUA.LogActivity(pf._User, "Modified user settings");

            this.Close();

        }
        #endregion
        #region Triggers
        private void CheckIfPreviousAvailable()
        {
            if (page == 1)
            {
                PreviousButton.Visible = false;
                PreviousButton.Enabled = false;
            }
            else
            {
                PreviousButton.Visible = true;
                PreviousButton.Enabled = true;
            }
        }
        private void CheckIfNextAvailable()
        {
            if (page == 5)
            {
                NextButton.Visible = false;
                NextButton.Enabled = false;
            }
            else
            {
                NextButton.Visible = true;
                NextButton.Enabled = true;
            }
        }

        //Once the password is comitted it is Encrypted immediatly so that when it is saved it is encrypted and we do not have to keep track of 
        //which passwords were modified
        //Set to minimum password length of 5 characters and no blank passwords no other complexity rules
        //Verifies there is a username for the password entered else it will set the password to ""
        private void Password_Comitted(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            Control[] ct = this.Controls.Find("Username" + tb.Tag.ToString(), true);
            TextBox tb2 = ct[0] as TextBox;
            Control[] ct2 = this.Controls.Find("Privilege" + tb.Tag.ToString(), true);
            ComboBox cb = ct2[0] as ComboBox;
            if (tb2.Text != "")
            {
                bool flag = true;
                if (tb.Text == "" || tb.Text.Length < 5)
                {
                    MessageBox.Show("Password is not long enough");
                    flag = false;
                    tb.Focus();
                }
                if (flag)
                {
                    tb.Text = Encryptpassword(tb.Text);
                }
            }
            else { tb.Text = ""; cb.SelectedIndex = -1; }

            for (int f = 0; f < 25; f++)
            {
                if ("Password" + f.ToString() == tb.Name)
                {
                    split[f * 3 + (page - 1) * 75 + 1] = tb.Text;
                    break;
                }
            }
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
            var pf = Application.OpenForms.OfType<PrimaryForm>().First();
            ComboBox cb = (ComboBox)sender;
            if (pf._SecLevel != 10 && cb.SelectedIndex == 5)
            {
                cb.SelectedIndex = 0;
            }

            if (pf._SecLevel != 10 && cb.SelectedIndex == 4)
            {
                if (pf._SecLevel != 5)
                {
                    cb.SelectedIndex = 0;
                }
            }
            for (int f = 0; f < 25; f++)
            {
                if ("Privilege" + f.ToString() == cb.Name)
                {
                    switch (cb.SelectedIndex)
                    {
                        case 3:
                            split[f * 3 + (page - 1) * 75 + 2] = "10";//admin
                            break;
                        case 2:
                            split[f * 3 + (page - 1) * 75 + 2] = "5";//manager
                            break;
                        case 1:
                            split[f * 3 + (page - 1) * 75 + 2] = "3";//qa super
                            break;
                        case 0:
                            split[f * 3 + (page - 1) * 75 + 2] = "1";// user
                            break;
                        default:
                            split[f * 3 + (page - 1) * 75 + 2] = "";
                            break;
                    }
                    break;
                }
            }
        }
        //Used to verify that a username is not already in use to prevent priviledge issues
        private void Username_Comitted(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            bool flag = false;
            for (int f = 0; f < 25; f++)
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
            if (tb.Text == "" || tb.Text == string.Empty || tb.Text == null)
            {
                if (this.ActiveControl is TextBox)
                {
                    Control[] ct = this.Controls.Find("Password" + this.ActiveControl.Tag.ToString(), true);
                    TextBox tb2 = ct[0] as TextBox;
                    Control[] ct2 = this.Controls.Find("Privilege" + this.ActiveControl.Tag.ToString(), true);
                    ComboBox cb = ct2[0] as ComboBox;
                    tb2.Text = "";
                    cb.SelectedIndex = -1;
                }
            }
            for (int f = 0; f < 25; f++)
            {
                if ("Username" + f.ToString() == tb.Name)
                {
                    split[f * 3 + (page - 1) * 75] = Encryptusername(tb.Text);
                    break;
                }
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
            username = username.ToLower();
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
        private void Enableall()
        {
            for (int f = 0; f < 25; f++)
            {
                Control[] ct1 = this.Controls.Find("Username" + f.ToString(), true);
                TextBox tb1 = ct1[0] as TextBox;
                Control[] ct = this.Controls.Find("Password" + f.ToString(), true);
                TextBox tb2 = ct[0] as TextBox;
                Control[] ct2 = this.Controls.Find("Privilege" + f.ToString(), true);
                ComboBox cb = ct2[0] as ComboBox;
                tb1.Enabled = true;
                tb2.Enabled = true;
                cb.Enabled = true;
            }
        }
        private void PreviousButton_Click(object sender, EventArgs e)
        {
            page = page - 1;
            PageNumber.Text = page.ToString();
            CheckIfPreviousAvailable();
            CheckIfNextAvailable();
            Enableall();
            LoadPage();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            page = page + 1;
            PageNumber.Text = page.ToString();
            CheckIfNextAvailable();
            CheckIfPreviousAvailable();
            Enableall();
            LoadPage();
        }
        private void LoadPage()
        {
            var pf = Application.OpenForms.OfType<PrimaryForm>().First();

            for (int n = 0; n < split.Length; n++)
            {
                split[n] = split[n].Replace("\r", "").Replace("\n", "");
            }
            for (int f = 75 * (page - 1); f < 75 * page; f = f + 3)
            {
                if (f + 2 < 75 * page)
                {
                    Control[] cs = this.Controls.Find("Privilege" + (f - (page - 1) * 75) / 3, true);
                    ComboBox pv = cs[0] as ComboBox;
                    this.Controls["Username" + (f - (page - 1) * 75) / 3].Text = Decryptusername(split[f]);
                    this.Controls["Password" + (f - (page - 1) * 75) / 3].Text = split[f + 1];
                    switch (split[f + 2])
                    {
                        case "10"://admin
                            pv.SelectedIndex = 3;
                            if (pf._SecLevel != 10)
                            {
                                pv.Enabled = false;
                                this.Controls["Username" + (f - (page - 1) * 75) / 3].Enabled = false;
                                this.Controls["Password" + (f - (page - 1) * 75) / 3].Enabled = false;
                            }
                            break;
                        case "5"://manager
                            if (pf._SecLevel != 10 && pf._SecLevel != 5)
                            {
                                pv.Enabled = false;
                                this.Controls["Username" + (f - (page - 1) * 75) / 3].Enabled = false;
                                this.Controls["Password" + (f - (page - 1) * 75) / 3].Enabled = false;
                            }
                            pv.SelectedIndex = 2;
                            break;
                        case "3"://qa super
                            pv.SelectedIndex = 1;
                            break;
                        case "1"://user
                            pv.SelectedIndex = 0;
                            break;
                        default:
                            pv.SelectedIndex = -1;
                            break;
                    }
                }
            }
        }
    }
}

