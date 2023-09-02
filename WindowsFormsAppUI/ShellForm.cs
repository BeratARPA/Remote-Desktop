using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppUI
{
    public partial class ShellForm : Form
    {
        public ShellForm()
        {
            InitializeComponent();
            textBoxYourIP.Text = GetMyIP();
        }

        private async void buttonConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxClientIP.Text) || string.IsNullOrEmpty(textBoxUserName.Text) || string.IsNullOrEmpty(textBoxPassword.Text))
            {
                MessageBox.Show("Enter the required information!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            rdpClient.Server = textBoxClientIP.Text;
            rdpClient.UserName = textBoxUserName.Text;
            rdpClient.AdvancedSettings2.ClearTextPassword = textBoxPassword.Text;
            rdpClient.AdvancedSettings8.EnableCredSspSupport = true;

            //Optional
            rdpClient.ColorDepth = 32;
            rdpClient.DesktopWidth = rdpClient.Width;
            rdpClient.DesktopHeight = rdpClient.Height;
            rdpClient.AdvancedSettings3.SmartSizing = true;

            rdpClient.Connect();

            await Task.Delay(2000);

            if (rdpClient.Connected.ToString() == "1")
            {
                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;
            }
            else
            {
                buttonConnect.Enabled = true;
                buttonDisconnect.Enabled = false;
            }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if (rdpClient.Connected.ToString() == "1")
            {
                rdpClient.Disconnect();
                buttonConnect.Enabled = true;
                buttonDisconnect.Enabled = false;
            }
        }

        public string GetMyIP()
        {
            var hostEntry = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var host in hostEntry.AddressList)
            {
                if (host.AddressFamily == AddressFamily.InterNetwork)
                {
                    return host.ToString();
                }
            }

            return "0.0.0.0";
        }
    }
}
