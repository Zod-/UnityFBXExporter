namespace UnityFBXExporter
{
    public class FbxGeometryTemplateNode : FbxObjectTypeBaseNode
    {
        public FbxGeometryTemplateNode() : base("Geometry", 1)
        {
            ChildNodes.Add(new FbxMeshTemplateNode());
        }
    }
}
