using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace MC4Interop.Resources
{
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	public class AppResources
	{
		private static System.Resources.ResourceManager resourceMan;

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
		public static System.Resources.ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(AppResources.resourceMan, null))
				{
					AppResources.resourceMan = new System.Resources.ResourceManager("MC4Interop.Resources.AppResources", typeof(AppResources).Assembly);
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