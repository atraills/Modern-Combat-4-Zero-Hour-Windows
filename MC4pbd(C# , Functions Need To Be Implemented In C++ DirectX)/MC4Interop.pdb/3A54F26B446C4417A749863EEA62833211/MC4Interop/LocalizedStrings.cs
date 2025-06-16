using MC4Interop.Resources;
using System;

namespace MC4Interop
{
	public class LocalizedStrings
	{
		private static AppResources _localizedResources;

		public AppResources LocalizedResources
		{
			get
			{
				return LocalizedStrings._localizedResources;
			}
		}

		static LocalizedStrings()
		{
			LocalizedStrings._localizedResources = new AppResources();
		}

		public LocalizedStrings()
		{
		}
	}
}