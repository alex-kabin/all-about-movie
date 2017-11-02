using System.Windows.Input;

namespace AllAboutMovie.Core.UI
{
	public interface IPluginSettingsManager
	{
		IPlugin Plugin { get; set; }
		IView CreateSettingsView();
		ICommand ApplyCommand { get; }
	}
}