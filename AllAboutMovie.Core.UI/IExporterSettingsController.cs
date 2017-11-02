using System.Windows.Controls;
using System.Windows.Input;

namespace AllAboutMovie.Core.UI
{
	public interface IExporterSettingsController
	{
		IMovieExporter Exporter { get; set; }
		IView CreateSettingsView();
		ICommand ApplyCommand { get; }
	}
}