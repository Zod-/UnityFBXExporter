namespace UnityFBXExporter
{
    public abstract class CustomValue
    {
    }

    public class MeshCacheValue : CustomValue
    {
        public MeshCache MeshCache { get; private set; }

        public MeshCacheValue(MeshCache meshCache)
        {
            MeshCache = meshCache;
        }
    }
}
