using ASCOM.Utilities;
using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ASCOM.TLTinylibHttpCam01.Camera
{
    [ComVisible(false)] // Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        const string NO_PORTS_MESSAGE = "No COM ports found";
        TraceLogger tl; // Holder for a reference to the driver's trace logger

        public SetupDialogForm(TraceLogger tlDriver)
        {
            InitializeComponent();

            // Save the provided trace logger for use within the setup dialogue
            tl = tlDriver;

            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void CmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here and update the state variables with results from the dialogue

            tl.Enabled = chkTrace.Checked;
            var ipaddr = "127.0.0.1";
            if (IPAddress.TryParse(ipaddrBox.Text, out IPAddress address))
            {
                switch (address.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        ipaddr = address.ToString();
                        // This is IPv4 address
                        break;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        ipaddr = "["+address.ToString()+"]";
                        // This is IPv6 address
                        break;
                    default:
                        break;
                }
            }

            var port = "5000";
            if (int.TryParse(portBox.Text, out int port_))
            {
                if (port_ > 0 && port_ < 65535)
                {
                    port = port_.ToString();
                }
            }

            CameraHardware.httpPort = port;
            CameraHardware.ipAddr = ipaddr;

            tl.LogMessage("Setup OK", $"New configuration values - Server: {ipaddrBox.Text}:{portBox.Text}");
        }

        private void CmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("https://ascom-standards.org/");
            }
            catch (Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {

            // Set the trace checkbox
            chkTrace.Checked = tl.Enabled;

            ipaddrBox.Text = CameraHardware.ipAddr;
            portBox.Text = CameraHardware.httpPort;

            tl.LogMessage("InitUI", $"Set UI controls to Trace: {chkTrace.Checked}, Server: {ipaddrBox.Text}:{portBox.Text}");
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            // Bring the setup dialogue to the front of the screen
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
            else
            {
                TopMost = true;
                Focus();
                BringToFront();
                TopMost = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}