namespace UnityFBXExporter
{
    public class FbxLayerElementNormalNode : FbxLayerElementBaseNode
    {
        public FbxLayerElementNormalNode(int layer, MeshCache cache) : base("LayerElementNormal", layer, "", "ByPolygonVertex", "Direct")
        {
            ArrayNode("Normals", new NormalsValue(cache), cache.FlippedTriangles.Length * 3);
        }
    }

    public class NormalsValue : MeshCacheValue
    {
        public NormalsValue(MeshCache meshCache) : base(meshCache)
        {
        }
    }
}
