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
            foreach (var triangle in MeshCache.FlippedTriangles)
            {
                sb.AppendFormat("{0},", triangle);
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
