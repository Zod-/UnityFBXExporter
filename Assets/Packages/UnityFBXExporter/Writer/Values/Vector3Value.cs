using System.Text;
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
            return _vector3.ToStringFbx(new StringBuilder(16)).ToString();
        }
    }
}
