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
        public DirectoryFile(string name) : base(name)
        {
        }

        public override void Add(DirectoryElement element)
        {
            if (element.Type == ElementType.FileClass)
            {
                _children.Add(element);
            }
            else
            {
                throw new InvalidOperationException("can't add");
            }
        }
        public override bool Remove(DirectoryElement element)
        {
            return _children.Remove(element);
        }
        public override ElementType Type => ElementType.File;
    }
}
