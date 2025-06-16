using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Windows.ApplicationModel;
using Windows.Phone.Management.Deployment;
using Windows.Phone.System.Analytics;
using Windows.Storage;
using Windows.System;

namespace IGPWindows8
{
	public class IGPBase
	{
		protected const int BANNER_STATE_NOT_IMPLEMENTED = 0;

		protected const int BANNER_STATE_NOT_LOADED = 1;

		protected const int BANNER_STATE_LOADING = 2;

		protected const int BANNER_STATE_INVISIBLE = 3;

		protected const int BANNER_STATE_VISIBLE = 4;

		protected const string BANNER_TYPE_AD = "adbanner";

		protected const string BANNER_TYPE_INTERSTITIAL = "interstitial";

		protected const string BANNER_TYPE_WELCOMESCREEN = "welcomescreen";

		protected const string BANNER_TYPE_IGP = "igpscreen";

		protected const int BANNER_STATE_CHANGED_HIDDEN = 0;

		protected const int BANNER_STATE_CHANGED_SHOWN = 1;

		protected const string IGP_OFFLINE_TRACK_FILE = "igpoffline.txt";

		protected const string m_szLoadingBGImage = "Resources/WindowsLoading.png";

		protected const string m_szConnectionFailedImage = "Resources/WindowsConnectionFailed.png";

		private const string TRACK_LOADING = "http://ingameads.gameloft.com/redir/winloading.php?game=GAME_CODE&country=COUNTRY&lg=LANGUAGE&ver=IGPREVISION&device=DEVICE_NAME&f=FIRMWARE&udid=UDID&g_ver=GAMEVERSION&date_r=FIRST_DATE&ddate=CURRENT_DATE";

		private const int SMALL_BANNER_WIDTH = 320;

		private const int SMALL_BANNER_HEIGHT = 50;

		private const int BIG_BANNER_WIDTH = 448;

		private const int BIG_BANNER_HEIGHT = 70;

		protected const int WELCOME_SCREEN_WIDTH = 650;

		protected const int WELCOME_SCREEN_HEIGHT = 400;

		protected const int RefScreenWidth = 800;

		protected const int RefScreenHeight = 480;

		protected IGPBase.IGPMessageDelegate IGPMessage;

		protected IGPBase.ErrorMessageDelegate ErrorOccurred;

		protected IGPBase.BannerStateChangedDelegate BannerStateChanged;

		protected static bool s_bLoadingTracked;

		public static bool SkipResumeTracking;

		private static string StartDate;

		public static string IGPVersion;

		protected WebClient m_WebPoster;

		protected Panel m_Parent;

		protected Image m_LoadingProgressImage;

		protected Image m_ConnectionFailedImage;

		private string hardcodedAddURL = "http://ingameads.gameloft.com/redir/ads/ads_server_view.php?from=DARK&lg=EN&udid=1bfc7f10d0fce73f54775c40908c4a30c4093c93&d=iPhone3,1&f=4.2.1";

		private string hardcodedInterURL = "http://www.bubblebox.com/images/bigpoint/seafight650x400.gif";

		private string hardcodedLoadingURL = "http://ingameads.gameloft.com/redir/ads/splashscreen_view.php?data=wGzckzv3EUCoiJEYuRkHTjcmmBbV2xVojpGcXwhZvsA=&enc=4&vo=1&type=windows";

		private string hardcodedWelcomeURL = "http://ingameads.gameloft.com/redir/ads/splashscreen_view.php?from=TESA&country=US&lg=EN&udid=359871041855009&d=Samsung_GT-P6200&f=4.1.1&game_ver=1.0.1&type=android&width=819&height=460";

		protected string m_szOperator = "";

		protected string m_szCountry = "RO";

		protected List<Thread> IGPThreads = new List<Thread>();

		protected bool m_bBannerLoaded;

		protected bool m_bBannerLoading;

		protected bool m_bBannerIsVisible;

		protected bool m_bInterstitialLoaded;

		protected bool m_bInterstitialLoading;

		protected bool m_bInterstitialVisible;

		protected bool m_bWelcomeScreenLoaded;

		protected bool m_bWelcomeScreenLoading;

		protected bool m_bWelcomeScreenVisible;

		protected bool m_bIGPVisible;

		protected bool m_bIGPLoading;

		protected bool m_bIGPLoaded;

		protected bool m_bBannerBig;

		protected double m_Width;

		protected double m_Height;

		protected double m_ScreenW;

		protected double m_ScreenH;

		protected bool m_bConnectivityFailedShown;

		protected bool m_bLoadingProgressShown;

		protected string m_szGameCode = "";

		protected string m_szUDID = "";

		protected string m_szDeviceName = "";

		protected string m_szFirmware = "";

		protected string m_szGameVersion = "";

		protected string[] m_szLangauges = new string[] { "EN", "FR", "DE", "IT", "SP", "JP", "KR", "CN", "BR", "RU" };

		protected int m_nLangauge;

		protected string m_szIGPExitButton = "Resources/igp_exit.png";

		private bool bPostResponseReceived = true;

		private string szPostResponse = "";

		public virtual double Height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		public virtual double ScreenHeight
		{
			get
			{
				return this.m_ScreenH;
			}
			set
			{
				this.m_ScreenH = value;
				double mScreenH = this.m_ScreenH / 480;
				if (this.m_bBannerBig)
				{
					this.Height = mScreenH * 70;
					return;
				}
				this.Height = mScreenH * 50;
			}
		}

		public virtual double ScreenWidth
		{
			get
			{
				return this.m_ScreenW;
			}
			set
			{
				this.m_ScreenW = value;
				double mScreenW = this.m_ScreenW / 800;
				if (this.m_bBannerBig)
				{
					this.Width = mScreenW * 448;
					return;
				}
				this.Width = mScreenW * 320;
			}
		}

		public virtual bool UseBigBanner
		{
			get
			{
				return this.m_bBannerBig;
			}
			set
			{
				if (!this.m_bBannerBig && value)
				{
					this.Width = this.m_ScreenW / 800 * 448;
					this.Height = this.m_ScreenH / 480 * 70;
					this.m_bBannerBig = true;
				}
				else if (this.m_bBannerBig && !value)
				{
					this.Width = this.m_ScreenW / 800 * 320;
					this.Height = this.m_ScreenH / 480 * 50;
					this.m_bBannerBig = false;
				}
				this.m_bBannerBig = value;
			}
		}

		public virtual double Width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = value;
			}
		}

		static IGPBase()
		{
			IGPBase.s_bLoadingTracked = false;
			IGPBase.SkipResumeTracking = false;
			IGPBase.StartDate = null;
			IGPBase.IGPVersion = "100";
		}

		public IGPBase(Panel parent)
		{
			this.m_Parent = parent;
			this.m_WebPoster = new WebClient();
			IGPBase gPBase = this;
			this.m_WebPoster.DownloadStringCompleted += new DownloadStringCompletedEventHandler(gPBase.OnWebClientDownloadStringCompleted);
			this.m_szUDID = this.GetUDID();
			this.m_szDeviceName = string.Concat(DeviceStatus.get_DeviceManufacturer(), "_", DeviceStatus.get_DeviceName());
			this.m_szFirmware = DeviceStatus.get_DeviceFirmwareVersion();
			this.m_szCountry = RegionInfo.CurrentRegion.Name;
			double actualWidth = Application.get_Current().get_Host().get_Content().get_ActualWidth();
			double actualHeight = Application.get_Current().get_Host().get_Content().get_ActualHeight();
			if (actualWidth < actualHeight)
			{
				actualWidth = actualHeight;
				actualHeight = Application.get_Current().get_Host().get_Content().get_ActualWidth();
			}
			this.ScreenWidth = actualWidth;
			this.ScreenHeight = actualHeight;
			this.SetLanguage("EN");
			this.m_LoadingProgressImage = new Image();
			this.m_LoadingProgressImage.set_Width(300);
			this.m_LoadingProgressImage.set_Height(200);
			this.m_LoadingProgressImage.set_Source(new BitmapImage(new Uri("Resources/WindowsLoading.png", UriKind.Relative)));
			this.m_LoadingProgressImage.set_HorizontalAlignment(1);
			this.m_LoadingProgressImage.set_VerticalAlignment(1);
			this.m_LoadingProgressImage.set_Visibility(1);
			this.m_ConnectionFailedImage = new Image();
			this.m_ConnectionFailedImage.set_Width(400);
			this.m_ConnectionFailedImage.set_Height(200);
			this.m_ConnectionFailedImage.set_Source(new BitmapImage(new Uri("Resources/WindowsConnectionFailed.png", UriKind.Relative)));
			this.m_ConnectionFailedImage.set_HorizontalAlignment(1);
			this.m_ConnectionFailedImage.set_VerticalAlignment(1);
			this.m_ConnectionFailedImage.set_Visibility(1);
		}

		public string EncryptURL(string input)
		{
			Encryptor.Encryption = Encryptor.eEncryptionType.EET_AES;
			string str = "";
			string str1 = this.ExtractURLData(input);
			str = input.Substring(0, input.Length - str1.Length);
			if (str1 != null && str1.Length > 0)
			{
				str = string.Concat(str, "data=");
				str1 = string.Concat(str1, "&os=winp");
				str1 = Encryptor.Encrypt(str1);
				str = string.Concat(str, str1, "&enc=4&os=winp");
			}
			return str;
		}

		public string ExtractURLData(string input)
		{
			int num = 0;
			num = 0;
			while (num < input.Length && input[num] != '?')
			{
				num++;
			}
			if (num + 1 >= input.Length)
			{
				return "";
			}
			return input.Substring(num + 1, input.Length - num - 1);
		}

		public virtual int GetBannerState(string type)
		{
			return 0;
		}

		private string GetOfflineLogs()
		{
			string str;
			string str1 = "";
			string path = ApplicationData.Current.LocalFolder.Path;
			string str2 = string.Concat(path, "/igpoffline.txt");
			try
			{
				StreamReader streamReader = new StreamReader(str2);
				string str3 = streamReader.ReadLine();
				while (!streamReader.EndOfStream)
				{
					str1 = (str1 != "" ? string.Concat(str1, "#", streamReader.ReadLine()) : streamReader.ReadLine());
				}
				streamReader.Close();
				streamReader.Dispose();
				StreamWriter streamWriter = new StreamWriter(str2);
				streamWriter.Close();
				streamWriter.Dispose();
				this.LogOffline(str3);
				str = str1;
			}
			catch (Exception exception)
			{
				this.SendErrorMessage("IGPBase::TrackIGPLoading", "Error reading offline track logs", " ");
				return null;
			}
			return str;
		}

		public virtual string GetPostResult()
		{
			return this.szPostResponse;
		}

		private string GetStartDate()
		{
			string str;
			string path = ApplicationData.Current.LocalFolder.Path;
			string str1 = string.Concat(path, "/igpoffline.txt");
			try
			{
				StreamReader streamReader = new StreamReader(str1);
				string str2 = streamReader.ReadLine();
				streamReader.Close();
				streamReader.Dispose();
				str = str2;
			}
			catch (Exception exception)
			{
				return null;
			}
			return str;
		}

		protected virtual string GetTemplate(IGPBase.eTemplateType type)
		{
			if (type == IGPBase.eTemplateType.TT_GAMELOFT_INTERSTITIAL)
			{
				return this.hardcodedInterURL;
			}
			if (type == IGPBase.eTemplateType.TT_WELCOME_SCREEN)
			{
				return this.hardcodedWelcomeURL;
			}
			if (type == IGPBase.eTemplateType.TT_LOADING)
			{
				return "http://ingameads.gameloft.com/redir/winloading.php?game=GAME_CODE&country=COUNTRY&lg=LANGUAGE&ver=IGPREVISION&device=DEVICE_NAME&f=FIRMWARE&udid=UDID&g_ver=GAMEVERSION&date_r=FIRST_DATE&ddate=CURRENT_DATE";
			}
			return this.hardcodedAddURL;
		}

		public string GetUDID()
		{
			return Encryptor.Encrypt_MD5(HostInformation.get_PublisherHostId());
		}

		protected virtual bool HandleNavigatingString(object sender, string url)
		{
			return false;
		}

		public virtual void HttpPost(string URL)
		{
			while (!this.bPostResponseReceived)
			{
				Thread.Sleep(100);
			}
			this.szPostResponse = null;
			this.bPostResponseReceived = false;
			this.m_WebPoster.DownloadStringAsync(new Uri(URL, UriKind.Absolute));
		}

		public bool LaunchInstalledApp(string appId)
		{
			IEnumerable<Package> packages = InstallationManager.FindPackagesForCurrentPublisher();
			for (int i = 0; i < packages.Count<Package>(); i++)
			{
				Package package = packages.ElementAt<Package>(i);
				if (package.Id.get_ProductId().ToLower() == appId)
				{
					package.Launch(string.Empty);
					return true;
				}
			}
			return false;
		}

		protected virtual void LoadBanner()
		{
		}

		protected virtual void LoadIGP()
		{
		}

		protected virtual void LoadInterstitial()
		{
		}

		protected virtual void LoadWelcomeScreen()
		{
		}

		private void LogOffline(string date)
		{
			string path = ApplicationData.Current.LocalFolder.Path;
			string str = string.Concat(path, "/igpoffline.txt");
			try
			{
				StreamWriter streamWriter = new StreamWriter(str, true);
				streamWriter.WriteLine(date);
				streamWriter.Close();
				streamWriter.Dispose();
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				this.SendErrorMessage("IGPBase::TrackApplicationLoading", string.Concat(" error opening for write with ", exception.Message), " ");
			}
		}

		public virtual void OnDestroy()
		{
			for (int i = 0; i < this.IGPThreads.Count; i++)
			{
				this.IGPThreads[i].Abort();
			}
			this.IGPThreads.Clear();
			this.m_LoadingProgressImage = null;
		}

		public virtual void OnPause()
		{
		}

		public virtual void OnResume()
		{
			if (IGPBase.SkipResumeTracking)
			{
				IGPBase.SkipResumeTracking = false;
				return;
			}
			this.TrackApplicationLoading(true);
		}

		protected virtual void OnWebClientDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			try
			{
				this.szPostResponse = e.Result;
			}
			catch (WebException webException)
			{
				this.szPostResponse = string.Concat("Failed with Exception ", webException.ToString());
			}
			catch (TargetInvocationException targetInvocationException)
			{
				this.szPostResponse = string.Concat("Failed with Exception ", targetInvocationException.ToString());
			}
			if (e.Error != null)
			{
				this.szPostResponse = string.Concat("Failed ", e.ToString());
			}
			this.bPostResponseReceived = true;
		}

		public void OpenBrowser(string link)
		{
			Launcher.LaunchUriAsync(new Uri(link));
		}

		public void OpenMarketApp(string appId)
		{
			Launcher.LaunchUriAsync(new Uri(string.Concat("zune:navigate?appid=", appId)));
		}

		protected void SendBannerStateChanged(string bannerType, int newState)
		{
			if (this.BannerStateChanged != null)
			{
				this.BannerStateChanged(newState, bannerType);
			}
		}

		protected void SendErrorMessage(string from, string reason, string sugestion)
		{
			if (this.ErrorOccurred != null)
			{
				this.ErrorOccurred(from, reason, sugestion);
			}
		}

		protected void SendIGPMessage(string messageType, string parameter, string from)
		{
			if (this.IGPMessage != null)
			{
				this.IGPMessage(messageType, parameter, from);
			}
		}

		public virtual void SetAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
		{
		}

		public void SetBannerStateChangedDelegate(IGPBase.BannerStateChangedDelegate handler)
		{
			this.BannerStateChanged = handler;
		}

		public void SetErrorMessageDelegate(IGPBase.ErrorMessageDelegate handler)
		{
			this.ErrorOccurred = handler;
		}

		public void SetIGPMessageDelgate(IGPBase.IGPMessageDelegate handler)
		{
			this.IGPMessage = handler;
		}

		public void SetLanguage(string language)
		{
			for (int i = 0; i < (int)this.m_szLangauges.Length; i++)
			{
				if (language.ToUpper() == this.m_szLangauges[i])
				{
					this.m_nLangauge = i;
					return;
				}
			}
		}

		public void SetProjectSpecifics(string gameCode, string gameVersion, string Operator)
		{
			this.m_szGameVersion = gameVersion;
			this.m_szOperator = Operator;
			this.m_szGameCode = gameCode;
			this.TrackApplicationLoading(false);
		}

		protected virtual void SetupWebBrowser(WebBrowser aBrowser)
		{
			aBrowser.set_Visibility(1);
			aBrowser.set_IsScriptEnabled(true);
			aBrowser.set_IsEnabled(false);
			aBrowser.set_AllowDrop(false);
		}

		public virtual void ShowBanner(bool bShow)
		{
			this.m_bBannerIsVisible = bShow;
		}

		protected void ShowConnectionFailedDialogue(string message, bool bShow)
		{
			if (!bShow)
			{
				if (this.m_bConnectivityFailedShown)
				{
					this.m_Parent.get_Children().Remove(this.m_ConnectionFailedImage);
				}
				this.m_bConnectivityFailedShown = true;
				this.m_ConnectionFailedImage.set_Visibility(1);
				return;
			}
			if (this.m_bConnectivityFailedShown)
			{
				this.m_Parent.get_Children().Remove(this.m_ConnectionFailedImage);
			}
			this.m_Parent.get_Children().Add(this.m_ConnectionFailedImage);
			this.m_ConnectionFailedImage.set_Visibility(0);
			this.m_bConnectivityFailedShown = true;
		}

		public virtual void ShowIGP(bool bShow)
		{
			this.m_bIGPVisible = bShow;
		}

		public virtual void ShowInterstitial(bool bShow)
		{
			this.m_bInterstitialVisible = bShow;
		}

		protected void ShowProgressDialogue(string message, bool bShow)
		{
			if (!bShow)
			{
				if (this.m_bLoadingProgressShown)
				{
					this.m_Parent.get_Children().Remove(this.m_LoadingProgressImage);
				}
				this.m_bLoadingProgressShown = false;
				this.m_LoadingProgressImage.set_Visibility(1);
				return;
			}
			if (this.m_bLoadingProgressShown)
			{
				this.m_Parent.get_Children().Remove(this.m_LoadingProgressImage);
			}
			this.m_Parent.get_Children().Add(this.m_LoadingProgressImage);
			this.m_LoadingProgressImage.set_Visibility(0);
			this.m_bLoadingProgressShown = true;
		}

		public virtual void ShowWelcomeScreen(bool bShow)
		{
			this.m_bWelcomeScreenVisible = bShow;
		}

		protected virtual string SolveTemplate(string aTemplate)
		{
			string str = aTemplate.Replace("GAME_CODE", this.m_szGameCode);
			str = str.Replace("LANGUAGE", this.m_szLangauges[this.m_nLangauge]);
			str = str.Replace("UDID", this.m_szUDID);
			str = str.Replace("FIRMWARE", this.m_szFirmware);
			str = str.Replace("GAMEVERSION", this.m_szGameVersion);
			str = str.Replace("DEVICE_NAME", this.m_szDeviceName);
			str = str.Replace("COUNTRY", this.m_szCountry);
			str = str.Replace("IGPREVISION", IGPBase.IGPVersion);
			return str.Replace(" ", "");
		}

		public virtual void Start()
		{
			this.LoadBanner();
			this.LoadInterstitial();
			this.LoadWelcomeScreen();
		}

		public virtual void TrackApplicationLoading(bool bOnResume)
		{
			if (!bOnResume && IGPBase.s_bLoadingTracked)
			{
				return;
			}
			string str = DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss");
			if (bOnResume)
			{
				str = string.Concat(str, "R");
			}
			if (IGPBase.StartDate == null)
			{
				IGPBase.StartDate = this.GetStartDate();
				if (IGPBase.StartDate == null)
				{
					if (bOnResume)
					{
						this.SendErrorMessage("IGPBase::TrackApplicationLoading", "no start date logged at the time onResume is logged", " ");
					}
					else
					{
						this.LogOffline(str);
					}
					IGPBase.StartDate = str;
				}
			}
			if (!NetworkInterface.GetIsNetworkAvailable())
			{
				this.LogOffline(str);
			}
			else
			{
				string offlineLogs = this.GetOfflineLogs();
				Encryptor.Encryption = Encryptor.eEncryptionType.EET_AES;
				string str1 = this.SolveTemplate(this.GetTemplate(IGPBase.eTemplateType.TT_LOADING));
				str1 = str1.Replace("FIRST_DATE", IGPBase.StartDate);
				str1 = str1.Replace("CURRENT_DATE", str);
				if (offlineLogs != null)
				{
					str1 = string.Concat(str1, "&offline=", offlineLogs);
				}
				this.HttpPost(this.EncryptURL(str1));
			}
			if (!bOnResume)
			{
				IGPBase.s_bLoadingTracked = true;
			}
		}

		public delegate void BannerStateChangedDelegate(int arg0, string arg1);

		public delegate void ErrorMessageDelegate(string arg0, string arg1, string arg2);

		protected enum eTemplateType
		{
			TT_REQUEST_AD,
			TT_REQUEST_INTERSTITIAL,
			TT_REQUEST_FREE_CASH,
			TT_LINK_VIDEO,
			TT_GAMELOFT_AD,
			TT_GAMELOFT_INTERSTITIAL,
			TT_WELCOME_SCREEN,
			TT_LOADING,
			TT_FREEMIUM_IGP,
			TT_TOTAL
		}

		public delegate void IGPMessageDelegate(string arg0, string arg1, string arg2);
	}
}