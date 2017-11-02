using System.Configuration;

namespace AllAboutMovie.WebCatalogBase
{
	public abstract class ConfigurationElementCollection<T> : ConfigurationElementCollection where T : ConfigurationElement, new()
	{
		public void Add(T customElement)
		{
			BaseAdd(customElement);
		}

		protected override void BaseAdd(ConfigurationElement element)
		{
			base.BaseAdd(element, false);
		}

		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new T();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return GetKey((T)element);
		}

		protected abstract object GetKey(T element);
		

		public T this[int index]
		{
			get { return (T)BaseGet(index); }
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}

		public new T this[string name]
		{
			get { return (T)BaseGet(name); }
		}

		public int IndexOf(T element)
		{
			return BaseIndexOf(element);
		}

		public void Remove(T element)
		{
			if (BaseIndexOf(element) >= 0)
				BaseRemove(GetKey(element));
		}

		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}

		public void Remove(string name)
		{
			BaseRemove(name);
		}

		public void Clear()
		{
			BaseClear();
		}
	}
}