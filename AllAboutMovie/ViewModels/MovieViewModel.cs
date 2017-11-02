using System;
using System.Configuration;
using System.Linq;
using System.Net.Cache;
using System.Windows.Media.Imaging;
using AllAboutMovie.Common;
using AllAboutMovie.Core;
using System.Collections.Generic;

namespace AllAboutMovie.ViewModels
{
	public class MovieViewModel : ViewModel<Movie>
	{
		public MovieViewModel(Movie movie) : base(movie)
		{
			bool loadThumbnails = false;
			Boolean.TryParse(ConfigurationManager.AppSettings["LoadThumbnails"], out loadThumbnails);
			LoadThumbnails = loadThumbnails;
		}

		public bool LoadThumbnails { get; private set; }

		public int Index { get; set; }
		public string ID { get { return Data.ID; } }
		
		public string OriginalTitle { get { return Data.OriginalTitle; } }
		public string TranslatedTitle { get { return Data.TranslatedTitle; } }

		public string Title
		{
			get
			{
				return String.IsNullOrEmpty(TranslatedTitle)
				       	? OriginalTitle
				       	: String.Format("{0} / {1}", OriginalTitle, TranslatedTitle);

			}
		}


		public int Year { get { return Data.Year; } }

		public string Slogan { get { return Data.Slogan; } }
		public string Budget { get { return Data.Budget; } }

		public IEnumerable<string> Countries { get { return Data.Countries; } }
		public string CountriesString { get { return String.Join(", ", Data.Countries); } }

		public string CountriesStringOrUnknown
		{
			get
			{
				var countriesString = CountriesString;
				return countriesString == String.Empty ? "?" : countriesString;
			}
		}

		public string CountriesStringOrEmpty
		{
			get
			{
				var countriesString = CountriesString;
				return countriesString == String.Empty ? String.Empty : String.Format("{0}; ", countriesString);
			}
		}

		public IEnumerable<string> Genres { get { return Data.Genres; } }
		public string GenresString { get { return String.Join(" / ", Data.Genres); } }

		public string GenresStringOrNull
		{
			get { var str = GenresString; return str == String.Empty ? null : str; }
		}

		public IEnumerable<Person> Directors { get { return Data.Directors; } }
		public string DirectorsString { get { return String.Join(", ", Data.Directors.Select(p => p.Name)); } }

		public string DirectorsStringOrUnknown
		{
			get { var str = DirectorsString; return str == String.Empty ? "?" : str; }
		}

		public IEnumerable<Person> Actors { get { return Data.Actors;} }
		public string ActorsString { get { return String.Join(", ", Data.Actors.Select(p => p.Name)); } }

		public string RuntimeInMinutes
		{
			get { return Data.RuntimeInMinutes.ToString(); }
		}

		public string RuntimeInMinutesOrEmpty
		{
			get { return Data.RuntimeInMinutes > 0 ? Data.RuntimeInMinutes.ToString() : String.Empty; }
		}

		public string RuntimeHourMinutes
		{
			get
			{
				return String.Format("{0}:{1:D2}", Data.RuntimeInMinutes / 60, Data.RuntimeInMinutes % 60);
			}
		}

		public string Storyline { get { return Data.Storyline; } }
		
		// public Uri PosterUri { get { return String.IsNullOrEmpty(Data.PosterUrl) ? null : new Uri(Data.PosterUrl, UriKind.Absolute);} }
		
		public string PosterUrl
		{
			get
			{
				return !String.IsNullOrEmpty(Data.PosterUrl) ? Data.PosterUrl : null; // "Resources/no-poster.png";
			}
		}
		// public Uri ThumbnailUri { get { return new Uri(Data.ThumbnailUrl, UriKind.Absolute); } }
		
		public string ThumbnailUrl
		{
			get
			{
				if (LoadThumbnails && !String.IsNullOrEmpty(Data.ThumbnailUrl))
					return Data.ThumbnailUrl;
				else
					return "Resources/movie.png";
			}
		}
		
		public IEnumerable<RatingViewModel> Ratings
		{
			get { return Data.Ratings.Select(r => new RatingViewModel(r)); }
		}
		//public Uri Url { get { return new Uri(Data.Url, UriKind.Absolute); } }
		public string Url { get { return Data.Url; } }

		public bool WonOscar { get { return Data.WonOscar; } }

		public string MPAA 
		{
			get { return Data.Mpaa == Core.MPAA.None ? "?" : Data.Mpaa.ToString(); }
		}

		public string MPAALogoUri
		{
			get 
			{
				return Data.Mpaa == Core.MPAA.None 
					? String.Empty 
					: String.Format("/Resources/mpaa-{0}.gif", Data.Mpaa.ToString().ToLower());
			}
		}

	}
}