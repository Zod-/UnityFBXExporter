namespace UnityFBXExporter
{
    public class FbxLayerConnectionNode : FbxNode
    {
        public FbxLayerConnectionNode(string type)
        {
            Name = "Layer";
            Node("Type", type);
            Node("TypedIndex", 0);
        }
    }
}
