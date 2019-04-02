using System;
using Tailviewer.BusinessLogic.Plugins;

namespace Tailviewer.Archiver.Plugins.Description
{
	/// <summary>
	///     Describes one implementation of <see cref="IPlugin" />.
	/// </summary>
	public sealed class PluginImplementationDescription
		: IPluginImplementationDescription
	{
		public PluginImplementationDescription(string fullTypeName, Type @interface)
		{
			FullTypeName = fullTypeName;
			Version = PluginInterfaceVersionAttribute.GetInterfaceVersion(@interface);
		}

		public PluginImplementationDescription(PluginInterfaceImplementation description)
		{
			FullTypeName = description.ImplementationTypename;
			Version = new PluginInterfaceVersion(description.InterfaceVersion);
		}

		#region Implementation of IPluginImplementationDescription

		public string FullTypeName { get; set; }

		public PluginInterfaceVersion Version { get; set; }

		#endregion
	}
}