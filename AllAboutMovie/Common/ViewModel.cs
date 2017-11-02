using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AllAboutMovie.Common
{
	public abstract class ViewModel : ObservableObject, IDataErrorInfo
	{
		private IController _controller;
		public virtual IController Controller
		{
			get { return _controller; }
			set { _controller = value; RaisePropertyChanged(() => Controller); }
		}

		private readonly IDictionary<string, ComplexValidationRule> _validationRules =
			new Dictionary<string, ComplexValidationRule>();

		protected void AddValidationRule<T>(Expression<Func<T>> property, IValidationRule validationRule)
		{
			var propertyName = GetPropertyName(property);
			if(_validationRules.ContainsKey(propertyName))
				_validationRules[propertyName].Rules.Add(validationRule);
			else
			{
				var complexRule = new ComplexValidationRule();
				complexRule.Rules.Add(validationRule);
				_validationRules.Add(propertyName, complexRule);
			}
		}

		protected void AddValidationRule<T>(Expression<Func<T>> property, string error, Func<bool> rule)
		{
			AddValidationRule(property, new DelegateValidationRule() { Error = error, Rule = rule });
		}

		public string this[string columnName]
		{
			get 
			{ 
				var errorString = String.Empty;
				if(_validationRules.ContainsKey(columnName))
				{
					var rule = _validationRules[columnName];
					errorString = rule.Validate();
				}
				return errorString;
			}
		}

		public string Error
		{
			get 
			{ 
				var errorBuilder = new StringBuilder();
				foreach (var propertyName in _validationRules.Keys)
				{
					var error = _validationRules[propertyName].Validate();
					if (!String.IsNullOrEmpty(error))
						errorBuilder.AppendLine(error);
				}

				return errorBuilder.ToString();
			}
		}
	}

	public abstract class ViewModel<T> : ViewModel
	{
		protected ViewModel(T data)
		{
			Data = data;
		}

		public T Data { get; private set; }
	}
}