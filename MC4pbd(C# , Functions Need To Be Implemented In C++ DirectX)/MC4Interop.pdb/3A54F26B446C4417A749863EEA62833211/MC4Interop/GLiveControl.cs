using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Navigation;
using System.Windows.Threading;
using Windows.Graphics.Display;
using Windows.Phone.System.Analytics;
using Windows.System;

namespace MC4Interop
{
	public class GLiveControl : UserControl
	{
		public static bool DEBUG;

		public static string DEBUGTAG;

		public bool GLLIVE_STARTED;

		public bool GLLIVE_OKLOADED;

		public string UDID;

		public string LANG;

		public string VER;

		public string DNAME;

		public string GGI_GAME = "53107";

		public string IGP_CODE = "M4W8";

		public string GAME_VERSION = "1.0.1";

		public string TEMPLATE_URL = "https://livewebapp.gameloft.com/glive3d/?udid=UDIDPHONE&lg=LANG&d=DEVICE_IPHONE&f=FIRMWARE_IPHONE&apptype=_html5&GGI=GGI_GAME&igp_code=FROM&game_ver=GAME_VERSION&height=HEIGHT_PHONE&width=WIDTH_PHONE&leftframe=yes&type=WINDOWS&country=CTRY&trophies=TROPHIES_LIST&ver=1.0.1";

		public string HOME_URL;

		public bool isRememberMe;

		public string strUserName = "";

		public string strPassword = "";

		public string trophiesList = "";

		public GlliveUserState GlliveUser;

		public bool isDOMloaded;

		public DispatcherTimer dispatcherTimer = new DispatcherTimer();

		public int timerCounter;

		public int timerLimitSeconds = 15;

		private delegateExitGLLive exitGLLiveFunc;

		private delegateShowGLLiveError showErrorFunc;

		internal Grid LayoutRoot;

		internal Grid ContentPanel;

		internal WebBrowser webBrowser1;

		internal ProgressBar progressBar;

		private bool _contentLoaded;

		static GLiveControl()
		{
			GLiveControl.DEBUG = false;
			GLiveControl.DEBUGTAG = "**GL** ";
		}

		public GLiveControl()
		{
			this.InitializeComponent();
			this.webBrowser1.set_IsEnabled(true);
			this.GlliveUser = new GlliveUserState();
			this.GlliveUser.GlliveUserEvent += new GlliveUserStateHandler(this.GlliveUser_GlliveUserEvent);
			this.GlliveUser.updateUserData(GlliveUserState.LOGIN_CANCEL, this.isRememberMe, this.strUserName, this.strPassword);
			this.UDID = Encryptor.Encrypt_MD5(HostInformation.get_PublisherHostId());
			this.LANG = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToUpper();
			this.VER = Environment.OSVersion.Version.Major.ToString();
			this.DNAME = DeviceExtendedProperties.GetValue("DeviceName").ToString();
			this.dispatcherTimer = new DispatcherTimer();
			this.timerCounter = 0;
			this.dispatcherTimer.set_Interval(TimeSpan.FromSeconds(1));
			this.dispatcherTimer.add_Tick(new EventHandler(this.dispatcherTimer_Tick));
			string str = this.TEMPLATE_URL.Replace("UDIDPHONE", this.UDID);
			str = str.Replace("DEVICE_IPHONE", this.DNAME);
			str = str.Replace("FIRMWARE_IPHONE", this.VER);
			str = str.Replace("GAME_VERSION", this.GAME_VERSION);
			double screenResolution = GLiveControl.GetScreenResolution("HEIGHT");
			str = str.Replace("HEIGHT_PHONE", screenResolution.ToString());
			double num = GLiveControl.GetScreenResolution("WIDTH");
			str = str.Replace("WIDTH_PHONE", num.ToString());
			this.HOME_URL = str;
			bool dEBUG = GLiveControl.DEBUG;
			bool flag = GLiveControl.DEBUG;
			WebBrowserExtensions.ClearCookiesAsync(this.webBrowser1);
			WebBrowserExtensions.ClearInternetCacheAsync(this.webBrowser1);
			this.webBrowser1.set_Visibility(1);
			bool dEBUG1 = GLiveControl.DEBUG;
			this.webBrowser1.set_Width(GLiveControl.GetScreenResolution("WIDTH"));
			this.webBrowser1.set_Height(GLiveControl.GetScreenResolution("HEIGHT"));
		}

		private async void DefaultLaunch(Uri uri)
		{
			if (!await Launcher.LaunchUriAsync(uri))
			{
				bool dEBUG = GLiveControl.DEBUG;
			}
			else if (GLiveControl.DEBUG)
			{
			}
		}

		private void disableTimer()
		{
			this.timerCounter = 0;
			this.dispatcherTimer.Stop();
		}

		private void dispatcherTimer_Tick(object sender, EventArgs e)
		{
			this.timerCounter++;
			if (this.timerCounter >= this.timerLimitSeconds)
			{
				bool dEBUG = GLiveControl.DEBUG;
				this.IsNoNetwork();
			}
			bool flag = GLiveControl.DEBUG;
		}

		public void exitGLlive()
		{
			bool dEBUG = GLiveControl.DEBUG;
			this.exitGLLiveFunc();
		}

		public static string GetDeviceUniqueID()
		{
			object obj = null;
			byte[] numArray = null;
			if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", ref obj))
			{
				numArray = (byte[])obj;
			}
			return Convert.ToBase64String(numArray);
		}

		public static double GetScreenResolution(string param)
		{
			double actualHeight;
			double actualWidth;
			if (DisplayProperties.NativeOrientation != DisplayOrientations.Landscape)
			{
				actualHeight = Application.get_Current().get_Host().get_Content().get_ActualHeight();
				actualWidth = Application.get_Current().get_Host().get_Content().get_ActualWidth();
			}
			else
			{
				actualHeight = Application.get_Current().get_Host().get_Content().get_ActualWidth();
				actualWidth = Application.get_Current().get_Host().get_Content().get_ActualHeight();
			}
			if (param == "WIDTH")
			{
				return actualHeight;
			}
			return actualWidth;
		}

		private void GlliveUser_GlliveUserEvent(object sender, GlliveUserEventArgs e)
		{
			PhoneApplicationService.get_Current().get_State()["GL_state"] = e.Status.ToString();
			PhoneApplicationService.get_Current().get_State()["GL_isrememberme"] = e.IsRememberMe.ToString();
			PhoneApplicationService.get_Current().get_State()["GL_username"] = e.UserName;
			PhoneApplicationService.get_Current().get_State()["GL_password"] = e.Password;
			if (e.Status == GlliveUserState.LOGIN_SUCCESS)
			{
				bool dEBUG = GLiveControl.DEBUG;
			}
			bool flag = GLiveControl.DEBUG;
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Application.LoadComponent(this, new Uri("/MC4Interop;component/GLivePage.xaml", UriKind.Relative));
			this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
			this.ContentPanel = (Grid)base.FindName("ContentPanel");
			this.webBrowser1 = (WebBrowser)base.FindName("webBrowser1");
			this.progressBar = (ProgressBar)base.FindName("progressBar");
		}

		private void IsNoNetwork()
		{
			this.webBrowser1.set_IsEnabled(false);
			this.webBrowser1.NavigateToString("");
			this.disableTimer();
			bool dEBUG = GLiveControl.DEBUG;
			this.showErrorFunc();
		}

		public void openExternalUrl(string externalUrl)
		{
			externalUrl = externalUrl.Replace("external:", "");
			bool dEBUG = GLiveControl.DEBUG;
			this.DefaultLaunch(new Uri(externalUrl));
			this.webBrowser1.Navigate(new Uri(this.HOME_URL, UriKind.Absolute));
		}

		private void PhoneApplicationPage_LayoutUpdated_1(object sender, EventArgs e)
		{
		}

		private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
		{
			this.HOME_URL = this.HOME_URL.Replace("LANG", this.LANG);
			this.HOME_URL = this.HOME_URL.Replace("GGI_GAME", this.GGI_GAME);
			this.HOME_URL = this.HOME_URL.Replace("FROM", this.IGP_CODE);
			this.HOME_URL = this.HOME_URL.Replace("TROPHIES_LIST", this.trophiesList);
			this.startGllive(new Uri(this.HOME_URL, UriKind.Absolute));
		}

		private void PhoneApplicationPage_Unloaded_1(object sender, RoutedEventArgs e)
		{
			this.disableTimer();
		}

		public void SetExitFunc(delegateExitGLLive function)
		{
			this.exitGLLiveFunc = function;
		}

		public void SetShowErrorFunc(delegateShowGLLiveError function)
		{
			this.showErrorFunc = function;
		}

		private void setValueUserData(string message)
		{
			message = message.Replace("setvalue:", "");
			if (message.Contains("isRememberMe"))
			{
				this.isRememberMe = (message.Replace("isRememberMe=", "") != "0" ? true : false);
				bool dEBUG = GLiveControl.DEBUG;
			}
			if (message.Contains("strUserName"))
			{
				this.strUserName = message.Replace("strUserName=", "");
				bool flag = GLiveControl.DEBUG;
			}
			if (message.Contains("strPassword"))
			{
				this.strPassword = message.Replace("strPassword=", "");
				bool dEBUG1 = GLiveControl.DEBUG;
			}
			if (message.Equals("loginFailed"))
			{
				this.GlliveUser.updateUserData(GlliveUserState.LOGIN_FAIL, false, "", "");
				bool flag1 = GLiveControl.DEBUG;
			}
			if (message.Equals("autologin"))
			{
				this.GlliveUser.updateUserData(GlliveUserState.LOGIN_AUTO, true, this.strUserName, this.strPassword);
				bool dEBUG2 = GLiveControl.DEBUG;
			}
			if (message.Equals("permloginok"))
			{
				this.GlliveUser.updateUserData(GlliveUserState.LOGIN_SUCCESS, true, this.strUserName, this.strPassword);
				bool flag2 = GLiveControl.DEBUG;
			}
			if (message.Equals("loginok"))
			{
				this.GlliveUser.updateUserData(GlliveUserState.LOGIN_SUCCESS, false, this.strUserName, this.strPassword);
				bool dEBUG3 = GLiveControl.DEBUG;
			}
		}

		private void startGllive(Uri url)
		{
			this.webBrowser1.Navigate(url);
		}

		public void userLogout()
		{
			this.isRememberMe = false;
			this.strUserName = "";
			this.strPassword = "";
			this.GlliveUser.updateUserData(GlliveUserState.LOGOUT_SUCCESS, this.isRememberMe, this.strUserName, this.strPassword);
		}

		private void webBrowser1_LoadCompleted_1(object sender, NavigationEventArgs e)
		{
			try
			{
				if (this.GLLIVE_STARTED.Equals(false))
				{
					this.disableTimer();
					bool dEBUG = GLiveControl.DEBUG;
					this.GLLIVE_STARTED = true;
					this.webBrowser1.set_Visibility(0);
					this.startGllive(new Uri(this.HOME_URL, UriKind.Absolute));
				}
			}
			catch (Exception exception)
			{
			}
		}

		private void webBrowser1_Loaded_1(object sender, RoutedEventArgs e)
		{
			bool dEBUG = GLiveControl.DEBUG;
		}

		private void webBrowser1_Navigated_1(object sender, NavigationEventArgs e)
		{
			bool dEBUG = GLiveControl.DEBUG;
		}

		private void webBrowser1_Navigating_1(object sender, NavigatingEventArgs e)
		{
			if (!this.dispatcherTimer.get_IsEnabled())
			{
				this.dispatcherTimer.Start();
			}
			if (!NetworkInterface.GetIsNetworkAvailable())
			{
				this.IsNoNetwork();
				bool dEBUG = GLiveControl.DEBUG;
			}
			bool flag = GLiveControl.DEBUG;
			this.progressBar.set_Visibility(0);
			e.get_Uri().AbsoluteUri.ToString();
			string str = e.get_Uri().Scheme.ToString();
			bool dEBUG1 = GLiveControl.DEBUG;
			bool flag1 = GLiveControl.DEBUG;
			if (str == "user" || str == "profile" || str == "game")
			{
				try
				{
					bool dEBUG2 = GLiveControl.DEBUG;
				}
				catch (Exception exception)
				{
					bool flag2 = GLiveControl.DEBUG;
				}
				e.Cancel = true;
			}
			if (str == "exit")
			{
				e.Cancel = true;
				this.exitGLlive();
			}
			this.isDOMloaded = false;
			bool dEBUG3 = GLiveControl.DEBUG;
		}

		private void webBrowser1_NavigationFailed_1(object sender, NavigationFailedEventArgs e)
		{
			if (!this.GLLIVE_STARTED)
			{
				this.IsNoNetwork();
			}
			else
			{
				bool dEBUG = GLiveControl.DEBUG;
				e.set_Handled(false);
				this.webBrowser1.Navigate(new Uri(this.HOME_URL, UriKind.Absolute));
				if (GLiveControl.DEBUG)
				{
					return;
				}
			}
		}

		private void webBrowser1_ScriptNotify_1(object sender, NotifyEventArgs e)
		{
			if (e.get_Value().ToString().Equals("isLOGOUT"))
			{
				bool dEBUG = GLiveControl.DEBUG;
				this.userLogout();
			}
			if (e.get_Value().ToString().StartsWith("external:"))
			{
				this.openExternalUrl(e.get_Value().ToString());
			}
			if (e.get_Value().ToString().Contains("setvalue:"))
			{
				this.setValueUserData(e.get_Value().ToString());
			}
			if (e.get_Value().ToString().Contains("alert:"))
			{
				string str = e.get_Value().ToString();
				MessageBox.Show(str.Replace("alert:", ""));
			}
			if (e.get_Value().ToString().Contains("console:"))
			{
				if (!e.get_Value().ToString().Contains("TypeError: Permission denied"))
				{
					e.get_Value().ToString();
					bool flag = GLiveControl.DEBUG;
				}
				else
				{
					this.IsNoNetwork();
				}
			}
			if (e.get_Value() == "DOMContentLoaded")
			{
				this.disableTimer();
				this.isDOMloaded = true;
				this.progressBar.set_Visibility(1);
				bool dEBUG1 = GLiveControl.DEBUG;
			}
			if (e.get_Value() == "exit:")
			{
				this.exitGLlive();
			}
		}

		private void webBrowser1_Unloaded_1(object sender, RoutedEventArgs e)
		{
			bool dEBUG = GLiveControl.DEBUG;
		}
	}
}