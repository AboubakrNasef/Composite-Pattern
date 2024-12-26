
using CompositeFileSystem.Composite;
using System.IO;
using System.Xml.Linq;
using Zu.TypeScript.TsTypes;
using Zu.TypeScript;

var folderPath = @"C:\_Aboubakr\TechTalk\DesignPatterns\D1\angular\factory\src";
try
{
    if (!Directory.Exists(folderPath))
    {
        Console.WriteLine($"Directory does not exist: {folderPath}");
        return;
    }

    var directoryStructure = CreateDirectoryStructure(folderPath);
    var printer = new DirectoryPrinter();
    directoryStructure.Accept(printer);

    Console.WriteLine();

}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}


DirectoryFolder CreateDirectoryStructure(string path)
{
    if (!Directory.Exists(path))
    {
        throw new DirectoryNotFoundException($"Directory not found: {path}");
    }

    string directoryName = Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar));
    if (string.IsNullOrEmpty(directoryName))
    {
        directoryName = path; // Use full path if we can't get the name (e.g., for root directories)
    }
    var rootFolder = new DirectoryFolder(directoryName, path);

    try
    {
        // Add files in current directory
        var files = Directory.GetFiles(path);
        foreach (var file in files)
        {
            string fileName = Path.GetFileName(file);
            var directoryFile = new DirectoryFile(fileName, file);
            rootFolder.Add(directoryFile);

            if (Path.GetExtension(fileName) == ".ts")
            {
                var ast = new TypeScriptAST(File.ReadAllText(file), fileName);
                var classes = ast.OfKind(SyntaxKind.ClassDeclaration);
                foreach (var cl in classes)
                {
                    var fileClass = new DirectoryFileClass(cl.IdentifierStr,  file);
                    directoryFile.Add(fileClass);
                }
            }
        }

        // Recursively add subdirectories
        var directories = Directory.GetDirectories(path);
        foreach (var dir in directories)
        {
            rootFolder.Add(CreateDirectoryStructure(dir));
        }
    }




    catch (UnauthorizedAccessException ex)
    {
        Console.WriteLine($"Access denied to some files or directories: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error accessing directory: {ex.Message}");
    }

    return rootFolder;
}