using System.Collections.Generic;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxLayerElementColorNode : FbxLayerElementBaseNode
    {
        private readonly MeshCache _cache;

        public FbxLayerElementColorNode(int layer, MeshCache cache) : base("LayerElementColor", layer, "Col", "ByPolygonVertex", "IndexToDirect")
        {
            _cache = cache;
            var colorIndices = CalculateColorIndices();
            ArrayNode("Colors", new MeshColorValue(_cache, colorIndices), colorIndices.Count * 4);
            ArrayNode("ColorIndex", new MeshColorIndexValue(_cache, colorIndices), _cache.FlippedTriangles.Length);
        }

        private Dictionary<Color, int> CalculateColorIndices()
        {
            var colors = _cache.Colors;
            var colorTable = new Dictionary<Color, int>(colors.Length); // reducing amount of data by only keeping unique colors.
            var idx = 0;

            // build index table of all the different colors present in the mesh            
            foreach (var color in colors)
            {
                if (colorTable.ContainsKey(color)) { continue; }
                colorTable[color] = idx;
                idx++;
            }
            return colorTable;
        }
    }
}
