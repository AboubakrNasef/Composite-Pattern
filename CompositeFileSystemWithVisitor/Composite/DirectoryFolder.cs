using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompositeFileSystemWithVisitor.VisitorApproach;

namespace CompositeFileSystem.Composite
{
    public class DirectoryFolder : DirectoryElement
    {
        public DirectoryFolder(string name) : base(name)
        {
        }

        public override ElementType Type => ElementType.Folder;
		public int FilesCount { get; private set; }
		#region CompositeRelated
		public override void Add(DirectoryElement element)
        {
            _children.Add(element);
			if (element.Type == ElementType.File)
			{
				FilesCount++;
			}
        }
        public override bool Remove(DirectoryElement element)
        {
            var result = _children.Remove(element);
			if (result && element.Type == ElementType.File)
			{
				FilesCount--;
			}
			return result;
        }
		#endregion

		#region Visitor
		public override bool Accept(IDirectoryElementVisitor visitor)
		{
			visitor.Visit(this);
			foreach (var child in _children)
			{
				child.Accept(visitor);
			}
			return true;
		}
		#endregion
	}
}
