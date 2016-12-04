using System;
using KinectGallery.Core.Enums;
using KinectGallery.Core.Services;

namespace KinectGallery.Wpf.Services.Impl
{
	public class SpecialFolderPathsWpf : ISpecialFolderPaths
	{
		public string GetFolderPath(SpecialFolderType folderType)
		{
			switch (folderType)
			{
				case SpecialFolderType.MyPictures:
					return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
				default:
					throw new ArgumentOutOfRangeException(nameof(folderType), folderType, null);
			}
		}
	}
}
