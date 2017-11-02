using System;
using AllAboutMovie.Core.UI;

namespace AllAboutMovie.Common
{
	public abstract class ActiveControllerBase<TView, TData> : IActiveController<TView, TData>, IDisposable
		where TView : class, IView
		where TData : ViewModel, new()
	{
		protected ActiveControllerBase(TView view)
		{
			View = view;
			ViewData = new TData() { Controller = this };
		}

		private bool _disposed = false;
		public virtual void Dispose()
		{
			if (!_disposed)
			{
				Disposing();
				_disposed = true;
			}
		}

		protected virtual void Disposing()
		{
		}

		public abstract void Run();
		
		public TView View { get; set; }

		public TData ViewData { get; private set; }
	}

	public abstract class PassiveControllerBase<TData> : IPassiveController<TData>, IDisposable
		where TData : ViewModel, new()
	{
		protected PassiveControllerBase()
		{
		}

		protected PassiveControllerBase(TData viewData)
		{
			ViewData = viewData;
		}

		private bool _disposed = false;
		public virtual void Dispose()
		{
			if (!_disposed)
			{
				Disposing();
				_disposed = true;
			}
		}

		protected virtual void Disposing()
		{
		}

		private TData _viewData;
		public TData ViewData
		{
			get { return _viewData; }
			set
			{
				_viewData = value;
				_viewData.Controller = this;
			}
		}
	}
}