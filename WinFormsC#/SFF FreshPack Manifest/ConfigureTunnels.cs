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

namespace FreshPackManifest
{
    public partial class ConfigureTunnels : Form
    {
        public ConfigureTunnels()
        {
            InitializeComponent();
        }
        string[] Tunnel_List = new string[1000];
        int TunnelCount = 0;
        private void OKBut_Click(object sender, EventArgs e)
        {
            string path8 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Tunnels.txt";
            path8 = path8.Replace("\r", "").Replace("\n", "");
            File.WriteAllText(path8,"");
            string Tunnels = "";
            for (int x = 0; x < Tunnel_List.Length; x++)
            {
                if (Tunnel_List[x] != "" && Tunnel_List[x] != string.Empty && Tunnel_List[x] != null)
                {
                    Tunnels = Tunnels + Tunnel_List[x] + "╨";
                }
            }
            Tunnels = Tunnels.Substring(0, Tunnels.Length - 1);
            File.WriteAllText(path8, Tunnels);
            this.Close();
        }

        private void CancelBut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddBut_Click(object sender, EventArgs e)
        {
            bool alreadyaTunnel = false;
            for (int x = 0; x < TunnelCount; x++)
            {
                if(EntryBox.Text == Tunnel_List[x])
                {
                    alreadyaTunnel = true;
                    break;
                }
            }
            if(alreadyaTunnel)
            {
                MessageBox.Show("Tunnel already exsists!");
            }
            else
            {
                Tunnel_List[TunnelCount] = EntryBox.Text;
                TunnelCount++;
                MessageBox.Show(EntryBox.Text + " has been added to the tunnel list!");
                Tunnel.Items.Clear();
                for (int x = 0; x < TunnelCount; x++)
                {
                    Tunnel.Items.Add(Tunnel_List[x]);
                }

                var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                LogUserActivity LUA = new LogUserActivity();
                LUA.LogActivity(pf._User, "Added Tunnel " + EntryBox.Text);
            }
        }

        private void RemoveBut_Click(object sender, EventArgs e)
        {
            bool alreadyaTunnel = false;
            int found = 0;
            for (int x = 0; x < TunnelCount; x++)
            {
                if (EntryBox.Text == Tunnel_List[x])
                {
                    alreadyaTunnel = true;
                    found = x;
                    break;
                }
            }
            if (alreadyaTunnel)
            {
                Tunnel_List[found] = "";
                Tunnel.Items.Clear();
                for (int x = 0; x < TunnelCount; x++)
                {
                    if (Tunnel_List[x] != "")
                    {
                        Tunnel.Items.Add(Tunnel_List[x]);
                    }
                }
                TunnelCount--;
                MessageBox.Show(EntryBox.Text + " has been removed!");

                var pf = Application.OpenForms.OfType<PrimaryForm>().First();
                LogUserActivity LUA = new LogUserActivity();
                LUA.LogActivity(pf._User,"Removed Tunnel " + EntryBox.Text);
            }
            else
            {
                MessageBox.Show("Tunnel does not exsist!");
            }
        }

        private void ConfigureTunnels_Load(object sender, EventArgs e)
        {
            string path8 = File.ReadAllText(Application.StartupPath + @"\ConfigurationPathFile.txt") + "\\Tunnels.txt";
            path8 = path8.Replace("\r", "").Replace("\n", "");
            string[] tunnels = File.ReadAllText(path8).Split('╨');
            Tunnel.Items.AddRange(tunnels);
            for(int x = 0; x < tunnels.Length; x ++)
            {
                tunnels[x] = tunnels[x].Replace("\r", "").Replace("\n", "");
                Tunnel_List[x] = tunnels[x];
                TunnelCount++;
            }
        }
    }
}
