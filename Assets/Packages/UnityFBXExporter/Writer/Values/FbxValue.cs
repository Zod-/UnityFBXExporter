namespace UnityFBXExporter
{
    public abstract class FbxValue
    {
    }

    public class MeshCacheValue : FbxValue
    {
        protected readonly MeshCache MeshCache;

        protected MeshCacheValue(MeshCache meshCache)
        {
            MeshCache = meshCache;
        }
    }
}
