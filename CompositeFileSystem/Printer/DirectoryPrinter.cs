// See https://aka.ms/new-console-template for more information
using CompositeFileSystem.Composite;
using Dumpify;

class DirectoryPrinter
{

    int left = 1;
    int top = 0;
    public void Visit(DirectoryElement directoryElement)
    {
    }
    public void Visit(DirectoryFolder directoryElement)
    {
        Console.BackgroundColor = ConsoleColor.Black;

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        var verticalBar = left == 1 ? "" : "|";
        var printerString = verticalBar + new string('_', left - 1) + " " + directoryElement.ToString();
        Console.SetCursorPosition(left, top);
        Console.Write(printerString);
        left++;
        top++;

    }
    public void Visit(DirectoryFile directoryElement)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        var printerString = "|" + new string('_', left - 1) + " " + directoryElement.ToString();
        Console.SetCursorPosition(left, top);
        Console.Write(printerString);
        // left++;
    }
    public void Visit(DirectoryFileClass directoryElement)
    {
        left++;
        top++;
        Console.ForegroundColor = ConsoleColor.Yellow;
        var printerString = "|" + new string('_', left - 1) + " " + directoryElement.ToString();
        Console.SetCursorPosition(left, top);
        Console.Write(printerString);

    }
    public void Leave(DirectoryElement directoryElement)
    {
    }

    public void Leave(DirectoryFolder directoryElement)
    {
        left--;
        Console.ForegroundColor = ConsoleColor.White;
    }
    public void Leave(DirectoryFile directoryElement)
    {
        top++;
        Console.ForegroundColor = ConsoleColor.White;
    }
    public void Leave(DirectoryFileClass directoryElement)
    {
        left--;
        Console.ForegroundColor = ConsoleColor.White;
    }
}