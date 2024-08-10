using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeFileSystem.Composite
{
    public class DirectoryFile :DirectoryElement
    {
        double _size = new Random().NextDouble() *100;
        public DirectoryFile(string name) : base(name)
        {
        }

        public override ElementType Type => ElementType.File;

        protected override double GetSize()
        {
            // simulating that wwe actually get the size of the file 
            return _size;
        }
    }
}
