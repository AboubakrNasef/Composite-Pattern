---
theme: default
title: 'Design Patterns Under The Hood — Visitor'
info: 'Visitor continuation: company analogy, Accept, and double dispatch in C#.'
colorSchema: light
aspectRatio: 16/9
canvasWidth: 980
fonts:
  sans: PT Sans
  mono: Consolas
  provider: none
transition: fade
drawings:
  persist: false
class: section
---

<div class="section-number">02</div>

# Visitor

Under The Hood

We built the tree. Now let's work with its data.

<!-- Continue after the Composite section. The original PowerPoint supplies the background artwork. -->

---

# Let's Continue Our FileSystem

<p class="lead">We already have files, folders, and a tree.</p>

Now we want to calculate total size and export a report.

<div class="takeaway">Where should these operations live?</div>

---

# Just Add More Methods?

```csharp
abstract class Node
{
    public abstract long CalculateSize();
    public abstract string ExportReport();
    public abstract void FindLargeFiles();
}
```

Each new operation changes the node contract and its implementations.

The tree classes start accumulating unrelated responsibilities.

---

# What About Checking the Type?

```csharp
void Report(Node node)
{
    switch (node)
    {
        case FileNode file:   ReportFile(file); break;
        case Folder folder:  ReportFolder(folder); break;
    }
}
```

This works. But each new operation can repeat the same type branching.

---

# What Would You Do in Real Life?

<div class="flow"><span>Company</span><span class="arrow">→</span><span>Departments</span><span class="arrow">→</span><span>Employees</span></div>

You need to collect working hours and prepare a company report.

<div class="takeaway">How would you collect the information?</div>

---

# Send Someone to Collect the Data

<v-clicks>

- A representative visits each department and its employees.
- Employees provide the information the representative needs.
- The representative collects the data and prepares the report.

</v-clicks>

<div class="takeaway">The reporting task belongs to the representative.</div>

---

# A Different Task, a Different Visitor

<div class="columns">
<div>

## Reporting representative

Collects working hours to prepare a report.

</div>
<div>

## Auditor

Examines records to check compliance.

</div>
</div>

The company structure stays the same. Each visitor brings a different operation.

---

# You Already Know the Idea

<p class="lead">Visiting people to collect or process information is a natural real-life approach.</p>

What if an operation could visit the objects in our tree?

<div class="takeaway">That intuition leads us to Visitor.</div>

<!-- The analogy motivates separating an operation from a structure. It does not by itself explain double dispatch; the next slides supply that mechanism. -->

---

# Visitor: The Idea

<p class="lead">Put an operation in a separate object that can work with each element type.</p>

Once the elements support the visitor contract, new operations can be added as new visitors.

<div class="takeaway">Behavioral pattern · operations over objects</div>

---

# Participants

**Elements:** objects we visit, such as employees and managers.

**Visitor contract:** declares a visit operation for each supported element type.

**Concrete visitor:** implements one task, such as reporting.

**Client / object structure:** supplies elements to the visitor.

---

# Why Not Just Call Visit?

```csharp
interface ICompanyVisitor
{
    void Visit(Employee employee);
    void Visit(Manager manager);
}

Employee person = new Manager();
ICompanyVisitor visitor = new ReportVisitor();
visitor.Visit(person);
```

Which overload does the compiler select?

<!-- Assume Manager inherits Employee, and ReportVisitor implements both interface methods. -->

---

# The Overload Surprise

```csharp
Employee person = new Manager();
visitor.Visit(person); // Selects Visit(Employee)
```

**Compile-time type:** `Employee` — used for overload selection.

**Runtime type:** `Manager` — does not change this overload choice.

<div class="takeaway">Overloading does not automatically dispatch on the argument's runtime type.</div>

<!-- Sources: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/
This example uses ordinary statically typed C#, not dynamic binding. -->

---

# Single Dispatch

```csharp
Employee person = new Manager();
person.GetDetails(); // Manager's override
```

Assume `GetDetails()` is virtual and `Manager` overrides it.

The receiver's runtime type selects the implementation.

<div class="takeaway">One runtime choice: which object's method runs?</div>

<!-- Sources: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/ -->

---

# Overloading vs. Overriding

| Mechanism | What selects the method? | When? |
| --- | --- | --- |
| Overloading | Compile-time argument types | Compile time |
| Virtual overriding | Runtime receiver type | Runtime |

Visitor combines a typed call with two dispatch steps.

<!-- Sources: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/
Scope: the ordinary statically typed calls shown here. -->

---
class: compact
---

# Let the Object Introduce Itself

```csharp
class Employee
{
    public virtual void Accept(ICompanyVisitor visitor)
        => visitor.Visit(this); // this is Employee
}

class Manager : Employee
{
    public override void Accept(ICompanyVisitor visitor)
        => visitor.Visit(this); // this is Manager
}
```

Inside `Manager.Accept`, the compiler knows that `this` is a `Manager`.

So it selects `Visit(Manager)`.

---

# Double Dispatch: First Step

```csharp
Employee person = new Manager();
ICompanyVisitor visitor = new ReportVisitor();

person.Accept(visitor);
```

**Element dispatch:** the runtime element is a `Manager`.

So the call enters `Manager.Accept(visitor)`.

---

# Double Dispatch: Second Step

```csharp
// Inside Manager.Accept:
visitor.Visit(this);
```

The compiler selects the signature `Visit(Manager)`.

**Visitor dispatch:** the runtime visitor is a `ReportVisitor`.

So `ReportVisitor.Visit(Manager)` runs.

<!-- The overload choice is static, not a third runtime dispatch. Interface dispatch chooses the concrete visitor implementation of that signature. -->

---

# Two Objects Choose the Behavior

<div class="flow"><span>Manager</span><span class="arrow">→</span><span>Accept</span><span class="arrow">→</span><span>ReportVisitor</span></div>

**Element type** selects the typed handoff.

**Visitor type** supplies the operation's implementation.

<div class="takeaway">Manager + ReportVisitor → manager reporting behavior</div>

---

# So Why Do We Need Accept?

`Accept` lets an element route a visitor to its typed `Visit` method.

It provides the first dispatch step without repeating type checks in every operation.

<div class="takeaway">Accept is the handoff. Traversing the tree is a separate choice.</div>

<!-- Accept is the conventional mechanism in classic Visitor, not the only possible design in C#. A simple switch may be sufficient for a small, fixed operation. -->

---
class: compact
---

# Back to Files and Folders

```csharp
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
```

---
class: compact
---

# Who Walks the Tree?

```csharp
sealed class Folder(params Node[] children) : Node
{
    public override void Accept(INodeVisitor visitor)
    {
        visitor.Visit(this);
        foreach (Node child in children)
            child.Accept(visitor);
    }
}
```

For this demo, `Folder.Accept` visits the folder, then its children.

Visitors do not recurse again, or nodes would be counted twice.

<!-- This is one traversal policy, not a requirement of Visitor. A visitor-controlled or external traversal can be useful when operations need different walking rules. -->

---
class: compact
---

# Demo: Calculate Total Size

```csharp
sealed class SizeVisitor : INodeVisitor
{
    public long Total { get; private set; }
    public void Visit(FileNode file) => Total += file.Bytes;
    public void Visit(Folder folder) { }
}

Node root = new Folder(
    new FileNode(120),
    new Folder(new FileNode(80)));

var size = new SizeVisitor();
root.Accept(size);
Console.WriteLine(size.Total); // 200
```

<!-- Use a fresh SizeVisitor for each independent total; its state accumulates across visits. Complete runnable example: demo/Program.cs. -->

---
class: compact
---

# A New Operation: Report

```csharp
sealed class ReportVisitor : INodeVisitor
{
    public void Visit(FileNode file)
        => Console.WriteLine($"File: {file.Bytes} bytes");

    public void Visit(Folder folder)
        => Console.WriteLine("Folder");
}

root.Accept(new ReportVisitor());
```

The node classes stay unchanged. The new visitor supplies the report.

---

# Wait… What About SOLID?

| Change | Effect |
| --- | --- |
| New operation | Add another visitor |
| New element type | Extend the visitor contract and its implementations |

<div class="takeaway">Visitor favors adding operations over adding element types.</div>

---

# When Does Visitor Fit?

- Element types are relatively stable.
- New operations appear frequently.
- Operations need different behavior for different element types.

Visitors need access to element data, which can increase coupling.

A small operation or frequently changing hierarchy may favor a simpler design.

---

# Visitor in C#

`ExpressionVisitor` visits expression-tree nodes with methods such as `VisitBinary` and `VisitConstant`.

It also supports rewriting: a visit can return a replacement expression.

<div class="takeaway">The same family of ideas, with a specialized API.</div>

<!-- Sources: https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expressionvisitor
ExpressionVisitor handles traversal and reconstruction; it is not identical to the void-returning visitor in this demo. -->

---

# Composite + Visitor

**Composite** organizes individual objects and groups into a hierarchy.

**Visitor** separates operations that work with its element types.

**Accept** connects an element to the right visitor method.

<div class="takeaway">They work well together. Neither requires the other.</div>

---
class: center
---

# Thank You

What changes more often in your system:<br>the object types or the operations?
