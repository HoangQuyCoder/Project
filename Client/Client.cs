using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ScreenSharingClient
{
    public partial class ClientForm : Form
    {
        private UdpClient udpClient;
        private IPEndPoint serverEndPoint;
        private Timer timer;

        public ClientForm()
        {
            InitializeComponent();
            btnStopSharing.Enabled = false; // Initially disable the Stop Sharing button
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
                StopSharing(); // Ensure sharing is stopped before disconnecting

                if (udpClient != null)
                {
                    udpClient.Close();
                    udpClient = null;
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
                MessageBox.Show("Error disconnecting client: " + ex.Message);
            }
        }

        private void btnStartSharing_Click(object sender, EventArgs e)
        {
            StartSharing();
        }

        private void StartSharing()
        {
            if (udpClient != null && timer == null)
            {
                timer = new Timer();
                timer.Interval = 500; // Send screenshot every half second
                timer.Tick += (s, ev) => SendScreenshot();
                timer.Start();
                btnStartSharing.Enabled = false; // Disable Start Sharing button
                btnStopSharing.Enabled = true; // Enable Stop Sharing button
                MessageBox.Show("Screen sharing started.");
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

                    if (udpClient != null)
                    {
                        byte[] stopSignal = Encoding.ASCII.GetBytes("STOP_SHARING");
                        udpClient.Send(stopSignal, stopSignal.Length, serverEndPoint);
                    }
                    btnStartSharing.Enabled = true; // Enable Start Sharing button
                    btnStopSharing.Enabled = false; // Disable Stop Sharing button
                    MessageBox.Show("Screen sharing stopped.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping screen sharing: " + ex.Message);
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
                serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                udpClient = new UdpClient();

                // Send a "PING" message to the server
                byte[] pingMessage = Encoding.ASCII.GetBytes("PING");
                udpClient.Send(pingMessage, pingMessage.Length, serverEndPoint);

                // Set a timeout for receiving the response
                udpClient.Client.ReceiveTimeout = 2000; // 2 seconds

                try
                {
                    // Try to receive a "PONG" response from the server
                    byte[] response = udpClient.Receive(ref serverEndPoint);
                    string responseMessage = Encoding.ASCII.GetString(response);

                    if (responseMessage == "PONG")
                    {
                        // Successfully connected
                        btnConnect.Enabled = false;
                        btnDisconnect.Enabled = true;
                        btnStartSharing.Enabled = true;
                        txt_IP.Enabled = false;
                        txt_PORT.Enabled = false;
                        MessageBox.Show("Connected to server.");
                    }
                    else
                    {
                        // Unexpected response
                        MessageBox.Show("Unexpected response from server.");
                    }
                }
                catch (SocketException)
                {
                    // Timeout or other network error
                    MessageBox.Show("Failed to connect to server. Server may not be running.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting client: " + ex.Message);
            }
        }

        private void SendScreenshot()
        {
            try
            {
                // Capture the screenshot
                Bitmap bmp = new Bitmap(1920, 1080);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); // Using JPEG to reduce size
                    byte[] buffer = ms.ToArray();

                    int chunkSize = 65000; // Each chunk is 65 KB
                    int totalChunks = (int)Math.Ceiling((double)buffer.Length / chunkSize);

                    for (int i = 0; i < totalChunks; i++)
                    {
                        int currentChunkSize = Math.Min(chunkSize, buffer.Length - (i * chunkSize));
                        byte[] chunkData = new byte[currentChunkSize];
                        Array.Copy(buffer, i * chunkSize, chunkData, 0, currentChunkSize);

                        udpClient.Send(chunkData, chunkData.Length, serverEndPoint);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending screenshot: " + ex.Message);
            }
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisconnectClient();
        }

        private void lbl_PORT_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ClientForm_Load(object sender, EventArgs e)
        {

        }
    }
}
