using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompositeFileSystemWithVisitor.VisitorApproach;

namespace CompositeFileSystem.Composite
{
    public class DirectoryFile : DirectoryElement
    {
        public DirectoryFile(string name) : base(name)
        {
        }
         
        public override ElementType Type => ElementType.File;

		public override bool Accept(IDirectoryElementVisitor visitor)
		{
			visitor.Visit(this);
			return true;
		}
	}
}
