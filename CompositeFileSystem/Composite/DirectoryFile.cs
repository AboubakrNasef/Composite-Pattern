using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeFileSystem.Composite
{
    public class DirectoryFile : DirectoryElement
    {
        private readonly FileInfo _fileInfo;

        public DirectoryFile(string name, string path) : base(name, path)
        {
            _fileInfo = new FileInfo(path);
        }

        public override ElementType Type => ElementType.File;

        protected override double GetSize()
        {
            return _fileInfo.Length;
        }
    }
}
