using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KinectGallery.Core.Extensions;
using KinectGallery.Core.Models;
using KinectGallery.Core.Services;

namespace KinectGallery.Wpf.Services.Impl
{
	public class FileServiceWpf : IFileService
	{
		private string Path => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

		public IEnumerable<Element> GetElements()
		{
			var elements = new List<Element>();
			AddAllElements(new DirectoryInfo(Path), elements);
			return elements;
		}

		private void AddAllElements(DirectoryInfo directory, ICollection<Element> elements, int n = 0)
		{
			foreach (var dir in directory.GetDirectories())
				AddAllElements(dir, elements, n + 1);

			foreach (var file in directory.GetFiles().Where(f => f.Extension.IsImageExtension()))
				elements.Add(new ImageElement(file.Name, file.FullName));

			if (n > 0)
				elements.Add(new DirectoryElement(directory.Name, directory.FullName));
		}
	}
}
