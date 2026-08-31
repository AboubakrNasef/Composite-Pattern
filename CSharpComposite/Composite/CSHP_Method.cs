namespace CSharpComposite.Composite
{
	public class CSHP_Method : DirectoryElement
	{
		public List<string> Parameters { get; set; } = [];

		public CSHP_Method(string name, string returnType) : base(name)
		{
		}

		public override ElementType Type => ElementType.CSHP_Method;

		public override void Add(DirectoryElement element)
		{
			_children.Add(element);
		}

		public override bool Remove(DirectoryElement element)
		{
			return _children.Remove(element);
		}
	}
}
