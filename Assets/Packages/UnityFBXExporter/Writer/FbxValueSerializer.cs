using System;
using System.Collections;
using System.Globalization;

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
