using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace FreshPackManifest
{
    public partial class ConfigureResources : Form
    {
        public ConfigureResources()
        {
            InitializeComponent();
        }

        #region Buttons
        //Button function for adding a new resource to a specified section
        private void DropButton_Click(object sender, EventArgs e)
        {
            try
            {
                string path13 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\FreshpackResources.txt";
                path13 = path13.Replace("\r", "").Replace("\n", "");
                string[] resources = File.ReadAllLines(path13);
                if (Alreadyin(SectionBox.SelectedIndex, DropText.Text) && !ContainsComma(DropText.Text))
                {
                    switch (SectionBox.SelectedIndex)
                    {
                        case 0:
                            if (DropText.Text.Length == 3)
                            {
                                resources[SectionBox.SelectedIndex] = resources[SectionBox.SelectedIndex] + "," + DropText.Text;

                                File.WriteAllLines(path13, resources);
                                var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                                LogUserActivity LUA = new LogUserActivity();
                                LUA.LogActivity(pf._User, "Added Freshpack resource: " + SectionBox.SelectedIndex.ToString() + ", " + DropText.Text);
                                MessageBox.Show("Successfully added!");
                            }
                            else
                            { MessageBox.Show("Section " + SectionBox.SelectedIndex.ToString() + " Must be three characters!"); }
                            break;
                        case 1:
                            if (DropText.Text.Length == 1)
                            {
                                resources[SectionBox.SelectedIndex] = resources[SectionBox.SelectedIndex] + "," + DropText.Text;

                                File.WriteAllLines(path13, resources);
                                var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                                LogUserActivity LUA = new LogUserActivity();
                                LUA.LogActivity(pf._User, "Added Freshpack resource: " + SectionBox.SelectedIndex.ToString() + ", " + DropText.Text);
                                MessageBox.Show("Successfully added!");
                            }
                            else
                            { MessageBox.Show("Section " + SectionBox.SelectedIndex.ToString() + " Must be One Character!"); }
                            break;
                        case 2:
                            if (DropText.Text.Length == 1)
                            {
                                resources[SectionBox.SelectedIndex] = resources[SectionBox.SelectedIndex] + "," + DropText.Text;

                                File.WriteAllLines(path13, resources);
                                var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                                LogUserActivity LUA = new LogUserActivity();
                                LUA.LogActivity(pf._User, "Added Freshpack resource: " + SectionBox.SelectedIndex.ToString() + ", " + DropText.Text);
                                MessageBox.Show("Successfully added!");
                            }
                            else
                            { MessageBox.Show("Section " + SectionBox.SelectedIndex.ToString() + " Must be One characters!"); }
                            break;
                        case 3:
                            if (DropText.Text.Length == 3 || DropText.Text.Length == 4)
                            {
                                resources[SectionBox.SelectedIndex] = resources[SectionBox.SelectedIndex] + "," + DropText.Text;

                                File.WriteAllLines(path13, resources);
                                var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                                LogUserActivity LUA = new LogUserActivity();
                                LUA.LogActivity(pf._User, "Added Freshpack resource: " + SectionBox.SelectedIndex.ToString() + ", " + DropText.Text);
                                MessageBox.Show("Successfully added!");
                            }
                            else
                            { MessageBox.Show("Section " + SectionBox.SelectedIndex.ToString() + " Must be three or four characters!"); }
                            break;
                        case 4:
                            if (DropText.Text.Length == 1)
                            {
                                resources[SectionBox.SelectedIndex] = resources[SectionBox.SelectedIndex] + "," + DropText.Text;

                                File.WriteAllLines(path13, resources);
                                var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                                LogUserActivity LUA = new LogUserActivity();
                                LUA.LogActivity(pf._User, "Added Freshpack resource: " + SectionBox.SelectedIndex.ToString() + ", " + DropText.Text);
                                MessageBox.Show("Successfully added!");
                            }
                            else
                            { MessageBox.Show("Section " + SectionBox.SelectedIndex.ToString() + " Must be one character!"); }
                            break;
                        case 5:
                            if (DropText.Text.Length == 1)
                            {
                                resources[SectionBox.SelectedIndex] = resources[SectionBox.SelectedIndex] + "," + DropText.Text;

                                File.WriteAllLines(path13, resources);
                                var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                                LogUserActivity LUA = new LogUserActivity();
                                LUA.LogActivity(pf._User, "Added Freshpack resource: " + SectionBox.SelectedIndex.ToString() + ", " + DropText.Text);
                                MessageBox.Show("Successfully added!");
                            }
                            else
                            { MessageBox.Show("Section " + SectionBox.SelectedIndex.ToString() + " Must be one character!"); }
                            break;
                        case 6:
                            if (DropText.Text.Length == 1)
                            {
                                resources[SectionBox.SelectedIndex] = resources[SectionBox.SelectedIndex] + "," + DropText.Text;

                                File.WriteAllLines(path13, resources);
                                var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                                LogUserActivity LUA = new LogUserActivity();
                                LUA.LogActivity(pf._User, "Added Freshpack resource: " + SectionBox.SelectedIndex.ToString() + ", " + DropText.Text);
                                MessageBox.Show("Successfully added!");
                            }
                            else
                            { MessageBox.Show("Section " + SectionBox.SelectedIndex.ToString() + " Must be one character!"); }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string path15 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\ErrorLog.txt";
                path15 = path15.Replace("\r", "").Replace("\n", "");
                using (StreamWriter w = File.AppendText(path15))
                {
                    w.WriteLine(" .ToString(): " + ex.ToString() + System.Environment.NewLine + " DATA: " + ex.Data.ToString() + System.Environment.NewLine + " MESSAGE: " + ex.Message);
                };
                MessageBox.Show("Contact your system admin if you are seeing this message" + System.Environment.NewLine + ex.ToString());
            }
        }
        //Remove button click will remove the specified resource from the specified section if it exsists within it
        private void Remove_Click(object sender, EventArgs e)
        {
            string path13 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\FreshpackResources.txt";
            path13 = path13.Replace("\r", "").Replace("\n", "");
            int Section = SectionBox.SelectedIndex;
            string removethis = DropText.Text;
            string[] resources = File.ReadAllLines(path13);
            string[] TheSection = resources[Section].Split(',');
            string[] NewSection = new string[TheSection.Length];
            bool found = false;
            for (int x = 0; x < TheSection.Length; x++)
            {
                if (TheSection[x] != removethis)
                {
                    NewSection[x] = TheSection[x];
                }
                else
                { found = true; }
            }
            if (found)
            {
                resources[Section] = "";
                for (int x = 0; x < NewSection.Length; x++)
                {
                    if (NewSection[x] != "" && NewSection[x] != string.Empty && NewSection[x] != null)
                    {
                        if (x == 0)
                        { resources[Section] = resources[Section] + NewSection[x]; }
                        else
                        { resources[Section] = resources[Section] + "," + NewSection[x]; }

                    }
                }
                File.WriteAllLines(path13, resources);
                var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                LogUserActivity LUA = new LogUserActivity();
                LUA.LogActivity(pf._User, "Removed Freshpack resource: " + Section + ", " + removethis);
                MessageBox.Show("The resource was found in the list and removed!");
            }
            else { MessageBox.Show("The resource entered was not found in the section entered!"); }


        }

        //unlike other forms the ok buttons simply closes the form the changes made are saved as they are made here.
        private void OkButt_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Like in other forms closes the form 
        private void CancelButt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
        #region Helper methods
        //Used to check if a resource already exsists in the list
        bool Alreadyin(int Section, string checkthis)
        {
            bool result = true;
            string path13 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + @"\FreshpackResources.txt";
            path13 = path13.Replace("\r", "").Replace("\n", "");
            string[] resources = File.ReadAllLines(path13);
            string[] TheSection = resources[Section].Split(',');
            for (int x = 0; x < TheSection.Length; x++)
            {
                if (TheSection[x] == checkthis)
                {
                    result = false;
                    MessageBox.Show(checkthis + " is already a part of section " + Section + "!");
                    break;
                }
            }
            return result;
        }
        //checks if the entered resource contains a comma which is not allowed
        bool ContainsComma(string checkthis)
        {
            bool result = false;
            char[] check = checkthis.ToCharArray();

            for (int x = 0; x < check.Length; x++)
            {
                if (check[x] == ',')
                {
                    result = true;
                    MessageBox.Show("Resource cannot contain commas!");
                    break;
                }
            }
            return result;
        }
        #endregion
        //Empty form load 
        private void ConfigureResources_Load(object sender, EventArgs e)
        {

        }
    }
}
