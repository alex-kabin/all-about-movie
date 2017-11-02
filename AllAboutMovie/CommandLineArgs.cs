using System;
using System.IO;
using System.Text.RegularExpressions;
using AllAboutMovie.Utils;

namespace AllAboutMovie
{
	public class CommandLineArgs
	{
		public CommandLineArgs() : this(Environment.GetCommandLineArgs())
		{
		}

		public CommandLineArgs(string[] args)
		{
			var searchTitle = String.Empty;
			var searchYear = String.Empty;

			if (args.Length > 1)
			{
				MovieFilePath = args[1];
				MovieFileHelper.ExtractTitleYearFromPath(MovieFilePath, out searchTitle, out searchYear);
			}

			SearchTitle = searchTitle;
			SearchYear = searchYear;
		}
		

		public void Reset()
		{
			MovieFilePath = null;
		}

		public bool Specified
		{
			get { return !String.IsNullOrEmpty(MovieFilePath); }
		}

		public bool MovieFileExists
		{
			get { return Specified && File.Exists(MovieFilePath); }
		}

		public string SearchTitle { get; private set; }
		public string SearchYear { get; private set; }
		public string MovieFilePath { get; private set; }
	}
}