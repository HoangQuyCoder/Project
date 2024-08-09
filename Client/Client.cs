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
            try
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
                    btnConnect.Enabled = true;
                    btnDisconnect.Enabled = false;
                    btnStartSharing.Enabled = false;
                    btnStopSharing.Enabled = false;
                    txt_IP.Enabled = true;
                    txt_PORT.Enabled = true;
                    MessageBox.Show("Disconnected from server.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error disconnect client: " + ex.Message);
            }
        }

        private void btnStartSharing_Click(object sender, EventArgs e)
        {
            try
            {
                if (client != null && networkStream != null)
                {
                    StartSharing();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error startsharing client: " + ex.Message);
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
                btnStartSharing.Enabled = false;
                btnStopSharing.Enabled = true;
            }
        }

        private void StopSharing()
        {
            try
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
                    btnStartSharing.Enabled = true;
                    btnStopSharing.Enabled = false;
                    MessageBox.Show("Screen sharing stopped.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopsharing client: " + ex.Message);
            }
        }

        private void btnStopSharing_Click(object sender, EventArgs e)
        {
            StopSharing();
        }

        private void ConnectClient()
        {
            try
            {
                string ip = txt_IP.Text;
                int port = int.Parse(txt_PORT.Text);
                client = new TcpClient(ip, port); // Connect to server
                networkStream = client.GetStream();

                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                btnStartSharing.Enabled = true;
                txt_IP.Enabled = false;
                txt_PORT.Enabled = false;
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
                Bitmap bmp = new Bitmap(1920, 1200);
                using (Graphics g = Graphics.FromImage(bmp))
                {

                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, new Size(bmp.Width, bmp.Height));
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

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisconnectClient();
        }
    }
}
