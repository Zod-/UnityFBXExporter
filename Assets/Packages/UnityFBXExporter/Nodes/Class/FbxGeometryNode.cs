using System.Collections.Generic;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxGeometryNode : FbxClassNode
    {
        private readonly Mesh _mesh;

        public FbxGeometryNode(Mesh mesh) : base("Geometry", InstanceId(mesh), "Mesh", "Mesh")
        {
            _mesh = mesh;

            Node("GeometryVersion", 124);
            Vertices();
            PolygonVertexIndex();
            Layer();
        }

        private void Layer()
        {
            var containsColor = _mesh.colors.Length == _mesh.vertices.Length;
            var layerElements = new List<FbxLayerElementBaseNode>();

            layerElements.Add(new FbxLayerElementNormalNode(0, _mesh));
            layerElements.Add(new FbxLayerElementUvNode(0, _mesh));
            layerElements.Add(new FbxLayerElementMaterialNode(0, _mesh));
            if (containsColor)
            {
                layerElements.Add(new FbxLayerElementColorNode(0, _mesh));
            }
            layerElements.ForEach(ChildNodes.Add);

            ChildNodes.Add(new FbxLayerNode(0, layerElements));
        }

        private void Vertices()
        {
            var meshVertices = _mesh.vertices;
            var vertices = new float[meshVertices.Length * 3];
            for (var i = 0; i < meshVertices.Length; i++)
            {
                vertices[i] = meshVertices[i].x * -1;
                vertices[i + 1] = meshVertices[i].y;
                vertices[i + 2] = meshVertices[i].z;
            }
            ArrayNode("Vertices", vertices);
        }

        private void PolygonVertexIndex()
        {
            ArrayNode("PolygonVertexIndex", FbxExporter.FlipYZTriangles(_mesh.triangles, true));
        }
    }
}
