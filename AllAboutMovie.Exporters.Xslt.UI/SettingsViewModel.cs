using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using AllAboutMovie.Exporters.Xslt.UI.Properties;

namespace AllAboutMovie.Exporters.Xslt.UI
{
	public class SettingsViewModel : IDataErrorInfo
	{
		public IEnumerable<string> Stylesheets { get; private set; }

		public string SelectedStylesheet { get; set; }

		public SettingsViewModel(IEnumerable<string> stylesheets)
		{
			Stylesheets = stylesheets;
			SelectedStylesheet = stylesheets.FirstOrDefault();
		}

		public string this[string columnName]
		{
			get { throw new NotImplementedException(); }
		}

		public string Error
		{
			get
			{
				if (String.IsNullOrEmpty(SelectedStylesheet))
					return Resources.StylesheetNotSelected;
				
				return String.Empty;
			}
		}
	}
}