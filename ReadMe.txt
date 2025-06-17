Modern Combat 4: Zero Hour Windows Phone 8 .XAP Port To Windows Desktop ReadMe
------------------------------------------------------------------------------

In the .cs or .pbd files, namely the .cs, you will find functions the game calls to initialize 'x' or 'y'.
I've already narrowed the functions we need for direct3d rendering of the game: They are found in: MC4HybridComponent.dll,
MC4Component.winmd, and MC4Interop.dll.

For instance see what insight can already be seen in MC4Interop.dll's extracted source files extracted 'App'
and 'MainPage' classes, which I put in a .cs files on the directory root along with this readme.


Even this line alone provides much insight into the Direct3D pipeline:

private Direct3DBackground m_d3dBackground = Direct3DBackground.GetInstance();

Direct3DBackground, which you will come to find exists in the MC4Component namespace, initialized through GetInstance();

internal DrawingSurfaceBackgroundGrid DrawingSurfaceBackground;

This is what is used to render on screen.

-------------------------------------------------------------------

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

WindowsRuntimeMarshal.AddEventHandler<LaunchGLLiveHandler>(new Func<LaunchGLLiveHandler, 
EventRegistrationToken>(null, Direct3DBackground.add_LaunchGLLiveEvent), 
new Action<EventRegistrationToken>(Direct3DBackground.remove_LaunchGLLiveEvent), 
new LaunchGLLiveHandler(this.LaunchGLLive));

I definitely need some help if for this to be done in a timely manner, and my novice experience with C++ is a bottleneck,
so contributors are welcome! Encouraged actually!
You will need the four .dat files as they were too large to upload, find them in the XAP:
https://www.mediafire.com/file/n3e6k1l3ri3632i/Modern_Combat_4_Para_512.xap/file

Visual Studio 2012 for WinRT access to the .dlls!


