using System;
using System.Globalization;
using System.Windows.Data;
using KinectGallery.Core.Models;

namespace KinectGallery.Wpf.Converters
{
	public class ElementToImageSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> (value as ImageElement)?.FullName;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
