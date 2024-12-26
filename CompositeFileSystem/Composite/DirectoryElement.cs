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
        protected IList<DirectoryElement> _children;
        public IReadOnlyList<DirectoryElement> Children => _children.AsReadOnly();

        public string Name { get; protected set; }
        public string Path { get; protected set; }
        public virtual double Size => Math.Round(GetSize(), 2);
        public abstract ElementType Type { get; }

        protected DirectoryElement(string name, string path)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Path = path ?? throw new ArgumentNullException(nameof(path));
            _children = [];
        }

        public virtual void Add(DirectoryElement element)
        {
            throw new NotImplementedException();
        }
        public virtual bool Remove(DirectoryElement element)
        {
            throw new NotImplementedException();
        }

        protected abstract double GetSize();

        public override string ToString()
        {
            return $"{Name} : {Size / 1000} KB";

        }
    }
}
