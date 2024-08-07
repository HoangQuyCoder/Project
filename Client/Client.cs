using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ScreenSharingClient
{
    public partial class ClientForm : Form
    {
        private TcpClient client;
        private NetworkStream networkStream;
        private Timer timer;

        public ClientForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            StartClient();
        }

        private void btnStartSharing_Click(object sender, EventArgs e)
        {
            if (client != null && networkStream != null)
            {
                timer = new Timer();
                timer.Interval = 1000; // Send screenshot every second
                timer.Tick += (s, ev) => SendScreenshot();
                timer.Start();
            }
            else
            {
                MessageBox.Show("Please connect to a server first.");
            }
        }

        private void StartClient()
        {
            try
            {
                string ip = txt_IP.Text;
                int port = int.Parse(txt_PORT.Text);
                client = new TcpClient(ip, port); // Connect to server
                networkStream = client.GetStream();
                MessageBox.Show("Connected to server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting client: " + ex.Message);
            }
        }

        private void SendScreenshot()
        {
            try
            {
                Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, bmp.Size);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] buffer = ms.ToArray();
                    networkStream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending screenshot: " + ex.Message);
            }
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
            }
            if (client != null)
            {
                client.Close();
            }
        }
    }
}
