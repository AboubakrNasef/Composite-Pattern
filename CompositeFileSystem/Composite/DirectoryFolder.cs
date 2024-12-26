using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeFileSystem.Composite
{
    public class DirectoryFolder : DirectoryElement
    {
        public DirectoryFolder(string name) : base(name)
        {
        }

        public override ElementType Type => ElementType.Folder;

        #region CompositeRelated
        public override void Add(DirectoryElement element)
        {
            _children.Add(element);
        }
        public override bool Remove(DirectoryElement element)
        {
            return _children.Remove(element);
        }
        #endregion
    }
}
