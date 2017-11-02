using AllAboutMovie.Common;

namespace AllAboutMovie.ViewModels
{
	public class SearchTitleViewModel : ViewModel
	{
		private string _text;
		public string Text
		{
			get { return _text; }
			set
			{
				if (_text != value)
				{
					_text = value;
					RaisePropertyChanged(() => Text);
					RaisePropertyChanged(() => IsEmpty);
				}
			}
		}

		public bool IsEmpty
		{
			get { return string.IsNullOrEmpty(_text); }
		}
	}
}