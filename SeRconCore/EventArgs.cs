using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeRconCore.User;

namespace SeRconCore
{
	#region NotificationReceived

	/// <summary>
	/// Represent information relative to a notification
	/// </summary>
	public class NotificationReceivedArgs : EventArgs
    {
        private readonly string m_notification;
        private readonly string m_type;
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

	#region Login Feedback

	/// <summary>
	/// Represent the result of a logging request
	/// </summary>
	public class LoggingFeedbackArgs : EventArgs
	{
		private readonly bool m_succeeded;

		public bool Succeeded
		{
			get { return m_succeeded; }
		}

		public LoggingFeedbackArgs(bool pSucceeded)
		{
			m_succeeded = pSucceeded;
		}
	}

	#endregion
}
