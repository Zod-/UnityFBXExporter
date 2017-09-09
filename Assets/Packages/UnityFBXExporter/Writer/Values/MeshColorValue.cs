using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UnityFBXExporter
{
    public class MeshColorValue : MeshCacheValue
    {
        private readonly Dictionary<Color, int> _colorIndices;

        public MeshColorValue(MeshCache meshCache, Dictionary<Color, int> colorIndices) : base(meshCache)
        {
            _colorIndices = colorIndices;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(_colorIndices.Count * 4 * 3);
            foreach (var color in _colorIndices)
            {
                sb.AppendFormat("{0},{1},{2},{3},", color.Key.r, color.Key.g, color.Key.b, color.Key.a);
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
