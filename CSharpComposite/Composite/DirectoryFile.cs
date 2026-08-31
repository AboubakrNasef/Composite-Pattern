namespace CSharpComposite.Composite
{
	public class DirectoryFile : DirectoryElement
	{
		public DirectoryFile(string name) : base(name)
		{
		}

		public override ElementType Type => ElementType.File;
	}
}
