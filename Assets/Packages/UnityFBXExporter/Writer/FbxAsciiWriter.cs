﻿using UnityEngine;

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
            WriteFbxNode(new FbxHeaderExtensionNode(path));
            WriteFbxNode(new FbxGlobalSettingsNode());
            WriteFbxNode(new FbxDefinitionsNode());

            var connections = new FbxConnectionsNode();
            WriteFbxNode(new FbxObjectsNode(gameObjects, connections));
            WriteFbxNode(connections);
        }

        private void WriteFileHeader()
        {
            _writer.AppendLine(FbxHeader);
        }

        private void WriteFbxNode(FbxNode node)
        {
            var valueNode = node as FbxValueNode;
            if (valueNode != null)
            {
                WriteFbxValueNode(valueNode);
            }
            else
            {
                WriteFbxGenericNode(node);
            }
        }

        private void WriteFbxGenericNode(FbxNode node)
        {
            _writer.AppendLine(string.Format("{0}{1}: {2} {{", Indent(), node.Name, node.GetMetaName()));
            _indent++;
            node.Nodes.ForEach(WriteFbxNode);
            WriteFbxProperties(node);
            WriteFbxConnectionsNode(node as FbxConnectionsNode);
            _indent--;
            _writer.AppendLine(string.Format("{0}}}", Indent()));
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
                string.Format(@"{0}P: ""{1}"", ""{2}"", ""{3}"", ""{4}"",{5}",
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

    public interface IWriter
    {
        void AppendLine(string value);
    }
}
