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
        private NetworkStream networkStream;

        public ServerForm()
        {
            InitializeComponent();
            StartServer();
        }

        private void StartServer()
        {
            try
            {
                server = new TcpListener(IPAddress.Any, 8000);
                server.Start();
                server.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), server);
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
                TcpClient client = server.EndAcceptTcpClient(ar);
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
                    using (MemoryStream ms = new MemoryStream(buffer, 0, bytesRead))
                    {
                        Image img = Image.FromStream(ms);
                        pictureBoxScreen.Invoke((MethodInvoker)delegate
                        {
                            pictureBoxScreen.Image = img;
                        });
                    }
                    BeginReceive(); // Continue receiving
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error receiving data: " + ex.Message);
            }
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (server != null)
            {
                server.Stop();
            }
        }
    }
}
