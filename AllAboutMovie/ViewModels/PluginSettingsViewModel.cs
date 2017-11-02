using System;
using System.Windows.Controls;
using System.Windows.Input;
using AllAboutMovie.Core.UI;

namespace AllAboutMovie.ViewModels
{
	public class PluginSettingsViewModel
	{
		public string Title { get; set; }
		public IView SettingsView { get; set; }
		public ICommand ApplyCommand { get; set; }
	}
}