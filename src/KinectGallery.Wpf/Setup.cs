using System.Windows.Threading;
using MvvmCross.Core.ViewModels;
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

		protected override IMvxWpfViewsContainer CreateWpfViewsContainer()
		{
			var container = base.CreateWpfViewsContainer();
//			container
			return base.CreateWpfViewsContainer();
		}
	}
}
