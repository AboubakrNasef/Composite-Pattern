using System;
using System.Collections.Generic;
using System.Text;
using CompositeFileSystem.Composite;

namespace CompositeFileSystemWithVisitor.VisitorApproach
{
	internal class PrinterVisitor() : IDirectoryElementVisitor
	{
		StringBuilder _sb = new StringBuilder();
		public string GetResult()
		{
			return _sb.ToString();
		}
		public void Visit(DirectoryFile file)
		{
			_sb.AppendLine($"File: {file.Name}" );
		}

		public void Visit(DirectoryFolder folder)
		{
			_sb.AppendLine($"Folder: {folder.Name} - files: {folder.FilesCount}");
		}
	}
}

