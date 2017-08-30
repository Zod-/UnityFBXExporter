using System.Collections.Specialized;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxModelNode : FBXNode
    {
        public new string Name { get { return "Material"; } }
        public new string Class { get { return _gameObject.name; } }

        private readonly GameObject _gameObject;

        public FbxModelNode(GameObject gameObject)
        {
            Id = FBXExporter.GetRandomFBXId();
            SubClass = gameObject.HasMesh() ? "Mesh" : "Null";
            _gameObject = gameObject;
        }

    }
}
