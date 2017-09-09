using System;
using System.Globalization;

namespace UnityFBXExporter
{
    public static class FbxValueSerializer
    {
        /// <summary>
        /// Serializes <see cref="FbxValue"/>, <see cref="char"/>, <see cref="long"/>, <see cref="int"/>, <see cref="float"/>, Array of <see cref="int"/>, Array of <see cref="float"/>
        /// </summary>
        /// <returns>Fbx representation of the value</returns>
        public static string Serialize(object value)
        {
            return value.GetType().IsArray ? SerializeCollection(value as Array) : SerializeValue(value);
        }

        private static string SerializeValue(object value)
        {
            if (!(value is FbxValue) && !(value is char) && !(value is long) && !(value is int) && !(value is float))
            {
                throw new ArgumentException("Unsupported value type for serialization: " + value.GetType(), "value");
            }
            return value.ToString();
        }

        public static string SerializeCollection(Array collection)
        {
            if (collection is float[])
            {
                return string.Join(",", Array.ConvertAll((float[])collection, i => i.ToString(CultureInfo.InvariantCulture)));
            }
            if (collection is int[])
            {
                return string.Join(",", Array.ConvertAll((int[])collection, i => i.ToString()));
            }
            throw new ArgumentException("Unsupported value type for serialization: " + collection.GetType(), "collection");
        }
    }
}
