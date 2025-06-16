using MC4Component;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MC4Interop
{
	public class VideoPlayer : UserControl
	{
		private List<float> m_Times;

		private List<string> m_SubTitles;

		private int m_iNumSub;

		public static string s_sMovieTitle;

		public static VideoPlayer.LANG s_Lang;

		public static bool s_IsInVideoPlayer;

		private MainPage m_mainPage;

		internal Grid LayoutRoot;

		internal Grid ContentPanel;

		internal Grid videoContent;

		internal MediaElement mediaElement;

		internal Grid subtitleContent;

		internal TextBlock subtitleText;

		internal Image skip;

		private bool _contentLoaded;

		static VideoPlayer()
		{
			VideoPlayer.s_IsInVideoPlayer = false;
		}

		public VideoPlayer(MainPage mainPage)
		{
			this.InitializeComponent();
			this.m_iNumSub = 0;
			this.m_SubTitles = new List<string>();
			this.m_Times = new List<float>();
			CompositionTarget.add_Rendering(new EventHandler(this.CompositionTarget_Rendering));
			this.m_mainPage = mainPage;
			this.LoadSubTitles();
			this.mediaElement.set_AutoPlay(true);
			this.mediaElement.set_Source(new Uri(VideoPlayer.s_sMovieTitle, UriKind.Relative));
			this.mediaElement.Play();
		}

		private void CompositionTarget_Rendering(object sender, EventArgs e)
		{
			int seconds = this.mediaElement.get_Position().Seconds;
			for (int i = 0; i < this.m_iNumSub; i++)
			{
				if ((float)seconds >= this.m_Times.ElementAt<float>(i) && (float)seconds < this.m_Times.ElementAt<float>(i + 1))
				{
					this.subtitleText.set_Text(this.m_SubTitles[i]);
				}
			}
		}

		public void GoToEnd()
		{
			if (this.mediaElement.get_NaturalDuration() != Duration.get_Automatic())
			{
				this.mediaElement.set_Position(this.mediaElement.get_NaturalDuration().get_TimeSpan());
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Application.LoadComponent(this, new Uri("/MC4Interop;component/VideoPlayer.xaml", UriKind.Relative));
			this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
			this.ContentPanel = (Grid)base.FindName("ContentPanel");
			this.videoContent = (Grid)base.FindName("videoContent");
			this.mediaElement = (MediaElement)base.FindName("mediaElement");
			this.subtitleContent = (Grid)base.FindName("subtitleContent");
			this.subtitleText = (TextBlock)base.FindName("subtitleText");
			this.skip = (Image)base.FindName("skip");
		}

		private void LoadSubTitles()
		{
			this.m_iNumSub = 0;
			this.m_SubTitles.Clear();
			this.m_Times.Clear();
			int num = 1;
			string str = "";
			if (VideoPlayer.s_sMovieTitle.Equals("data/briefing/briefing1.mp4"))
			{
				num = 1;
			}
			else if (VideoPlayer.s_sMovieTitle.Equals("data/briefing/briefing2.mp4"))
			{
				num = 2;
			}
			else if (VideoPlayer.s_sMovieTitle.Equals("data/briefing/briefing3.mp4"))
			{
				num = 3;
			}
			else if (VideoPlayer.s_sMovieTitle.Equals("data/briefing/briefing4.mp4"))
			{
				num = 4;
			}
			else if (VideoPlayer.s_sMovieTitle.Equals("data/briefing/briefing5.mp4"))
			{
				num = 5;
			}
			switch (VideoPlayer.s_Lang)
			{
				case VideoPlayer.LANG.EN:
				{
					str = string.Concat("data/briefing/briefing", Convert.ToString(num), "_English.srt");
					break;
				}
				case VideoPlayer.LANG.FR:
				{
					str = string.Concat("data/briefing/briefing", Convert.ToString(num), "_french.srt");
					break;
				}
				case VideoPlayer.LANG.DE:
				{
					str = string.Concat("data/briefing/briefing", Convert.ToString(num), "_german.srt");
					break;
				}
				case VideoPlayer.LANG.IT:
				{
					str = string.Concat("data/briefing/briefing", Convert.ToString(num), "_italian.srt");
					break;
				}
				case VideoPlayer.LANG.ES:
				{
					str = string.Concat("data/briefing/briefing", Convert.ToString(num), "_spanish.srt");
					break;
				}
				case VideoPlayer.LANG.JA:
				{
					str = string.Concat("data/briefing/briefing", Convert.ToString(num), "_japanese.srt");
					break;
				}
				case VideoPlayer.LANG.KO:
				{
					str = string.Concat("data/briefing/briefing", Convert.ToString(num), "_korean.srt");
					break;
				}
				case VideoPlayer.LANG.ZH:
				{
					str = string.Concat("data/briefing/briefing", Convert.ToString(num), "_chinese.srt");
					break;
				}
				case VideoPlayer.LANG.PT:
				{
					str = string.Concat("data/briefing/briefing", Convert.ToString(num), "_brazilian.srt");
					break;
				}
				case VideoPlayer.LANG.RU:
				{
					str = string.Concat("data/briefing/briefing", Convert.ToString(num), "_russian.srt");
					break;
				}
			}
			try
			{
				StreamReader streamReader = new StreamReader(str);
				byte num1 = 0;
				while (true)
				{
					string str1 = streamReader.ReadLine();
					string str2 = str1;
					if (str1 == null)
					{
						break;
					}
					num1 = (byte)(num1 + 1);
					if ((num1 & 3) == 2)
					{
						this.m_iNumSub++;
						Match match = Regex.Match(str2, "00:00:(?<from>[0-9]*),(?<fromF>[0-9]*) --> 00:00:(?<to>[0-9]*),(?<toF>[0-9]*)");
						if (this.m_iNumSub == 1)
						{
							this.m_Times.Add((float)Convert.ToInt32(match.Groups["from"].Value) + (float)Convert.ToInt32(match.Groups["fromF"].Value) / 1000f);
						}
						this.m_Times.Add((float)Convert.ToInt32(match.Groups["to"].Value) + (float)Convert.ToInt32(match.Groups["toF"].Value) / 1000f);
					}
					else if ((num1 & 3) == 3)
					{
						this.m_SubTitles.Add(str2);
					}
				}
			}
			catch (Exception exception)
			{
			}
		}

		public void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
		{
			this.subtitleText.set_Text("");
			if (this.m_mainPage.DrawingSurfaceBackground.get_Children().Contains(this))
			{
				this.m_mainPage.DrawingSurfaceBackground.get_Children().Remove(this);
			}
			Direct3DBackground.GetInstance().SetShowMovieFinish();
		}

		public void mediaElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.skip.set_Visibility(0);
		}

		public void ReplayMovie()
		{
			base.get_Dispatcher().BeginInvoke(() => {
				this.mediaElement.Stop();
				this.subtitleText.set_Text("");
				this.mediaElement.set_AutoPlay(true);
				this.mediaElement.set_Source(new Uri(VideoPlayer.s_sMovieTitle, UriKind.Relative));
				this.mediaElement.Play();
			});
		}

		public void skip_MouseLeftButtonUp(object sender, EventArgs e)
		{
			this.GoToEnd();
		}

		public void StopMovie()
		{
			this.mediaElement.Stop();
			this.subtitleText.set_Text("");
			base.get_Dispatcher().BeginInvoke(() => {
				if (this.m_mainPage.DrawingSurfaceBackground.get_Children().Contains(this))
				{
					this.m_mainPage.DrawingSurfaceBackground.get_Children().Remove(this);
				}
				Direct3DBackground.GetInstance().SetShowMovieFinish();
			});
		}

		public enum LANG
		{
			EN,
			FR,
			DE,
			IT,
			ES,
			JA,
			KO,
			ZH,
			PT,
			RU
		}
	}
}