namespace SeRconClient
{
	partial class Connect
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
		private void InitializeComponent()
		{
			this.grpConnection = new System.Windows.Forms.GroupBox();
			this.panPort = new System.Windows.Forms.Panel();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.panAddress = new System.Windows.Forms.Panel();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.btnConnect = new System.Windows.Forms.Button();
			this.lblPort = new System.Windows.Forms.Label();
			this.lblIp = new System.Windows.Forms.Label();
			this.lblPassword = new System.Windows.Forms.Label();
			this.panPassword = new System.Windows.Forms.Panel();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.grpConnection.SuspendLayout();
			this.panPort.SuspendLayout();
			this.panAddress.SuspendLayout();
			this.panPassword.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpConnection
			// 
			this.grpConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpConnection.Controls.Add(this.panPassword);
			this.grpConnection.Controls.Add(this.lblPassword);
			this.grpConnection.Controls.Add(this.panPort);
			this.grpConnection.Controls.Add(this.panAddress);
			this.grpConnection.Controls.Add(this.btnConnect);
			this.grpConnection.Controls.Add(this.lblPort);
			this.grpConnection.Controls.Add(this.lblIp);
			this.grpConnection.Location = new System.Drawing.Point(10, 10);
			this.grpConnection.Margin = new System.Windows.Forms.Padding(1);
			this.grpConnection.Name = "grpConnection";
			this.grpConnection.Size = new System.Drawing.Size(347, 120);
			this.grpConnection.TabIndex = 0;
			this.grpConnection.TabStop = false;
			this.grpConnection.Text = "Connect to a Rcon server";
			// 
			// panPort
			// 
			this.panPort.Controls.Add(this.txtPort);
			this.panPort.Location = new System.Drawing.Point(269, 41);
			this.panPort.Name = "panPort";
			this.panPort.Size = new System.Drawing.Size(73, 22);
			this.panPort.TabIndex = 8;
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(1, 1);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(71, 20);
			this.txtPort.TabIndex = 3;
			this.txtPort.Text = "8888";
			// 
			// panAddress
			// 
			this.panAddress.BackColor = System.Drawing.SystemColors.Control;
			this.panAddress.Controls.Add(this.txtAddress);
			this.panAddress.Location = new System.Drawing.Point(6, 41);
			this.panAddress.Name = "panAddress";
			this.panAddress.Size = new System.Drawing.Size(258, 22);
			this.panAddress.TabIndex = 7;
			// 
			// txtAddress
			// 
			this.txtAddress.Location = new System.Drawing.Point(1, 1);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(256, 20);
			this.txtAddress.TabIndex = 2;
			// 
			// btnConnect
			// 
			this.btnConnect.Location = new System.Drawing.Point(271, 83);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(70, 23);
			this.btnConnect.TabIndex = 5;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// lblPort
			// 
			this.lblPort.AutoSize = true;
			this.lblPort.Location = new System.Drawing.Point(268, 25);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(29, 13);
			this.lblPort.TabIndex = 4;
			this.lblPort.Text = "Port:";
			// 
			// lblIp
			// 
			this.lblIp.AutoSize = true;
			this.lblIp.Location = new System.Drawing.Point(6, 25);
			this.lblIp.Name = "lblIp";
			this.lblIp.Size = new System.Drawing.Size(48, 13);
			this.lblIp.TabIndex = 1;
			this.lblIp.Text = "Address:";
			// 
			// lblPassword
			// 
			this.lblPassword.AutoSize = true;
			this.lblPassword.Location = new System.Drawing.Point(6, 68);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(56, 13);
			this.lblPassword.TabIndex = 9;
			this.lblPassword.Text = "Password:";
			// 
			// panPassword
			// 
			this.panPassword.BackColor = System.Drawing.SystemColors.Control;
			this.panPassword.Controls.Add(this.txtPassword);
			this.panPassword.Location = new System.Drawing.Point(6, 84);
			this.panPassword.Name = "panPassword";
			this.panPassword.Size = new System.Drawing.Size(258, 22);
			this.panPassword.TabIndex = 8;
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(1, 1);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(256, 20);
			this.txtPassword.TabIndex = 2;
			// 
			// Connect
			// 
			this.AcceptButton = this.btnConnect;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(367, 140);
			this.Controls.Add(this.grpConnection);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Connect";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Connect...";
			this.grpConnection.ResumeLayout(false);
			this.grpConnection.PerformLayout();
			this.panPort.ResumeLayout(false);
			this.panPort.PerformLayout();
			this.panAddress.ResumeLayout(false);
			this.panAddress.PerformLayout();
			this.panPassword.ResumeLayout(false);
			this.panPassword.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpConnection;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.Label lblIp;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Panel panAddress;
		private System.Windows.Forms.Panel panPort;
		private System.Windows.Forms.Panel panPassword;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label lblPassword;
	}
}