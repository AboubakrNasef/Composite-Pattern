
using CompositeFileSystem.Composite;

var folderPath = @"C:\_Aboubakr\TechTalk\DesignPatterns\D1\angular\factory\src";

var ParentFolder = CreateDirectoryStructure(folderPath);

#region Print
var printer = new DirectoryPrinter();
ParentFolder.Accept(printer);
Console.WriteLine();
#endregion


DirectoryFolder CreateDirectoryStructure(string path)
{
    string directoryName = Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar));
    var rootFolder = new DirectoryFolder(directoryName);
    var files = Directory.GetFiles(path);
    foreach (var file in files)
    {
        string fileName = Path.GetFileName(file);
        rootFolder.Add(new DirectoryFile(fileName));
    }

    var directories = Directory.GetDirectories(path);
    foreach (var dir in directories)
    {
        rootFolder.Add(CreateDirectoryStructure(dir));
    }

    return rootFolder;
}