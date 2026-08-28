
using CompositeFileSystem.Composite;
 HashSet<string> ignores = new HashSet<string> { ".git", ".vs", "bin", "obj", "node_modules" };
Console.WriteLine("Composite Design Pattern - File System Example");
Console.WriteLine("EnterFolderPath:");
try
{


var folderPath =Console.ReadLine();

var ParentFolder = CreateDirectoryStructure(folderPath);
Console.WriteLine("  ");

#region Print
var printer = new DirectoryPrinter(Console.CursorLeft, Console.CursorTop);
ParentFolder.Accept(printer);
Console.WriteLine();
	#endregion
}
catch (Exception)
{

	throw;
}
finally
{
	Console.WriteLine("Press any key to exit...");
	Console.ReadKey();
	Console.ResetColor();
}

DirectoryElement CreateDirectoryStructure(string folderPath)
{
    string directoryName = Path.GetFileName(folderPath.TrimEnd(Path.DirectorySeparatorChar));
	DirectoryInfo rootDir = new DirectoryInfo(folderPath);

	var rootFolder = new DirectoryFolder(directoryName);
    var files = Directory.GetFiles(folderPath);
    foreach (var file in files)
    {
        string fileName = Path.GetFileName(file);
        rootFolder.Add(new DirectoryFile(fileName));
    }

    var directories = rootDir.GetDirectories().Where(d=>!ignores.Contains(d.Name, StringComparer.OrdinalIgnoreCase));
    foreach (var dir in directories)
    {
        rootFolder.Add(CreateDirectoryStructure(dir.FullName));
    }

    return rootFolder;
}