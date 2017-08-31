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
            Colors(colorIndices);
            ColorIndex(colorIndices);
        }

        private void Colors(Dictionary<Color, int> colorIndices)
        {
            var colors = new float[colorIndices.Count * 4];
            var i = 0;
            foreach (var color in colorIndices)
            {
                colors[i] = color.Key.r;
                colors[i + 1] = color.Key.g;
                colors[i + 2] = color.Key.b;
                colors[i + 3] = color.Key.a;
                i += 4;
            }
            ArrayNode("Colors", colors);
        }

        private void ColorIndex(Dictionary<Color, int> colorIndices)
        {
            var colors = _cache.Colors;
            var triangles = _cache.Triangles;
            var indices = new int[triangles.Length];
            for (var i = 0; i < triangles.Length; i++)
            {
                indices[i] = colorIndices[colors[i]];
            }
            ArrayNode("ColorIndex", FbxExporter.FlipYZTriangles(indices));
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
