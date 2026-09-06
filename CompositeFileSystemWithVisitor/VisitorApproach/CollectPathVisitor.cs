using System;
using System.Collections.Generic;
using System.Text;
using CompositeFileSystem.Composite;

namespace CompositeFileSystemWithVisitor.VisitorApproach
{
	internal class CollectPathVisitor : IDirectoryElementVisitor
	{
		Dictionary<string, string> paths = new();
		public IReadOnlyDictionary<string, string> Paths => paths;
		string currentPath = "";

		public void Visit(DirectoryFile file)
		{
			paths[file.Name] = $"{currentPath}/{file.Name}";
		}

		public void Visit(DirectoryFolder folder)
		{
			if (currentPath == "")
				currentPath = folder.Name;
			else
				currentPath = $"{currentPath}/{folder.Name}";
			paths[folder.Name] = currentPath;
		}
	}
}
