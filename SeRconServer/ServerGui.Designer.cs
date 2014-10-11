namespace SeRconServer
{
	partial class ServerGui
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
			this.logViewer = new SeRconCore.Control.LogViewer();
			this.SuspendLayout();
			// 
			// logViewer
			// 
			this.logViewer.AllowNavigation = false;
			this.logViewer.AllowWebBrowserDrop = false;
			this.logViewer.DateFormat = "yyyy/MM/dd | HH:mm:ss";
			this.logViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logViewer.Location = new System.Drawing.Point(0, 0);
			this.logViewer.MinimumSize = new System.Drawing.Size(20, 20);
			this.logViewer.Name = "logViewer";
			this.logViewer.Size = new System.Drawing.Size(624, 290);
			this.logViewer.StyleSheet = "/Style/log.css";
			this.logViewer.TabIndex = 0;
			this.logViewer.WebBrowserShortcutsEnabled = false;
			// 
			// ServerGui
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 290);
			this.Controls.Add(this.logViewer);
			this.Name = "ServerGui";
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerGui_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private SeRconCore.Control.LogViewer logViewer;
	}
}

