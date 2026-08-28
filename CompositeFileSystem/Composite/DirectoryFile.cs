using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeFileSystem.Composite
{
    public class DirectoryFile : DirectoryElement
    {
        public DirectoryFile(string name) : base(name)
        {
        }
         
        public override ElementType Type => ElementType.File;
    }
}
