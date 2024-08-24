using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.IO.Compression;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScreenSharingClient
{
    public partial class ClientForm : Form
    {
        private UdpClient udpClient;
        private IPEndPoint serverEndPoint;
        private Thread screenSharingThread;
        private bool isSharing;
        private List<int> clientIds = new List<int>();
        private int clientId;
        private int nextClientId = 1; // Bắt đầu từ 1

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

        private void btnStartSharing_Click(object sender, EventArgs e)
        {
            StartSharing();
        }

        private void btnStopSharing_Click(object sender, EventArgs e)
        {
            StopSharing();
        }

        private void StartSharing()
        {
            try
            {
                if (udpClient != null && screenSharingThread == null)
                {
                    isSharing = true;

                    screenSharingThread = new Thread(() =>
                    {
                        while (isSharing)
                        {
                            Task.Run(() => SendScreenshot(clientId));
                            Thread.Sleep(100); 
                        }
                    });
                    screenSharingThread.IsBackground = true;
                    screenSharingThread.Start();

                    btnStartSharing.Enabled = false; // Disable Start Sharing button
                    btnStopSharing.Enabled = true; // Enable Stop Sharing button
                    MessageBox.Show("Screen sharing started.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting screen sharing: " + ex.Message);
            }
        }
        private void StopSharing()
        {
            try
            {
                if (screenSharingThread != null)
                {
                    isSharing = false;
                    screenSharingThread.Join(); // Wait for the thread to finish
                    screenSharingThread = null;

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

        private void ConnectClient()
        {
            try
            {
                // Kiểm tra và lấy giá trị IP và PORT
                if (txt_IP == null || txt_PORT == null)
                {
                    MessageBox.Show("IP or Port controls are not initialized.");
                    return;
                }

                string ip = txt_IP.Text;
                if (!int.TryParse(txt_PORT.Text, out int port))
                {
                    MessageBox.Show("Invalid port number.");
                    return;
                }

                // Khởi tạo serverEndPoint
                serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                clientId = nextClientId++;
                clientIds.Add(clientId);

                // Khởi tạo UdpClient nếu chưa được khởi tạo
                if (udpClient == null)
                {
                    udpClient = new UdpClient();
                }

                // Gửi thông điệp "PING" đến server
                byte[] pingMessage = Encoding.ASCII.GetBytes("PING");
                udpClient.Send(pingMessage, pingMessage.Length, serverEndPoint);

                // Lấy port mà UdpClient đang sử dụng
                if (udpClient.Client.LocalEndPoint != null)
                {
                    int localPort = ((IPEndPoint)udpClient.Client.LocalEndPoint).Port;
                    lblPort.Text = $"Your Port: {localPort}"; // Cập nhật lblPort trên form
                }
                else
                {
                    MessageBox.Show("LocalEndPoint is not initialized.");
                }

                // Thiết lập timeout cho việc nhận phản hồi
                udpClient.Client.ReceiveTimeout = 2000;

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
                    txt_IP.Enabled = true;
                    txt_PORT.Enabled = true;
                    btnDisconnect.Enabled = false;
                    btnStartSharing.Enabled = false;
                    btnStopSharing.Enabled = false;
                    MessageBox.Show("Disconnected from server.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error disconnecting client: " + ex.Message);
            }
        }

        private void SendScreenshot(int clientId)
        {
            try
            {
                using (Bitmap bmp = new Bitmap(1920, 1080))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(0, 0, 0, 0, new Size(bmp.Width, bmp.Height));
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        var jpegEncoder = GetEncoder(ImageFormat.Jpeg);
                        var encoderParameters = new EncoderParameters(1);
                        encoderParameters.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 75L);
                        bmp.Save(ms, jpegEncoder, encoderParameters);

                        byte[] buffer = ms.ToArray();

                        // Compress data
                        using (var compressedStream = new MemoryStream())
                        {
                            using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                            {
                                gzipStream.Write(buffer, 0, buffer.Length);
                            }
                            byte[] compressedData = compressedStream.ToArray();

                            int chunkSize = 65000 - 8; // Reduced chunk size to accommodate clientId and sequence number
                            int totalChunks = (int)Math.Ceiling((double)compressedData.Length / chunkSize);

                            for (int i = 0; i < totalChunks; i++)
                            {
                                int currentChunkSize = Math.Min(chunkSize, compressedData.Length - (i * chunkSize));
                                byte[] chunkData = new byte[currentChunkSize + 8];

                                // Prepend clientId and sequence number
                                BitConverter.GetBytes(clientId).CopyTo(chunkData, 0);
                                BitConverter.GetBytes(i).CopyTo(chunkData, 4);
                                Array.Copy(compressedData, i * chunkSize, chunkData, 8, currentChunkSize);

                                udpClient.Send(chunkData, chunkData.Length, serverEndPoint);
                            }

                            // Send end marker
                            byte[] endMarker = Encoding.ASCII.GetBytes("END_OF_IMAGE");
                            udpClient.Send(endMarker, endMarker.Length, serverEndPoint);

                            Thread.Sleep(10);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }



        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopSharing(); // Stop sharing if it is running
            DisconnectClient(); // Disconnect the client
        }

        private void lblPort_Click(object sender, EventArgs e)
        {

        }
    }
}
