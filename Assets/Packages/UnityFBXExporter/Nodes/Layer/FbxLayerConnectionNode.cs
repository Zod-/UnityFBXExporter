namespace UnityFBXExporter
{
    public class FbxLayerConnectionNode : FbxNode
    {
        public FbxLayerConnectionNode(string type) : base("Layer")
        {
            Node("Type", type);
            Node("TypedIndex", 0);
        }
    }
}
