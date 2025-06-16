using System;

namespace ScheduledAgent
{
	public class LiveTileHanlder
	{
		public LiveTileHanlder()
		{
		}

		public static int UpdateFlipTemplate()
		{
			return (new FlipTemplateLog()).Update();
		}
	}
}