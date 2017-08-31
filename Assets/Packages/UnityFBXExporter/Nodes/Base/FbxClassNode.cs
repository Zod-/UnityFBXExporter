namespace UnityFBXExporter
{
    public class FbxClassNode : FbxNode
    {
        public long Id { get; private set; }
        public string Class { get; private set; }
        public string SubClass { get; private set; }

        public FbxClassNode(string name, long id, string clazz, string subClass = "") : base(name)
        {
            Id = id;
            Class = clazz;
            SubClass = subClass;
        }

    }
}
