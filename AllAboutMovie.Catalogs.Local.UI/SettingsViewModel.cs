using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AllAboutMovie.Catalogs.Local.UI
{
	public class SettingsViewModel : IDataErrorInfo
	{
		public string ExtensionsString { get; set; }
		public string FoldersString { get; set; }

		public string this[string columnName]
		{
			get { throw new NotImplementedException(); }
		}

		public string Error
		{
			get
			{
				if (String.IsNullOrEmpty(FoldersString))
					return "Please, specify folders to search";

				return String.Empty;
			}
		}
	}
}