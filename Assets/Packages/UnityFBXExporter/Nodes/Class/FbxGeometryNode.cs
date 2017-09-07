using System.Collections.Generic;
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
        public Mesh Mesh { get; private set; }
    }
    public class FbxGeometryNode : FbxClassNode
    {
        private readonly MeshCache _cache;

        public FbxGeometryNode(Mesh mesh) : base("Geometry", InstanceId(mesh), "Mesh", "Mesh")
        {
            _cache = new MeshCache(mesh);

            Node("GeometryVersion", 124);
            ArrayNode("Vertices", new VerticesValue(_cache), _cache.Vertices.Length * 3);
            ArrayNode("PolygonVertexIndex", new PolygonVertexIndexValue(_cache), _cache.Triangles.Length);
            Layer();
        }

        private void Layer()
        {
            var containsColor = _cache.Colors.Length == _cache.Vertices.Length;
            var layerElements = new List<FbxLayerElementBaseNode>
            {
                new FbxLayerElementNormalNode(0, _cache),
                new FbxLayerElementUvNode(0, _cache),
                new FbxLayerElementMaterialNode(0, _cache)
            };

            if (containsColor)
            {
                layerElements.Add(new FbxLayerElementColorNode(0, _cache));
            }
            layerElements.ForEach(ChildNodes.Add);

            ChildNodes.Add(new FbxLayerNode(0, layerElements));
        }
    }

    public class VerticesValue : MeshCacheValue
    {
        public VerticesValue(MeshCache meshCache) : base(meshCache)
        {
        }
    }

    public class PolygonVertexIndexValue : MeshCacheValue
    {
        public PolygonVertexIndexValue(MeshCache meshCache) : base(meshCache)
        {
        }
    }
}
