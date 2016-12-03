using System.Collections;
using System.Collections.Generic;
using KinectGallery.Core.Models;
using KinectGallery.Core.Services;
using MvvmCross.Core.ViewModels;

namespace KinectGallery.Core.ViewModels
{
	public class PhotoGalleryViewModel : MvxViewModel
	{
		private readonly IFileService _fileService;
		
		public IEnumerable<Element> Elements { get; set; }

		public PhotoGalleryViewModel(IFileService fileService)
		{
			_fileService = fileService;
		}

		public override void Start()
		{
			Elements = _fileService.GetElements();
		}
	}
}
