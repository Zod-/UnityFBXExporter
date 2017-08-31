﻿using System.Collections.Generic;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxGeometryNode : FbxClassNode
    {
        private readonly Mesh _mesh;

        public FbxGeometryNode(Mesh mesh) : base("Geometry", InstaceId(mesh), "Mesh", "Mesh")
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
            layerElements.ForEach(Nodes.Add);

            Nodes.Add(new FbxLayerNode(0, layerElements));
        }

        private void Vertices()
        {
            var vertices = new float[_mesh.vertices.Length * 3];
            for (var i = 0; i < _mesh.vertices.Length; i++)
            {
                var meshVertex = FbxExporter.ReverseTransformUnityCoordinate(_mesh.vertices[i]);
                vertices[i] = meshVertex.x;
                vertices[i + 1] = meshVertex.y;
                vertices[i + 2] = meshVertex.z;
            }
            ArrayNode("Vertices", vertices);
        }

        private void PolygonVertexIndex()
        {
            ArrayNode("PolygonVertexIndex", FbxExporter.FlipYZTriangles(_mesh.triangles, true));
        }
    }
}
