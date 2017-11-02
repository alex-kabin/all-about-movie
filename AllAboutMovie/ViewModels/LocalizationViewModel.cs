using System.Collections.Generic;
using AllAboutMovie.Common;

namespace AllAboutMovie.ViewModels
{
	public class LocalizationViewModel : ViewModel
	{
		public IEnumerable<LanguageViewModel> Languages { get; set; }

		private string _selectedCultureName;
		public string SelectedCultureName
		{
			get { return _selectedCultureName; }
			set { _selectedCultureName = value; RaisePropertyChanged(() => SelectedCultureName); }
		}
	}

	
}