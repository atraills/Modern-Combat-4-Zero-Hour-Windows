extern alias MC4SystemWindows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using MC4SystemWindows;
using System.Windows.Interop;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows; // For RenderForm or similar
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Toolkit.Graphics;
using SharpDX.DirectSound;
using SharpDX.IO;
using SharpDX.Toolkit;

namespace ModernCombat4
{  


    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class mc4swapchainWeaver
    {
        SwapChainDescription swapChainDescription = new SwapChainDescription();
    }
        // Buffer = 1, // Or 2 for double buffering
       //  ModeDescription = new ModeDescription(
        // SurfaceDescription.1280.768, // Or use a control's size
        // form.ClientSize.Height,
       //  new Rational( 60 HZ ), // Refresh rate (e.g., 60 FPS)
       //  Format.R8G8B8A8_UNorm, // Pixel format
       //  IsWindowed = true,
       //  OutputHandle = control.Handle, // Or use a control's Handle
        // SampleDescription = new SampleDescription(1, 0),
       //  SwapEffect = SwapEffect.Discard, // Or SwapEffect.FlipSequential  
       //  Usage = Usage.RenderTargetOutput,
    
         
// Create the device and swap chain
  //  SharpDX.Direct3D11.Device.CreateWithSwapChain(
   //  DriverType Hardware,
   //  DeviceCreationFlags // Optional flags
   //    SharpDX FeatureLevel,  // Supported feature levels
   //   swapChainDescription,
   //   SharpDX.Direct3D11.Device device,
    //  SharpDX.DXGI.SwapChain swapChain,

    
// Get the back buffer and render target view
   // Texture2D backBuffer = SharpDX.DXGI.Resource.FromSwapChain<Texture2D>(swapChain, 0);
  //  RenderTargetView renderTargetView = new RenderTargetView(device, backBuffer);

// ... (setup depth buffer, shaders, etc.)

// In your rendering loop:

// Clear the render target
  //  device.ImmediateContext.ClearRenderTargetView(renderTargetView, new SharpDX.Color4(0.0f, 0.0f, 0.0f, 1.0f));
}
 

   


