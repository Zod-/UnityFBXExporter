namespace UnityFBXExporter
{
    public class FbxModelTemplateNode : FbxObjectTypeBaseNode
    {
        public FbxModelTemplateNode() : base("Model", 1)
        {
            Nodes.Add(new FbxNodeTemplateNode());
        }
    }
}
