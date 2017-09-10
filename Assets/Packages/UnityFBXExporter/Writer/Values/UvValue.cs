using System.Text;

namespace UnityFBXExporter
{
    public class UvValue : MeshCacheValue
    {
        public UvValue(MeshCache meshCache) : base(meshCache)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder(MeshCache.Uv.Length * 4);
            for (var i = 0; i < MeshCache.Uv.Length; i++)
            {
                MeshCache.Uv[i].ToStringFbx(sb).Append(',');
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
