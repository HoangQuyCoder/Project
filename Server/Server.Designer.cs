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
        private System.Windows.Forms.TextBox txt_IP;
        private System.Windows.Forms.TextBox txt_PORT;

        private void InitializeComponent()
        {
            this.pictureBoxScreen = new System.Windows.Forms.PictureBox();
            this.btn_Start = new System.Windows.Forms.Button();
            this.txt_IP = new System.Windows.Forms.TextBox();
            this.txt_PORT = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScreen)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxScreen
            // 
            this.pictureBoxScreen.Location = new System.Drawing.Point(12, 235);
            this.pictureBoxScreen.Name = "pictureBoxScreen";
            this.pictureBoxScreen.Size = new System.Drawing.Size(776, 203);
            this.pictureBoxScreen.TabIndex = 0;
            this.pictureBoxScreen.TabStop = false;
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(113, 154);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(75, 23);
            this.btn_Start.TabIndex = 1;
            this.btn_Start.Text = "Start";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txt_IP
            // 
            this.txt_IP.Location = new System.Drawing.Point(53, 21);
            this.txt_IP.Name = "txt_IP";
            this.txt_IP.Size = new System.Drawing.Size(100, 22);
            this.txt_IP.TabIndex = 2;
            // 
            // txt_PORT
            // 
            this.txt_PORT.Location = new System.Drawing.Point(53, 85);
            this.txt_PORT.Name = "txt_PORT";
            this.txt_PORT.Size = new System.Drawing.Size(100, 22);
            this.txt_PORT.TabIndex = 3;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
