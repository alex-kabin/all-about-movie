using System.Configuration;
using System.Reflection;
using System.Linq;

namespace RegisterShellActions
{
	internal static class Configuration
	{
		public static RootConfigurationSection Current
		{
			get
			{
				return ConfigurationManager.GetSection("settings") as RootConfigurationSection;
			}
		}
	}


	internal class RootConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("actions", IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(ActionsCollection), AddItemName = "add",
			ClearItemsName = "clear",
			RemoveItemName = "remove")]
		public ActionsCollection Actions
		{
			get
			{
				return (ActionsCollection)base["actions"];
			}
		}


		[ConfigurationProperty("extensions", IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(ExtensionsCollection), AddItemName = "add",
			ClearItemsName = "clear",
			RemoveItemName = "remove")]
		public ExtensionsCollection Extensions
		{
			get
			{
				return (ExtensionsCollection)base["extensions"];
			}
		}
	}

	internal class ExtensionsCollection : ConfigurationElementCollection<ExtensionConfigurationElement>
	{
		protected override object GetKey(ExtensionConfigurationElement element)
		{
			return element.Name;
		}
	}

	internal class ActionsCollection : ConfigurationElementCollection<ActionConfigurationElement>
	{
		protected override object GetKey(ActionConfigurationElement element)
		{
			return element.Name;
		}
	}

	internal class ExtensionConfigurationElement : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get { return (string)this["name"]; }
			set { this["name"] = value; }
		}

		[ConfigurationProperty("actions", IsRequired = true)]
		public string ActionsString
		{
			get { return (string)this["actions"]; }
			set { this["actions"] = value; }
		}

		public string[] Actions
		{
			get
			{
				var actionsString = ActionsString;
				if (actionsString != null)
					return actionsString.Split(',').Select(s => s.Trim()).ToArray();
				else
					return new string[0];
			}
		}
	}

	internal class ActionConfigurationElement : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get { return (string)this["name"]; }
			set { this["name"] = value; }
		}

		[ConfigurationProperty("text", IsRequired = true)]
		public string Text
		{
			get { return (string)this["text"]; }
			set { this["text"] = value; }
		}

		[ConfigurationProperty("command", IsRequired = true)]
		public string Command
		{
			get { return (string)this["command"]; }
			set { this["command"] = value; }
		}

		[ConfigurationProperty("params", IsRequired = false)]
		public string Params
		{
			get { return (string)this["params"]; }
			set { this["params"] = value; }
		}
	}

	internal abstract class ConfigurationElementCollection<T> : ConfigurationElementCollection where T : ConfigurationElement, new()
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