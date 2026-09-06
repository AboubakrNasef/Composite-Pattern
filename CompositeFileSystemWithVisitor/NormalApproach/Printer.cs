using System;
using System.Collections.Generic;
using System.Text;
using CompositeFileSystem.Composite;

namespace CompositeFileSystemWithVisitor.NormalApproach
{
	internal static class NormalPrinter
	{
		public static void Print(DirectoryElement directoryElement)
		{

			switch (directoryElement)
			{
				case DirectoryFile file:
					Console.WriteLine(file.Name);
					break;
				case DirectoryFolder folder:
					Console.WriteLine($"{folder.Name} -> ({folder.FilesCount} files)");
					foreach (var child in directoryElement.Children)
					{
						Print(child);
					}
					break;
				default:
					break;
			}
		}
	}
}
