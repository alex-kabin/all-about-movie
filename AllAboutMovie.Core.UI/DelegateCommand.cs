using System;
using System.Windows.Input;

namespace AllAboutMovie.Core.UI
{
	public class DelegateCommand : ICommand
	{
		private readonly Predicate<object> _canExecute;
		private readonly Action<object> _execute;
		
		private readonly Func<bool> _canExecuteNoParams;
		private readonly Action _executeNoParams;

		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (_canExecute != null || _canExecuteNoParams != null)
					CommandManager.RequerySuggested += value;
			}
			remove
			{
				if (_canExecute != null || _canExecuteNoParams != null)
					CommandManager.RequerySuggested -= value;
			}
		}

		public DelegateCommand(Action<object> execute)
			: this(execute, null)
		{
		}

		public DelegateCommand(Action execute)
			: this(execute, null)
		{
		}

		public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public DelegateCommand(Action execute, Func<bool> canExecute)
		{
			_executeNoParams = execute;
			_canExecuteNoParams = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			if(_canExecute != null)
				return _canExecute(parameter);

			if (_canExecuteNoParams != null)
				return _canExecuteNoParams();

			return true;
		}

		public void Execute(object parameter)
		{
			if(_execute != null)
				_execute(parameter);

			if (_executeNoParams != null)
				_executeNoParams();
		}
	}
}