namespace KinectGallery.Core.Models
{
	public abstract class Element
	{
		public string Name { get; set; }
		public string FullName { get; set; }

		protected Element(string name, string fullName)
		{
			Name = name;
			FullName = fullName;
		}
	}
}
