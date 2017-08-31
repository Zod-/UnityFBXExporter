namespace UnityFBXExporter
{
    public class FbxValueNode : FbxNode
    {
        public object Value { get; private set; }

        public FbxValueNode(string name, object value) : base(name)
        {
            Value = value;
        }
    }
}
