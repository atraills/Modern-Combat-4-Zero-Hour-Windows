using System;
using System.Threading;

namespace MC4Interop
{
	public class GlliveUserState
	{
		public static int LOGIN_SUCCESS;

		public static int LOGIN_FAIL;

		public static int LOGIN_AUTO;

		public static int LOGIN_CANCEL;

		public static int LOGOUT_SUCCESS;

		private int status = GlliveUserState.LOGIN_CANCEL;

		private bool isRememberMe;

		private string username = "";

		private string password = "";

		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}

		public bool RememberMe
		{
			get
			{
				return this.isRememberMe;
			}
			set
			{
				this.isRememberMe = value;
			}
		}

		public int Status
		{
			get
			{
				return this.status;
			}
			set
			{
				this.status = value;
			}
		}

		public string Username
		{
			get
			{
				return this.username;
			}
			set
			{
				this.username = value;
			}
		}

		static GlliveUserState()
		{
			GlliveUserState.LOGIN_SUCCESS = 0;
			GlliveUserState.LOGIN_FAIL = 1;
			GlliveUserState.LOGIN_AUTO = 2;
			GlliveUserState.LOGIN_CANCEL = 3;
			GlliveUserState.LOGOUT_SUCCESS = 4;
		}

		public GlliveUserState()
		{
		}

		private void OnGlliveUserChanged(object sender, GlliveUserEventArgs myArgs)
		{
			if (this.GlliveUserEvent != null)
			{
				this.GlliveUserEvent(this, myArgs);
			}
		}

		public void updateUserData(int s, bool i, string u, string p)
		{
			this.status = s;
			this.isRememberMe = i;
			this.username = u;
			this.password = p;
			this.OnGlliveUserChanged(this, new GlliveUserEventArgs(s, i, u, p));
		}

		public event GlliveUserStateHandler GlliveUserEvent;
	}
}