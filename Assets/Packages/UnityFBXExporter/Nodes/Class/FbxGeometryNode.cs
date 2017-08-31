using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

namespace UnityFBXExporter
{
    public class MeshCache
    {
        public MeshCache(Mesh mesh)
        {
            Mesh = mesh;
            Vertices = mesh.vertices;
            Triangles = FbxExporter.FlipYZTriangles(mesh.triangles);
            Colors = mesh.colors;
            Normals = mesh.normals;
            Uv = mesh.uv;
        }

        public Vector2[] Uv { get; private set; }

        public Vector3[] Normals { get; private set; }

        public Color[] Colors { get; private set; }

        public int[] Triangles { get; private set; }

        public Vector3[] Vertices { get; private set; }
        public Mesh Mesh{ get; private set; }
    }

    public class FbxGeometryNode : FbxClassNode
    {
        private readonly MeshCache _cache;

        public FbxGeometryNode(Mesh mesh) : base("Geometry", InstanceId(mesh), "Mesh", "Mesh")
        {
            _cache = new MeshCache(mesh);

            Node("GeometryVersion", 124);
            Vertices();
            PolygonVertexIndex();
            Layer();
        }

        private void Layer()
        {
            var containsColor = _cache.Colors.Length == _cache.Vertices.Length;
            var layerElements = new List<FbxLayerElementBaseNode>();

            layerElements.Add(new FbxLayerElementNormalNode(0, _cache));
            layerElements.Add(new FbxLayerElementUvNode(0, _cache));
            layerElements.Add(new FbxLayerElementMaterialNode(0, _cache));
            if (containsColor)
            {
                layerElements.Add(new FbxLayerElementColorNode(0, _cache));
            }
            layerElements.ForEach(ChildNodes.Add);

            ChildNodes.Add(new FbxLayerNode(0, layerElements));
        }

        private void Vertices()
        {
            var meshVertices = _cache.Vertices;
            var vertices = new float[meshVertices.Length * 3];
            for (int i = 0, j = 0; i < meshVertices.Length; i++, j += 3)
            {
                vertices[j] = meshVertices[i].x * -1;
                vertices[j + 1] = meshVertices[i].y;
                vertices[j + 2] = meshVertices[i].z;
            }
            ArrayNode("Vertices", vertices);
        }

        private void PolygonVertexIndex()
        {
            ArrayNode("PolygonVertexIndex", FbxExporter.FlipYZTriangles(_cache.Mesh.triangles, true));
        }
    }
}
