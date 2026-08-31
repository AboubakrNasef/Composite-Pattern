namespace CSharpComposite.Composite
{
	public class CSHP_File : DirectoryElement
	{
		public CSHP_File(string name) : base(name)
		{
		}

		public override ElementType Type => ElementType.CSHP_File;

		#region CompositeRelated
		public override void Add(DirectoryElement element)
		{
			_children.Add(element);
		}
		public override bool Remove(DirectoryElement element)
		{
			return _children.Remove(element);
		}
		#endregion
	}
}
