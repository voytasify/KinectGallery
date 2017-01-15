using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using KinectGallery.Core.Extensions;
using KinectGallery.Core.ViewModels;
using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using MvvmCross.Wpf.Views;

namespace KinectGallery.Wpf.Views
{
	public partial class PhotoGalleryView : MvxWpfView
	{
		private GestureController _gestureController;

		public PhotoGalleryView()
		{
			InitializeComponent();
			Loaded += OnLoaded;

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

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			var sensor = SensorExtensions.Default();
			if (sensor == null)
				return;

			sensor.EnableAllStreams();
			sensor.SkeletonFrameReady += SensorOnSkeletonFrameReady;

			_gestureController = new GestureController(GestureType.All);
			_gestureController.GestureRecognized += GestureControllerOnGestureRecognized;

			sensor.Start();
		}

		private void SensorOnSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
		{
			using (var frame = e.OpenSkeletonFrame())
			{
				if (frame == null)
					return;

				var skeletons = frame.Skeletons().Where(s => s.TrackingState == SkeletonTrackingState.Tracked);
				foreach (var skeleton in skeletons)
					_gestureController.Update(skeleton);
			}
		}

		private void GestureControllerOnGestureRecognized(object sender, GestureEventArgs e)
		{
			switch (e.Name)
			{
				case "JoinedHands":
					ViewModel.StopScrollingCommand.ExecuteIfCan();
					break;
				case "SwipeLeft":
					ViewModel.ScrollLeftCommand.ExecuteIfCan();
					break;
				case "SwipeRight":
					ViewModel.ScrollRightCommand.ExecuteIfCan();
					break;
				case "SwipeUp":
					ViewModel.StartScrollingRightCommand.ExecuteIfCan();
					break;
				case "SwipeDown":
					ViewModel.StartScrollingLeftCommand.ExecuteIfCan();
					break;
				case "ZoomIn":
					ViewModel.SelectCommand.ExecuteIfCan();
					break;
				case "ZoomOut":
					ViewModel.CloseCommand.ExecuteIfCan();
					break;
			}
		}

		public new PhotoGalleryViewModel ViewModel
		{
			get { return (PhotoGalleryViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}
	}
}
