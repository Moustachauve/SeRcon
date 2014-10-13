using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltarNet;

namespace SeRconCore
{
	#region NotificationReceived

	/// <summary>
	/// Represent information relative to a notification
	/// </summary>
	public class NotificationReceivedArgs : EventArgs
	{
		private string m_notification;
		private string m_type;

		public string Message
		{
			get { return m_notification; }
		}

		public string Type
		{
			get { return m_type; }
		}

		public NotificationReceivedArgs(string pMessage, string type)
		{
			m_notification = pMessage;
			m_type = type;
		}
	}

	#endregion

	#region Authentication

	/// <summary>
	/// Represent the result of a logging request
	/// </summary>
	public class AuthenticationFeedbackArgs : EventArgs
	{
		private bool m_succeeded;
		private TcpClientInfo m_sender;

		/// <summary>
		/// True if successfuly connected, or false if connection was refused
		/// </summary>
		public bool Succeeded
		{
			get { return m_succeeded; }
		}

		/// <summary>
		/// Get the client who asked to authenticate to the server
		/// </summary>
		public TcpClientInfo Client
		{
			get { return m_sender; }
		}

		public AuthenticationFeedbackArgs(bool pSucceeded, TcpClientInfo pClient)
		{
			m_succeeded = pSucceeded;
			m_sender = pClient;
		}
	}

	#endregion
}
