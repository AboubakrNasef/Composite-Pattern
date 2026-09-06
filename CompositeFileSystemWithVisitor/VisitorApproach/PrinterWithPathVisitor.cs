using System;
using System.Collections.Generic;
using System.Text;
using CompositeFileSystem.Composite;

namespace CompositeFileSystemWithVisitor.VisitorApproach
{
	internal class PrinterWithPathVisitor(IReadOnlyDictionary<string,string> paths) : IDirectoryElementVisitor
	{
		StringBuilder _sb = new StringBuilder();
		public string GetResult()
		{
			return _sb.ToString();
		}
		public void Visit(DirectoryFile file)
		{
			_sb.AppendLine($"File: {file.Name} - Path: {paths[file.Name]}");
		}

		public void Visit(DirectoryFolder folder)
		{
			_sb.AppendLine($"Folder: {folder.Name} - files: {folder.FilesCount} - Path: {paths[folder.Name]}");
		}
	}
}
