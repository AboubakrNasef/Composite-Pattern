
using CompositeFileSystem.Composite;

var folderPath = @"C:\_Aboubakr\TechTalk\DesignPatterns\D1\angular\factory\src";

var ParentFolder = CreateDirectoryStructure(folderPath);
Console.WriteLine("--");

#region Print
var printer = new DirectoryPrinter();
ParentFolder.Accept(printer);
Console.WriteLine();
#endregion


DirectoryElement CreateDirectoryStructure(string folderPath)
{
    string directoryName = Path.GetFileName(folderPath.TrimEnd(Path.DirectorySeparatorChar));
    var rootFolder = new DirectoryFolder(directoryName);
    var files = Directory.GetFiles(folderPath);
    foreach (var file in files)
    {
        string fileName = Path.GetFileName(file);
        rootFolder.Add(new DirectoryFile(fileName));
    }

    var directories = Directory.GetDirectories(folderPath);
    foreach (var dir in directories)
    {
        rootFolder.Add(CreateDirectoryStructure(dir));
    }

    return rootFolder;
}