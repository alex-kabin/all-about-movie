using System;
using System.Windows.Input;

namespace AllAboutMovie.Core.UI
{
	public abstract class PluginSettingsManagerBase<T> : IPluginSettingsManager where T : IPlugin
	{
		protected T ConcretePlugin { get; set; }

		public IPlugin Plugin
		{
			get
			{
				return ConcretePlugin; }
			set
			{
				if (value is T)
					ConcretePlugin = (T)value;
				else
				{
					throw new InvalidOperationException(String.Format("Plugin should be of type '{0}'", typeof(T)));
				}
			}
		}

		public abstract IView CreateSettingsView();
		
		protected abstract void Apply(object data);

		protected virtual bool CanApply(object data)
		{
			return true;
		}

		private ICommand _applyCommand;
		public ICommand ApplyCommand
		{
			get { return _applyCommand ?? (_applyCommand = new DelegateCommand(Apply, CanApply)); }
		}
	}
}