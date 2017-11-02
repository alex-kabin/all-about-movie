namespace AllAboutMovie.Core
{
	public struct FileType
	{
		private string _extension;
		public string Extension { get { return _extension; } set { _extension = value; } }

		private string _description;
		public string Description { get { return _description; } set { _description = value; } }

		public FileType(string extension)
			: this(extension, null)
		{
		}

		public FileType(string extension, string description)
		{
			_extension = extension;
			_description = description;
		}
	}
}