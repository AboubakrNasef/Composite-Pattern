namespace FileSystemWithClasses;

public class DirectoryFolder
{
    public DirectoryFolder(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Name { get; }

    public List<DirectoryFile> Files { get; } = [];

    public List<DirectoryFolder> Folders { get; } = [];

    public void Print()
    {

		Console.ForegroundColor = ConsoleColor.DarkCyan;
		Console.WriteLine($"{Name}/");

		foreach (var file in Files)
		{
			Console.ForegroundColor = ConsoleColor.DarkMagenta;
			Console.WriteLine($"{file.Name}");
		}

		foreach (var childFolder in Folders)
		{
			childFolder.Print() ;
		}
	}
}
