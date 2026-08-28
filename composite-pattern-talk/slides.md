---
theme: default
title: "Composite Pattern: Treat One and Many the Same"
info: |
  A 30-minute introduction to the Composite design pattern for junior developers.
author: ""
transition: slide-left
clickAnimation: fade
drawings:
  persist: false
mdc: true
---

<div class="eyebrow">DESIGN PATTERNS · 30 MINUTES</div>

# Composite Pattern

## Treat one object and a group of objects the same

<div class="cover-tree" aria-hidden="true">
  <span>project/</span>
  <span>├─ src/</span>
  <span>│  ├─ Program.cs</span>
  <span>│  └─ Services/</span>
  <span>└─ README.md</span>
</div>

<!--
Open with the phrase “one object and a group of objects.” Do not define the pattern yet.
Tell the audience that we will first find the problem, then earn the pattern.
-->

---
layout: center
class: hook-slide
---

# You have already used it.

<div class="ecosystem" role="list">
  <span v-click role="listitem">LINQ + EF Core</span>
  <span v-click role="listitem">HTML + XML DOM</span>
  <span v-click role="listitem">Open XML</span>
  <span v-click role="listitem">UI frameworks</span>
  <span v-click role="listitem">Angular Forms</span>
  <span v-click role="listitem">PDF models</span>
</div>

<p v-click class="question">What shape keeps appearing?</p>

<!--
Reveal these quickly. Ask for guesses after the final item.
Be precise: LINQ itself is not Composite. IQueryable providers such as EF Core consume expression trees.
PDF APIs vary; call them tree-shaped unless discussing a library with a shared node abstraction.

[Sources]
- https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/expression-trees/
- https://developer.mozilla.org/en-US/docs/Web/API/Document_Object_Model/Anatomy_of_the_DOM
- https://angular.dev/guide/forms/reactive-forms
- https://learn.microsoft.com/en-us/office/open-xml/word/structure-of-a-wordprocessingml-document
-->

---
layout: two-cols-header
---

# The recurring shape is a tree

::left::

<div class="big-tree">
  <div class="tree-root">root</div>
  <div class="tree-row"><span>branch</span><span>branch</span></div>
  <div class="tree-row leaves"><span>leaf</span><span>leaf</span><span>leaf</span></div>
</div>

::right::

<div class="statement-stack">
  <p v-click><strong>Leaves</strong> represent individual things.</p>
  <p v-click><strong>Branches</strong> contain other things.</p>
  <p v-click><strong>Both</strong> still belong to the same structure.</p>
</div>

<div v-click class="takeaway">A tree is the clue. A shared abstraction is the pattern.</div>

<!--
This distinction matters: not every tree is automatically an implementation of the GoF Composite pattern.
The defining benefit is that clients can use leaf and container nodes through the same abstraction.
-->

---
layout: statement
---

# Let’s build a filesystem.

<div class="fs-tree large">
<pre>Project/
├── Program.cs
├── README.md
└── Services/
    ├── UserService.cs
    └── EmailService.cs</pre>
</div>

<div class="requirements" v-click>
  print · search · calculate size · nest forever
</div>

<!--
Ask the audience how they would model this before showing any design-pattern vocabulary.
The requirements deliberately include several operations so duplicated traversal becomes visible later.
-->

---
layout: two-cols-header
---

# First attempt: model the nouns

::left::

```csharp {all|1-5|7-12}
class FileItem
{
    public string Name { get; init; }
    public long Size { get; init; }
}

class Folder
{
    public string Name { get; init; }
    public List<FileItem> Files { get; } = [];
    public List<Folder> Folders { get; } = [];
}
```

::right::

<div class="side-copy">
  <p v-click>It looks reasonable.</p>
  <p v-click>It even matches the real world.</p>
  <p v-click class="warning">But the client now knows the structure.</p>
</div>

<!--
Do not call this bad code immediately. It is a natural first solution.
Then focus attention on the two separate collections: every operation must remember both.
-->

---
layout: center
---

# Printing means branching everywhere

```csharp {1-3|5-8|10-13|all}
void Print(Folder folder)
{
    Console.WriteLine(folder.Name);

    foreach (var file in folder.Files)
        Console.WriteLine(file.Name);

    foreach (var childFolder in folder.Folders)
        Print(childFolder);
}

// Search(...) repeats the walk.
// GetSize(...) repeats the walk again.
```

<div v-click class="code-caption">Every operation relearns how the tree works.</div>

<!--
Walk through the two loops, then reveal the repeated operations.
The problem is not recursion. The problem is ownership: every client owns structural knowledge.
-->

---
layout: fact
---

# One new type multiplies the branches

<div class="branch-equation">
  <span>operations</span>
  <b>×</b>
  <span>node types</span>
  <b>=</b>
  <span class="danger-text">change everywhere</span>
</div>

<div class="muted-examples" v-click>
  symbolic link · archive · parsed C# file · generated folder
</div>

<!--
This is the pressure that motivates the pattern.
Avoid making a universal complexity claim; explain it as a maintenance pattern in this design.
-->

---
layout: statement
class: pivot-slide
---

<div class="eyebrow">THE DESIGN QUESTION</div>

# What if the client did not care whether an element was a file or an entire folder?

<div v-click class="pivot-answer">Give both the same abstraction.</div>

<!--
Pause before revealing the answer. This is the conceptual pivot of the talk.
-->

---
layout: two-cols-header
---

# Composite: one contract for parts and wholes

::left::

<div class="role-map">
  <div><small>COMPONENT</small><strong>DirectoryElement</strong></div>
  <div v-click><small>LEAF</small><strong>DirectoryFile</strong></div>
  <div v-click><small>COMPOSITE</small><strong>DirectoryFolder</strong></div>
  <div v-click><small>CLIENT</small><strong>Printer / application</strong></div>
</div>

::right::

<div class="definition">
  Treat an <strong>individual object</strong> and a <strong>group of objects</strong> through the same interface.
</div>

<div v-click class="takeaway">The client sees elements. The elements know the tree.</div>

<!--
Now name the pattern and its four roles.
Keep “Component” separate from UI components; here it means the common abstraction in the pattern.
-->

---
layout: center
---

# The structure now says what the code means

<div class="class-tree">
  <div class="class-node component-node">DirectoryElement<br><small>Name · Children</small></div>
  <div class="connector vertical"></div>
  <div class="class-children">
    <div class="class-node leaf-node">DirectoryFile<br><small>Leaf</small></div>
    <div class="class-node composite-node">DirectoryFolder<br><small>Contains DirectoryElement</small></div>
  </div>
</div>

<div class="legend"><span class="leaf-dot"></span> no children <span class="composite-dot"></span> owns children</div>

<!--
Map the diagram back to the concrete filesystem. A folder can contain either subtype because both are DirectoryElement.
-->

---
layout: two-cols-header
---

# Start with the common abstraction

::left::

```csharp {1|3-4|6-7|all}
public abstract class DirectoryElement
{
    public string Name { get; protected set; }

    public IReadOnlyList<DirectoryElement> Children
        => _children.AsReadOnly();

    protected IList<DirectoryElement> _children = [];
}
```

::right::

<div class="annotation-list">
  <p v-click><span>1</span> One type for every tree node</p>
  <p v-click><span>2</span> Children use that same type</p>
  <p v-click><span>3</span> Recursion becomes natural</p>
</div>

<!--
This is adapted from CompositeFileSystem/Composite/DirectoryElement.cs.
Focus on the recursive type relationship: DirectoryElement contains DirectoryElement.
-->

---
layout: two-cols-header
---

# Leaves stop. Composites continue.

::left::

<div class="code-label leaf-label">LEAF</div>

```csharp
public class DirectoryFile
    : DirectoryElement
{
    public override ElementType Type
        => ElementType.File;
}
```

::right::

<div class="code-label composite-label">COMPOSITE</div>

```csharp {1-2|6-9|all}
public class DirectoryFolder
    : DirectoryElement
{
    public override void Add(
        DirectoryElement element)
    {
        _children.Add(element);
    }
}
```

<div v-click class="mini-takeaway">Different behavior. Shared identity.</div>

<!--
Connect this directly to the class names in the repository.
A file terminates traversal; a folder continues it by owning children.
-->

---
layout: center
---

# The traversal no longer knows concrete types

```csharp {1-2|3-6|7|all}
static void Accept(
    this DirectoryElement element,
    DirectoryPrinter printer)
{
    printer.Visit((dynamic)element);

    foreach (var child in element.Children)
        child.Accept(printer);

    printer.Leave((dynamic)element);
}
```

<div class="flow-line">
  <span>visit</span><b>→</b><span>recurse through children</span><b>→</b><span>leave</span>
</div>

<!--
This excerpt comes from DirectoryPrinterExtensions.cs.
For this talk, the important part is that traversal depends only on DirectoryElement and Children.
Dynamic dispatch here leads toward Visitor, but keep that as a teaser rather than changing topics.
-->

---
layout: two-cols-header
---

# What changed?

::left::

<div class="comparison bad">
  <h3>Before</h3>
  <p>Client knows files</p>
  <p>Client knows folders</p>
  <p>Every operation walks both</p>
  <p>New types add branches</p>
</div>

::right::

<div class="comparison good">
  <h3>With Composite</h3>
  <p>Client knows DirectoryElement</p>
  <p>Containers expose children</p>
  <p>One recursive traversal</p>
  <p>New nodes join the tree</p>
</div>

<div v-click class="takeaway centered">We did not remove complexity. We put it where it belongs.</div>

<!--
Composite does not make different types identical. It gives the client a uniform structural view.
This is also a good place to mention that an overly broad base abstraction can become awkward.
-->

---
layout: statement
---

<div class="eyebrow">NEW REQUIREMENT</div>

# Expand every C# file.

## Show its classes, properties, and methods inside the same tree.

<div class="file-chip">UserService.cs <span>+</span></div>

<!--
Ask whether DirectoryFile can remain a leaf if it now contains members.
Let the audience notice the role change before revealing the expanded tree.
-->

---
layout: two-cols-header
---

# A “file” can become a Composite

::left::

<div class="fs-tree parsed">
<pre>UserService.cs
└── UserService
    ├── Name
    ├── GetUser()
    └── SaveUser()</pre>
</div>

::right::

<div class="role-map compact">
  <div v-click><small>COMPOSITE</small><strong>CSharpFileElement</strong></div>
  <div v-click><small>COMPOSITE</small><strong>ClassElement</strong></div>
  <div v-click><small>LEAF</small><strong>PropertyElement</strong></div>
  <div v-click><small>LEAF</small><strong>MethodElement</strong></div>
</div>

<div v-click class="takeaway">Roles describe structure—not real-world labels.</div>

<!--
This is the key correction: once a C# file owns children, it is no longer a leaf in this model.
The parser can use Roslyn to discover declarations, but parser details are not the lesson.
-->

---
layout: center
---

# The existing traversal still works

<div class="unchanged-code">

```csharp {2-3}
foreach (var child in element.Children)
    child.Accept(printer);
```

</div>

<div class="impact-row">
  <div><strong>Added</strong><span>new element types</span></div>
  <div><strong>Added</strong><span>C# parsing</span></div>
  <div class="untouched"><strong>Untouched</strong><span>tree traversal</span></div>
</div>

<div v-click class="takeaway centered">Extensibility is visible in what we do not edit.</div>

<!--
When the demo is implemented, show an actual run here.
Call out the untouched traversal file. That is stronger evidence than merely saying “open for extension.”
-->

---
layout: two-cols-header
---

# Now recognize it in frameworks

::left::

<div class="framework-map angular-map">
  <h3>Angular Forms</h3>
  <p><small>COMPONENT</small> AbstractControl</p>
  <p><small>LEAF</small> FormControl</p>
  <p><small>COMPOSITES</small> FormGroup · FormArray</p>
  <p><small>SHARED OPERATIONS</small> validate · reset · status</p>
</div>

::right::

<div class="framework-map dom-map">
  <h3>HTML DOM</h3>
  <p><small>COMPONENT</small> Node</p>
  <p><small>LEAF EXAMPLE</small> Text</p>
  <p><small>COMPOSITE EXAMPLE</small> Element with children</p>
  <p><small>SHARED OPERATIONS</small> traverse · inspect · mutate</p>
</div>

<div class="source-footer">Sources: angular.dev · developer.mozilla.org</div>

<!--
Angular’s documentation explicitly identifies AbstractControl as the base of FormControl, FormGroup, and FormArray.
The DOM is a logical tree of Node objects; individual node capabilities vary, so present it as a Composite-shaped API mapping.

[Sources]
- https://angular.dev/guide/forms/reactive-forms
- https://developer.mozilla.org/en-US/docs/Web/API/Document_Object_Model/Anatomy_of_the_DOM
-->

---
layout: center
class: task-slide
---

<div class="eyebrow">YOUR INVESTIGATION</div>

# Find a Composite in the wild

<div class="investigation-grid">
  <div>
    <h3>Choose one</h3>
    <p>EF Core expression tree</p>
    <p>HTML DOM</p>
    <p>Angular Forms</p>
    <p>Open XML</p>
    <p>A UI visual tree</p>
  </div>
  <div>
    <h3>Identify</h3>
    <p>Component</p>
    <p>Leaf</p>
    <p>Composite</p>
    <p>Shared operation</p>
    <p>Client</p>
  </div>
</div>

<div v-click class="task-question">Is it truly Composite—or only tree-shaped?</div>

<!--
This can be an individual follow-up task or a short pair discussion.
The final question prevents pattern matching based on shape alone.
-->

---
layout: statement
class: closing-slide
---

# When you see a tree…

<div class="closing-checks">
  <p v-click>Do leaves and containers share an abstraction?</p>
  <p v-click>Can the client apply one operation to both?</p>
  <p v-click>Can recursion stay independent of concrete types?</p>
</div>

<div v-click class="closing-line">Then Composite may be the design you need.</div>

<!--
Resolve the opening: many familiar APIs repeat this shape because uniform tree traversal is valuable.
Leave this slide visible for questions.
-->
