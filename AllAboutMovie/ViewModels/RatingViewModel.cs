using System;
using AllAboutMovie.Common;
using AllAboutMovie.Core;

namespace AllAboutMovie.ViewModels
{
	public class RatingViewModel : ViewModel<Rating>
	{
		public RatingViewModel(Rating rating) : base(rating)
		{
		}

		public string RatedBy { get { return Data.RatedBy; } }
		
		public string ValueString
		{
			get
			{
				var valueString = Data.Value;

				if(!String.IsNullOrEmpty(Data.MaxValue))
					valueString = String.Format("{0}/{1}", valueString, Data.MaxValue);
				
				if(Data.Votes > 0)
					valueString = String.Format("{0} ({1:N0})", valueString, Data.Votes);
				
				return valueString;
			}
		}

		public string MaxValue
		{
			get
			{
				return Data.MaxValue;
			}
		}
	}
}