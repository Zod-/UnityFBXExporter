using System.Collections.Generic;

namespace UnityFBXExporter
{
    public static class FbxAsciiWriter
    {
        public static IEnumerable<string> SerializeDocument(FbxDocument document)
        {
            return SerializeChildNodes(document.ChildNodes, 0);
        }

        public static IEnumerable<string> SerializeGenericNode(FbxNode node, int indent)
        {
            if (string.IsNullOrEmpty(node.Header))
            {
                yield return node.Header;
            }
            yield return OpenObject(node, indent);
            foreach (var childLines in SerializeChildNodes(node.ChildNodes, indent + 1))
            {
                yield return childLines;
            }
            foreach (var childLines in SerializeProperties(node.Properties, indent + 1))
            {
                yield return childLines;
            }
            yield return CloseObject(indent);
        }

        public static IEnumerable<string> SerializeProperties(List<FbxNode> nodes, int indent)
        {
            if (nodes.Count == 0) { yield break; }
            yield return OpenObject(nodes[0], indent);
            foreach (var childLines in SerializeChildNodes(nodes, indent + 1))
            {
                yield return childLines;
            }
            yield return CloseObject(indent);
        }

        public static IEnumerable<string> SerializeChildNodes(List<FbxNode> nodes, int indent)
        {
            foreach (var node in nodes)
            {
                if (node is FbxValueNode)
                {
                    yield return SerializeIndentedObject(node, indent);
                }
                else
                {
                    foreach (var childLines in SerializeGenericNode(node, indent))
                    {
                        yield return childLines;
                    }
                }
            }
        }

        public static string OpenObject(FbxNode node, int indent)
        {
            return SerializeIndentedObject(node.GetFormattedMetaName(), indent);
        }

        public static string CloseObject(int indent)
        {
            return SerializeIndentedObject("}", indent);
        }

        public static string SerializeIndentedObject(object obj, int indent)
        {
            return string.Format("{0}{1}", Indent(indent), obj);
        }

        public static string Indent(int indent)
        {
            return new string('\t', indent);
        }
    }
}
