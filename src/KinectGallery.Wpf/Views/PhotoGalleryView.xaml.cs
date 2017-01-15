using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using System.Windows.Threading;
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
		private bool _kinectSetupComplete;

		public PhotoGalleryView()
		{
			InitializeComponent();
			Loaded += OnLoaded;

			Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
			{
				var navWindow = Window.GetWindow(this) as NavigationWindow;
				if (navWindow != null)
					navWindow.ShowsNavigationUI = false;
			}));

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
			if (_kinectSetupComplete == false)
				DoKinectSetup();
		}

		private void DoKinectSetup()
		{
			var sensor = SensorExtensions.Default();
			if (sensor == null)
				return;

			sensor.EnableAllStreams();
			sensor.SkeletonFrameReady += SensorOnSkeletonFrameReady;

			_gestureController = new GestureController(GestureType.All);
			_gestureController.GestureRecognized += GestureControllerOnGestureRecognized;

			sensor.Start();

			_kinectSetupComplete = true;
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
			GestureNameTextBlock.Text = e.Name;

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
