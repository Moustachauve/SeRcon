using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeRconCore
{
	internal class SaltContainer
	{
		#region Attributes

		private byte[] m_salt;
		private DateTime m_expiration;

		#endregion

		#region Properties

		public byte[] Salt
		{
			get { return m_salt; }
		}

		public DateTime Expiration
		{
			get { return m_expiration; }
		}

		#endregion

		#region Constructor

		public SaltContainer(byte[] pSalt, DateTime pExpiration)
		{
			m_salt = pSalt;
			m_expiration = pExpiration;
		}

		#endregion
	}
}
