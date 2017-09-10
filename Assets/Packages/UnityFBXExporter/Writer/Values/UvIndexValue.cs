using System.Text;

namespace UnityFBXExporter
{
    public class UvIndexValue : MeshCacheValue
    {
        public UvIndexValue(MeshCache meshCache) : base(meshCache)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < MeshCache.Triangles.Length; i += 3)
            {
                sb.Append(MeshCache.Triangles[i]).Append(',');
                sb.Append(MeshCache.Triangles[i + 2]).Append(',');
                sb.Append(MeshCache.Triangles[i + 1]).Append(',');
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
