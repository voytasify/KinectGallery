using System.Windows;
using System.Windows.Controls;
using KinectGallery.Core.Models;

namespace KinectGallery.Wpf.Utils
{
	public class PhotoGalleryTemplateSelector : DataTemplateSelector
	{
		public DataTemplate ImageTemplate { get; set; }
		public DataTemplate DirectoryTemplate { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item is ImageElement)
				return ImageTemplate;
			if (item is DirectoryElement)
				return DirectoryTemplate;
			return null;
		}
	}
}
