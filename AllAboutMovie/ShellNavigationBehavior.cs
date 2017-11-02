using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace AllAboutMovie
{
	public static class ShellNavigationBehavior
	{
		public static bool GetEnabled(DependencyObject obj)
		{
			return (bool)obj.GetValue(EnabledProperty);
		}

		public static void SetEnabled(DependencyObject obj, bool value)
		{
			obj.SetValue(EnabledProperty, value);
		}

		public static readonly DependencyProperty EnabledProperty =
			DependencyProperty.RegisterAttached("Enabled", typeof(bool), typeof(ShellNavigationBehavior),
			new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, OnEnabledPropertyChanged));

		private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var hyperlink = d as Hyperlink;
			if(hyperlink == null)
				return;

			bool oldValue = (bool)e.OldValue;
			bool newValue = (bool)e.NewValue;

			if(oldValue == false && newValue == true)
			{
				hyperlink.RequestNavigate += hyperlink_RequestNavigate;
			}
			else if(oldValue == true && newValue == false)
			{
				hyperlink.RequestNavigate -= hyperlink_RequestNavigate;
			}
		}

		private static void hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			var fileName = e.Uri.AbsoluteUri;
			if (e.Uri.IsFile)
				fileName = e.Uri.LocalPath;

			Process.Start(new ProcessStartInfo(fileName));
			e.Handled = true;
		}



	}
}