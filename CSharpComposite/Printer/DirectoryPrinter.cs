using CSharpComposite.Composite;

class DirectoryPrinter(int left, int top)
{
	int left = left + 1;
	int top = top;
	public void Visit(DirectoryFolder directoryElement)
	{
		Console.BackgroundColor = ConsoleColor.Black;

		Console.ForegroundColor = ConsoleColor.DarkCyan;
		var verticalBar = left == 1 ? "" : "|";
		var printerString = verticalBar + new string('_', left - 1) + directoryElement.ToString();
		Console.SetCursorPosition(left, top);
		Console.Write(printerString);
		left++;
		top++;

	}
	public void Visit(DirectoryFile directoryElement)
	{
		Console.ForegroundColor = ConsoleColor.DarkMagenta;
		var printerString = "|" + new string('_', left - 1) + directoryElement.ToString();
		Console.SetCursorPosition(left, top);
		Console.Write(printerString);
		//  left++;
	}

	public void Visit(CSHP_File cshpElement)
	{
		Console.BackgroundColor = ConsoleColor.Black;

		Console.ForegroundColor = ConsoleColor.Yellow;
		var verticalBar = left == 1 ? "" : "|";
		var printerString = verticalBar + new string('_', left - 1) + "📄 " + cshpElement.ToString();
		Console.SetCursorPosition(left, top);
		Console.Write(printerString);
		left++;
		top++;
	}

	public void Visit(CSHP_Method cshpElement)
	{
		Console.ForegroundColor = ConsoleColor.Blue;
		var printerString = "|" + new string('_', left - 1) + "⚙️  " + cshpElement.ToString() + $"(): {cshpElement.ReturnType}";
		Console.SetCursorPosition(left, top);
		Console.Write(printerString);
		top++;
	}

	public void Visit(CSHP_Property cshpElement)
	{
		Console.ForegroundColor = ConsoleColor.Green;
		var printerString = "|" + new string('_', left - 1) + "📋 " + cshpElement.ToString() + $": {cshpElement.PropertyType}";
		Console.SetCursorPosition(left, top);
		Console.Write(printerString);
		top++;
	}

	public void Leave(CSHP_File cshpElement)
	{
		left--;
		Console.ForegroundColor = ConsoleColor.White;
	}

	public void Leave(CSHP_Method cshpElement)
	{
		Console.ForegroundColor = ConsoleColor.White;
	}

	public void Leave(CSHP_Property cshpElement)
	{
		Console.ForegroundColor = ConsoleColor.White;
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
}