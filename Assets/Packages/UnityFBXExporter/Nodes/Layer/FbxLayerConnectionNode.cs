namespace UnityFBXExporter
{
    public class FbxLayerConnectionNode : FbxNode
    {
        public FbxLayerConnectionNode(string type) : base("LayerElement")
        {
            Node("Type", type);
            Node("TypedIndex", 0);
        }
    }
}
