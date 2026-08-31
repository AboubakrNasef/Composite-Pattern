namespace CSharpComposite.Composite
{
	public class CSHP_Method : DirectoryElement
	{
		public string ReturnType { get; set; }
		public List<string> Parameters { get; set; } = [];

		public CSHP_Method(string name, string returnType) : base(name)
		{
			ReturnType = returnType ?? "void";
		}

		public override ElementType Type => ElementType.CSHP_Method;
	}
}
