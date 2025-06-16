using Microsoft.Phone;
using Microsoft.Phone.Scheduler;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace ScheduledAgent
{
	public class ScheduledAgent : ScheduledTaskAgent
	{
		static ScheduledAgent()
		{
			Deployment.get_Current().get_Dispatcher().BeginInvoke(() => Application.get_Current().add_UnhandledException(new EventHandler<ApplicationUnhandledExceptionEventArgs>(ScheduledAgent.UnhandledException)));
		}

		public ScheduledAgent()
		{
		}

		protected override void OnInvoke(ScheduledTask task)
		{
			LiveTileHanlder.UpdateFlipTemplate();
			base.NotifyComplete();
		}

		private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			if (Debugger.IsAttached)
			{
				Debugger.Break();
			}
		}
	}
}