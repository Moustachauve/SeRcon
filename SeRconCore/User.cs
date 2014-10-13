using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace SeRconCore
{
	public class User
	{
		#region Attributes

		private IPAddress m_ip;
		private bool m_isLoggedIn;

		#endregion

		#region Properties

		/// <summary>
		/// Get the client Ip address
		/// </summary>
		public IPAddress Ip
		{
			get { return m_ip; }
		}

		/// <summary>
		/// Determine whether the user has authenticate or not
		/// </summary>
		public bool IsLoggedIn
		{
			get { return m_isLoggedIn; }
			internal set
			{
				m_isLoggedIn = value;
			}
		}

		#endregion

		#region Constructor

		public User(IPAddress pIp)
		{
			m_ip = pIp;
		}

		#endregion
	}
}
