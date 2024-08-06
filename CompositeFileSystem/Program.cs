using CompositeFileSystem.Composite;
using Dumpify;
using File = CompositeFileSystem.Composite.File;

var ParentFolder = new Folder("ParentFolder");
var subFolder1 = new Folder("subFolder1");
var subFolder2 = new Folder("subFolder2");
var file1 = new File("file1");
var file2 = new File("file2");
subFolder1.Add(file1);
ParentFolder.Add(subFolder1);
ParentFolder.Add(subFolder2);
ParentFolder.Add(file2);

Printage.Print(ParentFolder);

static class Printage
{
    public  static void Print(DirectoryElement element)
    {
        Console.Write($"{element.Name}:{element.Size}");
        var childs = element.Childrens;
        if(childs is  null) return ;
      for (int i = 0; i < childs.Count; i++)
      {
        Print(childs[i]);
           Console.Write($"{new string('-',i+1)}{childs[i].Name}:{childs[i].Size}");
      }
    }
}