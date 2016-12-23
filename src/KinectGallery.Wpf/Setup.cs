using System.Windows.Threading;
using KinectGallery.Core.Services;
using KinectGallery.Wpf.Services.Impl;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Wpf.Platform;
using MvvmCross.Wpf.Views;

namespace KinectGallery.Wpf
{
	public class Setup : MvxWpfSetup
	{
		public Setup(Dispatcher uiThreadDispatcher, IMvxWpfViewPresenter presenter) 
			: base(uiThreadDispatcher, presenter)
		{
		}

		protected override IMvxApplication CreateApp()
			=> new Core.App();

		protected override void InitializeFirstChance()
		{
			base.InitializeFirstChance();

			Mvx.LazyConstructAndRegisterSingleton<IFileService>(() => new FileServiceWpf());
			Mvx.LazyConstructAndRegisterSingleton<ISpecialFolderPaths>(() => new SpecialFolderPathsWpf());
		}
	}
}
