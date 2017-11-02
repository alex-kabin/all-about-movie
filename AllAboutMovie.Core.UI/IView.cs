using System.Windows.Input;

namespace AllAboutMovie.Core.UI
{
	public interface IView
	{
		object DataContext { get; set; }

		Cursor Cursor { get; set; }
	}
}