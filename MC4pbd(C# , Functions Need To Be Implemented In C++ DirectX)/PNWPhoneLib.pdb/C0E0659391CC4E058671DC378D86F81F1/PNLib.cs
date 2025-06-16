using Microsoft.Phone.Notification;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using PushNotificationRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using Windows.System;

namespace Push_WP
{
	public sealed class PNLib
	{
		private static HttpNotificationChannel s_PushChannel;

		private static string s_URI;

		private static string s_AuthentificatedServerName;

		private static CallbackPacker s_CallbackPacker;

		private static EventWaitHandle s_waitHandle;

		public static bool resumed;

		static PNLib()
		{
			PNLib.resumed = false;
			PNLib.s_waitHandle = new ManualResetEvent(true);
			PNLib.s_URI = "";
			PNLib.s_AuthentificatedServerName = "*.gameloft.com";
		}

		public PNLib()
		{
		}

		public static void CancelAllLocalToasts()
		{
			try
			{
				List<string> strs = new List<string>();
				foreach (Reminder action in ScheduledActionService.GetActions<Reminder>())
				{
					strs.Add(action.get_Name());
				}
				foreach (string str in strs)
				{
					ScheduledActionService.Remove(str);
				}
			}
			catch (Exception exception)
			{
			}
		}

		private static void CleanUp()
		{
			if (PNLib.s_PushChannel != null)
			{
				PNLib.s_PushChannel.Close();
			}
		}

		private static string GetURI()
		{
			TimeSpan timeSpan = new TimeSpan(0, 0, 15);
			PNLib.s_waitHandle.WaitOne(timeSpan);
			return PNLib.s_URI;
		}

		private static int Init()
		{
			PNLib.QueryURI();
			return 0;
		}

		public static async void OpenLink(string link)
		{
			if (link.Length > 0)
			{
				try
				{
					Uri uri = new Uri(link, UriKind.Absolute);
					await Launcher.LaunchUriAsync(uri);
				}
				catch
				{
				}
			}
		}

		private static string ParseKeyString(string ParamString, string key)
		{
			string empty = string.Empty;
			int num = ParamString.IndexOf(key);
			if (num > -1)
			{
				int num1 = ParamString.IndexOf("&", num);
				empty = (num1 <= -1 ? ParamString.Substring(num + key.Length) : ParamString.Substring(num + key.Length, num1 - num - key.Length));
				empty = empty.Trim();
			}
			return empty;
		}

		public static bool ParseNotification(NavigationContext navigation_context)
		{
			bool flag;
			try
			{
				string item = navigation_context.get_QueryString()["type"];
				item = item.Trim();
				if (item == "message")
				{
					if (!PNLib.resumed)
					{
						Application.get_Current().Terminate();
					}
				}
				else if (item == "url")
				{
					string str = navigation_context.get_QueryString()["link"];
					str = str.Trim();
					PNLib.PopUp_Notification("!!!", string.Concat("Do you want to open link ", str, " ?"), str, true);
				}
				else if (item == "launch")
				{
					flag = true;
					return flag;
				}
				flag = false;
			}
			catch
			{
				bool flag1 = PNLib.resumed;
				flag = false;
			}
			return flag;
		}

		private static void PopUp_Notification(string body, string title, string link, bool isDialog)
		{
			Deployment.get_Current().get_Dispatcher().BeginInvoke(() => {
				if (!isDialog)
				{
					MessageBox.Show(body, title, 0);
					return;
				}
				if (MessageBox.Show(body, title, 1) == 1)
				{
					PNLib.OpenLink(link);
				}
			});
		}

		private static void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
		{
			PNLib.s_URI = e.get_ChannelUri().ToString();
			PNLib.s_waitHandle.Set();
		}

		private static void PushChannel_ConnectionStatusChanged(object sender, NotificationChannelConnectionEventArgs e)
		{
		}

		private static void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
		{
			PNLib.s_waitHandle.Set();
		}

		private static void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
		{
			string str;
			PNContext pNContext = new PNContext();
			e.get_Collection().TryGetValue("wp:Text1", out pNContext.title);
			e.get_Collection().TryGetValue("wp:Text2", out pNContext.body);
			if (e.get_Collection().ContainsKey("wp:Param"))
			{
				e.get_Collection().TryGetValue("wp:Param", out str);
				pNContext.launch = str.Trim();
				string str1 = PNLib.SearchForType(str).Trim();
				string str2 = str1;
				if (str1 != null)
				{
					if (str2 == "message")
					{
						pNContext.type = PNType.message;
						PNLib.s_CallbackPacker.context = pNContext;
						PNLib.s_CallbackPacker.CallNativeCallback();
						PNLib.PopUp_Notification(pNContext.body, pNContext.title, "", false);
						return;
					}
					if (str2 != "url")
					{
						goto Label2;
					}
					pNContext.type = PNType.url;
					string str3 = PNLib.SearchForURL(str);
					str3 = str3.Trim();
					if (PNLib.s_CallbackPacker != null)
					{
						PNLib.s_CallbackPacker.context = pNContext;
						bool flag = PNLib.s_CallbackPacker.CallNativeCallback();
						if (str3.Length > 0 && flag)
						{
							PNLib.PopUp_Notification(pNContext.body, pNContext.title, str3, true);
							return;
						}
						else
						{
							return;
						}
					}
					else
					{
						return;
					}
				}
			Label2:
				pNContext.type = PNType.launch;
				PNLib.PopUp_Notification(pNContext.body, pNContext.title, "", false);
			}
		}

		public static void PushLocalToast(string Title, string Content)
		{
			ShellToast shellToast = new ShellToast();
			shellToast.set_Content(Content);
			shellToast.set_Title(Title);
			shellToast.Show();
		}

		public static void PushLocalToast(string Title, string Content, int seconds)
		{
			try
			{
				DateTime dateTime = DateTime.Now.AddSeconds((double)seconds);
				DateTime universalTime = DateTime.Now.ToUniversalTime();
				Guid guid = Guid.NewGuid();
				string str = universalTime.Ticks.ToString();
				int millisecond = universalTime.Millisecond;
				Reminder reminder = new Reminder(string.Concat(str, "-", millisecond.ToString(), guid.ToString()));
				reminder.set_Title(Title);
				reminder.set_Content(Content);
				reminder.set_RecurrenceType(0);
				reminder.set_BeginTime(dateTime.AddSeconds(30));
				ScheduledActionService.Add(reminder);
			}
			catch (Exception exception)
			{
			}
		}

		public static void PushLocalToast(string Title, string Content, DateTime Pn_time)
		{
			DateTime now = DateTime.Now;
			if (now.CompareTo(Pn_time) < 0)
			{
				TimeSpan timeSpan = Pn_time.Subtract(now);
				PNLib.PushLocalToast(Title, Content, (int)timeSpan.TotalSeconds);
			}
		}

		public static void PushLocalToast(string Title, string Content, int day, int month, int year, int hour, int minute, int second)
		{
			DateTime now = DateTime.Now;
			DateTime dateTime = new DateTime(year, month, day, hour, minute, second);
			if (now.CompareTo(dateTime) < 0)
			{
				TimeSpan timeSpan = dateTime.Subtract(now);
				PNLib.PushLocalToast(Title, Content, (int)timeSpan.TotalSeconds);
			}
		}

		private static void QueryURI()
		{
			try
			{
				PNLib.s_PushChannel = HttpNotificationChannel.Find("PNLib_MC4");
				if (PNLib.s_PushChannel != null)
				{
					PNLib.s_PushChannel.add_ChannelUriUpdated(new EventHandler<NotificationChannelUriEventArgs>(PNLib.PushChannel_ChannelUriUpdated));
					PNLib.s_PushChannel.add_ErrorOccurred(new EventHandler<NotificationChannelErrorEventArgs>(PNLib.PushChannel_ErrorOccurred));
					PNLib.s_PushChannel.add_ShellToastNotificationReceived(new EventHandler<NotificationEventArgs>(PNLib.PushChannel_ShellToastNotificationReceived));
					if (PNLib.s_PushChannel.get_ChannelUri() != null)
					{
						PNLib.s_URI = PNLib.s_PushChannel.get_ChannelUri().ToString();
					}
					PNLib.s_waitHandle.Set();
				}
				else
				{
					PNLib.s_waitHandle.Reset();
					PNLib.s_PushChannel = new HttpNotificationChannel("PNLib_MC4", PNLib.s_AuthentificatedServerName);
					PNLib.s_PushChannel.add_ChannelUriUpdated(new EventHandler<NotificationChannelUriEventArgs>(PNLib.PushChannel_ChannelUriUpdated));
					PNLib.s_PushChannel.add_ErrorOccurred(new EventHandler<NotificationChannelErrorEventArgs>(PNLib.PushChannel_ErrorOccurred));
					PNLib.s_PushChannel.add_ShellToastNotificationReceived(new EventHandler<NotificationEventArgs>(PNLib.PushChannel_ShellToastNotificationReceived));
					PNLib.s_PushChannel.Open();
					PNLib.s_PushChannel.BindToShellTile();
					PNLib.s_PushChannel.BindToShellToast();
				}
			}
			catch (Exception exception)
			{
				string message = exception.Message;
			}
		}

		public static void Register()
		{
			WindowsRuntimeMarshal.AddEventHandler<Delegate_GetURI>(new Func<Delegate_GetURI, EventRegistrationToken>(null, CPushNotificationRuntime.add_s_RegistrationEven_GetURI), new Action<EventRegistrationToken>(CPushNotificationRuntime.remove_s_RegistrationEven_GetURI), new Delegate_GetURI(PNLib.GetURI));
			WindowsRuntimeMarshal.AddEventHandler<Delegate_Init>(new Func<Delegate_Init, EventRegistrationToken>(null, CPushNotificationRuntime.add_s_RegistrationEven_Init), new Action<EventRegistrationToken>(CPushNotificationRuntime.remove_s_RegistrationEven_Init), new Delegate_Init(PNLib.Init));
			WindowsRuntimeMarshal.AddEventHandler<Delegate_ShowNotification>(new Func<Delegate_ShowNotification, EventRegistrationToken>(null, CPushNotificationRuntime.add_s_RegistrationEven_ShowNotification), new Action<EventRegistrationToken>(CPushNotificationRuntime.remove_s_RegistrationEven_ShowNotification), new Delegate_ShowNotification(PNLib.PushLocalToast));
			WindowsRuntimeMarshal.AddEventHandler<Delegate_PushNotification_delayed>(new Func<Delegate_PushNotification_delayed, EventRegistrationToken>(null, CPushNotificationRuntime.add_s_RegistrationEven_PushNotification_delayed), new Action<EventRegistrationToken>(CPushNotificationRuntime.remove_s_RegistrationEven_PushNotification_delayed), new Delegate_PushNotification_delayed(PNLib.PushLocalToast));
			WindowsRuntimeMarshal.AddEventHandler<Delegate_PushNotification_to_date>(new Func<Delegate_PushNotification_to_date, EventRegistrationToken>(null, CPushNotificationRuntime.add_s_RegistrationEven_PushNotification_to_date), new Action<EventRegistrationToken>(CPushNotificationRuntime.remove_s_RegistrationEven_PushNotification_to_date), new Delegate_PushNotification_to_date(PNLib.PushLocalToast));
			WindowsRuntimeMarshal.AddEventHandler<Delegate_SetCallbackPacker>(new Func<Delegate_SetCallbackPacker, EventRegistrationToken>(null, CPushNotificationRuntime.add_s_RegistrationEven_SetCallbackPacker), new Action<EventRegistrationToken>(CPushNotificationRuntime.remove_s_RegistrationEven_SetCallbackPacker), new Delegate_SetCallbackPacker(PNLib.SetCBPacker));
			WindowsRuntimeMarshal.AddEventHandler<Delegate_CleanUp>(new Func<Delegate_CleanUp, EventRegistrationToken>(null, CPushNotificationRuntime.add_s_RegistrationEvent_CleanUp), new Action<EventRegistrationToken>(CPushNotificationRuntime.remove_s_RegistrationEvent_CleanUp), new Delegate_CleanUp(PNLib.CleanUp));
			WindowsRuntimeMarshal.AddEventHandler<Delegate_CancelAllLocalToasts>(new Func<Delegate_CancelAllLocalToasts, EventRegistrationToken>(null, CPushNotificationRuntime.add_s_RegistrationEvent_CancelAllLocalToasts), new Action<EventRegistrationToken>(CPushNotificationRuntime.remove_s_RegistrationEvent_CancelAllLocalToasts), new Delegate_CancelAllLocalToasts(PNLib.CancelAllLocalToasts));
		}

		private static string SearchForBody(string ParamString)
		{
			return PNLib.ParseKeyString(ParamString, "body=");
		}

		private static string SearchForSubject(string ParamString)
		{
			return PNLib.ParseKeyString(ParamString, "subject=");
		}

		private static string SearchForType(string ParamString)
		{
			return PNLib.ParseKeyString(ParamString, "type=");
		}

		private static string SearchForURL(string ParamString)
		{
			return PNLib.ParseKeyString(ParamString, "link=");
		}

		private static void SetCBPacker(CallbackPacker packer)
		{
			PNLib.s_CallbackPacker = packer;
		}
	}
}