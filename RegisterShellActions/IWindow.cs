using System;
using System.ComponentModel;

namespace RegisterShellActions
{
	public interface IWindow
	{
		object DataContext { get; set; }
		void Show();

		event EventHandler Closed;
		event CancelEventHandler Closing;
	}
}