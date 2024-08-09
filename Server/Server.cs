using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ScreenSharingServer
{
    public partial class ServerForm : Form
    {
        private TcpListener server;
        private TcpClient client;
        private NetworkStream networkStream;

        public ServerForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartServer();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        private void StartServer()
        {
            try
            {
                string ip = txt_IP.Text;
                int port = int.Parse(txt_PORT.Text);
                IPAddress ipAddress = IPAddress.Parse(ip);
                server = new TcpListener(ipAddress, port);
                server.Start();
                BeginAcceptClient();
                btn_Start.Enabled = false; // Vô hiệu hóa nút Start sau khi bắt đầu server
                btn_Stop.Enabled = true;
                txt_IP.Enabled = false; // Vô hiệu hóa ô nhập IP
                txt_PORT.Enabled = false; // Vô hiệu hóa ô nhập Port
                MessageBox.Show("Server started. Waiting for clients...");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting server: " + ex.Message);
            }
        }

        private void StopServer()
        {
            try
            {
                // Đóng kết nối client
                if (client != null)
                {
                    client.Close();
                    client = null;
                }

                // Đóng mạng và server
                if (networkStream != null)
                {
                    networkStream.Close();
                    networkStream = null;
                }

                if (server != null)
                {
                    server.Stop();
                    server = null;
                }
                btn_Start.Enabled = true;
                txt_IP.Enabled = true;
                txt_PORT.Enabled = true;
                MessageBox.Show("Server stopped and client connection closed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping server: " + ex.Message);
            }
        }


        private void BeginAcceptClient()
        {
            try
            {
                server.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), server);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi bắt đầu lắng nghe client: " + ex.Message);
            }
        }

        private void AcceptClient(IAsyncResult ar)
        {
            try
            {
                if (server == null)
                {
                    MessageBox.Show("Server is not initialized.");
                    return;
                }

                client = server.EndAcceptTcpClient(ar);

                if (client != null)
                {
                    networkStream = client.GetStream();
                    BeginReceive();
                    BeginAcceptClient();
                }
                else
                {
                    MessageBox.Show("Failed to accept client connection.");
                }
            }
            catch (ObjectDisposedException)
            {
                // This can happen if the server is stopped while waiting for a connection
                MessageBox.Show("Server has been stopped and cannot accept clients.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error accepting client: " + ex.Message);
            }
        }

        private void BeginReceive()
        {
            try
            {
                if (networkStream != null)
                {
                    byte[] buffer = new byte[1024 * 1024 * 10]; // 10MB buffer
                    networkStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(ReceiveData), buffer);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error beginning to receive data: " + ex.Message);
            }
        }

        private void ReceiveData(IAsyncResult ar)
        {
            try
            {
                if (networkStream == null)
                {
                    MessageBox.Show("Network stream is not initialized.");
                    return;
                }

                if (client == null || !client.Connected)
                {
                    MessageBox.Show("Client is not connected.");
                    return;
                }

                byte[] buffer = (byte[])ar.AsyncState;
                if (buffer == null)
                {
                    MessageBox.Show("Received buffer is null.");
                    return;
                }

                int bytesRead = networkStream.EndRead(ar);

                if (bytesRead > 0)
                {
                    string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    if (receivedData == "STOP_SHARING")
                    {
                        pictureBoxScreen.Invoke((MethodInvoker)delegate
                        {
                            pictureBoxScreen.Image = null; // Clear the image
                        });
                    }
                    else
                    {
                        pictureBoxScreen.Invoke((MethodInvoker)delegate
                        {
                            using (MemoryStream ms = new MemoryStream(buffer, 0, bytesRead))
                            {
                                Image img = Image.FromStream(ms);
                                pictureBoxScreen.Image = img;
                                pictureBoxScreen.SizeMode = PictureBoxSizeMode.Zoom;
                                pictureBoxScreen.Size = GetScaledImageSize(img.Size, pictureBoxScreen.Size);
                            }
                        });
                    }

                    BeginReceive();
                }
            }
            catch (IOException)
            {
                // IOException xảy ra khi client ngắt kết nối
                MessageBox.Show("Client disconnected.");
                ResetConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error receiving data: " + ex.Message);
                ResetConnection();
            }
        }

        private void ResetConnection()
        {
            if (client != null)
            {
                client.Close();
                client = null;
            }

            if (networkStream != null)
            {
                networkStream.Close();
                networkStream = null;
            }
        }
        private Size GetScaledImageSize(Size imageSize, Size containerSize)
        {
            float aspectRatio = (float)imageSize.Width / imageSize.Height;
            float containerAspectRatio = (float)containerSize.Width / containerSize.Height;

            if (aspectRatio > containerAspectRatio)
            {
                return new Size(containerSize.Width, (int)(containerSize.Width / aspectRatio));
            }
            else
            {
                return new Size((int)(containerSize.Height * aspectRatio), containerSize.Height);
            }
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
        }
    }
}
