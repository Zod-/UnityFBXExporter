using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxModelNode : FbxNode
    {
        public new string Name { get { return "Model"; } }
        public new string Class { get { return _gameObject.name; } }

        private readonly GameObject _gameObject;

        public FbxModelNode(GameObject gameObject)
        {
            Id = FBXExporter.GetRandomFBXId();
            SubClass = gameObject.HasMesh() ? "Mesh" : "Null";
            _gameObject = gameObject;
            Node("Version", 232);
            Node("Shading", 'T');
            Node("Culling", "CullingOff");
            Property("RotationOrder", "enum", "", "", 4);
            Property("RotationActive", "bool", "", "", 1);
            Property("InheritType", "enum", "", "", 1);
            Property("ScalingMax", "Vector3D", "Vector", "", Vector3.zero);
            Property("DefaultAttributeIndex", "int", "Integer", "", 0);
            Property("currentUVSet", "KString", "", "U", "map1");
            Position();
            Rotation();
            Scaling();
        }

        private void Scaling()
        {
            var scale = _gameObject.transform.localScale;
            Property("Lcl Scaling", "Lcl Scaling", "", "A", scale);
        }

        private void Rotation()
        {
            var rot = _gameObject.transform.localEulerAngles;
            rot.y *= -1;
            rot.z *= -1;
            Property("Lcl Rotation", "Lcl Rotation", "", "A+", rot);
        }

        private void Position()
        {
            var pos = _gameObject.transform.localPosition;
            pos.x *= -1;
            Property("Lcl Translation", "Lcl Translation", "", "A+", pos);
        }
    }
}
