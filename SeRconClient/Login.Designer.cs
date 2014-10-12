namespace SeRconClient
{
	partial class Login
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblUsername = new System.Windows.Forms.Label();
			this.panAddress = new System.Windows.Forms.Panel();
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.panPassword = new System.Windows.Forms.Panel();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.btnLogIn = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.panAddress.SuspendLayout();
			this.panPassword.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.btnLogIn);
			this.groupBox1.Controls.Add(this.panPassword);
			this.groupBox1.Controls.Add(this.lblPassword);
			this.groupBox1.Controls.Add(this.panAddress);
			this.groupBox1.Controls.Add(this.lblUsername);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(223, 152);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Log in";
			// 
			// lblUsername
			// 
			this.lblUsername.AutoSize = true;
			this.lblUsername.Location = new System.Drawing.Point(6, 25);
			this.lblUsername.Name = "lblUsername";
			this.lblUsername.Size = new System.Drawing.Size(58, 13);
			this.lblUsername.TabIndex = 1;
			this.lblUsername.Text = "Username:";
			// 
			// panAddress
			// 
			this.panAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panAddress.BackColor = System.Drawing.SystemColors.Control;
			this.panAddress.Controls.Add(this.txtUsername);
			this.panAddress.Location = new System.Drawing.Point(6, 41);
			this.panAddress.Name = "panAddress";
			this.panAddress.Size = new System.Drawing.Size(211, 22);
			this.panAddress.TabIndex = 8;
			// 
			// txtUsername
			// 
			this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtUsername.Location = new System.Drawing.Point(1, 1);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(209, 20);
			this.txtUsername.TabIndex = 2;
			// 
			// panel1
			// 
			this.panPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panPassword.BackColor = System.Drawing.SystemColors.Control;
			this.panPassword.Controls.Add(this.txtPassword);
			this.panPassword.Location = new System.Drawing.Point(6, 88);
			this.panPassword.Name = "panel1";
			this.panPassword.Size = new System.Drawing.Size(211, 22);
			this.panPassword.TabIndex = 10;
			// 
			// txtPassword
			// 
			this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPassword.Location = new System.Drawing.Point(1, 1);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(209, 20);
			this.txtPassword.TabIndex = 2;
			this.txtPassword.UseSystemPasswordChar = true;
			// 
			// lblPassword
			// 
			this.lblPassword.AutoSize = true;
			this.lblPassword.Location = new System.Drawing.Point(6, 72);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(56, 13);
			this.lblPassword.TabIndex = 9;
			this.lblPassword.Text = "Password:";
			// 
			// btnLogIn
			// 
			this.btnLogIn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnLogIn.Location = new System.Drawing.Point(75, 121);
			this.btnLogIn.Name = "btnLogIn";
			this.btnLogIn.Size = new System.Drawing.Size(75, 23);
			this.btnLogIn.TabIndex = 11;
			this.btnLogIn.Text = "Log In";
			this.btnLogIn.UseVisualStyleBackColor = true;
			this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
			// 
			// Login
			// 
			this.AcceptButton = this.btnLogIn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(247, 176);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Login";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Log In...";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.panAddress.ResumeLayout(false);
			this.panAddress.PerformLayout();
			this.panPassword.ResumeLayout(false);
			this.panPassword.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblUsername;
		private System.Windows.Forms.Panel panPassword;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.Panel panAddress;
		private System.Windows.Forms.TextBox txtUsername;
		private System.Windows.Forms.Button btnLogIn;
	}
}