namespace CompositeFileSystem.Composite
{
    public class DirectoryFolder : DirectoryElement
    {
        public DirectoryFolder(string name) : base(name)
        {
            
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
           return _childrens.Select(s => s.Size).Sum();
        }
    }
}
