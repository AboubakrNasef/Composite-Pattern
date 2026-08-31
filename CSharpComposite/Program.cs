using CSharpComposite.Composite;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
				var tree = CSharpSyntaxTree.ParseText(fileContent);
				var root = tree.GetRoot();

				// Extract methods using AST
				var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
				foreach (var method in methods)
				{
					try
					{
						var methodName = method.Identifier.Text;
						var returnType = method.ReturnType.ToString();

						if (methodName != directoryInfo.Name) // Skip constructors
						{
							var cshpMethod = new CSHP_Method(methodName, returnType);

							// Extract parameters
							foreach (var param in method.ParameterList.Parameters)
							{
								try
								{
									var paramName = param.Identifier.Text;
									var paramType = param.Type?.ToString() ?? "var";
									var parameter = new CSHP_Parameter(paramName, paramType);
									cshpMethod.Add(parameter);
								}
								catch
								{
									// Skip problematic parameters
								}
							}

							codeFile.Add(cshpMethod);
						}
					}
					catch
					{
						// Skip problematic methods
					}
				}

				// Extract properties using AST
				var properties = root.DescendantNodes().OfType<PropertyDeclarationSyntax>();
				foreach (var prop in properties)
				{
					try
					{
						var propertyName = prop.Identifier.Text;
						var propertyType = prop.Type.ToString();
						var hasGetter = prop.AccessorList?.Accessors.Any(a => a.Kind() == SyntaxKind.GetAccessorDeclaration) ?? false;
						var hasSetter = prop.AccessorList?.Accessors.Any(a => a.Kind() == SyntaxKind.SetAccessorDeclaration) ?? false;

						var cshpProperty = new CSHP_Property(propertyName, propertyType, hasGetter, hasSetter);
						codeFile.Add(cshpProperty);
					}
					catch
					{
						// Skip problematic properties
					}
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
			foreach (var child in method.Children)
			{
				PrintElement(child, printer);
			}
			printer.Leave(method);
			break;
		case CSHP_Property property:
			printer.Visit(property);
			printer.Leave(property);
			break;
		case CSHP_Parameter parameter:
			printer.Visit(parameter);
			printer.Leave(parameter);
			break;
	}
}
