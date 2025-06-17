using System;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Phone.Graphics.Interop;
using Windows.Phone.Input.Interop;
using Windows.UI.Core;

namespace MC4Component
{
    [MarshalingBehavior(MarshalingType.Agile)]
    [Static(typeof(__IDirect3DBackgroundStatics), 1)]
    [Threading(ThreadingModel.Both)]
    [Version(1)]
    [WebHostHidden]
    public sealed class Direct3DBackground : IDrawingSurfaceManipulationHandler, __IDirect3DBackgroundPublicNonVirtuals, __IDirect3DBackgroundProtectedNonVirtuals
    {
        public Size NativeResolution
        {
            [MethodImpl(MethodCodeType=3)]
            get;
            [MethodImpl(MethodCodeType=3)]
            set;
        }

        public Size RenderResolution
        {
            [MethodImpl(MethodCodeType=3)]
            get;
            [MethodImpl(MethodCodeType=3)]
            set;
        }

        public Size WindowBounds
        {
            [MethodImpl(MethodCodeType=3)]
            get;
            [MethodImpl(MethodCodeType=3)]
            set;
        }

        [MethodImpl(MethodCodeType=3)]
        public extern IDrawingSurfaceBackgroundContentProvider CreateContentProvider();

        [MethodImpl(MethodCodeType=3)]
        public extern void DisableGyroscope();

        [MethodImpl(MethodCodeType=3)]
        public extern bool EnableGyroscope();

        [MethodImpl(MethodCodeType=3)]
        public extern void FakeInitXBLUser();

        [MethodImpl(MethodCodeType=3)]
        public extern void FBWallPost([In] string link, [In] string title, [In] string description);

        [MethodImpl(MethodCodeType=3)]
        public extern string GetDeviceFirmwareVersion();

        [MethodImpl(MethodCodeType=3)]
        public extern string GetDeviceName();

        [MethodImpl(MethodCodeType=3)]
        public static extern Direct3DBackground GetInstance();

        [MethodImpl(MethodCodeType=3)]
        public extern string GetRegionCode();

        [MethodImpl(MethodCodeType=3)]
        public extern bool HasGyroscope();

        [MethodImpl(MethodCodeType=3)]
        public extern void HideGameloftLogo();

        [MethodImpl(MethodCodeType=3)]
        public extern void InitIGPModule();

        [MethodImpl(MethodCodeType=3)]
        public extern bool IsInGLLive();

        [MethodImpl(MethodCodeType=3)]
        public extern bool IsPhoneMusicPlaying();

        [MethodImpl(MethodCodeType=3)]
        public extern void LaunchGLLive();

        [MethodImpl(MethodCodeType=3)]
        public extern void LaunchMarketPlace([In] bool simulate);

        [MethodImpl(MethodCodeType=3)]
        public extern void LaunchReview();

        [MethodImpl(MethodCodeType=3)]
        public extern int NetworkType();

        [MethodImpl(MethodCodeType=3)]
        public extern bool OnBackButtonPressed();

        [MethodImpl(MethodCodeType=3)]
        public extern void OnOrientationChanged([In] bool isLandscapeLeft);

        [MethodImpl(MethodCodeType=3)]
        protected extern void OnPointerMoved([In] DrawingSurfaceManipulationHost sender, [In] PointerEventArgs args);

        [MethodImpl(MethodCodeType=3)]
        protected extern void OnPointerPressed([In] DrawingSurfaceManipulationHost sender, [In] PointerEventArgs args);

        [MethodImpl(MethodCodeType=3)]
        protected extern void OnPointerReleased([In] DrawingSurfaceManipulationHost sender, [In] PointerEventArgs args);

        [MethodImpl(MethodCodeType=3)]
        public extern void PausePhoneMusic();

        [MethodImpl(MethodCodeType=3)]
        public extern void ReplayMovie();

        [MethodImpl(MethodCodeType=3)]
        public extern void ReqPushNotificationPopup();

        [MethodImpl(MethodCodeType=3)]
        public extern void RequestPauseGame();

        [MethodImpl(MethodCodeType=3)]
        public extern void RequestResumeGame();

        [MethodImpl(MethodCodeType=3)]
        public extern void ResumePhoneMusic();

        [MethodImpl(MethodCodeType=3)]
        public extern void SetAutoLockScreenEnabled([In] bool isEnabled);

        [MethodImpl(MethodCodeType=3)]
        public extern void SetFullVersion([In] bool isFull, [In] bool fromSimulation);

        [MethodImpl(MethodCodeType=3)]
        public extern void SetGLLiveState([In] bool glliveShowed);

        [MethodImpl(MethodCodeType=3)]
        public extern void SetIGPState([In] bool igpShowed);

        [MethodImpl(MethodCodeType=3)]
        public extern void SetLaunchByPN([In] bool pn);

        [MethodImpl(MethodCodeType=3)]
        public extern void SetLogoVisible([In] bool visible);

        [MethodImpl(MethodCodeType=3)]
        public extern void SetManipulationHost([In] DrawingSurfaceManipulationHost manipulationHost);

        [MethodImpl(MethodCodeType=3)]
        public extern void SetNeedUpdateGame([In] bool isUpdateGame);

        [MethodImpl(MethodCodeType=3)]
        public extern void SetPhoneHasSetupFaceBookAcc([In] int status);

        [MethodImpl(MethodCodeType=3)]
        public extern void SetPhoneMusicPlaying([In] bool a_bPhoneMusicPlaying);

        [MethodImpl(MethodCodeType=3)]
        public extern void SetPopupResult([In] int a_iButtonPressed);

        [MethodImpl(MethodCodeType=3)]
        public extern void SetShowMovieFinish();

        [MethodImpl(MethodCodeType=3)]
        public extern void ShowAlert([In] string title, [In] string body, [In] string cancelButtonTitle);

        [MethodImpl(MethodCodeType=3)]
        public extern void ShowConfirmMessagePopup([In] string title, [In] string body, [In] int numButton, [In] string firstButtonName, [In] string secondButtonName);

        [MethodImpl(MethodCodeType=3)]
        public extern void ShowMovie([In] string movieTitle);

        [MethodImpl(MethodCodeType=3)]
        public extern bool SimpleCheckWifi();

        [MethodImpl(MethodCodeType=3)]
        public extern void StartIGP();

        [MethodImpl(MethodCodeType=3)]
        public extern void StopMovie();

        public static event FBWallPostHandler FBWallPostEvent;

        public static event GetDeviceFirmWareVersionHandler GetDeviceFirmwareVersionEvent;

        public static event GetRegionCodeHandler GetDeviceNameEvent;

        public static event GetRegionCodeHandler GetRegionCodeEvent;

        public static event HideGameloftLogoHandler HideGameloftLogoEvent;

        public static event IsPhoneMusicPlayingHandler IsPhoneMusicPlayingEvent;

        public static event IsWifiAvailabeHandler IsWifiAvailabeEvent;

        public static event LaunchGLLiveHandler LaunchGLLiveEvent;

        public static event LaunchMarketPlaceHandler LaunchMarketPlaceEvent;

        public static event LaunchReviewHandler LaunchReviewEvent;

        public static event NetworkTypeEnabledHandler NetworkTypeEnabledEvent;

        public static event PausePhoneMusicHandler PausePhoneMusicEvent;

        public static event ReplayMovieHandler ReplayMovieEvent;

        public event RequestAdditionalFrameHandler RequestAdditionalFrame;

        public static event ResumePhoneMusicHandler ResumePhoneMusicEvent;

        public static event SetAutoLockScreenEnabledHandler SetAutoLockScreenEnabledEvent;

        public static event ShowAlertHandler ShowAlertEvent;

        public static event ShowConfirmMessagePopupHandler ShowConfirmMessagePopupEvent;

        public static event ShowMovieHandler ShowMovieEvent;

        public static event StopMovieHandler StopMovieEvent;
    }
}