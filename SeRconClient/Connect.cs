﻿using System;
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
	public partial class Connect : Form
	{
		private Client m_parent;

		public Connect(Client pParent)
		{
			InitializeComponent();

			m_parent = pParent;
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			bool isValid = true;
			int port;
			bool portIsInt = int.TryParse(txtPort.Text, out port);

			if(txtAddress.Text.Length == 0)
			{
				txtAddress.BackColor = Color.MistyRose;
				panAddress.BackColor = Color.Red;
				isValid = false;
			}
			else
			{
				txtAddress.BackColor = SystemColors.Window;
				panAddress.BackColor = SystemColors.Window;
			}
			if (txtPort.Text.Length == 0 || txtPort.Text.Length > 5 || !IsDigitsOnly(txtPort.Text))
			{
				txtPort.BackColor = Color.MistyRose;
				panPort.BackColor = Color.Red;
				isValid = false;
			}
			else
			{
				txtPort.BackColor = SystemColors.Window;
				panPort.BackColor = SystemColors.Window;
			}

			if(isValid)
			{
				m_parent.ConnectToServer(txtAddress.Text, txtPort.Text);
				this.Close();
			}
		}

		private bool IsDigitsOnly(string str)
		{
			foreach (char c in str)
			{
				if (c < '0' || c > '9')
					return false;
			}

			return true;
		}
	}
}
