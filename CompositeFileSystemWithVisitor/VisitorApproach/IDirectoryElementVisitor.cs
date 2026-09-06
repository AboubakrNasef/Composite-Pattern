using System;
using System.Collections.Generic;
using System.Text;
using CompositeFileSystem.Composite;

namespace CompositeFileSystemWithVisitor.VisitorApproach
{
	public interface IDirectoryElementVisitor
	{
		void Visit(DirectoryFile file);
		void Visit(DirectoryFolder folder);
	}
}
