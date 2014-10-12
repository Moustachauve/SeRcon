using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AltarNet;

namespace SeRconCore
{
	public class ClientManager
	{
		#region Event definitions

		/// <summary>
		/// Occurs when the connection to the Rcon server is lost
		/// </summary>
		public event EventHandler<TcpEventArgs> OnDisconnected;

		/// <summary>
		/// Occurs when a notification is received from the server
		/// </summary>
		public event EventHandler<NotificationReceivedArgs> OnNotificationReceived;

		/// <summary>
		/// Occurs when the Rcon server answer the logging request
		/// </summary>
		public event EventHandler<LoggingFeedbackArgs> OnLoggingFeedback;

		#endregion

		#region Attributes

		private TcpClientHandler m_client;
		private IPAddress m_ip;
		private int m_port;

		private string m_username;
		private bool m_isLoggedIn;

		private Exception m_lastConnectionError;

		#endregion

		#region Properties

		/// <summary>
		/// Get or set the Ip address of the Rcon server
		/// </summary>
		public IPAddress Ip
		{
			get { return m_ip; }
			set
			{
				if (IsConnected)
					throw new InvalidOperationException("Can't change server Ip address while connected to a server");
				m_ip = value;
			}
		}

		/// <summary>
		/// Get or set the port of the Rcon server
		/// </summary>
		public int Port
		{
			get { return m_port; }
			set {
				if (IsConnected)
					throw new InvalidOperationException("Can't change server port while connected to a server");
				m_port = value;
			}
		}

		/// <summary>
		/// Determine whether the client is connected to a server or not
		/// </summary>
		public bool IsConnected
		{
			get { return m_client != null; }
		}

		/// <summary>
		/// Get the username used to log in the server. Null if not logged in
		/// </summary>
		public string Username
		{
			get { return m_username; }
		}

		/// <summary>
		/// Determine whether the client is logged in to the server or not
		/// </summary>
		public bool IsLoggedIn
		{
			get { return m_isLoggedIn; }
		}

		/// <summary>
		/// Get the last connection error detail
		/// </summary>
		public Exception LastConnectionError
		{
			get { return m_lastConnectionError; }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Create a client with the default server port (8888), but without server Ip address
		/// </summary>
		public ClientManager()
		{
			m_port = 8888;
		}

		/// <summary>
		/// Create a client with a specified server Ip address and server port
		/// </summary>
		/// <param name="pIp"></param>
		/// <param name="pPort"></param>
		public ClientManager(IPAddress pIp, int pPort)
		{
			m_ip = pIp;
			m_port = pPort;
		}

		#endregion

		#region Connect / Disconnect

		/// <summary>
		/// Connect to a server. If already connected to one, it will be disconnected to connect to the new one
		/// </summary>
		/// <returns>True if able to connect, or false if it didn't succeed</returns>
		public bool Connect()
		{
			if (m_ip == null)
				throw new InvalidOperationException("Can not connect to a server if no Ip address is specified");
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

		public async Task<bool> ConnectAsync()
		{
			if (m_ip == null)
				throw new InvalidOperationException("Can not connect to a server if no Ip address is specified");
			if (IsConnected)
			{
				Disconnect();
			}
			m_client = new TcpClientHandler(m_ip, m_port);

			bool isSuccessful = await m_client.ConnectAsync();

			if (isSuccessful)
			{
				m_client.Disconnected += m_client_Disconnected;
				m_client.ReceivedFull += m_client_ReceivedFull;
				return true;
			}

			m_lastConnectionError = m_client.LastConnectError;
			m_client = null;
			return false;
		}

		/// <summary>
		/// Disconnect from current server
		/// </summary>
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

		private void m_client_Disconnected(object sender, TcpEventArgs e)
		{
			m_client = null;
			if (OnDisconnected != null)
			{
				OnDisconnected(this, e);
			}
		}

		private void m_client_ReceivedFull(object sender, TcpReceivedEventArgs e)
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
			if (OnNotificationReceived != null)
			{
				string message = Encoding.UTF8.GetString(data, 1, data.Length - 1);
				OnNotificationReceived(this, new NotificationReceivedArgs(message, "[SERVER] "));
			}
		}

		#endregion

		#region Login Feedback

		private void LoginFeedbackHandler(TcpReceivedEventArgs e)
		{
			m_isLoggedIn = e.Data[1] == 1;

			if(!m_isLoggedIn)
			{
				m_username = null;
			}

			if (OnLoggingFeedback != null)
			{
				OnLoggingFeedback(this, new LoggingFeedbackArgs(m_isLoggedIn));
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

		/// <summary>
		/// Send a logging request to the server
		/// </summary>
		/// <param name="pUsername">Username of the user</param>
		/// <param name="pPassword">Password of the user</param>
		public void SendLoggingRequest(string pUsername, string pPassword)
		{
			if (!IsConnected)
				throw new InvalidOperationException("Can't send a logging request while not connected to a server");
			if (IsLoggedIn)
				throw new InvalidOperationException("Can't send a logging resquest while already logged in");

			m_username = pUsername;

			//TODO: Find a way to secure the password transfer over the internet
			byte[] username = Encoding.UTF8.GetBytes(pUsername);
			byte[] password = Encoding.UTF8.GetBytes(pPassword);

			byte[] command = new byte[username.Length + password.Length + 3];
			command[0] = (byte)CommandType.Login;

			command[1] = (byte)username.Length;
			username.CopyTo(command, 2);

			command[username.Length + 2] = (byte)password.Length;
			password.CopyTo(command, username.Length + 3);

			m_client.Send(command);
		}

	}
}
