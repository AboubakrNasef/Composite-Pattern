using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CompositeFileSystem.Composite
{
    public abstract class DirectoryElement
    {
        protected DirectoryElement(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _children = [];
        }

        #region Common
        public string Name { get; protected set; }
        #endregion

        #region CompositeRelated

        protected IList<DirectoryElement> _children;
        public IReadOnlyList<DirectoryElement> Children => _children.AsReadOnly();

        public virtual void Add(DirectoryElement element)
        {
            throw new NotImplementedException();
        }
        public virtual bool Remove(DirectoryElement element)
        {
            throw new NotImplementedException();
        }
        #endregion

        public abstract ElementType Type { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
