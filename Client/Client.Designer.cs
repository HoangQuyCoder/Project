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
        private System.Windows.Forms.Label lbl_IP;
        private System.Windows.Forms.Label lbl_PORT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPort;

        private void InitializeComponent()
        {
            this.btnStartSharing = new System.Windows.Forms.Button();
            this.txt_IP = new System.Windows.Forms.TextBox();
            this.txt_PORT = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnStopSharing = new System.Windows.Forms.Button();
            this.lbl_IP = new System.Windows.Forms.Label();
            this.lbl_PORT = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStartSharing
            // 
            this.btnStartSharing.Enabled = false;
            this.btnStartSharing.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnStartSharing.Location = new System.Drawing.Point(98, 331);
            this.btnStartSharing.Name = "btnStartSharing";
            this.btnStartSharing.Size = new System.Drawing.Size(155, 48);
            this.btnStartSharing.TabIndex = 5;
            this.btnStartSharing.Text = "Start Sharing";
            this.btnStartSharing.UseVisualStyleBackColor = true;
            this.btnStartSharing.Click += new System.EventHandler(this.btnStartSharing_Click);
            // 
            // txt_IP
            // 
            this.txt_IP.Font = new System.Drawing.Font("Arial", 10F);
            this.txt_IP.Location = new System.Drawing.Point(200, 105);
            this.txt_IP.Name = "txt_IP";
            this.txt_IP.Size = new System.Drawing.Size(150, 30);
            this.txt_IP.TabIndex = 1;
            this.txt_IP.Text = "127.0.0.1";
            // 
            // txt_PORT
            // 
            this.txt_PORT.Font = new System.Drawing.Font("Arial", 10F);
            this.txt_PORT.Location = new System.Drawing.Point(200, 151);
            this.txt_PORT.Name = "txt_PORT";
            this.txt_PORT.Size = new System.Drawing.Size(150, 30);
            this.txt_PORT.TabIndex = 2;
            this.txt_PORT.Text = "5000";
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnConnect.Location = new System.Drawing.Point(226, 233);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(150, 40);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnDisconnect.ForeColor = System.Drawing.Color.Red;
            this.btnDisconnect.Location = new System.Drawing.Point(421, 119);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(136, 40);
            this.btnDisconnect.TabIndex = 4;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnStopSharing
            // 
            this.btnStopSharing.Enabled = false;
            this.btnStopSharing.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnStopSharing.ForeColor = System.Drawing.Color.Red;
            this.btnStopSharing.Location = new System.Drawing.Point(334, 331);
            this.btnStopSharing.Name = "btnStopSharing";
            this.btnStopSharing.Size = new System.Drawing.Size(162, 48);
            this.btnStopSharing.TabIndex = 6;
            this.btnStopSharing.Text = "Stop Sharing";
            this.btnStopSharing.UseVisualStyleBackColor = true;
            this.btnStopSharing.Click += new System.EventHandler(this.btnStopSharing_Click);
            // 
            // lbl_IP
            // 
            this.lbl_IP.Font = new System.Drawing.Font("Arial", 10F);
            this.lbl_IP.Location = new System.Drawing.Point(94, 108);
            this.lbl_IP.Name = "lbl_IP";
            this.lbl_IP.Size = new System.Drawing.Size(100, 20);
            this.lbl_IP.TabIndex = 7;
            this.lbl_IP.Text = "IP Address:";
            // 
            // lbl_PORT
            // 
            this.lbl_PORT.Font = new System.Drawing.Font("Arial", 10F);
            this.lbl_PORT.Location = new System.Drawing.Point(94, 151);
            this.lbl_PORT.Name = "lbl_PORT";
            this.lbl_PORT.Size = new System.Drawing.Size(100, 20);
            this.lbl_PORT.TabIndex = 8;
            this.lbl_PORT.Text = "Port:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Aquamarine;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Goldenrod;
            this.label1.Location = new System.Drawing.Point(165, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(331, 38);
            this.label1.TabIndex = 9;
            this.label1.Text = "Screen-Sharing-App";
            // 
            // lblPort
            // 
            this.lblPort.Font = new System.Drawing.Font("Arial", 10F);
            this.lblPort.Location = new System.Drawing.Point(95, 200);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(150, 30);
            this.lblPort.TabIndex = 10;
            this.lblPort.Text = "";
            this.lblPort.Click += new System.EventHandler(this.lblPort_Click);
            // 
            // ClientForm
            // 
            this.BackColor = System.Drawing.Color.MediumTurquoise;
            this.ClientSize = new System.Drawing.Size(653, 462);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_PORT);
            this.Controls.Add(this.lbl_IP);
            this.Controls.Add(this.btnStopSharing);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.txt_PORT);
            this.Controls.Add(this.txt_IP);
            this.Controls.Add(this.btnStartSharing);
            this.Controls.Add(this.lblPort);
            this.Name = "ClientForm";
            this.Text = "Screen Sharing Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
