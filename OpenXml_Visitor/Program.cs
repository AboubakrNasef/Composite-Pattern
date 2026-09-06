using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

var outputPath = Path.Combine(Environment.CurrentDirectory, "openxml-example.docx");
var visitorOutputPath = Path.Combine(Environment.CurrentDirectory, "openxml-visitor-result.docx");

WordDocumentService.CreateDocument(
    outputPath,
    title: "Open XML Visitor Example",
    author: "Composite Pattern Demo",
    paragraphs:
    [
        "This Word document was created with the Open XML SDK.",
        "The visitor can manipulate Open XML paragraphs without changing the traversal code.",
        "Open XML documents are composed of nested elements that can be visited and read."
    ]);

File.Copy(outputPath, visitorOutputPath, overwrite: true);

var highlightVisitor = new HighlightVisitor("visitor");
WordDocumentService.ApplyVisitor(visitorOutputPath, highlightVisitor);

var document = WordDocumentService.ExtractDocument(visitorOutputPath);

Console.WriteLine($"Created: {outputPath}");
Console.WriteLine($"Visitor output: {visitorOutputPath}");
Console.WriteLine($"Highlighted paragraphs: {highlightVisitor.Matches}");
Console.WriteLine($"Title: {document.Title}");
Console.WriteLine($"Author: {document.Author}");
Console.WriteLine("Extracted paragraphs after visitor manipulation:");
foreach (var paragraph in document.Paragraphs)
{
    Console.WriteLine($"- {paragraph}");
}

public sealed record ExtractedWordDocument(
    string? Title,
    string? Author,
    IReadOnlyList<string> Paragraphs);

public interface IWordElementVisitor
{
    void Visit(Document document);
    void Visit(Body body);
    void Visit(Paragraph paragraph);
    void Visit(Run run);
}

public static class WordDocumentWalker
{
    public static void Walk(OpenXmlElement element, IWordElementVisitor visitor)
    {
        switch (element)
        {
            case Document document:
                visitor.Visit(document);
                break;
            case Body body:
                visitor.Visit(body);
                break;
            case Paragraph paragraph:
                visitor.Visit(paragraph);
                break;
            case Run run:
                visitor.Visit(run);
                break;
        }

        foreach (var child in element.ChildElements)
        {
            Walk(child, visitor);
        }
    }
}

public sealed class HighlightVisitor : IWordElementVisitor
{
    private readonly string _searchText;

    public HighlightVisitor(string searchText)
    {
        _searchText = searchText;
    }

    public int Matches { get; private set; }

    public void Visit(Document document)
    {
    }

    public void Visit(Body body)
    {
    }

    public void Visit(Paragraph paragraph)
    {
        var text = string.Concat(paragraph.Descendants<Text>().Select(text => text.Text));
        if (!text.Contains(_searchText, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        Matches++;

        foreach (var run in paragraph.Descendants<Run>())
        {
            run.RunProperties ??= new RunProperties();
            run.RunProperties.Append(
                new Bold(),
                new Highlight { Val = HighlightColorValues.Yellow });
        }
    }

    public void Visit(Run run)
    {
    }
}

public static class WordDocumentService
{
    public static void CreateDocument(
        string filePath,
        string title,
        string author,
        IEnumerable<string> paragraphs)
    {
        var directory = Path.GetDirectoryName(Path.GetFullPath(filePath));
        if (directory is not null)
        {
            Directory.CreateDirectory(directory);
        }

        using var document = WordprocessingDocument.Create(
            filePath,
            DocumentFormat.OpenXml.WordprocessingDocumentType.Document);

        var mainPart = document.AddMainDocumentPart();
        var body = new Body(
            new Paragraph(
                new ParagraphProperties(
                    new ParagraphStyleId { Val = "Title" }),
                new Run(new Text(title))));

        body.Append(paragraphs.Select(text => new Paragraph(new Run(new Text(text)))));
        mainPart.Document = new Document(body);

        document.PackageProperties.Title = title;
        document.PackageProperties.Creator = author;
        document.PackageProperties.Subject = "Open XML document extraction";
        document.PackageProperties.Description = "Created and read with the Open XML SDK.";
        mainPart.Document.Save();
    }

    public static void ApplyVisitor(string filePath, IWordElementVisitor visitor)
    {
        using var document = WordprocessingDocument.Open(filePath, true);
        var root = document.MainDocumentPart?.Document
            ?? throw new InvalidDataException("The Word document has no main document part.");

        WordDocumentWalker.Walk(root, visitor);
        root.Save();
    }

    public static ExtractedWordDocument ExtractDocument(string filePath)
    {
        using var document = WordprocessingDocument.Open(filePath, false);

        var paragraphs = document.MainDocumentPart?.Document.Body?
            .Elements<Paragraph>()
            .Select(paragraph => string.Concat(paragraph.Descendants<Text>().Select(text => text.Text)))
            .Where(text => !string.IsNullOrWhiteSpace(text))
            .ToArray()
            ?? [];

        return new ExtractedWordDocument(
            document.PackageProperties.Title,
            document.PackageProperties.Creator,
            paragraphs);
    }
}
