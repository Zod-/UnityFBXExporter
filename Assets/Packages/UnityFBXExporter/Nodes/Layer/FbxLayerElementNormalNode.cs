using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxLayerElementNormalNode : FbxLayerElementBaseNode
    {
        private Mesh _mesh;
        public FbxLayerElementNormalNode(int layer, Mesh mesh) : base("LayerElementNormal", layer, "", "ByPolygonVertex", "Direct")
        {
            _mesh = mesh;

            Normals();
        }

        private void Normals()
        {
            var triangles = _mesh.triangles;
            var meshNormals = _mesh.normals;
            var normals = new float[triangles.Length * 3];

            for (int i = 0, j = 0; i < triangles.Length; i += 3, j += 9)
            {
                var normal = meshNormals[triangles[i]];
                normals[j] = normal.x * -1;
                normals[j + 1] = normal.y;
                normals[j + 2] = normal.z;

                normal = meshNormals[triangles[i + 2]];
                normals[j + 3] = normal.x * -1;
                normals[j + 4] = normal.y;
                normals[j + 5] = normal.z;

                normal = meshNormals[triangles[i + 1]];
                normals[j + 6] = normal.x * -1;
                normals[j + 7] = normal.y;
                normals[j + 8] = normal.z;
            }
            ArrayNode("Normals", normals);
        }
    }
}
