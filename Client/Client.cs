using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
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
            ConnectClient();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            DisconnectClient();
        }

        private void DisconnectClient()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }

            // Gửi tín hiệu ngừng chia sẻ tới server
            if (networkStream != null)
            {
                try
                {
                    byte[] stopSignal = Encoding.ASCII.GetBytes("STOP_SHARING");
                    networkStream.Write(stopSignal, 0, stopSignal.Length);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending stop signal: " + ex.Message);
                }
                finally
                {
                    networkStream.Close();
                    networkStream = null;
                }
            }

            if (client != null)
            {
                client.Close();
                client = null;
                MessageBox.Show("Disconnected from server.");
            }
            else
            {
                MessageBox.Show("Not connected to the server.");
            }
        }

        private void btnStartSharing_Click(object sender, EventArgs e)
        {
            if (client != null && networkStream != null)
            {
                StartSharing();
            }
            else
            {
                MessageBox.Show("Please connect to a server first.");
            }
        }

        private void StartSharing()
        {
            if (timer == null)
            {
                timer = new Timer();
                timer.Interval = 1000; // Send screenshot every second
                timer.Tick += (s, ev) => SendScreenshot();
                timer.Start();
            }
        }


        private void btnStopSharing_Click(object sender, EventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;

                // Send stop signal to server
                if (networkStream != null)
                {
                    byte[] stopSignal = Encoding.ASCII.GetBytes("STOP_SHARING");
                    networkStream.Write(stopSignal, 0, stopSignal.Length);
                }

                MessageBox.Show("Screen sharing stopped.");
            }
            else
            {
                MessageBox.Show("Screen sharing is not currently running.");
            }
        }

        private void ConnectClient()
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
                // Kiểm tra xem networkStream có được khởi tạo không
                if (networkStream == null || !client.Connected)
                {
                    MessageBox.Show("Network stream is not initialized or the client is not connected.");
                    StopSharing();
                    return;
                }

                // Chụp ảnh màn hình
                Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, bmp.Size);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] buffer = ms.ToArray();

                    // Kiểm tra xem buffer có hợp lệ không
                    if (buffer != null && buffer.Length > 0)
                    {
                        networkStream.Write(buffer, 0, buffer.Length);
                    }
                    else
                    {
                        MessageBox.Show("Screenshot data is empty.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending screenshot: " + ex.Message);
                StopSharing(); // Stop sharing on error
            }
        }

        private void StopSharing()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }

            MessageBox.Show("Screen sharing stopped.");
        }




        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisconnectClient();
        }
    }
}
