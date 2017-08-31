namespace UnityFBXExporter
{
    public class FbxLayerBaseNode : FbxNode
    {
        public int Layer { get; protected set; }

        public FbxLayerBaseNode(string name, int layer, int version = 101)
        {
            Name = name;
            Layer = layer;
            Node("Version", version);
        }
    }
}
