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

		public event EventHandler<TcpEventArgs> OnClientConnected;
		public event EventHandler<TcpEventArgs> OnClientDisonnected;
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

		public bool IsRunning
		{
			get { return m_server != null; }
		}

		public ICollection<TcpClientInfo> ConnectedClients
		{
			get { return m_server.Clients.Values; }
		}

		#endregion

		#region Constructor

		public ServerManager()
		{
			//TODO: Load preference from file
			m_ip = IPAddress.Any;
			m_port = 8888;

			//TODO: Load admin list from file
			SHA256 sha256 = SHA256.Create();
			m_adminList = new Dictionary<string, byte[]> { { "test", sha256.ComputeHash(Encoding.UTF8.GetBytes("test")) } };
		}

		public ServerManager(IPAddress pIp, int pPort)
		{
			m_ip = pIp;
			m_port = pPort;

			//TODO: Load admin list from file
			SHA256 sha256 = SHA256.Create();
			m_adminList = new Dictionary<string, byte[]> { { "test", sha256.ComputeHash(Encoding.UTF8.GetBytes("test")) } };
		}

		~ServerManager()
		{
			Stop();
		}

		#endregion

		#region Start / Stop

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

		public void Stop()
		{
			if (!IsRunning)
				return;
			this.SendToAll("Server is shutting down...");

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

		void m_server_Connected(object sender, TcpEventArgs e)
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

		void m_server_Disconnected(object sender, TcpEventArgs e)
		{
			if (OnClientDisonnected != null)
			{
				OnClientDisonnected(this, e);
			}
		}

		#endregion

		#region OnCommandReceived

		void m_server_ReceivedFull(object sender, TcpReceivedEventArgs e)
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

		public void SendToAll(string pMessage)
		{
            byte[] data = Command.PrefixCommand(CommandType.Notification, pMessage);
			m_server.SendAll(data);
		}

		public void SendTo(TcpClientInfo pClient, string pMessage)
		{
            byte[] data = Command.PrefixCommand(CommandType.Notification, pMessage);
            m_server.Send(pClient, data);
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

	}
}
