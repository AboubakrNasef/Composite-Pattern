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
        protected IList<DirectoryElement> _childrens;

        public IReadOnlyList<DirectoryElement> Childrens => _childrens.AsReadOnly();

        public string Name { get; protected set; }
        public virtual double Size => Math.Round(GetSize(), 2);
        public abstract ElementType Type { get; }

        protected DirectoryElement(string name)
        {
            Name = name;
            _childrens = new List<DirectoryElement>();
        }
        public virtual void Add(DirectoryElement element)
        {
            throw new NotImplementedException();
        }
        public virtual bool Remove(DirectoryElement element)
        {
            throw new NotImplementedException();
        }

        public virtual DirectoryElement this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected abstract double GetSize();
        public virtual void Rename(string newName)
        {
            Name = newName;
        }

        public override string ToString()
        {
            return $"{Name} : {Size} MB";

        }
    }
}
