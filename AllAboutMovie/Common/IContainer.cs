using System;

namespace AllAboutMovie.Common
{
	public interface IContainer : IDisposable
	{
		T Resolve<T>();
		T Resolve<T>(string name);
		void Inject(object target);
	}
}