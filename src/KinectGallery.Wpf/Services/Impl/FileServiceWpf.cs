using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KinectGallery.Core.Extensions;
using KinectGallery.Core.Models;
using KinectGallery.Core.Services;

namespace KinectGallery.Wpf.Services.Impl
{
	public class FileServiceWpf : IFileService
	{
		public async Task<IEnumerable<Element>> GetElements(string folderPath)
		{
			return await Task.Run(() =>
			{
				var elements = new List<Element>();
				var directory = new DirectoryInfo(folderPath);

				var parentDirectory = directory.Parent;
				if(parentDirectory != null)
					elements.Add(new RootDirectoryElement(parentDirectory.Name, parentDirectory.FullName));

				foreach (var dir in directory.GetDirectories())
					elements.Add(new DirectoryElement(dir.Name, dir.FullName));

				foreach (var file in directory.GetFiles().Where(f => f.Extension.IsImageExtension()))
					elements.Add(new ImageElement(file.Name, file.FullName));

				return elements;
			});
		}
	}
}
