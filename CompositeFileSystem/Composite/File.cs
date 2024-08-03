using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeFileSystem.Composite
{
    public class File :DirectoryElement
    {
        public File(string name) : base(name)
        {
        }

        public override ElementType Type => ElementType.File;

        protected override double GetSize()
        {
            // simulating that wwe actually get the size of the file 
            return new Random().NextDouble();
        }
    }
}
