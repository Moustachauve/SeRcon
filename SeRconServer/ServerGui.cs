﻿using System;
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
			m_serverHandler.OnAuthenticationRequest += m_serverHandler_OnAuthenticationRequest;
			m_serverHandler.OnSaltRequested += m_serverHandler_OnSaltRequested;

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

		private void OnClientConnect(TcpEventArgs e)
		{
			var userInfo = (User)e.Client.Tag;

			logViewer.WriteLine(userInfo.Ip + " connected");
			m_serverHandler.Notify("Welcome to this server.", e.Client);
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

		private void OnClientDisconnect(TcpEventArgs e)
		{
			var userInfo = (User)e.Client.Tag;

			logViewer.WriteLine(userInfo.Ip + " disconnected");
			m_serverHandler.NotifyAllAdmin(userInfo.Ip + " disconnected");
		}

		#endregion

		#region OnAuthenticationRequest

		void m_serverHandler_OnAuthenticationRequest(object sender, AuthenticationFeedbackArgs e)
		{
			if (InvokeRequired)
			{
				this.Invoke((MethodInvoker)delegate
				{
					OnAuthenticationRequest(e);
				});
			}
			else
			{
				OnAuthenticationRequest(e);
			}
		}

		private void OnAuthenticationRequest(AuthenticationFeedbackArgs e)
		{
			var userInfo = (User)e.Client.Tag;

			string message = "";

			switch (e.Result)
			{
				case AuthenticationResult.Failed:
					message = " tried to log in with invalid password";
					break;
				case AuthenticationResult.Success:
					message = " successfuly logged in";
					m_serverHandler.NotifyAllAdmin(userInfo.Ip + " logged in", e.Client);
					break;
				case AuthenticationResult.RequestExpired:
					message = " tried to log in with expired salt";
					break;
				case AuthenticationResult.Error:
					message = " tried to log in, but an error occured";
					break;
			}

			logViewer.WriteLine(userInfo.Ip + message);
		}

		#endregion

		#region OnSaltRequested

		void m_serverHandler_OnSaltRequested(object sender, EventArgs e)
		{
			if (InvokeRequired)
			{
				this.Invoke((MethodInvoker)delegate
				{
					OnSaltRequested(e);
				});
			}
			else
			{
				OnSaltRequested(e);
			}
		}

		private void OnSaltRequested(EventArgs e)
		{
			logViewer.WriteLine("Someone requested a salt");
		}

		#endregion

		private void ServerGui_FormClosing(object sender, FormClosingEventArgs e)
		{
			m_serverHandler.Stop();
		}

	}
}
