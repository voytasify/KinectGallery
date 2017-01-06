using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace KinectGallery.Wpf.Views
{
	public partial class PhotoGalleryView
	{
		public PhotoGalleryView()
		{
			InitializeComponent();

			ImageListView.ItemContainerGenerator.StatusChanged += (sender, args) =>
			{
				var listView = ImageListView;
				if (listView.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
				{
					var index = listView.SelectedIndex;
					if (index < 0)
						return;

					var item = listView.ItemContainerGenerator.ContainerFromIndex(listView.SelectedIndex) as ListViewItem;
					item?.Focus();
				}
			};

			ImageListView.SelectionChanged += (sender, args) =>
			{
				var listView = ImageListView;
				listView.ScrollIntoView(listView.SelectedItem);
			};
		}
	}
}
