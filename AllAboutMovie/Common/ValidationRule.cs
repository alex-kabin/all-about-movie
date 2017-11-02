using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AllAboutMovie.Common
{
	public interface IValidationRule
	{
		string Error { get; set; }
		string Validate();
	}

	public class DelegateValidationRule : IValidationRule
	{
		public string Error { get; set; }
		public Func<bool> Rule { get; set; }
		
		public string Validate()
		{
			return Rule() ? String.Empty : Error;
		}
	}

	public class ComplexValidationRule : IValidationRule
	{
		public string Error { get; set; }
		public List<IValidationRule> Rules { get; set; }

		public ComplexValidationRule()
		{
			Rules = new List<IValidationRule>();
		}

		public string Validate()
		{
			var complexError = String.Join(Environment.NewLine,
			                               from r in Rules
			                               let error = r.Validate()
			                               where !String.IsNullOrEmpty(error)
			                               select error);

			if (complexError != String.Empty)
			{
				return Error ?? complexError;
			}

			return String.Empty;
		}
	}
}
