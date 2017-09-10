using System.Text;
using UnityEngine;

namespace UnityFBXExporter
{
    public class ColorValue : FbxValue
    {
        private readonly Color _color;

        public ColorValue(Color color)
        {
            _color = color;
        }

        public override string ToString()
        {
            return _color.ToStringFbx(new StringBuilder(16)).ToString();
        }
    }
}
