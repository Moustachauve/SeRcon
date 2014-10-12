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
			this.lblIp = new System.Windows.Forms.Label();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.btnConnect = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.panAddress = new System.Windows.Forms.Panel();
			this.panPort = new System.Windows.Forms.Panel();
			this.grpConnection.SuspendLayout();
			this.panAddress.SuspendLayout();
			this.panPort.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpConnection
			// 
			this.grpConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpConnection.Controls.Add(this.panPort);
			this.grpConnection.Controls.Add(this.panAddress);
			this.grpConnection.Controls.Add(this.btnCancel);
			this.grpConnection.Controls.Add(this.btnConnect);
			this.grpConnection.Controls.Add(this.lblPort);
			this.grpConnection.Controls.Add(this.lblIp);
			this.grpConnection.Location = new System.Drawing.Point(10, 10);
			this.grpConnection.Margin = new System.Windows.Forms.Padding(1);
			this.grpConnection.Name = "grpConnection";
			this.grpConnection.Size = new System.Drawing.Size(347, 100);
			this.grpConnection.TabIndex = 0;
			this.grpConnection.TabStop = false;
			this.grpConnection.Text = "Connect to a Rcon server";
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
			// txtAddress
			// 
			this.txtAddress.Location = new System.Drawing.Point(1, 1);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(256, 20);
			this.txtAddress.TabIndex = 2;
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(1, 1);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(71, 20);
			this.txtPort.TabIndex = 3;
			this.txtPort.Text = "8888";
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
			// btnConnect
			// 
			this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnConnect.Location = new System.Drawing.Point(266, 67);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(75, 23);
			this.btnConnect.TabIndex = 5;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(198, 67);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(62, 23);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// panIp
			// 
			this.panAddress.BackColor = System.Drawing.SystemColors.Control;
			this.panAddress.Controls.Add(this.txtAddress);
			this.panAddress.Location = new System.Drawing.Point(6, 41);
			this.panAddress.Name = "panIp";
			this.panAddress.Size = new System.Drawing.Size(258, 22);
			this.panAddress.TabIndex = 7;
			// 
			// panPort
			// 
			this.panPort.Controls.Add(this.txtPort);
			this.panPort.Location = new System.Drawing.Point(269, 41);
			this.panPort.Name = "panPort";
			this.panPort.Size = new System.Drawing.Size(73, 22);
			this.panPort.TabIndex = 8;
			// 
			// Form1
			// 
			this.AcceptButton = this.btnConnect;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(367, 120);
			this.Controls.Add(this.grpConnection);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Connect...";
			this.grpConnection.ResumeLayout(false);
			this.grpConnection.PerformLayout();
			this.panAddress.ResumeLayout(false);
			this.panAddress.PerformLayout();
			this.panPort.ResumeLayout(false);
			this.panPort.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpConnection;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.Label lblIp;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Panel panAddress;
		private System.Windows.Forms.Panel panPort;
	}
}