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
                sb.AppendFormat("{0},{1},{2},", MeshCache.Triangles[i], MeshCache.Triangles[i + 2], -1 + MeshCache.Triangles[i + 1] * -1);
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
