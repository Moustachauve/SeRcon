using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeRconCore.Control
{
	public class LogViewer : WebBrowser
	{
		private string m_stylesheet;
		private string m_dateFormat;

		/// <summary>
		/// Get or set the path to the stylesheet relative to the executable
		/// </summary>
		public string StyleSheet
		{
			get { return m_stylesheet; }
			set { m_stylesheet = value; }
		}

		public string DateFormat
		{
			get { return m_dateFormat; }
			set { m_dateFormat = value; }
		}

		public LogViewer()
			: base()
		{
			this.Navigate("about:blank");
			this.AllowNavigation = false;
			this.WebBrowserShortcutsEnabled = false;
			this.AllowWebBrowserDrop = false;

			m_stylesheet = "/Style/log.css";
			m_dateFormat = "yyyy/MM/dd | HH:mm:ss";

			Clear();
		}

		public void Clear()
		{
			Document.OpenNew(false);
			WriteHtml("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">");
			WriteHtml("<html lang=\"en\">");
			WriteHtml("<head>");
			WriteHtml("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">");
			WriteHtml("<title>" + DateTime.Now.ToString() + "</title>");
			//WriteHtml("<link rel=\"stylesheet\" type=\"text/css\" href=\"Style/log.css\">");
			WriteHtml("<link rel=\"StyleSheet\" HREF=\"file:///" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("\\", "/") + m_stylesheet + "\" />");
			WriteHtml("</head>");
			WriteHtml("<body>");
		}

		private void WriteHtml(string html)
		{
			Document.Write(html + Environment.NewLine);
		}

		/// <summary>
		/// Get the current date in a formated html block
		/// </summary>
		/// <returns>String containing the html markup and the current date</returns>
		public string GetHtmlDate()
		{
			string date = DateTime.Now.ToString(m_dateFormat);
			return "<span class='date'>[" + date + "]</span> ";
		}

		/// <summary>
		/// Write a message in the viewer without the date
		/// This method should only be used to add information to another message on another line.
		/// </summary>
		/// <param name="message">The message to insert in the viewer</param>
		public void Write(string message)
		{
			WriteHtml("<div>" + message + "</div>");
		}

		/// <summary>
		/// Write a message in the viewer as a normal message
		/// </summary>
		/// <param name="pMessage">The message to insert in the viewer</param>
		public void WriteLine(string pMessage)
		{
			WriteLine(pMessage, MessageType.Normal);
		}

		/// <summary>
		/// Write a message in the viewer with the correct prefix and the time
		/// </summary>
		/// <param name="pMessage">The message</param>
		public void WriteLine(string pMessage, MessageType pMessageType)
		{
			string html = GetHtmlDate();
			switch (pMessageType)
			{
				case MessageType.Notification:
					html += "<span class='prefix notif'>[NOTIF]</span> ";
					break;
				case MessageType.Error:
					html += "<span class='prefix error'>[ERROR]</span> ";
					break;
				case MessageType.ServerAction:
					html += "<span class='prefix server'>[SERVER]</span> ";
					break;
				case MessageType.GuestAction:
					html += "<span class='prefix guest'>[GUEST]</span> ";
					break;
				case MessageType.AdminAction:
					html += "<span class='prefix admin'>[ADMIN]</span> ";
					break;
			}
			html += pMessage;

			WriteHtml("<div>" + html + "</div>");
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			this.ResumeLayout(false);

		}
	}

	public enum MessageType
	{
		Normal,
		Notification,
		Error,
		ServerAction,
		GuestAction,
		AdminAction
	}
}
