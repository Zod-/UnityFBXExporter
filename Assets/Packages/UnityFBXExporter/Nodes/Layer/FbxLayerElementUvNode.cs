namespace UnityFBXExporter
{
    public class FbxLayerElementUvNode : FbxLayerElementBaseNode
    {
        public FbxLayerElementUvNode(int layer, MeshCache cache) : base("LayerElementUV", layer, "map1", "ByPolygonVertex", "IndexToDirect")
        {
            ArrayNode("UV", new UvValue(cache), cache.Uv.Length * 2);
            ArrayNode("UVIndex", new UvIndexValue(cache), cache.FlippedTriangles.Length); 
        }
    }

    public class UvValue : MeshCacheValue
    {
        public UvValue(MeshCache meshCache) : base(meshCache)
        {
        }
    }
    public class UvIndexValue : MeshCacheValue
    {
        public UvIndexValue(MeshCache meshCache) : base(meshCache)
        {
        }
    }
}
