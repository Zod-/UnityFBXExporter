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
            var uv = new float[meshUv.Length * 2];
            for (int i = 0, j = 0; i < meshUv.Length; i++, j += 2)
            {
                uv[j] = meshUv[i].x;
                uv[j + 1] = meshUv[i].y;
            }
            ArrayNode("UV", uv);
        }

        private void UvIndex()
        {
            ArrayNode("UVIndex", FbxExporter.FlipYZTriangles(_mesh.triangles));
        }
    }
}
