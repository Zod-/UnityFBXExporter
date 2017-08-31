using System;
using System.Collections;
using System.Text;
using UnityEngine;

namespace UnityFBXExporter
{
    public static class FbxValueSerializer
    {
        public static string Serialize(object value)
        {
            var collection = value as ICollection;
            return collection != null ? SerializeCollection(collection) : SerializeValue(value);
        }

        private static string SerializeValue(object value)
        {
            if (IsPrimitive(value))
            {
                return value.ToString();
            }
            if (value is string)
            {
                return string.Format("\"{0}\"", value);
            }
            if (value is Color)
            {
                return SerializeColor((Color)value);
            }
            if (value is Vector3)
            {
                return SerializeVector3((Vector3)value);
            }
            Debug.LogError("Type unknown " + value);
            return string.Empty;
        }

        private static bool IsPrimitive(object value)
        {
            if (value is int)
            {
                return true;
            }
            if (value is long)
            {
                return true;
            }
            if (value is float)
            {
                return true;
            }
            if (value is char)
            {
                return true;
            }
            return false;
        }

        private static string SerializeVector3(Vector3 value)
        {
            return string.Format("{0},{1},{2}", value.x, value.y, value.z);
        }

        private static string SerializeColor(Color color)
        {
            return string.Format("{0},{1},{2}", color.r, color.g, color.b);
        }

        private static string SerializeCollection(ICollection collection)
        {
            var sb = new StringBuilder(collection.Count);
            var i = 0;
            foreach (var value in collection)
            {
                sb.Append(SerializeValue(value));
                if (i != collection.Count - 1)
                {
                    sb.Append(',');
                }
                i++;
            }
            return sb.ToString();
        }
    }
}
