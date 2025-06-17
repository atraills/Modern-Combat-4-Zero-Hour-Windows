using System;
using Windows.Foundation;
using Windows.Foundation.Metadata;

namespace MC4Component
{
    [ExclusiveTo(typeof(Direct3DBackground))]
    [Guid(3224850985, 7710, 16167, 177, 49, 121, 162, 62, 33, 79, 181)]
    [Version(1)]
    [WebHostHidden]
    internal interface __IDirect3DBackgroundStatics
    {
        Direct3DBackground GetInstance();

        event FBWallPostHandler FBWallPostEvent;

        event GetDeviceFirmWareVersionHandler GetDeviceFirmwareVersionEvent;

        event GetRegionCodeHandler GetDeviceNameEvent;

        event GetRegionCodeHandler GetRegionCodeEvent;

        event HideGameloftLogoHandler HideGameloftLogoEvent;

        event IsPhoneMusicPlayingHandler IsPhoneMusicPlayingEvent;

        event IsWifiAvailabeHandler IsWifiAvailabeEvent;

        event LaunchGLLiveHandler LaunchGLLiveEvent;

        event LaunchMarketPlaceHandler LaunchMarketPlaceEvent;

        event LaunchReviewHandler LaunchReviewEvent;

        event NetworkTypeEnabledHandler NetworkTypeEnabledEvent;

        event PausePhoneMusicHandler PausePhoneMusicEvent;

        event ReplayMovieHandler ReplayMovieEvent;

        event ResumePhoneMusicHandler ResumePhoneMusicEvent;

        event SetAutoLockScreenEnabledHandler SetAutoLockScreenEnabledEvent;

        event ShowAlertHandler ShowAlertEvent;

        event ShowConfirmMessagePopupHandler ShowConfirmMessagePopupEvent;

        event ShowMovieHandler ShowMovieEvent;

        event StopMovieHandler StopMovieEvent;
    }
}