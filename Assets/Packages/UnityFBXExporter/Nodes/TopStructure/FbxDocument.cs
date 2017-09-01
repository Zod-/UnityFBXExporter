using UnityEngine;

namespace UnityFBXExporter
{
    public class FbxDocument : FbxNode
    {
        public FbxDocument(GameObject[] gameObjects, string path) : base("Document")
        {
            ChildNodes.Add(new FbxHeaderExtensionNode(path));
            ChildNodes.Add(new FbxGlobalSettingsNode());
            ChildNodes.Add(new FbxDefinitionsNode());

            var connections = new FbxConnectionsNode();
            ChildNodes.Add(new FbxObjectsNode(gameObjects, connections));
            ChildNodes.Add(connections);
        }
    }
}
