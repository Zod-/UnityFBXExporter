using System.Collections.Generic;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxLayerElementColorNode : FbxLayerElementBaseNode
    {
        private readonly Mesh _mesh;

        public FbxLayerElementColorNode(int layer, Mesh mesh) : base("LayerElementColor", layer, "Col", "ByPolygonVertex", "IndexToDirect")
        {
            _mesh = mesh;
            var colorIndices = CalculateColorIndices();
            Colors(colorIndices);
            ColorIndex(colorIndices);
        }

        private void Colors(Dictionary<Color, int> colorIndices)
        {
            var colors = new List<float>(colorIndices.Count * 4);
            foreach (var color in colorIndices)
            {
                colors.Add(color.Key.r);
                colors.Add(color.Key.g);
                colors.Add(color.Key.b);
                colors.Add(color.Key.a);
            }
            ArrayNode("Colors", colors);
        }

        private void ColorIndex(Dictionary<Color, int> colorIndices)
        {
            var colors = _mesh.colors;
            var triangles = _mesh.triangles;
            var indices = new int[triangles.Length];
            for (var i = 0; i < triangles.Length; i++)
            {
                indices[i] = colorIndices[colors[i]];
            }
            ArrayNode("ColorIndex", FbxExporter.FlipYZTriangles(indices));
        }

        private Dictionary<Color, int> CalculateColorIndices()
        {
            var colors = _mesh.colors;
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
