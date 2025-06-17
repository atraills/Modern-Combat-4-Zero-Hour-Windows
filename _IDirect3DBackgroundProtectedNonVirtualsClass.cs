using System;
using Windows.Foundation.Metadata;
using Windows.Phone.Input.Interop;
using Windows.UI.Core;

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