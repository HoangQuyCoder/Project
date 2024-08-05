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

        public ClientForm()
        {
            InitializeComponent();
        }

        private void btnStartSharing_Click(object sender, EventArgs e)
        {
            StartClient();
        }

        private void StartClient()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 8000); // Kết nối tới server
                networkStream = client.GetStream();
                Timer timer = new Timer();
                timer.Interval = 1000; // Gửi ảnh mỗi giây
                timer.Tick += (s, ev) => SendScreenshot();
                timer.Start();
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
                Graphics g = Graphics.FromImage(bmp);
                g.CopyFromScreen(0, 0, 0, 0, bmp.Size);

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
            if (client != null)
            {
                client.Close();
            }
        }
    }
}
