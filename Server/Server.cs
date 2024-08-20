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
        private Dictionary<IPEndPoint, PictureBox> clientPictureBoxes;
        private Dictionary<IPEndPoint, Label> clientLabels;
        public ServerForm()
        {
            InitializeComponent();
            clientPictureBoxes = new Dictionary<IPEndPoint, PictureBox>();
            clientLabels = new Dictionary<IPEndPoint, Label>();
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

                btn_Start.Enabled = false;
                btn_Stop.Enabled = true;
                txt_IP.Enabled = false; 
                txt_PORT.Enabled = false;
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
                        ClearImage(clientEndPoint);
                        imageChunks.Clear(); // Clear any partial image data
                        continue;
                    }

                    if (receivedText == "PING")
                    {
                        // Send back a "PONG" response
                        byte[] pongMessage = Encoding.ASCII.GetBytes("PONG");
                        udpServer.Send(pongMessage, pongMessage.Length, clientEndPoint);
                    }
                    else
                    {
                        if (IsEndOfImageChunk(receivedData))
                        {
                            ProcessImage(imageChunks);
                            imageChunks.Clear(); // Clear chunks after processing the image
                        }
                        else
                        {
                            imageChunks.Add(receivedData);
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
            try
            {
                byte[] imageData = imageChunks.SelectMany(a => a).ToArray();

                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image img = Image.FromStream(ms);
                    if (flowLayoutPanel.InvokeRequired)
                    {
                        flowLayoutPanel.Invoke(new Action(() => UpdateClientImage(clientEndPoint, img)));
                    }
                    else
                    {
                        UpdateClientImage(clientEndPoint, img);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing image: " + ex.Message);
            }
        }

        private void UpdateClientImage(IPEndPoint clientEndPoint, Image img)
        {
            PictureBox clientPictureBox;

            // Check PictureBox client
            if (!clientPictureBoxes.TryGetValue(clientEndPoint, out clientPictureBox))
            {
                clientPictureBox = new PictureBox
                {
                    Size = new Size(200, 150),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Margin = new Padding(10)
                };

                clientPictureBox.Size = GetScaledImageSize(img.Size, clientPictureBox.Size);
                flowLayoutPanel.Controls.Add(clientPictureBox);
                clientPictureBoxes[clientEndPoint] = clientPictureBox;

                // Create label for client
                Label clientLabel = new Label
                {
                    Text = clientEndPoint.ToString(), // Display IP address and port
                    AutoSize = true,
                    Margin = new Padding(10)
                };
                flowLayoutPanel.Controls.Add(clientLabel);
                clientLabels[clientEndPoint] = clientLabel; // Save label on the dictionary
            }

            clientPictureBox.Image = img; // Update images for client
        }

        private bool IsEndOfImageChunk(byte[] chunkData)
        {
            string dataAsString = Encoding.ASCII.GetString(chunkData);
            return dataAsString.Contains("END_OF_IMAGE");
        }

        private void ClearImage(IPEndPoint clientEndPoint)
        {
            if (clientPictureBoxes.TryGetValue(clientEndPoint, out PictureBox pictureBox))
            {
                pictureBox.Invoke((MethodInvoker)delegate
                {
                    pictureBox.Image = null;
                    flowLayoutPanel.Controls.Remove(pictureBox);
                    clientPictureBoxes.Remove(clientEndPoint);
                });
            }

            if (clientLabels.TryGetValue(clientEndPoint, out Label clientLabel))
            {
                clientLabel.Invoke((MethodInvoker)delegate
                {
                    flowLayoutPanel.Controls.Remove(clientLabel);
                    clientLabels.Remove(clientEndPoint);
                });
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
