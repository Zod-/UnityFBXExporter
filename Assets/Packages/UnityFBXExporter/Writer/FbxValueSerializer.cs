using System;
using UnityEngine;

namespace UnityFBXExporter
{
    public static class FbxValueSerializer
    {
        public static string Serialize(object value)
        {
            return value.GetType().IsArray ? SerializeCollection((object[])value) : SerializeValue(value);
        }

        public static string SerializeValue(object value)
        {
            if (value is string)
            {
                return string.Format("\"{0}\"", value);
            }
            if (value is Color)
            {
                var color = (Color)value;
                return SerializeCollection(new object[] { color.r, color.g, color.b });
            }
            if (value is Vector3)
            {
                var color = (Vector3)value;
                return SerializeCollection(new object[] { color.x, color.y, color.z });
            }
            return value.ToString();
        }

        public static string SerializeCollection(object[] collection)
        {
            return string.Join(",", Array.ConvertAll(collection, i => i.ToString()));
        }
    }
}
