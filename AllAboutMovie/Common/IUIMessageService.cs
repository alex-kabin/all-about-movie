using System.Windows;

namespace AllAboutMovie.Common
{
	public interface IUIMessageService
	{
		void ShowError(string text);
		void ShowWarning(string text);
		void ShowInfo(string text);
		bool? ShowQuestion(string text, MessageBoxButton buttons);
	}
}