using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
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

        private void StartServer()
        {
            try
            {
                string ip = txt_IP.Text;
                int port = int.Parse(txt_PORT.Text);
                IPAddress ipAddress = IPAddress.Parse(ip);
                server = new TcpListener(ipAddress, port);
                server.Start();
                server.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), server);
                MessageBox.Show("Server started. Waiting for clients...");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting server: " + ex.Message);
            }
        }

        private void AcceptClient(IAsyncResult ar)
        {
            try
            {
                client = server.EndAcceptTcpClient(ar);
                networkStream = client.GetStream();
                BeginReceive();
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
                byte[] buffer = new byte[1024 * 1024 * 10]; // 10MB buffer
                networkStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(ReceiveData), buffer);
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
                byte[] buffer = (byte[])ar.AsyncState;
                int bytesRead = networkStream.EndRead(ar);

                if (bytesRead > 0)
                {
                    pictureBoxScreen.Invoke((MethodInvoker)delegate
                    {
                        using (MemoryStream ms = new MemoryStream(buffer, 0, bytesRead))
                        {
                            Image img = Image.FromStream(ms);
                            pictureBoxScreen.Image = img;
                            pictureBoxScreen.SizeMode = PictureBoxSizeMode.Zoom; // Maintain aspect ratio
                            pictureBoxScreen.Size = GetScaledImageSize(img.Size, pictureBoxScreen.Size);
                        }
                    });
                    BeginReceive();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error receiving data: " + ex.Message);
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
            if (server != null)
            {
                server.Stop();
            }
            if (client != null)
            {
                client.Close();
            }
        }
    }
}
