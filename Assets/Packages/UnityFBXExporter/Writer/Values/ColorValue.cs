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
            return string.Format("{0},{1},{2}", _color.r, _color.g, _color.b);
        }
    }
}
