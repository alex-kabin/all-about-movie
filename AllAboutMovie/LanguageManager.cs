using System;
using System.Globalization;
using System.Threading;
using AllAboutMovie.Common;
using AllAboutMovie.Core;
using AllAboutMovie.Properties;

namespace AllAboutMovie
{
	public static class LanguageManager
	{
		public static CultureInfo[] GetLocalizationCultures()
		{
			return new [] { CultureInfo.InstalledUICulture, CultureInfo.GetCultureInfo("ru-RU") };
		}

		public static string CurrentCultureName
		{
			get { return Thread.CurrentThread.CurrentUICulture.Name; }
		}

		public static void ApplyLanguage(string cultureName)
		{
			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(cultureName);

			var uiMessageService = Container.Global.Resolve<IUIMessageService>();
			if (uiMessageService != null)
				uiMessageService.ShowInfo(Resources.LanguageSwitchMessage);

			var settingsService = Container.Global.Resolve<ISettingsService>();
			if (settingsService != null)
				settingsService["Culture"] = cultureName;
		}

		public static void ApplyLanaguage()
		{
			var settingsService = Container.Global.Resolve<ISettingsService>();
			if (settingsService != null)
			{
				var cultureName = settingsService["Culture"];
				if (!String.IsNullOrWhiteSpace(cultureName))
					Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(cultureName);
			}
		}
	}
}