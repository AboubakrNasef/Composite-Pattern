Employee person = new Manager();
var report = new CompanyReportVisitor();
ICompanyVisitor visitor = report;
visitor.Visit(person);
if (report.LastVisit != "Employee") throw new Exception("Overload selection failed");
Console.WriteLine($"Direct Visit: {report.LastVisit}");
person.Accept(visitor);
if (report.LastVisit != "Manager") throw new Exception("Double dispatch failed");
Console.WriteLine($"Via Accept: {report.LastVisit}");

Node root = new Folder(new FileNode(120), new Folder(new FileNode(80)));
var size = new SizeVisitor();
root.Accept(size);
if (size.Total != 200) throw new Exception("Total should be 200");
Console.WriteLine($"Total: {size.Total}");
root.Accept(new ReportVisitor());

interface ICompanyVisitor
{
    void Visit(Employee employee);
    void Visit(Manager manager);
}
class Employee
{
    public virtual void Accept(ICompanyVisitor visitor) => visitor.Visit(this);
}
class Manager : Employee
{
    public override void Accept(ICompanyVisitor visitor) => visitor.Visit(this);
}
sealed class CompanyReportVisitor : ICompanyVisitor
{
    public string LastVisit { get; private set; } = "";
    public void Visit(Employee employee) => LastVisit = "Employee";
    public void Visit(Manager manager) => LastVisit = "Manager";
}
interface INodeVisitor
{
    void Visit(FileNode file);
    void Visit(Folder folder);
}
abstract class Node
{
    public abstract void Accept(INodeVisitor visitor);
}
sealed class FileNode(long bytes) : Node
{
    public long Bytes { get; } = bytes;
    public override void Accept(INodeVisitor v) => v.Visit(this);
}
sealed class Folder(params Node[] children) : Node
{
    public override void Accept(INodeVisitor visitor)
    {
        visitor.Visit(this);
        foreach (Node child in children) child.Accept(visitor);
    }
}
sealed class SizeVisitor : INodeVisitor
{
    public long Total { get; private set; }
    public void Visit(FileNode file) => Total += file.Bytes;
    public void Visit(Folder folder) { }
}
sealed class ReportVisitor : INodeVisitor
{
    public void Visit(FileNode file) => Console.WriteLine($"File: {file.Bytes} bytes");
    public void Visit(Folder folder) => Console.WriteLine("Folder");
}
