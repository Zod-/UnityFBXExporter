namespace UnityFBXExporter
{
    public class FBXValueNode : FBXNode
    {
        public FBXValueNode(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
