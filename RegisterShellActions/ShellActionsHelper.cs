using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace RegisterShellActions
{
	internal static class ShellActionsHelper
	{
		public static void Register(IEnumerable<Tuple<string, string>> extensionsActions)
		{
			foreach (var extensionAction in extensionsActions)
			{
				var extensionConfig = Configuration.Current.Extensions[extensionAction.Item1];
				if(extensionConfig != null)
				{
					var actionConfig = GetActionConfig(extensionAction.Item2);
					if(actionConfig != null)
						RegisterExtensionAction(extensionAction.Item1, actionConfig);
				}
			}
		}

		public static void Unregister(IEnumerable<Tuple<string, string>> extensionsActions)
		{
			foreach (var extensionAction in extensionsActions)
			{
				UnregisterExtensionAction(extensionAction.Item1, extensionAction.Item2);
			}
		}

		public static bool IsRegistered(string extension, string action)
		{
			var actionConfig = GetActionConfig(action);
			if (actionConfig == null)
				return false;

			var extensionKey = OpenExtensionRegistryKey(extension, RegistryKeyPermissionCheck.ReadSubTree);
			if (extensionKey == null)
				return false;

			var shellKey = extensionKey.OpenSubKey("shell");
			if (shellKey == null)
				return false;

			var actionKey = shellKey.OpenSubKey(actionConfig.Name);
			if (actionKey != null)
			{
				var actionText = actionKey.GetValue(null) as string;
				if (actionText != actionConfig.Text)
					return false;

				var commandKey = actionKey.OpenSubKey("command");
				if (commandKey != null)
				{
					return true;
				}
			}
			return false;
		}

		private static ActionConfigurationElement GetActionConfig(string actionName)
		{
			if (Configuration.Current.Actions == null)
				return null;

			return Configuration.Current.Actions[actionName];
		}

		private static RegistryKey OpenExtensionRegistryKey(string extension, RegistryKeyPermissionCheck check)
		{
			var extensionKeyName = "." + extension;
			RegistryKey extensionKey = Registry.ClassesRoot.OpenSubKey(extensionKeyName, check);
			if (extensionKey == null)
				return extensionKey;

			var extensionEntryName = extensionKey.GetValue(null) as string;
			if (!String.IsNullOrEmpty(extensionEntryName))
			{
				extensionKey = Registry.ClassesRoot.OpenSubKey(extensionEntryName, check);
				if (extensionKey != null)
					return extensionKey;
			}

			return null;
		}

		private static void RegisterExtensionAction(string extension, ActionConfigurationElement actionConfig)
		{
			if (String.IsNullOrEmpty(actionConfig.Command))
				return;

			var extensionKey = OpenExtensionRegistryKey(extension, RegistryKeyPermissionCheck.ReadWriteSubTree);
			if(extensionKey == null)
				return;
			
			var shellKey = extensionKey.CreateSubKey("shell");
			if(shellKey == null)
				return;

			var actionKey = shellKey.CreateSubKey(actionConfig.Name);
			if (actionKey != null)
			{
				actionKey.SetValue(null, actionConfig.Text);
				var commandKey = actionKey.CreateSubKey("command");
				if(commandKey != null)
				{
					var command = PrepareCommandText(actionConfig.Command, actionConfig.Params);
					if(command != null)
						commandKey.SetValue(null, command);
				}
			}
		}

		private static void UnregisterExtensionAction(string extension, string action)
		{
			var extensionKey = OpenExtensionRegistryKey(extension, RegistryKeyPermissionCheck.ReadWriteSubTree);
			if (extensionKey == null)
				return;

			var shellKey = extensionKey.OpenSubKey("shell", RegistryKeyPermissionCheck.ReadWriteSubTree);
			if (shellKey == null)
				return;

			shellKey.DeleteSubKeyTree(action);
		}

		private static string PrepareCommandText(string command, string parameters)
		{
			if (String.IsNullOrEmpty(command))
				return null;

			var commandPath = command;
			if(!Path.IsPathRooted(command))
			{
				var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				commandPath = Path.Combine(currentDir, command);
			}

			if(!String.IsNullOrEmpty(parameters))
			{
				var p = parameters.Trim().Replace('\'', '"');
				commandPath = String.Format("\"{0}\" {1}", commandPath, p);
			}

			return commandPath;
		}
	}
}