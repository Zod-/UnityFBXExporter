namespace UnityFBXExporter
{
    public class FbxGeometryTemplateNode : FbxObjectTypeBaseNode
    {
        public FbxGeometryTemplateNode() : base("Geometry", 1)
        {
            Nodes.Add(new FbxMeshTemplateNode());
        }
    }
}
