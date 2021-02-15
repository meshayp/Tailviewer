﻿using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Tailviewer.Archiver.Plugins;
using Tailviewer.BusinessLogic.LogFiles;
using Tailviewer.Core.LogFiles;
using Tailviewer.Ui.Properties;

namespace Tailviewer.Ui.Controls.SidePanel.Outline
{
	public sealed class PropertyPresenterRegistry
		: IPropertyPresenterPlugin
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly IReadOnlyList<IPropertyPresenterPlugin> _plugins;
		private readonly IReadOnlyDictionary<IReadOnlyPropertyDescriptor, string> _wellKnownDisplayNames;
		private readonly IReadOnlyDictionary<IReadOnlyPropertyDescriptor, Func<string, IPropertyPresenter>> _wellKnownPresenters;

		public PropertyPresenterRegistry(IPluginLoader pluginSystem)
		{
			_plugins = pluginSystem.LoadAllOfType<IPropertyPresenterPlugin>();

			_wellKnownDisplayNames = new Dictionary<IReadOnlyPropertyDescriptor, string>
			{
				{ Core.LogFiles.Properties.LogEntryCount, "Count" },
				{ Core.LogFiles.Properties.Name, "Name" },
				{ Core.LogFiles.Properties.StartTimestamp, "First Timestamp" },
				{ Core.LogFiles.Properties.EndTimestamp, "Last Timestamp" },
				{ Core.LogFiles.Properties.Duration, "Duration" },
				{ Core.LogFiles.Properties.LastModified, "Last Modified" },
				{ Core.LogFiles.Properties.Created, "Created" },
				{ Core.LogFiles.Properties.Size, "Size" },

				{ Core.LogFiles.Properties.PercentageProcessed, "Processed" },
				{ Core.LogFiles.Properties.Format, "Format" },
				{ Core.LogFiles.Properties.Encoding, "Encoding" }
			};
			_wellKnownPresenters = new Dictionary<IReadOnlyPropertyDescriptor, Func<string, IPropertyPresenter>>
			{
				{Core.LogFiles.Properties.Encoding, displayName => new EncodingPropertyPresenter(displayName)}
			};
		}

		#region Implementation of IPropertyPresenterPlugin

		public IPropertyPresenter TryCreatePresenterFor(IReadOnlyPropertyDescriptor property)
		{
			foreach (var plugin in _plugins)
			{
				try
				{
					var presenter = plugin.TryCreatePresenterFor(property);
					if (presenter != null)
						return presenter;
				}
				catch (Exception e)
				{
					Log.WarnFormat("Caught unexpected exception: {0}", e);
				}
			}

			if (property is IWellKnownReadOnlyPropertyDescriptor wellKnownProperty)
			{
				if (!_wellKnownDisplayNames.TryGetValue(property, out var displayName))
					return null; //< Well known properties without a display name are not intended to be shown...

				if (_wellKnownPresenters.TryGetValue(property, out var factory))
					return factory(displayName); //< For some properties, we offer specialized presenters

				return new DefaultPropertyPresenter(displayName); //< But for most, the default will do
			}

			// As far as other properties are concerned, we will just display them.
			return new DefaultPropertyPresenter(property.Id);
		}

		#endregion
	}
}
