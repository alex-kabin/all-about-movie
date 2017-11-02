namespace AllAboutMovie.Core
{
	public interface ISettingsService
	{
		string this[string settingKey] { get; set; }
		void Save();
	}
}