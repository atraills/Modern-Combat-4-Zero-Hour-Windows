using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace MC4Interop
{
	public class ServiceUnavailable : UserControl
	{
		private delegateExitGLLiveFromError exitGLLiveFunc;

		internal Grid LayoutRoot;

		internal Grid ContentPanel;

		internal TextBlock txtMessage;

		internal Button btnOk;

		private bool _contentLoaded;

		public ServiceUnavailable()
		{
			this.InitializeComponent();
			string text = this.txtMessage.get_Text();
			this.txtMessage.set_Text(text.Replace("[SERVICE_NAME]", "Gameloft Live!"));
			this.btnOk.set_Content((MessageBoxButton)0);
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.exitGLLiveFunc();
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Application.LoadComponent(this, new Uri("/MC4Interop;component/ServiceUnavailable.xaml", UriKind.Relative));
			this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
			this.ContentPanel = (Grid)base.FindName("ContentPanel");
			this.txtMessage = (TextBlock)base.FindName("txtMessage");
			this.btnOk = (Button)base.FindName("btnOk");
		}

		public void SetExitFunc(delegateExitGLLiveFromError function)
		{
			this.exitGLLiveFunc = function;
		}
	}
}