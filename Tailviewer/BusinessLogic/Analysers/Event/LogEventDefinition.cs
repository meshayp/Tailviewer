﻿using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Tailviewer.BusinessLogic.LogFiles;
using Tailviewer.Settings.Dashboard.Analysers.Event;

namespace Tailviewer.BusinessLogic.Analysers.Event
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class LogEventDefinition
	{
		private readonly Regex _regex;

		public LogEventDefinition(EventSettings settings)
			: this(settings.FilterExpression)
		{
			
		}

		public LogEventDefinition(string filterExpression)
		{
			_regex = new Regex(filterExpression);
		}

		[Pure]
		public object[] TryExtractEventFrom(LogLine logLine)
		{
			var match = _regex.Match(logLine.Message);
			if (!match.Success)
				return null;

			var captures = match.Captures;
			var count = captures.Count;
			var fields = new object[count];
			for (int i = 0; i < count; ++i)
			{
				fields[i] = captures[i];
			}

			return fields;
		}
	}
}