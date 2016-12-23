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

			SelectCommand = new MvxAsyncCommand(SelectAction);
			CloseCommand = new MvxCommand(CloseAction);
		}

		public override async void Start()
		{
			Elements = await _fileService.GetElements(_specialFolderPaths.GetFolderPath(SpecialFolderType.MyPictures));
		}

		private bool _zoomMode;
		public bool ZoomMode
		{
			get { return _zoomMode; }
			set
			{
				_zoomMode = value;
				RaisePropertyChanged(() => ZoomMode);
			}
		}

		private Element _selectedElement;
		public Element SelectedElement
		{
			get { return _selectedElement; }
			set
			{
				_selectedElement = value;
				RaisePropertyChanged(() => SelectedElement);
			}
		}

		private IEnumerable<Element> _elements;
		public IEnumerable<Element> Elements
		{
			get { return _elements; }
			set
			{
				_elements = value;
				RaisePropertyChanged(() => Elements);
			}
		}

		public IMvxCommand SelectCommand { get; private set; }
		private async Task SelectAction()
		{
			if (SelectedElement is ImageElement)
				ZoomMode = true;
			else if (SelectedElement is DirectoryElement)
				Elements = await _fileService.GetElements(SelectedElement.FullName);
		}

		public IMvxCommand CloseCommand { get; private set; }
		private void CloseAction()
			=> ZoomMode = false;
	}
}
