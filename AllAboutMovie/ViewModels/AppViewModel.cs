using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using AllAboutMovie.Common;
using System.Collections.Generic;
using AllAboutMovie.Core;
using AllAboutMovie.Properties;

namespace AllAboutMovie.ViewModels
{
	public class AppViewModel : ViewModel
	{
		public string AppTitle { get; set; }

		public LocalizationViewModel Localization { get; set; }

		public SearchTitleViewModel SearchTitle { get; set; }
		public string SearchYear { get; set; }

		private bool _isLoading;
		public bool IsLoading
		{
			get { return _isLoading; }
			set
			{
				if (_isLoading != value)
				{
					_isLoading = value;
					RaisePropertyChanged(() => IsLoading);
				}
			}
		}

		private string _loadMessage;
		public string LoadMessage
		{
			get { return _loadMessage; }
			set { _loadMessage = value; RaisePropertyChanged(() => LoadMessage); }
		}

		public IEnumerable<IMovieExporterMetadata> Exporters { get; set; }
		private string _selectedExporterName;
		public string SelectedExporterName
		{
			get { return _selectedExporterName; }
			set { _selectedExporterName = value; RaisePropertyChanged(() => SelectedExporterName); }
		}

		public IEnumerable<IMovieCatalogMetadata> Catalogs { get; set; }
		private string _selectedCatalogName;
		public string SelectedCatalogName
		{
			get { return _selectedCatalogName; }
			set
			{
				_selectedCatalogName = value;
				RaisePropertyChanged(() => SelectedCatalogName);
				FoundMovies = null;
				SelectedMovieID = null;
			}
		}

		private ObservableCollection<MovieViewModel> _foundMovies;
		public ObservableCollection<MovieViewModel> FoundMovies
		{
			get { return _foundMovies; }
			set
			{
				_foundMovies = value; 
				RaisePropertyChanged(() => FoundMovies);
				if (_foundMovies != null && _foundMovies.Any())
					SelectedMovieID = _foundMovies.First().ID;
			}
		}

		private string _selectedMovieID;
		public string SelectedMovieID
		{
			get { return _selectedMovieID; }
			set
			{
				if (_selectedMovieID != value)
				{
					_selectedMovieID = value;
					RaisePropertyChanged(() => SelectedMovieID);
					Movie = null;
				}
			}
		}

		private MovieViewModel _movie;
		public MovieViewModel Movie
		{
			get { return _movie; }
			set { _movie = value; RaisePropertyChanged(() => Movie); }
		}
		
		public AppViewModel()
		{
			Localization = new LocalizationViewModel();
			SearchTitle = new SearchTitleViewModel() { Text = String.Empty };
			SearchYear = String.Empty;

			AddValidationRule(
				() => SearchYear,
				Resources.BadYearError,
				delegate
					{
						if (String.IsNullOrEmpty(SearchYear))
							return true;

						uint year;
						return uint.TryParse(SearchYear, out year);
					});
		}
	}
}