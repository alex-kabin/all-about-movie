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
using AllAboutMovie.ViewModels;

namespace AllAboutMovie
{
	[Export("PluginSettingsDialog", typeof(IWindow))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class PluginSettingsDialog : IWindow
	{
		public PluginSettingsDialog()
		{
			InitializeComponent();
		}

		private PluginSettingsViewModel ViewModel
		{
			get { return this.DataContext as PluginSettingsViewModel; }
		}
		
		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (ViewModel != null)
				{
					ViewModel.ApplyCommand.Execute(ViewModel.SettingsView.DataContext);
					DialogResult = true;
					Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, Properties.Resources.InfoTitle, MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}
	}
}
