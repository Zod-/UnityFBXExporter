namespace UnityFBXExporter
{
    public class FbxLayerElementUvNode : FbxLayerElementBaseNode
    {
        public FbxLayerElementUvNode(int layer, MeshCache cache) : base("LayerElementUV", layer, "map1", "ByPolygonVertex", "IndexToDirect")
        {
            ArrayNode("UV", new UvValue(cache), cache.Uv.Length * 2);
            ArrayNode("UVIndex", new UvIndexValue(cache), cache.Triangles.Length); 
        }
    }
}
