using System;
using System.Windows.Input;
using AllAboutMovie.Common;
using AllAboutMovie.Core.UI;
using AllAboutMovie.Utils;
using AllAboutMovie.ViewModels;

namespace AllAboutMovie.Controllers
{
	public class SearchTitleController : PassiveControllerBase<SearchTitleViewModel>
	{
		public SearchTitleController(SearchTitleViewModel viewData) :  base(viewData)
		{
		}
		
		#region Translit
		private void Translit()
		{
			ViewData.Text = TranslitHelper.EngToRus(ViewData.Text);
		}

		private ICommand _translitCommand;
		public ICommand TranslitCommand
		{
			get
			{
				return _translitCommand ??
				       (_translitCommand = new DelegateCommand(
				                           	Translit,
				                           	() => !String.IsNullOrEmpty(ViewData.Text)));
			}
		}
		#endregion // Translit
	}
}