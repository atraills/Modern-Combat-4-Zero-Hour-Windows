#include "stdafx.h"
#include <roapi.h>
#include <wrl/client.h>
#include <windows.h> 
#include <inspectable.h>
#include <wrl/wrappers/corewrappers.h>
#include <iostream>
#include <d3d11.h>
#include <d3d11_1.h>
#include <dxgi.h>
#include <DirectXTK.h>
#include <DirectXCollision.h>
#include <DirectXColors.h>
#include <DirectXMath.h>
#include <DirectXPackedVector.h>
#include <CommonStates.h>
#include <DDSTextureLoader.h>
#include <Effects.h>
#include <GeometricPrimitive.h>
#include <PrimitiveBatch.h>
#include <SpriteBatch.h>
#include <SpriteFont.h>
#include <VertexTypes.h>
#include <WICTextureLoader.h>


using Microsoft::WRL::ComPtr;
using namespace Microsoft::WRL;
using Microsoft::WRL::Wrappers::HStringReference;






struct __declspec(uuid("0E17C91A-8741-A03B-8A04-BED6AAD99A2A"))
__IDirect3DBackgroundProtectedNonVirtuals : public IUnknown // Or IInspectable, depending on your COM needs
{
    STDMETHOD_(void, OnPointerMoved)(IDrawingSurfaceManipulationHost* sender, PointerEventArgs* args) = 0;
    STDMETHOD_(void, OnPointerPressed)(IDrawingSurfaceManipulationHost* sender, PointerEventArgs* args) = 0;
    STDMETHOD_(void, OnPointerReleased)(IDrawingSurfaceManipulationHost* sender, PointerEventArgs* args) = 0;
};

// Define interfaces (these can be here or in a header)
struct __declspec(uuid("c052f1a9-1e1e-3f27-b131-79a23e214fb5"))
__IDirect3DBackgroundStatics : public IInspectable
{
    STDMETHOD(GetInstance)(IInspectable** instance) = 0;
};

struct __declspec(uuid("77f14e3e-d665-35a4-87a5-dfd81ba8b73a"))
__IDirect3DBackgroundPublicNonVirtuals : public IInspectable
{
    STDMETHOD_(void, RequestPauseGame)() = 0;
    STDMETHOD_(void, Unobscure)() = 0;
};

struct __declspec(uuid(




class WndKey {
public:
    static LRESULT CALLBACK StaticWndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam) {
        WndKey* pThis = nullptr;
        if (msg == WM_NCCREATE) {
            pThis = static_cast<WndKey*>(reinterpret_cast<CREATESTRUCT*>(lParam)->lpCreateParams);
            SetWindowLongPtr(hwnd, GWLP_USERDATA, reinterpret_cast<LONG_PTR>(pThis));
        } else {
            pThis = reinterpret_cast<WndKey*>(GetWindowLongPtr(hwnd, GWLP_USERDATA));
        }

        if (pThis) {
            return pThis->WndProc(hwnd, msg, wParam, lParam);
        }
        return DefWindowProc(hwnd, msg, wParam, lParam);
    }

    LRESULT WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam) {
        switch (msg) {
            case WM_CLOSE:
                PostQuitMessage(0);
                break;
            default:
                return DefWindowProc(hwnd, msg, wParam, lParam);
        }
        return 0;
    }
};
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
    case WM_KEYDOWN:
        // Handle key presses
        // wParam contains the virtual-key code
        switch (wParam)
        {
        case VK_LEFT:
            // Process LEFT ARROW key
            break;
        case VK_RIGHT:
            // Process RIGHT ARROW key
            break;
        // ... handle other keys ...
        }
        break;

    case WM_KEYUP:
        // Handle key releases
        // wParam contains the virtual-key code
        break;

    case WM_CHAR:
        // Handle character input
        // wParam contains the character code
        break;

    case WM_DESTROY:
        PostQuitMessage(0); // Send a WM_QUIT message to exit the application
        break;

    default:
        // Handle other messages
        return DefWindowProc(hWnd, message, wParam, lParam);
    }

    return 0; // Return 0 if the message is handled
}
// Function definition (outside WinMain)
HRESULT CreateDeviceAndSwapChain(HWND hWnd, ComPtr<ID3D11Device>& d3dDevice, ComPtr<ID3D11DeviceContext>& immediateContext, ComPtr<IDXGISwapChain>& swapChain) {
    HRESULT hr = S_OK; // Declare hr within the function's scope
    // ... swap chain creation code ...
    return hr; // Return the HRESULT
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow) {

	// Get the Direct3DBackgroundWrapper instance
Direct3DBackgroundWrapper^ direct3DBackground = __IDirect3DBackgroundPublicNonVirtuals::GetInstance();

// Hook the rendering process !!!!
// direct3DBackground->HookRendering(gcnew NativeRenderingCallback(MyNativeRenderingHandler));

// ... (Rest of your application code) ...



    HRESULT hr = S_OK; // Declare hr here, outside the if statement
    // Initialize the Windows Runtime (if needed)
    hr = RoInitialize(RO_INIT_MULTITHREADED); // Or RO_INIT_APARTMENTTHREADED

	ComPtr<__IDirect3DBackgroundStatics> direct3DBackground; // Adjust type based on what GetInstance returns
    // Use interop to call GetInstance() and get the pointer.
    // e.g., hr = YourInteropCallToGetInstance(&direct3DBackground);

    if (SUCCEEDED(hr))
    {

		ComPtr<IInspectable> contentProvider;
        // ... (rest of Windows Runtime activation code) ...
        // Window creation code
        const auto pClassName = _T("MC4");
        WNDCLASSEX wc = { 0 };
        wc.cbSize = sizeof( wc );
        wc.style = CS_OWNDC;
        wc.lpfnWndProc = DefWindowProc; // Replace with your window procedure
        wc.cbClsExtra = 0;
        wc.cbWndExtra = 0;
        wc.hInstance = hInstance;
        wc.hIcon = nullptr;
        wc.hCursor = nullptr;
        wc.hbrBackground = nullptr;
        wc.lpszMenuName =  nullptr;
        wc.lpszClassName = pClassName;
        wc.hIconSm = nullptr;
        RegisterClassEx( &wc );

        HWND hWnd = CreateWindowEx(
            0,pClassName,
            L"Modern Combat 4: Zero Hour",
            WS_OVERLAPPEDWINDOW,
            CW_USEDEFAULT, CW_USEDEFAULT, 1280, 720,
            nullptr,nullptr,hInstance,nullptr );


		 if (SUCCEEDED(hr))
        {
            // --- Get the D3D surface from the content provider ---
            ComPtr<ID3D11Texture2D> gameRenderSurface; // Adjust type
            // e.g., hr = contentProvider->GetSurface(&gameRenderSurface); // Assuming a GetSurface method

            if (SUCCEEDED(hr))
            {
                // --- Create a render target view for the game's surface ---
                ComPtr<ID3D11RenderTargetView> gameRenderTargetView; // Adjust type
                hr = d3dDevice->CreateRenderTargetView(gameRenderSurface.Get(), nullptr, gameRenderTargetView.ReleaseAndGetAddressOf());

                if (SUCCEEDED(hr))

        if (!hWnd) {
            // Handle window creation error
            RoUninitialize();
            return -1;
        }

        ShowWindow(hWnd, nCmdShow);

        // Create device and swap chain
        ComPtr<ID3D11Device> d3dDevice;
        ComPtr<ID3D11DeviceContext> immediateContext;
        ComPtr<IDXGISwapChain> swapChain;
        hr = CreateDeviceAndSwapChain(hWnd, d3dDevice, immediateContext, swapChain); // Assign the return value to hr

        if (FAILED(hr)) {
            // Handle Direct3D initialization error
            RoUninitialize();
            return -1;
        }

    // --- Declare DirectX 11 resources here ---
    ComPtr<ID3D11Buffer> vertexBuffer; // Example vertex buffer
    ComPtr<ID3D11InputLayout> inputLayout; // Example input layout
    ComPtr<ID3D11VertexShader> vertexShader; // Example vertex shader
    ComPtr<ID3D11PixelShader> pixelShader; // Example pixel shader
    ComPtr<ID3D11RenderTargetView> renderTargetView; // Example render target view
	ComPtr<ID3D11Buffer> indexBuffer;
    // --- Initialize these resources (e.g., load data, create buffers, compile shaders) ---
    // You'll need to replace this with your game's specific initialization
    // Example: (Assuming a function to create a vertex buffer)
    // CreateMyGameVertexBuffers(d3dDevice.Get(), vertexBuffer.ReleaseAndGetAddressOf());
    // CreateMyGameShaders(d3dDevice.Get(), vertexShader.ReleaseAndGetAddressOf(), pixelShader.ReleaseAndGetAddressOf());
    // CreateMyGameInputLayout(d3dDevice.Get(), vertexShader.Get(), inputLayout.ReleaseAndGetAddressOf());
    // --- Create the Render Target View from the swap chain ---
    ComPtr<ID3D11Texture2D> backBufferTexture;
    hr = swapChain->GetBuffer(0, IID_PPV_ARGS(&backBufferTexture));
    if (SUCCEEDED(hr))
    {
        hr = d3dDevice->CreateRenderTargetView(backBufferTexture.Get(), nullptr, renderTargetView.ReleaseAndGetAddressOf());
    }

    
MSG msg = {0};
while (true) // Keep looping indefinitely until WM_QUIT is processed
{
    // PeekMessage checks if a message is available without blocking
    if (PeekMessage(&msg, nullptr, 0, 0, PM_REMOVE))
    {
        // If a message is available, process it
        if (msg.message == WM_QUIT) // Check if the message is WM_QUIT
        {
            break; // Exit the loop
        }

        TranslateMessage(&msg); // Translate virtual-key messages to character messages
        DispatchMessage(&msg);  // Dispatch the message to the window procedure
    }

    else // <-- This is where your rendering code goes
    {
        // --- Rendering Code Starts Here ---
        // 1. Clear the Render Target
        float clearColor[] = { 0.0f, 0.0f, 0.0f, 1.0f }; // Example: Black
        immediateContext->ClearRenderTargetView( MC4Interop::Direct3Dbackground.Get(), clearColor); // {Link: Learn Microsoft https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nf-d3d11-id3d11devicecontext-clearrendertargetview}
        // 2. Set Render Targets (and Depth/Stencil View if you have one)
        immediateContext->OMSetRenderTargets(1, MC4Interop::Direct3Dbackground::__IDirect3DBackgroundProtectedNonVirtuals.GetAddressOf(), nullptr); // {Link: Learn Microsoft https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nf-d3d11-id3d11devicecontext-omsetrendertargets}
        // 3. Set Viewport
        D3D11_VIEWPORT viewport;
        // Set viewport parameters (adjust based on your window size)
        viewport.TopLeftX = 0;
        viewport.TopLeftY = 0;
        viewport.Width = (float)1280; // Get window width
        viewport.Height = (float)720; // Get window height
        viewport.MinDepth = 0.0f;
        viewport.MaxDepth = 1.0f;
        immediateContext->RSSetViewports(1, &viewport);

        // 4. Set Input Assembler (IA) stage
        // Bind vertex buffer(s)
        UINT stride = sizeof(32); // Replace with your vertex structure size
        UINT offset = 0;
        immediateContext->IASetVertexBuffers(0, 1, &vertexBuffer, &stride, &offset); // {Link: Learn Microsoft https://learn.microsoft.com/en-us/windows/win32/direct3d11/d3d10-graphics-programming-guide-input-assembler-stage-getting-started}

		immediateContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
		// Set Shader Stages
		immediateContext->VSSetShader(vertexShader.Get(), nullptr, 0);
        immediateContext->PSSetShader(pixelShader.Get(), nullptr, 0);

        // Bind index buffer (if you're using one)
        immediateContext->IASetIndexBuffer(indexBuffer.Get(), DXGI_FORMAT_R32_UINT, 0); // Assuming 32-bit indices

        // Set input layout
        immediateContext->IASetInputLayout(inputLayout.Get()); // {Link: Learn Microsoft https://learn.microsoft.com/en-us/windows/win32/direct3d11/d3d10-graphics-programming-guide-input-assembler-stage-getting-started}

        // Set primitive topology (e.g., triangle list)
        immediateContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
        // Set viewport
        // ... (set viewport based on window size) ...
        immediateContext->RSSetViewports(1, &viewport);

		  [MethodImpl(MethodCodeType=3)]
        public static extern Direct3DBackground GetInstance();
        // --- Set game-specific resources ---
        // Set vertex buffers, index buffers, input layout, shaders, etc.
        // immediateContext->IASetVertexBuffers(0, 1, gameVertexBuffer.GetAddressOf(), &gameStride, &gameOffset);
        // immediateContext->IASetIndexBuffer(gameIndexBuffer.Get(), DXGI_FORMAT_R32_UINT, 0);
        // immediateContext->IASetInputLayout(gameInputLayout.Get());
        // immediateContext->VSSetShader(gameVertexShader.Get(), nullptr, 0);
        // immediateContext->PSSetShader(gamePixelShader.Get(), nullptr, 0);
        // 7. Present the Swap Chain

        swapChain->Present(0, 0);

        // --- Rendering Code Ends Here ---
	}
}

       // Cleanup
       
   
       // Add this else block here, after the if (SUCCEEDED(hr)) block
   


    return (int)msg.wParam;

}
 

}






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



namespace MC4Component
{
    [ExclusiveTo(typeof(Direct3DBackground))]
    [Guid(448016317, 16775, 15280, 138, 4, 190, 214, 170, 217, 154, 42)]
    [Version(1)]
    [WebHostHidden]
    internal interface __IDirect3DBackgroundProtectedNonVirtuals
    {
        void OnPointerMoved([In] DrawingSurfaceManipulationHost sender, [In] PointerEventArgs args);

        void OnPointerPressed([In] DrawingSurfaceManipulationHost sender, [In] PointerEventArgs args);

        void OnPointerReleased([In] DrawingSurfaceManipulationHost sender, [In] PointerEventArgs args);
    }
}


extern "C" __declspec(dllexport) void InitializeGameRendering()
{
    // Get the Direct3DBackground instance (using managed C++)
    msclr::auto_gcroot<Direct3DBackground> direct3DBackground = Direct3DBackground::GetInstance();

    // Hook into the rendering process (using managed C++)
    // direct3DBackground->HookRendering(...); // Replace with the actual hook mechanism
}

// Implement native callbacks or other logic as needed
// ...
// Define a managed wrapper class for Direct3DBackground
public ref class Direct3DBackgroundWrapper
{
public:
    // Call the game's GetInstance method
    static Direct3DBackgroundWrapper^ GetInstanceWrapper()
    {
        // Use managed C++ to call the game's GetInstance method
        Direct3DBackground^ instance = Direct3DBackground::GetInstance();
        return gcnew Direct3DBackgroundWrapper(instance);
    }

    // Call the game's CompositionTarget_Rendering method (or hook into it)
    void HookRendering(NativeRenderingCallback^ callback)
    {
        // Use managed C++ to hook into the rendering process
        // e.g., CompositionTarget::Rendering += gcnew EventHandler(this, &Direct3DBackgroundWrapper::HandleRendering);
    }

private:
    Direct3DBackground^ m_instance;

    Direct3DBackgroundWrapper(Direct3DBackground^ instance) : m_instance(instance) { }

    // Managed handler for the Rendering event
    void HandleRendering(Object^ sender, EventArgs^ e)
    {
        // Call the native callback function
        m_callback();
    }

    NativeRenderingCallback^ m_callback;
};

// In your native C++ application:

// Define a native callback function
void MyNativeRenderingHandler(void* d3dDevice, void* d3dContext, ...) {
    // ... (Your rendering code) ...
}

// In WinMain or initialization code:

// Get the Direct3DBackgroundWrapper instance
Direct3DBackgroundWrapper^ direct3DBackground = Direct3DBackgroundWrapper::GetInstanceWrapper();

// Hook the rendering process
direct3DBackground->HookRendering(gcnew NativeRenderingCallback(MyNativeRenderingHandler));





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


namespace MC4Interop.Resources
{
    [CompilerGenerated]
    [DebuggerNonUserCode]
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    public class AppResources
    {
        private static ResourceManager resourceMan;

        private static CultureInfo resourceCulture;

        public static string AppBarButtonText
        {
            get
            {
                return AppResources.ResourceManager.GetString("AppBarButtonText", AppResources.resourceCulture);
            }
        }

        public static string AppBarMenuItemText
        {
            get
            {
                return AppResources.ResourceManager.GetString("AppBarMenuItemText", AppResources.resourceCulture);
            }
        }

        public static string ApplicationTitle
        {
            get
            {
                return AppResources.ResourceManager.GetString("ApplicationTitle", AppResources.resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static CultureInfo Culture
        {
            get
            {
                return AppResources.resourceCulture;
            }
            set
            {
                AppResources.resourceCulture = value;
            }
        }

        public static string ResourceFlowDirection
        {
            get
            {
                return AppResources.ResourceManager.GetString("ResourceFlowDirection", AppResources.resourceCulture);
            }
        }

        public static string ResourceLanguage
        {
            get
            {
                return AppResources.ResourceManager.GetString("ResourceLanguage", AppResources.resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(AppResources.resourceMan, null))
                {
                    AppResources.resourceMan = new ResourceManager("MC4Interop.Resources.AppResources", typeof(AppResources).Assembly);
                }
                return AppResources.resourceMan;
            }
        }

        public static string ServiceUnavailable
        {
            get
            {
                return AppResources.ResourceManager.GetString("ServiceUnavailable", AppResources.resourceCulture);
            }
        }

        internal AppResources()
        {
        }
    }
}

using MC4Component 
using MC4Component::Direct3DBackground::IDrawingSurfaceBackgroundContentProvider::CreateContentProvider 

// This might not be needed, but the game might not think so; it may need translation,
namespace MC4Interop
{
    public class App : Application
    {
        private const string PeriodicTaskName = "MC4";

        private bool phoneApplicationInitialized;

        private bool _contentLoaded;

        public PhoneApplicationFrame RootFrame
        {
            get;
            private set;
        }

        public App()
        {
            base.add_UnhandledException(new EventHandler<ApplicationUnhandledExceptionEventArgs>(this.Application_UnhandledException));
            this.InitializeComponent();
            this.InitializePhoneApplication();
            this.RootFrame.add_Obscured(new EventHandler<ObscuredEventArgs>(this.Application_Obscured));
            this.RootFrame.add_Unobscured(new EventHandler(this.Application_Unobscured));
            if (Debugger.IsAttached)
            {
                Application.get_Current().get_Host().get_Settings().set_EnableFrameRateCounter(true);
                PhoneApplicationService.get_Current().set_UserIdleDetectionMode(1);
            }
        }

        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            LicenseInformation licenseInformation = new LicenseInformation();
            Direct3DBackground.GetInstance().SetFullVersion(!licenseInformation.IsTrial(), false);
            this.RemoveAgent();
            PNLib.resumed = true;
        }

        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            this.StartPeriodicAgent();
        }

        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            this.StartPeriodicAgent();
            PNLib.resumed = false;
        }

        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            LicenseInformation licenseInformation = new LicenseInformation();
            Direct3DBackground.GetInstance().SetFullVersion(!licenseInformation.IsTrial(), false);
            this.RemoveAgent();
            LiveTileHanlder.UpdateFlipTemplate();
        }

        private void Application_Obscured(object sender, ObscuredEventArgs e)
        {
            Direct3DBackground.GetInstance().RequestPauseGame();
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        private void Application_Unobscured(object sender, EventArgs e)
        {
            Direct3DBackground.GetInstance().RequestResumeGame();
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            if (e.get_NavigationMode() == 4)
            {
                this.RootFrame.add_Navigated(new NavigatedEventHandler(this, App.ClearBackStackAfterReset));
            }
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            this.RootFrame.remove_Navigated(new NavigatedEventHandler(this, App.ClearBackStackAfterReset));
            if (e.get_NavigationMode() != null)
            {
                return;
            }
            while (this.RootFrame.RemoveBackEntry() != null)
            {
            }
        }

        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            if (base.get_RootVisual() != this.RootFrame)
            {
                base.set_RootVisual(this.RootFrame);
            }
            this.RootFrame.remove_Navigated(new NavigatedEventHandler(this, App.CompleteInitializePhoneApplication));
        }

        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
            {
                return;
            }
            this._contentLoaded = true;
            Application.LoadComponent(this, new Uri("/MC4Interop;component/App.xaml", UriKind.Relative));
        }

        private void InitializePhoneApplication()
        {
            if (this.phoneApplicationInitialized)
            {
                return;
            }
            this.RootFrame = new PhoneApplicationFrame();
            this.RootFrame.add_Navigated(new NavigatedEventHandler(this, App.CompleteInitializePhoneApplication));
            this.RootFrame.add_NavigationFailed(new NavigationFailedEventHandler(this, App.RootFrame_NavigationFailed));
            this.RootFrame.add_Navigated(new NavigatedEventHandler(this, App.CheckForResetNavigation));
            this.phoneApplicationInitialized = true;
        }

        private void RemoveAgent(string name)
        {
            try
            {
                if (ScheduledActionService.Find(name) is PeriodicTask)
                {
                    ScheduledActionService.Remove(name);
                }
            }
            catch (Exception exception)
            {
            }
        }

        private void RemoveAgent()
        {
            this.RemoveAgent("MC4");
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        private void StartPeriodicAgent()
        {
            PeriodicTask periodicTask = ScheduledActionService.Find("MC4") as PeriodicTask;
            if (periodicTask != null)
            {
                this.RemoveAgent("MC4");
            }
            periodicTask = new PeriodicTask("MC4");
            periodicTask.set_Description("--------");
            try
            {
                ScheduledActionService.Add(periodicTask);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                exception.Message.Contains("BNS Error: The action is disabled");
                exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added.");
            }
        }
    }
}