using CSharpComposite.Composite;

class DirectoryPrinter(int left, int top)
{
	int left = left + 1;
	int top = top;

	private bool TrySetCursorPosition(int posLeft, int posTop)
	{
		posTop = Math.Min(posTop, 30);
		if (posLeft >= 0 && posLeft < Console.BufferWidth && posTop >= 0 && posTop < Console.BufferHeight)
		{
			Console.SetCursorPosition(posLeft, posTop);
			return true;
		}
		return false;
	}

	private void WriteAtPosition(int posLeft, int posTop, string text)
	{
		if (TrySetCursorPosition(posLeft, posTop))
		{
			Console.Write(text);
		}
		else
		{
			Console.WriteLine(text);
		}
	}

	public void Visit(DirectoryFolder directoryElement)
	{
		Console.BackgroundColor = ConsoleColor.Black;

		Console.ForegroundColor = ConsoleColor.DarkCyan;
		var verticalBar = left == 1 ? "" : "|";
		var printerString = verticalBar + new string('_', left - 1) + directoryElement.ToString();
		WriteAtPosition(left, top, printerString);
		left++;
		top++;

	}
	public void Visit(DirectoryFile directoryElement)
	{
		Console.ForegroundColor = ConsoleColor.DarkMagenta;
		var printerString = "|" + new string('_', left - 1) + directoryElement.ToString();
		WriteAtPosition(left, top, printerString);
		//  left++;
	}

	public void Visit(CSHP_File cshpElement)
	{
		Console.BackgroundColor = ConsoleColor.Black;

		Console.ForegroundColor = ConsoleColor.Yellow;
		var verticalBar = left == 1 ? "" : "|";
		var printerString = verticalBar + new string('_', left - 1) + cshpElement.ToString();
		WriteAtPosition(left, top, printerString);
		left++;
		top++;
	}

	public void Visit(CSHP_Method cshpElement)
	{
		Console.ForegroundColor = ConsoleColor.Blue;
		var printerString = "|" + new string('_', left - 1) + cshpElement.ToString() + "()";
		WriteAtPosition(left, top, printerString);
		left++;
		top++;
	}

	public void Visit(CSHP_Property cshpElement)
	{
		Console.ForegroundColor = ConsoleColor.Green;
		var printerString = "|" + new string('_', left - 1) + cshpElement.ToString();
		WriteAtPosition(left, top, printerString);
		top++;
	}

	public void Leave(CSHP_File cshpElement)
	{
		left--;
		Console.ForegroundColor = ConsoleColor.White;
	}

	public void Leave(CSHP_Method cshpElement)
	{
		left--;
		Console.ForegroundColor = ConsoleColor.White;
	}

	public void Leave(CSHP_Property cshpElement)
	{
		Console.ForegroundColor = ConsoleColor.White;
	}

	public void Visit(CSHP_Parameter cshpElement)
	{
		Console.ForegroundColor = ConsoleColor.Cyan;
		var printerString = "|" + new string('_', left - 1) + cshpElement.ToString() + ": " + cshpElement.ParameterType;
		WriteAtPosition(left, top, printerString);
		top++;
	}

	public void Leave(CSHP_Parameter cshpElement)
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