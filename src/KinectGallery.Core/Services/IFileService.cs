using System.Collections.Generic;
using System.Threading.Tasks;
using KinectGallery.Core.Models;

namespace KinectGallery.Core.Services
{
	public interface IFileService
	{
		Task<IEnumerable<Element>> GetElements(string folderPath);
	}
}
