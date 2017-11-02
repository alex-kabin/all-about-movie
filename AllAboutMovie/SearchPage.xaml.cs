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
using AllAboutMovie.Common;
using AllAboutMovie.Controllers;

namespace AllAboutMovie
{
	/// <summary>
	/// Interaction logic for SearchPage.xaml
	/// </summary>
	public partial class SearchPage : Page
	{
		public SearchPage()
		{
			InitializeComponent();
		}

		private void GoToDetails(object sender, EventArgs e)
		{
			var viewModel = this.DataContext as ViewModel;
			if(viewModel != null)
			{
				var controller = viewModel.Controller as AppController;
				if(controller != null && controller.GoToDetailsCommand.CanExecute(null))
					controller.GoToDetailsCommand.Execute(null);
			}
		}
	}
}
