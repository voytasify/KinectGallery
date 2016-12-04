namespace KinectGallery.Core.Extensions
{
	public static class StringExtensions
	{
		public static bool IsImageExtension(this string extension)
			=> extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp";
	}
}
