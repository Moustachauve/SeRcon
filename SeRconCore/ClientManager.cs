using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using AltarNet;

namespace SeRconCore
{
	public class ClientManager
	{
		#region Event definitions

		public event EventHandler<TcpEventArgs> OnDisconnected;
		public event EventHandler<NotificationReceivedArgs> OnMessageReceived;
		public event EventHandler<LoginFeedbackArgs> OnLogingFeedback;

		#endregion

		#region Attributes

		private TcpClientHandler m_client;
		private IPAddress m_ip;
		private int m_port;

		private bool m_isLoggedIn;

		private Exception m_lastConnectionError;

		#endregion

		#region Properties

		public IPAddress Ip
		{
			get { return m_ip; }
			set { m_ip = value; }
		}

		public int Port
		{
			get { return m_port; }
			set { m_port = value; }
		}

		public bool IsConnected
		{
			get { return m_client != null; }
		}

		public bool IsLoggedIn
		{
			get { return m_isLoggedIn; }
		}

		public Exception LastConnectionError
		{
			get { return m_lastConnectionError; }
		}

		#endregion

		#region Constructor

		public ClientManager(IPAddress pIp, int pPort)
		{
			m_ip = pIp;
			m_port = pPort;
		}

		#endregion

		#region Connect / Stop

		public bool Connect()
		{
			if (IsConnected)
			{
				Disconnect();
			}
			m_client = new TcpClientHandler(m_ip, m_port);
			if (m_client.Connect())
			{
				m_client.Disconnected += m_client_Disconnected;
				m_client.ReceivedFull += m_client_ReceivedFull;
				return true;
			}

			m_lastConnectionError = m_client.LastConnectError;
			m_client = null;
			return false;
		}

		public void Disconnect()
		{
			if (!IsConnected)
				return;
			m_client.Disconnect();
			m_client = null;
			m_isLoggedIn = false;
		}

		#endregion

		#region Events

		void m_client_Disconnected(object sender, TcpEventArgs e)
		{
			m_client = null;
			if (OnDisconnected != null)
			{
				OnDisconnected(this, e);
			}
		}

		void m_client_ReceivedFull(object sender, TcpReceivedEventArgs e)
		{
			var commandReceived = (CommandType)e.Data[0];

			switch (commandReceived)
			{
				case CommandType.Notification:
					NotificationReceived(e.Data);
					break;
				case CommandType.Broadcast:
					break;
				case CommandType.Login:
					LoginFeedbackHandler(e);
					break;
				case CommandType.Error:
					break;
				default:
					break;
			}

		}

		#region Command Handler

		#region Notification

		private void NotificationReceived(byte[] data)
		{
			if (OnMessageReceived != null)
			{
				string message = Encoding.UTF8.GetString(data, 1, data.Length - 1);
				OnMessageReceived(this, new NotificationReceivedArgs(message, "[SERVER] "));
			}
		}

		#endregion

		#region Login Feedback

		private void LoginFeedbackHandler(TcpReceivedEventArgs e)
		{
			m_isLoggedIn = e.Data[1] == 1;

			if(OnLogingFeedback != null)
			{
				OnLogingFeedback(this, new LoginFeedbackArgs(m_isLoggedIn));
			}
		}

		#endregion

		#endregion

		#endregion

		[Obsolete("This method is only there for testing purpose to test communication with server. Do not use.")]
		public void SendMessage()
		{
			byte[] data = Command.PrefixCommand(CommandType.Notification, "Hello World!");
			m_client.Send(data);
		}

		public void SendLogin(string pUsername, string pPassword)
		{
			byte[] username = Encoding.UTF8.GetBytes(pUsername);
			byte[] Password = Encoding.UTF8.GetBytes(pPassword);

			byte[] command = new byte[username.Length + Password.Length + 3];
			command[0] = (byte)CommandType.Login;

			command[1] = (byte)username.Length;
			username.CopyTo(command, 2);

			command[username.Length + 2] = (byte)Password.Length;
			Password.CopyTo(command, username.Length + 3);

			m_client.Send(command);
		}

	}
}
