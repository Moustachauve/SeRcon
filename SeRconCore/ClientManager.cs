using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
		public event EventHandler<AuthenticationFeedbackArgs> OnLoggingFeedback;

		/// <summary>
		/// Occurs when the server send the session salt after connecting
		/// </summary>
		public event EventHandler<EventArgs> OnSaltReceived;

		#endregion

		#region Attributes

		private TcpClientHandler m_client;
		private IPAddress m_ip;
		private int m_port;

		private byte[] m_salt;

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
			set
			{
				if (IsConnected)
					throw new InvalidOperationException("Can't change server port while connected to a server");
				m_port = value;
			}
		}

		/// <summary>
		/// Get the byte array used during this session by the server to salt sensitive data
		/// </summary>
		public byte[] SessionSalt
		{
			get { return m_salt; }
		}

		/// <summary>
		/// Determine whether the client is connected to a server or not
		/// </summary>
		public bool IsConnected
		{
			get { return m_client != null; }
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

		#region Disconnected from server

		private void m_client_Disconnected(object sender, TcpEventArgs e)
		{
			m_client = null;
			if (OnDisconnected != null)
			{
				OnDisconnected(this, e);
			}
		}

		#endregion

		private void m_client_ReceivedFull(object sender, TcpReceivedEventArgs e)
		{
			var commandReceived = (CommandType)e.Data[0];

			switch (commandReceived)
			{
				case CommandType.SaltRequest:
					SessionSaltReceived(e.Data);
					break;
				case CommandType.Notification:
					NotificationReceived(e.Data);
					break;
				case CommandType.Login:
					LoginFeedbackHandler(e.Data);
					break;
			}

		}

		#region Command Handler

		#region Session salt

		private void SessionSaltReceived(byte[] pData)
		{
			int saltLength = pData[1];

			m_salt = new byte[saltLength];
			Array.Copy(pData, 2, m_salt, 0, saltLength);

			if (OnSaltReceived != null)
			{
				OnSaltReceived(this, null);
			}
		}

		#endregion

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

		private void LoginFeedbackHandler(byte[] pData)
		{
			AuthenticationResult result = (AuthenticationResult)pData[1];
			m_isLoggedIn = result == AuthenticationResult.Success;

			if (OnLoggingFeedback != null)
			{
				OnLoggingFeedback(this, new AuthenticationFeedbackArgs(result, m_client.InfoHandler));
			}
		}

		#endregion

		#endregion

		#endregion

		/// <summary>
		/// Send a logging request to the server
		/// </summary>
		/// <param name="pUsername">Username of the user</param>
		/// <param name="pPassword">Password of the user</param>
		public async void SendLoggingRequest(string pPassword)
		{
			if (!IsConnected)
				throw new InvalidOperationException("Can't send a logging request while not connected to a server");
			if (IsLoggedIn)
				throw new InvalidOperationException("Can't send a logging resquest while already logged in");

			//Ask the server for a new salt
			m_salt = null;
			m_client.Send(new byte[] { (byte)CommandType.SaltRequest });

			//Wait until we received the session salt from the server
			await Task.Run(() => 
			{
				int timeWaiting = 0;
				while (m_salt == null && timeWaiting < 15000) 
				{
					Thread.Sleep(25);
					timeWaiting += 25;
				}; 
			});

			if(m_salt == null)
			{
				if(OnLoggingFeedback != null)
					OnLoggingFeedback(this, new AuthenticationFeedbackArgs(AuthenticationResult.Error, m_client.InfoHandler));

				return;
			}

			SHA256Managed hashstring = new SHA256Managed();
			byte[] password = new byte[pPassword.Length + m_salt.Length]; //= Encoding.UTF8.GetBytes(pPassword);
			Encoding.UTF8.GetBytes(pPassword).CopyTo(password, 0);
			Array.Copy(m_salt, 0, password, pPassword.Length, m_salt.Length);

			byte[] hashedPassword = hashstring.ComputeHash(password);

			byte[] command = new byte[hashedPassword.Length + 2];
			command[0] = (byte)CommandType.Login;
			command[1] = (byte)hashedPassword.Length;

			hashedPassword.CopyTo(command, 2);

			m_client.Send(command);
		}

	}
}
