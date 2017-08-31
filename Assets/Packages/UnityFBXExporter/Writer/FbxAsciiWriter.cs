using System.Collections.Generic;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxAsciiWriter
    {

        public const string FbxHeader = @"; FBX 7.3.0 project file
; Copyright (C) 1997-2010 Autodesk Inc. and/or its licensors.
; All rights reserved.
; ----------------------------------------------------
";
        private readonly IWriter _writer;
        private int _indent;

        public FbxAsciiWriter(IWriter writer)
        {
            _writer = writer;
        }

        public void WriteGameObjects(GameObject[] gameObjects, string path)
        {
            WriteFileHeader();
            WriteFbxGenericNode(new FbxHeaderExtensionNode(path));
            WriteFbxGenericNode(new FbxGlobalSettingsNode());
            WriteFbxGenericNode(new FbxDefinitionsNode());

            var connections = new FbxConnectionsNode();
            WriteFbxGenericNode(new FbxObjectsNode(gameObjects, connections));
            WriteFbxGenericNode(connections);
        }

        private void WriteFileHeader()
        {
            _writer.AppendLine(FbxHeader);
        }

        private void WriteFbxGenericNode(FbxNode node)
        {
            _writer.AppendLine(string.Format("{0}{1}: {2} {{", Indent(), node.Name, node.GetMetaName()));
            _indent++;
            WriteFbxChildNodes(node.ChildNodes); //recursive ayy
            WriteFbxProperties(node);
            WriteFbxConnectionsNode(node as FbxConnectionsNode);
            _indent--;
            _writer.AppendLine(string.Format("{0}}}", Indent()));
        }

        private void WriteFbxChildNodes(List<FbxNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (node is FbxValueNode)
                {
                    WriteFbxValueNode(node as FbxValueNode);
                }
                else
                {
                    WriteFbxGenericNode(node);
                }
            }
        }

        private void WriteFbxConnectionsNode(FbxConnectionsNode node)
        {
            if (node == null) { return; }
            node.Connections.ForEach(WriteFbxConnections);
        }

        private void WriteFbxConnections(FbxConnectionProperty conn)
        {
            _writer.AppendLine(string.Format("{0}C: \"{1}\",{2},{3}", Indent(), conn.Type, conn.ChildId, conn.ParentId));
        }


        private void WriteFbxProperties(FbxNode node)
        {
            if (node.Properties.Count == 0) { return; }

            _writer.AppendLine(string.Format("{0}Properties70:  {{", Indent()));
            _indent++;
            node.Properties.ForEach(WriteFbxProperty);
            _indent--;
            _writer.AppendLine(string.Format("{0}}}", Indent()));
        }
        private void WriteFbxProperty(FbxProperty prop)
        {
            _writer.AppendLine(
                string.Format(@"{0}P: ""{1}"", ""{2}"", ""{3}"", ""{4}"", {5}",
                Indent(),
                prop.Name,
                prop.Type,
                prop.Label,
                prop.Flags,
                FbxValueSerializer.Serialize(prop.Value)
                )
            );
        }

        private void WriteFbxValueNode(FbxValueNode fbxValueNode)
        {
            _writer.AppendLine(string.Format("{0}{1}: {2}", Indent(), fbxValueNode.Name, FbxValueSerializer.Serialize(fbxValueNode.Value)));
        }

        private string Indent()
        {
            return new string('\t', _indent);
        }
    }

}
