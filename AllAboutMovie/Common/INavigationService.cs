using System;

namespace AllAboutMovie.Common
{
	public interface INavigationService
	{
		void GoTo(Uri uri);

		bool CanGoBack { get; }
		void GoBack();

		bool CanGoForward { get; }
		void GoForward();

		Uri Current { get; }
	}
}