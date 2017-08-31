using System.Collections.Generic;
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
            var normals = new List<float>(triangles.Length * 3);

            for (var i = 0; i < triangles.Length; i += 3)
            {
                var normal = FbxExporter.ReverseTransformUnityCoordinate(meshNormals[triangles[i]]);
                normals.Add(normal.x);
                normals.Add(normal.y);
                normals.Add(normal.z);

                normal = FbxExporter.ReverseTransformUnityCoordinate(meshNormals[triangles[i + 2]]);
                normals.Add(normal.x);
                normals.Add(normal.y);
                normals.Add(normal.z);

                normal = FbxExporter.ReverseTransformUnityCoordinate(meshNormals[triangles[i + 1]]);
                normals.Add(normal.x);
                normals.Add(normal.y);
                normals.Add(normal.z);
            }
            ArrayNode("Normals", normals);
        }
    }
}
