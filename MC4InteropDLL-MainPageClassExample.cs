using IGPBridgeLibrary;
using MC4Component;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.UserData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using Push_WP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Navigation;
using System.Windows.Threading;
using Windows.Foundation;

namespace MC4Interop
{
    public class MainPage : PhoneApplicationPage, IApplicationService
    {
        private string m_link;

        private string m_title;

        private string m_description;

        private Direct3DBackground m_d3dBackground = Direct3DBackground.GetInstance();

        private MarketplaceDetailTask _marketPlaceDetailTask = new MarketplaceDetailTask();

        private MarketplaceReviewTask _marketPlaceReviewTask = new MarketplaceReviewTask();

        private VideoPlayer m_videoPlayer;

        private static bool m_IsFirstTime;

        private DispatcherTimer frameworkDispatcherTimer;

        private static int count;

        private GLiveControl GLiveCtrl;

        private ServiceUnavailable GLiveErrorCtrl;

        internal DrawingSurfaceBackgroundGrid DrawingSurfaceBackground;

        internal Image ImgGameloftLogo;

        private bool _contentLoaded;

        static MainPage()
        {
            MainPage.m_IsFirstTime = true;
            MainPage.count = 0;
        }

        public MainPage()
        {
            try
            {
                PNLib.Register();
            }
            catch
            {
            }
            this.InitializeComponent();
            this.frameworkDispatcherTimer = new DispatcherTimer();
            this.frameworkDispatcherTimer.set_Interval(TimeSpan.FromTicks((long)333333));
            this.frameworkDispatcherTimer.add_Tick(new EventHandler(this.frameworkDispatcherTimer_Tick));
            FrameworkDispatcher.Update();
            WindowsRuntimeMarshal.AddEventHandler<LaunchGLLiveHandler>(new Func<LaunchGLLiveHandler, EventRegistrationToken>(null, Direct3DBackground.add_LaunchGLLiveEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_LaunchGLLiveEvent), new LaunchGLLiveHandler(this.LaunchGLLive));
            WindowsRuntimeMarshal.AddEventHandler<ShowMovieHandler>(new Func<ShowMovieHandler, EventRegistrationToken>(null, Direct3DBackground.add_ShowMovieEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_ShowMovieEvent), new ShowMovieHandler(this.ShowMovie));
            WindowsRuntimeMarshal.AddEventHandler<StopMovieHandler>(new Func<StopMovieHandler, EventRegistrationToken>(null, Direct3DBackground.add_StopMovieEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_StopMovieEvent), new StopMovieHandler(this.StopMovie));
            WindowsRuntimeMarshal.AddEventHandler<ReplayMovieHandler>(new Func<ReplayMovieHandler, EventRegistrationToken>(null, Direct3DBackground.add_ReplayMovieEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_ReplayMovieEvent), new ReplayMovieHandler(this.ReplayMovie));
            WindowsRuntimeMarshal.AddEventHandler<LaunchMarketPlaceHandler>(new Func<LaunchMarketPlaceHandler, EventRegistrationToken>(null, Direct3DBackground.add_LaunchMarketPlaceEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_LaunchMarketPlaceEvent), new LaunchMarketPlaceHandler(this.LaunchMarketPlace));
            WindowsRuntimeMarshal.AddEventHandler<LaunchReviewHandler>(new Func<LaunchReviewHandler, EventRegistrationToken>(null, Direct3DBackground.add_LaunchReviewEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_LaunchReviewEvent), new LaunchReviewHandler(this.LaunchReview));
            WindowsRuntimeMarshal.AddEventHandler<SetAutoLockScreenEnabledHandler>(new Func<SetAutoLockScreenEnabledHandler, EventRegistrationToken>(null, Direct3DBackground.add_SetAutoLockScreenEnabledEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_SetAutoLockScreenEnabledEvent), new SetAutoLockScreenEnabledHandler(this.SetAutoLockScreenEnabled));
            WindowsRuntimeMarshal.AddEventHandler<ShowAlertHandler>(new Func<ShowAlertHandler, EventRegistrationToken>(null, Direct3DBackground.add_ShowAlertEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_ShowAlertEvent), new ShowAlertHandler(this.ShowAlert));
            WindowsRuntimeMarshal.AddEventHandler<FBWallPostHandler>(new Func<FBWallPostHandler, EventRegistrationToken>(null, Direct3DBackground.add_FBWallPostEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_FBWallPostEvent), new FBWallPostHandler(this.PostFB));
            WindowsRuntimeMarshal.AddEventHandler<NetworkTypeEnabledHandler>(new Func<NetworkTypeEnabledHandler, EventRegistrationToken>(null, Direct3DBackground.add_NetworkTypeEnabledEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_NetworkTypeEnabledEvent), new NetworkTypeEnabledHandler(this.NetworkAccessType));
            WindowsRuntimeMarshal.AddEventHandler<IsWifiAvailabeHandler>(new Func<IsWifiAvailabeHandler, EventRegistrationToken>(null, Direct3DBackground.add_IsWifiAvailabeEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_IsWifiAvailabeEvent), new IsWifiAvailabeHandler(this.IsWifiAvailabe));
            WindowsRuntimeMarshal.AddEventHandler<HideGameloftLogoHandler>(new Func<HideGameloftLogoHandler, EventRegistrationToken>(null, Direct3DBackground.add_HideGameloftLogoEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_HideGameloftLogoEvent), new HideGameloftLogoHandler(this.HideGameloftLogo));
            WindowsRuntimeMarshal.AddEventHandler<ShowConfirmMessagePopupHandler>(new Func<ShowConfirmMessagePopupHandler, EventRegistrationToken>(null, Direct3DBackground.add_ShowConfirmMessagePopupEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_ShowConfirmMessagePopupEvent), new ShowConfirmMessagePopupHandler(this.ShowConfirmMessagePopup));
            WindowsRuntimeMarshal.AddEventHandler<IsPhoneMusicPlayingHandler>(new Func<IsPhoneMusicPlayingHandler, EventRegistrationToken>(null, Direct3DBackground.add_IsPhoneMusicPlayingEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_IsPhoneMusicPlayingEvent), new IsPhoneMusicPlayingHandler(this.IsPhoneMusicPlaying));
            WindowsRuntimeMarshal.AddEventHandler<PausePhoneMusicHandler>(new Func<PausePhoneMusicHandler, EventRegistrationToken>(null, Direct3DBackground.add_PausePhoneMusicEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_PausePhoneMusicEvent), new PausePhoneMusicHandler(this.PausePhoneMusic));
            WindowsRuntimeMarshal.AddEventHandler<ResumePhoneMusicHandler>(new Func<ResumePhoneMusicHandler, EventRegistrationToken>(null, Direct3DBackground.add_ResumePhoneMusicEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_ResumePhoneMusicEvent), new ResumePhoneMusicHandler(this.ResumePhoneMusic));
            WindowsRuntimeMarshal.AddEventHandler<GetDeviceFirmWareVersionHandler>(new Func<GetDeviceFirmWareVersionHandler, EventRegistrationToken>(null, Direct3DBackground.add_GetDeviceFirmwareVersionEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_GetDeviceFirmwareVersionEvent), new GetDeviceFirmWareVersionHandler(this.GetFirmwareVersion));
            WindowsRuntimeMarshal.AddEventHandler<GetRegionCodeHandler>(new Func<GetRegionCodeHandler, EventRegistrationToken>(null, Direct3DBackground.add_GetRegionCodeEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_GetRegionCodeEvent), new GetRegionCodeHandler(this.GetRegionCode));
            WindowsRuntimeMarshal.AddEventHandler<GetRegionCodeHandler>(new Func<GetRegionCodeHandler, EventRegistrationToken>(null, Direct3DBackground.add_GetDeviceNameEvent), new Action<EventRegistrationToken>(Direct3DBackground.remove_GetDeviceNameEvent), new GetRegionCodeHandler(this.GetDeviceName));
            IGPBridgeClass.InitIGPBridgeClass(this.DrawingSurfaceBackground);
            this.m_d3dBackground.InitIGPModule();
            this.m_d3dBackground.FakeInitXBLUser();
            this.m_d3dBackground.SetIGPState(false);
            this.m_d3dBackground.SetGLLiveState(false);
            if (MainPage.m_IsFirstTime)
            {
                MainPage.m_IsFirstTime = false;
                this.ImgGameloftLogo.set_Visibility(0);
                if (MediaPlayer.get_GameHasControl())
                {
                    this.m_d3dBackground.SetPhoneMusicPlaying(false);
                    return;
                }
                this.m_d3dBackground.SetPhoneMusicPlaying(true);
            }
        }

        private void Contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            bool flag = false;
            try
            {
                using (IEnumerator<Contact> enumerator = e.get_Results().GetEnumerator())
                {
                    do
                    {
                        if (!enumerator.MoveNext())
                        {
                            break;
                        }
                        foreach (Account account in enumerator.Current.get_Accounts())
                        {
                            if (account.get_Kind() != 3)
                            {
                                continue;
                            }
                            flag = true;
                            break;
                        }
                    }
                    while (!flag);
                }
            }
            catch (Exception exception)
            {
            }
            if (!flag)
            {
                this.m_d3dBackground.SetPhoneHasSetupFaceBookAcc(2);
                return;
            }
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.set_Title(this.m_title);
            shareLinkTask.set_LinkUri(new Uri(this.m_link, UriKind.Absolute));
            shareLinkTask.set_Message(this.m_description);
            this.m_d3dBackground.SetPhoneHasSetupFaceBookAcc(1);
            shareLinkTask.Show();
        }

        private void DrawingSurfaceBackground_Loaded(object sender, RoutedEventArgs e)
        {
            this.m_d3dBackground.WindowBounds = new Size((double)((float)Application.get_Current().get_Host().get_Content().get_ActualWidth()), (double)((float)Application.get_Current().get_Host().get_Content().get_ActualHeight()));
            this.m_d3dBackground.NativeResolution = new Size((double)((float)Math.Floor(Application.get_Current().get_Host().get_Content().get_ActualWidth() * (double)Application.get_Current().get_Host().get_Content().get_ScaleFactor() / 100 + 0.5)), (double)((float)Math.Floor(Application.get_Current().get_Host().get_Content().get_ActualHeight() * (double)Application.get_Current().get_Host().get_Content().get_ScaleFactor() / 100 + 0.5)));
            this.m_d3dBackground.RenderResolution = this.m_d3dBackground.NativeResolution;
            this.DrawingSurfaceBackground.SetBackgroundContentProvider(this.m_d3dBackground.CreateContentProvider());
            this.DrawingSurfaceBackground.SetBackgroundManipulationHandler(this.m_d3dBackground);
        }

        private void ExitGLLive()
        {
            if (this.m_d3dBackground.IsInGLLive())
            {
                if (this.DrawingSurfaceBackground.get_Children().Contains(this.GLiveCtrl))
                {
                    this.DrawingSurfaceBackground.get_Children().Remove(this.GLiveCtrl);
                }
                this.GLiveCtrl = null;
                this.m_d3dBackground.SetGLLiveState(false);
            }
        }

        private void ExitGLLiveFromError()
        {
            if (this.m_d3dBackground.IsInGLLive())
            {
                if (this.DrawingSurfaceBackground.get_Children().Contains(this.GLiveErrorCtrl))
                {
                    this.DrawingSurfaceBackground.get_Children().Remove(this.GLiveErrorCtrl);
                }
                this.GLiveErrorCtrl = null;
                this.m_d3dBackground.SetGLLiveState(false);
            }
        }

        private void frameworkDispatcherTimer_Tick(object sender, EventArgs e)
        {
            FrameworkDispatcher.Update();
        }

        private string GetDeviceName()
        {
            string str;
            try
            {
                string deviceName = DeviceStatus.get_DeviceName();
                if (string.IsNullOrEmpty(deviceName))
                {
                    deviceName = "WP8";
                }
                str = deviceName;
            }
            catch (Exception exception)
            {
                return "WP8";
            }
            return str;
        }

        private string GetFirmwareVersion()
        {
            string deviceFirmwareVersion;
            try
            {
                deviceFirmwareVersion = DeviceStatus.get_DeviceFirmwareVersion();
            }
            catch (Exception exception)
            {
                return "";
            }
            return deviceFirmwareVersion;
        }

        private string GetRegionCode()
        {
            try
            {
                RegionInfo currentRegion = RegionInfo.CurrentRegion;
                if (currentRegion != null)
                {
                    return currentRegion.Name;
                }
            }
            catch (Exception exception)
            {
            }
            return "";
        }

        private void HideGameloftLogo()
        {
            base.get_Dispatcher().BeginInvoke(() => {
                if (this.DrawingSurfaceBackground.get_Children().Contains(this.ImgGameloftLogo))
                {
                    this.DrawingSurfaceBackground.get_Children().Remove(this.ImgGameloftLogo);
                }
                this.ImgGameloftLogo.set_Visibility(1);
                this.m_d3dBackground.SetLogoVisible(false);
            });
        }

        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
            {
                return;
            }
            this._contentLoaded = true;
            Application.LoadComponent(this, new Uri("/MC4Interop;component/MainPage.xaml", UriKind.Relative));
            this.DrawingSurfaceBackground = (DrawingSurfaceBackgroundGrid)base.FindName("DrawingSurfaceBackground");
            this.ImgGameloftLogo = (Image)base.FindName("ImgGameloftLogo");
        }

        private bool IsPhoneMusicPlaying()
        {
            return !MediaPlayer.get_GameHasControl();
        }

        private bool IsWifiAvailabe()
        {
            if (DeviceNetworkInformation.get_IsNetworkAvailable())
            {
                return DeviceNetworkInformation.get_IsWiFiEnabled();
            }
            return false;
        }

        private void LaunchGLLive()
        {
            try
            {
                base.get_Dispatcher().BeginInvoke(() => {
                    this.GLiveCtrl = null;
                    this.GLiveCtrl = new GLiveControl();
                    this.GLiveCtrl.SetExitFunc(new delegateExitGLLive(this.ExitGLLive));
                    this.GLiveCtrl.SetShowErrorFunc(new delegateShowGLLiveError(this.ShowGLLiveError));
                    this.DrawingSurfaceBackground.get_Children().Add(this.GLiveCtrl);
                    this.m_d3dBackground.SetGLLiveState(true);
                });
            }
            catch (Exception exception)
            {
            }
        }

        private void LaunchMarketPlace(bool simulate)
        {
            if (!simulate)
            {
                this._marketPlaceDetailTask.Show();
                return;
            }
            base.get_Dispatcher().BeginInvoke(() => {
                if (MessageBox.Show("This version demonstrates the implementation of TnB. Press 'OK' to simulate Purchase successful. Press 'Cancel' to continue in trial mode.", "Debug TnB", 1) == 1)
                {
                    Direct3DBackground.GetInstance().SetFullVersion(true, true);
                }
            });
        }

        private void LaunchReview()
        {
            this._marketPlaceReviewTask.Show();
        }

        private int NetworkAccessType()
        {
            MainPage.<>c__DisplayClassa variable = null;
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);
            int num = 0;
            DeviceNetworkInformation.ResolveHostNameAsync(new DnsEndPoint("microsoft.com", 80), new NameResolutionCallback(variable, (NameResolutionResult networkInfo) => {
                NetworkInterfaceInfo networkInterface = networkInfo.get_NetworkInterface();
                if (networkInterface != null)
                {
                    NetworkInterfaceType interfaceType = networkInterface.get_InterfaceType();
                    if (interfaceType == 6)
                    {
                        this.internetConnectionAvailable = 1;
                    }
                    else if (interfaceType == 71)
                    {
                        this.internetConnectionAvailable = 1;
                    }
                    else
                    {
                        switch (interfaceType)
                        {
                            case 145:
                            case 146:
                            {
                                switch (networkInterface.get_InterfaceSubtype())
                                {
                                    case 1:
                                    case 2:
                                    case 4:
                                    {
                                        this.internetConnectionAvailable = 2;
                                        break;
                                    }
                                    case 3:
                                    case 5:
                                    case 6:
                                    case 7:
                                    {
                                        this.internetConnectionAvailable = 3;
                                        break;
                                    }
                                    case 10:
                                    case 11:
                                    {
                                        this.internetConnectionAvailable = 4;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                this.manualResetEvent.Set();
            }), null);
            manualResetEvent.WaitOne(TimeSpan.FromMilliseconds(500));
            if (num == 0)
            {
                if (this.IsWifiAvailabe())
                {
                    return 1;
                }
                return 0;
            }
            if (num == 1)
            {
                return 2;
            }
            if (num >= 2 && num <= 4)
            {
                return 3;
            }
            return 0;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (!this.m_d3dBackground.IsInGLLive())
            {
                if (!VideoPlayer.s_IsInVideoPlayer)
                {
                    e.Cancel = this.m_d3dBackground.OnBackButtonPressed();
                    return;
                }
                this.m_videoPlayer.GoToEnd();
                VideoPlayer.s_IsInVideoPlayer = false;
                e.Cancel = true;
                return;
            }
            if (this.DrawingSurfaceBackground.get_Children().Contains(this.GLiveCtrl))
            {
                this.DrawingSurfaceBackground.get_Children().Remove(this.GLiveCtrl);
            }
            this.GLiveCtrl = null;
            if (this.DrawingSurfaceBackground.get_Children().Contains(this.GLiveErrorCtrl))
            {
                this.DrawingSurfaceBackground.get_Children().Remove(this.GLiveErrorCtrl);
            }
            this.GLiveErrorCtrl = null;
            this.m_d3dBackground.SetGLLiveState(false);
            e.Cancel = true;
        }

        private void OnMessageBoxClosed(IAsyncResult ar)
        {
            int? nullable = Guide.EndShowMessageBox(ar);
            if (!nullable.HasValue)
            {
                this.m_d3dBackground.SetPopupResult(2);
                return;
            }
            this.m_d3dBackground.SetPopupResult(nullable.Value + 1);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if ((PNLib.resumed && MainPage.count == 1 || !PNLib.resumed) && PNLib.ParseNotification(base.get_NavigationContext()))
            {
                this.m_d3dBackground.SetLaunchByPN(true);
            }
            if (PNLib.resumed)
            {
                if (MainPage.count == 0)
                {
                    MainPage.count++;
                    return;
                }
                MainPage.count = 0;
            }
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            base.OnOrientationChanged(e);
            if (e.get_Orientation() == 18)
            {
                this.m_d3dBackground.OnOrientationChanged(false);
                return;
            }
            if (e.get_Orientation() == 34)
            {
                this.m_d3dBackground.OnOrientationChanged(true);
            }
        }

        private void PausePhoneMusic()
        {
            MediaPlayer.Pause();
        }

        private void PostFB(string link, string title, string description)
        {
            this.m_link = link;
            this.m_title = title;
            this.m_description = description;
            Contacts contact = new Contacts();
            contact.add_SearchCompleted(new EventHandler<ContactsSearchEventArgs>(this.Contacts_SearchCompleted));
            contact.SearchAsync(string.Empty, 0, string.Empty);
        }

        private void PostMediaFB(string photolink, string title, string description)
        {
            ShareMediaTask shareMediaTask = new ShareMediaTask();
            shareMediaTask.set_FilePath(photolink);
            shareMediaTask.Show();
        }

        public void ReplayMovie()
        {
            if (this.m_videoPlayer != null)
            {
                this.m_videoPlayer.ReplayMovie();
                VideoPlayer.s_IsInVideoPlayer = true;
            }
        }

        private void ResumePhoneMusic()
        {
            MediaPlayer.Resume();
        }

        private void SetAutoLockScreenEnabled(bool isEnabled)
        {
            if (isEnabled)
            {
                PhoneApplicationService.get_Current().set_UserIdleDetectionMode(0);
                return;
            }
            PhoneApplicationService.get_Current().set_UserIdleDetectionMode(1);
        }

        private void ShowAlert(string title, string body, string button)
        {
            List<string> strs = new List<string>()
            {
                button
            };
            Guide.BeginShowMessageBox(title, body, strs, 0, 0, null, null);
        }

        public void ShowConfirmMessagePopup(string title, string body, int numButton, string firstButtonName, string secondButtonName)
        {
            List<string> strs;
            if (numButton == 0)
            {
                return;
            }
            strs = (numButton != 1 ? new List<string>()
            {
                firstButtonName,
                secondButtonName
            } : new List<string>()
            {
                firstButtonName
            });
            Guide.BeginShowMessageBox(title, body, strs, 0, 1, new AsyncCallback(this.OnMessageBoxClosed), null);
        }

        private void ShowGLLiveError()
        {
            if (this.m_d3dBackground.IsInGLLive())
            {
                if (this.DrawingSurfaceBackground.get_Children().Contains(this.GLiveCtrl))
                {
                    this.DrawingSurfaceBackground.get_Children().Remove(this.GLiveCtrl);
                }
                this.GLiveCtrl = null;
                try
                {
                    base.get_Dispatcher().BeginInvoke(() => {
                        this.GLiveErrorCtrl = null;
                        this.GLiveErrorCtrl = new ServiceUnavailable();
                        this.GLiveErrorCtrl.SetExitFunc(new delegateExitGLLiveFromError(this.ExitGLLiveFromError));
                        this.DrawingSurfaceBackground.get_Children().Add(this.GLiveErrorCtrl);
                    });
                }
                catch (Exception exception)
                {
                }
            }
        }

        public void ShowMovie(string movieTitle, int lang)
        {
            VideoPlayer.s_sMovieTitle = movieTitle;
            VideoPlayer.s_Lang = (VideoPlayer.LANG)lang;
            VideoPlayer.s_IsInVideoPlayer = true;
            try
            {
                base.get_Dispatcher().BeginInvoke(() => {
                    this.m_videoPlayer = new VideoPlayer(this);
                    this.DrawingSurfaceBackground.get_Children().Add(this.m_videoPlayer);
                });
            }
            catch (Exception exception)
            {
            }
        }

        public void StopMovie()
        {
            if (this.m_videoPlayer != null)
            {
                this.m_videoPlayer.StopMovie();
            }
            VideoPlayer.s_IsInVideoPlayer = false;
        }

        void System.Windows.IApplicationService.StartService(ApplicationServiceContext context)
        {
            this.frameworkDispatcherTimer.Start();
        }

        void System.Windows.IApplicationService.StopService()
        {
            this.frameworkDispatcherTimer.Stop();
        }
    }
}