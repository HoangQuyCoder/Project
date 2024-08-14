using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ScreenSharingServer
{
    public partial class ServerForm : Form
    {
        private UdpClient udpServer;
        private IPEndPoint clientEndPoint;
        private Thread listenerThread;
        private bool isRunning;

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
                udpServer = new UdpClient(port);
                clientEndPoint = new IPEndPoint(IPAddress.Any, port); // Listen from any client
                isRunning = true;

                listenerThread = new Thread(ListenForClients);
                listenerThread.IsBackground = true;
                listenerThread.Start();

                btn_Start.Enabled = false; // Disable Start button after server starts
                btn_Stop.Enabled = true;
                txt_IP.Enabled = false; // Disable IP input field
                txt_PORT.Enabled = false; // Disable Port input field
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
                isRunning = false;

                if (udpServer != null)
                {
                    udpServer.Close();
                    udpServer = null;
                }

                if (listenerThread != null)
                {
                    listenerThread.Join(); // Wait for the thread to finish
                    listenerThread = null;
                }

                btn_Start.Enabled = true;
                btn_Stop.Enabled = false;
                txt_IP.Enabled = true;
                txt_PORT.Enabled = true;
                MessageBox.Show("Server stopped.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping server: " + ex.Message);
            }
        }

        private void ListenForClients()
        {
            try
            {
                List<byte[]> imageChunks = new List<byte[]>();

                while (isRunning)
                {
                    byte[] receivedData = udpServer.Receive(ref clientEndPoint);
                    string receivedText = Encoding.ASCII.GetString(receivedData);

                    if (receivedText == "STOP_SHARING")
                    {
                        ClearImage();
                        continue; // Skip further processing for stop signal
                    }

                    if (receivedText == "PING")
                    {
                        // Send back a "PONG" response
                        byte[] pongMessage = Encoding.ASCII.GetBytes("PONG");
                        udpServer.Send(pongMessage, pongMessage.Length, clientEndPoint);
                    }
                    else
                    {
                        imageChunks.Add(receivedData);

                        if (receivedData.Length < 65000) // Assume last chunk indicates end of image
                        {
                            ProcessImage(imageChunks);
                            imageChunks.Clear(); // Clear chunks after processing the image
                        }
                    }
                }
            }
            catch (SocketException)
            {
                if (udpServer == null) return; // Server was closed
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error receiving data: " + ex.Message);
            }
        }

        private void ProcessImage(List<byte[]> imageChunks)
        {
            byte[] imageData = imageChunks.SelectMany(a => a).ToArray();

            using (MemoryStream ms = new MemoryStream(imageData))
            {
                Image img = Image.FromStream(ms);
                pictureBoxScreen.Invoke((MethodInvoker)delegate
                {
                    pictureBoxScreen.Image = img;
                    pictureBoxScreen.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBoxScreen.Size = GetScaledImageSize(img.Size, pictureBoxScreen.Size);
                });
            }
        }

        private void ClearImage()
        {
            pictureBoxScreen.Invoke((MethodInvoker)delegate
            {
                pictureBoxScreen.Image = null; // Clear current image
            });
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

        private void ServerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
