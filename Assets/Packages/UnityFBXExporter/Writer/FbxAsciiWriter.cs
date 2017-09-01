using System.Collections.Generic;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxAsciiWriter
    {
        private readonly IWriter _writer;

        public FbxAsciiWriter(IWriter writer)
        {
            _writer = writer;
        }

        public void WriteGameObjects(GameObject[] gameObjects, string path)
        {
            WriteChildNodes(new FbxDocument(gameObjects, path).ChildNodes, 0);
        }

        internal void WriteGenericNode(FbxNode node, int indent)
        {
            WriteCommentHeader(node);
            OpenObject(node, indent);
            WriteChildNodes(node.ChildNodes, indent + 1); //recursive ayy
            WriteProperties(node.Properties, indent + 1);
            CloseObject(indent);
        }

        internal void WriteProperties(List<FbxNode> nodes, int indent)
        {
            if (nodes.Count == 0) { return; }
            OpenObject(nodes[0], indent);
            WriteChildNodes(nodes, indent + 1);
            CloseObject(indent);
        }

        internal void WriteChildNodes(List<FbxNode> nodes, int indent)
        {
            foreach (var node in nodes)
            {
                if (node is FbxValueNode)
                {
                    WriteIndentedObject(node, indent);
                }
                else
                {
                    WriteGenericNode(node, indent);
                }
            }
        }

        internal void WriteCommentHeader(FbxNode node)
        {
            if (string.IsNullOrEmpty(node.Header)) { return; }
            WriteIndentedObject("}", 0);
        }

        internal void OpenObject(FbxNode node, int indent)
        {
            WriteIndentedObject(node.GetFormattedMetaName(), indent);
        }

        internal void CloseObject(int indent)
        {
            WriteIndentedObject("}", indent);
        }

        internal void WriteIndentedObject(object obj, int indent)
        {
            _writer.AppendLine(string.Format("{0}{1}", Indent(indent), obj));
        }

        internal static string Indent(int indent)
        {
            return new string('\t', indent);
        }
    }
}
