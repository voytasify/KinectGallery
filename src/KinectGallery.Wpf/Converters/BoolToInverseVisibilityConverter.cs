﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KinectGallery.Wpf.Converters
{
	public class BoolToInverseVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> (bool)value ? Visibility.Collapsed : Visibility.Visible;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
