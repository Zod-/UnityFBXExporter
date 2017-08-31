namespace UnityFBXExporter
{
    public class FbxLayerBaseNode : FbxNode
    {
        public int Layer { get; private set; }

        public FbxLayerBaseNode(string name, int layer, int version = 101) : base(name)
        {
            Layer = layer;
            Node("Version", version);
        }
    }
}
