using System;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Phone.Graphics.Interop;

namespace MC4Component
{
    [ExclusiveTo(typeof(Direct3DBackground))]
    [Guid(2011560094, 54885, 13732, 135, 165, 223, 216, 27, 168, 183, 58)]
    [Version(1)]
    [WebHostHidden]
    internal interface __IDirect3DBackgroundPublicNonVirtuals
    {
        Size NativeResolution
        {
            get;
            set;
        }

        Size RenderResolution
        {
            get;
            set;
        }

        Size WindowBounds
        {
            get;
            set;
        }

        IDrawingSurfaceBackgroundContentProvider CreateContentProvider();

        void DisableGyroscope();

        bool EnableGyroscope();

        void FakeInitXBLUser();

        void FBWallPost([In] string link, [In] string title, [In] string description);

        string GetDeviceFirmwareVersion();

        string GetDeviceName();

        string GetRegionCode();

        bool HasGyroscope();

        void HideGameloftLogo();

        void InitIGPModule();

        bool IsInGLLive();

        bool IsPhoneMusicPlaying();

        void LaunchGLLive();

        void LaunchMarketPlace([In] bool simulate);

        void LaunchReview();

        int NetworkType();

        bool OnBackButtonPressed();

        void OnOrientationChanged([In] bool isLandscapeLeft);

        void PausePhoneMusic();

        void ReplayMovie();

        void ReqPushNotificationPopup();

        void RequestPauseGame();

        void RequestResumeGame();

        void ResumePhoneMusic();

        void SetAutoLockScreenEnabled([In] bool isEnabled);

        void SetFullVersion([In] bool isFull, [In] bool fromSimulation);

        void SetGLLiveState([In] bool glliveShowed);

        void SetIGPState([In] bool igpShowed);

        void SetLaunchByPN([In] bool pn);

        void SetLogoVisible([In] bool visible);

        void SetNeedUpdateGame([In] bool isUpdateGame);

        void SetPhoneHasSetupFaceBookAcc([In] int status);

        void SetPhoneMusicPlaying([In] bool a_bPhoneMusicPlaying);

        void SetPopupResult([In] int a_iButtonPressed);

        void SetShowMovieFinish();

        void ShowAlert([In] string title, [In] string body, [In] string cancelButtonTitle);

        void ShowConfirmMessagePopup([In] string title, [In] string body, [In] int numButton, [In] string firstButtonName, [In] string secondButtonName);

        void ShowMovie([In] string movieTitle);

        bool SimpleCheckWifi();

        void StartIGP();

        void StopMovie();

        event RequestAdditionalFrameHandler RequestAdditionalFrame;
    }
}