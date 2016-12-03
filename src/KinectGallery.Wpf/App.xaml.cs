﻿using System.Windows;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Wpf.Views;

namespace KinectGallery.Wpf
{
	public partial class App : Application
	{
		private bool _setupComplete;

		private void DoSetup()
		{
			var presenter = new MvxSimpleWpfViewPresenter(MainWindow);

			var setup = new Setup(Dispatcher, presenter);
			setup.Initialize();

			var start = Mvx.Resolve<IMvxAppStart>();
			start.Start();

			_setupComplete = true;
		}

		protected override void OnActivated(System.EventArgs e)
		{
			if (_setupComplete == false)
				DoSetup();

			base.OnActivated(e);
		}
	}
}
