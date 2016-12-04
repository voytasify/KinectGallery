using KinectGallery.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;

namespace KinectGallery.Core
{
	public class App : MvxApplication
	{
		public override void Initialize()
		{
			base.Initialize();

			Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<PhotoGalleryViewModel>());
		}
	}
}
