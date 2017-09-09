using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UnityFBXExporter
{
    public class MeshColorIndexValue : MeshCacheValue
    {
        private readonly Dictionary<Color, int> _colorIndices;

        public MeshColorIndexValue(MeshCache meshCache, Dictionary<Color, int> colorIndices) : base(meshCache)
        {
            _colorIndices = colorIndices;
        }

        public override string ToString()
        {
            var trianglesLength = MeshCache.FlippedTriangles.Length;
            var sb = new StringBuilder(trianglesLength * 2);
            for (var i = 0; i < trianglesLength; i++)
            {
                sb.AppendFormat("{0},", _colorIndices[MeshCache.Colors[i]]);
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
