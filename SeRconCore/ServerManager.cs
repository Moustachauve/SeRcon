using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using AltarNet;
using SeRconCore.User;

namespace SeRconCore
{
	public class ServerManager
	{

		#region Event definitions

		/// <summary>
		/// Occurs when a client connect to the server
		/// </summary>
		public event EventHandler<TcpEventArgs> OnClientConnected;

		/// <summary>
		/// Occurs when a client disconnect from the server
		/// </summary>
		public event EventHandler<TcpEventArgs> OnClientDisonnected;

		[Obsolete("This method is for testing purpose and will be removed")]
		/// <summary>
		/// Occurs when a message is received from an other client
		/// </summary>
		public event EventHandler<NotificationReceivedArgs> OnMessageReceived;

		#endregion

		#region Attributes

		private TcpServerHandler m_server;
		private IPAddress m_ip;
		private int m_port;

		private byte[] m_sessionSalt;

		private byte[] m_serverPassword;
		private ulong m_currUserIndex;

		#endregion

		#region Properties

		/// <summary>
		/// Get or set the Ip address the Rcon will listen to
		/// </summary>
		public IPAddress Ip
		{
			get { return m_ip; }
			set
			{
				if (IsRunning)
					throw new InvalidOperationException("Can't change server listen Ip while server is running");
				m_ip = value;
			}
		}

		/// <summary>
		/// Get or set the port the Rcon will listen to
		/// </summary>
		public int Port
		{
			get { return m_port; }
			set
			{
				if (IsRunning)
					throw new InvalidOperationException("Can't change server listen port while server is running");
				m_port = value;
			}
		}

		/// <summary>
		/// Get the byte array used during this session to salt sensitive data
		/// </summary>
		public byte[] SessionSalt
		{
			get { return m_sessionSalt; }
		}

		/// <summary>
		/// Determine whether the Rcon server is running or not
		/// </summary>
		public bool IsRunning
		{
			get { return m_server != null; }
		}

		/// <summary>
		/// Get a list of all the client that are connected to the Rcon server
		/// </summary>
		public ICollection<TcpClientInfo> ConnectedClients
		{
			get
			{
				if (!IsRunning)
					throw new InvalidOperationException("Can't get list of client while the server is not running");

				return m_server.Clients.Values;
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Create a new Rcon server manager that listen to all Ip address with the default port (8888)
		/// </summary>
		public ServerManager()
		{
			m_ip = IPAddress.Any;
			m_port = 8888;

			m_sessionSalt = GenerateRandomSalt(10);

			//TODO: Load password from config file
			SHA256 sha256 = SHA256.Create();

			byte[] password = Encoding.UTF8.GetBytes("test");
			byte[] passwordSalted = new byte[10 + password.Length];

			Array.Copy(password, 0, passwordSalted, 0, password.Length);
			Array.Copy(m_sessionSalt, 0, passwordSalted, password.Length, m_sessionSalt.Length);

			m_serverPassword = sha256.ComputeHash(passwordSalted);
		}

		/// <summary>
		/// Create a new Rcon server manager with a specific Ip address and port
		/// </summary>
		/// <param name="pIp">Ip address to listen</param>
		/// <param name="pPort">Port to listen</param>
		public ServerManager(IPAddress pIp, int pPort)
		{
			m_ip = pIp;
			m_port = pPort;

			//TODO: Load admin list from file
			SHA256 sha256 = SHA256.Create();
			m_serverPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes("test"));
		}

		#endregion

		#region Start / Stop

		/// <summary>
		/// Start the Rcon server and begin to listen for connection
		/// </summary>
		public void Start()
		{
			if (IsRunning)
				return;

			m_currUserIndex = 0;
			m_server = new TcpServerHandler(m_ip, m_port);

			//TODO: Make ssl certification
			//m_server.SSLServerCertificate = SslHelper.GetOrCreateSelfSignedCertificate("NameHere");
			m_server.Connected += m_server_Connected;
			m_server.Disconnected += m_server_Disconnected;
			m_server.ReceivedFull += m_server_ReceivedFull;
			m_server.Start();
		}

		/// <summary>
		/// Close all connection to the Rcon server and stop it
		/// </summary>
		public void Stop()
		{
			if (!IsRunning)
				return;
			this.NotifyAll("Server is shutting down...");

			m_server.Connected -= m_server_Connected;
			m_server.Disconnected -= m_server_Disconnected;
			m_server.ReceivedFull -= m_server_ReceivedFull;

			m_server.DisconnectAll();
			m_server.Stop();
			m_server = null;
		}

		#endregion

		#region Event listeners

		#region OnClientConnect

		private void m_server_Connected(object sender, TcpEventArgs e)
		{
			UserInfo client = new UserInfo(m_currUserIndex++, "Guest");
			e.Client.Tag = client;

			byte[] message = new byte[m_sessionSalt.Length + 2];
			message[0] = (byte)CommandType.SessionSalt;
			message[1] = (byte)m_sessionSalt.Length;
			Array.Copy(m_sessionSalt, 0, message, 2, m_sessionSalt.Length);

			m_server.Send(e.Client, message);

			if (OnClientConnected != null)
			{
				OnClientConnected(this, e);
			}
		}

		#endregion

		#region OnCliendDisconnect

		private void m_server_Disconnected(object sender, TcpEventArgs e)
		{
			if (OnClientDisonnected != null)
			{
				OnClientDisonnected(this, e);
			}
		}

		#endregion

		#region OnCommandReceived

		private void m_server_ReceivedFull(object sender, TcpReceivedEventArgs e)
		{
			var commandReceived = (CommandType)e.Data[0];
			switch (commandReceived)
			{
				case CommandType.Notification:
					NotificationReceived(e.Data);
					break;
				case CommandType.Login:
					LoginRequest(e);
					break;
				case CommandType.Error:
					break;
			}
		}

		#endregion

		#region Command Handler

		#region Login Request

		private void LoginRequest(TcpReceivedEventArgs e)
		{
			var user = (UserInfo)e.Client.Tag;			

			byte passwordLenght = e.Data[1];
			byte[] password = new byte[passwordLenght];
			Array.Copy(e.Data, 2, password, 0, passwordLenght);

			user.IsLoggedIn = m_serverPassword.SequenceEqual(password);

			byte[] command = new byte[2];
			command[0] = (byte)CommandType.Login;
			command[1] = (byte)(user.IsLoggedIn ? 1 : 0);
			m_server.Send(e.Client, command);
		}

		#endregion

		[Obsolete("This method is only used for testing purpose")]
		private void NotificationReceived(byte[] data)
		{
			if (OnMessageReceived != null)
			{
				string message = Encoding.UTF8.GetString(data, 1, data.Length - 1);
				OnMessageReceived(this, new NotificationReceivedArgs(message, "Client"));
			}
		}

		#endregion

		#endregion

		#region Server commands

		/// <summary>
		/// Send a notification to all logged-in user
		/// </summary>
		/// <param name="pMessage">Message to be sent</param>
		public void NotifyAllAdmin(string pMessage)
		{
			if (!IsRunning)
				throw new InvalidOperationException("Can't send a notification while server is not running");

			foreach (var currClient in ConnectedClients)
			{
				var clientInfo = (UserInfo)currClient.Tag;
				if(clientInfo.IsLoggedIn)
				{
					Notify(pMessage, currClient);
				}
			}
		}

		/// <summary>
		/// Send a notification to all connected clients
		/// </summary>
		/// <param name="pMessage">Message to be sent</param>
		public void NotifyAll(string pMessage)
		{
			if (!IsRunning)
				throw new InvalidOperationException("Can't send a notification while server is not running");

			byte[] data = Command.PrefixCommand(CommandType.Notification, pMessage);
			m_server.SendAll(data);
		}

		/// <summary>
		/// Send a notification to a client
		/// </summary>
		/// <param name="pMessage">Message to be sent</param>
		/// <param name="pClient">Client to be notified</param>
		public void Notify(string pMessage, TcpClientInfo pClient)
		{
			if (!IsRunning)
				throw new InvalidOperationException("Can't send a notification while server is not running");

			byte[] data = Command.PrefixCommand(CommandType.Notification, pMessage);
			m_server.Send(pClient, data);
		}

		#endregion

		private byte[] GenerateRandomSalt(int saltLength)
		{
			Random rnd = new Random();
			byte[] salt = new byte[saltLength];
			for (int i = 0; i < saltLength; i++)
			{
				salt[i] = (byte)rnd.Next(0, 255);
			}

			return salt;
		}
	}
}
