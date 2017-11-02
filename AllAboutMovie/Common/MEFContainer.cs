using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace AllAboutMovie.Common
{
	public class MEFContainer : IContainer
	{
		private readonly CompositionContainer _container;

		public MEFContainer(CompositionContainer container)
		{
			_container = container;
		}

		public T Resolve<T>()
		{
			return _container.GetExportedValue<T>();
		}

		public T Resolve<T>(string name)
		{
			return _container.GetExportedValue<T>(name);
		}

		public void Inject(object target)
		{
			_container.SatisfyImportsOnce(target);
		}

		public void Dispose()
		{
			_container.Dispose();
		}
	}
}