using System;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace UnityFBXExporter
{
    public static class FbxValueSerializer
    {
        public static string Serialize(object value)
        {
            return value.GetType().IsArray ? SerializeCollection(value as Array) : SerializeValue(value);
        }

        public static string SerializeValue(object value)
        {
            if (value is string)
            {
                return SerializeString((string)value);
            }
            if (value is Color)
            {
                return SerializeColor((Color)value);
            }
            if (value is Vector3)
            {
                return SerializeVector3((Vector3)value);
            }
            if (value is VerticesValue)
            {
                return SerializeVertices((VerticesValue)value);
            }
            if (value is PolygonVertexIndexValue)
            {
                return SerializePolygonVertexIndexes((PolygonVertexIndexValue)value);
            }
            if (value is ColorValue)
            {
                return SerializeColors((ColorValue)value);
            }
            if (value is ColorIndexValue)
            {
                return SerializeColorIndexes((ColorIndexValue)value);
            }
            if (value is NormalsValue)
            {
                return SerializeNormals((NormalsValue)value);
            }
            if (value is UvValue)
            {
                return SerializeUv((UvValue)value);
            }
            if (value is UvIndexValue)
            {
                return SerializeUvIndex((UvIndexValue)value);
            }
            return value.ToString();
        }

        private static string SerializeUv(UvValue value)
        {
            var meshUv = value.MeshCache.Uv;
            var sb = new StringBuilder(meshUv.Length * 4);
            for (int i = 0; i < meshUv.Length; i++)
            {
                sb.AppendFormat("{0},{1}", meshUv[i].x, meshUv[i].y);
                if (i != meshUv.Length - 1)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        private static string SerializeUvIndex(UvIndexValue value)
        {
            return SerializeCollection(value.MeshCache.Triangles);
        }

        private static string SerializeNormals(NormalsValue value)
        {
            var triangles = value.MeshCache.Triangles;
            var meshNormals = value.MeshCache.Normals;
            var sb = new StringBuilder(triangles.Length * 3);
            for (var i = 0; i < triangles.Length; i++)
            {
                var normal = meshNormals[triangles[i]];
                sb.AppendFormat("{0},{1},{2}", normal.x * -1, normal.y, normal.z);
                if (i != triangles.Length - 1)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        private static string SerializeColorIndexes(ColorIndexValue value)
        {
            var colors = value.MeshCache.Colors;
            var trianglesLength = value.MeshCache.Triangles.Length;
            var sb = new StringBuilder(trianglesLength * 2);
            for (var i = 0; i < trianglesLength; i++)
            {
                sb.Append(value.ColorIndices[colors[i]]);
                if (i != trianglesLength - 1)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        private static string SerializeColors(ColorValue value)
        {
            var sb = new StringBuilder(value.ColorIndices.Count * 4 * 3);
            var i = 0;
            var last = value.ColorIndices.Count - 1;
            foreach (var color in value.ColorIndices)
            {
                sb.AppendFormat("{0},{1},{2},{3}", color.Key.r, color.Key.g, color.Key.b, color.Key.a);
                if (i != last)
                {
                    sb.Append(",");
                }
                i++;
            }
            return sb.ToString();
        }

        private static string SerializePolygonVertexIndexes(PolygonVertexIndexValue value)
        {
            return SerializeCollection(FbxExporter.FlipYZTriangles(value.MeshCache.Mesh.triangles, true));
        }

        private static string SerializeString(string value)
        {
            return string.Format("\"{0}\"", value);
        }

        private static string SerializeVertices(VerticesValue value)
        {
            var meshVertices = value.MeshCache.Vertices;
            var sb = new StringBuilder(meshVertices.Length * 3);
            for (var i = 0; i < meshVertices.Length; i++)
            {
                sb.AppendFormat("{0},{1},{2}", meshVertices[i].x * -1, meshVertices[i].y, meshVertices[i].z);
                if (i != meshVertices.Length - 1)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        private static string SerializeVector3(Vector3 vector)
        {
            return SerializeCollection(new[] { vector.x, vector.y, vector.z });
        }

        private static string SerializeColor(Color color)
        {
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
