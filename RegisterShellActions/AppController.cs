using System.Windows;

namespace RegisterShellActions
{
	public class AppController : IController
	{
		private AppViewModel ViewModel { get; set; }

		public AppController(IWindow mainWindow)
		{
			ViewModel = new AppViewModel() { Controller = this };
			mainWindow.DataContext = ViewModel;
			mainWindow.Show();
		}
	}
}