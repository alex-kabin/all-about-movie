using System;
using System.Windows;
using AllAboutMovie.Core.UI;

namespace AllAboutMovie.Common
{
	public interface IController
	{
	}

	public interface IActiveController : IController
	{
		void Run();
	}

	public interface IPassiveController<TData> : IController
	{
		TData ViewData { get; set; }
	}

	public interface IActiveController<TView, out TData> : IActiveController
		where TView : class, IView
	{
		TData ViewData { get; }
		
		TView View { get; set; }
	}
}