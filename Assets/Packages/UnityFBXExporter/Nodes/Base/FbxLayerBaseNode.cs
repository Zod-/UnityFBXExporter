namespace UnityFBXExporter
{
    public class FbxLayerBaseNode : FbxNode
    {
        private readonly int _layer;

        protected FbxLayerBaseNode(string name, int layer, int version = 101) : base(name)
        {
            _layer = layer;
            Node("Version", version);
        }

        public override string GetMetaName()
        {
            return _layer.ToString();
        }
    }
}
