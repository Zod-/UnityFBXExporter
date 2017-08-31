using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxObjectsNode : FbxNode
    {
        private readonly FbxConnectionsNode _connections;
        private readonly HashSet<long> _geometryCache = new HashSet<long>();
        private readonly HashSet<long> _materialCache = new HashSet<long>();

        public FbxObjectsNode(GameObject[] gameObjects, FbxConnectionsNode connections)
        {
            _connections = connections;
            Name = "Objects";
            foreach (var gameObject in gameObjects)
            {
                AddModels(gameObject);
                AddGeometry(gameObject);
                AddMaterials(gameObject);
            }
        }

        private void AddModels(GameObject root)
        {
            foreach (var transform in root.transform.EnumerateHierarchy())
            {
                var gameObject = transform.gameObject;
                ModelNode(gameObject);
                var parentId = gameObject == root ? 0 : InstaceId(gameObject.transform.parent);
                _connections.Add(parentId, InstaceId(gameObject));
            }
        }

        private void AddGeometry(GameObject root)
        {
            foreach (var transform in root.transform.EnumerateHierarchy())
            {
                var gameObject = transform.gameObject;
                var mesh = gameObject.GetMesh();
                if (mesh == null) { continue; }

                if (!_geometryCache.Contains(InstaceId(mesh)))
                {
                    GeometryNode(mesh);
                    _geometryCache.Add(InstaceId(mesh));
                }
                _connections.Add(InstaceId(mesh), InstaceId(gameObject));
            }
        }

        private void AddMaterials(GameObject root)
        {
            foreach (var renderer in root.GetComponentsInChildren<MeshRenderer>())
            {
                foreach (var mat in renderer.sharedMaterials)
                {
                    if (mat == null) { continue; }
                    if (!_materialCache.Contains(InstaceId(mat)))
                    {
                        MaterialNode(mat);
                        _materialCache.Add(InstaceId(mat));
                    }
                    _connections.Add(InstaceId(mat), InstaceId(renderer.gameObject));
                }
            }
        }
    }
}
