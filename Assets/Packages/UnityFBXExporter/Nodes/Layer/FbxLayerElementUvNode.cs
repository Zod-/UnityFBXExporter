using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxLayerElementUvNode : FbxLayerElementBaseNode
    {
        private readonly MeshCache _cache;

        public FbxLayerElementUvNode(int layer, MeshCache cache) : base("LayerElementUV", layer, "map1", "ByPolygonVertex", "IndexToDirect")
        {
            _cache = cache;
            Uv();
            UvIndex();
        }

        private void Uv()
        {
            var meshUv = _cache.Uv;
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
            ArrayNode("UVIndex", _cache.Triangles);
        }
    }
}
