using System.Text;

namespace UnityFBXExporter
{
    public class VerticesValue : MeshCacheValue
    {
        public VerticesValue(MeshCache meshCache) : base(meshCache)
        {
        }

        public override string ToString()
        {
            var meshVertices = MeshCache.Vertices;
            var sb = new StringBuilder(meshVertices.Length * 3);
            for (var i = 0; i < meshVertices.Length; i++)
            {
                sb.AppendFormat("{0},{1},{2},", meshVertices[i].x * -1, meshVertices[i].y, meshVertices[i].z);
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
