namespace ScreenSharingClient
{
    partial class ClientForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnStartSharing;
        private System.Windows.Forms.TextBox txt_IP;
        private System.Windows.Forms.TextBox txt_PORT;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnStopSharing;

        private void InitializeComponent()
        {
            this.btnStartSharing = new System.Windows.Forms.Button();
            this.txt_IP = new System.Windows.Forms.TextBox();
            this.txt_PORT = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnStopSharing = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStartSharing
            // 
            this.btnStartSharing.Location = new System.Drawing.Point(206, 235);
            this.btnStartSharing.Name = "btnStartSharing";
            this.btnStartSharing.Size = new System.Drawing.Size(120, 40);
            this.btnStartSharing.TabIndex = 5;
            this.btnStartSharing.Enabled = false;
            this.btnStartSharing.Text = "Start Sharing";
            this.btnStartSharing.Click += new System.EventHandler(this.btnStartSharing_Click);
            // 
            // txt_IP
            // 
            this.txt_IP.Location = new System.Drawing.Point(133, 26);
            this.txt_IP.Name = "txt_IP";
            this.txt_IP.Size = new System.Drawing.Size(100, 26);
            this.txt_IP.TabIndex = 1;
            this.txt_IP.Text = "127.0.0.1";
            // 
            // txt_PORT
            // 
            this.txt_PORT.Location = new System.Drawing.Point(299, 26);
            this.txt_PORT.Name = "txt_PORT";
            this.txt_PORT.Size = new System.Drawing.Size(100, 26);
            this.txt_PORT.TabIndex = 2;
            this.txt_PORT.Text = "50000";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(133, 136);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 36);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(299, 134);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(100, 40);
            this.btnDisconnect.TabIndex = 4;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnStopSharing
            // 
            this.btnStopSharing.Location = new System.Drawing.Point(206, 305);
            this.btnStopSharing.Name = "btnStopSharing";
            this.btnStopSharing.Size = new System.Drawing.Size(120, 40);
            this.btnStopSharing.TabIndex = 6;
            this.btnStopSharing.Text = "Stop Sharing";
            this.btnStopSharing.Enabled = false;
            this.btnStopSharing.Click += new System.EventHandler(this.btnStopSharing_Click);
            // 
            // ClientForm
            // 
            this.ClientSize = new System.Drawing.Size(613, 437);
            this.Controls.Add(this.btnStopSharing);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.txt_PORT);
            this.Controls.Add(this.txt_IP);
            this.Controls.Add(this.btnStartSharing);
            this.Name = "ClientForm";
            this.Text = "Screen Sharing Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
