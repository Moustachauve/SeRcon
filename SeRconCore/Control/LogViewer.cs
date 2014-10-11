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
			WriteLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">");
			WriteLine("<html lang=\"en\">");
			WriteLine("<head>");
			WriteLine("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">");
			WriteLine("<title>" + DateTime.Now.ToString() + "</title>");
			//WriteLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"Style/log.css\">");
			WriteLine("<link rel=\"StyleSheet\" HREF=\"file:///" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("\\", "/") + m_stylesheet + "\" />");
			WriteLine("</head>");
			WriteLine("<body>");

		}

		private void WriteLine(string text)
		{
			Document.Write(text + Environment.NewLine);
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
		/// Write a message in the viewer
		/// </summary>
		/// <param name="pMessage">The message to insert in the viewer</param>
		public void Write(string pMessage)
		{
			WriteLine("<div> " + pMessage + "</div>");
		}

		/// <summary>
		/// Write a message in the viewer with the correct prefix and the time
		/// </summary>
		/// <param name="pMessage">The message</param>
		public void Write(string pMessage, MessageType pMessageType)
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
				case MessageType.GuestAction:
					html += "<span class='prefix error'>[GUEST]</span> ";
					break;
				case MessageType.AdminAction:
					html += "<span class='prefix error'>[ADMIN]</span> ";
					break;
			}
			html += pMessage;

			Write(html);
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			this.ResumeLayout(false);

		}
	}

	public enum MessageType
	{
		Notification,
		Error,
		GuestAction,
		AdminAction
	}
}
