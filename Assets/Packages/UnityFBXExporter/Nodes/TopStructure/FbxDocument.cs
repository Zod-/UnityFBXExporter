using System;
using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxDocument : FbxNode
    {
        public FbxDocument(GameObject[] gameObjects, string path, DateTime dateTime) : base("Document")
        {
            ChildNodes.Add(new FbxHeaderExtensionNode(path, dateTime));
            ChildNodes.Add(new FbxGlobalSettingsNode());
            ChildNodes.Add(new FbxDefinitionsNode());

            var connections = new FbxConnectionsNode();
            ChildNodes.Add(new FbxObjectsNode(gameObjects, connections));
            ChildNodes.Add(connections);
        }

        public FbxDocument(GameObject[] gameObjects, string path) : this(gameObjects, path, DateTime.Now)
        {
        }
    }
}
