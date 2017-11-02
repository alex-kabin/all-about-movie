using System;
using System.Linq;
using System.ComponentModel.Composition;
using AllAboutMovie.Core;
using AllAboutMovie.Core.UI;

namespace AllAboutMovie.Catalogs.Local.UI
{
	[Export(typeof(IPluginSettingsManager))]
	[PluginSettingsManagerMetadata(PluginType = typeof(LocalMovieCatalog))]
	public class LocalCatalogSettingsManager : PluginSettingsManagerBase<LocalMovieCatalog>
	{
		[Import]
		private ISettingsService Settings { get; set; }

		public override IView CreateSettingsView()
		{
			string foldersString = null;
			string extensionsString = null;

			if (ConcretePlugin.Folders != null)
				foldersString = String.Join("\n", ConcretePlugin.Folders);
			if (ConcretePlugin.Extensions != null)
				extensionsString = String.Join(",", ConcretePlugin.Extensions);

			if (foldersString == null && Settings != null)
			{
				foldersString = Settings["Local_Folders"];
				if (foldersString != null)
					foldersString = foldersString.Replace("|", "\n");
			}

			if (extensionsString == null && Settings != null)
			{
				extensionsString = Settings["Local_Extensions"];
			}

			if (foldersString == null)
				foldersString = @"C:\";
			if (extensionsString == null)
				extensionsString = "*.avi,*.mkv";

			return new SettingsControl()
			       	{
			       		DataContext = new SettingsViewModel()
			       		              	{
			       		              		FoldersString = foldersString,
			       		              		ExtensionsString = extensionsString
			       		              	}
			       	};
		}

		protected override void Apply(object data)
		{
			var viewModel = data as SettingsViewModel;
			if (viewModel != null)
			{
				var viewModelError = viewModel.Error;
				if (String.IsNullOrEmpty(viewModelError))
				{
					var folders = viewModel.FoldersString.Split(new [] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
					ConcretePlugin.Folders = folders;

					var extensions = viewModel.ExtensionsString.Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries);
					ConcretePlugin.Extensions = extensions;

					Settings["Local_Folders"] = viewModel.FoldersString.Replace("\n", "|");
					Settings["Local_Extensions"] = viewModel.ExtensionsString;
				}
				else
					throw new ApplicationException(viewModelError);
			}
		}
	}
}