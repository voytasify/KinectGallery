using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using KinectGallery.Core.Extensions;
using KinectGallery.Core.ViewModels;
using LightBuzz.Vitruvius;
using Microsoft.Kinect;

namespace KinectGallery.Wpf.Views
{
	public partial class PhotoGalleryView
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
			switch (e.Type)
			{
				case GestureType.JoinedHands:
					Vm.StopScrollingCommand.ExecuteIfCan();
					break;
				case GestureType.SwipeLeft:
					Vm.ScrollLeftCommand.ExecuteIfCan();
					break;
				case GestureType.SwipeRight:
					Vm.ScrollRightCommand.ExecuteIfCan();
					break;
				case GestureType.SwipeUp:
					Vm.StartScrollingRightCommand.ExecuteIfCan();
					break;
				case GestureType.SwipeDown:
					Vm.StartScrollingLeftCommand.ExecuteIfCan();
					break;
				case GestureType.ZoomIn:
					Vm.SelectCommand.ExecuteIfCan();
					break;
				case GestureType.ZoomOut:
					Vm.CloseCommand.ExecuteIfCan();
					break;
			}
		}

		protected PhotoGalleryViewModel Vm => (PhotoGalleryViewModel) ViewModel;
	}
}
