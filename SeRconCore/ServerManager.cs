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

		private ulong m_currUserIndex;

		private Dictionary<string, byte[]> m_adminList;

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

			//TODO: Load admin list from file
			SHA256 sha256 = SHA256.Create();
			m_adminList = new Dictionary<string, byte[]> { { "test", sha256.ComputeHash(Encoding.UTF8.GetBytes("test")) } };
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
			m_adminList = new Dictionary<string, byte[]> { { "test", sha256.ComputeHash(Encoding.UTF8.GetBytes("test")) } };
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

			int usernameLength = e.Data[1];
			string username = Encoding.UTF8.GetString(e.Data, 2, usernameLength);

			int passwordLenght = e.Data[usernameLength + 2];
			string password = Encoding.UTF8.GetString(e.Data, usernameLength + 3, passwordLenght);

			bool success = TryLogIn(username, password);

			if (success)
			{
				user.Name = username;
				user.IsLoggedIn = true;
			}
			byte[] command = new byte[2];
			command[0] = (byte)CommandType.Login;
			command[1] = (byte)(success ? 1 : 0);
			m_server.Send(e.Client, command);
		}

		private bool TryLogIn(string pUsername, string pPassword)
		{
			if (!m_adminList.ContainsKey(pUsername))
				return false;

			byte[] passwordBytes = Encoding.UTF8.GetBytes(pPassword);
			SHA256Managed hashstring = new SHA256Managed();
			byte[] hash = hashstring.ComputeHash(passwordBytes);

			return m_adminList[pUsername].SequenceEqual(hash);
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

	}
}
