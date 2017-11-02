using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AllAboutMovie.Common;

namespace AllAboutMovie
{
	[Export("RootWindow", typeof(IWindow))]
	[Export("RootNavigator", typeof(INavigationService))]
	[Export(typeof(IDialogService))]
	[Export(typeof(IUIMessageService))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public partial class MainWindow : IWindow, INavigationService, IDialogService, IUIMessageService
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		public void GoTo(Uri uri)
		{
			this.Navigate(uri);
		}

		bool INavigationService.CanGoBack
		{
			get { return this.NavigationService.CanGoBack; }
		}

		void INavigationService.GoBack()
		{
			this.NavigationService.GoBack();
		}

		bool INavigationService.CanGoForward
		{
			get { return this.NavigationService.CanGoForward; }
		}

		void INavigationService.GoForward()
		{
			this.NavigationService.GoForward();
		}

		public Uri Current
		{
			get
			{
				return NavigationService.Source;
			}
		}

		public void Show(IWindow window, object dataContext, Action callback)
		{
			Window childWindow = window as Window;
			if (childWindow != null)
			{
				childWindow.Owner = this;
				childWindow.Closed += delegate { callback(); };
				childWindow.Show();
			}
		}

		public bool? ShowModal(IWindow window, object dataContext)
		{
			Window childWindow = window as Window;
			if (childWindow != null)
			{
				childWindow.Owner = this;
				childWindow.DataContext = dataContext;
				return childWindow.ShowDialog();
			}

			return null;
		}

		public void ShowError(string text)
		{
			MessageBox.Show(this, text, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
		}

		public void ShowWarning(string text)
		{
			MessageBox.Show(this, text, Properties.Resources.WarningTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		public void ShowInfo(string text)
		{
			MessageBox.Show(this, text, Properties.Resources.InfoTitle, MessageBoxButton.OK, MessageBoxImage.Information);
		}

		public bool? ShowQuestion(string text, MessageBoxButton buttons)
		{
			var result = MessageBox.Show(this, text, Properties.Resources.QuestionTitle, buttons, MessageBoxImage.Question);
			if (result == MessageBoxResult.OK || result == MessageBoxResult.Yes)
				return true;
			if (result == MessageBoxResult.Cancel || result == MessageBoxResult.No)
				return false;

			return null;
		}
	}
}
