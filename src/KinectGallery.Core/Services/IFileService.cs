using System.Collections.Generic;
using KinectGallery.Core.Models;

namespace KinectGallery.Core.Services
{
	public interface IFileService
	{
		IEnumerable<Element> GetElements();
	}
}
