using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityFBXExporter
{
    public class SampleData
    {
        public static string GetTestData(string relativePath)
        {
            return File.ReadAllText("Assets\\Packages\\UnityFBXExporter\\Editor\\Nodes\\" + relativePath);
        }

        public static string ToLines(IEnumerable<string> serializedNode)
        {
            return string.Join("\r\n", serializedNode.ToArray());
        }
    }
}
