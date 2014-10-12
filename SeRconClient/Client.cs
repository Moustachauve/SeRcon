using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using AltarNet;
using SeRconCore;
using SeRconCore.Control;

namespace SeRconClient
{
	public partial class Client : Form
	{
		ClientManager m_clientManager;

		public Client()
		{
			InitializeComponent();

			m_clientManager = new ClientManager();
			m_clientManager.OnDisconnected += m_clientManager_OnDisconnected;
		}

		#region Client event handler

		void m_clientManager_OnDisconnected(object sender, TcpEventArgs e)
		{
			if (InvokeRequired)
			{
				this.Invoke((MethodInvoker)delegate
				{
					OnDisconnected(e);
				});
			}
			else
			{
				OnDisconnected(e);
			}
		}

		private void OnDisconnected(TcpEventArgs e)
		{
			lblStatus.Text = "Connexion to " + m_clientManager.Ip + ":" + m_clientManager.Port + " was lost";
			lgvConsole.WriteLine("Connexion to " + m_clientManager.Ip + ":" + m_clientManager.Port + " was lost");
			UpdateElementConnected();
		}

		#endregion

		#region MNU

		#region File

		private void mnu_file_exit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		#endregion

		private void mnu_server_connect_Click(object sender, EventArgs e)
		{
			new Connect(this).ShowDialog();
		}

		#endregion

		#region Form update

		/// <summary>
		/// Update the form depending if the user is connected or not
		/// </summary>
		private void UpdateElementConnected()
		{
			mnu_server_connect.Enabled = !m_clientManager.IsConnected;
			mnu_server_disconnect.Enabled = m_clientManager.IsConnected;
		}

		#endregion

		#region Connection to server

		/// <summary>
		/// Connect this client to a server
		/// </summary>
		/// <param name="address">Address of the server</param>
		/// <param name="port">Port of the server</param>
		public async void ConnectToServer(string address, string port)
		{
			lblStatus.Text = "Connecting to " + address + ":" + port + "...";
			lgvConsole.WriteLine("Connecting to " + address + ":" + port + "...");

			mnu_server_connect.Enabled = false;

			IPAddress serverIp;
			if (!IPAddress.TryParse(address, out serverIp))
			{
				try
				{
					IPAddress[] ipArr = Dns.GetHostAddresses(address);
					foreach (IPAddress currIp in ipArr)
					{
						if (currIp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
						{
							serverIp = currIp;
							break;
						}
					}
				}
				catch (Exception)
				{

					lgvConsole.WriteLine("Could not resolve " + address + " to an Ip address.", MessageType.Error);
					lblStatus.Text = "Could not resolve " + address + " to an Ip address.";

					UpdateElementConnected();

					return;
				}
			}

			m_clientManager.Ip = serverIp;
			m_clientManager.Port = int.Parse(port);

			bool isSuccessful = await m_clientManager.ConnectAsync();

			if (isSuccessful)
			{
				lblStatus.Text = "Connected to " + address + ":" + port;
				lgvConsole.WriteLine("Connected to " + address + ":" + port);
			}
			else
			{
				lblStatus.Text = "Failed to connect to " + address + ":" + port;
				lgvConsole.WriteLine("Failed to connect to " + address + ":" + port, MessageType.Error);
				lgvConsole.Write(m_clientManager.LastConnectionError.Message);
			}

			UpdateElementConnected();
		}

		#endregion
	}
}
