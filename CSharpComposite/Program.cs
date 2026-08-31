using CSharpComposite.Composite;

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
    var printer = new DirectoryPrinter(Console.CursorLeft, Console.CursorTop);
    rootElement.Accept(printer);
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

    var rootFile = new CSHP_File(directory.Name);
    var csharpFiles = directory.GetFiles("*.cs", SearchOption.AllDirectories)
        .Where(f => !f.FullName.Contains("\\obj\\") && !f.FullName.Contains("\\bin\\"))
        .OrderBy(f => f.Name)
        .ToList();

    foreach (var file in csharpFiles)
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

                if (methodName != directory.Name) // Skip constructors
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

            if (codeFile.Children.Count > 0)
            {
                rootFile.Add(codeFile);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not parse {file.Name}: {ex.Message}");
        }
    }

    return rootFile;
}
