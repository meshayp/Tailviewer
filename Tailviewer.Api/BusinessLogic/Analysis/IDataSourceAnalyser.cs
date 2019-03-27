﻿using System;
using Tailviewer.BusinessLogic.LogFiles;

namespace Tailviewer.BusinessLogic.Analysis
{
	/// <summary>
	///     Represents a live analysis or a snapshot (depending on <see cref="IsFrozen" />) of a
	///     previous analysis.
	///     When this is a live analysis, then changes to <see cref="Configuration" /> are eventually
	///     forwarded to a <see cref="ILogAnalyser" />.
	/// </summary>
	public interface IDataSourceAnalyser
		: IDisposable
	{
		/// <summary>
		///     A unique id which identifies this analyser.
		/// </summary>
		AnalyserId Id { get; }

		/// <summary>
		///     The id of the factory which created this analyser.
		/// </summary>
		LogAnalyserFactoryId LogAnalyserPluginId { get; }

		/// <summary>
		///     The id of the factory which created this analyser.
		/// </summary>
		DataSourceAnalyserPluginId DataSourceAnalyserPluginId { get; }

		/// <summary>
		///     The current progress of the analysis.
		/// </summary>
		Percentage Progress { get; }

		/// <summary>
		///     The result of the analysis.
		///     May change over the lifetime of this analyser.
		///     May be null.
		/// </summary>
		ILogAnalysisResult Result { get; }

		/// <summary>
		///     Whether or not this analyser is frozen.
		///     A frozen analyser may not be modified and thus changing
		///     the configuration is not allowed.
		/// </summary>
		bool IsFrozen { get; }

		/// <summary>
		///     The current configuration used by the analyser.
		///     When the configuration is changed (by calling the setter),
		///     then the analysis is restarted using the new configuration
		///     and <see cref="Result" /> will eventually represent the result
		///     of the analysis using the new configuration.
		/// </summary>
		ILogAnalyserConfiguration Configuration { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="logFile"></param>
		void OnAddLogFile(ILogFile logFile);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="logFile"></param>
		void OnRemoveLogFile(ILogFile logFile);
	}
}