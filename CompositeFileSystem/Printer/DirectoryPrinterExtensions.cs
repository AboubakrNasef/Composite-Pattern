// See https://aka.ms/new-console-template for more information
using CompositeFileSystem.Composite;

static class DirectoryPrinterExtensions
{
    public static void Accept(this DirectoryElement directoryElement, DirectoryPrinter printer)
    {
        printer.Visit((dynamic)directoryElement);
        foreach (var item in directoryElement.Childrens)
        {
            item.Accept(printer);
        }
        printer.Leave((dynamic)directoryElement);
    }


}
