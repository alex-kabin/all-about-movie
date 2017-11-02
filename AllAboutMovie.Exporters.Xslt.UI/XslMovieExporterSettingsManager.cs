using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using AllAboutMovie.Core;
using AllAboutMovie.Core.UI;

namespace AllAboutMovie.Exporters.Xslt.UI
{
	[Export(typeof(IPluginSettingsManager))]
	[PluginSettingsManagerMetadata(PluginType = typeof(XslMovieExporter))]
	public class XslMovieExporterSettingsManager : PluginSettingsManagerBase<XslMovieExporter>
	{
		[Import]
		private ISettingsService Settings { get; set; }

		public override IView CreateSettingsView()
		{
			var viewModel = new SettingsViewModel(ConcretePlugin.GetStylesheets());
			
			string selectedStylesheet = ConcretePlugin.Stylesheet;
			if (selectedStylesheet == null && Settings != null)
				selectedStylesheet = Settings["Xslt_Stylesheet"];
			if (!String.IsNullOrEmpty(selectedStylesheet))
				viewModel.SelectedStylesheet = selectedStylesheet;

			return new SettingsControl() { DataContext = viewModel };
		}

		protected override void Apply(object data)
		{
			var viewModel = data as SettingsViewModel;
			if(viewModel != null)
			{
				var viewModelError = viewModel.Error;
				if (String.IsNullOrEmpty(viewModelError))
				{
					ConcretePlugin.Stylesheet = viewModel.SelectedStylesheet;
					if (Settings != null)
						Settings["Xslt_Stylesheet"] = viewModel.SelectedStylesheet;
				}
				else
					throw new ApplicationException(viewModelError);
			}
		}
	}
}
