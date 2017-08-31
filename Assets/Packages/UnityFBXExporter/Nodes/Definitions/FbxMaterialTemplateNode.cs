namespace UnityFBXExporter
{
    public class FbxMaterialTemplateNode : FbxObjectTypeBaseNode
    {
        public FbxMaterialTemplateNode() : base("Material", 1)
        {
            Nodes.Add(new FbxSurfacePhongTemplateNode());
        }
    }
}
