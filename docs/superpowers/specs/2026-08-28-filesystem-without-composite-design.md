# FileSystem Without Composite Design

## Goal

Add a second .NET 8 console project that demonstrates a reasonable filesystem implementation without the Composite pattern. It should closely match the existing `CompositeFileSystem` program’s observable behavior so the two designs can be compared during the talk.

## Project structure

Create a project named `FileSystem` and add it to `CompositeFileSystem.slnx`.

The project will contain normally named types and files. Nothing will use `Naive` in its name:

```text
FileSystem/
├── FileSystem.csproj
├── Program.cs
├── DirectoryFile.cs
└── DirectoryFolder.cs
```

## Domain model

`DirectoryFile` represents a file and contains its name.

`DirectoryFolder` represents a folder and contains:

- Its name.
- A `List<DirectoryFile>` named `Files`.
- A `List<DirectoryFolder>` named `Folders`.

There is deliberately no common `DirectoryElement` abstraction. A folder keeps files and child folders in separate collections.

## Program methods

`Program.cs` contains the filesystem behavior directly. It does not introduce builder, service, printer, or helper objects.

`CreateDirectoryStructure(string path)` recursively reads a real directory from disk.

- Files become `DirectoryFile` objects.
- Directories become `DirectoryFolder` objects.
- `.git`, `.vs`, `bin`, `obj`, and `node_modules` directories are ignored using case-insensitive comparison.
- A missing or invalid root path produces a clear argument or directory error rather than an incomplete tree.

The filesystem dependency is the real `System.IO` API.

`PrintDirectoryStructure(DirectoryFolder folder, string indentation)` writes the structure directly to the console.

Printing will intentionally expose the structural duplication:

1. Print the current folder.
2. Loop over the folder’s `Files` collection and print every file.
3. Loop over the folder’s `Folders` collection and recursively print every child folder.

The output will use a deterministic text tree and avoid cursor-position APIs, making comparison and testing reliable.

## Console application

The top-level program will:

1. Display the filesystem-demo title.
2. Ask for a folder path.
3. Build the directory structure.
4. Print the tree.
5. Report invalid input with a concise error message.

The project will not use `Dumpify`; it needs only the .NET runtime.

## Comparison with CompositeFileSystem

The two projects should solve the same user-visible problem while differing structurally:

| FileSystem | CompositeFileSystem |
|---|---|
| Separate `DirectoryFile` and `DirectoryFolder` models | Common `DirectoryElement` abstraction |
| Separate `Files` and `Folders` collections | One `Children` collection |
| Program methods know both concrete types | Traversal works through the component abstraction |
| Every new operation repeats the two collection branches | Tree traversal can be reused |

The implementation should remain clear and professional. The educational contrast comes from the model’s coupling, not from intentionally careless code.

## Constraints

- Target .NET 8 to match `CompositeFileSystem`.
- Use nullable reference types and implicit usings.
- Do not rename or modify existing Composite types.
- Do not stage or commit any files.
