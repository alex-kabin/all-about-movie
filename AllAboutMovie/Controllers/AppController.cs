using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Input;
using AllAboutMovie.Common;
using AllAboutMovie.Core;
using AllAboutMovie.Core.UI;
using AllAboutMovie.Properties;
using AllAboutMovie.Utils;
using AllAboutMovie.ViewModels;
using Microsoft.Win32;
using NLog;
using Container = AllAboutMovie.Common.Container;

namespace AllAboutMovie.Controllers
{
	[Export("RootController", typeof(IActiveController))]
	public class AppController : ActiveControllerBase<IWindow, AppViewModel>, IPartImportsSatisfiedNotification
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		[ImportMany]
		private Lazy<IPluginSettingsManager, IPluginSettingsManagerMetadata>[] ImportedSettingsManagers { get; set; }
		private IDictionary<Type, Lazy<IPluginSettingsManager>> _settingsManagers;

		[ImportMany]
		private Lazy<IMovieCatalog, IMovieCatalogMetadata>[] ImportedCatalogs { get; set; }
		private IDictionary<string, Lazy<IMovieCatalog>> _catalogs;

		[ImportMany]
		private Lazy<IMovieExporter, IMovieExporterMetadata>[] ImportedExporters { get; set; }
		private IDictionary<string, Lazy<IMovieExporter>> _exporters;

		[Import]
		private ISettingsService Settings { get; set; }

		private readonly List<Uri> _pages = new List<Uri>
		                                    	{
		                                    		new Uri("SearchPage.xaml", UriKind.RelativeOrAbsolute),
		                                    		new Uri("DetailsPage.xaml", UriKind.RelativeOrAbsolute)
		                                    	};

		private readonly INavigationService _navigationService;
		private readonly IDialogService _dialogService;
		private readonly IUIMessageService _uiMessageService;

		private IMovieExporter CurrentExporter
		{
			get
			{
				Lazy<IMovieExporter> exporter;
				if (_exporters.TryGetValue(ViewData.SelectedExporterName, out exporter))
				{
					return exporter.Value;
				}

				return null;
			}
		}
		private IMovieCatalog CurrentCatalog
		{
			get
			{
				Lazy<IMovieCatalog> catalog;
				if (_catalogs.TryGetValue(ViewData.SelectedCatalogName, out catalog))
				{
					return catalog.Value;
				}

				return null;
			}
		}

		private CommandLineArgs _inputArgs;
		private CancellationTokenSource _cancellationTokenSource;
		
		[ImportingConstructor]
		public AppController(
			[Import("RootWindow")] IWindow window,
			[Import("RootNavigator")] INavigationService navigationService,
			[Import] IUIMessageService uiMessageService,
			[Import] IDialogService dialogService) : base(window)
		{
			_navigationService = navigationService;
			_dialogService = dialogService;
			_uiMessageService = uiMessageService;

			_inputArgs = new CommandLineArgs();

			CreateChildControllers();
			InitializeViewData();
		}

		private void CreateChildControllers()
		{
			new SearchTitleController(ViewData.SearchTitle);
			new LocalizationController(ViewData.Localization);
		}

		private void InitializeViewData()
		{
			ViewData.SearchTitle.Text = _inputArgs.SearchTitle;
			ViewData.SearchYear = _inputArgs.SearchYear;
			if (_inputArgs.Specified)
				ViewData.AppTitle = String.Format("{0} [{1}]", ApplicationInfo.ProductName, _inputArgs.MovieFilePath);
			else
				ViewData.AppTitle = String.Format("{0}", ApplicationInfo.ProductName);
		}

		public override void Run()
		{
			View.DataContext = ViewData;
			View.Closed += View_Closed;
			View.Show();
			GoToSearch();
		}

		private void SaveSettings()
		{
			if (Settings != null)
			{
				Settings["Catalog"] = ViewData.SelectedCatalogName;
				Settings["Exporter"] = ViewData.SelectedExporterName;
				Settings.Save();
			}
		}

		private void View_Closed(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void NotifyBeginLoad(string message)
		{
			ViewData.LoadMessage = message;
			ViewData.IsLoading = true;
		}

		private void NotifyEndLoad()
		{
			ViewData.IsLoading = false;
		}

		private bool TunePlugin(IPlugin plugin, string pluginName)
		{
			var pluginType = plugin.GetType();
			IPluginSettingsManager pluginSettingsManager = null;

			if (_settingsManagers.ContainsKey(pluginType))
			{
				pluginSettingsManager = _settingsManagers[pluginType].Value;
			}

			if (pluginSettingsManager != null)
			{
				pluginSettingsManager.Plugin = plugin;
				var settingsDialog = Container.Global.Resolve<IWindow>("PluginSettingsDialog");
				var pluginSettingsViewModel =
					new PluginSettingsViewModel
						{
							Title = String.Format(Resources.PluginSettingsTitle, pluginName),
							SettingsView = pluginSettingsManager.CreateSettingsView(),
							ApplyCommand = pluginSettingsManager.ApplyCommand
						};

				return _dialogService.ShowModal(settingsDialog, pluginSettingsViewModel) == true;
			}
			return true;
		}

		#region Search
		private IAsyncResult SearchMoviesAsync()
		{
			NotifyBeginLoad(Resources.SearchingMovie);
			Func<Tuple<string, int>, CancellationToken, IEnumerable<Movie>> operation =
				(searchParams, token) =>
					{
						var title = searchParams.Item1;
						var year = searchParams.Item2;
						if (year != 0)
							return CurrentCatalog.SearchByTitleAndYear(title, year);
						else
							return CurrentCatalog.SearchByTitle(title);
					};

			string movieTitle = ViewData.SearchTitle.Text;
			int movieYear;
			int.TryParse(ViewData.SearchYear, out movieYear);

			_cancellationTokenSource = new CancellationTokenSource();
			return operation.InvokeAsync(
				Tuple.Create(movieTitle, movieYear),
				(movies, error) =>
					{
						NotifyEndLoad();
						if (error != null)
						{
							Log.Error(error);
							_uiMessageService.ShowError(error.Message);
						}
						else
						{
							if (movies != null)
							{
								int index = 1;
								ViewData.FoundMovies =
									new ObservableCollection<MovieViewModel>(
										movies.Select(m => new MovieViewModel(m) { Index = index++ }));
							}
							else
							{
								ViewData.FoundMovies = null;
								_uiMessageService.ShowInfo(Resources.MovieNotFound);
							}
						}
					},
				_cancellationTokenSource.Token
				);
		}

		public void Search()
		{
			if(TunePlugin(CurrentCatalog, ViewData.SelectedCatalogName))
				SearchMoviesAsync();
		}

		private ICommand _searchCommand;
		public ICommand SearchCommand
		{
			get
			{
				return _searchCommand ??
				       (_searchCommand = new DelegateCommand(Search,
				                                             () =>
				                                             ViewData.SelectedCatalogName != null 
															 && !ViewData.SearchTitle.IsEmpty));
			}
		}
		#endregion // Search

		#region Export
		private static string GetMovieFileName(Movie movie)
		{
			string fileName = null;

			try
			{
				fileName = MovieFileNameGenerator.Default.GetMovieFileName(movie);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			if (String.IsNullOrEmpty(fileName))
			{
				fileName = movie.OriginalTitle;
				if (!String.IsNullOrEmpty(movie.TranslatedTitle))
				{
					fileName = String.Format("{0} ({1})", movie.TranslatedTitle, movie.OriginalTitle);
				}
			}

			var cleanFileName = new StringBuilder(fileName);
			foreach (var invalidFileNameChar in Path.GetInvalidFileNameChars())
				cleanFileName.Replace(invalidFileNameChar, '_');

			return cleanFileName.ToString();
		}

		private ICommand _exportCommand;
		public void Export()
		{
			var exporter = CurrentExporter;
			if(!TunePlugin(exporter, ViewData.SelectedExporterName))
				return;

			var movie = ViewData.Movie.Data;

			var fileExporter = exporter as IFileMovieExporter;
			if (fileExporter != null)
			{
				var outputFileType = fileExporter.GetOutputFileType();
				var outputFileTypeLabel = String.IsNullOrEmpty(outputFileType.Description)
				                          	? String.Format("{0} {1}", outputFileType.Extension.ToUpper(), Resources.File)
				                          	: outputFileType.Description;

				var saveFileDialog = new SaveFileDialog
				                     	{
				                     		AddExtension = true,
											OverwritePrompt = true,
				                     		Filter = String.Format("{0}|*.{1}", outputFileTypeLabel, outputFileType.Extension),
											FileName = GetMovieFileName(movie)
				                     	};
				
				if (_inputArgs.MovieFileExists)
					saveFileDialog.InitialDirectory = Path.GetDirectoryName(_inputArgs.MovieFilePath);

				if (saveFileDialog.ShowDialog() == true)
				{
					fileExporter.FileName = saveFileDialog.FileName;
				}
				else
				{
					return;
				}
			}

			try
			{
				exporter.Export(movie);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				_uiMessageService.ShowInfo(String.Format(Resources.DetailsExportError, ex.Message));
				return;
			}

			if(fileExporter != null && _inputArgs.MovieFileExists)
			{
				var oldFileName = Path.GetFileName(_inputArgs.MovieFilePath);
				var oldFilePath = Path.GetDirectoryName(_inputArgs.MovieFilePath);
				var fileExtension = Path.GetExtension(_inputArgs.MovieFilePath);
				var newFileName = Path.GetFileNameWithoutExtension(fileExporter.FileName)+fileExtension;
				var newFilePath = Path.Combine(oldFilePath, newFileName);
				
				if (!File.Exists(newFilePath) && (oldFileName != newFileName))
				{
					var question = String.Format(Resources.RenameMovieFileQuestion, oldFileName, newFileName);
					if (_uiMessageService.ShowQuestion(question, MessageBoxButton.YesNo) == true)
					{
						try
						{
							File.Move(_inputArgs.MovieFilePath, newFilePath);
							_inputArgs.Reset();
						}
						catch (Exception ex)
						{
							Log.Error(ex);
							_uiMessageService.ShowInfo(String.Format(Resources.RenameMovieFileError, ex.Message));
						}
					}
				}
			}
		}

		public ICommand ExportCommand
		{
			get { return _exportCommand ?? (_exportCommand = new DelegateCommand(Export, () => !String.IsNullOrEmpty(ViewData.SelectedExporterName))); }
		}
		#endregion // ExportCommand

		#region Exit
		public void Exit()
		{
			Application.Current.Shutdown();
		}

		private ICommand _exitCommand;
		public ICommand ExitCommand
		{
			get { return _exitCommand ?? (_exitCommand = new DelegateCommand(Exit, () => true)); }
		}
		#endregion // Exit

		#region GoToSearch
		public void GoToSearch()
		{
			_navigationService.GoTo(_pages[0]);
		}

		private ICommand _goToSearchCommand;
		public ICommand GoToSearchCommand
		{
			get { return _goToSearchCommand ?? (_goToSearchCommand = new DelegateCommand(GoToSearch)); }
		}
		#endregion // GoToSearch

		#region GoToDetails
		private IAsyncResult LoadMovieAsync(Action action)
		{
			if (ViewData.Movie != null)
			{
				action();
				return null;
			}

			var movieID = ViewData.SelectedMovieID;
			NotifyBeginLoad(Resources.LoadingDetails);

			Func<string, CancellationToken, Movie> operation = (id, token) => CurrentCatalog.GetById(id);
			_cancellationTokenSource = new CancellationTokenSource();
			return operation.InvokeAsync(movieID,
			                             (movie, error) =>
			                             	{
			                             		NotifyEndLoad();
			                             		if (error != null)
			                             		{
			                             			Log.Error(error);
			                             			_uiMessageService.ShowError(error.Message);
			                             		}
			                             		else
			                             		{
			                             			if (movie != null)
			                             			{
			                             				ViewData.Movie = new MovieViewModel(movie);
			                             				action();
			                             			}
			                             			else
			                             			{
			                             				_uiMessageService.ShowInfo(Resources.MovieNotFound);
			                             			}
			                             		}
			                             	},
			                             _cancellationTokenSource.Token);
		}

		private void GoToDetails()
		{
			LoadMovieAsync(() => _navigationService.GoTo(_pages[1]));
		}

		private ICommand _goToDetailsCommand;
		public ICommand GoToDetailsCommand
		{
			get
			{
				return _goToDetailsCommand ??
				       (_goToDetailsCommand = new DelegateCommand(
				                              	GoToDetails,
				                              	() => ViewData.SelectedMovieID != null && !ViewData.IsLoading));
			}
		}
		#endregion // GoToDetails

		#region Cancel
		public void Cancel()
		{
			if (_cancellationTokenSource != null)
			{
				_cancellationTokenSource.Cancel();
				NotifyEndLoad();
			}
		}

		private ICommand _cancelCommand;
		public ICommand CancelCommand
		{
			get
			{
				return _cancelCommand ??
						 (_cancelCommand = new DelegateCommand(Cancel, () => _cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested));
			}
		}
		#endregion // Cancel

		#region Реализация IPartImportsSatisfiedNotification
		void IPartImportsSatisfiedNotification.OnImportsSatisfied()
		{
			_settingsManagers = new Dictionary<Type, Lazy<IPluginSettingsManager>>();
			foreach (var settingsManager in ImportedSettingsManagers)
				_settingsManagers.Add(settingsManager.Metadata.PluginType, settingsManager);

			_catalogs = new Dictionary<string, Lazy<IMovieCatalog>>();
			foreach (var importedCatalog in ImportedCatalogs)
				_catalogs.Add(importedCatalog.Metadata.Name, importedCatalog);
			ViewData.Catalogs = ImportedCatalogs.Select(c => c.Metadata);

			var lastCatalogName = Settings != null ? Settings["Catalog"] ?? String.Empty : String.Empty;
			if (_catalogs.ContainsKey(lastCatalogName))
				ViewData.SelectedCatalogName = lastCatalogName;
			else
				ViewData.SelectedCatalogName = _catalogs.Keys.FirstOrDefault();

			_exporters = new Dictionary<string, Lazy<IMovieExporter>>();
			foreach (var importedExporter in ImportedExporters)
				_exporters.Add(importedExporter.Metadata.Name, importedExporter);
			ViewData.Exporters = ImportedExporters.Select(c => c.Metadata);

			var lastExporterName = Settings != null ? Settings["Exporter"] ?? String.Empty : String.Empty;
			if (_exporters.ContainsKey(lastExporterName))
				ViewData.SelectedExporterName = lastExporterName;
			else
				ViewData.SelectedExporterName = _exporters.Keys.FirstOrDefault();
		}
		#endregion
	}
}