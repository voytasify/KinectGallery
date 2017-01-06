using MvvmCross.Core.ViewModels;

namespace KinectGallery.Core.Extensions
{
	public static class CommandExtensions
	{
		public static void ExecuteIfCan(this IMvxCommand command)
		{
			if(command.CanExecute())
				command.Execute();
		}
	}
}
