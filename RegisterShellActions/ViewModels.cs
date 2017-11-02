using System.Collections.Generic;

namespace RegisterShellActions
{
	public class ExtensionViewModel
	{
		public string Key { get; set; }
		public string Name
		{
			get { return Key.ToUpper() + " file"; }
		} 
		public IEnumerable<ActionViewModel> Actions { get; set; }
	}

	public class ActionViewModel
	{
		public string Key { get; set; }
		public string Text { get; set; }
		public bool Selected { get; set; }
		public bool Registered { get; set; }
	}
}