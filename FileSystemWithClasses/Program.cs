using FileSystemWithClasses;

var ignoredDirectories = new HashSet<string>(
    [".git", ".vs", "bin", "obj", "node_modules"],
    StringComparer.OrdinalIgnoreCase);

Console.WriteLine("File System Printing With Classes");
Console.WriteLine("Enter folder path:");

var folderPath = Console.ReadLine();

if (string.IsNullOrWhiteSpace(folderPath))
{
    Console.WriteLine("A folder path is required.");
    return;
}

try
{
    var rootFolder = CreateDirectoryStructure(folderPath);

    Console.WriteLine();
    rootFolder.Print();
}
catch (Exception exception)
{
    Console.WriteLine($"Unable to read the folder: {exception.Message}");
}
finally
{
    Console.ResetColor();
}

DirectoryFolder CreateDirectoryStructure(string path)
{
    var directory = new DirectoryInfo(path);

    if (!directory.Exists)
    {
        throw new DirectoryNotFoundException($"Folder '{path}' was not found.");
    }

    var folder = new DirectoryFolder(directory.Name);

    foreach (var file in directory.GetFiles().OrderBy(file => file.Name))
    {
        folder.Files.Add(new DirectoryFile(file.Name));
    }

    foreach (var childDirectory in directory
                 .GetDirectories()
                 .Where(child => !ignoredDirectories.Contains(child.Name))
                 .OrderBy(child => child.Name))
    {
        folder.Folders.Add(CreateDirectoryStructure(childDirectory.FullName));
    }

    return folder;
}

void PrintDirectoryStructure(DirectoryFolder folder, string indentation = "")
{
    
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine($"{indentation}{folder.Name}/");

    foreach (var file in folder.Files)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"{indentation}  {file.Name}");
    }

    foreach (var childFolder in folder.Folders)
    {
        PrintDirectoryStructure(childFolder, $"{indentation}  ");
    }
}
