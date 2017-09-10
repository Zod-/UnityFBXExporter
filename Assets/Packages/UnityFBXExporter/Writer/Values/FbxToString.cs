using System.Text;
using UnityEngine;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace UnityFBXExporter
{
    public static class FbxToString
    {
        internal static StringBuilder ToStringFbx(this Vector3 value, StringBuilder sb)
        {
            value.x.ToStringFbx(sb);
            sb.Append(',');
            value.y.ToStringFbx(sb);
            sb.Append(',');
            value.z.ToStringFbx(sb);
            return sb;
        }
        internal static StringBuilder ToStringFbx(this Color value, StringBuilder sb)
        {
            value.r.ToStringFbx(sb);
            sb.Append(',');
            value.g.ToStringFbx(sb);
            sb.Append(',');
            value.b.ToStringFbx(sb);
            return sb;
        }

        internal static StringBuilder ToStringFbxAlpha(this Color value, StringBuilder sb)
        {
            value.r.ToStringFbx(sb);
            sb.Append(',');
            value.g.ToStringFbx(sb);
            sb.Append(',');
            value.b.ToStringFbx(sb);
            sb.Append(',');
            value.a.ToStringFbx(sb);
            return sb;
        }

        internal static StringBuilder ToStringFbx(this Vector2 value, StringBuilder sb)
        {
            value.x.ToStringFbx(sb);
            sb.Append(',');
            value.y.ToStringFbx(sb);
            return sb;
        }

        internal static StringBuilder ToStringFbx(this float value, StringBuilder sb)
        {
            if (value == 0f)
            {
                sb.Append('0');
            }
            else if (value == 1f)
            {
                sb.Append('1');
            }
            else if (value == -1f)
            {
                sb.Append("-1");
            }
            else
            {
                sb.Append(value);
            }
            return sb;
        }
    }
}
