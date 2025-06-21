extern alias MC4SystemWindows;
using Windows.Phone.Input.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using MC4SystemWindows;
using MC4Interop;
using IGPBridgeLibrary;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows; // For RenderForm or similar
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Toolkit.Graphics;
using SharpDX.DirectSound;
using SharpDX.IO;
using SharpDX.Toolkit;
using SharpDX.Direct3D9;
using MC4Component;
using Microsoft.Xna.Framework;




namespace ModernCombat4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    
    /// </summary>
    public partial class MainWindow
    {

        private MC4Component.Direct3DBackground direct3dBackground;

        public MainWindow()
        {
            InitializeComponent();

            // Create an instance of Direct3DBackground
            direct3dBackground = Direct3DBackground.i();


            // Other initialization code...
        }
    }
        
                     

     

    // abstract class D3DImage(double 1280, double 768);
    // {
         
          //   public static readonly System.Windows.DependencyProperty IsFrontBufferAvailableProperty;
         //   GetDeviceNameHandler IDrawingSurfaceManipulationHandler.D3DImage.LOGIN_SUCCESS SetManipulationHost;
             

        
    public partial class direct3dinterface 
    {
      //  _d3dImage = D3DViewPort; subject to change !!
      //  SetBackBuffer
       MainWindow InitializeComponent; D3DResourceType Direct3DBackground;
     
       D3DResourceType MainWindow;
        D3DImage BeginScene;

       public void Clear(SharpDX.Color White)
    {


       

        SharpDX.Direct3D11.RenderTargetView D3DViewPort;

        SharpDX.Direct3D11.Device device;
          
        SwapChain swapChain;
    }
       // SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType Hardware, DeviceCreationFlags, desc, D3DImage, out SwapChain);
      //  var context = device.ImmediateContext;
        }
    
}


       
   




    
      // System.Windows.Interop.Get
        //  D3DImage.InitializeComponent();
       // public object desktopHost { get; set; }
      //  public object IManipulationHost { get; set; }
        // Assuming D3DBackground has a desktop equivalent that accepts a manipulation handler
        public class DesktopD3DBackground
        {
            public void SetManipulationHost()
            {
                
                // Internal logic to associate the handler with the drawing surface
            }
        }

        public class _viewportminiManipulationRender  // Your custom interface
        {
            // Implement manipulation handling logic using desktop UI events
            // For example, in WPF:
            // private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
            // {
            

            //    // Handle manipulation updates
            // }
  }
    
    
