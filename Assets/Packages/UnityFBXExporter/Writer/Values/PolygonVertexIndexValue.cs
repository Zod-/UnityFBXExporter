namespace UnityFBXExporter
{
    public class PolygonVertexIndexValue : MeshCacheValue
    {
        public PolygonVertexIndexValue(MeshCache meshCache) : base(meshCache)
        {
        }

        public override string ToString()
        {
            var triangles = MeshCache.Triangles;
            var flippedTriangles = FbxExporter.FlipYZTriangles(triangles, true);
            return FbxValueSerializer.SerializeCollection(flippedTriangles);
        }
    }
}
