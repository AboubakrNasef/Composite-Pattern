
using CompositeFileSystem.Composite;
using System.IO;
using System.Xml.Linq;
using Zu.TypeScript.TsTypes;
using Zu.TypeScript;

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
        var directoryFile = new DirectoryFile(fileName, file);
        rootFolder.Add(directoryFile);

        if (Path.GetExtension(fileName) == ".ts")
        {
            var ast = new TypeScriptAST(File.ReadAllText(file), fileName);
            var classes = ast.OfKind(SyntaxKind.ClassDeclaration);
            foreach (var cl in classes)
            {
                var fileClass = new DirectoryFileClass(cl.IdentifierStr, file);
                directoryFile.Add(fileClass);
            }
        }
    }

    var directories = Directory.GetDirectories(path);
    foreach (var dir in directories)
    {
        rootFolder.Add(CreateDirectoryStructure(dir));
    }

    return rootFolder;
}