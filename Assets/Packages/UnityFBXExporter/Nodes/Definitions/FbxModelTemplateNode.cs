namespace UnityFBXExporter
{
    public class FbxModelTemplateNode : FbxObjectTypeBaseNode
    {
        public FbxModelTemplateNode() : base("Model", 1)
        {
            ChildNodes.Add(new FbxNodeTemplateNode());
        }
    }
}
