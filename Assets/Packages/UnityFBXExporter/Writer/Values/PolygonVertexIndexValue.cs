using System.Text;

namespace UnityFBXExporter
{
    public class PolygonVertexIndexValue : MeshCacheValue
    {
        public PolygonVertexIndexValue(MeshCache meshCache) : base(meshCache)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < MeshCache.Triangles.Length; i += 3)
            {
                sb.Append(MeshCache.Triangles[i]).Append(',');
                sb.Append(MeshCache.Triangles[i + 2]).Append(',');
                sb.Append(-1 + MeshCache.Triangles[i + 1] * -1).Append(',');
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
