using System.Collections.Generic;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxLayerElementUvNode : FbxLayerElementBaseNode
    {
        private readonly Mesh _mesh;

        public FbxLayerElementUvNode(int layer, Mesh mesh) : base("LayerElementUV", layer, "map1", "ByPolygonVertex", "IndexToDirect")
        {
            _mesh = mesh;
            Uv();
            UvIndex();
        }

        private void Uv()
        {
            var meshUv = _mesh.uv;
            var uv = new List<float>(meshUv.Length * 2);
            for (var i = 0; i < meshUv.Length; i++)
            {
                uv.Add(meshUv[i].x);
                uv.Add(meshUv[i].y);
            }
            ArrayNode("UV", uv);
        }

        private void UvIndex()
        {
            ArrayNode("UVIndex", FBXExporter.FlipYZTriangles(_mesh.triangles));
        }
    }
}
