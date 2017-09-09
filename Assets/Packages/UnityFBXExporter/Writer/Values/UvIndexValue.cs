namespace UnityFBXExporter
{
    public class UvIndexValue : MeshCacheValue
    {
        public UvIndexValue(MeshCache meshCache) : base(meshCache)
        {
        }

        public override string ToString()
        {
            return FbxValueSerializer.SerializeCollection(MeshCache.FlippedTriangles);
        }
    }
}
