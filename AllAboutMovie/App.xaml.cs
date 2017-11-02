using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using AllAboutMovie.Common;
using AllAboutMovie.Core;
using AllAboutMovie.Properties;

namespace AllAboutMovie
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		[Import("RootController")]
		private IActiveController AppController { get; set; }

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			LanguageManager.ApplyLanaguage();
			Container.Global.Inject(this);	
			AppController.Run();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);
			Container.Global.Dispose();
		}
	}
}
