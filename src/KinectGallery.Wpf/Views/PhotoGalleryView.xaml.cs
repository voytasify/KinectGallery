using System;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using MvvmCross.Wpf.Views;

namespace KinectGallery.Wpf.Views
{
	public partial class PhotoGalleryView : MvxWpfView
	{
		public PhotoGalleryView()
		{
			InitializeComponent();

			Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
			{
				var navWindow = Window.GetWindow(this) as NavigationWindow;
				if (navWindow != null)
					navWindow.ShowsNavigationUI = false;
			}));
		}
	}
}
