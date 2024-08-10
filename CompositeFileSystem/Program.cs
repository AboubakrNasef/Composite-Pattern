// See https://aka.ms/new-console-template for more information
using CompositeFileSystem.Composite;
using Dumpify;
using System;



var parentFolder = new DirectoryFolder("ParentFolder");
var folder1 = new DirectoryFolder("Folder1");
var folder2 = new DirectoryFolder("Folder2");
var file1 = new DirectoryFile("File1");
var file2 = new DirectoryFile("File2");
var folder3 = new DirectoryFolder("Folder3");
var file4 = new DirectoryFile("File4");
var file5 = new DirectoryFile("File5");
var file6 = new DirectoryFile("File6");
var file7 = new DirectoryFile("File7");
var file8 = new DirectoryFile("File8");

folder1.Add(file1);
parentFolder.Add(folder1);
parentFolder.Add(folder2);
parentFolder.Add(file2);
folder2.Add(file4);
folder2.Add(folder3);
folder3.Add(file6);
folder3.Add(file7);
folder3.Add(file8);

var printer = new DirectoryPrinter();
parentFolder.Accept(printer);
var posintion = Console.GetCursorPosition();
Console.SetCursorPosition(posintion.Left,posintion.Top+10);