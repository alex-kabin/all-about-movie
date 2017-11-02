using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegisterShellActions
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private AppViewModel ViewModel
		{
			get { return this.DataContext as AppViewModel; }
			set { this.DataContext = value; }
		}

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			bool removeAll = Environment.GetCommandLineArgs()
				.Contains("/removeall", StringComparer.InvariantCultureIgnoreCase);

			var viewModel = PrepareViewModel();
			if (removeAll)
			{
				viewModel.Extensions.SelectMany(ext => ext.Actions).ToList().ForEach(a => a.Selected = false);
			}
			ViewModel = viewModel;
			if (removeAll)
			{
				ApplyChanges();
				Close();
			}
		}

		private AppViewModel PrepareViewModel()
		{
			return new AppViewModel()
					{
						Extensions =
							Configuration.Current.Extensions
							.Cast<ExtensionConfigurationElement>()
							.Select(e => new ExtensionViewModel()
											{
												Key = e.Name,
												Actions = (from an in e.Actions
														   let a = Configuration.Current.Actions[an]
														   where a != null
														   let isreg = ShellActionsHelper.IsRegistered(e.Name, an)
														   select new ActionViewModel()
																{
																	Key = an,
																	Text = a.Text,
																	Selected = isreg,
																	Registered = isreg
																}).ToList()
											}).ToList()


					};
		}


		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			if(ApplyChanges())
				Close();
		}
		
		private bool ApplyChanges()
		{
			var registerPairs = (from ext in ViewModel.Extensions
			                     from act in ext.Actions
			                     where act.Selected && !act.Registered
			                     select Tuple.Create(ext.Key, act.Key)).ToArray();

			var unregisterPairs = (from ext in ViewModel.Extensions
			                       from act in ext.Actions
			                       where !act.Selected && act.Registered
			                       select Tuple.Create(ext.Key, act.Key)).ToArray();

			try
			{
				ShellActionsHelper.Register(registerPairs);
				ShellActionsHelper.Unregister(unregisterPairs);	
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
			return true;
		}

		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
