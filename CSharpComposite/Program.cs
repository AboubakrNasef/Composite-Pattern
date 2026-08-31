using CSharpComposite.Composite;
HashSet<string> ignores = new HashSet<string> { ".git", ".vs", "bin", "obj", "node_modules" };
Console.WriteLine("C# Code Analysis - Composite Design Pattern");
Console.WriteLine("Enter folder path:");

try
{
	var folderPath = Console.ReadLine();

	if (string.IsNullOrWhiteSpace(folderPath))
	{
		Console.WriteLine("A folder path is required.");
		return;
	}

	Console.WriteLine();
	var rootElement = CreateCodeStructure(folderPath);

	#region Print
	var printer = new DirectoryPrinter(0, Console.CursorTop);
	PrintElement(rootElement, printer);
	Console.WriteLine();
	#endregion
}
catch (Exception exception)
{
	Console.WriteLine($"Unable to read the folder: {exception.Message}");
}
finally
{
	Console.ResetColor();
}

DirectoryElement CreateCodeStructure(string folderPath)
{
	var directory = new DirectoryInfo(folderPath);

	if (!directory.Exists)
	{
		throw new DirectoryNotFoundException($"Folder '{folderPath}' was not found.");
	}

	var rootFolder = new DirectoryFolder(directory.Name);
	ProcessDirectory(rootFolder, directory);

	return rootFolder;
}

void ProcessDirectory(DirectoryFolder parentFolder, DirectoryInfo directoryInfo)
{
	// Process subdirectories
	foreach (var subDirectory in directoryInfo.GetDirectories().Where(d => !ignores.Contains(d.Name)))

	{
		var subFolder = new DirectoryFolder(subDirectory.Name);
		parentFolder.Add(subFolder);
		ProcessDirectory(subFolder, subDirectory);
	}

	// Process files
	foreach (var file in directoryInfo.GetFiles().OrderBy(f => f.Name))
	{
		if (file.Extension == ".cs")
		{
			try
			{
				var codeFile = new CSHP_File(file.Name);
				var fileContent = File.ReadAllText(file.FullName);

				// Extract methods
				var methodPattern = @"(?:public|private|protected|internal)\s+(?:static\s+)?(?:async\s+)?(\w+[\w<>,\s]*?)\s+(\w+)\s*\(([^)]*)\)";
				var methodMatches = System.Text.RegularExpressions.Regex.Matches(fileContent, methodPattern);

				foreach (System.Text.RegularExpressions.Match match in methodMatches)
				{
					var returnType = match.Groups[1].Value.Trim();
					var methodName = match.Groups[2].Value.Trim();

					if (methodName != directoryInfo.Name) // Skip constructors
					{
						var method = new CSHP_Method(methodName, returnType);
						codeFile.Add(method);
					}
				}

				// Extract properties
				var propertyPattern = @"(?:public|private)\s+(\w+[\w<>,\s]*)\s+(\w+)\s*\{\s*([^}]*)\}";
				var propertyMatches = System.Text.RegularExpressions.Regex.Matches(fileContent, propertyPattern);

				foreach (System.Text.RegularExpressions.Match match in propertyMatches)
				{
					var propertyType = match.Groups[1].Value.Trim();
					var propertyName = match.Groups[2].Value.Trim();
					var accessors = match.Groups[3].Value.ToLower();

					var hasGetter = accessors.Contains("get");
					var hasSetter = accessors.Contains("set");

					var property = new CSHP_Property(propertyName, propertyType, hasGetter, hasSetter);
					codeFile.Add(property);
				}

				parentFolder.Add(codeFile);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Warning: Could not parse {file.Name}: {ex.Message}");
			}
		}
		else
		{
			// Create DirectoryFile for non-C# files
			var directoryFile = new DirectoryFile(file.Name);
			parentFolder.Add(directoryFile);
		}
	}
}

void PrintElement(DirectoryElement element, DirectoryPrinter printer)
{
	switch (element)
	{
		case DirectoryFolder folder:
			printer.Visit(folder);
			foreach (var child in folder.Children)
			{
				PrintElement(child, printer);
			}
			printer.Leave(folder);
			break;
		case DirectoryFile file:
			printer.Visit(file);
			printer.Leave(file);
			break;
		case CSHP_File cshpFile:
			printer.Visit(cshpFile);
			foreach (var child in cshpFile.Children)
			{
				PrintElement(child, printer);
			}
			printer.Leave(cshpFile);
			break;
		case CSHP_Method method:
			printer.Visit(method);
			printer.Leave(method);
			break;
		case CSHP_Property property:
			printer.Visit(property);
			printer.Leave(property);
			break;
	}
}
