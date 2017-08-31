namespace UnityFBXExporter
{
    public class FbxClassNode : FbxNode
    {
        public long Id { get; protected set; }
        public string Class { get; protected set; }
        public string SubClass { get; protected set; }
    }
}
