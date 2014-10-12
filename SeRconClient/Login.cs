using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeRconClient
{
	//TODO: check if logging request succeed direclty in this form
	//Only close it if user cancel or if logging request succeed
	public partial class Login : Form
	{
		private Client m_parent;

		public Login(Client pParent)
		{
			InitializeComponent();

			m_parent = pParent;
		}

		private void btnLogIn_Click(object sender, EventArgs e)
		{
			bool isValid = true;

			if(txtUsername.Text.Length == 0)
			{
				txtUsername.BackColor = Color.MistyRose;
				panAddress.BackColor = Color.Red;
				isValid = false;
			}
			else
			{
				txtUsername.BackColor = SystemColors.Window;
				panAddress.BackColor = SystemColors.Window;
			}
			if (txtPassword.Text.Length == 0)
			{
				txtPassword.BackColor = Color.MistyRose;
				panPassword.BackColor = Color.Red;
				isValid = false;
			}
			else
			{
				txtPassword.BackColor = SystemColors.Window;
				panPassword.BackColor = SystemColors.Window;
			}

			if(isValid)
			{
				m_parent.RequestLogging(txtUsername.Text, txtPassword.Text);
				this.Close();
			}
		}
	}
}
