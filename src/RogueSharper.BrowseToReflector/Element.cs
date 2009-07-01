namespace RogueSharper.BrowseToReflector
{
    internal class Element
    {
        public static readonly Element NotFound = new Element("", "", "");

        public string AssemblyFile { get; private set; }
        public string TypeName { get; private set; }
        public string MemberName { get; private set; }

        public Element(string assemblyFile, string typeName, string memberName)
        {
            this.AssemblyFile = assemblyFile;
            this.TypeName = typeName;
            this.MemberName = memberName;
        }
    }
}