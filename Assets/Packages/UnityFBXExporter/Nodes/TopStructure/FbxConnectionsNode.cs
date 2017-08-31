using System.Collections.Generic;

namespace UnityFBXExporter
{
    public class FbxConnectionsNode : FbxNode
    {
        public List<FbxConnectionProperty> Connections = new List<FbxConnectionProperty>();

        public FbxConnectionsNode()
        {
            Name = "Connections";
        }

        public void Add(long parentId, long childId)
        {
            Connections.Add(new FbxConnectionProperty(parentId, childId));
        }
    }
}
