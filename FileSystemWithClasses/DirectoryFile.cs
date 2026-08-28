namespace FileSystemWithClasses;

public class DirectoryFile
{
    public DirectoryFile(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Name { get; }
}
