using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeRconCore.User
{
    public class UserInfo
    {
        #region Attributes

        private string m_name;
        private readonly ulong m_id;
	    private bool m_isLoggedIn;

        #endregion

        #region Properties

        public ulong Id
        {
            get { return m_id; }
        }

        public string Name
        {
            get { return m_name; }
            internal set { m_name = value; }
        }

	    public bool IsLoggedIn
	    {
			get { return m_isLoggedIn; }
			internal set { m_isLoggedIn = value; }
	    }

        #endregion

        #region Constructor

        public UserInfo(ulong pId, string pName)
        {
            m_id = pId;
            m_name = pName;
        }

        #endregion

        #region Methods

        #endregion
    }
}
