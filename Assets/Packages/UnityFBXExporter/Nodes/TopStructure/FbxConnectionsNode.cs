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

        public FbxConnectionsNode() : base("Connections")
        {
        }

        public void Add(long parentId, long childId)
        {
            ChildNodes.Add(new FbxConnectionProperty(parentId, childId));
        }
    }
}
