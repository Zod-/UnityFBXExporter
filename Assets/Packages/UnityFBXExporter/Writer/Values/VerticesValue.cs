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
                (meshVertices[i].x * -1).ToStringFbx(sb).Append(',');
                meshVertices[i].y.ToStringFbx(sb).Append(',');
                meshVertices[i].z.ToStringFbx(sb).Append(',');
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
