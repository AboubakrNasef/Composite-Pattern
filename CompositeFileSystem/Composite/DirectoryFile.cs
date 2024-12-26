using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zu.TypeScript;
using Zu.TypeScript.TsTypes;

namespace CompositeFileSystem.Composite
{
    public class DirectoryFile : DirectoryElement
    {
        private readonly FileInfo _fileInfo;

        public DirectoryFile(string name, string path) : base(name, path)
        {
            _fileInfo = new FileInfo(path);
            AddTsFunctions();
        }

        private void AddTsFunctions()
        {
            if (_fileInfo.Extension == ".ts")
            {
                var ast = new TypeScriptAST(File.ReadAllText(Path), Name);
                var classes = ast.OfKind(SyntaxKind.ClassDeclaration);
                foreach (var classDeclaration in classes)
                {
                    var FileClass = new DirectoryFileClass(classDeclaration.IdentifierStr, Path);
                    _children.Add(FileClass);
                }
            }
        }

        public override ElementType Type => ElementType.File;

        protected override double GetSize()
        {
            return _fileInfo.Length;
        }
    }
}
