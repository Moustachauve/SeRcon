using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AltarNet;
using SeRconCore;
using SeRconCore.Control;

namespace SeRconServer
{
	public partial class ServerGui : Form
	{
		private ServerManager m_serverHandler;

		public ServerGui()
		{
			InitializeComponent();

			m_serverHandler = new ServerManager();
			m_serverHandler.OnClientConnected += m_serverHandler_OnClientConnected;
			m_serverHandler.OnClientDisonnected += m_serverHandler_OnClientDisonnected;
			m_serverHandler.OnMessageReceived += m_serverHandler_OnMessageReceived;

			Start();
		}

		public void Start()
		{
			if (m_serverHandler.IsRunning)
			{
				logViewer.WriteLine("The server is already running!", MessageType.Error);
				return;
			}
				m_serverHandler.Start();
				logViewer.WriteLine("Server is now listening to " + m_serverHandler.Ip + ":" + m_serverHandler.Port, MessageType.Notification);
			
		}

		#region OnClientConnect

		void m_serverHandler_OnClientConnected(object sender, AltarNet.TcpEventArgs e)
		{
			if (InvokeRequired)
			{
				this.Invoke((MethodInvoker)delegate
				{
					OnClientConnect(e);
				});
			}
			else
			{
				OnClientConnect(e);
			}
		}

		private void OnClientConnect(TcpEventArgs arg)
		{
			logViewer.WriteLine("Client connected");
			m_serverHandler.Notify("Welcome to this server.", arg.Client);
		}

		#endregion

		#region OnClientDisconnected

		void m_serverHandler_OnClientDisonnected(object sender, AltarNet.TcpEventArgs e)
		{
			if (InvokeRequired)
			{
				this.Invoke((MethodInvoker)delegate
				{
					OnClientDisconnect(e);
				});
			}
			else
			{
				OnClientDisconnect(e);
			}
		}

		private void OnClientDisconnect(TcpEventArgs arg)
		{
			logViewer.WriteLine("Client disconnected");
		}

		#endregion

		#region OnMessageReceived

		void m_serverHandler_OnMessageReceived(object sender, NotificationReceivedArgs e)
		{
			if (InvokeRequired)
			{
				this.Invoke((MethodInvoker)delegate
				{
					OnMessageReceived(e);
				});
			}
			else
			{
				OnMessageReceived(e);
			}
		}

		private void OnMessageReceived(NotificationReceivedArgs e)
		{
			logViewer.WriteLine("[" + e.Type + "]: " + e.Message, MessageType.GuestAction);
		}

		#endregion

		private void ServerGui_FormClosing(object sender, FormClosingEventArgs e)
		{
			m_serverHandler.Stop();
		}

	}
}
