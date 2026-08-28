var ignoredDirectories = new HashSet<string>(
    [".git", ".vs", "bin", "obj", "node_modules"],
    StringComparer.OrdinalIgnoreCase);

Console.WriteLine("Direct File System Printing");
Console.WriteLine("Enter folder path:");

var folderPath = Console.ReadLine();

if (string.IsNullOrWhiteSpace(folderPath))
{
    Console.WriteLine("A folder path is required.");
    return;
}

try
{
    Console.WriteLine();
    PrintDirectoryStructure(folderPath);
}
catch (Exception exception)
{
    Console.WriteLine($"Unable to read the folder: {exception.Message}");
}
finally
{
    Console.ResetColor();
}

void PrintDirectoryStructure(string path, string indentation = "")
{
    var directory = new DirectoryInfo(path);

    if (!directory.Exists)
    {
        throw new DirectoryNotFoundException($"Folder '{path}' was not found.");
    }

    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine($"{indentation}{directory.Name}/");

    foreach (var file in directory.GetFiles().OrderBy(file => file.Name))
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"{indentation}  {file.Name}");
    }

    foreach (var childDirectory in directory
                 .GetDirectories()
                 .Where(child => !ignoredDirectories.Contains(child.Name))
                 .OrderBy(child => child.Name))
    {
        PrintDirectoryStructure(childDirectory.FullName, $"{indentation}  ");
    }
}
