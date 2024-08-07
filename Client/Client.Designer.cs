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

        private void InitializeComponent()
        {
            this.btnStartSharing = new System.Windows.Forms.Button();
            this.txt_IP = new System.Windows.Forms.TextBox();
            this.txt_PORT = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStartSharing
            // 
            this.btnStartSharing.Location = new System.Drawing.Point(397, 250);
            this.btnStartSharing.Name = "btnStartSharing";
            this.btnStartSharing.Size = new System.Drawing.Size(120, 40);
            this.btnStartSharing.TabIndex = 0;
            this.btnStartSharing.Text = "Start Sharing";
            this.btnStartSharing.Click += new System.EventHandler(this.btnStartSharing_Click);
            // 
            // txt_IP
            // 
            this.txt_IP.Location = new System.Drawing.Point(72, 12);
            this.txt_IP.Name = "txt_IP";
            this.txt_IP.Size = new System.Drawing.Size(100, 22);
            this.txt_IP.TabIndex = 1;
            // 
            // txt_PORT
            // 
            this.txt_PORT.Location = new System.Drawing.Point(72, 51);
            this.txt_PORT.Name = "txt_PORT";
            this.txt_PORT.Size = new System.Drawing.Size(100, 22);
            this.txt_PORT.TabIndex = 2;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(127, 136);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // ClientForm
            // 
            this.ClientSize = new System.Drawing.Size(594, 347);
            this.Controls.Add(this.btnConnect);
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
