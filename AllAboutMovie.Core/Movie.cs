using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AllAboutMovie.Core
{
	public class Movie
	{
		public string ID { get; set; }
		public string OriginalTitle { get; set; }
		public string TranslatedTitle { get; set; }
		public int Year { get; set; }
		public List<string> Countries { get; set; }
		public List<string> Genres { get; set; }
		public List<Person> Directors { get; set; }
		public List<Person> Actors { get; set; }
		
		[XmlIgnore]
		public TimeSpan Runtime { get; set; }
		
		[XmlElement("Runtime")]
		public int RuntimeInMinutes
		{
			get { return (int)Runtime.TotalMinutes; }
			set { Runtime = TimeSpan.FromMinutes(value); }
		}

		public string Budget { get; set; }
		public string Slogan { get; set; }
		public string Storyline { get; set; }
		//public BitmapImage Poster { get; set; }
		//public BitmapImage Thumbnail { get; set; }
		public string PosterUrl { get; set; }
		public string ThumbnailUrl { get; set; }
		public List<Rating> Ratings { get; set; }
		public string Url { get; set; }

		[XmlAttribute("Oscar")]
		public bool WonOscar { get; set; }

		[XmlAttribute("MPAA")]
		public MPAA Mpaa { get; set; }

		public string Comment { get; set; }
		public Movie()
		{
			Genres = new List<string>();
			Directors = new List<Person>();
			Actors = new List<Person>();
			Ratings = new List<Rating>();
			Countries = new List<string>();
		}
	}
}
