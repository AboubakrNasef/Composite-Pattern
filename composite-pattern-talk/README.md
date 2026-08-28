# Composite Pattern Talk

## Communication job

By the end of this 30-minute talk, junior developers should be able to recognize a part-whole tree, identify the Component, Leaf, Composite, and Client roles, and explain why treating individual objects and groups through one abstraction simplifies recursive code.

## Central takeaway

The Composite pattern lets client code treat one object and a group of objects uniformly. It is especially useful when a domain naturally forms a tree.

## Narrative arc

1. The audience already uses Composite-shaped APIs.
2. A filesystem implementation exposes the pain of a naive tree design.
3. Composite moves that complexity behind a common abstraction.
4. Parsing C# files demonstrates how the model grows without rewriting traversal code.
5. The audience applies the pattern by investigating a real-world example.

Target 27-28 minutes of prepared material, leaving 2-3 minutes for transitions, discussion, or overruns.

## Talk outline

### 1. You already use Composite — 3 minutes

- Rapidly show familiar tree-based APIs:
  - LINQ and EF Core expression trees
  - HTML and XML DOM trees
  - WordprocessingML/Open XML element trees
  - UI control and component trees
  - Angular reactive forms
  - PDF page and object trees
- Ask: "What do all these systems have in common?"
- Reveal the recurring structure: individual nodes and containers are accessed through a shared abstraction.
- Keep this section fast. Its purpose is curiosity, not a detailed explanation of every API.

Accuracy note: LINQ itself is not generally an implementation of Composite. The precise example is `System.Linq.Expressions` and the expression trees consumed by `IQueryable` providers such as EF Core. PDF libraries vary, so describe PDF as a tree-shaped document model unless the chosen library exposes a clear common component abstraction.

### 2. Model a filesystem — 2 minutes

- Show a small project tree containing folders and files.
- Introduce the requirements:
  - Print every item.
  - Search by name.
  - Calculate size.
  - Support unlimited nesting.
- Ask how the audience would represent it before naming the pattern.

### 3. Build the naive version — 5 minutes

- Start with unrelated `File` and `Folder` types.
- Put traversal and type decisions in the client.
- Show `if`, `switch`, or type-casting branches for files and folders.
- Add a second operation and expose the repeated traversal logic.
- Emphasize the problems:
  - The client knows every concrete type.
  - Recursive behavior is duplicated.
  - New node types require edits in several places.

### 4. Name the design pressure — 2 minutes

- Files and folders are different, but both are directory entries.
- The client should not own the tree's structural rules.
- Transition with: "What if the client could treat a file and an entire folder through the same abstraction?"

### 5. Introduce Composite — 3 minutes

- Define it in plain language: "Treat one object and a group of objects in the same way."
- Map the roles to the example:
  - Component: `DirectoryElement`
  - Leaf: `DirectoryFile`
  - Composite: `DirectoryFolder`
  - Client: the printer or application
- Show one small class diagram beside the filesystem tree.

### 6. Refactor to Composite — 5 minutes

- Introduce the `DirectoryElement` abstraction.
- Make files and folders derive from it.
- Let `DirectoryFolder` contain a collection of `DirectoryElement` children.
- Traverse the tree recursively through the common abstraction.
- Contrast the naive and Composite clients:
  - No concrete-type branching for traversal.
  - One recursive algorithm.
  - Client code depends on the abstraction.

Use the existing `CompositeFileSystem` project as the running demonstration.

### 7. Extend the tree with parsed C# code — 5 minutes

- Add a new requirement: expand C# files to show their classes, methods, and properties.
- Parse a C# source file and produce a structure such as:

```text
UserService.cs
└── UserService
    ├── Name
    ├── GetUser()
    └── SaveUser()
```

- Introduce new element types such as:
  - `ClassElement`
  - `MethodElement`
  - `PropertyElement`
- Point out that a parsed C# file is now a Composite because it owns children. Methods and properties are the new Leaves.
- Demonstrate that the existing printer and recursive traversal still work.
- Make the extensibility claim concrete by highlighting which files changed and which did not.

### 8. Revisit real-world examples — 2 minutes

- Map Angular reactive forms:
  - Component: `AbstractControl`
  - Leaf: `FormControl`
  - Composites: `FormGroup` and `FormArray`
- Map the HTML DOM:
  - Component: `Node`
  - Leaf example: a text node
  - Composite example: an element containing child nodes
- Explain that a structure being a tree is necessary but not sufficient: the useful Composite idea is the common abstraction shared by individual and container nodes.

### 9. Investigation task — 2 minutes

- Ask participants to investigate one of:
  - LINQ/EF Core expression trees
  - HTML DOM
  - Angular reactive forms
  - Open XML document elements
  - A UI framework's visual tree
- Have them answer:
  - What is the Component?
  - What are the Leaves?
  - What are the Composites?
  - Which operation is shared?
  - Who acts as the Client?
  - Is it genuinely Composite, or only tree-shaped?

### 10. Close — 1 minute

- Composite represents part-whole hierarchies.
- Leaves and containers share a common abstraction.
- Recursive client code becomes simpler.
- New node types can be introduced with fewer changes to existing clients.
- End with: "The next time you see a tree, ask whether the client should care if a node is one object or a collection."

## Slidev direction

- Use one recurring filesystem tree that evolves throughout the deck.
- Reveal opening examples rapidly, one per click.
- Animate duplicated branches accumulating in the naive implementation.
- Transition from the filesystem tree to the Component/Leaf/Composite diagram.
- Keep code samples focused and generally below 15 visible lines.
- Use line highlighting to introduce one structural change at a time.
- Show the same traversal before and after adding parsed C# elements.
- Place detailed explanations and source links in speaker notes.
- Finish on the investigation task rather than a generic "Thank you" slide.

## Sources for claims

- Microsoft Learn, Expression Trees: <https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/expression-trees/>
- MDN, Anatomy of the DOM: <https://developer.mozilla.org/en-US/docs/Web/API/Document_Object_Model/Anatomy_of_the_DOM>
- Angular, Reactive Forms: <https://angular.dev/guide/forms/reactive-forms>
- Microsoft Learn, Structure of a WordprocessingML document: <https://learn.microsoft.com/en-us/office/open-xml/word/structure-of-a-wordprocessingml-document>

## Future folder structure

When the Slidev deck is implemented, keep the presentation isolated in this folder:

```text
composite-pattern-talk/
├── README.md
├── package.json
├── slides.md
├── components/
├── snippets/
├── public/
└── styles/
```

The C# application remains in `CompositeFileSystem`; Slidev code snippets should reference or mirror that implementation without moving the demo project into the talk folder.
