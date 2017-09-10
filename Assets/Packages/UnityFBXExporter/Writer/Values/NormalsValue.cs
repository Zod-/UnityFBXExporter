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
            foreach (var triangle in MeshCache.Triangles)
            {
                var normal = MeshCache.Normals[triangle];
                (normal.x * -1).ToStringFbx(sb).Append(',');
                normal.y.ToStringFbx(sb).Append(',');
                normal.z.ToStringFbx(sb).Append(',');
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
