using System.Collections.Generic;
using System.Threading.Tasks;
using KinectGallery.Core.Enums;
using KinectGallery.Core.Models;
using KinectGallery.Core.Services;
using MvvmCross.Core.ViewModels;

namespace KinectGallery.Core.ViewModels
{
	public class PhotoGalleryViewModel : MvxViewModel
	{
		private readonly IFileService _fileService;
		private readonly ISpecialFolderPaths _specialFolderPaths;

		public PhotoGalleryViewModel(IFileService fileService, ISpecialFolderPaths specialFolderPaths)
		{
			_fileService = fileService;
			_specialFolderPaths = specialFolderPaths;

			SelectCommand = new MvxAsyncCommand<Element>(SelectAction);
		}

		public override async void Start()
		{
			Elements = await _fileService.GetElements(_specialFolderPaths.GetFolderPath(SpecialFolderType.MyPictures));
		}

		private IEnumerable<Element> _elements;
		public IEnumerable<Element> Elements
		{
			get { return _elements; }
			set
			{
				_elements = value;
				RaisePropertyChanged();
			}
		}

		public IMvxCommand SelectCommand { get; private set; }
		private async Task SelectAction(Element element)
		{
			if (element is ImageElement)
			{
				//TODO
			}
			else if (element is DirectoryElement)
			{
				Elements = await _fileService.GetElements(element.FullName);
			}
		}
	}
}
