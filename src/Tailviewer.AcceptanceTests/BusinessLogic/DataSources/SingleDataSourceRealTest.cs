﻿using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using Tailviewer.AcceptanceTests.BusinessLogic.LogFiles;
using Tailviewer.BusinessLogic;
using Tailviewer.BusinessLogic.DataSources;
using Tailviewer.BusinessLogic.LogFiles;
using Tailviewer.Core.Filters;
using Tailviewer.Core.LogFiles;
using Tailviewer.Settings;
using Tailviewer.Test;

namespace Tailviewer.AcceptanceTests.BusinessLogic.DataSources
{
	[TestFixture]
	public sealed class SingleDataSourceRealTest
	{
		[SetUp]
		public void SetUp()
		{
			_scheduler = new DefaultTaskScheduler();
			_logFileFactory = new SimplePluginLogFileFactory(_scheduler);
			_settings = new DataSource(TextLogFileAcceptanceTest.File20Mb)
			{
				Id = DataSourceId.CreateNew()
			};
			_dataSource = new SingleDataSource(_logFileFactory, _scheduler, _settings, TimeSpan.FromMilliseconds(100));
		}

		[TearDown]
		public void TearDown()
		{
			_dataSource.Dispose();
			_scheduler.Dispose();
		}

		private DataSource _settings;
		private SingleDataSource _dataSource;
		private DefaultTaskScheduler _scheduler;
		private ILogFileFactory _logFileFactory;

		[Test]
		public void TestCtor()
		{
			_dataSource.FilteredLogFile.Property(x => x.GetProperty(Properties.PercentageProcessed)).ShouldAfter(TimeSpan.FromSeconds(15)).Be(Percentage.HundredPercent);

			_dataSource.UnfilteredLogFile.Should().NotBeNull();
			_dataSource.FilteredLogFile.Should().NotBeNull();

			_dataSource.Property(x => x.UnfilteredLogFile.GetProperty(Properties.LogEntryCount)).ShouldEventually().Be(165342);
			_dataSource.Property(x => x.FilteredLogFile.GetProperty(Properties.LogEntryCount)).ShouldEventually().Be(165342);
		}

		[Test]
		public void TestLevelFilter1()
		{
			_dataSource.LevelFilter = LevelFlags.Info;
			_dataSource.FilteredLogFile.Should().NotBeNull();
			_dataSource.FilteredLogFile.Property(x => x.GetProperty(Properties.PercentageProcessed)).ShouldAfter(TimeSpan.FromSeconds(15)).Be(Percentage.HundredPercent);

			// TODO: Find the bug in the EndOfSourceReached implementation!!!!
			Thread.Sleep(1000);

			_dataSource.FilteredLogFile.GetProperty(Properties.LogEntryCount).Should().Be(5);
		}

		[Test]
		public void TestStringFilter1()
		{
			_dataSource.UnfilteredLogFile.Property(x => x.GetProperty(Properties.PercentageProcessed))
					   .ShouldEventually().Be(Percentage.HundredPercent);

			_dataSource.QuickFilterChain = new[] {new SubstringFilter("info", true)};
			_dataSource.FilteredLogFile.Should().NotBeNull();

			_dataSource.FilteredLogFile.Property(x => x.GetProperty(Properties.PercentageProcessed)).ShouldAfter(TimeSpan.FromSeconds(15)).Be(Percentage.HundredPercent);

			_dataSource.FilteredLogFile.GetProperty(Properties.LogEntryCount).Should().Be(5);
		}

		[Test]
		[FlakyTest(3)]
		[Description("Verifies that the levels are counted correctly")]
		public void TestLevelCount1()
		{
			_dataSource.FilteredLogFile.Property(x => x.GetProperty(Properties.PercentageProcessed)).ShouldAfter(TimeSpan.FromSeconds(15)).Be(Percentage.HundredPercent);

			_dataSource.Property(x => x.TotalCount).ShouldEventually().Be(165342);
			_dataSource.DebugCount.Should().Be(165337);
			_dataSource.InfoCount.Should().Be(5);
			_dataSource.WarningCount.Should().Be(0);
			_dataSource.ErrorCount.Should().Be(0);
			_dataSource.FatalCount.Should().Be(0);
		}
	}
}