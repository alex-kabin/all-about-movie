using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows;
using System.Linq;
using AllAboutMovie.Common;
using AllAboutMovie.Core;
using AllAboutMovie.Properties;
using AllAboutMovie.ViewModels;

namespace AllAboutMovie.Controllers
{
	public class LocalizationController : PassiveControllerBase<LocalizationViewModel>
	{
		public LocalizationController(LocalizationViewModel viewData)
			: base(viewData)
		{
			var cultures = LanguageManager.GetLocalizationCultures();
			viewData.Languages = (from c in cultures
			                      select new LanguageViewModel()
			                             	{
			                             		Name = c.NativeName,
			                             		CultureName = c.Name,
			                             		CultureFlagUrl = String.Format("/Resources/{0}.png", c.ThreeLetterWindowsLanguageName)
			                             	}).ToList();

			viewData.SelectedCultureName = LanguageManager.CurrentCultureName;
			viewData.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(viewData_PropertyChanged);
		}

		private void viewData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (ViewData.SelectedCultureName != LanguageManager.CurrentCultureName)
			{
				LanguageManager.ApplyLanguage(ViewData.SelectedCultureName);
			}
		}

		
	}
}