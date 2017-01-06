using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

			Func<bool> canExecute = () => SelectedElement != null && !Scrolling;
			ScrollLeftCommand = new MvxCommand(ScrollLeft, canExecute);
			ScrollRightCommand = new MvxCommand(ScrollRight, canExecute);
			StartScrollingLeftCommand = new MvxCommand(StartScrollingLeftAction, canExecute);
			StartScrollingRightCommand = new MvxCommand(StartScrollingRightAction, canExecute);

			StopScrollingCommand = new MvxCommand(StopScrollingAction, () => Scrolling);
			SelectCommand = new MvxAsyncCommand(SelectAction, () => !Scrolling);

			TokenSource = new CancellationTokenSource();
		}

		public override async void Start()
		{
			Elements = await _fileService.GetElements(_specialFolderPaths.GetFolderPath(SpecialFolderType.MyPictures));
			SelectedElement = Elements.ElementAtOrDefault(0);
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

		public IMvxCommand ScrollLeftCommand { get; private set; }

		private void ScrollLeft()
		{
			var elementBefore = Elements.TakeWhile(e => e.Name != SelectedElement.Name).LastOrDefault();
			if (elementBefore == null)
				return;

			SelectedElement = elementBefore;
		}

		public IMvxCommand ScrollRightCommand { get; private set; }

		private void ScrollRight()
		{
			var elementAfter = Elements.SkipWhile(e => e.Name != SelectedElement.Name).ElementAtOrDefault(1);
			if (elementAfter == null)
				return;

			SelectedElement = elementAfter;
		}

		public IMvxCommand StartScrollingLeftCommand { get; private set; }

		private void StartScrollingLeftAction()
		{
			Scrolling = true;

			if(TokenSource.IsCancellationRequested)
				TokenSource = new CancellationTokenSource();

			RotatePersonsAsync(ScrollDirection.Left, TokenSource.Token);
		}

		public IMvxCommand StartScrollingRightCommand { get; private set; }

		private void StartScrollingRightAction()
		{
			Scrolling = true;

			if (TokenSource.IsCancellationRequested)
				TokenSource = new CancellationTokenSource();

			RotatePersonsAsync(ScrollDirection.Right, TokenSource.Token);
		}

		public IMvxCommand StopScrollingCommand { get; private set; }
		private void StopScrollingAction()
		{
			Scrolling = false;
			TokenSource.Cancel();
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
		private void CloseAction() => ZoomMode = false;

		public bool Scrolling { get; private set; }
		public Timer Timer { get; private set; }
		public CancellationTokenSource TokenSource { get; private set; }

		private async Task RotatePersonsAsync(ScrollDirection direction, CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				switch (direction)
				{
					case ScrollDirection.Left:
						ScrollLeft();
						break;
					case ScrollDirection.Right:
						ScrollRight();
						break;
				}

				await Task.Delay(750, cancellationToken);
			}
		}
	}
}
