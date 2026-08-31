namespace CSharpComposite.Composite
{
	public class CSHP_Property : DirectoryElement
	{
		public string PropertyType { get; set; }
		public bool HasGetter { get; set; }
		public bool HasSetter { get; set; }

		public CSHP_Property(string name, string propertyType, bool hasGetter = true, bool hasSetter = true) : base(name)
		{
			PropertyType = propertyType;
			HasGetter = hasGetter;
			HasSetter = hasSetter;
		}

		public override ElementType Type => ElementType.CSHP_Property;
	}
}
