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
            var triangles = MeshCache.FlippedTriangles;
            var sb = new StringBuilder(triangles.Length * 3);
            foreach (var triangle in triangles)
            {
                var normal = MeshCache.Normals[triangle];
                sb.AppendFormat("{0},{1},{2},", normal.x * -1, normal.y, normal.z);
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
