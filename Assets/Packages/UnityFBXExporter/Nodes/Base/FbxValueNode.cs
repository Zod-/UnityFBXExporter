namespace UnityFBXExporter
{
    public class FbxValueNode : FbxNode
    {
        public object Value { get; private set; }

        public FbxValueNode(string name, object value) : base(name)
        {
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, FbxValueSerializer.Serialize(Value));
        }
    }
}
