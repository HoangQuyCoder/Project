namespace ScreenSharingServer
{
    partial class ServerForm
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

        private System.Windows.Forms.PictureBox pictureBoxScreen;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.TextBox txt_IP;
        private System.Windows.Forms.TextBox txt_PORT;
        private System.Windows.Forms.Label lbl_IP;
        private System.Windows.Forms.Label lbl_PORT;

        private void InitializeComponent()
        {
            this.pictureBoxScreen = new System.Windows.Forms.PictureBox();
            this.btn_Start = new System.Windows.Forms.Button();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.txt_IP = new System.Windows.Forms.TextBox();
            this.txt_PORT = new System.Windows.Forms.TextBox();
            this.lbl_IP = new System.Windows.Forms.Label();
            this.lbl_PORT = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScreen)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxScreen
            // 
            this.pictureBoxScreen.Location = new System.Drawing.Point(80, 113);
            this.pictureBoxScreen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBoxScreen.Name = "pictureBoxScreen";
            this.pictureBoxScreen.Size = new System.Drawing.Size(798, 458);
            this.pictureBoxScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxScreen.TabIndex = 0;
            this.pictureBoxScreen.TabStop = false;
            // 
            // btn_Start
            // 
            this.btn_Start.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Start.Location = new System.Drawing.Point(558, 20);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(120, 55);
            this.btn_Start.TabIndex = 3;
            this.btn_Start.Text = "Start";
            this.btn_Start.UseVisualStyleBackColor = false;
            this.btn_Start.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btn_Stop
            // 
            this.btn_Stop.Enabled = false;
            this.btn_Stop.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Stop.Location = new System.Drawing.Point(758, 20);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(120, 55);
            this.btn_Stop.TabIndex = 4;
            this.btn_Stop.Text = "Stop";
            this.btn_Stop.UseVisualStyleBackColor = false;
            this.btn_Stop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txt_IP
            // 
            this.txt_IP.Font = new System.Drawing.Font("Arial", 10F);
            this.txt_IP.Location = new System.Drawing.Point(80, 40);
            this.txt_IP.Name = "txt_IP";
            this.txt_IP.Size = new System.Drawing.Size(143, 30);
            this.txt_IP.TabIndex = 1;
            this.txt_IP.Text = "127.0.0.1";
            // 
            // txt_PORT
            // 
            this.txt_PORT.Font = new System.Drawing.Font("Arial", 10F);
            this.txt_PORT.Location = new System.Drawing.Point(304, 40);
            this.txt_PORT.Name = "txt_PORT";
            this.txt_PORT.Size = new System.Drawing.Size(157, 30);
            this.txt_PORT.TabIndex = 2;
            this.txt_PORT.Text = "5000";
            // 
            // lbl_IP
            // 
            this.lbl_IP.Font = new System.Drawing.Font("Arial", 10F);
            this.lbl_IP.Location = new System.Drawing.Point(80, 20);
            this.lbl_IP.Name = "lbl_IP";
            this.lbl_IP.Size = new System.Drawing.Size(100, 20);
            this.lbl_IP.TabIndex = 5;
            this.lbl_IP.Text = "IP Address:";
            // 
            // lbl_PORT
            // 
            this.lbl_PORT.Font = new System.Drawing.Font("Arial", 10F);
            this.lbl_PORT.Location = new System.Drawing.Point(300, 20);
            this.lbl_PORT.Name = "lbl_PORT";
            this.lbl_PORT.Size = new System.Drawing.Size(100, 20);
            this.lbl_PORT.TabIndex = 6;
            this.lbl_PORT.Text = "Port:";
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1219, 653);
            this.Controls.Add(this.lbl_PORT);
            this.Controls.Add(this.lbl_IP);
            this.Controls.Add(this.btn_Stop);
            this.Controls.Add(this.txt_PORT);
            this.Controls.Add(this.txt_IP);
            this.Controls.Add(this.btn_Start);
            this.Controls.Add(this.pictureBoxScreen);
            this.Name = "ServerForm";
            this.Text = "Screen Sharing Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScreen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
