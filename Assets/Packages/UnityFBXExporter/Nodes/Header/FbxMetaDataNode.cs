namespace UnityFBXExporter
{
    public class FbxMetaDataNode : FbxNode
    {
        public FbxMetaDataNode() : base("MetaData")
        {
            Node("Version", 100);
            Node("Title", "");
            Node("Subject", "");
            Node("Author", "");
            Node("Keywords", "");
            Node("Revision", "");
            Node("Comment", "");
        }
    }
}
