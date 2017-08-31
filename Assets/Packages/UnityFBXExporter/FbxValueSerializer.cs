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
            return value.ToString();
        }

        private static string SerializeCollection(ICollection collection)
        {
            var sb = new StringBuilder(collection.Count);
            var i = 0;
            foreach (var value in collection)
            {
                sb.Append(value);
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
