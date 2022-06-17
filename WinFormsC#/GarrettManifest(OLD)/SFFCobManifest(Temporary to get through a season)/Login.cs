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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        #region Buttons
        //closes the form not changing user if this is the first time login then this will asign the 0 seclevel and hide almost all functions
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Checks entered data against the list of credentials in the security file if any are found to match the form will close setting the appropriate 
        //security level for that user hiding/showing controls as elsewhere programmed
        //if no match is found then a messagebox appears stating such and the form does not close
        private void LoginButton_Click(object sender, EventArgs e)
        {
            var pf = Application.OpenForms.OfType<CobManifest2021>().First();
            string path7 = Application.StartupPath + "\\security.txt";
            path7 = path7.Replace("\r", "").Replace("\n", "");
            string fullfile = File.ReadAllText(path7);
            string[] split = fullfile.Split(',');
            bool flag = false;
            for (int n = 0; n < split.Length; n++)
            {
                split[n] = split[n].Replace("\r", "").Replace("\n", "");
            }
            for (int f = 0; f < split.Length && flag == false; f = f + 3)
            {
                if (Encryptusername(UsernametextBox.Text.ToLower()) == split[f] && UsernametextBox.Text != "")
                {
                    if(Encryptpassword(PasswordtextBox.Text) == split[f+1])
                    {
                        pf._SecLevel = Int32.Parse(split[f + 2]);
                        flag = true;
                    }
                }
            }
            if (flag) 
            { 
            //    this.Close(); 
            }
            else if (!flag) { MessageBox.Show("Either your username or password were inccorect!"); DialogResult = DialogResult.None; }

        }
        #endregion
        #region Encryotion/Decryotion
        //Used to encrypt the entered username in order to check it against the saved one in the text file
        string Encryptusername(string username)
        {
            username.ToLower();
            char[] cusername = username.ToCharArray();
            for(int f = 0; f < cusername.Length; f++)
            {
                cusername[f] += (char) 5;
            }
            string eusername = new string(cusername);
            return eusername;
        }
        //unused but could be used to decrypt the username in the 
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
        //used to hash the entered password in order to check it against the saved hash for the user uses MD5 hashing algorithm because it is faster and no
        //extreme security concerns are present
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
        #endregion
    }
}
