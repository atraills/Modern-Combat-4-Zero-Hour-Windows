using Microsoft.Phone.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace IGPWindows8
{
	public class IGPFreemium : IGPBase
	{
		protected WebBrowser m_WebBrowser;

		protected string FREEMIUM_IGP_LINK_TEMPLATE = "http://ingameads.gameloft.com/redir/freemium/hdfreemium.php?from=GAME_CODE&country=COUNTRY&lg=LANGUAGE&udid=UDID&ver=IGPREVISION&d=DEVICE_NAME&f=FIRMWARE&game_ver=GAMEVERSION&igp_rev=IGPREVISION";

		protected string LINK_BACK = "http://signal-back.com";

		protected string GAMEINFORMATIONS_PAGE = "http://ingameads.gameloft.com/redir/android/index.php?page=gameinformation&igp_rev=IGPREVISION";

		protected string GETIT_PAGE = "http://ingameads.gameloft.com/redir/?from=";

		private Image m_Background;

		private Button m_ExitButton;

		private bool m_bIgnoreConnectivity;

		private bool m_bWebBrowserVisible;

		private bool m_bBackgroundVisible;

		private string[] m_szLandscapeBgs = new string[] { "Resources/window_en.png", "Resources/window_fr.png", "Resources/window_de.png", "Resources/window_it.png", "Resources/window_sp.png", "Resources/window_jp.png", "Resources/window_kr.png", "Resources/window_cn.png", "Resources/window_br.png", "Resources/window_ru.png" };

		private string[] m_szPortraitBgs = new string[] { "Resources/window_portrait_en.png", "Resources/window_portrait_fr.png", "Resources/window_portrait_de.png", "Resources/window_portrait_it.png", "Resources/window_portrait_sp.png", "Resources/window_portrait_jp.png", "Resources/window_portrait_kr.png", "Resources/window_portrait_cn.png", "Resources/window_portrait_br.png", "Resources/window_portrait_ru.png" };

		public override double ScreenHeight
		{
			get
			{
				return base.ScreenHeight;
			}
			set
			{
				base.ScreenHeight = value;
				if (this.m_WebBrowser != null)
				{
					double screenHeight = this.ScreenHeight - 90 - 35;
					if (screenHeight < 0)
					{
						screenHeight = 0;
					}
					this.m_WebBrowser.set_Height(screenHeight);
				}
				if (this.m_Background != null)
				{
					this.m_Background.set_Height(value);
				}
			}
		}

		public override double ScreenWidth
		{
			get
			{
				return base.ScreenWidth;
			}
			set
			{
				base.ScreenWidth = value;
				if (this.m_WebBrowser != null)
				{
					double screenWidth = this.ScreenWidth - 100 - (value - 800);
					if (screenWidth < 0)
					{
						screenWidth = 0;
					}
					this.m_WebBrowser.set_Width(screenWidth);
				}
				if (this.m_Background != null)
				{
					this.m_Background.set_Width(value);
				}
			}
		}

		public IGPFreemium(Panel parent) : base(parent)
		{
			this.InitControls();
			this.m_bIGPLoaded = false;
			this.ScreenWidth = this.ScreenWidth;
			this.ScreenHeight = this.ScreenHeight;
			parent.get_Children().Add(this.m_Background);
			parent.get_Children().Add(this.m_WebBrowser);
			parent.get_Children().Add(this.m_ExitButton);
		}

		public override int GetBannerState(string type)
		{
			if (type.ToLower() == "igp")
			{
				if (this.m_WebBrowser != null && this.m_bWebBrowserVisible)
				{
					return 4;
				}
				if (this.m_WebBrowser != null && !this.m_bWebBrowserVisible && this.m_bIGPLoaded)
				{
					return 3;
				}
				if (this.m_bIGPLoading)
				{
					return 2;
				}
				if (!this.m_bIGPLoaded)
				{
					return 1;
				}
			}
			else if (type.ToLower() == "background")
			{
				if (this.m_bBackgroundVisible)
				{
					return 1;
				}
				return 0;
			}
			return base.GetBannerState(type);
		}

		protected override string GetTemplate(IGPBase.eTemplateType type)
		{
			if (type == IGPBase.eTemplateType.TT_FREEMIUM_IGP)
			{
				return this.FREEMIUM_IGP_LINK_TEMPLATE;
			}
			return base.GetTemplate(type);
		}

		protected override bool HandleNavigatingString(object sender, string url)
		{
			if (!url.StartsWith("play:"))
			{
				if (url.StartsWith("market://"))
				{
					return true;
				}
				if (url.StartsWith("zune:"))
				{
					this.m_bIgnoreConnectivity = true;
					this.ShowIGP(false);
					return false;
				}
				if (url.StartsWith(this.LINK_BACK))
				{
					this.ShowIGP(false);
					return true;
				}
				return base.HandleNavigatingString(sender, url);
			}
			string str = url.Substring("play:".Length);
			str = str.Replace("%7B", "{");
			str = str.Replace("%7D", "}");
			if (!base.LaunchInstalledApp(str))
			{
				base.OpenMarketApp(str);
			}
			this.ShowIGP(false);
			return true;
		}

		private void InitControls()
		{
			float screenWidth = (float)this.ScreenWidth / 800f;
			float screenHeight = (float)this.ScreenHeight / 480f;
			float single = (float)this.ScreenWidth - 800f;
			double num = this.ScreenHeight;
			this.m_WebBrowser = new WebBrowser();
			this.SetupWebBrowser(this.m_WebBrowser);
			this.m_WebBrowser.add_LoadCompleted(new LoadCompletedEventHandler(this, IGPFreemium.OnBrowserLoadFinished));
			this.m_WebBrowser.set_HorizontalAlignment(1);
			this.m_WebBrowser.set_VerticalAlignment(1);
			this.m_WebBrowser.add_NavigationFailed(new NavigationFailedEventHandler(this, IGPFreemium.OnBrowserNavigationFailed));
			this.m_WebBrowser.add_ScriptNotify(new EventHandler<NotifyEventArgs>(this.OnBrowserScriptNotify));
			this.m_WebBrowser.add_Navigating(new EventHandler<NavigatingEventArgs>(this.OnBrowserNavigating));
			this.m_WebBrowser.add_Navigated(new EventHandler<NavigationEventArgs>(this.OnBrowserNavigated));
			this.m_WebBrowser.set_Width((this.ScreenWidth - 56) * (double)screenWidth);
			this.m_WebBrowser.set_Height((this.ScreenHeight - 90 - 35) * (double)screenHeight);
			this.m_WebBrowser.set_Margin(new Thickness((double)(screenWidth * 48f + single * 0.5f), (double)(screenHeight * 90f), (double)(screenWidth * 28f), (double)(screenHeight * 35f)));
			this.m_WebBrowser.set_HorizontalAlignment(0);
			this.m_WebBrowser.set_VerticalAlignment(0);
			this.m_WebBrowser.set_HorizontalContentAlignment(0);
			this.m_WebBrowser.set_VerticalContentAlignment(0);
			this.m_Background = new Image();
			this.m_Background.set_Visibility(1);
			this.m_bBackgroundVisible = false;
			this.m_ExitButton = new Button();
			ImageBrush imageBrush = new ImageBrush();
			imageBrush.set_ImageSource(new BitmapImage(new Uri(this.m_szIGPExitButton, UriKind.Relative)));
			this.m_ExitButton.set_Background(imageBrush);
			this.m_ExitButton.set_Width(100);
			this.m_ExitButton.set_Height(90);
			this.m_ExitButton.set_Margin(new Thickness(-10, -7, (double)(30f + single * 0.5f), 0));
			this.m_ExitButton.set_Visibility(1);
			this.m_ExitButton.set_VerticalAlignment(0);
			this.m_ExitButton.set_HorizontalAlignment(2);
			this.m_ExitButton.set_BorderBrush(null);
			this.m_ExitButton.add_Click(new RoutedEventHandler(this, IGPFreemium.m_ExitButton_Click));
		}

		protected override void LoadIGP()
		{
			base.LoadIGP();
			this.m_bIGPLoaded = false;
			this.m_bIGPLoading = true;
			string str = this.SolveTemplate(this.GetTemplate(IGPBase.eTemplateType.TT_FREEMIUM_IGP));
			int width = (int)this.m_WebBrowser.get_Width() - 90;
			int height = (int)this.m_WebBrowser.get_Height();
			object[] objArray = new object[] { str, "&width=", width, "&height=", height };
			string str1 = base.EncryptURL(string.Concat(objArray));
			this.m_WebBrowser.Navigate(new Uri(str1, UriKind.Absolute));
		}

		private void m_ExitButton_Click(object sender, RoutedEventArgs e)
		{
			this.ShowIGP(false);
		}

		protected void OnBrowserLoadFinished(object sender, NavigationEventArgs e)
		{
			this.m_bIGPLoaded = true;
			if (this.m_bIGPVisible)
			{
				this.m_WebBrowser.set_Visibility(0);
				this.m_bWebBrowserVisible = true;
				this.m_WebBrowser.set_IsEnabled(true);
			}
			this.m_bIGPLoaded = true;
			base.ShowProgressDialogue("not implemented yet", false);
		}

		protected void OnBrowserNavigated(object sender, NavigationEventArgs e)
		{
		}

		protected void OnBrowserNavigating(object sender, NavigatingEventArgs e)
		{
			string absoluteUri = e.get_Uri().AbsoluteUri;
			e.Cancel = this.HandleNavigatingString(sender, absoluteUri);
		}

		protected void OnBrowserNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			base.ShowProgressDialogue("not implemented yet", false);
			if (this.m_bIgnoreConnectivity)
			{
				this.m_bIgnoreConnectivity = false;
				return;
			}
			if (this.m_bIGPVisible)
			{
				base.ShowConnectionFailedDialogue("not implemeted yet", true);
			}
		}

		protected void OnBrowserScriptNotify(object sender, NotifyEventArgs e)
		{
		}

		public override void OnDestroy()
		{
			this.m_Parent.get_Children().Remove(this.m_Background);
			this.m_Parent.get_Children().Remove(this.m_ExitButton);
			this.m_Parent.get_Children().Remove(this.m_WebBrowser);
			this.m_Background = null;
			this.m_ExitButton = null;
			this.m_WebBrowser = null;
			base.OnDestroy();
		}

		public override void ShowIGP(bool bShow)
		{
			base.ShowIGP(bShow);
			if (!bShow)
			{
				this.m_bIGPVisible = false;
				Visibility visibility = this.m_Background.get_Visibility();
				this.m_Background.set_Visibility(1);
				this.m_ExitButton.set_Visibility(1);
				this.m_WebBrowser.set_Visibility(1);
				this.m_bBackgroundVisible = false;
				this.m_bWebBrowserVisible = false;
				if (this.m_Background.get_Visibility() != visibility)
				{
					base.SendBannerStateChanged("igpscreen", 0);
				}
				base.ShowProgressDialogue("Not Implemente Yet", false);
				base.ShowConnectionFailedDialogue("Not Implemented Yet", false);
				return;
			}
			this.LoadIGP();
			try
			{
				this.m_Background.set_Source(new BitmapImage(new Uri(this.m_szLandscapeBgs[this.m_nLangauge], UriKind.Relative)));
			}
			catch (Exception exception)
			{
				exception.ToString();
			}
			if (!this.m_bIGPLoaded)
			{
				base.ShowProgressDialogue("not implemented yet", true);
			}
			else
			{
				this.m_WebBrowser.set_Visibility(0);
				this.m_bWebBrowserVisible = true;
			}
			Visibility visibility1 = this.m_Background.get_Visibility();
			this.m_Background.set_Visibility(0);
			this.m_bBackgroundVisible = true;
			this.m_ExitButton.set_Visibility(0);
			if (this.m_Background.get_Visibility() != visibility1)
			{
				base.SendBannerStateChanged("igpscreen", 1);
			}
			this.m_bIGPVisible = true;
		}

		public override void Start()
		{
		}
	}
}