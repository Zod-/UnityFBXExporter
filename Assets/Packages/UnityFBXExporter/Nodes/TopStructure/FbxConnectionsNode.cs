using System.Collections.Generic;

namespace UnityFBXExporter
{
    public class FbxConnectionsNode : FbxNode
    {
        public override string Header
        {
            get
            {
                return
@"; Object connections
;------------------------------------------------------------------
";
            }
        }

        public readonly List<FbxConnectionProperty> Connections = new List<FbxConnectionProperty>();

        public FbxConnectionsNode() : base("Connections")
        {
        }

        public void Add(long parentId, long childId)
        {
            Connections.Add(new FbxConnectionProperty(parentId, childId));
        }
    }
}
