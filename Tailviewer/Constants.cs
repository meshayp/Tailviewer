﻿using System;
using System.Reflection;

namespace Tailviewer
{
	public static class Constants
	{
		public static readonly string ApplicationTitle;
		public static readonly string MainWindowTitle;
		public static readonly Version ApplicationVersion;

		static Constants()
		{
			ApplicationTitle = "Tailviewer";
			ApplicationVersion = Assembly.GetExecutingAssembly().GetName().Version;
			MainWindowTitle = string.Format("Tailviewer, v{0}", ApplicationVersion);
		}
	}
}