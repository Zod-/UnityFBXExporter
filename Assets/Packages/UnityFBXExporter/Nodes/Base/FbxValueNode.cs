namespace UnityFBXExporter
{
    public class FbxValueNode : FbxNode
    {
        public FbxValueNode(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
