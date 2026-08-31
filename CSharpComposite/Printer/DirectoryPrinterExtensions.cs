using CSharpComposite.Composite;

static class DirectoryPrinterExtensions
{
    public static void Accept(this DirectoryElement directoryElement, DirectoryPrinter printer)
    {
        printer.Visit((dynamic)directoryElement);
        foreach (var item in directoryElement.Children)
        {
            item.Accept(printer);
        }
        printer.Leave((dynamic)directoryElement);
    }
}
