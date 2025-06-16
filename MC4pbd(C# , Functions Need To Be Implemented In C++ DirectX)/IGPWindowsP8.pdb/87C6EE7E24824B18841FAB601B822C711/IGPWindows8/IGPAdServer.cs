using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace IGPWindows8
{
	public class IGPAdServer : IGPBase
	{
		protected string AD_REQUEST_URL_TEMPLATE = "http://ingameads.gameloft.com/redir/ads_server.php?game_code=GAME_CODE&udid=UDID&d=DEVICE_NAME&f=FIRMWARE&lg=LANGUAGE&game_ver=GAMEVERSION&igp_rev=IGPREVISION";

		protected string INTERSTITIAL_REQUEST_URL_TEMPLATE = "http://ingameads.gameloft.com/redir/ads_server.php?game_code=GAME_CODE&udid=UDID&d=DEVICE_NAME&f=FIRMWARE&lg=LANGUAGE&game_ver=GAMEVERSION&igp_rev=IGPREVISION";

		protected string AD_REQUEST_FREE_CASH_URL_TEMPLATE = "http://ingameads.gameloft.com/redir/ads_server.php?game_code=GAME_CODE&udid=UDID&d=DEVICE_NAME&f=FIRMWARE&lg=LANGUAGE&game_ver=GAMEVERSION&freecash=1";

		protected string AD_LINK_VIDEO_TEMPLATE = "http://ingameads.gameloft.com/redir/ads_capping.php?game=GAME_CODE&udid=UDID&igp_rev=IGPREVISION";

		protected string GAMELOFT_AD_URL_TEMPLATE = "http://ingameads.gameloft.com/redir/ads/ads_server_view.php?from=GAME_CODE&lg=LANGUAGE&udid=UDID&d=DEVICE_NAME&f=FIRMWARE&game_ver=GAMEVERSION&igp_rev=IGPREVISION";

		protected string WELCOMESCREEN_URL_TEMPLATE = "http://ingameads.gameloft.com/redir/ads/splashscreen_view.php?from=GAME_CODE&country=COUNTRY&lg=LANGUAGE&udid=UDID&d=DEVICE_NAME&f=FIRMWARE&game_ver=GAMEVERSION&os=winp&igp_rev=IGPREVISION";

		protected string GAMELOFT_INTERSTITIAL_URL_TEMPLATE = "http://ingameads.gameloft.com/redir/ads/interstitial_view.php?from=GAME_CODE&country=COUNTRY&lg=LANGUAGE&udid=UDID&d=DEVICE_NAME&f=FIRMWARE&game_ver=GAMEVERSION&igp_rev=IGPREVISION";

		private static bool bInterstitialResponseReceived;

		private static bool bAdBannerResponseReceived;

		private HorizontalAlignment AdBannerHorizontalAlignment = 1;

		private VerticalAlignment AdBannerVerticalAlignment = 2;

		protected WebBrowser m_AdControl;

		protected WebBrowser m_InterstitialControl;

		protected WebBrowser m_WelcomeScreenControl;

		private bool m_bAdBannerVisibilityVisible;

		private bool m_bInterstitialVisibilityVisible;

		private bool m_bWelcomeScreenVisibilityVisible;

		public override double Height
		{
			get
			{
				return base.Height;
			}
			set
			{
				base.Height = value;
				if (this.m_AdControl != null)
				{
					this.m_AdControl.set_Height(value);
				}
			}
		}

		public override double Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
				if (this.m_AdControl != null)
				{
					this.m_AdControl.set_Width(value);
				}
			}
		}

		static IGPAdServer()
		{
		}

		public IGPAdServer(Panel parent) : base(parent)
		{
			this.RegisterWebBrowser(IGPAdServer.eBrowserTypes.BT_INTERSTITIAL, ref this.m_InterstitialControl, true);
			this.RegisterWebBrowser(IGPAdServer.eBrowserTypes.BT_AD_BANNER, ref this.m_AdControl, true);
			this.RegisterWebBrowser(IGPAdServer.eBrowserTypes.BT_WELCOMESCREEN, ref this.m_WelcomeScreenControl, true);
			this.ScreenWidth = this.ScreenWidth;
			this.ScreenHeight = this.ScreenHeight;
		}

		private bool AwaitForAdResponse()
		{
			while (!IGPAdServer.bAdBannerResponseReceived)
			{
				Task.Delay(100).Wait();
			}
			return this.m_bBannerLoaded;
		}

		public override int GetBannerState(string type)
		{
			if (type.ToLower() == "adbanner")
			{
				if (this.m_AdControl != null && this.m_bAdBannerVisibilityVisible)
				{
					return 4;
				}
				if (this.m_AdControl != null && !this.m_bAdBannerVisibilityVisible && this.m_bBannerLoaded)
				{
					return 3;
				}
				if (this.m_bBannerLoading)
				{
					return 2;
				}
				if (!this.m_bBannerLoaded)
				{
					return 1;
				}
			}
			else if (type.ToLower() == "interstitial")
			{
				if (this.m_InterstitialControl != null && this.m_bInterstitialVisibilityVisible)
				{
					return 4;
				}
				if (this.m_InterstitialControl != null && !this.m_bInterstitialVisibilityVisible && this.m_bInterstitialLoaded)
				{
					return 3;
				}
				if (this.m_bInterstitialLoading)
				{
					return 2;
				}
				if (!this.m_bInterstitialLoaded)
				{
					return 1;
				}
			}
			else if (type.ToLower() == "welcomescreen")
			{
				if (this.m_WelcomeScreenControl != null && this.m_bWelcomeScreenVisibilityVisible)
				{
					return 4;
				}
				if (this.m_WelcomeScreenControl != null && !this.m_bWelcomeScreenVisibilityVisible && this.m_bWelcomeScreenLoaded)
				{
					return 3;
				}
				if (this.m_bWelcomeScreenLoading)
				{
					return 2;
				}
				if (!this.m_bWelcomeScreenLoaded)
				{
					return 1;
				}
			}
			return base.GetBannerState(type);
		}

		protected override string GetTemplate(IGPBase.eTemplateType type)
		{
			switch (type)
			{
				case IGPBase.eTemplateType.TT_REQUEST_AD:
				{
					return this.AD_REQUEST_URL_TEMPLATE;
				}
				case IGPBase.eTemplateType.TT_REQUEST_INTERSTITIAL:
				{
					return this.INTERSTITIAL_REQUEST_URL_TEMPLATE;
				}
				case IGPBase.eTemplateType.TT_REQUEST_FREE_CASH:
				{
					return this.AD_REQUEST_FREE_CASH_URL_TEMPLATE;
				}
				case IGPBase.eTemplateType.TT_LINK_VIDEO:
				{
					return base.GetTemplate(type);
				}
				case IGPBase.eTemplateType.TT_GAMELOFT_AD:
				{
					return this.GAMELOFT_AD_URL_TEMPLATE;
				}
				case IGPBase.eTemplateType.TT_GAMELOFT_INTERSTITIAL:
				{
					return this.GAMELOFT_INTERSTITIAL_URL_TEMPLATE;
				}
				case IGPBase.eTemplateType.TT_WELCOME_SCREEN:
				{
					return this.WELCOMESCREEN_URL_TEMPLATE;
				}
				default:
				{
					return base.GetTemplate(type);
				}
			}
		}

		protected override bool HandleNavigatingString(object sender, string url)
		{
			if (sender == this.m_AdControl)
			{
				if (url.StartsWith("zune:"))
				{
					this.ShowBanner(false);
					return false;
				}
			}
			else if (sender == this.m_WelcomeScreenControl)
			{
				if (url.StartsWith("exit:"))
				{
					this.ShowWelcomeScreen(false);
					return true;
				}
				if (url.StartsWith("play:"))
				{
					return true;
				}
				if (url.StartsWith("goto:"))
				{
					string str = url.Substring("goto:".Length);
					base.SendIGPMessage("goto", str, "WelcomeScreen");
					return true;
				}
				if (url.StartsWith("market://"))
				{
					return true;
				}
				if (url.StartsWith("stk:"))
				{
					return true;
				}
			}
			else if (sender == this.m_InterstitialControl)
			{
				if (url.StartsWith("link:"))
				{
					base.OpenBrowser(url.Substring("link:".Length));
					this.ShowInterstitial(false);
					return true;
				}
				if (url.StartsWith("market:"))
				{
					this.ShowInterstitial(false);
					return true;
				}
				if (url.StartsWith("play:"))
				{
					this.ShowInterstitial(false);
					return true;
				}
				if (url.StartsWith("amzn://"))
				{
					this.ShowInterstitial(false);
					return true;
				}
				if (url.StartsWith("exit:"))
				{
					this.ShowInterstitial(false);
					return true;
				}
				if (url.StartsWith("goto:"))
				{
					this.ShowInterstitial(false);
					return true;
				}
			}
			return base.HandleNavigatingString(sender, url);
		}

		private void LoadAdBannerByRequest(object threadIdx)
		{
			int num = (int)threadIdx;
			this.LoadByRequest(IGPAdServer.eRequestTypes.RT_AD_BANNER);
			if ((int)threadIdx < this.IGPThreads.Count)
			{
				this.IGPThreads.RemoveAt((int)threadIdx);
			}
		}

		private bool LoadAdBannerByType(string type)
		{
			IGPAdServer.bAdBannerResponseReceived = false;
			this.m_bBannerLoaded = false;
			if (type.ToUpper() != "GAMELOFT")
			{
				IGPAdServer.bAdBannerResponseReceived = true;
			}
			else
			{
				string str = this.SolveTemplate(this.GetTemplate(IGPBase.eTemplateType.TT_GAMELOFT_AD));
				object[] width = new object[] { str, "&width=", this.Width, "&height=", this.Height };
				str = string.Concat(width);
				str = base.EncryptURL(str);
				if (this.m_AdControl != null)
				{
					this.m_AdControl.get_Dispatcher().BeginInvoke(() => this.m_AdControl.Navigate(new Uri(str, UriKind.Absolute)));
				}
			}
			while (!IGPAdServer.bAdBannerResponseReceived)
			{
				Thread.Sleep(100);
			}
			return this.m_bBannerLoaded;
		}

		protected override void LoadBanner()
		{
			base.LoadBanner();
			Thread thread = new Thread(new ParameterizedThreadStart(this.LoadAdBannerByRequest));
			int count = this.IGPThreads.Count;
			this.IGPThreads.Add(thread);
			thread.Start(count);
		}

		private void LoadByRequest(IGPAdServer.eRequestTypes type)
		{
			string str = null;
			if (type == IGPAdServer.eRequestTypes.RT_INTERSTITIAL)
			{
				str = this.SolveTemplate(this.GetTemplate(IGPBase.eTemplateType.TT_REQUEST_INTERSTITIAL));
				this.m_bInterstitialLoading = true;
			}
			else if (type == IGPAdServer.eRequestTypes.RT_AD_BANNER)
			{
				str = this.SolveTemplate(this.GetTemplate(IGPBase.eTemplateType.TT_REQUEST_AD));
				this.m_bBannerLoading = true;
			}
			if (str == null)
			{
				return;
			}
			this.HttpPost(base.EncryptURL(str));
			string str1 = "";
			while (true)
			{
				string postResult = this.GetPostResult();
				str1 = postResult;
				if (postResult != null)
				{
					break;
				}
				Thread.Sleep(100);
			}
			if (str1.StartsWith("Failed"))
			{
				return;
			}
			char[] chrArray = new char[] { '\\', '|' };
			string[] strArrays = str1.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				if (type == IGPAdServer.eRequestTypes.RT_AD_BANNER)
				{
					if (this.LoadAdBannerByType(strArrays[i]))
					{
						break;
					}
				}
				else if (type == IGPAdServer.eRequestTypes.RT_INTERSTITIAL && this.LoadInterstitialByType(strArrays[i]))
				{
					break;
				}
			}
			if (type == IGPAdServer.eRequestTypes.RT_AD_BANNER)
			{
				this.m_bBannerLoading = false;
				return;
			}
			if (type == IGPAdServer.eRequestTypes.RT_INTERSTITIAL)
			{
				this.m_bInterstitialLoading = false;
			}
		}

		protected override void LoadInterstitial()
		{
			base.LoadInterstitial();
			Thread thread = new Thread(new ParameterizedThreadStart(this.LoadInterstitialByRequest));
			int count = this.IGPThreads.Count;
			this.IGPThreads.Add(thread);
			thread.Start(count);
		}

		private void LoadInterstitialByRequest(object threadIdx)
		{
			this.LoadByRequest(IGPAdServer.eRequestTypes.RT_INTERSTITIAL);
			if ((int)threadIdx < this.IGPThreads.Count)
			{
				this.IGPThreads.RemoveAt((int)threadIdx);
			}
		}

		private bool LoadInterstitialByType(string type)
		{
			IGPAdServer.bInterstitialResponseReceived = false;
			this.m_bInterstitialLoaded = false;
			if (type.ToUpper() != "GAMELOFT")
			{
				IGPAdServer.bInterstitialResponseReceived = true;
			}
			else
			{
				string str = this.SolveTemplate(this.GetTemplate(IGPBase.eTemplateType.TT_GAMELOFT_INTERSTITIAL));
				object[] screenWidth = new object[] { str, "&width=", this.ScreenWidth, "&height=", this.ScreenHeight };
				str = string.Concat(screenWidth);
				str = base.EncryptURL(str);
				this.m_InterstitialControl.get_Dispatcher().BeginInvoke(() => this.m_InterstitialControl.Navigate(new Uri(str, UriKind.Absolute)));
			}
			while (!IGPAdServer.bInterstitialResponseReceived)
			{
				Thread.Sleep(100);
			}
			return this.m_bInterstitialLoaded;
		}

		protected override void LoadWelcomeScreen()
		{
			base.LoadWelcomeScreen();
			this.m_bWelcomeScreenLoading = true;
			object obj = this.SolveTemplate(this.GetTemplate(IGPBase.eTemplateType.TT_WELCOME_SCREEN));
			object[] screenWidth = new object[] { obj, "&width=", (int)this.ScreenWidth, "&height=", (int)this.ScreenHeight };
			string str = string.Concat(screenWidth);
			string str1 = str;
			str1 = base.EncryptURL(str);
			this.m_WelcomeScreenControl.Navigate(new Uri(str1, UriKind.Absolute));
		}

		private void m_WelcomeScreenControl_Navigated(object sender, NavigationEventArgs e)
		{
		}

		public override void OnDestroy()
		{
			this.RegisterWebBrowser(IGPAdServer.eBrowserTypes.BT_INTERSTITIAL, ref this.m_InterstitialControl, false);
			this.RegisterWebBrowser(IGPAdServer.eBrowserTypes.BT_AD_BANNER, ref this.m_AdControl, false);
			this.RegisterWebBrowser(IGPAdServer.eBrowserTypes.BT_WELCOMESCREEN, ref this.m_WelcomeScreenControl, false);
			base.OnDestroy();
		}

		protected virtual void OnWebBrowserinishedLoading(object sender, NavigationEventArgs e)
		{
			WebBrowser webBrowser = (WebBrowser)sender;
			Visibility visibility = webBrowser.get_Visibility();
			Visibility visibility1 = 0;
			string str = "unknown";
			if (sender == this.m_WelcomeScreenControl)
			{
				if (this.m_bWelcomeScreenVisible)
				{
					this.m_bWelcomeScreenVisibilityVisible = true;
				}
				else
				{
					visibility1 = 1;
					this.m_bWelcomeScreenVisibilityVisible = false;
				}
				this.m_bWelcomeScreenLoaded = true;
				this.m_bWelcomeScreenLoading = false;
				str = "welcomescreen";
			}
			else if (sender == this.m_AdControl)
			{
				if (this.m_bBannerIsVisible)
				{
					this.m_bAdBannerVisibilityVisible = true;
				}
				else
				{
					visibility1 = 1;
					this.m_bAdBannerVisibilityVisible = false;
				}
				this.m_bBannerLoaded = true;
				str = "adbanner";
			}
			else if (sender == this.m_InterstitialControl)
			{
				if (this.m_bInterstitialVisible)
				{
					this.m_bInterstitialVisibilityVisible = true;
				}
				else
				{
					visibility1 = 1;
					this.m_bInterstitialVisibilityVisible = false;
				}
				this.m_bInterstitialLoaded = true;
				str = "interstitial";
			}
			webBrowser.set_Visibility(visibility1);
			if (webBrowser.get_Visibility() != visibility)
			{
				base.SendBannerStateChanged(str, (webBrowser.get_Visibility() == null ? 1 : 0));
			}
			webBrowser.set_IsEnabled(visibility1 == 0);
		}

		protected virtual void OnWebBrowserNavigating(object sender, NavigatingEventArgs e)
		{
			try
			{
				e.Cancel = this.HandleNavigatingString(sender, e.get_Uri().AbsoluteUri);
			}
			catch (Exception exception)
			{
			}
		}

		protected virtual void OnWebBrowserNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			if (sender == this.m_WelcomeScreenControl)
			{
				this.m_bWelcomeScreenLoaded = false;
				this.ShowWelcomeScreen(false);
			}
			else if (sender == this.m_AdControl)
			{
				this.m_bBannerLoaded = false;
				IGPAdServer.bAdBannerResponseReceived = true;
			}
			else if (sender == this.m_InterstitialControl)
			{
				this.m_bInterstitialLoaded = false;
				IGPAdServer.bInterstitialResponseReceived = true;
			}
			e.set_Handled(true);
		}

		protected virtual void OnWebBrowserScriptNotify(object sender, NotifyEventArgs e)
		{
		}

		protected virtual void RegisterWebBrowser(IGPAdServer.eBrowserTypes type, ref WebBrowser brow, bool bRegister)
		{
			if (bRegister)
			{
				brow = new WebBrowser();
				this.SetupWebBrowser(brow);
				brow.set_Margin(new Thickness(0, 0, 0, 0));
				brow.set_Padding(new Thickness(0, 0, 0, 0));
				brow.set_AllowDrop(false);
				brow.set_BorderBrush(null);
				brow.set_HorizontalContentAlignment(1);
				brow.set_VerticalContentAlignment(1);
				brow.set_VerticalAlignment(1);
				brow.set_HorizontalAlignment(1);
				WebBrowserExtensions.ClearInternetCacheAsync(brow);
				IGPAdServer gPAdServer = this;
				brow.add_Navigating(new EventHandler<NavigatingEventArgs>(gPAdServer.OnWebBrowserNavigating));
				IGPAdServer gPAdServer1 = this;
				brow.add_NavigationFailed(new NavigationFailedEventHandler(gPAdServer1, gPAdServer1.OnWebBrowserNavigationFailed));
				IGPAdServer gPAdServer2 = this;
				brow.add_LoadCompleted(new LoadCompletedEventHandler(gPAdServer2, gPAdServer2.OnWebBrowserinishedLoading));
				IGPAdServer gPAdServer3 = this;
				brow.add_ScriptNotify(new EventHandler<NotifyEventArgs>(gPAdServer3.OnWebBrowserScriptNotify));
				brow.set_Visibility(1);
				this.m_Parent.get_Children().Add(brow);
			}
			else if (brow != null)
			{
				this.m_Parent.get_Children().Remove(brow);
				brow = null;
			}
			if (type != IGPAdServer.eBrowserTypes.BT_AD_BANNER)
			{
				if (type == IGPAdServer.eBrowserTypes.BT_INTERSTITIAL)
				{
					this.m_bInterstitialLoaded = false;
					this.m_bInterstitialVisible = false;
					return;
				}
				if (type == IGPAdServer.eBrowserTypes.BT_WELCOMESCREEN)
				{
					this.m_bWelcomeScreenLoaded = false;
					this.m_bWelcomeScreenVisible = false;
				}
			}
			else
			{
				this.m_bBannerLoaded = false;
				this.m_bBannerIsVisible = false;
				if (bRegister)
				{
					brow.set_Width(this.Width);
					brow.set_Height(this.Height);
					brow.set_HorizontalAlignment(this.AdBannerHorizontalAlignment);
					brow.set_VerticalAlignment(this.AdBannerVerticalAlignment);
					return;
				}
			}
		}

		public override void SetAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
		{
			base.SetAlignment(horizontal, vertical);
			this.AdBannerHorizontalAlignment = horizontal;
			this.AdBannerVerticalAlignment = vertical;
			this.m_AdControl.set_HorizontalAlignment(this.AdBannerHorizontalAlignment);
			this.m_AdControl.set_VerticalAlignment(this.AdBannerVerticalAlignment);
		}

		public override void ShowBanner(bool bShow)
		{
			base.ShowBanner(bShow);
			if (bShow)
			{
				this.RegisterWebBrowser(IGPAdServer.eBrowserTypes.BT_AD_BANNER, ref this.m_AdControl, true);
				this.m_bAdBannerVisibilityVisible = false;
				this.m_bBannerIsVisible = true;
				this.LoadBanner();
				return;
			}
			bool flag = false;
			if (this.m_AdControl != null && this.m_AdControl.get_Visibility() == null)
			{
				flag = true;
			}
			this.RegisterWebBrowser(IGPAdServer.eBrowserTypes.BT_AD_BANNER, ref this.m_AdControl, false);
			this.m_bAdBannerVisibilityVisible = false;
			if (flag)
			{
				base.SendBannerStateChanged("adbanner", 0);
			}
		}

		public override void ShowInterstitial(bool bShow)
		{
			if (bShow)
			{
				this.LoadInterstitial();
			}
			base.ShowInterstitial(bShow);
			this.m_InterstitialControl.set_Width(this.m_ScreenW);
			this.m_InterstitialControl.set_Height(this.m_ScreenH);
			if (!this.m_bInterstitialLoaded)
			{
				Visibility visibility = this.m_InterstitialControl.get_Visibility();
				this.m_InterstitialControl.set_Visibility(1);
				this.m_bInterstitialVisibilityVisible = false;
				if (this.m_InterstitialControl.get_Visibility() != visibility)
				{
					base.SendBannerStateChanged("interstitial", (this.m_InterstitialControl.get_Visibility() == null ? 1 : 0));
				}
				this.m_InterstitialControl.set_IsEnabled(false);
				return;
			}
			Visibility visibility1 = this.m_InterstitialControl.get_Visibility();
			this.m_InterstitialControl.set_Visibility((bShow ? 0 : 1));
			this.m_bInterstitialVisibilityVisible = bShow;
			if (this.m_InterstitialControl.get_Visibility() != visibility1)
			{
				base.SendBannerStateChanged("interstitial", (this.m_InterstitialControl.get_Visibility() == null ? 1 : 0));
			}
			this.m_InterstitialControl.set_IsEnabled(bShow);
		}

		public override void ShowWelcomeScreen(bool bShow)
		{
			if (bShow)
			{
				this.LoadWelcomeScreen();
			}
			base.ShowWelcomeScreen(bShow);
			this.m_WelcomeScreenControl.set_Width(this.ScreenWidth);
			this.m_WelcomeScreenControl.set_Height(this.ScreenHeight);
			if (!this.m_bWelcomeScreenLoaded)
			{
				Visibility visibility = this.m_WelcomeScreenControl.get_Visibility();
				this.m_WelcomeScreenControl.set_Visibility(1);
				this.m_bWelcomeScreenVisibilityVisible = false;
				if (this.m_WelcomeScreenControl.get_Visibility() != visibility)
				{
					base.SendBannerStateChanged("welcomescreen", (this.m_WelcomeScreenControl.get_Visibility() == null ? 1 : 0));
				}
				this.m_WelcomeScreenControl.set_IsEnabled(false);
				return;
			}
			Visibility visibility1 = this.m_WelcomeScreenControl.get_Visibility();
			this.m_WelcomeScreenControl.set_Visibility((bShow ? 0 : 1));
			this.m_bWelcomeScreenVisibilityVisible = bShow;
			if (this.m_WelcomeScreenControl.get_Visibility() != visibility1)
			{
				base.SendBannerStateChanged("welcomescreen", (this.m_WelcomeScreenControl.get_Visibility() == null ? 1 : 0));
			}
			this.m_WelcomeScreenControl.set_IsEnabled(bShow);
		}

		public override void Start()
		{
		}

		protected enum eBrowserTypes
		{
			BT_AD_BANNER,
			BT_INTERSTITIAL,
			BT_WELCOMESCREEN
		}

		private enum eRequestTypes
		{
			RT_AD_BANNER,
			RT_INTERSTITIAL
		}
	}
}