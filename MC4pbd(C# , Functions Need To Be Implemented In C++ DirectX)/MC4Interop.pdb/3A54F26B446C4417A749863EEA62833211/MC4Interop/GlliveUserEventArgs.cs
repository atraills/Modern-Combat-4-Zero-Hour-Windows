using System;

namespace MC4Interop
{
	public class GlliveUserEventArgs : EventArgs
	{
		private int status;

		private bool rememberme;

		private string username;

		private string password;

		public bool IsRememberMe
		{
			get
			{
				return this.rememberme;
			}
		}

		public string Password
		{
			get
			{
				return this.password;
			}
		}

		public int Status
		{
			get
			{
				return this.status;
			}
		}

		public string UserName
		{
			get
			{
				return this.username;
			}
		}

		public GlliveUserEventArgs(int t, bool r, string u, string p)
		{
			this.status = t;
			this.rememberme = r;
			this.username = u;
			this.password = p;
		}
	}
}