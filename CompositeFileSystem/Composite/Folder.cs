namespace CompositeFileSystem.Composite
{
    public class Folder : DirectoryElement
    {
        public Folder(string name) : base(name)
        {
            _childrens = new List<DirectoryElement>();
        }

        public override ElementType Type => ElementType.Folder;

        public override void Add(DirectoryElement element)
        {
            _childrens!.Add(element);
        }
        public override bool Remove(DirectoryElement element)
        {
           return  _childrens!.Remove(element);
        }
        public override DirectoryElement this[int index] => _childrens![index];
        protected override double GetSize()
        {
            var size = 0.0;
            foreach (var item in _childrens!)
            {
                size += item.Size;
            }
            return size;
        }
    }
}
