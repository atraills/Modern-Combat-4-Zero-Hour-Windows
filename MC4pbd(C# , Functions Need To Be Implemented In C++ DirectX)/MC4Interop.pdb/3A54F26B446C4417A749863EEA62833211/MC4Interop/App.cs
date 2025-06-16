using MC4Component;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Marketplace;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using Push_WP;
using ScheduledAgent;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Navigation;

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