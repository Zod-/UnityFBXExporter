using UnityEngine;

namespace UnityFBXExporter
{
    public class Vector3Value : FbxValue
    {
        private readonly Vector3 _vector3;

        public Vector3Value(Vector3 vector3)
        {
            _vector3 = vector3;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", _vector3.x, _vector3.y, _vector3.z);
        }
    }
}
