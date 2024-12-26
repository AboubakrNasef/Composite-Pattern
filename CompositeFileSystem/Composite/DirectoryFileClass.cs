using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeFileSystem.Composite
{
    public class DirectoryFileClass : DirectoryElement
    {
        public DirectoryFileClass(string name, string path) : base(name, path)
        {
        }

        public override ElementType Type => ElementType.FileClass;

        protected override double GetSize()
        {
            return 0;
        }
    }
}
