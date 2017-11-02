using System;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AllAboutMovie.Common
{
	/// <summary>
	/// This is the abstract base class for any object that provides property change notifications.  
	/// </summary>
	[Serializable]
	public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
	{
		protected ObservableObject()
		{
		}

		[NonSerialized]
		private PropertyChangedEventHandler propertyChanged;

        [NonSerialized]
        private PropertyChangingEventHandler propertyChanging;

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged
		{
			add { propertyChanged += value; }
			remove { propertyChanged -= value; }
		}

        /// <summary>
        /// Occurs when a property value changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging
        {
            add { propertyChanging += value; }
            remove { propertyChanging -= value; }
        }

		protected static string GetPropertyName<T>(Expression<Func<T>> property)
		{
			var propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;
			if (propertyInfo == null)
				throw new ArgumentException("The lambda expression 'property' should point to a valid Property");

			return propertyInfo.Name;
		}


		/// <summary>
		/// Raises the <see cref="E:PropertyChanged"/> event.
		/// </summary>
		/// <param name="propertyName">The property name of the property that has changed.</param>
		[SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaisePropertyChanged(string propertyName)
		{
			CheckPropertyName(propertyName);
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		[SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyLambda)
		{
			string propertyName = GetPropertyName(propertyLambda);
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        protected void RaisePropertyChanging(string propertyName)
        {
            CheckPropertyName(propertyName);
            OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }

        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        protected void RaisePropertyChanging<T>(Expression<Func<T>> propertyLambda)
        {
            string propertyName = GetPropertyName(propertyLambda);
            OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }

		/// <summary>
		/// Raises the <see cref="E:PropertyChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="RBIT.Sys.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (propertyChanged != null) { propertyChanged(this, e); }
		}

        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            if (propertyChanging != null) { propertyChanging(this, e); }
        }

		[Conditional("DEBUG")]
		private void CheckPropertyName(string propertyName)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)[propertyName];
			if (propertyDescriptor == null)
			{
				throw new InvalidOperationException(string.Format(null,
					"The property with the propertyName '{0}' doesn't exist.", propertyName));
			}
		}
	}
}
