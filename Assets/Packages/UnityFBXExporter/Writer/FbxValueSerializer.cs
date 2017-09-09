using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace UnityFBXExporter
{
    public static class FbxValueSerializer
    {
        private static readonly Dictionary<Type, Func<object, string>> Switch = new Dictionary<Type, Func<object, string>>{
            { typeof(string), SerializeString},
            { typeof(Color), SerializeColor},
            { typeof(Vector3), SerializeVector3},
            { typeof(VerticesValue), SerializeVertices},
            { typeof(PolygonVertexIndexValue), SerializePolygonVertexIndexes},
            { typeof(ColorValue), SerializeColorValue},
            { typeof(ColorIndexValue), SerializeColorIndexes},
            { typeof(NormalsValue), SerializeNormals},
            { typeof(UvValue), SerializeUv},
            { typeof(UvIndexValue), SerializeUvIndex}
        };

        public static string Serialize(object value)
        {
            return value.GetType().IsArray ? SerializeCollection(value as Array) : SerializeValue(value);
        }

        public static string SerializeValue(object value)
        {
            var type = value.GetType();
            return Switch.ContainsKey(type) ? Switch[type](value) : value.ToString();
        }

        private static string SerializeUv(object value)
        {
            var meshUv = ((UvValue)value).MeshCache.Uv;
            var sb = new StringBuilder(meshUv.Length * 4);
            for (var i = 0; i < meshUv.Length; i++)
            {
                sb.AppendFormat("{0},{1},", meshUv[i].x, meshUv[i].y);
            }
            return sb.ToString().TrimEnd(',');
        }

        private static string SerializeUvIndex(object value)
        {
            return SerializeCollection(((UvIndexValue)value).MeshCache.FlippedTriangles);
        }

        private static string SerializeNormals(object value)
        {
            var normalValue = (NormalsValue)value;
            var triangles = normalValue.MeshCache.FlippedTriangles;
            var sb = new StringBuilder(triangles.Length * 3);
            foreach (var triangle in triangles)
            {
                var normal = normalValue.MeshCache.Normals[triangle];
                sb.AppendFormat("{0},{1},{2},", normal.x * -1, normal.y, normal.z);
            }
            return sb.ToString().TrimEnd(',');
        }

        private static string SerializeColorIndexes(object value)
        {
            var colorIndexValue = (ColorIndexValue)value;
            var trianglesLength = colorIndexValue.MeshCache.FlippedTriangles.Length;
            var sb = new StringBuilder(trianglesLength * 2);
            for (var i = 0; i < trianglesLength; i++)
            {
                sb.AppendFormat("{0},", colorIndexValue.ColorIndices[colorIndexValue.MeshCache.Colors[i]]);
            }
            return sb.ToString().TrimEnd(',');
        }

        private static string SerializeColorValue(object value)
        {
            var colorValue = (ColorValue)value;
            var sb = new StringBuilder(colorValue.ColorIndices.Count * 4 * 3);
            foreach (var color in colorValue.ColorIndices)
            {
                sb.AppendFormat("{0},{1},{2},{3},", color.Key.r, color.Key.g, color.Key.b, color.Key.a);
            }
            return sb.ToString().TrimEnd(',');
        }

        private static string SerializePolygonVertexIndexes(object value)
        {
            var triangles = ((PolygonVertexIndexValue)value).MeshCache.Triangles;
            var flippedTriangles = FbxExporter.FlipYZTriangles(triangles, true);
            return SerializeCollection(flippedTriangles);
        }

        private static string SerializeString(object value)
        {
            return string.Format("\"{0}\"", value);
        }

        private static string SerializeVertices(object value)
        {
            var meshVertices = ((VerticesValue)value).MeshCache.Vertices;
            var sb = new StringBuilder(meshVertices.Length * 3);
            for (var i = 0; i < meshVertices.Length; i++)
            {
                sb.AppendFormat("{0},{1},{2},", meshVertices[i].x * -1, meshVertices[i].y, meshVertices[i].z);
            }
            return sb.ToString().TrimEnd(',');
        }

        private static string SerializeVector3(object value)
        {
            var vector = (Vector3)value;
            return SerializeCollection(new[] { vector.x, vector.y, vector.z });
        }

        private static string SerializeColor(object value)
        {
            var color = (Color)value;
            return SerializeCollection(new[] { color.r, color.g, color.b });
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
            return string.Empty;
        }
    }
}
