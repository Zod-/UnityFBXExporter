using System;
using System.Globalization;
using UnityEngine;

namespace UnityFBXExporter
{
    public static class FbxValueSerializer
    {
        public static string Serialize(object value)
        {
            return value.GetType().IsArray ? SerializeCollection(value as Array) : SerializeValue(value);
        }

        private static string SerializeValue(object value)
        {
            if (value is string)
            {
                return string.Format("\"{0}\"", value);
            }
            if (value is Color)
            {
                var color = (Color)value;
                return SerializeCollection(new[] { color.r, color.g, color.b });
            }
            if (value is Vector3)
            {
                var color = (Vector3)value;
                return SerializeCollection(new[] { color.x, color.y, color.z });
            }
            return value.ToString();
        }

        private static string SerializeCollection(Array collection)
        {
            if (collection is float[])
            {
                return string.Join(",", Array.ConvertAll((float[])collection, i => i.ToString(CultureInfo.InvariantCulture)));
            }
            return string.Join(",", Array.ConvertAll((int[])collection, i => i.ToString()));
        }
    }
}
