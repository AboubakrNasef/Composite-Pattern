using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeFileSystem.Composite
{
    public class DirectoryFolder : DirectoryElement
    {
        public DirectoryFolder(string name, string path) : base(name, path)
        {
        }

        public override ElementType Type => ElementType.Folder;

        public override void Add(DirectoryElement element)
        {
            _children.Add(element);
        }
        public override bool Remove(DirectoryElement element)
        {
            return _children.Remove(element);
        }
        protected override double GetSize()
        {
            return _children.Select(s => s.Size).Sum();
        }
    }
}
