using System.Collections.Generic;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxObjectsNode : FbxNode
    {
        public override string Header
        {
            get
            {
                return
@"; Object properties
;------------------------------------------------------------------
";
            }
        }
        private readonly FbxConnectionsNode _connections;
        private readonly HashSet<long> _geometryCache = new HashSet<long>();
        private readonly HashSet<long> _materialCache = new HashSet<long>();

        public FbxObjectsNode(IEnumerable<GameObject> gameObjects, FbxConnectionsNode connections) : base("Objects")
        {
            _connections = connections;
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
                ChildNodes.Add(new FbxModelNode(gameObject));
                var parentId = gameObject == root ? 0 : InstanceId(gameObject.transform.parent.gameObject);
                _connections.Add(parentId, InstanceId(gameObject));
            }
        }

        private void AddGeometry(GameObject root)
        {
            foreach (var transform in root.transform.EnumerateHierarchy())
            {
                var gameObject = transform.gameObject;
                var mesh = gameObject.GetMesh();
                if (mesh == null) { continue; }

                if (!_geometryCache.Contains(InstanceId(mesh)))
                {
                    ChildNodes.Add(new FbxGeometryNode(mesh));
                    _geometryCache.Add(InstanceId(mesh));
                }
                _connections.Add(InstanceId(gameObject), InstanceId(mesh));
            }
        }

        private void AddMaterials(GameObject root)
        {
            foreach (var renderer in root.GetComponentsInChildren<MeshRenderer>())
            {
                foreach (var mat in renderer.sharedMaterials)
                {
                    if (mat == null) { continue; }
                    if (!_materialCache.Contains(InstanceId(mat)))
                    {
                        ChildNodes.Add(new FbxMaterialNode(mat));
                        _materialCache.Add(InstanceId(mat));
                    }
                    _connections.Add(InstanceId(renderer.gameObject), InstanceId(mat));
                }
            }
        }
    }
}
