using KinectGallery.Core.Enums;

namespace KinectGallery.Core.Services
{
	public interface ISpecialFolderPaths
	{
		string GetFolderPath(SpecialFolderType folderType);
	}
}
