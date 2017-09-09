using System.Text;

namespace UnityFBXExporter
{
    public class NormalsValue : MeshCacheValue
    {
        public NormalsValue(MeshCache meshCache) : base(meshCache)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder(MeshCache.Triangles.Length * 3);
            for (var i = 0; i < MeshCache.Triangles.Length; i += 3)
            {
                var normal = MeshCache.Normals[MeshCache.Triangles[i]];
                sb.AppendFormat("{0},{1},{2},", normal.x * -1, normal.y, normal.z);

                normal = MeshCache.Normals[MeshCache.Triangles[i + 1]];
                sb.AppendFormat("{0},{1},{2},", normal.x * -1, normal.y, normal.z);

                normal = MeshCache.Normals[MeshCache.Triangles[i + 2]];
                sb.AppendFormat("{0},{1},{2},", normal.x * -1, normal.y, normal.z);
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
