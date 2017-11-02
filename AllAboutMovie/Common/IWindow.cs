using System;
using System.ComponentModel;
using System.Windows.Input;
using AllAboutMovie.Core.UI;

namespace AllAboutMovie.Common
{
	public interface IWindow : IView
	{
		void Show();
		void Close();

		event EventHandler Closed;
		event CancelEventHandler Closing;
	}
}