﻿using System.Collections.Generic;
using System.Linq;
using Tailviewer.Archiver.Plugins;
using Tailviewer.BusinessLogic.Plugins;

namespace Tailviewer.BusinessLogic.LogFiles
{
	/// <summary>
	///     Responsible for matching a log file to a particular format, if possible.
	/// </summary>
	internal sealed class LogFileFormatMatcher
		: ILogFileFormatMatcher
	{
		private readonly IReadOnlyList<ILogFileFormatMatcher> _matchers;

		public LogFileFormatMatcher(IPluginLoader pluginLoader)
		{
			_matchers = pluginLoader.LoadAllOfType<ILogFileFormatMatcherPlugin>()
			                        .Select(x => new LogFileFormatMatcherProxy(x.CreateMatcher()))
			                        .ToList();
		}

		#region Implementation of ILogFileFormatMatcher

		public bool TryMatchFormat(string fileName, byte[] initialContent, out ILogFileFormat format)
		{
			foreach (var matcher in _matchers)
				if (matcher.TryMatchFormat(fileName, initialContent, out format))
					return true;

			format = null;
			return false;
		}

		public bool TryMatchFormat(string fileName, IReadOnlyList<string> initialContent, out ILogFileFormat format)
		{
			foreach (var matcher in _matchers)
				if (matcher.TryMatchFormat(fileName, initialContent, out format))
					return true;

			format = null;
			return false;
		}

		#endregion
	}
}