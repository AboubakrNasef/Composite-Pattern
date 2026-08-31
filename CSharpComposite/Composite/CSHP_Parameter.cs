namespace CSharpComposite.Composite
{
	public class CSHP_Parameter : DirectoryElement
	{
		public string ParameterType { get; set; }

		public CSHP_Parameter(string name, string parameterType) : base(name)
		{
			ParameterType = parameterType;
		}

		public override ElementType Type => ElementType.CSHP_Parameter;
	}
}
