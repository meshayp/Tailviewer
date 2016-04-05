﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Tailviewer.Ui.Converters
{
	internal sealed class TimeSpanConverter
		: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is TimeSpan))
				return null;

			var age = (TimeSpan) value;
			TimeSpan oneMonth = TimeSpan.FromDays(30);
			TimeSpan oneWeek = TimeSpan.FromDays(7);
			TimeSpan oneDay = TimeSpan.FromDays(1);
			TimeSpan oneHour = TimeSpan.FromHours(1);
			TimeSpan oneMinute = TimeSpan.FromMinutes(1);
			TimeSpan oneSecond = TimeSpan.FromSeconds(1);

			if (age > oneMonth)
				return Format(age, oneMonth, "day");
			if (age > oneWeek)
				return Format(age, oneWeek, "week");
			if (age > oneDay)
				return Format(age, oneDay, "day");
			if (age > oneHour)
				return Format(age, oneHour, "hour");
			if (age > oneMinute)
				return Format(age, oneMinute, "minute");

			return Format(age, oneSecond, "second");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}

		private object Format(TimeSpan value, TimeSpan divider, string caption)
		{
			var number = (int) (value.TotalMilliseconds/divider.TotalMilliseconds);
			if (number == 1)
				return string.Format("{0} {1}", number, caption);

			return string.Format("{0} {1}s", number, caption);
		}
	}
}