// MISC HEADER
#include "stdafx.h"
#include "resource.h"
#include <roapi.h>
#include <wrl/client.h>
#include <windows.h> 
#include <inspectable.h>
#include <wrl/wrappers/corewrappers.h>
#include <iostream>
#include <d3d11.h>
#include <d3d11_1.h>
#include <dxgi.h>
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