using System.Drawing;
using System.Windows.Forms;
using System;

namespace ScreenSharingClient
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// 
        private System.Windows.Forms.Button btnStartSharing;


        private void InitializeComponent()
        {
            // Thiết lập form
            this.Text = "Screen Sharing Client";
            this.Size = new Size(300, 150);

            // Tạo và thiết lập nút Start Sharing
            btnStartSharing = new Button();
            btnStartSharing.Text = "Start Sharing";
            btnStartSharing.Size = new Size(120, 40);
            btnStartSharing.Location = new Point(90, 40);
            btnStartSharing.Click += new EventHandler(this.btnStartSharing_Click);

            // Thêm nút vào form
            this.Controls.Add(btnStartSharing);

            // Thiết lập sự kiện FormClosing
            this.FormClosing += new FormClosingEventHandler(this.ClientForm_FormClosing);
        }

        #endregion
    }
}

