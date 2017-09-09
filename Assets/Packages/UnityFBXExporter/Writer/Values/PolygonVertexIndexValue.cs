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
            var triangles = MeshCache.Triangles;
            var sb = new StringBuilder();
            foreach (var triangle in FbxExporter.FlipYZTriangles(triangles, true))
            {
                sb.AppendFormat("{0},", triangle);
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
