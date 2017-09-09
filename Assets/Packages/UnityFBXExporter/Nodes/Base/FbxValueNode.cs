namespace UnityFBXExporter
{
    public class FbxValueNode : FbxNode
    {
        protected readonly object Value;

        public FbxValueNode(string name, object value) : base(name)
        {
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Value);
        }
    }
}
