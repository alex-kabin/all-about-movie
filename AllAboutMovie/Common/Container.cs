using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace AllAboutMovie.Common
{
	public class Container
	{
		private static readonly IContainer Instance;

		public static IContainer Global
		{
			get { return Instance; }
		}

		static Container()
		{
			AggregateCatalog catalog = new AggregateCatalog();
			catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
			catalog.Catalogs.Add(new DirectoryCatalog(".", "AllAboutMovie.Catalogs.*.dll"));
			catalog.Catalogs.Add(new DirectoryCatalog(".", "AllAboutMovie.Exporters.*.dll"));

			var composition = new CompositionContainer(catalog);
			Instance = new MEFContainer(composition);
		}
	}
}